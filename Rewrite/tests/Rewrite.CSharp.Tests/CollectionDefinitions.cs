using Rewrite.Test.CSharp;
using Rewrite.Test.Engine.Remote;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

[CollectionDefinition("LocalPrinter")]
public class LocalPrinterCollection : ICollectionFixture<LocalPrinterFixture>;

[CollectionDefinition("RemotePrinter")]
public class RemotePrinterCollection : ICollectionFixture<RemotingFixture>;

public static class Collections
{
    #if REMOTE_PRINTER
    public const string PrinterAccess = "RemotePrinter";
    #else
    public const string PrinterAccess = "LocalPrinter";
    #endif
}
