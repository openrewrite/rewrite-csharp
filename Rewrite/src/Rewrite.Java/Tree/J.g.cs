//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    Space Prefix { get; }

    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(JavaVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptJava(v.Adapt<J, JavaVisitor<P>>(), p);
    }

    J? AcceptJava<P>(JavaVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

}