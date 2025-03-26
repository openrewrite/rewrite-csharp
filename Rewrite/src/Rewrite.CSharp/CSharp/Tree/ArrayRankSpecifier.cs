using System.Diagnostics;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public partial interface Cs : J
{

    partial class ArrayRankSpecifier
    {
        public JavaType? Type => Sizes.Count == 0 ? null : Sizes[0].Type;

        public ArrayRankSpecifier WithType(JavaType? type)
        {
            throw new NotImplementedException();
        }
    }

}
