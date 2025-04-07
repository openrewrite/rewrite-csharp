using Rewrite.CSharp.Tests;

namespace Rewrite.Recipes.Tests;

public class GlobalHooks
{
    
    [Before(TestDiscovery)]
    public static void BeforeTestDiscovery()
    {
        TestSetup.Initialize();
    }
}