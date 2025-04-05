using System.Reflection;

namespace Rewrite.Core.Config;

#if Analyzer
internal
#else
public 
#endif
readonly struct TypeName : IEquatable<TypeName>
{
    public string FullName { get; }
    public string AssemblyName { get; }
    
    public TypeName(string fullName, string assemblyName)
    {
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
    }
    
    public static TypeName Parse(string assemblyQualifiedName)
    {
        var wellKnownType = assemblyQualifiedName switch
        {
            "string" => typeof(string),
            "string?" => typeof(string),
            "int" => typeof(int),
            "int?" => typeof(int?),
            "float" => typeof(float),
            "float?" => typeof(float?),
            "bool" => typeof(bool),
            "bool?" => typeof(bool?),
            "decimal" => typeof(decimal),
            "decimal?" => typeof(decimal?),
            _ => null
        };
        if (wellKnownType != null)
        {
            return new TypeName(wellKnownType.FullName!, wellKnownType.Assembly.GetName().Name!);
        }
        if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
            throw new ArgumentException("Value cannot be null or empty.", nameof(assemblyQualifiedName));

        var parts = assemblyQualifiedName.Split(',');
        if (parts.Length < 2)
            throw new FormatException("Assembly qualified name must contain at least a type name and an assembly name.");

        string fullName = parts[0].Trim();
        string assemblyName = parts[1].Trim();

        return new TypeName(fullName, assemblyName);
    }

    public override string ToString() => $"{FullName}, {AssemblyName}";

    public static implicit operator string(TypeName typeName) => typeName.ToString();

    public bool Equals(TypeName other) =>
        string.Equals(FullName, other.FullName, StringComparison.Ordinal) &&
        string.Equals(AssemblyName, other.AssemblyName, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is TypeName other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((FullName?.GetHashCode() ?? 0) * 397) ^ (AssemblyName?.GetHashCode() ?? 0);
        }
    }

    public static bool operator ==(TypeName left, TypeName right) => left.Equals(right);
    public static bool operator !=(TypeName left, TypeName right) => !left.Equals(right);
}