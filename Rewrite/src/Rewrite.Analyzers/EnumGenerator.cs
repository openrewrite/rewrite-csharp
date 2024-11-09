using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
              namespace Rewrite.RewriteCSharp.Tree;

              public partial interface CsContainer
              {
                  public record Location(CsSpace.Location BeforeLocation, CsRightPadded.Location ElementLocation)
                  {
                      {{ scanner.CsContainerEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new(CsSpace.Location.{{enumValue}}, CsRightPadded.Location.{{enumValue}});
                      """, "\n").Ident(2) }}
                  }
              }
              """;
        var csSpaceSrc = $$"""
               namespace Rewrite.RewriteCSharp.Tree;

               public partial interface CsSpace
               {
                   public record Location
                   {
                       {{ scanner.CsSpaceEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new();
                      """, "\n").Ident(2) }}
                   }
               }
               """;

        var csRightPaddedSrc = $$"""
               namespace Rewrite.RewriteCSharp.Tree;

               public partial interface CsRightPadded
               {
                   public record Location(CsSpace.Location AfterLocation)
                   {
                       {{ scanner.CsRightPaddedEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new (CsSpace.Location.{{enumValue}});
                      """, "\n").Ident(2) }}
                   }
               }
               """;

        var csLeftPaddedSrc = $$"""
               namespace Rewrite.RewriteCSharp.Tree;

               public partial interface CsLeftPadded
               {
                   public record Location(CsSpace.Location BeforeLocation)
                   {
                       {{ scanner.CsLeftPaddedEnumValues.Render(enumValue => $$"""
                      public static readonly Location {{enumValue}} = new (CsSpace.Location.{{enumValue}});
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
    }
    public class SyntaxScanner : ISyntaxReceiver
    {
        public HashSet<string> CsContainerEnumValues { get; } = new();
        public HashSet<string> CsRightPaddedEnumValues { get; } = new();
        public HashSet<string> CsLeftPaddedEnumValues { get; } = new();
        public HashSet<string> CsSpaceEnumValues { get; } = new();
        public string? AssemblyFileVersion { get; set; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MemberAccessExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: IdentifierNameSyntax { Identifier.Text: "CsContainer" },
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
                    Expression: IdentifierNameSyntax { Identifier.Text: "CsRightPadded" },
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
                        Expression: IdentifierNameSyntax { Identifier.Text: "CsLeftPadded" },
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
                        Expression: IdentifierNameSyntax { Identifier.Text: "CsSpace" },
                        Name: IdentifierNameSyntax { Identifier.Text: "Location" },
                    }
                } csSpaceLocation)
            {

                CsSpaceEnumValues.Add(csSpaceLocation.Name.Identifier.Text);
            }

        }
    }
}
