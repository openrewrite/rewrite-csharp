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
    /// Represents a tuple expression in C#.
    /// Can be used in tuple construction, deconstruction and tuple literals.
    /// Examples:
    /// <code>
    /// // Tuple construction
    /// var point = (1, 2);
    /// // Named tuple elements
    /// var person = (name: "John", age: 25);
    /// // Nested tuples
    /// var nested = (1, (2, 3));
    /// // Tuple type with multiple elements
    /// (string name, int age) person = ("John", 25);
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class TupleExpression(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Argument> arguments
    ) : Cs, Expression, Expression<TupleExpression>, J<TupleExpression>, MutableTree<TupleExpression>
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
            return v.VisitTupleExpression(this, p);
        }

        public Guid Id => id;

        public TupleExpression WithId(Guid newId)
        {
            return newId == id ? this : new TupleExpression(newId, prefix, markers, _arguments);
        }
        public Space Prefix => prefix;

        public TupleExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TupleExpression(id, newPrefix, markers, _arguments);
        }
        public Markers Markers => markers;

        public TupleExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TupleExpression(id, prefix, newMarkers, _arguments);
        }
        private readonly JContainer<Cs.Argument> _arguments = arguments;
        public IList<Cs.Argument> Arguments => _arguments.GetElements();

        public TupleExpression WithArguments(IList<Cs.Argument> newArguments)
        {
            return Padding.WithArguments(JContainer<Cs.Argument>.WithElements(_arguments, newArguments));
        }
        public sealed record PaddingHelper(Cs.TupleExpression T)
        {
            public JContainer<Cs.Argument> Arguments => T._arguments;

            public Cs.TupleExpression WithArguments(JContainer<Cs.Argument> newArguments)
            {
                return T._arguments == newArguments ? T : new Cs.TupleExpression(T.Id, T.Prefix, T.Markers, newArguments);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TupleExpression && other.Id == Id;
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