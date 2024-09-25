//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
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
    public partial class NamedArgument(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<J.Identifier>? nameColumn,
    Expression expression
    ) : Cs, Expression, MutableTree<NamedArgument>
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
            return v.VisitNamedArgument(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public NamedArgument WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public NamedArgument WithId(Guid newId)
        {
            return newId == id ? this : new NamedArgument(newId, prefix, markers, _nameColumn, expression);
        }
        public Space Prefix => prefix;

        public NamedArgument WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NamedArgument(id, newPrefix, markers, _nameColumn, expression);
        }
        public Markers Markers => markers;

        public NamedArgument WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NamedArgument(id, prefix, newMarkers, _nameColumn, expression);
        }
        private readonly JRightPadded<J.Identifier>? _nameColumn = nameColumn;
        public J.Identifier? NameColumn => _nameColumn?.Element;

        public NamedArgument WithNameColumn(J.Identifier? newNameColumn)
        {
            return Padding.WithNameColumn(JRightPadded<J.Identifier>.WithElement(_nameColumn, newNameColumn));
        }
        public Expression Expression => expression;

        public NamedArgument WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new NamedArgument(id, prefix, markers, _nameColumn, newExpression);
        }
        public sealed record PaddingHelper(Cs.NamedArgument T)
        {
            public JRightPadded<J.Identifier>? NameColumn => T._nameColumn;

            public Cs.NamedArgument WithNameColumn(JRightPadded<J.Identifier>? newNameColumn)
            {
                return T._nameColumn == newNameColumn ? T : new Cs.NamedArgument(T.Id, T.Prefix, T.Markers, newNameColumn, T.Expression);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NamedArgument && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}