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
    /// Represents a C# pointer type declaration.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Basic pointer declaration
    ///     int* ptr;
    ///        ^
    ///     // Pointer to pointer
    ///     int** ptr;
    ///         ^
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class PointerType(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<TypeTree> elementType
    ) : Cs,TypeTree,Expression    {
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
            return v.VisitPointerType(this, p);
        }

        public Guid Id { get;  set; } = id;

        public PointerType WithId(Guid newId)
        {
            return newId == Id ? this : new PointerType(newId, Prefix, Markers, _elementType);
        }
        public Space Prefix { get;  set; } = prefix;

        public PointerType WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new PointerType(Id, newPrefix, Markers, _elementType);
        }
        public Markers Markers { get;  set; } = markers;

        public PointerType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new PointerType(Id, Prefix, newMarkers, _elementType);
        }
        private JRightPadded<TypeTree> _elementType = elementType;
        public TypeTree ElementType => _elementType.Element;

        public PointerType WithElementType(TypeTree newElementType)
        {
            return Padding.WithElementType(_elementType.WithElement(newElementType));
        }
        public sealed record PaddingHelper(Cs.PointerType T)
        {
            public JRightPadded<TypeTree> ElementType { get => T._elementType;  set => T._elementType = value; }

            public Cs.PointerType WithElementType(JRightPadded<TypeTree> newElementType)
            {
                return ElementType == newElementType ? T : new Cs.PointerType(T.Id, T.Prefix, T.Markers, newElementType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is PointerType && other.Id == Id;
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