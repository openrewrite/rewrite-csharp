namespace Rewrite.Core;

public interface IPrinterFactory
{
    internal static IPrinterFactory? PRINTER_FACTORY_THREAD_LOCAL;

    public static IPrinterFactory? Current() => PRINTER_FACTORY_THREAD_LOCAL;
    public static IPrinterFactory? Default { get; set; }

    public static void Set(IPrinterFactory printerFactory) => PRINTER_FACTORY_THREAD_LOCAL = printerFactory;

    public TreeVisitor<Tree, PrintOutputCapture<TP>> CreatePrinter<TP>();
}
