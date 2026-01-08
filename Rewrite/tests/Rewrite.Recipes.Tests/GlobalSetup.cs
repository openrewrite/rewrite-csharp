using Rewrite.CSharp.Tests;
using TUnit.Core;

namespace Rewrite.Recipes.Tests;

public class GlobalSetup
{
    
    [Before(HookType.TestDiscovery)]
    public static void Initialize()
    {
        CommonTestHooks.BeforeTestSession();
    }
}