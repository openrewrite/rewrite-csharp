using Rewrite.Core;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

public class LocalPrinterFixture : IDisposable
{
    public LocalPrinterFixture()
    {
        ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
        IPrinterFactory.Set(new LocalPrinterFactory());
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
