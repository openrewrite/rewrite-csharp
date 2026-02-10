using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for types implementing <c>IReflect</c> or deriving from
    /// <c>Type</c>/<c>TypeInfo</c> that use <c>DynamicallyAccessedMemberTypes.All</c>,
    /// which should be replaced with more restricted annotations in .NET 10.
    /// </summary>
    /// <remarks>
    /// In .NET 10, <c>IReflect.InvokeMember</c>, <c>Type.FindMembers</c>, and
    /// <c>TypeInfo.DeclaredMembers</c> use more restricted <c>DynamicallyAccessedMemberTypes</c>
    /// annotations instead of <c>All</c>. Types implementing these APIs should update their
    /// annotations accordingly.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/reflection/10/ireflect-damt-annotations">
    /// More restricted annotations on InvokeMember/FindMembers/DeclaredMembers documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ReflectDamtAnnotationsAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for the DynamicallyAccessedMemberTypes.All annotation change detection.
        /// </summary>
        public const string DiagnosticId = "ORNETX0022";

        private static readonly LocalizableString Title =
            "DynamicallyAccessedMemberTypes.All should use more restricted annotations in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "Type '{0}' uses DynamicallyAccessedMemberTypes.All and implements IReflect or derives from " +
            "Type/TypeInfo. In .NET 10, these APIs use more restricted annotations. " +
            "Update to use specific member types instead of All.";

        private static readonly LocalizableString Description =
            "In .NET 10, IReflect.InvokeMember, Type.FindMembers, and TypeInfo.DeclaredMembers use more " +
            "restricted DynamicallyAccessedMemberTypes annotations instead of All. Types implementing " +
            "IReflect or deriving from Type/TypeInfo should update their DynamicallyAccessedMembers " +
            "attributes to use the specific member types: PublicFields, NonPublicFields, PublicMethods, " +
            "NonPublicMethods, PublicProperties, NonPublicProperties, PublicConstructors, and " +
            "NonPublicConstructors.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri:
                "https://learn.microsoft.com/en-us/dotnet/core/compatibility/reflection/10/ireflect-damt-annotations");

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for attribute syntax nodes to detect
        /// <c>DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)</c> on types that
        /// implement <c>IReflect</c> or derive from <c>Type</c>/<c>TypeInfo</c>.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
        }

        private static void AnalyzeAttribute(SyntaxNodeAnalysisContext context)
        {
            var attribute = (AttributeSyntax)context.Node;

            // Quick syntax check: attribute name should contain "DynamicallyAccessedMembers"
            var attrName = attribute.Name.ToString();
            if (!attrName.Contains("DynamicallyAccessedMembers"))
                return;

            // Check the attribute has arguments and at least one contains .All
            if (attribute.ArgumentList is null || attribute.ArgumentList.Arguments.Count == 0)
                return;

            var argExpression = attribute.ArgumentList.Arguments[0].Expression;
            if (!ContainsMemberAccessForAll(argExpression))
                return;

            // Ensure the attribute is directly on a type declaration
            if (attribute.Parent is not AttributeListSyntax attributeList)
                return;
            if (attributeList.Parent is not TypeDeclarationSyntax typeDecl)
                return;

            // Semantic check: verify this is actually DynamicallyAccessedMembersAttribute
            var attrSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
            if (attrSymbol?.ContainingType?.ToDisplayString() !=
                "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute")
                return;

            // Semantic check: does the type implement IReflect or derive from Type/TypeInfo?
            var typeSymbol = context.SemanticModel.GetDeclaredSymbol(typeDecl);
            if (typeSymbol is null)
                return;

            if (!ImplementsIReflect(typeSymbol) && !DerivesFromTypeOrTypeInfo(typeSymbol))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, attribute.GetLocation(), typeSymbol.Name));
        }

        private static bool ContainsMemberAccessForAll(ExpressionSyntax expression)
        {
            if (expression is MemberAccessExpressionSyntax memberAccess)
                return memberAccess.Name.Identifier.Text == "All";

            if (expression is BinaryExpressionSyntax binary)
                return ContainsMemberAccessForAll(binary.Left) || ContainsMemberAccessForAll(binary.Right);

            return false;
        }

        private static bool ImplementsIReflect(INamedTypeSymbol typeSymbol)
        {
            foreach (var iface in typeSymbol.AllInterfaces)
            {
                if (iface.ToDisplayString() == "System.Reflection.IReflect")
                    return true;
            }

            return false;
        }

        private static bool DerivesFromTypeOrTypeInfo(INamedTypeSymbol typeSymbol)
        {
            var baseType = typeSymbol.BaseType;
            while (baseType is not null)
            {
                var name = baseType.ToDisplayString();
                if (name is "System.Type" or "System.Reflection.TypeInfo")
                    return true;
                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
