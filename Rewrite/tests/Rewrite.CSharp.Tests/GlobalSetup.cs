﻿using Rewrite.Tests;

namespace Rewrite.CSharp.Tests;

public class GlobalSetup
{
    
    [Before(TestDiscovery)]
    public static void Initialize()
    {
        CommonTestHooks.BeforeTestSession();
    }
}