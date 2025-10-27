using Microsoft.CodeAnalysis.Testing;

namespace Rewrite.RoslynRecipe.Tests;

public static class KnownPackages
{
    public static class Net9
    {
        public const string PackageVersion = "9.0.0"; 
        public static PackageIdentity Microsoft_AspNetCore_OpenApi = new PackageIdentity("Microsoft.AspNetCore.OpenApi", PackageVersion);
    }
    public static class Net10
    {
        public const string PackageVersion = "10.0.0-rc.2.25502.107"; 
        public static PackageIdentity Microsoft_AspNetCore_OpenApi = new PackageIdentity("Microsoft.AspNetCore.OpenApi", PackageVersion);
        
        
    }

    
}