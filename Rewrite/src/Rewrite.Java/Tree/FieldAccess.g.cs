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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class FieldAccess(
    Guid id,
    Space prefix,
    Markers markers,
    Expression target,
    JLeftPadded<Identifier> name,
    JavaType? type
    ) : J, TypeTree, Expression, Statement, Expression<FieldAccess>, TypedTree<FieldAccess>, J<FieldAccess>, TypeTree<FieldAccess>, MutableTree<FieldAccess>
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

        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitFieldAccess(this, p);
        }

        public Guid Id => id;

        public FieldAccess WithId(Guid newId)
        {
            return newId == id ? this : new FieldAccess(newId, prefix, markers, target, _name, type);
        }
        public Space Prefix => prefix;

        public FieldAccess WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new FieldAccess(id, newPrefix, markers, target, _name, type);
        }
        public Markers Markers => markers;

        public FieldAccess WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new FieldAccess(id, prefix, newMarkers, target, _name, type);
        }
        public Expression Target => target;

        public FieldAccess WithTarget(Expression newTarget)
        {
            return ReferenceEquals(newTarget, target) ? this : new FieldAccess(id, prefix, markers, newTarget, _name, type);
        }
        private readonly JLeftPadded<J.Identifier> _name = name;
        public J.Identifier Name => _name.Element;

        public FieldAccess WithName(J.Identifier newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }
        public JavaType? Type => type;

        public FieldAccess WithType(JavaType? newType)
        {
            return newType == type ? this : new FieldAccess(id, prefix, markers, target, _name, newType);
        }
        public sealed record PaddingHelper(J.FieldAccess T)
        {
            public JLeftPadded<J.Identifier> Name => T._name;

            public J.FieldAccess WithName(JLeftPadded<J.Identifier> newName)
            {
                return T._name == newName ? T : new J.FieldAccess(T.Id, T.Prefix, T.Markers, T.Target, newName, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is FieldAccess && other.Id == Id;
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