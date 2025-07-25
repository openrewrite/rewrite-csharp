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
    /// <summary>
    /// Represents a deconstruction pattern in Java.
    /// <br/>Example:
    /// <code>{@code
    /// case Point(int x, int y):
    ///     // use x and y
    /// }</code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class DeconstructionPattern(
    Guid id,
    Space prefix,
    Markers markers,
    Expression deconstructor,
    JContainer<J> nested,
    JavaType type
    ) : J,TypedTree    {
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
            return v.VisitDeconstructionPattern(this, p);
        }

        public Guid Id { get;  set; } = id;

        public DeconstructionPattern WithId(Guid newId)
        {
            return newId == Id ? this : new DeconstructionPattern(newId, Prefix, Markers, Deconstructor, _nested, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public DeconstructionPattern WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new DeconstructionPattern(Id, newPrefix, Markers, Deconstructor, _nested, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public DeconstructionPattern WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new DeconstructionPattern(Id, Prefix, newMarkers, Deconstructor, _nested, Type);
        }
        public Expression Deconstructor { get;  set; } = deconstructor;

        public DeconstructionPattern WithDeconstructor(Expression newDeconstructor)
        {
            return ReferenceEquals(newDeconstructor, Deconstructor) ? this : new DeconstructionPattern(Id, Prefix, Markers, newDeconstructor, _nested, Type);
        }
        private JContainer<J> _nested = nested;
        public IList<J> Nested => _nested.GetElements();

        public DeconstructionPattern WithNested(IList<J> newNested)
        {
            return Padding.WithNested(JContainer<J>.WithElements(_nested, newNested));
        }
        public JavaType Type { get;  set; } = type;

        public DeconstructionPattern WithType(JavaType newType)
        {
            return newType == Type ? this : new DeconstructionPattern(Id, Prefix, Markers, Deconstructor, _nested, newType);
        }
        public sealed record PaddingHelper(J.DeconstructionPattern T)
        {
            public JContainer<J> Nested { get => T._nested;  set => T._nested = value; }

            public J.DeconstructionPattern WithNested(JContainer<J> newNested)
            {
                return Nested == newNested ? T : new J.DeconstructionPattern(T.Id, T.Prefix, T.Markers, T.Deconstructor, newNested, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DeconstructionPattern && other.Id == Id;
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