using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzer to detect structs with InlineArrayAttribute that also have explicit Size in StructLayoutAttribute.
    /// In .NET 10, specifying explicit Size on an inline array struct is disallowed and will throw TypeLoadException.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InlineArrayExplicitSizeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0004";

        private static readonly LocalizableString Title = "Inline array struct cannot have explicit Size in .NET 10";
        private static readonly LocalizableString MessageFormat = "Struct with InlineArrayAttribute cannot have explicit Size in StructLayoutAttribute. This will throw TypeLoadException in .NET 10.";
        private static readonly LocalizableString Description = "In .NET 10, specifying explicit Size on a struct with InlineArrayAttribute is disallowed. " +
            "The combination is ambiguous and contradicts the specification. Attempting to load such a type will throw TypeLoadException. " +
            "To fix this, introduce a wrapper struct around the element type or the inline array type and specify Size on the wrapper instead.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/inlinearray-explicit-size-disallowed");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for struct declarations
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var namedType = (INamedTypeSymbol)context.Symbol;

            // Only analyze structs
            if (namedType.TypeKind != TypeKind.Struct)
                return;

            // Check if the struct has InlineArrayAttribute
            var hasInlineArray = false;
            AttributeData? inlineArrayAttribute = null;

            foreach (var attr in namedType.GetAttributes())
            {
                if (attr.AttributeClass?.ToString() == "System.Runtime.CompilerServices.InlineArrayAttribute")
                {
                    hasInlineArray = true;
                    inlineArrayAttribute = attr;
                    break;
                }
            }

            if (!hasInlineArray)
                return;

            // Check if the struct has StructLayoutAttribute with explicit Size
            foreach (var attr in namedType.GetAttributes())
            {
                if (attr.AttributeClass?.ToString() != "System.Runtime.InteropServices.StructLayoutAttribute")
                    continue;

                // Check if Size is explicitly set
                // Size can be set via named argument
                var sizeArgument = attr.NamedArguments.FirstOrDefault(na => na.Key == "Size");
                if (!sizeArgument.Equals(default(KeyValuePair<string, TypedConstant>)))
                {
                    // Size is explicitly set - this is the problematic pattern
                    // Report diagnostic on the struct identifier
                    var syntaxReference = namedType.DeclaringSyntaxReferences.FirstOrDefault();
                    if (syntaxReference != null)
                    {
                        var syntaxNode = syntaxReference.GetSyntax(context.CancellationToken);
                        if (syntaxNode is StructDeclarationSyntax structDecl)
                        {
                            var diagnostic = Diagnostic.Create(Rule, structDecl.Identifier.GetLocation(), namedType.Name);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                    return;
                }
            }
        }
    }
}
