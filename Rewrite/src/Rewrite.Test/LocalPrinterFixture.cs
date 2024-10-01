using Rewrite.Core;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

public class LocalPrinterFixture
{
    public LocalPrinterFixture()
    {
        ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
        IPrinterFactory.Set(new LocalPrinterFactory());
    }
}
