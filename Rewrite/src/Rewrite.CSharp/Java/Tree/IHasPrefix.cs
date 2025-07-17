using Rewrite.RewriteJava.Tree;

namespace Rewrite.Core;
public partial interface IHasPrefix
{
    Space Prefix { get; }
    IHasPrefix WithPrefix(Space prefix);
}