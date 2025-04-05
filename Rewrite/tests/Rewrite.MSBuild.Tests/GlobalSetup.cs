// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly

using Rewrite.CSharp.Tests;

[assembly: Retry(3)]
[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Rewrite.MSBuild.Tests;

public class GlobalHooks
{
    [Before(Assembly)]
    public static void AssemblySetUp()
    {
        TestSetup.BeforeAssembly();
    }
    [Before(TestSession)]
    public static void SetUp()
    {
    }

    [After(TestSession)]
    public static void CleanUp()
    {
    }
}