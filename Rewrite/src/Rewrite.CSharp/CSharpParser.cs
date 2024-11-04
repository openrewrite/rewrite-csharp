using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Parser;
using Rewrite.RewriteCSharp.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteCSharp;

public class CSharpParser : Core.Parser
{
    private readonly IEnumerable<MetadataReference> _references;

    private CSharpParser(IEnumerable<MetadataReference> references)
    {
        _references = references;
    }

    public class Builder : Core.Parser.Builder
    {
        // TODO check if this is needed and how to best supply these defaults
        private IEnumerable<MetadataReference> _references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
        ];

        public Builder References(IEnumerable<MetadataReference> references)
        {
            _references = references;
            return this;
        }

        public override Core.Parser.Builder Clone()
        {
            return new Builder();
        }

        public override CSharpParser Build()
        {
            return new CSharpParser(_references);
        }
    }

    public SourceFile Parse([LanguageInjection("C#")]string source)
    {
        var input = new Core.Parser.Input(SourcePathFromSourceText("", source), () => new MemoryStream(Encoding.UTF8.GetBytes(source)));
        return ParseInputs([input], null, new InMemoryExecutionContext()).Single();
    }

    public string SourcePathFromSourceText(string prefix, string sourceCode)
    {
        return Path.Combine(prefix, "Test.cs");
    }

    public IEnumerable<SourceFile> ParseInputs(IEnumerable<Core.Parser.Input> sources, string? relativeTo,
        ExecutionContext ctx)
    {
        var references = _references.ToArray();
        var encoding = Encoding.GetEncoding((this as Core.Parser).GetCharset(ctx));

        var sourceFiles = new List<SourceFile>();
        Parallel.ForEach(sources,source =>
        {
            SourceFile cs;
            try
            {
                using var stream = source.GetSource(ctx);
                var sourceText = SourceText.From(stream, encoding);
                var syntaxTree = CSharpSyntaxTree.ParseText(sourceText, path: source.GetRelativePath(relativeTo));
                var root = syntaxTree.GetRoot();
                var compilation = CSharpCompilation.Create("Dummy")
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(references)
                    .AddSyntaxTrees(syntaxTree);
                var semanticModel = compilation.GetSemanticModel(syntaxTree);

                var visitor = new CSharpParserVisitor(this, semanticModel);
                cs = visitor.Visit(root) as Cs.CompilationUnit ?? throw new InvalidOperationException("Visitor.Visit returned null instead of Compilation Unit");
            }
            catch (Exception t)
            {
                // ctx.OnError.accept(t);
                cs = ParseError.Build(this, source, relativeTo, ctx, t);
            }

            sourceFiles.Add(cs!);
        });
        return sourceFiles;
    }
}
