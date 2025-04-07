using Rewrite.CSharp.Tests;

namespace Rewrite.Recipes.Tests;

public class GlobalSetup
{
    
    [Before(TestDiscovery)]
    public static void Initialize()
    {
        CommonTestHooks.BeforeTestSession();
    }
}