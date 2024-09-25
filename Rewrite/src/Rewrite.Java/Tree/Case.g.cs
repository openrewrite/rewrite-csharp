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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    public partial class Case(
    Guid id,
    Space prefix,
    Markers markers,
    Case.Type caseType,
    JContainer<Expression> expressions,
    JContainer<Statement> statements,
    JRightPadded<J>? body
    ) : J, Statement, MutableTree<Case>
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
            return v.VisitCase(this, p);
        }

        public Guid Id => id;

        public Case WithId(Guid newId)
        {
            return newId == id ? this : new Case(newId, prefix, markers, caseType, _expressions, _statements, _body);
        }
        public Space Prefix => prefix;

        public Case WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Case(id, newPrefix, markers, caseType, _expressions, _statements, _body);
        }
        public Markers Markers => markers;

        public Case WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Case(id, prefix, newMarkers, caseType, _expressions, _statements, _body);
        }
        public Type CaseType => caseType;

        public Case WithCaseType(Type newCaseType)
        {
            return newCaseType == caseType ? this : new Case(id, prefix, markers, newCaseType, _expressions, _statements, _body);
        }
        private readonly JContainer<Expression> _expressions = expressions;
        public IList<Expression> Expressions => _expressions.GetElements();

        public Case WithExpressions(IList<Expression> newExpressions)
        {
            return Padding.WithExpressions(JContainer<Expression>.WithElements(_expressions, newExpressions));
        }
        private readonly JContainer<Statement> _statements = statements;
        public IList<Statement> Statements => _statements.GetElements();

        public Case WithStatements(IList<Statement> newStatements)
        {
            return Padding.WithStatements(JContainer<Statement>.WithElements(_statements, newStatements));
        }
        private readonly JRightPadded<J>? _body = body;
        public J? Body => _body?.Element;

        public Case WithBody(J? newBody)
        {
            return Padding.WithBody(JRightPadded<J>.WithElement(_body, newBody));
        }
        public enum Type
        {
            Statement,
            Rule,
        }
        public sealed record PaddingHelper(J.Case T)
        {
            public JContainer<Expression> Expressions => T._expressions;

            public J.Case WithExpressions(JContainer<Expression> newExpressions)
            {
                return T._expressions == newExpressions ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, newExpressions, T._statements, T._body);
            }

            public JContainer<Statement> Statements => T._statements;

            public J.Case WithStatements(JContainer<Statement> newStatements)
            {
                return T._statements == newStatements ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, T._expressions, newStatements, T._body);
            }

            public JRightPadded<J>? Body => T._body;

            public J.Case WithBody(JRightPadded<J>? newBody)
            {
                return T._body == newBody ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, T._expressions, T._statements, newBody);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Case && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}