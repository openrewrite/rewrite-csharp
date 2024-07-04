using Rewrite.Core;

namespace Rewrite.RewriteJava.Tree;

public interface JavaSourceFile<out T> : J, MutableSourceFile<T> where T : MutableSourceFile<T>
{
    // TODO do we even need this in C#?
    // J.Package? PackageDeclaration { get; }
    // IList<J.Import> Imports { get; }
}