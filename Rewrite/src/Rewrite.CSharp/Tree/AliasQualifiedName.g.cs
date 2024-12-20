//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108 // 'member1' hides inherited member 'member2'. Use the new keyword if hiding was intended.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;
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
    /// <summary>
    /// Represents a C# alias qualified name, which uses an extern alias to qualify a name.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Using LibA to qualify TypeName
    ///     LibA::TypeName
    ///     // Using LibB to qualify namespace
    ///     LibB::System.Collections
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class AliasQualifiedName(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<J.Identifier> alias,
    Expression name
    ) : Cs, TypeTree, Expression, Rewrite.Core.Marker.Marker, Expression<AliasQualifiedName>, TypedTree<AliasQualifiedName>, J<AliasQualifiedName>, TypeTree<AliasQualifiedName>, MutableTree<AliasQualifiedName>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitAliasQualifiedName(this, p);
        }

        public Guid Id => id;

        public AliasQualifiedName WithId(Guid newId)
        {
            return newId == id ? this : new AliasQualifiedName(newId, prefix, markers, _alias, name);
        }
        public Space Prefix => prefix;

        public AliasQualifiedName WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AliasQualifiedName(id, newPrefix, markers, _alias, name);
        }
        public Markers Markers => markers;

        public AliasQualifiedName WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AliasQualifiedName(id, prefix, newMarkers, _alias, name);
        }
        private readonly JRightPadded<J.Identifier> _alias = alias;
        public J.Identifier Alias => _alias.Element;

        public AliasQualifiedName WithAlias(J.Identifier newAlias)
        {
            return Padding.WithAlias(_alias.WithElement(newAlias));
        }
        public Expression Name => name;

        public AliasQualifiedName WithName(Expression newName)
        {
            return ReferenceEquals(newName, name) ? this : new AliasQualifiedName(id, prefix, markers, _alias, newName);
        }
        public sealed record PaddingHelper(Cs.AliasQualifiedName T)
        {
            public JRightPadded<J.Identifier> Alias => T._alias;

            public Cs.AliasQualifiedName WithAlias(JRightPadded<J.Identifier> newAlias)
            {
                return T._alias == newAlias ? T : new Cs.AliasQualifiedName(T.Id, T.Prefix, T.Markers, newAlias, T.Name);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AliasQualifiedName && other.Id == Id;
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}