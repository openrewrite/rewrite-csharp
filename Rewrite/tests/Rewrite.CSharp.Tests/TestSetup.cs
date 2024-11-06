

using System.Reflection;
using Rewrite.Test.Engine.Remote;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("Rewrite.CSharp.Tests.TestSetup", "Rewrite.CSharp.Tests")]

namespace Rewrite.CSharp.Tests;

public sealed class TestSetup : XunitTestFramework, IDisposable
{
    private IDisposable _fixture = null!;
    public TestSetup(IMessageSink messageSink)
        :base(messageSink)
    {
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
    {
#if REMOTE_PRINTER
        _fixture = new RemotingFixture();
#else
        _fixture = new LocalPrinterFixture();
#endif

        return base.CreateExecutor(assemblyName);
    }

    public new void Dispose()
    {
        _fixture.Dispose();
    }
}
