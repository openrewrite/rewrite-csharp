// using Newtonsoft.Json.Linq;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Analyzers.Authoring;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Rewrite.Analyzers;

[Generator]
public sealed partial class CovariantGenerator : IIncrementalGenerator
{
        
    static bool HasCovariantMethods(INamedTypeSymbol symbol) => symbol
        .GetMembers()
        .OfType<IMethodSymbol>()
        .Any(methodSymbol => SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, symbol));
        
    static IEnumerable<IMethodSymbol> SelectCovariantMethods(INamedTypeSymbol symbol) => symbol
        .GetMembers()
        .OfType<IMethodSymbol>()
        .Where(methodSymbol => SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, symbol));
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {


        var partialsToGenerate = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, _) => node.IsPartialTypeDeclaration(),
                transform: (syntaxContext, _) =>
                {
                    
                    var typeDeclaration = (TypeDeclarationSyntax)syntaxContext.Node;
                    var semanticModel = syntaxContext.SemanticModel;
                    var typeModel = semanticModel.GetDeclaredSymbol(typeDeclaration);
                    if (typeModel == null || !(HasCovariantMethods(typeModel) || typeModel.AllInterfaces.Any(HasCovariantMethods)))
                        return default;
                    var count = typeModel.Locations.Length;
                    var partialDeclarationIndex = typeModel.Locations.Select(x => x.SourceTree).ToList().IndexOf(typeDeclaration.SyntaxTree);
                    if (partialDeclarationIndex > 0) // force generating only once per local symbol
                    {
                        return default;
                    }


// #pragma warning disable CS0162 // Unreachable code detected
                    var sb = new StringBuilder();
                    using var writer = new IndentedTextWriter(new StringWriter(sb), "    ");
                    var partialDeclaration = PartialTypeModel.GetPartialDeclaration(typeDeclaration);
                    writer.WriteLine("//test1");
                    writer.WriteLine(partialDeclaration.Before);
                    writer.Indent = partialDeclaration.BodyIndentationLevel;
                    try
                    {
                        GenerateCovariantReturnTypeMembers(writer, typeModel, semanticModel);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    // if (HasCovariantMethods(namedSymbol) || namedSymbol.AllInterfaces.Any(HasCovariantMethods))
                    // {
                    //     var genericVersion = typeDeclaration.PreservingParent(node => CreateGenericVersionOfType(node, namedSymbol));
                    //     var model = BuildModel(namedSymbol, genericVersion, typeDeclaration.GetGeneratorQualifiedSourceFileName(this));
                    //     return model;
                    // }
                    //
                    // var selfTypeName = typeDeclaration.Identifier.ValueText;
                    // var isInterface = typeDeclaration.Keyword.IsKind(InterfaceKeyword);
                    // // if (typeDeclaration.Keyword.IsKind(InterfaceKeyword))
                    // if (true)
                    // {
                    //     var parentCovariantMethods = typeModel.AllInterfaces.SelectMany(SelectCovariantMethods).ToList();
                    //     foreach (var method in parentCovariantMethods)
                    //     {
                    //         var parentTypeName = method.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    //         if (method.IsAbstract) // no default implementation
                    //         {
                    //             var requiresNew = isInterface;
                    //             if (!isInterface)
                    //             {
                    //                 
                    //                 var classSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration)!;
                    //                 var targetInterface = method.ContainingType;
                    //                 
                    //                 var heirarchyInterface = classSymbol.AllInterfaces
                    //                     .Where(i => !SymbolEqualityComparer.Default.Equals(i, targetInterface) || i.AllInterfaces.Contains(targetInterface, SymbolEqualityComparer.Default))
                    //                     .ToImmutableArray();
                    //                 foreach (var iface in heirarchyInterface)
                    //                 {
                    //                     writer.WriteLine($"{iface} {iface}.{method.Name}() => {method.Name}();");
                    //                 }
                    //                 requiresNew = classSymbol?.FindImplementationForInterfaceMember(method) == null;
                    //             }
                    //
                    //             if (requiresNew)
                    //             {
                    //                 writer.WriteLine($"public new {selfTypeName} {method.Name}();");
                    //             }
                    //
                    //             writer.WriteLine($"{parentTypeName} {parentTypeName}.{method.Name}() => {method.Name}();");
                    //         }
                    //         else
                    //         {
                    //             writer.WriteLine($"public new  {selfTypeName} {method.Name}() => ({selfTypeName})(({parentTypeName})this).{method.Name}();");
                    //         }
                    //     }
                    // parentCovariantMethods.SelectMany(method =>
                    // {
                    //     var type = MethodToGenerate.GenerationType.NewCovariant;
                    //
                    //     return
                    //     new []{
                    //         new MethodToGenerate()
                    //         {
                    //             TemplateType = type,
                    //             ReturnType = typeModel.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    //             Name = method.Name,
                    //             Parameters = method.Parameters
                    //                 .Select(p => new ParameterModel
                    //                 {
                    //                     Name = p.Name,
                    //                     TypeName = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    //                 })
                    //                 .ToImmutableEquatableArray()
                    //         }
                    //     };
                    // });
                    // }

                    writer.Indent--;
                    writer.Write(partialDeclaration.After);

                    return (Source: writer.InnerWriter.ToString(), FileName: typeDeclaration.GetGeneratorQualifiedSourceFileName(this));
// #pragma warning restore CS0162 // Unreachable code detected

                })
            .Where(model => model.Source != null)
            .Select((x, _) => (Source: x.Source!, FileName: x.FileName!));


        // partialsToGenerate.Combine(context.CompilationProvider)
        // context.CompilationProvider.Select(x => x.getse)

        // var partialTypesThatAreCovariantOrInheritCovariant = partialTypes
        //     .Where(x =>
        //         x.SemanticModel.GetDeclaredSymbol(x.Node) is INamedTypeSymbol namedSymbol && (HasCovariantMethods(namedSymbol) || namedSymbol.AllInterfaces.Any(HasCovariantMethods)));


        //
        // var allNamedSymbols = context.CompilationProvider.SelectMany(static (compilation, _) =>
        // {
        //     var collector = new SymbolCollector();
        //     collector.VisitNamespace(compilation.GlobalNamespace);
        //     return collector.Results;
        // });
        //
        // var allInterfaces = allNamedSymbols
        //     .Where(x => x.TypeKind == TypeKind.Interface)
        //     .Where(x => x.ContainingNamespace.ToDisplayString().StartsWith("MyProject"));
        //
        // var currentAssemblyInterface = allInterfaces
        //     .Where(x => x.Locations.Any(loc => loc.IsInSource));
        //
        //
        //
        // var interfacesWithCovariantReturns = allInterfaces
        //     .Where(HasCovariantMethods)
        //     .Collect()
        //     .Select((x,_) => x.ToImmutableHashSet(SymbolEqualityComparer<INamedTypeSymbol>.Default));
        //
        // var interfacesWithCovariantReturnsInCurrentAssembly = interfacesWithCovariantReturns
        //     .SelectMany((x, _) => x)
        //     .Where(x => x.Locations.Any(loc => loc.IsInSource))
        //     .Collect()
        //     .Select((x,_) => x.ToImmutableHashSet(SymbolEqualityComparer<INamedTypeSymbol>.Default));

        // var interfacesHavingCovariantReturnBase = currentAssemblyInterface
        //     .Where(iface => iface
        //         .AllInterfaces
        //         .Any(baseInterface => interfacesWithCovariantReturns.Contains(baseInterface)));

        // ex.
        // interface A : B {}
        // interface B { B Hi(); }
        // var interfacesHavingCovariantReturnBaseTypesInCurrentAssembly = currentAssemblyInterface
        //     .Combine(interfacesWithCovariantReturns)
        //     .Where((currentAndAll) =>
        //     {
        //         var (currentInterface, allCovariantInterfaces) = currentAndAll;
        //         return currentInterface
        //             .AllInterfaces
        //             .Any(baseInterface => allCovariantInterfaces.Contains(baseInterface));
        //     })
        //     .Select((x,_) => x.Left);
        //
        // var interfacesRequiringGenericVersion = interfacesHavingCovariantReturnBaseTypesInCurrentAssembly
        //     .Collect()
        //     .Combine(interfacesWithCovariantReturnsInCurrentAssembly)
        //     .SelectMany((pair, _) => pair.Left.Union(pair.Right, SymbolEqualityComparer<INamedTypeSymbol>.Default))
        //     .Select((x,_) => (TypeDeclarationSyntax)x.DeclaringSyntaxReferences.First().GetSyntax())
        //     .Select((x,_) => new TemplateModel(x, x.GetGeneratorQualifiedSourceFileName(this)));

        // var interfacesRequiringGenericVersion = interfacesHavingCovariantReturnBaseTypes
        //     .Combine(interfacesWithCovariantReturnsInCurrentAssembly)
        //     .SelectMany(x => x.)

        // .Union(interfacesWithCovariantReturnsInCurrentAssembly, SymbolEqualityComparer.Default)
        // .Cast<INamedTypeSymbol>()
        // .ToImmutableHashSet(SymbolEqualityComparer<INamedTypeSymbol>.Default);


        context.RegisterSourceOutput(partialsToGenerate, (spc, templateModel) => { spc.AddSource(templateModel.FileName, SourceText.From(templateModel.Source, Encoding.UTF8)); });

    }

    TypeDeclarationSyntax CreateGenericVersionOfType(TypeDeclarationSyntax node, INamedTypeSymbol nodeSymbol)
    {
        var originalInterfaceFullyQualifiedName = ParseTypeName(nodeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        return node
            .WithTypeParameterList(
                TypeParameterList(
                    SingletonSeparatedList(
                        TypeParameter("T"))))
            .WithConstraintClauses(SingletonList(

                TypeParameterConstraintClause(IdentifierName("T"))
                    .WithConstraints(SingletonSeparatedList<TypeParameterConstraintSyntax>(TypeConstraint(originalInterfaceFullyQualifiedName)))
            ))
            .WithBaseList(
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(originalInterfaceFullyQualifiedName))));
    }
}

// static CovariantInterfaceModel BuildModel(INamedTypeSymbol symbol, TypeDeclarationSyntax syntax, string fileName)
    // {
    //     
    //     return new CovariantInterfaceModel(syntax, fileName)
    //     {
    //         Name = symbol.Name,
    //         TypeParameters = ImmutableEquatableArray<TypeParameterModel>.Empty,
    //         Methods = SelectCovariantMethods(symbol)
    //             .Select(m => new CovariantMethodModel
    //             {
    //                 Name = m.Name,
    //                 IsAbstract = m.IsAbstract,
    //                 Parameters = m.Parameters
    //                     .Select(p => new ParameterModel
    //                     {
    //                         Name = p.Name,
    //                         TypeName = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
    //                     })
    //                     .ToImmutableEquatableArray()
    //             })
    //             .ToImmutableEquatableArray(),
    //
    //         BaseInterfaces = symbol.Interfaces
    //             .Where(HasCovariantMethods)
    //             .Select(i => BuildModel(i, syntax, fileName)) // use same syntax/fileName; base interfaces won't affect layout
    //             .ToImmutableEquatableArray()
    //     };
    // }

    
//     private string RenderBody(CovariantInterfaceModel model)
//     {
//         var body = $$"""
//             {{model.Methods.Render(method => $$"""
//             public new T {{method.Name}}{{method.Parameters.RenderParameters(p => $"{p.TypeName} {p.Name}")}};
//             {{model.Name}} {{model.Name}}.{{method.Name}}{{method.Parameters.RenderParameters(p => $"{p.TypeName} {p.Name}")}} => {{method.Name}}({{method.Parameters.Render(p => p.Name, ", ")}});
//             """)}}
//             """;
//
//         return body;
//     }
//
// }
//
// public class ReducedModelKeyComparer
// {
//     public static IEqualityComparer<ReducedModel> Instance { get; set; } = null!;
// }
//
// public class ReducedModel
// {
//     public ReducedModel(SyntaxNode contextNode)
//     {
//         throw new NotImplementedException();
//     }
//
//     public SyntaxTree Node { get; set; }
// }
//
// public static class Extensions
// {
//     
// }
//
//
// class SymbolCollector : SymbolVisitor
// {
//     public List<INamedTypeSymbol> Results { get; } = new();
//
//     public override void VisitNamespace(INamespaceSymbol symbol)
//     {
//         foreach (var member in symbol.GetMembers())
//             member.Accept(this);
//     }
//
//     public override void VisitNamedType(INamedTypeSymbol symbol)
//     {
//         Results.Add(symbol);
//
//         foreach (var nested in symbol.GetTypeMembers())
//             nested.Accept(this);
//     }
// }
//
// public class SymbolEqualityComparer<T> : IEqualityComparer<T> where T : ISymbol
// {
//     public static readonly SymbolEqualityComparer<T> Default = new();
//     public bool Equals(T? x, T? y) => SymbolEqualityComparer.Default.Equals(x, y);
//
//     public int GetHashCode(T obj) => SymbolEqualityComparer.Default.GetHashCode(obj);
// }