
namespace Rewrite.Core;

public record RecipePackage(string NugetPackageName, string NugetPackageVersion);
/// <summary>
/// Represents a single recipe inside a nuget package
/// </summary>
public record InstallableRecipe(string Id, string NugetPackageName, string NugetPackageVersion) : RecipePackage(NugetPackageName, NugetPackageVersion)
{
    public string ToUri() => $"recipe://{NugetPackageName}/{NugetPackageVersion}/{Id}";


    public static InstallableRecipe ParseUri(string uri) => ParseUri(new Uri(uri));
    public static InstallableRecipe ParseUri(Uri uri)
    {
        var segments = uri.PathAndQuery.Trim('/').Split("/");
        if (uri.Scheme != "recipe" || segments.Length != 2) throw new InvalidOperationException("Uri should be in format of 'recipe://PACKAGE_ID/PACKAGE_VERSION/RECIPE_ID"); 
        var packageName = uri.Host;
        var version = segments[0];
        var id = segments[1];
        return new InstallableRecipe(id, packageName, version);
    }

    // public RecipeIdentity(string type, string nugetPackageName, string nugetPackageVersion) : this(InferTypeQualifiedName(type, nugetPackageName), nugetPackageName, nugetPackageVersion ) { }


    // public static TypeName InferTypeQualifiedName(string name, string packageName)


    // {


    //     if (name.IndexOf(',') >= 0)


    //         return TypeName.Parse(name);


    //     return new TypeName(name, packageName);


    // }


    // public RecipeIdentity(TypeName type, string nugetPackageName, string nugetPackageVersion)


    // {


    //     if (type.AssemblyName == null)


    //         throw new InvalidOperationException("TypeName must be assembly qualified");


    //     this.Type = type;


    //     this.NugetPackageName = nugetPackageName;


    //     this.NugetPackageVersion = nugetPackageVersion;


    // }


    // public TypeName Type { get; init; }


    // public void Deconstruct(out TypeName Type, out string NugetPackageName, out string NugetPackageVersion)

    // {

    //     Type = this.Type;

    //     NugetPackageName = this.NugetPackageName;

    //     NugetPackageVersion = this.NugetPackageVersion;

    // }


    // public override string ToString()

    // {

    //     return $"{Type.FullName}, {NugetPackageName}.{NugetPackageVersion}";

    // }


    // public string ToUri() => $"recipe://{NugetPackageName}/{NugetPackageVersion}/{Type.AssemblyName}/{Type.FullName}";


    // public static RecipeIdentity ParseUri(Uri uri)

    // {

    //     var segments = uri.PathAndQuery.Trim('/').Split("/");

    //     var packageName = uri.Host;

    //     var version = segments[0];

    //     var assemblyName = segments[1];

    //     var recipeName = segments[2];

    //     var typeName = new TypeName(recipeName, assemblyName);

    //     return new RecipeIdentity(typeName, packageName, version);

    // }
}

