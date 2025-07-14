// Full corrected implementation for covariant return source generator
// Handles correct delegation chain and minimal explicit implementations

using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Rewrite.Analyzers;

public partial class CovariantGenerator
{
   

    private static string GetMethodSignature(IMethodSymbol method)
    {
        var sb = new StringBuilder();
        sb.Append(method.Name);

        if (method.TypeParameters.Length > 0)
        {
            sb.Append('<');
            for (int i = 0; i < method.TypeParameters.Length; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(method.TypeParameters[i].Name);
            }

            sb.Append('>');
        }

        sb.Append('(');
        for (int i = 0; i < method.Parameters.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(method.Parameters[i].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        }

        sb.Append(')');
        return sb.ToString();
    }

    private static IEnumerable<IMethodSymbol> GetAllVirtualMethods(INamedTypeSymbol typeSymbol)
    {
        var current = typeSymbol;
        while (current != null)
        {
            foreach (var member in current.GetMembers())
            {
                if (member is IMethodSymbol method &&
                    (method.IsVirtual || method.IsOverride) &&
                    !method.IsSealed &&
                    method.MethodKind == MethodKind.Ordinary)
                {
                    yield return method;
                }
            }

            current = current.BaseType;
        }
    }

    private static bool HasDefaultImplementation(IMethodSymbol interfaceMethod)
    {
        return interfaceMethod.IsAbstract == false;
    }

    private static void WriteTypeParameters(IndentedTextWriter writer, IMethodSymbol method)
    {
        if (method.TypeParameters.Length == 0)
            return;

        writer.Write("<");
        for (int i = 0; i < method.TypeParameters.Length; i++)
        {
            if (i > 0) writer.Write(", ");
            writer.Write(method.TypeParameters[i].Name);
        }

        writer.Write(">");
    }

    private static void WriteTypeArguments(IndentedTextWriter writer, IMethodSymbol method)
    {
        if (method.TypeParameters.Length == 0)
            return;

        writer.Write("<");
        for (int i = 0; i < method.TypeParameters.Length; i++)
        {
            if (i > 0) writer.Write(", ");
            writer.Write(method.TypeParameters[i].Name);
        }

        writer.Write(">");
    }

    private static void WriteTypeParameterConstraints(IndentedTextWriter writer, IMethodSymbol method)
    {
        foreach (var typeParam in method.TypeParameters)
        {
            var constraints = new List<string>();

            if (typeParam.HasReferenceTypeConstraint)
                constraints.Add("class");
            if (typeParam.HasValueTypeConstraint)
                constraints.Add("struct");
            if (typeParam.HasUnmanagedTypeConstraint)
                constraints.Add("unmanaged");
            if (typeParam.HasNotNullConstraint)
                constraints.Add("notnull");

            foreach (var constraintType in typeParam.ConstraintTypes)
            {
                constraints.Add(constraintType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            }

            if (typeParam.HasConstructorConstraint && !typeParam.HasValueTypeConstraint)
                constraints.Add("new()");

            if (constraints.Count > 0)
            {
                writer.Write(" where ");
                writer.Write(typeParam.Name);
                writer.Write(" : ");
                writer.Write(string.Join(", ", constraints));
            }
        }
    }

    private static void WriteParameters(IndentedTextWriter writer, IMethodSymbol method)
    {
        for (int i = 0; i < method.Parameters.Length; i++)
        {
            if (i > 0) writer.Write(", ");
            var param = method.Parameters[i];

            if (param.RefKind == RefKind.Ref)
                writer.Write("ref ");
            else if (param.RefKind == RefKind.Out)
                writer.Write("out ");
            else if (param.RefKind == RefKind.In)
                writer.Write("in ");

            writer.Write(param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            writer.Write(" ");
            writer.Write(param.Name);
        }
    }

    private static void WriteArgumentList(IndentedTextWriter writer, IMethodSymbol method)
    {
        for (int i = 0; i < method.Parameters.Length; i++)
        {
            if (i > 0) writer.Write(", ");
            var param = method.Parameters[i];

            if (param.RefKind == RefKind.Ref)
                writer.Write("ref ");
            else if (param.RefKind == RefKind.Out)
                writer.Write("out ");

            writer.Write(param.Name);
        }
    }

    private static bool ParametersMatch(IMethodSymbol method1, IMethodSymbol method2)
    {
        if (method1.Parameters.Length != method2.Parameters.Length)
            return false;

        if (method1.TypeParameters.Length != method2.TypeParameters.Length)
            return false;

        for (int i = 0; i < method1.Parameters.Length; i++)
        {
            if (!SymbolEqualityComparer.Default.Equals(
                    method1.Parameters[i].Type,
                    method2.Parameters[i].Type))
                return false;
        }

        return true;
    }

    private static string GetMethodSignatureWithoutReturnType(IMethodSymbol method)
    {
        var sb = new StringBuilder();
        sb.Append(method.Name);

        if (method.TypeParameters.Length > 0)
        {
            sb.Append('<');
            for (int i = 0; i < method.TypeParameters.Length; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(method.TypeParameters[i].Name);
            }

            sb.Append('>');
        }

        sb.Append('(');
        for (int i = 0; i < method.Parameters.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(method.Parameters[i].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        }

        sb.Append(')');
        return sb.ToString();
    }
}
