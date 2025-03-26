using System.Diagnostics.CodeAnalysis;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Cs : J
{
    partial class AliasQualifiedName
    {
        public bool Equals(Core.Marker.Marker? other)
        {
            return other is AliasQualifiedName otherAlias && otherAlias.Alias.Equals(this.Alias) && otherAlias.Name.Equals(this.Name);
        }
    }
}
