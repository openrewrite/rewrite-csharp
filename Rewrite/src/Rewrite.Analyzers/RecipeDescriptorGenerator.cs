using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Rewrite.Core.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Rewrite.Analyzers;

[Generator]
public class RecipeMetadataGenerator : ISourceGenerator
{
    private static readonly SymbolDisplayFormat FullTypeFormat = new SymbolDisplayFormat(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
    );
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new RecipeSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not RecipeSyntaxReceiver receiver)
            return;

        var recipeBaseType = context.Compilation.GetTypeByMetadataName("Rewrite.Core.Recipe");
        var displayNameAttr = context.Compilation.GetTypeByMetadataName(typeof(DisplayNameAttribute).FullName!);
        var descriptionAttr = context.Compilation.GetTypeByMetadataName(typeof(DescriptionAttribute).FullName!);
        var tagsAttr = context.Compilation.GetTypesByMetadataName(typeof(TagsAttribute).FullName!).First(x => x.ContainingAssembly.Name != this.GetType().Assembly.GetName().Name);

        if (recipeBaseType is null || displayNameAttr is null || descriptionAttr is null || tagsAttr is null)
            return;

        var descriptors = new List<RecipeDescriptor>();

        foreach (var classDecl in receiver.CandidateClasses)
        {
            var model = context.Compilation.GetSemanticModel(classDecl.SyntaxTree);
            if (model.GetDeclaredSymbol(classDecl) is not INamedTypeSymbol classSymbol)
                continue;

            if (classSymbol.IsAbstract || classSymbol.TypeKind != TypeKind.Class)
                continue;

            if (!InheritsFrom(classSymbol, recipeBaseType))
                continue;

            var displayName = GetAttributeConstructorArg<string>(classSymbol, displayNameAttr);
            var description = GetAttributeConstructorArg<string>(classSymbol, descriptionAttr);
            var tags = GetAttributeConstructorArg<string[]>(classSymbol, tagsAttr);

            if (displayName == null || description == null)
                continue;

            var optionDescriptors = new List<OptionDescriptor>();

            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                if (member.DeclaredAccessibility != Accessibility.Public)
                    continue;

                if (!HasSetOrInitAccessor(member))
                    continue;

                if (!SymbolEqualityComparer.Default.Equals(member.ContainingType, classSymbol))
                    continue;

                var isRequired = member.IsRequired || !IsNullable(member);

                var propDisplayName = GetAttributeConstructorArg<string>(member, displayNameAttr);
                var propDescription = GetAttributeConstructorArg<string>(member, descriptionAttr);

                optionDescriptors.Add(new OptionDescriptor
                {
                    Name = member.Name,
                    Type = member.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    Required = isRequired,
                    DisplayName = propDisplayName,
                    Description = propDescription
                });
            }

            descriptors.Add(new RecipeDescriptor
            {
                TypeName = TypeName.Parse($"{classSymbol.ToDisplayString()}, {classSymbol.ContainingAssembly.Name}"),
                DisplayName = displayName,
                Description = description,
                Tags = tags?.ToList() ?? [],
                Options = optionDescriptors
            });
        }

        var jsonList = descriptors
            .Select(d => JsonConvert.SerializeObject(d, Formatting.Indented))
            .ToArray();

        var jsonArrayLiteral = string.Join(",\n", jsonList.Select(json => $$$""""
                                  $$"""
                                  {{{json}}}
                                  """
                                  """"));

        var source = $$"""
                       using System;
                       using Rewrite.Core;
                       
                       [assembly: Recipes(
                       {{jsonArrayLiteral}}
                       )]
                       """;

        context.AddSource("RecipeMetadata.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static bool HasSetOrInitAccessor(IPropertySymbol propertySymbol)
    {
        var syntaxRefs = propertySymbol.DeclaringSyntaxReferences;
        foreach (var syntaxRef in syntaxRefs)
        {
            if (syntaxRef.GetSyntax() is PropertyDeclarationSyntax propDecl &&
                propDecl.AccessorList != null)
            {
                foreach (var accessor in propDecl.AccessorList.Accessors)
                {
                    if (accessor.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.SetAccessorDeclaration ||
                        accessor.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.InitAccessorDeclaration)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    private static bool InheritsFrom(INamedTypeSymbol symbol, INamedTypeSymbol baseType)
    {
        while (symbol.BaseType != null)
        {
            if (SymbolEqualityComparer.Default.Equals(symbol.BaseType, baseType))
                return true;
            symbol = symbol.BaseType;
        }
        return false;
    }

    private static T? GetAttributeConstructorArg<T>(ISymbol symbol, INamedTypeSymbol attrType)
    {
        foreach (var attr in symbol.GetAttributes())
        {
            if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, attrType))
            {
                if (attr.ConstructorArguments.Length == 1)
                {
                    return (T?)attr.ConstructorArguments[0].Value;
                }
            }
        }
        return default;
    }

    private static bool IsNullable(IPropertySymbol property)
    {
        if (property.Type is INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            namedType.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
            return true;

        return property.NullableAnnotation == NullableAnnotation.Annotated;
    }

    class RecipeSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds && cds.BaseList != null)
            {
                CandidateClasses.Add(cds);
            }
        }
    }
}
