using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Rewrite.Core.Config;

namespace Rewrite.Core;


public record RecipeIdentity
{
    public RecipeIdentity(string type, string nugetPackageName, string nugetPackageVersion) : this(InferTypeQualifiedName(type, nugetPackageName), nugetPackageName, nugetPackageVersion ) { }

    public static TypeName InferTypeQualifiedName(string name, string packageName)
    {
        if (name.IndexOf(',') >= 0)
            return TypeName.Parse(name);
        return new TypeName(name, packageName);
    }
    public RecipeIdentity(TypeName type, string nugetPackageName, string nugetPackageVersion)
    {
        if (type.AssemblyName == null)
            throw new InvalidOperationException("TypeName must be assembly qualified");
        this.Type = type;
        this.NugetPackageName = nugetPackageName;
        this.NugetPackageVersion = nugetPackageVersion;
    }

    public TypeName Type { get; init; }
    public string NugetPackageName { get; init; }
    public string NugetPackageVersion { get; init; }

    public void Deconstruct(out TypeName Type, out string NugetPackageName, out string NugetPackageVersion)
    {
        Type = this.Type;
        NugetPackageName = this.NugetPackageName;
        NugetPackageVersion = this.NugetPackageVersion;
    }

    public override string ToString()
    {
        return $"{Type.FullName}, {NugetPackageName}.{NugetPackageVersion}";
    }
}

