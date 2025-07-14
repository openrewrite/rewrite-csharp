using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rewrite.Core;
using Rewrite.Core.Quark;
using Rewrite.CSharp.Tests;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteJava.Tree;
using Spectre.Console;

namespace Rewrite.Test;

public class RewriteTest
{
    protected readonly ITestOutputHelper _output = new();

    public void RewriteRun(params SourceSpec[] sourceSpecs)
    {
        RewriteRun(spec => { }, sourceSpecs);
    }

    public void RewriteRun(Action<RecipeSpec> spec, params SourceSpecs[] sourceSpecs)
    {
        var flatSourceSpecs = sourceSpecs.SelectMany(specs => specs).ToArray();
        RewriteRun(spec, flatSourceSpecs);
    }

    public void RewriteRun(Action<RecipeSpec> spec, params SourceSpec[] sourceSpecs)
    {
        var testClassSpec = RecipeSpec.Defaults();
        Defaults(testClassSpec);

        var testMethodSpec = RecipeSpec.Defaults();
        spec(testMethodSpec);

        var markerPrinter = testMethodSpec.MarkerPrinter ?? testClassSpec.MarkerPrinter ?? PrintOutputCapture<int>.IMarkerPrinter.Default;

        var recipe = testMethodSpec.Recipe ?? testClassSpec.Recipe!;
        recipe.Should().NotBeNull("A recipe must be specified");

        var ctx = testMethodSpec.ExecutionContext ?? testClassSpec.ExecutionContext ?? DefaultExecutionContext(sourceSpecs);

        foreach (var sourceSpec in sourceSpecs)
        {
            sourceSpec.CustomizeExecutionContext?.Invoke(ctx);
        }

        
        
        var methodSpecParsers = testMethodSpec.Parsers;
        // Clone class-level parsers to ensure that no state leaks between tests
        
        var testClassSpecParsers = new List<IParser.Builder>(testClassSpec.Parsers.Select(item => item.Clone()));
        var allKnownParsers = methodSpecParsers
            .Concat(testClassSpecParsers)
            .Where(x => x.SourceFileType != null)
            .GroupBy(x => x.SourceFileType!, x => x.Build())
            .ToDictionary(x => x.Key, x => x.First());
        
        var sourceSpecsByParser = allKnownParsers
            .Join(sourceSpecs, x => x.Key, x => x.SourceFileType, (kv, spec) => (Parser: kv.Value, Spec: spec))
            .ToLookup(x => x.Parser, x => x.Spec);
        
        var specBySourceFile = new Dictionary<SourceFile, SourceSpec>(sourceSpecs.Length);

        var capture = new PrintOutputCapture<int>(0, markerPrinter);

        foreach (var sourceSpecsForParser in sourceSpecsByParser)
        {
            var inputs = new Dictionary<SourceSpec, IParser.Input>(sourceSpecsForParser.Count());
            var parser = sourceSpecsForParser.Key;

            foreach (var sourceSpec in sourceSpecsForParser.Where(x => x.Before != null))
            {
                var beforeTrimmed = sourceSpec.NoTrim ? sourceSpec.Before! : TrimIndentPreserveCrLf(sourceSpec.Before) ?? "";

                var sourcePath = sourceSpec.SourcePath != null
                    ? Path.Combine(sourceSpec.Dir, sourceSpec.SourcePath)
                    : parser.SourcePathFromSourceText(sourceSpec.Dir, beforeTrimmed);

                foreach (var consumer in testMethodSpec.AllSources.Union(testClassSpec.AllSources))
                {
                    consumer(sourceSpec);
                }

                inputs[sourceSpec] = new IParser.Input(sourcePath, () => new MemoryStream(Encoding.UTF8.GetBytes(beforeTrimmed)));
            }

            var relativeTo = testMethodSpec.RelativeTo ?? testClassSpec.RelativeTo;
            var sourceSpecIter = inputs.Keys.GetEnumerator();

            var sourceFiles = parser.ParseInputs(inputs.Values, relativeTo, ctx).ToList();
            sourceFiles.Should().HaveSameCount(inputs.Values);

            // iterate over parsed files
            for (var i = 0; i < sourceFiles.Count; i++)
            {
                var sourceFile = sourceFiles[i];
                EnsureNoParseErrorsOrUnknowns(sourceFile);
                RenderLstTree(sourceFile);

                var markers = sourceFile.Markers;

                sourceSpecIter.MoveNext().Should().BeTrue();
                var nextSpec = sourceSpecIter.Current;

                markers = nextSpec.Markers.MarkerList.Aggregate(markers, (current, marker) => current.SetByType(marker));
                sourceFile = sourceFile?.WithMarkers(markers);

                if (sourceFile != null)
                    nextSpec.ValidateSource?.Invoke(sourceFile, TypeValidation.Before(testMethodSpec, testClassSpec));

                int j = 0;
                foreach (var input in inputs.Values)
                {
                    if (j++ == i && sourceFile is not Quark)
                    {
                        var before = StringUtils.ReadFully(input.GetSource(ctx), Encoding.GetEncoding(parser.GetCharset(ctx)));
                        var after = sourceFile!.PrintAll(capture.Clone());
                        after.ShouldBeSameAs(before);
                    }
                }

                var mapped = nextSpec.BeforeRecipe(sourceFile!);
                specBySourceFile[mapped] = nextSpec;
            }
        }

        var beforeList = specBySourceFile.Keys.ToList();
        
        var afterList = RunRecipe(recipe, beforeList);
        for (var i = 0; i < beforeList.Count; i++)
        {
            var before = beforeList[i];
            var after = afterList[i];
            var sourceSpec = specBySourceFile[before];

            if (after != null && sourceSpec.After != null)
            {
                AssertContentEquals(
                    before,
                    sourceSpec.After,
                    after.PrintAll(capture.Clone()),
                    "Expecting content after recipe ..."
                );
            }
            else if (after != null)
            {
                AssertContentEquals(
                    before,
                    sourceSpec.Before!,
                    after.PrintAll(capture.Clone()),
                    "Content not expected to change ..."
                );
            }

            // TODO figure out how to do this without reflection
            var afterRecipe = sourceSpec.GetType()?.GetProperty("AfterRecipe")?.GetValue(sourceSpec);
            if (afterRecipe != null)
            {
                var methodInfo = afterRecipe.GetType().GetMethod("Invoke");
                methodInfo?.Invoke(afterRecipe, [before]);
            }
        }
    }

    private static void EnsureNoParseErrorsOrUnknowns(SourceFile sourceFile)
    {
        if (sourceFile is ParseError parseError)
        {
            var error = parseError.Markers.FindFirst<ParseExceptionResult>()!;
            throw new Exception(error.Message);
        }

        var unknown = sourceFile.Descendents().OfType<J.Unknown.Source>().SelectMany(x => x.Markers.OfType<ParseExceptionResult>().Select(x => x.TreeType)).ToList();
        if(unknown.Any())
        {
            throw new Exception($"LST should not contain unknown nodes: {unknown.First()}");
        }
    }

    private void RenderLstTree(SourceFile sourceFile)
    {
        var customProperties = TestContext.Parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
        bool shouldRenderLst = !EnvironmentInfo.IsCI;
        if (customProperties.TryGetValue("RenderLST", out var renderLst) && bool.TryParse(renderLst, out var renderLstValue))
        {
            shouldRenderLst = renderLstValue;
        }
        if(shouldRenderLst)
        {
            AnsiConsole.Write(sourceFile.RenderLstTree());
        }
    }

    private IList<SourceFile?> RunRecipe(Recipe recipe, IList<SourceFile> sourceFiles)
    {
        return sourceFiles.Select(x => (SourceFile?)recipe.GetVisitor().Visit(x, new InMemoryExecutionContext())).ToList();
    }

    private static void AssertContentEquals(SourceFile? sourceFile, string readFully, string printAll,
        string whenParsing)
    {
        printAll.Should().Be(readFully, because: whenParsing);
    }

    private static string? TrimIndentPreserveCrLf(string? text)
    {
        if (text == null) return null;

        text = text.EndsWith("\r\n") ? text.Substring(0, text.Length - 2) : text;
        text = text.Replace('\r', '⏎');
        text = text.Replace('⏎', '\r');
        // FIXME implement `StringUtils.TrimIndent()` as required
        return text;
    }

    public virtual IExecutionContext DefaultExecutionContext(params SourceSpec[] sourceSpecs)
    {
        return new InMemoryExecutionContext(e => Assert.Fail("The recipe threw an exception: " + e));
    }

    protected virtual void Defaults(RecipeSpec spec)
    {
        spec.Recipe = Recipe.Noop();
    }
}

internal static class StringUtils
{
    public static string ReadFully(Stream stream, Encoding encoding)
    {
        using var reader = new StreamReader(stream, encoding);
        return reader.ReadToEnd();
    }
}
public class WhitespaceRewriter : CSharpSyntaxRewriter
{
    int _i = 0;
    public override SyntaxToken VisitToken(SyntaxToken token)
    {
        var trailingTrivia = token.TrailingTrivia.Where(x => x.IsKind(SyntaxKind.EndOfLineTrivia));
        return base.VisitToken(token
            .WithLeadingTrivia(SyntaxFactory.Comment($"/*{_i++}*/"))
            .WithTrailingTrivia(trailingTrivia));
    }
}
