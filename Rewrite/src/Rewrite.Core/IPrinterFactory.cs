namespace Rewrite.Core;

public interface IPrinterFactory
{
    internal static readonly ThreadLocal<IPrinterFactory> PRINTER_FACTORY_THREAD_LOCAL = new();
    
    public static IPrinterFactory? Current() => PRINTER_FACTORY_THREAD_LOCAL.Value;

    public static void Set(IPrinterFactory printerFactory) => PRINTER_FACTORY_THREAD_LOCAL.Value = printerFactory;

    public TreeVisitor<Tree, PrintOutputCapture<TP>> CreatePrinter<TP>();
}