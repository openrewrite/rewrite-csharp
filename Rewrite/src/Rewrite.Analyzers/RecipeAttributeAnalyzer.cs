using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RecipeAttributeAnalyzer : DiagnosticAnalyzer
    {
        public const string DisplayNameId = "RECIPE001";
        public const string DescriptionId = "RECIPE002";
        public const string PropertyDisplayNameId = "RECIPE003";
        public const string PropertyDescriptionId = "RECIPE004";

        private static readonly DiagnosticDescriptor MissingDisplayName = new DiagnosticDescriptor(
            DisplayNameId,
            "Missing DisplayName attribute",
            "Class '{0}' must have a DisplayNameAttribute",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingDescription = new DiagnosticDescriptor(
            DescriptionId,
            "Missing Description attribute",
            "Class '{0}' must have a DescriptionAttribute",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingPropertyDisplayName = new DiagnosticDescriptor(
            PropertyDisplayNameId,
            "Missing DisplayName attribute on property",
            "Property '{0}' must have a DisplayNameAttribute",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingPropertyDescription = new DiagnosticDescriptor(
            PropertyDescriptionId,
            "Missing Description attribute on property",
            "Property '{0}' must have a DescriptionAttribute",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(MissingDisplayName, MissingDescription, MissingPropertyDisplayName, MissingPropertyDescription);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
        }

        private void AnalyzeClass(SymbolAnalysisContext context)
        {
            var namedType = (INamedTypeSymbol)context.Symbol;

            // Skip if not a non-abstract class.
            if (namedType.TypeKind != TypeKind.Class || namedType.IsAbstract)
                return;

            // Check if the type derives from Rewrite.Core.Recipe.
            var baseType = namedType.BaseType;
            while (baseType != null)
            {
                if (baseType.ToString() == "Rewrite.Core.Recipe")
                    break;
                baseType = baseType.BaseType;
            }
            if (baseType == null)
                return;

            bool hasDisplayName = false;
            bool hasDescription = false;

            foreach (var attr in namedType.GetAttributes())
            {
                var attrName = attr.AttributeClass?.ToString();
                if (attrName == typeof(DisplayNameAttribute).FullName)
                    hasDisplayName = true;
                else if (attrName == typeof(DescriptionAttribute).FullName)
                    hasDescription = true;
            }

            if (!hasDisplayName)
            {
                var diagnostic = Diagnostic.Create(MissingDisplayName, namedType.Locations[0], namedType.Name);
                context.ReportDiagnostic(diagnostic);
            }

            if (!hasDescription)
            {
                var diagnostic = Diagnostic.Create(MissingDescription, namedType.Locations[0], namedType.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeProperty(SymbolAnalysisContext context)
        {
            var propertySymbol = (IPropertySymbol)context.Symbol;

            // Only analyze public properties.
            if (propertySymbol.DeclaredAccessibility != Accessibility.Public)
                return;

            // Only analyze properties with a public setter (covers both set and init).
            var setMethod = propertySymbol.SetMethod;
            if (setMethod == null || setMethod.DeclaredAccessibility != Accessibility.Public)
                return;

            // Ensure the property belongs to a class derived from Rewrite.Core.Recipe.
            var containingType = propertySymbol.ContainingType;
            var baseType = containingType;
            bool isRecipe = false;
            while (baseType != null)
            {
                if (baseType.ToString() == "Rewrite.Core.Recipe")
                {
                    isRecipe = true;
                    break;
                }
                baseType = baseType.BaseType;
            }
            if (!isRecipe)
                return;

            bool hasDisplayName = false;
            bool hasDescription = false;

            foreach (var attr in propertySymbol.GetAttributes())
            {
                var attrName = attr.AttributeClass?.ToString();
                if (attrName == typeof(DisplayNameAttribute).FullName)
                    hasDisplayName = true;
                else if (attrName == typeof(DescriptionAttribute).FullName)
                    hasDescription = true;
            }

            if (!hasDisplayName)
            {
                var diagnostic = Diagnostic.Create(MissingPropertyDisplayName, propertySymbol.Locations[0], propertySymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

            if (!hasDescription)
            {
                var diagnostic = Diagnostic.Create(MissingPropertyDescription, propertySymbol.Locations[0], propertySymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
