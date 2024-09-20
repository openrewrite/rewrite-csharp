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
    public partial class StatementExpression(
    Guid id,
    Space prefix,
    Markers markers,
    Statement statement
    ) : Cs, Expression, MutableTree<StatementExpression>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitStatementExpression(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public StatementExpression WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public StatementExpression WithId(Guid newId)
        {
            return newId == id ? this : new StatementExpression(newId, prefix, markers, statement);
        }
        public Space Prefix => prefix;

        public StatementExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new StatementExpression(id, newPrefix, markers, statement);
        }
        public Markers Markers => markers;

        public StatementExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new StatementExpression(id, prefix, newMarkers, statement);
        }
        public Statement Statement => statement;

        public StatementExpression WithStatement(Statement newStatement)
        {
            return ReferenceEquals(newStatement, statement) ? this : new StatementExpression(id, prefix, markers, newStatement);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is StatementExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}