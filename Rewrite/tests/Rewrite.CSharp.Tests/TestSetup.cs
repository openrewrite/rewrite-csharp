//
//
// using Rewrite.Test.Engine.Remote;
// using Xunit.Abstractions;
// using Xunit.Sdk;
//
// [assembly: Xunit.TestFramework("Rewrite.CSharp.Tests.TestSetup", "Rewrite.CSharp.Tests")]
//
// namespace Rewrite.CSharp.Tests;
//
// public sealed class TestSetup : XunitTestFramework, IDisposable
// {
// #if REMOTE_PRINTER
//     private readonly IDisposable _fixture = new RemotingFixture();
// #else
//     private readonly IDisposable _fixture = new LocalPrinterFixture();
// #endif
//     public TestSetup(IMessageSink messageSink)
//         :base(messageSink)
//     {
//     }
//
//     public new void Dispose()
//     {
//         _fixture.Dispose();
//     }
// }
