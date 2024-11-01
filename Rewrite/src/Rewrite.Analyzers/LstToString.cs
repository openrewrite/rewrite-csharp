using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rewrite.Analyzers;

[Generator]
public class LstToGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxScanner());
    }

    public void Execute(GeneratorExecutionContext context)
    {
         var scanner = (SyntaxScanner)context.SyntaxReceiver!;
         string GetSrc(List<ClassDeclarationSyntax> classes, string parent)
         {
             var ns = parent == "J" ? "RewriteJava" : "RewriteCSharp";
             return $$"""
               using Rewrite.Core;

               using Rewrite.RewriteJava.Tree;
               namespace Rewrite.{{ns}}.Tree
               {
                   public partial interface {{parent}}
                   {
                       {{ classes.Render(c => $$"""
                                                partial class {{c.Identifier}}{{c.TypeParameterList?.Parameters.Render(p => p.Identifier.Text, ",", "<", ">", renderEmpty: false)}}
                                                {
                                                  public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
                                                }
                                                """, "\n").Ident(2) }}
                   }
               }
               """;
         }
         if(scanner.CsClasses.Count > 0)
            context.AddSource("CsLstToString.gs", SourceText.From(GetSrc(scanner.CsClasses.Values.ToList(), "Cs"), Encoding.UTF8));
         if(scanner.JClasses.Count > 0)
            context.AddSource("JLstToString.gs", SourceText.From(GetSrc(scanner.JClasses.Values.ToList(), "J"), Encoding.UTF8));

    }
    public class SyntaxScanner : ISyntaxReceiver
    {
        public Dictionary<string, ClassDeclarationSyntax> CsClasses { get; } = new();
        public Dictionary<string, ClassDeclarationSyntax> JClasses { get; } = new();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax
                {
                    Parent: InterfaceDeclarationSyntax
                    {
                        Identifier.Text: "Cs" or "J" ,
                    } parent
                } classDeclaration)
            {
                if (parent.Identifier.Text == "J")
                    JClasses[classDeclaration.Identifier.Text] = classDeclaration;
                else
                    CsClasses[classDeclaration.Identifier.Text] = classDeclaration;
            }
        }
    }
}
