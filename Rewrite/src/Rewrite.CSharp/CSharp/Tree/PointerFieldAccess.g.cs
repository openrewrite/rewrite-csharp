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
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class PointerFieldAccess(
    Guid id,
    Space prefix,
    Markers markers,
    Expression target,
    JLeftPadded<J.Identifier> name,
    JavaType? type
    ) : Cs,TypeTree,Expression,Statement    {
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
            return v.VisitPointerFieldAccess(this, p);
        }

        public Guid Id { get;  set; } = id;

        public PointerFieldAccess WithId(Guid newId)
        {
            return newId == Id ? this : new PointerFieldAccess(newId, Prefix, Markers, Target, _name, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public PointerFieldAccess WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new PointerFieldAccess(Id, newPrefix, Markers, Target, _name, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public PointerFieldAccess WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new PointerFieldAccess(Id, Prefix, newMarkers, Target, _name, Type);
        }
        public Expression Target { get;  set; } = target;

        public PointerFieldAccess WithTarget(Expression newTarget)
        {
            return ReferenceEquals(newTarget, Target) ? this : new PointerFieldAccess(Id, Prefix, Markers, newTarget, _name, Type);
        }
        private JLeftPadded<J.Identifier> _name = name;
        public J.Identifier Name => _name.Element;

        public PointerFieldAccess WithName(J.Identifier newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }
        public JavaType? Type { get;  set; } = type;

        public PointerFieldAccess WithType(JavaType? newType)
        {
            return newType == Type ? this : new PointerFieldAccess(Id, Prefix, Markers, Target, _name, newType);
        }
        public sealed record PaddingHelper(Cs.PointerFieldAccess T)
        {
            public JLeftPadded<J.Identifier> Name { get => T._name;  set => T._name = value; }

            public Cs.PointerFieldAccess WithName(JLeftPadded<J.Identifier> newName)
            {
                return Name == newName ? T : new Cs.PointerFieldAccess(T.Id, T.Prefix, T.Markers, T.Target, newName, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is PointerFieldAccess && other.Id == Id;
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