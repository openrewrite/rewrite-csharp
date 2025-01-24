using Rewrite.Core;

namespace Rewrite.RewriteJava.Tree;

public interface JavaSourceFile<out T> : JavaSourceFile, MutableSourceFile<T> where T : MutableSourceFile<T>
{

}

public interface JavaSourceFile : J, MutableSourceFile
{
    // TODO do we even need this in C#?
    // J.Package? PackageDeclaration { get; }
    // IList<J.Import> Imports { get; }
}
