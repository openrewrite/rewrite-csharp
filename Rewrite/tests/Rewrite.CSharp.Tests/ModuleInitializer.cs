using System.Runtime.CompilerServices;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

public class ModuleInitializer
{
    [ModuleInitializer]
    internal static void OnAssemblyLoad()
    {
        if (ITestExecutionContext.Current() == null)
        {
            ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
            IPrinterFactory.Set(new LocalPrinterFactory());
        }
    }
}
