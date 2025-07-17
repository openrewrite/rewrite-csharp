﻿// Full corrected implementation for covariant return source generator
// Handles correct delegation chain and minimal explicit implementations

using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Rewrite.Analyzers;

public partial class CovariantGenerator
{
    private static void GenerateCovariantReturnTypeMembers(
        IndentedTextWriter writer,
        INamedTypeSymbol typeSymbol,
        SemanticModel semanticModel)
    {
        if (typeSymbol.TypeKind == TypeKind.Interface)
        {
            GenerateInterfaceCovariantMembers(writer, typeSymbol);
        }
        else if (typeSymbol.TypeKind == TypeKind.Class)
        {
            GenerateClassCovariantMembers(writer, typeSymbol);
        }
    }

    private static void GenerateInterfaceCovariantMembers(
        IndentedTextWriter writer,
        INamedTypeSymbol interfaceSymbol)
    {
        var allMethods = interfaceSymbol.AllInterfaces
            .SelectMany(i => i.GetMembers().OfType<IMethodSymbol>()
                .Where(m => !m.IsStatic &&
                            m.MethodKind == MethodKind.Ordinary &&
                            SymbolEqualityComparer.Default.Equals(m.ReturnType, i))
                .Select(m => (declaring: i, method: m)))
            .GroupBy(m => GetMethodSignature(m.method))
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var kv in allMethods)
        {
            var sig = kv.Key;
            var entry = kv.Value;
            if (interfaceSymbol.GetMembers().OfType<IMethodSymbol>()
                .Any(m => GetMethodSignature(m) == sig))
                continue;

            var declaring = entry.declaring;
            var method = entry.method;

            writer.Write("public new ");
            writer.Write(interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            writer.Write(" ");
            writer.Write(method.Name);
            WriteTypeParameters(writer, method);
            writer.Write("(");
            WriteParameters(writer, method);
            writer.Write(")");

            bool hasDefaultImpl = HasDefaultImplementation(method);
            WriteTypeParameterConstraints(writer, method);
            var returnType = interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            if (hasDefaultImpl)
            {
                writer.Write(" => (");
                writer.Write(interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(")((");
                writer.Write(declaring.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(")this).");
                writer.Write(method.Name);
                WriteTypeArguments(writer, method);
                writer.Write("(");
                WriteArgumentList(writer, method);
                writer.WriteLine(");");
            }
            else
            {
                writer.WriteLine(";");

                writer.Write(declaring.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(" ");
                writer.Write(declaring.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(".");
                writer.Write(method.Name);
                WriteTypeParameters(writer, method);
                writer.Write("(");
                WriteParameters(writer, method);
                writer.Write(")");
                WriteTypeParameterConstraints(writer, method);
                writer.Write(" => ");
                writer.Write("(");
                writer.Write(returnType);
                writer.Write(")");
                writer.Write(method.Name);
                WriteTypeArguments(writer, method);
                writer.Write("(");
                WriteArgumentList(writer, method);
                writer.WriteLine(");");
            }
        }
    }

    private static IEnumerable<(INamedTypeSymbol iface, IMethodSymbol method)> PredictAllInterfaceCovariantMethods(INamedTypeSymbol classSymbol)
    {
        var results = new List<(INamedTypeSymbol, IMethodSymbol)>();
        var visited = new HashSet<string>();

        foreach (var iface in classSymbol.AllInterfaces)
        {
            foreach (var ancestor in iface.AllInterfaces.Concat(new[] { iface }))
            {
                var methods = ancestor.GetMembers().OfType<IMethodSymbol>()
                    .Where(m => !m.IsStatic &&
                                m.MethodKind == MethodKind.Ordinary &&
                                SymbolEqualityComparer.Default.Equals(m.ReturnType, ancestor));

                foreach (var method in methods)
                {
                    var sig = iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + "." + GetMethodSignatureWithoutReturnType(method);
                    if (visited.Add(sig))
                    {
                        results.Add((iface, method));
                    }
                }
            }
        }

        return results;
    }

    private static void GenerateClassCovariantMembers(
        IndentedTextWriter writer,
        INamedTypeSymbol classSymbol)
    {
        var classMethods = classSymbol.GetMembers().OfType<IMethodSymbol>()
            .Where(m => !m.IsStatic && m.MethodKind == MethodKind.Ordinary)
            .ToList();

        var methodImplementations = new Dictionary<string, IMethodSymbol>();
        foreach (var classMethod in classMethods)
        {
            var methodSig = GetMethodSignatureWithoutReturnType(classMethod);
            foreach (var iface in classSymbol.AllInterfaces)
            {
                var interfaceMethod = iface.GetMembers().OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.Name == classMethod.Name &&
                                         ParametersMatch(m, classMethod) &&
                                         m.TypeParameters.Length == classMethod.TypeParameters.Length);

                if (interfaceMethod != null &&
                    IsReturnTypeCompatible(classMethod.ReturnType, interfaceMethod.ReturnType))
                {
                    methodImplementations[methodSig] = classMethod;
                }
            }
        }

        var methodGroups = PredictAllInterfaceCovariantMethods(classSymbol)
            .GroupBy(x => GetMethodSignatureWithoutReturnType(x.method))
            .ToDictionary(g => g.Key, g => g.ToList());

        var emittedRedirects = new HashSet<string>();
        var existingMethods = new HashSet<string>(classMethods.Select(GetMethodSignatureWithoutReturnType));

        foreach (var kv in methodGroups)
        {
            var methodSig = kv.Key;
            var group = kv.Value;
            IMethodSymbol template = group.First().method;

            bool hasUserImpl = methodImplementations.ContainsKey(methodSig);
            bool hasSyntheticImpl = existingMethods.Contains(methodSig);
            bool anyHasUserDefaultImpl = group.Any(x => !x.method.IsAbstract);
            bool needsImpl = !hasUserImpl && !hasSyntheticImpl && anyHasUserDefaultImpl;

            if (!classSymbol.IsAbstract && needsImpl)
            {
                existingMethods.Add(methodSig);

                writer.Write("public virtual ");
                writer.Write(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(" ");
                writer.Write(template.Name);
                WriteTypeParameters(writer, template);
                writer.Write("(");
                WriteParameters(writer, template);
                writer.WriteLine(")");
                WriteTypeParameterConstraints(writer, template);
                writer.WriteLine("{");
                writer.Indent++;

                var primaryIface = group.First(x => !x.method.IsAbstract).iface;
                writer.Write("return (");
                writer.Write(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(")((");
                writer.Write(primaryIface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(")this).");
                writer.Write(template.Name);
                WriteTypeArguments(writer, template);
                writer.Write("(");
                WriteArgumentList(writer, template);
                writer.WriteLine(");");

                writer.Indent--;
                writer.WriteLine("}");
            }

            foreach (var pair in group)
            {
                
                var iface = pair.iface;
                var method = pair.method;
                var redirectKey = iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + "." + methodSig;
                if (!emittedRedirects.Add(redirectKey)) continue;
                //
                var impl = classSymbol.FindImplementationForInterfaceMember(method);
                if (impl is IMethodSymbol { IsImplicitlyDeclared: false } && SymbolEqualityComparer.Default.Equals(impl.ContainingSymbol, classSymbol))
                    continue;
                
                var returnType = iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                writer.Write(iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(" ");
                writer.Write(iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                writer.Write(".");
                writer.Write(method.Name);
                WriteTypeParameters(writer, method);
                writer.Write("(");
                WriteParameters(writer, method);
                writer.Write(")");
                WriteTypeParameterConstraints(writer, method);
                writer.Write(" => ");
                writer.Write("(");
                writer.Write(returnType);
                writer.Write(")");
                writer.Write(template.Name);
                WriteTypeArguments(writer, method);
                writer.Write("(");
                WriteArgumentList(writer, method);
                writer.WriteLine(");");
            }
        }

        if (classSymbol.BaseType != null)
        {
            var baseMethods = GetAllVirtualMethods(classSymbol.BaseType)
                .Where(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, m.ContainingType));

            foreach (var baseMethod in baseMethods)
            {
                var overridingMethod = classSymbol.GetMembers()
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.IsOverride &&
                                         m.Name == baseMethod.Name &&
                                         ParametersMatch(m, baseMethod));

                if (overridingMethod == null)
                {
                    writer.Write("public override ");
                    writer.Write(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    writer.Write(" ");
                    writer.Write(baseMethod.Name);
                    WriteTypeParameters(writer, baseMethod);
                    writer.Write("(");
                    WriteParameters(writer, baseMethod);
                    writer.Write(")");
                    WriteTypeParameterConstraints(writer, baseMethod);
                    writer.Write(" => (");
                    writer.Write(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    writer.Write(")base.");
                    writer.Write(baseMethod.Name);
                    WriteTypeArguments(writer, baseMethod);
                    writer.Write("(");
                    WriteArgumentList(writer, baseMethod);
                    writer.WriteLine(");");
                }
            }
        }
    }

    private static bool IsReturnTypeCompatible(ITypeSymbol derivedType, ITypeSymbol baseType)
    {
        if (SymbolEqualityComparer.Default.Equals(derivedType, baseType))
            return true;

        if (derivedType is INamedTypeSymbol namedDerived)
        {
            var currentBase = namedDerived.BaseType;
            while (currentBase != null)
            {
                if (SymbolEqualityComparer.Default.Equals(currentBase, baseType))
                    return true;
                currentBase = currentBase.BaseType;
            }

            if (baseType.TypeKind == TypeKind.Interface)
            {
                return namedDerived.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, baseType));
            }
        }

        return false;
    }
}
