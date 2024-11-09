using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rewrite.Analyzers;

[Generator]
public class LstInterfaceImplementationsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new LstLocator());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var scanner = (LstLocator)context.SyntaxReceiver!;

    }
}
