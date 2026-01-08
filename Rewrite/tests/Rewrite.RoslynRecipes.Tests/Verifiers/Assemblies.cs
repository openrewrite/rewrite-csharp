using Microsoft.CodeAnalysis.Testing;

namespace Rewrite.RoslynRecipes.Tests.Verifiers;

public static class Assemblies
{
    private const string Net100Version = "10.0.0-rc.2.25502.107";
    private const string Net90Version = "9.0.0";
    // private static readonly Lazy<ReferenceAssemblies> _lazyNet100 =
    //     new(() => new ReferenceAssemblies(
    //         "net10.0",
    //         new PackageIdentity(
    //             "Microsoft.NETCore.App.Ref",
    //             Net100Version),
    //         Path.Combine("ref", "net10.0")));
    //
    // private static readonly Lazy<ReferenceAssemblies> _lazyNet90 =
    //     new(() => new ReferenceAssemblies(
    //         "net9.0",
    //         new PackageIdentity(
    //             "Microsoft.NETCore.App.Ref",
    //             Net100Version),
    //         Path.Combine("ref", "net9.0")));
    
    // private static readonly Lazy<ReferenceAssemblies> _lazyAspNet100 = new(() => _lazyNet100.Value.AddPackages([new PackageIdentity("Microsoft.AspNetCore.App.Ref", Net100Version)]));
    private static readonly Lazy<ReferenceAssemblies> _lazyAspNet100 = new(() => ReferenceAssemblies.Net.Net100.AddPackage("Microsoft.AspNetCore.App.Ref"));
    private static readonly Lazy<ReferenceAssemblies> _lazyAspNet90 = new(() => ReferenceAssemblies.Net.Net90.AddPackage("Microsoft.AspNetCore.App.Ref"));
    
    public static ReferenceAssemblies AspNet100 => ReferenceAssemblies.Net.Net100.AddPackage("Microsoft.AspNetCore.App.Ref");
    public static ReferenceAssemblies AspNet90 => ReferenceAssemblies.Net.Net90.AddPackage("Microsoft.AspNetCore.App.Ref");
    public static ReferenceAssemblies Net90 => ReferenceAssemblies.Net.Net90;
    public static ReferenceAssemblies Net100 => ReferenceAssemblies.Net.Net100;

    public static ReferenceAssemblies AddPackage(this ReferenceAssemblies referenceAssemblies, string package)
    {
        return referenceAssemblies.AddPackage(package, referenceAssemblies.ReferenceAssemblyPackage!.Version);
    }
    public static ReferenceAssemblies AddPackage(this ReferenceAssemblies referenceAssemblies, string package, string version)
    {
        if(referenceAssemblies.ReferenceAssemblyPackage?.Version == null)
            throw new InvalidOperationException("ReferenceAssemblyPackage.Version is null");
        return referenceAssemblies.AddPackages([new PackageIdentity(package, version)]);
    }
}
//
// class Usage
// {
//     static void Test()
//     {
//         Assemblies.Net10
//     }
// }