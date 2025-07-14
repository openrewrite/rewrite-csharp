using System.Text.RegularExpressions;

namespace Rewrite.Core;

public static class TypeExtensions
{
    private static readonly Regex _assemblyAttributesRegex = new(
        @"\s*,\s*(Version|Culture|PublicKeyToken)\s*=\s*[^,\[\]]+",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);
    /// <summary>
    /// Returns the assembly-qualified name of the given <see cref="Type"/>, excluding version, culture, and public key token information.
    /// </summary>
    /// <param name="type">The type to get the name for.</param>
    /// <returns>A string containing the type's assembly-qualified name without version, culture, or public key token.</returns>
    public static string AssemblyQualifiedNameWithoutVersion(this Type type)
    {
        return _assemblyAttributesRegex.Replace(type.AssemblyQualifiedName!, string.Empty);
        //
        // if (type == null)
        //     throw new ArgumentNullException(nameof(type));
        //
        // var fullName = type.FullName ?? type.Name;
        // var assemblyName = type.Assembly.GetName().Name;
        //
        // return $"{fullName}, {assemblyName}";
    }
}








