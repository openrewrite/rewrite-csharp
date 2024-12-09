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
    public partial class Argument(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<J.Identifier>? nameColumn,
    Keyword? refKindKeyword,
    Expression expression
    ) : Cs, Expression, Expression<Argument>, J<Argument>, MutableTree<Argument>
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
            return v.VisitArgument(this, p);
        }

        public Guid Id => id;

        public Argument WithId(Guid newId)
        {
            return newId == id ? this : new Argument(newId, prefix, markers, _nameColumn, refKindKeyword, expression);
        }
        public Space Prefix => prefix;

        public Argument WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Argument(id, newPrefix, markers, _nameColumn, refKindKeyword, expression);
        }
        public Markers Markers => markers;

        public Argument WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Argument(id, prefix, newMarkers, _nameColumn, refKindKeyword, expression);
        }
        private readonly JRightPadded<J.Identifier>? _nameColumn = nameColumn;
        public J.Identifier? NameColumn => _nameColumn?.Element;

        public Argument WithNameColumn(J.Identifier? newNameColumn)
        {
            return Padding.WithNameColumn(JRightPadded<J.Identifier>.WithElement(_nameColumn, newNameColumn));
        }
        public Cs.Keyword? RefKindKeyword => refKindKeyword;

        public Argument WithRefKindKeyword(Cs.Keyword? newRefKindKeyword)
        {
            return ReferenceEquals(newRefKindKeyword, refKindKeyword) ? this : new Argument(id, prefix, markers, _nameColumn, newRefKindKeyword, expression);
        }
        public Expression Expression => expression;

        public Argument WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Argument(id, prefix, markers, _nameColumn, refKindKeyword, newExpression);
        }
        public sealed record PaddingHelper(Cs.Argument T)
        {
            public JRightPadded<J.Identifier>? NameColumn => T._nameColumn;

            public Cs.Argument WithNameColumn(JRightPadded<J.Identifier>? newNameColumn)
            {
                return T._nameColumn == newNameColumn ? T : new Cs.Argument(T.Id, T.Prefix, T.Markers, newNameColumn, T.RefKindKeyword, T.Expression);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Argument && other.Id == Id;
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