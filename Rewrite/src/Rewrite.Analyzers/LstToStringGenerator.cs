using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rewrite.Analyzers;

[Generator]
public class LstToStringGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new LstLocator());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var scanner = (LstLocator)context.SyntaxReceiver!;
        var classes = scanner.LstClasses;


        foreach (var kv in classes)
        {
            var fullyQualifiedName = kv.Key;
            var typeDeclarationSyntax = kv.Value;
            var src = typeDeclarationSyntax.RenderPartial(() => """
              public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
              """);
            src = SyntaxFactory.ParseCompilationUnit(src).NormalizeWhitespace().ToFullString();

            context.AddSource($"{fullyQualifiedName}.LstToString.g.cs", SourceText.From(src, Encoding.UTF8));

        }

    }

}
