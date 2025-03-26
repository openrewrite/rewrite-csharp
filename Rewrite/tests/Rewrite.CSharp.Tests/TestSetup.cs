namespace Rewrite.CSharp.Tests;

public static class TestSetup
{
    [Before(HookType.Assembly)]
    public static void BeforeAssembly()
    {
        ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
        var localPrinter = new LocalPrinterFactory();
        IPrinterFactory.Set(new LocalPrinterFactory());
        IPrinterFactory.Default = localPrinter;
        // Initialization.Initialize();
        // ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
        // var localPrinter = new LocalPrinterFactory();
        // IPrinterFactory.Set(localPrinter);
        // IPrinterFactory.Default = localPrinter;
        // Initialization.Initialize();
        // SenderContext.Register(typeof(Cs), () => new CSharpSender());
        // ReceiverContext.Register(typeof(Cs), () => new CSharpReceiver());
        // SenderContext.Register(typeof(Json), () => new JsonSender());
        // ReceiverContext.Register(typeof(Json), () => new JsonReceiver());
        // SenderContext.Register(typeof(Xml), () => new XmlSender());
        // ReceiverContext.Register(typeof(Xml), () => new XmlReceiver());
        // SenderContext.Register(typeof(Yaml), () => new YamlSender());
        // ReceiverContext.Register(typeof(Yaml), () => new YamlReceiver());
        // SenderContext.Register(typeof(Properties), () => new PropertiesSender());
        // ReceiverContext.Register(typeof(Properties), () => new PropertiesReceiver());
    }
}
