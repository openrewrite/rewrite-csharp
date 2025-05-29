using Rewrite.CSharp.Tests;
using Rewrite.Tests;

namespace Rewrite.Recipes.Tests;

public class GlobalSetup
{
    
    [Before(TestDiscovery)]
    public static void Initialize()
    {
        CommonTestHooks.BeforeTestSession();
    }
}