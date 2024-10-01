using Rewrite.Core;
using Rewrite.RewriteCSharp;

namespace Rewrite.CSharp.Tests;

public class LocalPrinterFactory : IPrinterFactory
{
    public TreeVisitor<Core.Tree, PrintOutputCapture<TP>> CreatePrinter<TP>()
    {
        return new LocalPrinter<TP>();
    }

    private class LocalPrinter<T> : TreeVisitor<Rewrite.Core.Tree, PrintOutputCapture<T>>
    {
        readonly CSharpPrinter<T> _printer = new ();
        public override Core.Tree? Visit(Core.Tree? tree, PrintOutputCapture<T> p)
        {
            return _printer.Visit(tree, p);
        }
    }
}
