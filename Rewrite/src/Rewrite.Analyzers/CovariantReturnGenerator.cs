using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#pragma warning disable CS0162 // Unreachable code detected

namespace Rewrite.Analyzers
{
    [Generator]
    public class CovariantReturnGenerator : ISourceGenerator
    {
        private ImmutableHashSet<INamedTypeSymbol> _interfacesRequiringGenericVersion = null!;

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {

            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            var compilation = context.Compilation;
            var model = compilation.GetSemanticModel(receiver.Classes.FirstOrDefault()?.SyntaxTree ?? receiver.Interfaces.FirstOrDefault()?.SyntaxTree!);
            var allInterfaces1 = model.LookupNamespacesAndTypes(0)
                .OfType<INamedTypeSymbol>()
                .Where(x => x.TypeKind == TypeKind.Interface)
                .Where(x => x.ContainingNamespace.ToDisplayString().StartsWith("Rewrite"))
                .ToList();
            // compilation.GetSemanticModel(x.SyntaxTree).

            var currentAssemblyInterface = receiver.Interfaces
                .Select(x => compilation.GetSemanticModel(x.SyntaxTree).GetDeclaredSymbol(x)!)
                .Distinct(SymbolEqualityComparer<INamedTypeSymbol>.Default)
                .ToList();

            var allInterfaces = allInterfaces1.Union(currentAssemblyInterface, SymbolEqualityComparer<INamedTypeSymbol>.Default).ToList();

            // var allInterfaces = receiver.Interfaces
            //     .Select(x => GetFullyQualifiedName(x, compilation))
            //     .Distinct()
            //     .Select(x => compilation.GetTypeByMetadataName(x))
            //     .ToList();

            var interfacesWithCovariantReturns = allInterfaces
                .Where(interfaceSymbol => interfaceSymbol
                    .GetMembers()
                    .OfType<IMethodSymbol>()
                    .Any(methodSymbol => SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType, interfaceSymbol)))
                .ToImmutableHashSet(SymbolEqualityComparer.Default);

            var interfacesWithCovariantReturnsInCurrentAssembly = interfacesWithCovariantReturns
                .Cast<INamedTypeSymbol>()
                .Where(x => currentAssemblyInterface.Contains(x, SymbolEqualityComparer<ISymbol>.Default))
                .ToList();

            _interfacesRequiringGenericVersion = currentAssemblyInterface
                .Where(iface => iface
                    .AllInterfaces
                    .Any(baseInterface => interfacesWithCovariantReturns.Contains(baseInterface)))
                .Union(interfacesWithCovariantReturnsInCurrentAssembly, SymbolEqualityComparer.Default)
                .Cast<INamedTypeSymbol>()
                .ToImmutableHashSet(SymbolEqualityComparer<INamedTypeSymbol>.Default);

            var allInterfacesWithGenericVersion = allInterfaces
                .Where(iface => iface
                    .AllInterfaces
                    .Any(baseInterface => interfacesWithCovariantReturns.Contains(baseInterface)))
                .Union(interfacesWithCovariantReturnsInCurrentAssembly, SymbolEqualityComparer.Default)
                .Cast<INamedTypeSymbol>()
                .ToImmutableHashSet(SymbolEqualityComparer<INamedTypeSymbol>.Default);


            foreach (var interfaceSymbol in _interfacesRequiringGenericVersion.Cast<INamedTypeSymbol>())
            {

                var source = GenerateCovariantInterface(interfaceSymbol);
                source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();
                context.AddSource($"{ConvertToMetadataName(interfaceSymbol.ToString())}.covariant.g.cs", SourceText.From(source, Encoding.UTF8));
            }
            context.AddSource($"_test.cs", $"//{allInterfacesWithGenericVersion.Count}");

            var classesImplementingCovariantTypes = receiver.Classes
                .Select(syntax => (Syntax: syntax, Model: compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax)!))
                .Where(x => x.Model.AllInterfaces.Any(@interface => _interfacesRequiringGenericVersion.Contains(@interface)))
                .Select(x => x.Model)
                .Distinct(SymbolEqualityComparer<INamedTypeSymbol>.Default)
                .ToList();

            foreach (var classSymbol in classesImplementingCovariantTypes)
            {
                var source = GenerateClassGenericInterfaceImplementation(classSymbol);
                source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();
                context.AddSource($"{ConvertToMetadataName(classSymbol.ToString())}.covariant.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        static string ConvertToMetadataName(string typeName)
        {
            // Regex to find generic type parameters
            var match = Regex.Match(typeName, @"^(.+?)<([^>]+)>$");
            if (!match.Success) return typeName; // Not a generic type

            string baseName = match.Groups[1].Value;
            string typeParams = match.Groups[2].Value;

            // Count number of generic parameters
            int genericArgCount = typeParams.Split(',').Length;
            return $"{baseName}`{genericArgCount}";
        }

        private (string, string) GeneratePartialDeclarationScope(INamedTypeSymbol symbol)
        {

            var parentTypes = new Stack<INamedTypeSymbol>();
            var parent = (INamespaceOrTypeSymbol)symbol.ContainingSymbol;
            while (parent.IsType)
            {
                parentTypes.Push((INamedTypeSymbol)parent);
                if (((INamespaceOrTypeSymbol)parent.ContainingSymbol).IsNamespace)
                    break;
                parent = (INamedTypeSymbol)parent.ContainingSymbol;
            }

            var builder = new StringBuilder();
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            builder.AppendLine($"namespace {namespaceName};");
            builder.AppendLine();

            var sublevel = 0;
            while (parentTypes.Count > 0)
            {
                var parentType = parentTypes.Pop();
                builder.AppendLine($"public partial {parentType.TypeKind.ToString().ToLower()} {parentType.Name}");
                builder.AppendLine("{");
                sublevel++;
            }

            var end = new string('}', sublevel);

            return (builder.ToString(), end);
        }

        private string GenerateClassGenericInterfaceImplementation(INamedTypeSymbol classSymbol)
        {
            var builder = new StringBuilder();
            var (start, end) = GeneratePartialDeclarationScope(classSymbol);
            builder.AppendLine(start);

            var interfaces = classSymbol
                .Interfaces
                .Where(@interface => _interfacesRequiringGenericVersion.Contains(@interface))
                .ToList();


            // Generate the generic version of the interface
            builder.AppendLine($"public partial class {classSymbol.Name}{GetTypeParameters(classSymbol)} : {string.Join(", ", interfaces.Select(x => $"{x}<{classSymbol.Name}{GetTypeParameters(classSymbol)}>"))}");
            builder.AppendLine("{}");
            builder.AppendLine(end);
            return builder.ToString();
        }

        private IEnumerable<MethodDeclarationSyntax> GetMethodsWithReturnTypeAsType(InterfaceDeclarationSyntax declaration)
        {
            return declaration.Members.OfType<MethodDeclarationSyntax>().Where(x => x.ReturnType.ToString() == declaration.Identifier.ValueText);
        }


        private string GenerateCovariantInterface(INamedTypeSymbol typeSymbol)
        {
            var builder = new StringBuilder();
            var (start, end) = GeneratePartialDeclarationScope(typeSymbol);
            builder.AppendLine(start);

            // Generate the generic version of the interface
            builder.Append($"public partial interface {typeSymbol.Name}<out T> : {typeSymbol.Name}");

            // Add the generic version of the parent interfaces
            foreach (var parentInterface in typeSymbol.Interfaces.Where(x => _interfacesRequiringGenericVersion.Contains(x)))
            {
                builder.Append($", {parentInterface.Name}<T>");
            }

            builder.AppendLine($" where T : {typeSymbol.Name}");
            // builder.AppendLine($" where T : J");
            builder.AppendLine();
            builder.AppendLine("{");

            bool IsImplemented(IMethodSymbol methodSymbol)
            {
                if(methodSymbol.MethodKind != MethodKind.ExplicitInterfaceImplementation)
                    return false;

                return methodSymbol
                    .DeclaringSyntaxReferences
                    .Select(r => (MethodDeclarationSyntax)r.GetSyntax())
                    .Any(methodSyntax => methodSyntax.Body is not null || methodSyntax.ExpressionBody is not null);

            }

            var methodsRequiringCovariantDefinitions = typeSymbol
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind == MethodKind.Ordinary)
                .Where(method => method.ReturnType.Equals(typeSymbol, SymbolEqualityComparer.Default) && method.IsAbstract)
                .GroupBy(x => (x.Name, x.ReturnType.ToDisplayString(), GetParameterList(x)))
                .Select(x => x.First())
                .ToList();


            // Generate new methods with covariant return types
            foreach (var method in methodsRequiringCovariantDefinitions)
            {
                builder.AppendLine($"    public new T {method.Name}({GetParameterList(method)});");
                builder.AppendLine($"    {typeSymbol.Name} {typeSymbol.Name}.{method.Name}{GetTypeParameters(method)}({GetParameterList(method)}) => {method.Name}({GetArgumentList(method)});");
            }

            builder.AppendLine("}");
            builder.AppendLine();

            // Generate the non-generic version of the interface
            builder.AppendLine($"public partial interface {typeSymbol.Name}");
            builder.AppendLine("{");

            // Add default implementations for methods inherited from parent interfaces
            var distinctMethods  = new HashSet<string>();
            foreach (var baseInterface in typeSymbol.AllInterfaces)
            {

                foreach (var method in baseInterface.GetMembers().OfType<IMethodSymbol>().Where(x => !IsImplemented(x)))
                {
                    var methodSignature = $"{typeSymbol.Name} {method.Name}{GetTypeParameters(method)}({GetParameterList(method)})";
                    if (method.ReturnType.Equals(baseInterface, SymbolEqualityComparer.Default) && !distinctMethods.Contains(methodSignature))
                    {
                        builder.AppendLine($"    public new {methodSignature} => ({typeSymbol.Name})(({baseInterface.Name})this).{method.Name}({GetArgumentList(method)});");
                        distinctMethods.Add(methodSignature);
                    }
                }
            }

            builder.AppendLine("}");
            builder.AppendLine(end);

            return builder.ToString();
        }
        private string GetTypeParameters(INamedTypeSymbol method)
        {
            if (method.TypeParameters.IsEmpty)
                return "";
            return $"<{string.Join(", ", method.TypeParameters.Select(p => p.Name))}>";
        }
        private string GetTypeParameters(IMethodSymbol method)
        {
            if (method.TypeParameters.IsEmpty)
                return "";
            return $"<{string.Join(", ", method.TypeParameters.Select(p => p.Name))}>";
        }
        private string GetParameterList(IMethodSymbol method)
        {
            return string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToString().Replace("?","")} {p.Name}"));
        }

        private string GetArgumentList(IMethodSymbol method)
        {
            return string.Join(", ", method.Parameters.Select(p => p.Name));
        }
    }

    public class SyntaxReceiver : ISyntaxReceiver
    {
        public List<InterfaceDeclarationSyntax> Interfaces { get; } = new();

        public List<ClassDeclarationSyntax> Classes { get; set; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not TypeDeclarationSyntax typeDeclaration || !typeDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                return;
            switch (syntaxNode)
            {
                case InterfaceDeclarationSyntax interfaceDeclaration:
                    Interfaces.Add(interfaceDeclaration);
                    break;
                case ClassDeclarationSyntax classDeclaration when classDeclaration.BaseList?.Types.Any() == true:
                    Classes.Add(classDeclaration);
                    break;
            }
        }
    }
}
