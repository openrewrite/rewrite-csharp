/*
 * Copyright 2024 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;

// using Rewrite.RewriteJava.Tree;
using Tree = Rewrite.Core.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpPrinter<TState> : CSharpPrinter<PrintOutputCapture<TState>, TState>
{
}

public class CSharpPrinter<TOutputCapture, TState> : CSharpVisitor<TOutputCapture> where TOutputCapture : PrintOutputCapture<TState>
{
    private readonly CSharpJavaPrinter _delegate;
    private readonly JavaPrinter<TOutputCapture, TState> _javaPrinter = new();

    public CSharpPrinter()
    {
        _delegate = new CSharpJavaPrinter(this);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public override J? Visit(Rewrite.Core.Tree? node, TOutputCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
    {
        if (!(node is Cs))
        {
            // Re-route printing to the Java printer
            return _delegate.Visit(node, p);
        }
        else
        {
            return base.Visit(node, p);
        }
    }

    public override J? VisitOperatorDeclaration(Cs.OperatorDeclaration node, TOutputCapture p)
    {
        var @operator = node.OperatorToken switch
        {
            Cs.OperatorDeclaration.Operator.Plus => "+",
            Cs.OperatorDeclaration.Operator.Minus => "-",
            Cs.OperatorDeclaration.Operator.Bang => "!",
            Cs.OperatorDeclaration.Operator.Tilde => "~",
            Cs.OperatorDeclaration.Operator.PlusPlus => "++",
            Cs.OperatorDeclaration.Operator.MinusMinus => "--",
            Cs.OperatorDeclaration.Operator.Star => "*",
            Cs.OperatorDeclaration.Operator.Division => "/",
            Cs.OperatorDeclaration.Operator.Percent => "%",
            Cs.OperatorDeclaration.Operator.LeftShift => "<<",
            Cs.OperatorDeclaration.Operator.RightShift => ">>",
            Cs.OperatorDeclaration.Operator.LessThan => "<",
            Cs.OperatorDeclaration.Operator.GreaterThan => ">",
            Cs.OperatorDeclaration.Operator.LessThanEquals => "<=",
            Cs.OperatorDeclaration.Operator.GreaterThanEquals => ">=",
            Cs.OperatorDeclaration.Operator.Equals => "==",
            Cs.OperatorDeclaration.Operator.NotEquals => "!=",
            Cs.OperatorDeclaration.Operator.Ampersand => "&",
            Cs.OperatorDeclaration.Operator.Bar => "|",
            Cs.OperatorDeclaration.Operator.Caret => "^",
            Cs.OperatorDeclaration.Operator.True => "true",
            Cs.OperatorDeclaration.Operator.False => "false",
            _ => throw new InvalidOperationException("OperatorToken does not have a valid value")
        };
        BeforeSyntax(node, Space.Location.POINTER_TYPE_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Modifiers, p);
        VisitRightPadded(node.Padding.ExplicitInterfaceSpecifier, JRightPadded.Location.OPERATOR_DECLARATION_EXPLICIT_INTERFACE_SPECIFIER, ".", p);
        Visit(node.ReturnType, p);
        Visit(node.OperatorKeyword, p);
        Visit(node.CheckedKeyword, p);
        VisitSpace(node.Padding.OperatorToken.Before, Space.Location.OPERATOR_DECLARATION_OPERATOR_TOKEN,  p);
        p.Append(@operator);
        VisitContainer("(", node.Padding.Parameters, JContainer.Location.OPERATOR_DECLARATION_PARAMETERS, ",", ")", p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPointerType(Cs.PointerType node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.POINTER_TYPE_PREFIX, p);
        VisitRightPadded(node.Padding.ElementType, JRightPadded.Location.POINTER_TYPE_ELEMENT_TYPE, "*", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTry(Cs.Try node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TRY_PREFIX, p);
        p.Append("try");
        Visit(node.Body, p);
        Visit(node.Catches, p);
        VisitLeftPadded("finally", node.Padding.Finally, JLeftPadded.Location.TRY_FINALLIE, p);
        AfterSyntax(node, p);
        return node;
    }



    public override J? VisitTryCatch(Cs.Try.Catch node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CATCH_PREFIX, p);
        p.Append("catch");
        if (node.Parameter.Tree.TypeExpression != null)
        {
            Visit(node.Parameter, p);
        }

        VisitLeftPadded("when", node.Padding.FilterExpression, JLeftPadded.Location.TRY_CATCH_FILTER_EXPRESSION, p);

        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArrayType(Cs.ArrayType node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ARRAY_TYPE_PREFIX, p);
        Visit(node.TypeExpression, p);
        Visit(node.Dimensions, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAliasQualifiedName(Cs.AliasQualifiedName node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ALIAS_QUALIFIED_NAME_PREFIX, p);
        VisitRightPadded(node.Padding.Alias, JRightPadded.Location.ALIAS_QUALIFIED_NAME_ALIAS, p);
        p.Append("::");
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypeParameter(Cs.TypeParameter node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TYPE_PARAMETER_PREFIX, p);
        Visit(node.AttributeLists, p);
        VisitLeftPaddedEnum(node.Padding.Variance, JLeftPadded.Location.TYPE_PARAMETER_VARIANCE, p);
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryExpression(Cs.QueryExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.QUERY_EXPRESSION_PREFIX, p);
        Visit(node.FromClause, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryContinuation(Cs.QueryContinuation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.QUERY_CONTINUATION_PREFIX, p);
        p.Append("into");
        Visit(node.Identifier, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFromClause(Cs.FromClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.FROM_CLAUSE_PREFIX, p);
        p.Append("from");
        Visit(node.TypeIdentifier, p);
        VisitRightPadded(node.Padding.Identifier, JRightPadded.Location.FROM_CLAUSE_IDENTIFIER, p);
        p.Append("in");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryBody(Cs.QueryBody node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.QUERY_BODY_PREFIX, p);
        foreach (var clause in node.Clauses)
        {
            Visit(clause, p);
        }
        Visit(node.SelectOrGroup, p);
        Visit(node.Continuation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLetClause(Cs.LetClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.LET_CLAUSE_PREFIX, p);
        p.Append("let");
        VisitRightPadded(node.Padding.Identifier, JRightPadded.Location.LET_CLAUSE_IDENTIFIER, p);
        p.Append("=");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitJoinClause(Cs.JoinClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.JOIN_CLAUSE_PREFIX, p);
        p.Append("join");
        VisitRightPadded(node.Padding.Identifier, JRightPadded.Location.JOIN_CLAUSE_IDENTIFIER, p);
        p.Append("in");
        VisitRightPadded(node.Padding.InExpression, JRightPadded.Location.JOIN_CLAUSE_IN_EXPRESSION, p);
        p.Append("on");
        VisitRightPadded(node.Padding.LeftExpression, JRightPadded.Location.JOIN_CLAUSE_LEFT_EXPRESSION, p);
        p.Append("equals");
        Visit(node.RightExpression, p);
        Visit(node.Into, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitWhereClause(Cs.WhereClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.WHERE_CLAUSE_PREFIX, p);
        p.Append("where");
        Visit(node.Condition, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitJoinIntoClause(Cs.JoinIntoClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.JOIN_INTO_CLAUSE_PREFIX, p);
        p.Append("into");
        Visit(node.Identifier, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitOrderByClause(Cs.OrderByClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.JOIN_INTO_CLAUSE_PREFIX, p);
        p.Append("orderby");
        VisitRightPadded(node.Padding.Orderings, JRightPadded.Location.ORDER_BY_CLAUSE_ORDERINGS, ",", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitForEachVariableLoop(Cs.ForEachVariableLoop node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.FOR_EACH_LOOP_PREFIX, p);
        p.Append("foreach");
        var ctrl = node.ControlElement;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_VARIABLE, "in", p);
        VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_ITERABLE, "", p);
        p.Append(')');
        VisitStatement(node.Padding.Body, JRightPadded.Location.FOR_EACH_VARIABLE_LOOP_BODY, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitGroupClause(Cs.GroupClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.GROUP_CLAUSE_PREFIX, p);
        p.Append("group");
        VisitRightPadded(node.Padding.GroupExpression, JRightPadded.Location.GROUP_CLAUSE_GROUP_EXPRESSION, "by", p);
        Visit(node.Key,  p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSelectClause(Cs.SelectClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SELECT_CLAUSE_PREFIX, p);
        p.Append("select");
        Visit(node.Expression,  p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitOrdering(Cs.Ordering node, TOutputCapture p)
    {
        var direction = node.Direction.ToString()?.ToLower() ?? "";
        BeforeSyntax(node, Space.Location.ORDERING_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.ORDERING_EXPRESSION,  p);
        p.Append(direction);
        AfterSyntax(node, p);
        return node;
    }

    protected void VisitRightPadded<T>(JRightPadded<T>? node, JRightPadded.Location location, string? suffix, TOutputCapture p) where T : J
    {
        if (node != null)
        {
            PreVisitRightPadded(node, p);
            BeforeSyntax(Space.EMPTY, node.Markers, (Space.Location?)null, p);
            Visit(node.Element, p);
            AfterSyntax(node.Markers, p);
            VisitSpace(node.After, location.AfterLocation, p);
            if (suffix != null)
            {
                p.Append(suffix);
            }
            PostVisitRightPadded(node, p);
        }
    }

    protected void VisitRightPadded<T>(IList<JRightPadded<T>> nodes, JRightPadded.Location location, string suffixBetween, TOutputCapture p,
        [CallerArgumentExpression("nodes")] string? valueArgumentExpression = null) where T : J
    {
        for (int i = 0; i < nodes.Count; i++)
        {

            var node = nodes[i];
            PreVisitRightPadded(node, p);
            Visit(node.Element, p);
            VisitSpace(node.After, location.AfterLocation, p);
            VisitMarkers(node.Markers, p);
            var isLastElement = i < nodes.Count - 1;
            if (isLastElement)
            {
                p.Append(suffixBetween);
            }
            PostVisitRightPadded(node, p);

        }
    }

    public override J? VisitSwitchExpression(Cs.SwitchExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.SWITCH_EXPRESSION_EXPRESSION, p);
        p.Append("switch");
        VisitContainer("{", node.Padding.Arms, JContainer.Location.SWITCH_EXPRESSION_ARMS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchStatement(Cs.SwitchStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_STATEMENT_PREFIX, p);
        p.Append("switch");
        VisitContainer("(", node.Padding.Expression, JContainer.Location.SWITCH_STATEMENT_EXPRESSION, ",", ")", p);
        VisitContainer("{", node.Padding.Sections, JContainer.Location.SWITCH_STATEMENT_SECTIONS, "", "}", p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchSection(Cs.SwitchSection node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_SECTION_PREFIX, p);
        Visit(node.Labels, p);
        VisitStatements(node.Padding.Statements, JRightPadded.Location.SWITCH_SECTION_STATEMENTS, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnsafeStatement(Cs.UnsafeStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.UNSAFE_STATEMENT_PREFIX, p);
        p.Append("unsafe");
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCheckedExpression(Cs.CheckedExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CHECKED_EXPRESSION_PREFIX, p);
        Visit(node.CheckedOrUncheckedKeyword, p);
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }
    public override J? VisitCheckedStatement(Cs.CheckedStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CHECKED_STATEMENT_PREFIX, p);
        Visit(node.Keyword, p);
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRefExpression(Cs.RefExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.REF_EXPRESSION_PREFIX, p);
        p.Append("ref");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRefType(Cs.RefType node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.REF_TYPE_PREFIX, p);
        p.Append("ref");
        Visit(node.ReadonlyKeyword, p);
        Visit(node.TypeIdentifier, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRangeExpression(Cs.RangeExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.RANGE_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Start, JRightPadded.Location.RANGE_EXPRESSION_START, p);
        p.Append("..");
        Visit(node.End, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFixedStatement(Cs.FixedStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.FIXED_STATEMENT_PREFIX, p);
        p.Append("fixed");
        Visit(node.Declarations, p);
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLockStatement(Cs.LockStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.LOCK_STATEMENT_PREFIX, p);
        p.Append("lock");
        Visit(node.Expression, p);
        VisitStatement(node.Padding.Statement, JRightPadded.Location.LOCK_STATEMENT_STATEMENT, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CASE_PATTERN_SWITCH_LABEL_PREFIX, p);
        p.Append("case");
        Visit(node.Pattern, p);
        VisitLeftPadded("when", node.Padding.WhenClause, JLeftPadded.Location.CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE, p);
        VisitSpace(node.ColonToken, Space.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.Append(":");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDefaultSwitchLabel(Cs.DefaultSwitchLabel node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DEFAULT_SWITCH_LABEL_PREFIX, p);
        p.Append("default");
        VisitSpace(node.ColonToken, Space.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.Append(":");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchExpressionArm(Cs.SwitchExpressionArm node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_EXPRESSION_ARM_PREFIX, p);
        Visit(node.Pattern, p);
        VisitLeftPadded("when", node.Padding.WhenExpression, JLeftPadded.Location.SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION, p);
        VisitLeftPadded("=>", node.Padding.Expression, JLeftPadded.Location.SWITCH_EXPRESSION_ARM_EXPRESSION, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDefaultExpression(Cs.DefaultExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DEFAULT_EXPRESSION_PREFIX, p);
        p.Append("default");
        if (node.TypeOperator != null)
        {
            VisitContainer("(", node.Padding.TypeOperator, JContainer.Location.DEFAULT_EXPRESSION_TYPE_OPERATOR, "", ")", p);
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitYield(Cs.Yield node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.YIELD_PREFIX, p);
        p.Append("yield");
        Visit(node.ReturnOrBreakKeyword, p);
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitImplicitElementAccess(Cs.ImplicitElementAccess node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.IMPLICIT_ELEMENT_ACCESS_PREFIX, p);
        VisitContainer("[", node.Padding.ArgumentList, JContainer.Location.IMPLICIT_ELEMENT_ACCESS_ARGUMENT_LIST, ",", "]", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTupleExpression(Cs.TupleExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TUPLE_EXPRESSION_PREFIX, p);
        VisitContainer("(", node.Padding.Arguments, JContainer.Location.TUPLE_EXPRESSION_ARGUMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTupleType(Cs.TupleType node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TUPLE_TYPE_PREFIX, p);
        VisitContainer("(", node.Padding.Elements, JContainer.Location.TUPLE_TYPE_ELEMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTupleElement(Cs.TupleElement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TUPLE_ELEMENT_PREFIX, p);
        Visit(node.Type, p);
        if (node.Name != null)
        {
            Visit(node.Name, p);
        }
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.PARENTHESIZED_VARIABLE_DESIGNATION_PREFIX, p);
        VisitContainer("(", node.Padding.Variables, JContainer.Location.PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArgument(Cs.Argument node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ARGUMENT_PREFIX, p);
        var padding = node.Padding;

        if (node.NameColumn != null)
        {
            VisitRightPadded(padding.NameColumn, JRightPadded.Location.ARGUMENT_NAME_COLUMN, p);
            p.Append(':');
        }

        if (node.RefKindKeyword != null)
        {
            Visit(node.RefKindKeyword, p);
        }

        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitKeyword(Cs.Keyword node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.KEYWORD_PREFIX, p);
        p.Append(node.Kind.ToString().ToLowerInvariant());
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitBinaryPattern(Cs.BinaryPattern node, TOutputCapture p)
    {
        var @operator = node.Operator switch
        {
            Cs.BinaryPattern.OperatorType.And => "and",
            Cs.BinaryPattern.OperatorType.Or => "or",
            _ => throw new ArgumentOutOfRangeException()
        };
        BeforeSyntax(node, Space.Location.BINARY_PATTERN_PREFIX, p);
        Visit(node.Left, p);
        VisitSpace(node.Padding.Operator.Before, Space.Location.BINARY_PATTERN_OPERATOR, p);
        p.Append(@operator);
        Visit(node.Right, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConstantPattern(Cs.ConstantPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CONSTANT_PATTERN_PREFIX, p);
        Visit(node.Value, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDiscardPattern(Cs.DiscardPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DISCARD_PATTERN_PREFIX, p);
        p.Append("_");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitListPattern(Cs.ListPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.LIST_PATTERN_PREFIX, p);
        VisitContainer("[", node.Padding.Patterns, JContainer.Location.LIST_PATTERN_PATTERNS, ",", "]", p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParenthesizedPattern(Cs.ParenthesizedPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.PARENTHESIZED_PATTERN_PREFIX, p);
        VisitContainer("(", node.Padding.Pattern, JContainer.Location.LIST_PATTERN_PATTERNS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRecursivePattern(Cs.RecursivePattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.RECURSIVE_PATTERN_PREFIX, p);
        Visit(node.TypeQualifier, p);
        Visit(node.PositionalPattern, p);
        Visit(node.PropertyPattern, p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRelationalPattern(Cs.RelationalPattern node, TOutputCapture p)
    {
        var @operator = node.Operator switch
        {
            Cs.RelationalPattern.OperatorType.LessThan => "<",
            Cs.RelationalPattern.OperatorType.LessThanOrEqual => "<=",
            Cs.RelationalPattern.OperatorType.GreaterThan => ">",
            Cs.RelationalPattern.OperatorType.GreaterThanOrEqual => ">=",
            _ => throw new ArgumentOutOfRangeException()
        };
        BeforeSyntax(node, Space.Location.RELATIONAL_PATTERN_PREFIX, p);
        VisitSpace(node.Padding.Operator.Before, Space.Location.RELATIONAL_PATTERN_OPERATOR, p);
        p.Append(@operator);
        Visit(node.Value, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSlicePattern(Cs.SlicePattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SLICE_PATTERN_PREFIX, p);
        p.Append("..");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypePattern(Cs.TypePattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TYPE_PATTERN_PREFIX, p);
        Visit(node.TypeIdentifier, p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnaryPattern(Cs.UnaryPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.UNARY_PATTERN_PREFIX, p);
        Visit(node.Operator, p);
        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitVarPattern(Cs.VarPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.VAR_PATTERN_PREFIX, p);
        p.Append("var");
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPositionalPatternClause(Cs.PositionalPatternClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.POSITIONAL_PATTERN_CLAUSE_PREFIX, p);
        VisitContainer("(", node.Padding.Subpatterns, JContainer.Location.POSITIONAL_PATTERN_CLAUSE_SUBPATTERNS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPropertyPatternClause(Cs.PropertyPatternClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.PROPERTY_PATTERN_CLAUSE_PREFIX, p);
        VisitContainer("{", node.Padding.Subpatterns, JContainer.Location.PROPERTY_PATTERN_CLAUSE_SUBPATTERNS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIsPattern(Cs.IsPattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.IS_PATTERN_PREFIX, p);
        Visit(node.Expression, p);
        VisitSpace(node.Padding.Pattern.Before, Space.Location.IS_PATTERN_PATTERN, p);
        p.Append("is");
        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSubpattern(Cs.Subpattern node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SUBPATTERN_PREFIX, p);
        if (node.Name != null)
        {
            Visit(node.Name, p);
            VisitSpace(node.Padding.Pattern.Before, Space.Location.SUBPATTERN_PATTERN, p);
            p.Append(":");
        }

        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DISCARD_VARIABLE_DESIGNATION_PREFIX, p);
        Visit(node.Discard, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSingleVariableDesignation(Cs.SingleVariableDesignation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.SINGLE_VARIABLE_DESIGNATION_PREFIX, p);
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }



    public override J? VisitUsingStatement(Cs.UsingStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.USING_STATEMENT_PREFIX, p);
        if (node.AwaitKeyword != null)
        {
            Visit(node.AwaitKeyword, p);
        }

        VisitLeftPadded("using", node.Padding.Expression, JLeftPadded.Location.USING_STATEMENT_EXPRESSION, p);
        Visit(node.Statement, p);
        AfterSyntax(node, p);

        return node;
    }


    public override J? VisitUnary(Cs.Unary node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.UNARY_PREFIX, p);
        switch (node.Operator)
        {
            case Cs.Unary.Types.FromEnd:
                p.Append("^");
                Visit(node.Expression, p);
                break;
            case Cs.Unary.Types.PointerIndirection:
                p.Append("*");
                Visit(node.Expression, p);
                break;
            case Cs.Unary.Types.PointerType:
                Visit(node.Expression, p);
                p.Append("*");
                break;
            case Cs.Unary.Types.AddressOf:
                p.Append("&");
                Visit(node.Expression, p);
                break;
            case Cs.Unary.Types.SuppressNullableWarning:
                Visit(node.Expression, p);
                p.Append("!");
                break;
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAccessorDeclaration(Cs.AccessorDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ACCESSOR_DECLARATION_PREFIX, p);
        Visit(node.Attributes, p);
        Visit(node.Modifiers, p);
        VisitLeftPaddedEnum(node.Padding.Kind, JLeftPadded.Location.ACCESSOR_DECLARATION_KIND, p);
        Visit(node.ExpressionBody, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArrowExpressionClause(Cs.ArrowExpressionClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ARROW_EXPRESSION_CLAUSE_PREFIX, p);
        p.Append("=>");
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.ARROW_EXPRESSION_CLAUSE_EXPRESSION, ";", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitStackAllocExpression(Cs.StackAllocExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.COMPILATION_UNIT_PREFIX, p);
        p.Append("stackalloc");
        var newArray = node.Expression;
        VisitSpace(newArray.Prefix, Space.Location.NEW_ARRAY_INITIALIZER, p);
        Visit(newArray.TypeExpression, p);
        Visit(newArray.Dimensions, p);
        VisitContainer("{", newArray.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitPointerFieldAccess(Cs.PointerFieldAccess node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.POINTER_FIELD_ACCESS_PREFIX, p);
        Visit(node.Target, p);
        VisitLeftPadded("->", node.Padding.Name, JLeftPadded.Location.POINTER_FIELD_ACCESS_NAME, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitGotoStatement(Cs.GotoStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.GOTO_STATEMENT_PREFIX, p);
        p.Append("goto");
        Visit(node.CaseOrDefaultKeyword, p);
        Visit(node.Target, p);
        return node;
    }

    public override J? VisitEventDeclaration(Cs.EventDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.EVENT_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Modifiers, p);
        VisitLeftPadded("event", node.Padding.TypeExpression, JLeftPadded.Location.EVENT_DECLARATION_TYPE_EXPRESSION, p);
        VisitRightPadded(node.Padding.InterfaceSpecifier, JRightPadded.Location.EVENT_DECLARATION_INTERFACE_SPECIFIER, ".", p);
        Visit(node.Name, p);
        VisitContainer("{", node.Padding.Accessors, JContainer.Location.EVENT_DECLARATION_ACCESSORS, "", "}", p);
        return node;
    }


    public override JRightPadded<T>? VisitRightPadded<T>(JRightPadded<T>? node, JRightPadded.Location loc, TOutputCapture p)
    {
        PreVisitRightPadded(node, p);
        var result = base.VisitRightPadded(node, loc, p);
        PostVisitRightPadded(result, p);
        return result;
    }

    public override J? VisitCompilationUnit(Cs.CompilationUnit node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.COMPILATION_UNIT_PREFIX, p);

        foreach (var externAlias in node.Padding.Externs)
        {
            VisitRightPadded(externAlias, JRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in node.Padding.Usings)
        {
            VisitRightPadded(usingDirective, JRightPadded.Location.COMPILATION_UNIT_USINGS, p);
            p.Append(';');
        }

        foreach (var attributeList in node.AttributeLists)
        {
            Visit(attributeList, p);
        }

        VisitStatements(node.Padding.Members, JRightPadded.Location.COMPILATION_UNIT_MEMBERS, p);
        VisitSpace(node.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        AfterSyntax(node, p);

        return node;
    }

    public override J? VisitClassDeclaration(Cs.ClassDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CLASS_DECLARATION_PREFIX, p);
        Visit(node.AttributeList, p);
        Visit(node.Modifiers, p);

        Visit(node.Kind, p);
        Visit(node.Name, p);
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.CLASS_DECLARATION_TYPE_PARAMETERS, ",", ">", p);
        VisitContainer("(", node.Padding.PrimaryConstructor, JContainer.Location.CLASS_DECLARATION_PRIMARY_CONSTRUCTOR, ",", ")", p);
        VisitLeftPadded(":", node.Padding.Extendings, JLeftPadded.Location.CLASS_DECLARATION_EXTENDINGS, p);
        VisitContainer(node.Padding.Extendings == null ? ":" : ",", node.Padding.Implementings, JContainer.Location.CLASS_DECLARATION_IMPLEMENTINGS, ",", "", p);
        VisitContainer("", node.Padding.TypeParameterConstraintClauses, JContainer.Location.CLASS_DECLARATION_TYPE_PARAMETERS, "", "", p);

        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMethodDeclaration(Cs.MethodDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        Visit(node.Attributes, p);

        Visit(node.Modifiers, p);

        Visit(node.ReturnTypeExpression, p);
        VisitRightPadded(node.Padding.ExplicitInterfaceSpecifier, JRightPadded.Location.METHOD_DECLARATION_EXPLICIT_INTERFACE_SPECIFIER, ".", p);
        Visit(node.Name, p);


        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.METHOD_DECLARATION_TYPE_PARAMETERS, ",", ">", p);

        if (node.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
        {
            VisitContainer("(", node.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        VisitContainer(node.Padding.TypeParameterConstraintClauses, JContainer.Location.METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitAnnotatedStatement(Cs.AnnotatedStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ANNOTATED_STATEMENT_PREFIX, p);

        Visit(node.AttributeLists, p);
        Visit(node.Statement, p);
        AfterSyntax(node, p);

        return node;
    }

    public override J? VisitAttributeList(Cs.AttributeList node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ATTRIBUTE_LIST_PREFIX, p);
        p.Append('[');
        var padding = node.Padding;
        if (padding.Target != null)
        {
            VisitRightPadded(padding.Target, JRightPadded.Location.ATTRIBUTE_LIST_TARGET, p);
            p.Append(':');
        }

        VisitRightPadded(padding.Attributes, JRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, ",", p);
        p.Append(']');
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ARRAY_RANK_SPECIFIER_PREFIX, p);
        VisitContainer("", node.Padding.Sizes, JContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, ",", "", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAssignmentOperation(Cs.AssignmentOperation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        Visit(node.Variable, p);
        VisitLeftPadded(node.Padding.Operator, JLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        if (node.Operator == Cs.AssignmentOperation.OperatorType.NullCoalescing)
        {
            p.Append("??=");
        }

        Visit(node.Assignment, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAwaitExpression(Cs.AwaitExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.AWAIT_EXPRESSION_PREFIX, p);
        p.Append("await");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitBinary(Cs.Binary node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.BINARY_PREFIX, p);
        Visit(node.Left, p);
        VisitSpace(node.Padding.Operator.Before, Space.Location.BINARY_OPERATOR, p);
        if (node.Operator == Cs.Binary.OperatorType.As)
        {
            p.Append("as");
        }
        else if (node.Operator == Cs.Binary.OperatorType.NullCoalescing)
        {
            p.Append("??");
        }

        Visit(node.Right, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration node,
        TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        VisitRightPadded(node.Padding.Name,
            JRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.Append('{');

        foreach (var externAlias in node.Padding.Externs)
        {
            VisitRightPadded(externAlias, JRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in node.Padding.Usings)
        {
            VisitRightPadded(usingDirective, JRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.Append(';');
        }

        VisitStatements(node.Padding.Members,
            JRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        VisitSpace(node.End, Space.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p);
        p.Append('}');
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCollectionExpression(Cs.CollectionExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.COLLECTION_EXPRESSION_PREFIX, p);
        p.Append('[');
        VisitRightPadded(node.Padding.Elements, JRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS,
            ",", p);
        p.Append(']');
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEnumDeclaration(Cs.EnumDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ENUM_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Modifiers, p);
        VisitLeftPadded("enum", node.Padding.Name, JLeftPadded.Location.ENUM_DECLARATION_NAME, p);
        if (node.BaseType != null)
        {
            VisitLeftPadded(":", node.Padding.BaseType, JLeftPadded.Location.ENUM_DECLARATION_BASE_TYPE, p);
        }

        VisitContainer("{", node.Padding.Members, JContainer.Location.ENUM_DECLARATION_MEMBERS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitExpressionStatement(Cs.ExpressionStatement node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.AWAIT_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.EXPRESSION_STATEMENT_EXPRESSION, ";", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitExternAlias(Cs.ExternAlias node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.EXTERN_ALIAS_PREFIX, p);
        p.Append("extern");
        VisitLeftPadded("alias", node.Padding.Identifier, JLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");

        VisitRightPadded(node.Padding.Name, JRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, ";", p);

        VisitStatements(node.Padding.Externs, JRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_EXTERNS, p);
        // foreach (var externAlias in namespaceDeclaration.Padding.Externs)
        // {
        //     VisitRightPadded(externAlias, JRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
        // }
        VisitStatements(node.Padding.Usings, JRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS,p);

        // foreach (var usingDirective in namespaceDeclaration.Padding.Usings)
        // {
        //     VisitRightPadded(usingDirective, JRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
        //     p.Append(';');
        // }

        VisitStatements(node.Padding.Members, JRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        return node;
    }

    public override J? VisitInterpolatedString(Cs.InterpolatedString node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.INTERPOLATED_STRING_PREFIX, p);
        p.Append(node.Start);
        VisitRightPadded(node.Padding.Parts, JRightPadded.Location.INTERPOLATED_STRING_PARTS, "", p);
        p.Append(node.End);
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitInterpolation(Cs.Interpolation node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.INTERPOLATION_PREFIX, p);
        p.Append('{');
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.INTERPOLATION_EXPRESSION, p);

        if (node.Alignment != null)
        {
            p.Append(',');
            VisitRightPadded(node.Padding.Alignment, JRightPadded.Location.INTERPOLATION_ALIGNMENT, p);
        }

        if (node.Format != null)
        {
            p.Append(':');
            VisitRightPadded(node.Padding.Format, JRightPadded.Location.INTERPOLATION_FORMAT, p);
        }

        p.Append('}');
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitNullSafeExpression(Cs.NullSafeExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.NULL_SAFE_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION,
            p);
        p.Append("?");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPropertyDeclaration(Cs.PropertyDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.PROPERTY_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Modifiers, p);
        Visit(node.TypeExpression, p);
        VisitRightPadded(node.Padding.InterfaceSpecifier, JRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, ".", p);
        Visit(node.Name, p);
        Visit(node.Accessors, p);
        Visit(node.ExpressionBody, p);
        VisitLeftPadded("=", node.Padding.Initializer, JLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUsingDirective(Cs.UsingDirective node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.USING_DIRECTIVE_PREFIX, p);

        if (node.Global)
        {
            p.Append("global");
            VisitRightPadded(node.Padding.Global, JRightPadded.Location.USING_DIRECTIVE_GLOBAL, p);
        }

        p.Append("using");

        if (node.Static)
        {
            VisitLeftPadded(node.Padding.Static, JLeftPadded.Location.USING_DIRECTIVE_STATIC, p);
            p.Append("static");
        }
        else if (node.Alias != null)
        {
            if (node.Unsafe)
            {
                VisitLeftPadded(node.Padding.Unsafe, JLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p);
                p.Append("unsafe");
            }

            VisitRightPadded(node.Padding.Alias, JRightPadded.Location.USING_DIRECTIVE_ALIAS, p);
            p.Append('=');
        }

        Visit(node.NamespaceOrType, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CONVERSION_OPERATOR_DECLARATION_PREFIX, p);
        foreach (var modifier in node.Modifiers)
        {
            Visit(modifier, p);
        }

        VisitLeftPadded(node.Padding.Kind, JLeftPadded.Location.CONVERSION_OPERATOR_DECLARATION_KIND, p);
        p.Append(node.Kind.ToString().ToLower());
        VisitLeftPadded("operator", node.Padding.ReturnType, JLeftPadded.Location.CONVERSION_OPERATOR_DECLARATION_RETURN_TYPE, p);
        VisitContainer("(", node.Padding.Parameters, JContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ")", p);
        Visit(node.ExpressionBody, p);
        Visit(node.Body, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEnumMemberDeclaration(Cs.EnumMemberDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.ENUM_MEMBER_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Name, p);
        VisitLeftPadded("=", node.Padding.Initializer, JLeftPadded.Location.ENUM_MEMBER_DECLARATION_INITIALIZER, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIndexerDeclaration(Cs.IndexerDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.INDEXER_DECLARATION_PREFIX, p);
        foreach (var modifier in node.Modifiers)
        {
            Visit(modifier, p);
        }

        Visit(node.TypeExpression, p);
        VisitRightPadded(node.Padding.ExplicitInterfaceSpecifier, JRightPadded.Location.INDEXER_DECLARATION_EXPLICIT_INTERFACE_SPECIFIER, ".", p);
        Visit(node.Indexer, p);
        VisitContainer("[", node.Padding.Parameters, JContainer.Location.INDEXER_DECLARATION_PARAMETERS, ",", "]", p);
        VisitLeftPadded("", node.Padding.ExpressionBody, JLeftPadded.Location.INDEXER_DECLARATION_EXPRESSION_BODY, p); //todo: probably should be => as inner block is just wrong representation
        Visit(node.Accessors, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDelegateDeclaration(Cs.DelegateDeclaration node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DELEGATE_DECLARATION_PREFIX, p);
        Visit(node.Attributes, p);
        Visit(node.Modifiers, p);
        VisitLeftPadded("delegate", node.Padding.ReturnType, JLeftPadded.Location.DELEGATE_DECLARATION_RETURN_TYPE, p);
        Visit(node.Identifier, p);
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ">", p);
        VisitContainer("(", node.Padding.Parameters, JContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ")", p);
        VisitContainer(node.Padding.TypeParameterConstraintClauses, JContainer.Location.DELEGATE_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDestructorDeclaration(Cs.DestructorDeclaration node, TOutputCapture p)
    {
        var method = node.MethodCore;
        BeforeSyntax(method, Space.Location.DESTRUCTOR_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(method.LeadingAnnotations, p);
        foreach (var modifier in method.Modifiers)
        {
            _delegate.Visit(modifier, p);
        }

        Visit(method.Annotations.Name.Annotations, p);
        p.Append("~");
        Visit(method.Name, p);

        VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);

        Visit(method.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConstructor(Cs.Constructor node, TOutputCapture p)
    {
        var method = node.ConstructorCore;
        BeforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(method.LeadingAnnotations, p);
        foreach (var modifier in method.Modifiers)
        {
            _delegate.Visit(modifier, p);
        }

        Visit(method.Annotations.Name.Annotations, p);
        Visit(method.Name, p);



        if (method.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
        {
            VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        Visit(node.Initializer, p);

        Visit(method.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConstructorInitializer(Cs.ConstructorInitializer node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        p.Append(":");
        Visit(node.Keyword, p);
        VisitContainer("(", node.Padding.Arguments, JContainer.Location.CONSTRUCTOR_INITIALIZER_ARGUMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLambda(Cs.Lambda node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.LAMBDA_PREFIX, p);

        var javaLambda = node.LambdaExpression;

        VisitMarkers(node.Markers, p);
        Visit(node.Modifiers, p);
        Visit(node.ReturnType, p);
        Visit(javaLambda, p);
        AfterSyntax(node, p);
        return node;
    }


    public override Space VisitSpace(Space node, Space.Location? loc, TOutputCapture p)
    {
        return _javaPrinter.VisitSpace(node, loc, p);
        // return _delegate.VisitSpace(space, loc, p);
    }



    protected void VisitLeftPaddedEnum<T>(JLeftPadded<T>? node, JLeftPadded.Location location, TOutputCapture p) where T : Enum
    {
        if (node == null)
            return;
        VisitLeftPadded(node, location, p);
        p.Append(node.Element.ToString().ToLower());
    }
    protected void VisitLeftPadded<T>(string prefix, JLeftPadded<T>? node, JLeftPadded.Location location, TOutputCapture p,
        [CallerArgumentExpression("node")] string? valueArgumentExpression = null) where T : J
    {
        if (node != null)
        {
            PreVisitLeftPadded(node, p);
            BeforeSyntax(node.Before, node.Markers, location.BeforeLocation, p);

            p.Append(prefix);

            Visit(node.Element, p);
            AfterSyntax(node.Markers, p);
            PostVisitLeftPadded(node,p);
        }
    }


    protected virtual void VisitContainer<T>(string before, JContainer<T>? node, JContainer.Location location, string suffixBetween, string after, TOutputCapture p,
        [CallerArgumentExpression("node")] string? valueArgumentExpression = null) where T : J
    {
        PreVisitContainer(node, p);
        if (node == null)
        {
            return;
        }

        VisitSpace(node.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitRightPadded(node.Padding.Elements, location.ElementLocation, suffixBetween, p);
        p.Append(after);
        PostVisitContainer(node, p);
    }



    protected void VisitStatements(string before, JContainer<Statement>? node, JContainer.Location location, string after, TOutputCapture p)
    {
        if (node == null)
        {
            return;
        }

        VisitSpace(node.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitStatements(node.Padding.Elements, location.ElementLocation, p);
        p.Append(after);
    }

    protected void VisitStatements<T>(IList<JRightPadded<T>> node, JRightPadded.Location location, TOutputCapture p) where T : Statement
    {
        foreach (var paddedStat in node)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatements(IList<JRightPadded<Statement>> node, JRightPadded.Location location, TOutputCapture p)
    {
        foreach (var paddedStat in node)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement<T>(JRightPadded<T>? node, JRightPadded.Location location, TOutputCapture p) where T : Statement
    {

        if (node == null)
        {
            return;
        }
        PreVisitRightPadded(node, p);
        Visit(node.Element, p);
        VisitSpace(node.After, location.AfterLocation, p);
        VisitMarkers(node.Markers, p);

        if (Cursor.Parent?.Value is J.Block && Cursor.Parent?.Parent?.Value is J.NewClass)
        {
            p.Append(',');
            return;
        }

        PostVisitRightPadded(node, p);

        _delegate.PrintStatementTerminator(node.Element, p);

    }

    public override J? VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_PREFIX, p);
        p.Append("where");
        VisitRightPadded(node.Padding.TypeParameter, JRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER,  p);
        p.Append(":");
        VisitContainer("", node.Padding.TypeParameterConstraints, JContainer.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS, ",", "", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitClassOrStructConstraint(Cs.ClassOrStructConstraint node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CLASS_OR_STRUCT_CONSTRAINT_PREFIX, p);
        p.Append(node.Kind == Cs.ClassOrStructConstraint.TypeKind.Class ? "class" : "struct");
        return node;
    }

    public override J? VisitConstructorConstraint(Cs.ConstructorConstraint node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.CONSTRUCTOR_CONSTRAINT_PREFIX, p);
        p.Append("new()");
        return node;
    }

    public override J? VisitDefaultConstraint(Cs.DefaultConstraint node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.DEFAULT_CONSTRAINT_PREFIX, p);
        p.Append("default");
        return node;
    }

    public override J? VisitInitializerExpression(Cs.InitializerExpression node, TOutputCapture p)
    {
        BeforeSyntax(node, Space.Location.BLOCK_PREFIX, p);
        VisitContainer("{", node.Padding.Expressions, JContainer.Location.INITIALIZER_EXPRESSION_EXPRESSIONS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }


    protected virtual void PreVisitContainer<T>(JContainer<T>? node, TOutputCapture state)
    {

    }

    protected  virtual void PreVisitRightPadded<T>(JRightPadded<T>? node, TOutputCapture state)
    {
    }

    protected  virtual void PreVisitLeftPadded<T>(JLeftPadded<T>? node, TOutputCapture state)
    {
    }

    protected virtual void PostVisitContainer<T>(JContainer<T>? node, TOutputCapture state)
    {

    }

    protected  virtual void PostVisitRightPadded<T>(JRightPadded<T>? node, TOutputCapture state)
    {
    }

    protected  virtual void PostVisitLeftPadded<T>(JLeftPadded<T>? node, TOutputCapture state)
    {
    }


    private class CSharpJavaPrinter(CSharpPrinter<TOutputCapture, TState> _parent) : JavaPrinter<TOutputCapture, TState>
    {
        public override J? PreVisit(Core.Tree? node, TOutputCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
        {
            return _parent.PreVisit(node, p);
        }

        public override J? PostVisit(Core.Tree node, TOutputCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
        {
            return _parent.PostVisit(node, p);
        }
#if DEBUG_VISITOR
        [DebuggerStepThrough]
#endif
        public override J? Visit(Rewrite.Core.Tree? node, TOutputCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
        {
            if (node is Cs)
            {
                // Re-route printing back up to C#
                return _parent.Visit(node, p);
            }
            else if(node is null or J)
            {
                return base.Visit(node, p);
            }
            // else if (tree is ParseError parseError)
            // {
            //     p.Out.Append("/* LST PARSER ERROR\n");
            //     var parseExceptionResult = (ParseExceptionResult)parseError.Markers.First();
            //     p.Out.Append(parseExceptionResult.Message);
            //     p.Out.Append('\n');
            //     p.Out.Append("*/\n");
            //     p.Out.Append(parseError.Text);
            // }

            return base.Visit(node, p);
        }

        public override Space VisitSpace(Space space, Space.Location? loc, TOutputCapture p)
        {
            // this workaround ensures that parent receives VisitSpace callback on EVERY call, not just ones initiated by parent
            return _parent.VisitSpace(space, loc, p);
        }

        public override J VisitNewArray(J.NewArray node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.NEW_ARRAY_PREFIX, p);
            p.Append("new");

            Visit(node.TypeExpression, p);
            // VisitArrayDimension()
            Visit(node.Dimensions, p);
            VisitContainer("{", node.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
            AfterSyntax(node, p);
            return node;
        }




        public override J? VisitClassDeclarationKind(J.ClassDeclaration.Kind node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.CLASS_KIND, p);
            string kindStr = node.KindType switch
            {
                J.ClassDeclaration.Kind.Types.Class => "class",
                J.ClassDeclaration.Kind.Types.Annotation => "class",
                J.ClassDeclaration.Kind.Types.Enum => "enum",
                J.ClassDeclaration.Kind.Types.Interface => "interface",
                J.ClassDeclaration.Kind.Types.Record => "record",
                J.ClassDeclaration.Kind.Types.Value => "struct",
                _ => ""
            };
            p.Append(kindStr);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitClassDeclaration(J.ClassDeclaration node, TOutputCapture p)
        {
            var csClassDeclaration = _parent.Cursor.Value as Cs.ClassDeclaration;

            BeforeSyntax(node, Space.Location.CLASS_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(node.LeadingAnnotations, p);
            foreach (var modifier in node.Modifiers)
            {
                Visit(modifier, p);
            }

            Visit(node.Padding.DeclarationKind.Annotations, p);
            Visit(node.Padding.DeclarationKind, p);
            Visit(node.Name, p);
            VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", node.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
            VisitLeftPadded(":", node.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
            VisitContainer(node.Padding.Extends == null ? ":" : ",", node.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
            foreach (var typeParameterClause in csClassDeclaration?.TypeParameterConstraintClauses ?? [])
            {
                _parent.Visit(typeParameterClause, p);
            }
            Visit(node.Body, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitAnnotation(J.Annotation node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.ANNOTATION_PREFIX, p);
            Visit(node.AnnotationType, p);
            VisitContainer("(", node.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitBlock(J.Block node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.BLOCK_PREFIX, p);

            if (node.Static)
            {
                p.Append("static");
                VisitRightPadded(node.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
            }

            if (node.Markers.FirstOrDefault(m => m is SingleExpressionBlock) != null)
            {
                p.Append("=>");
                var statement = node.Padding.Statements.First();
                if (statement.Element is Cs.ExpressionStatement expressionStatement)
                {
                    // expression statements model their own semicolon
                    Visit(expressionStatement, p);
                }
                else
                {
                    VisitRightPadded(statement, JRightPadded.Location.BLOCK_STATEMENT, ";", p);
                }
                // VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);

                // VisitSpace(block.End, Space.Location.BLOCK_END, p);
            }
            else if (!node.Markers.OfType<OmitBraces>().Any())
            {
                p.Append('{');
                VisitStatements(node.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(node.End, Space.Location.BLOCK_END, p);
                p.Append('}');
            }
            else
            {
                if (node.Padding.Statements.Any())
                {
                    VisitStatements(node.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                }
                else
                {
                    p.Append(";");
                }

                VisitSpace(node.End, Space.Location.BLOCK_END, p);
            }

            AfterSyntax(node, p);
            return node;
        }

        protected override void VisitStatements(IList<JRightPadded<Statement>> node, JRightPadded.Location location, TOutputCapture p)
        {

            for (int i = 0; i < node.Count; i++)
            {
                var paddedStat = node[i];
                VisitStatement(paddedStat, location, p);
                if (i < node.Count - 1 &&
                    (Cursor.Parent?.Value is J.NewClass ||
                     (Cursor.Parent?.Value is J.Block &&
                      Cursor.GetParent(2)?.Value is J.NewClass)))
                {
                    p.Append(',');
                }
            }
        }

        public override J VisitMethodDeclaration(J.MethodDeclaration node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(node.LeadingAnnotations, p);
            foreach (var modifier in node.Modifiers)
            {
                Visit(modifier, p);
            }

            Visit(node.ReturnTypeExpression, p);
            Visit(node.Annotations.Name.Annotations, p);
            Visit(node.Name, p);

            var typeParameters = node.Annotations.TypeParameters;
            if (typeParameters != null)
            {
                Visit(typeParameters.Annotations, p);
                VisitSpace(typeParameters.Prefix, Space.Location.TYPE_PARAMETERS, p);
                VisitMarkers(typeParameters.Markers, p);
                p.Append('<');
                VisitRightPadded(typeParameters.Padding.Parameters, JRightPadded.Location.TYPE_PARAMETER, ",", p);
                p.Append('>');
            }

            if (node.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
            {
                VisitContainer("(", node.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
            }


            VisitContainer("throws", node.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
            Visit(node.Body, p);
            VisitLeftPadded("default", node.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitMethodInvocation(J.MethodInvocation node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.METHOD_INVOCATION_PREFIX, p);
            var prefix = node.Name.SimpleName != "" ? "." : "";
            VisitRightPadded(node.Padding.Select, JRightPadded.Location.METHOD_SELECT, prefix, p);
            Visit(node.Name, p);
            VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", node.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitCatch(J.Try.Catch node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.CATCH_PREFIX, p);
            p.Append("catch");
            if (node.Parameter.Tree.TypeExpression != null)
            {
                Visit(node.Parameter, p);
            }

            Visit(node.Body, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitForEachLoop(J.ForEachLoop node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.FOR_EACH_LOOP_PREFIX, p);
            p.Append("foreach");
            var ctrl = node.LoopControl;
            VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
            p.Append('(');
            VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, "in", p);
            VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, "", p);
            p.Append(')');
            VisitStatement(node.Padding.Body, JRightPadded.Location.FOR_BODY, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitInstanceOf(J.InstanceOf node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.INSTANCEOF_PREFIX, p);
            VisitRightPadded(node.Padding.Expression, JRightPadded.Location.INSTANCEOF, "is", p);
            Visit(node.Clazz, p);
            Visit(node.Pattern, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitLambda(J.Lambda node, TOutputCapture p)
        {
            BeforeSyntax(node, Space.Location.LAMBDA_PREFIX, p);
            VisitSpace(node.Params.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
            VisitMarkers(node.Params.Markers, p);

            if (node.Params.Parenthesized)
            {
                p.Append('(');
                VisitRightPadded(node.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
                p.Append(')');
            }
            else
            {
                VisitRightPadded(node.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
            }

            VisitSpace(node.Arrow, Space.Location.LAMBDA_ARROW_PREFIX, p);
            p.Append("=>");
            Visit(node.Body, p);
            AfterSyntax(node, p);
            return node;
        }

        public override J VisitPrimitive(J.Primitive node, TOutputCapture p)
        {
            string keyword = node.Type.Kind switch
            {
                JavaType.Primitive.PrimitiveType.Boolean => "bool",
                JavaType.Primitive.PrimitiveType.Byte => "byte",
                JavaType.Primitive.PrimitiveType.Char => "char",
                JavaType.Primitive.PrimitiveType.Double => "double",
                JavaType.Primitive.PrimitiveType.Float => "float",
                JavaType.Primitive.PrimitiveType.Int => "int",
                JavaType.Primitive.PrimitiveType.Long => "long",
                JavaType.Primitive.PrimitiveType.Short => "short",
                JavaType.Primitive.PrimitiveType.Void => "void",
                JavaType.Primitive.PrimitiveType.String => "string",
                JavaType.Primitive.PrimitiveType.None => throw new InvalidOperationException(
                    "Unable to print None primitive"),
                JavaType.Primitive.PrimitiveType.Null => throw new InvalidOperationException(
                    "Unable to print Null primitive"),
                _ => throw new InvalidOperationException("Unable to print non-primitive type")
            };

            BeforeSyntax(node, Space.Location.PRIMITIVE_PREFIX, p);
            p.Append(keyword);
            AfterSyntax(node, p);
            return node;
        }

        public override Core.Marker.Marker VisitMarker(Core.Marker.Marker node, TOutputCapture p)
        {
            if (node is Semicolon)
            {
                p.Append(';');
            }
            else if (node is TrailingComma trailingComma)
            {
                p.Append(',');
                VisitSpace(trailingComma.Suffix, Space.Location.LANGUAGE_EXTENSION, p);
            }

            return node;
        }

        // override print
        public override void PrintStatementTerminator(Statement node, TOutputCapture p)
        {
            while (node is Cs.AnnotatedStatement annotatedStatement)
                node = annotatedStatement.Statement;
            // while (s is Cs.FixedStatement f && f.Block.Markers.OfType<OmitBraces>().Any() && f.Block.Statements.Count > 0)
            //     s = f.Block.Statements.First();

            if (node is
                Cs.ExpressionStatement or
                Cs.AwaitExpression { Expression: J.ForEachLoop { Body: J.Block } })
            {
                return;
            }

            if (node is
                Cs.AssignmentOperation or
                Cs.Yield or
                Cs.DelegateDeclaration or
                Cs.UsingStatement { Statement: not J.Block and not Cs.UsingStatement and not Cs.ExpressionStatement } or
                Cs.AwaitExpression { Expression: not J.ForEachLoop { Body: not J.Block}} or
                Cs.PropertyDeclaration { Initializer: not null } or
                Cs.EventDeclaration { Accessors: null } or
                Cs.GotoStatement or
                Cs.AccessorDeclaration { Body: null, ExpressionBody: null } or
                Cs.UsingDirective or
                Cs.ExternAlias
                )
            {
                p.Append(';');
            }
            else if(Cursor.Parent?.Value is not Cs.ExpressionStatement)
            {
                base.PrintStatementTerminator(node, p);
            }
        }
    }

    public override Markers VisitMarkers(Markers? markers, TOutputCapture p)
    {
        return _delegate.VisitMarkers(markers, p);
    }

    private static readonly Func<string, string> JAVA_MARKER_WRAPPER =
        o => "/*~~" + o + (string.IsNullOrEmpty(o) ? "" : "~~") + ">*/";

    private void BeforeSyntax(J cs, Space.Location loc, TOutputCapture p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }


    private void BeforeSyntax(Space prefix, Markers markers, Space.Location? loc, TOutputCapture p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc.Value, p);
        }

        VisitMarkers(markers, p);

        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }


    private void AfterSyntax(Cs g, TOutputCapture p)
    {
        AfterSyntax(g.Markers, p);
    }

    private void AfterSyntax(Markers markers, TOutputCapture p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }
}
