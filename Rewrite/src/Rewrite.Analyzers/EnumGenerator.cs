using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Analyzers.Authoring;

namespace Rewrite.Analyzers;

[Generator]
public class EnumGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxScanner());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var scanner = (SyntaxScanner)context.SyntaxReceiver!;

        scanner.CsRightPaddedEnumValues.UnionWith(scanner.CsContainerEnumValues);
        scanner.CsSpaceEnumValues.UnionWith(scanner.CsContainerEnumValues);
        scanner.CsSpaceEnumValues.UnionWith(scanner.CsRightPaddedEnumValues);
        scanner.CsSpaceEnumValues.UnionWith(scanner.CsLeftPaddedEnumValues);
        // scanner.CsLeftPaddedEnumValues.UnionWith(scanner.CsContainerEnumValues);
        var csContainerSrc = $$"""
              namespace Rewrite.RewriteJava.Tree;

              public partial class JContainer
              {
                  public partial record Location
                  {
                      {{ scanner.CsContainerEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new(Space.Location.{{enumValue}}, JRightPadded.Location.{{enumValue}});
                      """, "\n").Ident(2) }}
                  }
              }
              """;
        var csSpaceSrc = $$"""
               namespace Rewrite.RewriteCSharp.Tree;

               public partial class CsSpace
               {
                   public enum Location
                   {
                       {{ scanner.CsSpaceEnumValues.Render(enumValue => $$"""
                      {{enumValue}}
                      """, ",\n").Ident(2) }}
                   }
               }
               """;

        var allSpaceValues = scanner.CsSpaceEnumValues.Union(scanner.JSpaceEnumValues).OrderBy(x => x).ToList();
        var spaceSrc = $$"""
               namespace Rewrite.RewriteJava.Tree;

               public partial class Space
               {
                   public enum Location
                   {
                       {{ allSpaceValues.Render(enumValue => $$"""
                      {{enumValue}}
                      """, ",\n").Ident(2) }}
                   }
               }
               """;

        var csRightPaddedSrc = $$"""
               namespace Rewrite.RewriteJava.Tree;

               public partial class JRightPadded
               {
                   public partial record Location
                   {
                       {{ scanner.CsRightPaddedEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new (Space.Location.{{enumValue}});
                      """, "\n").Ident(2) }}
                   }
               }
               """;

        var csLeftPaddedSrc = $$"""
               namespace Rewrite.RewriteJava.Tree;

               public partial class JLeftPadded
               {
                   public partial record Location
                   {
                       {{ scanner.CsLeftPaddedEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new (Space.Location.{{enumValue}});
                      """, "\n").Ident(2) }}
                   }
               }
               """;
        if(scanner.CsContainerEnumValues.Count > 0)
            context.AddSource("CsContainer.gs", SourceText.From(csContainerSrc, Encoding.UTF8));
        if(scanner.CsRightPaddedEnumValues.Count > 0)
            context.AddSource("CsRightPadded.gs", SourceText.From(csRightPaddedSrc, Encoding.UTF8));
        if(scanner.CsLeftPaddedEnumValues.Count > 0)
            context.AddSource("CsLeftPadded.gs", SourceText.From(csLeftPaddedSrc, Encoding.UTF8));
        if(scanner.CsSpaceEnumValues.Count > 0)
            context.AddSource("CsSpace.gs", SourceText.From(csSpaceSrc, Encoding.UTF8));
        if(allSpaceValues.Count > 0)
            context.AddSource("Space.gs", SourceText.From(spaceSrc, Encoding.UTF8));
    }
    public class SyntaxScanner : ISyntaxReceiver
    {
        public HashSet<string> CsContainerEnumValues { get; } = new();
        public HashSet<string> CsRightPaddedEnumValues { get; } = new();
        public HashSet<string> CsLeftPaddedEnumValues { get; } = new();
        public HashSet<string> CsSpaceEnumValues { get; } = new();
        public HashSet<string> JSpaceEnumValues { get; } = new();
        public string? AssemblyFileVersion { get; set; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MemberAccessExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: IdentifierNameSyntax { Identifier.Text: "JContainer" },
                        Name: IdentifierNameSyntax { Identifier.Text: "Location" },
                    }
                } csContainerLocation)
            {

                CsContainerEnumValues.Add(csContainerLocation.Name.Identifier.Text);
            }

            if (syntaxNode is MemberAccessExpressionSyntax
            {
                Expression: MemberAccessExpressionSyntax
                {
                    Expression: IdentifierNameSyntax { Identifier.Text: "JRightPadded" },
                    Name: IdentifierNameSyntax { Identifier.Text: "Location" },
                }
            } csRightPaddedLocation)
            {

                CsRightPaddedEnumValues.Add(csRightPaddedLocation.Name.Identifier.Text);
            }
            if (syntaxNode is MemberAccessExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: IdentifierNameSyntax { Identifier.Text: "JLeftPadded" },
                        Name: IdentifierNameSyntax { Identifier.Text: "Location" },
                    }
                } csLeftPaddedLocation)
            {

                CsLeftPaddedEnumValues.Add(csLeftPaddedLocation.Name.Identifier.Text);
            }
            if (syntaxNode is MemberAccessExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: IdentifierNameSyntax { Identifier.Text: "Space" },
                        Name: IdentifierNameSyntax { Identifier.Text: "Location" },
                    }
                } csSpaceLocation)
            {

                CsSpaceEnumValues.Add(csSpaceLocation.Name.Identifier.Text);
            }

            if (syntaxNode is EnumDeclarationSyntax { Identifier: { Text: "JSpaceLocation" } } jSpaceLocation)
            {
                foreach (var val in jSpaceLocation.Members.Select(x => x.Identifier.Text))
                {
                    JSpaceEnumValues.Add(val);

                }
            }
        }
    }
}
