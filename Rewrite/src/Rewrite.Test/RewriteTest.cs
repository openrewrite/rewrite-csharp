using System.Reflection;
using System.Text;
using Rewrite.Core;
using Rewrite.Core.Quark;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test;

public class RewriteTest
{
    public void RewriteRun(params SourceSpecs[] sourceSpecs)
    {
        RewriteRun(spec => { }, sourceSpecs);
    }

    public void RewriteRun(Action<RecipeSpec> spec, params SourceSpecs[] sourceSpecs)
    {
        var flatSourceSpecs = sourceSpecs.SelectMany(specs => specs).ToArray();
        RewriteRun(spec, flatSourceSpecs);
    }

    public void RewriteRun(Action<RecipeSpec> spec, SourceSpec[] sourceSpecs)
    {
        var testClassSpec = RecipeSpec.Defaults();
        Defaults(testClassSpec);

        var testMethodSpec = RecipeSpec.Defaults();
        spec(testMethodSpec);

        PrintOutputCapture<int>.IMarkerPrinter markerPrinter;
        if (testMethodSpec.MarkerPrinter != null)
        {
            markerPrinter = testMethodSpec.MarkerPrinter;
        }
        else if (testClassSpec.MarkerPrinter != null)
        {
            markerPrinter = testClassSpec.MarkerPrinter;
        }
        else
        {
            markerPrinter = PrintOutputCapture<int>.IMarkerPrinter.Default;
        }

        var recipe = testMethodSpec.Recipe ?? testClassSpec.Recipe!;
        recipe.Should().NotBeNull("A recipe must be specified");

        var cycles = testMethodSpec.Cycles ?? testClassSpec.Cycles;
        var expectedCyclesThatMakeChanges =
            testMethodSpec.ExpectedCyclesThatMakeChanges ?? testClassSpec.ExpectedCyclesThatMakeChanges;

        // FIXME implement
        // If there are any tests that have assertions (an "after"), then set the expected cycles.
        // for (SourceSpec<?> s : sourceSpecs) {
        //     if (s.after != null) {
        //         expectedCyclesThatMakeChanges = testMethodSpec.expectedCyclesThatMakeChanges == null ?
        //             testClassSpec.getExpectedCyclesThatMakeChanges(cycles) :
        //             testMethodSpec.getExpectedCyclesThatMakeChanges(cycles);
        //         break;
        //     }
        // }

        var ctx = testMethodSpec.ExecutionContext ??
                  testClassSpec.ExecutionContext ?? DefaultExecutionContext(sourceSpecs);
        var testExecutionContext = ITestExecutionContext.Current();
        if (testExecutionContext != null)
        {
            testExecutionContext.Reset(ctx);
            // RemotingExecutionContextView.View(ctx).RemotingContext = /**/testExecutionContext;
        }

        foreach (var sourceSpec in sourceSpecs)
        {
            sourceSpec.CustomizeExecutionContext?.Invoke(ctx);
        }

        IList<Validated<object>> validations = new List<Validated<object>>();
        recipe!.ValidateAll(ctx, validations);
        validations.Should().NotContain(v => v.IsInvalid(), "Recipe validation must have no failures");

        var sourceSpecsByParser = new Dictionary<Parser.Builder, List<SourceSpec>>();
        var methodSpecParsers = testMethodSpec.Parsers;
        // Clone class-level parsers to ensure that no state leaks between tests
        var testClassSpecParsers = new List<Parser.Builder>(testClassSpec.Parsers.Select(item => item.Clone()));
        foreach (var sourceSpec in sourceSpecs)
        {
            // ----- method specific parser -------------------------
            if (RewriteTestUtils.GroupSourceSpecsByParser(methodSpecParsers, sourceSpecsByParser, sourceSpec))
            {
                continue;
            }

            // ----- test default parser -------------------------
            if (RewriteTestUtils.GroupSourceSpecsByParser(testClassSpecParsers, sourceSpecsByParser, sourceSpec))
            {
                continue;
            }

            // ----- default parsers for each SourceFile type -------------------------
            var key = sourceSpec.Parser.Clone();
            if (!sourceSpecsByParser.ContainsKey(key))
                sourceSpecsByParser[key] = [];
            sourceSpecsByParser[key].Add(sourceSpec);
        }

        var specBySourceFile = new Dictionary<SourceFile, SourceSpec>(sourceSpecs.Length);

        var capture = new PrintOutputCapture<int>(0, markerPrinter);

        foreach (var sourceSpecsForParser in sourceSpecsByParser)
        {
            var inputs =
                new Dictionary<SourceSpec, Parser.Input>(sourceSpecsForParser.Value.Count);
            var parser = sourceSpecsForParser.Key.Build();

            foreach (var sourceSpec in sourceSpecsForParser.Value)
            {
                if (sourceSpec.Before == null)
                {
                    continue;
                }

                var beforeTrimmed = sourceSpec.NoTrim ? sourceSpec.Before : TrimIndentPreserveCrLf(sourceSpec.Before);

                var sourcePath = sourceSpec.SourcePath != null
                    ? Path.Combine(sourceSpec.Dir, sourceSpec.SourcePath)
                    : parser.SourcePathFromSourceText(sourceSpec.Dir, beforeTrimmed);

                foreach (var consumer in testMethodSpec.AllSources)
                {
                    consumer(sourceSpec);
                }

                foreach (var consumer in testClassSpec.AllSources)
                {
                    consumer(sourceSpec);
                }

                inputs[sourceSpec] = new Parser.Input(sourcePath,
                    () => new MemoryStream(Encoding.UTF8.GetBytes(beforeTrimmed)));
            }

            var relativeTo = testMethodSpec.RelativeTo ?? testClassSpec.RelativeTo;

            var sourceSpecIter = inputs.Keys.GetEnumerator();

            var requirePrintEqualsInput = ctx.GetMessage(ExecutionContext.RequirePrintEqualsInput, true);
            if (requirePrintEqualsInput)
            {
                ctx.PutMessage(ExecutionContext.RequirePrintEqualsInput, false);
            }

            var sourceFiles = parser.ParseInputs(inputs.Values, relativeTo, ctx).ToList();
            sourceFiles.Should().HaveSameCount(inputs.Values);

            if (requirePrintEqualsInput)
            {
                ctx.PutMessage(ExecutionContext.RequirePrintEqualsInput, requirePrintEqualsInput);
            }

            for (var i = 0; i < sourceFiles.Count; i++)
            {
                var sourceFile = sourceFiles[i];
                var markers = sourceFile.Markers;

                sourceSpecIter.MoveNext().Should().BeTrue();
                var nextSpec = sourceSpecIter.Current;

                markers = nextSpec.Markers.MarkerList.Aggregate(markers,
                    (current, marker) => current.SetByType(marker));
                sourceFile = (sourceFile as MutableTree<SourceFile>)?.WithMarkers(markers);

                if (sourceFile != null)
                    nextSpec.ValidateSource?.Invoke(sourceFile, TypeValidation.Before(testMethodSpec, testClassSpec));

                int j = 0;
                foreach (var input in inputs.Values)
                {
                    if (j++ == i && sourceFile is not Quark)
                    {
                        AssertContentEquals(
                            sourceFile,
                            StringUtils.ReadFully(input.GetSource(ctx), Encoding.GetEncoding(parser.GetCharset(ctx))),
                            sourceFile!.PrintAll(capture.Clone()),
                            "When parsing..."
                        );

                        // FIXME implement
                        // try
                        // {
                        //     WhitespaceValidationService service = sourceFile.Service<WhitespaceValidationService>();
                        //     SourceFile whitespaceValidated = (SourceFile)service.Visitor.Visit(sourceFile, ctx);
                        //
                        //     if (whitespaceValidated != null && whitespaceValidated != sourceFile)
                        //     {
                        //         throw new Exception("Source file was parsed...");
                        //     }
                        // }
                        // catch (NotSupportedException e)
                        // {
                        //     // Language/parser does not provide whitespace validation and that's OK for now
                        // }
                    }
                }

                var mapped = nextSpec.BeforeRecipe(sourceFile);
                specBySourceFile[mapped] = nextSpec;
            }
        }

        var recipeOptions = new Dictionary<string, object?>();
        foreach (var property in recipe.GetType().GetTypeInfo().DeclaredProperties)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(recipe, null);
            recipeOptions[char.ToLower(propertyName[0]) + propertyName[1..]] = propertyValue;
        }

        var beforeList = specBySourceFile.Keys.ToList();
        var afterList = testExecutionContext!.RunRecipe(recipe, recipeOptions, beforeList);
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
            var afterRecipe = sourceSpec.GetType().GetProperty("AfterRecipe").GetValue(sourceSpec);
            if (afterRecipe != null)
            {
                var methodInfo = afterRecipe.GetType().GetMethod("Invoke");
                methodInfo?.Invoke(afterRecipe, [before]);
            }
        }
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

    public virtual ExecutionContext DefaultExecutionContext(SourceSpec[] sourceSpecs)
    {
        return new InMemoryExecutionContext(e => Assert.Fail("The recipe threw an exception: " + e));
    }

    public virtual void Defaults(RecipeSpec spec)
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