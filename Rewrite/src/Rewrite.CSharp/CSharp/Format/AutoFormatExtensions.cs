using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Format;

public static class AutoFormatExtensions
{
    
    public static T Format<T>(this T lst, Cursor cursor) where T : class, J
    {
        AutoFormatVisitor<object> formatVisitor = lst is Cs.CompilationUnit ? new() : new(lst);
        if (cursor.Value != lst)
        {
            cursor = new Cursor(cursor.Parent, lst);
        }
        return (T)formatVisitor.Visit(lst, new object(), cursor)!;
    }
}