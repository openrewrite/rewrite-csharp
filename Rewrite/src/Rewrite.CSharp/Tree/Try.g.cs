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
    public partial class Try(
    Guid id,
    Space prefix,
    Markers markers,
    J.Block body,
    IList<Try.Catch> catches,
    JLeftPadded<J.Block>? @finally
    ) : Cs, Statement, J<Try>, MutableTree<Try>
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
            return v.VisitTry(this, p);
        }

        public Guid Id => id;

        public Try WithId(Guid newId)
        {
            return newId == id ? this : new Try(newId, prefix, markers, body, catches, _finally);
        }
        public Space Prefix => prefix;

        public Try WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Try(id, newPrefix, markers, body, catches, _finally);
        }
        public Markers Markers => markers;

        public Try WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Try(id, prefix, newMarkers, body, catches, _finally);
        }
        public J.Block Body => body;

        public Try WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new Try(id, prefix, markers, newBody, catches, _finally);
        }
        public IList<Catch> Catches => catches;

        public Try WithCatches(IList<Catch> newCatches)
        {
            return newCatches == catches ? this : new Try(id, prefix, markers, body, newCatches, _finally);
        }
        private readonly JLeftPadded<J.Block>? _finally = @finally;
        public J.Block? Finally => _finally?.Element;

        public Try WithFinally(J.Block? newFinally)
        {
            return Padding.WithFinally(JLeftPadded<J.Block>.WithElement(_finally, newFinally));
        }
        /// <summary>
        /// Represents a C# catch clause in a try/catch statement, which optionally includes a filter expression.
        /// <br/>
        /// For example:
        /// <code>
        ///     // Simple catch clause
        ///     catch (Exception e) { }
        ///     // Catch with filter expression
        ///     catch (Exception e) when (e.Code == 404) { }
        ///     // Multiple catch clauses with filters
        ///     try {
        ///         // code
        ///     }
        ///     catch (ArgumentException e) when (e.ParamName == "id") { }
        ///     catch (Exception e) when (e.InnerException != null) { }
        /// </code>
        /// </summary>
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Catch(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<J.VariableDeclarations> parameter,
    JLeftPadded<J.ControlParentheses<Expression>>? filterExpression,
    J.Block body
        ) : Cs, J<Catch>, MutableTree<Catch>
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
                return v.VisitTryCatch(this, p);
            }

            public Guid Id => id;

            public Catch WithId(Guid newId)
            {
                return newId == id ? this : new Catch(newId, prefix, markers, parameter, _filterExpression, body);
            }
            public Space Prefix => prefix;

            public Catch WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Catch(id, newPrefix, markers, parameter, _filterExpression, body);
            }
            public Markers Markers => markers;

            public Catch WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Catch(id, prefix, newMarkers, parameter, _filterExpression, body);
            }
            public J.ControlParentheses<J.VariableDeclarations> Parameter => parameter;

            public Catch WithParameter(J.ControlParentheses<J.VariableDeclarations> newParameter)
            {
                return ReferenceEquals(newParameter, parameter) ? this : new Catch(id, prefix, markers, newParameter, _filterExpression, body);
            }
            private readonly JLeftPadded<J.ControlParentheses<Expression>>? _filterExpression = filterExpression;
            public J.ControlParentheses<Expression>? FilterExpression => _filterExpression?.Element;

            public Catch WithFilterExpression(J.ControlParentheses<Expression>? newFilterExpression)
            {
                return Padding.WithFilterExpression(JLeftPadded<J.ControlParentheses<Expression>>.WithElement(_filterExpression, newFilterExpression));
            }
            public J.Block Body => body;

            public Catch WithBody(J.Block newBody)
            {
                return ReferenceEquals(newBody, body) ? this : new Catch(id, prefix, markers, parameter, _filterExpression, newBody);
            }
            public sealed record PaddingHelper(Cs.Try.Catch T)
            {
                public JLeftPadded<J.ControlParentheses<Expression>>? FilterExpression => T._filterExpression;

                public Cs.Try.Catch WithFilterExpression(JLeftPadded<J.ControlParentheses<Expression>>? newFilterExpression)
                {
                    return T._filterExpression == newFilterExpression ? T : new Cs.Try.Catch(T.Id, T.Prefix, T.Markers, T.Parameter, newFilterExpression, T.Body);
                }

            }

            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Catch && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public sealed record PaddingHelper(Cs.Try T)
        {
            public JLeftPadded<J.Block>? Finally => T._finally;

            public Cs.Try WithFinally(JLeftPadded<J.Block>? newFinally)
            {
                return T._finally == newFinally ? T : new Cs.Try(T.Id, T.Prefix, T.Markers, T.Body, T.Catches, newFinally);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Try && other.Id == Id;
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