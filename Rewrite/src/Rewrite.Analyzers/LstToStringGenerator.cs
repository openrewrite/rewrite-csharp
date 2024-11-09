using System.Text;
using Microsoft.CodeAnalysis;
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
         string? GetSrc(List<LstInfo> classes)
         {

             if (classes.Count == 0)
                 return null;

             var parent = classes[0].OwningInterfaceName;
             var ns = parent switch
             {
                 "J" => "RewriteJava",
                 "Cs" => "RewriteCSharp",
                 _ => null
             };
             if (ns == null)
                 return null;
             return $$"""
               using Rewrite.Core;

               using Rewrite.RewriteJava.Tree;
               namespace Rewrite.{{ns}}.Tree
               {
                   public partial interface {{parent}}
                   {
                       {{ classes.Render(c => $$"""
                        partial class {{c.ClassName}}{{c.Class.TypeParameterList?.Parameters.Render(p => p.Identifier.Text, ",", "<", ">", renderEmpty: false)}}
                        {
                          public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
                        }
                        """, "\n").Ident(2) }}
                   }
               }
               """;
         }

         void Render(string owningInterface)
         {
             var src = GetSrc(scanner.LstClasses.Values.Where(x => x.OwningInterfaceName == owningInterface).ToList());
             if (src == null) return;
             context.AddSource($"{owningInterface}LstToString.gs", SourceText.From(src, Encoding.UTF8));
         }
         Render("Cs");
         Render("J");
         // if(scanner.CsClasses.Count > 0)
         //    context.AddSource("CsLstToString.gs", SourceText.From(GetSrc(scanner.CsClasses.Values.ToList(), "Cs"), Encoding.UTF8));
         // if(scanner.JClasses.Count > 0)
         //    context.AddSource("JLstToString.gs", SourceText.From(GetSrc(scanner.JClasses.Values.ToList(), "J"), Encoding.UTF8));

    }

}
