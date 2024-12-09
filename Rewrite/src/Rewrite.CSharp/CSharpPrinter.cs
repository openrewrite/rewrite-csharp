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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

// using Rewrite.RewriteJava.Tree;
using Tree = Rewrite.Core.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpPrinter<TState> : CSharpVisitor<PrintOutputCapture<TState>>
{
    private readonly CSharpJavaPrinter _delegate;

    public CSharpPrinter()
    {
        _delegate = new CSharpJavaPrinter(this);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public override J? Visit(Rewrite.Core.Tree? tree, PrintOutputCapture<TState> p)
    {
        if (!(tree is Cs))
        {
            // Re-route printing to the Java printer
            return _delegate.Visit(tree, p);
        }
        else
        {
            return base.Visit(tree, p);
        }
    }

    public override Cs VisitArrayType(Cs.ArrayType newArray, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(newArray, CsSpace.Location.ARRAY_TYPE_PREFIX, p);
        Visit(newArray.TypeExpression, p);
        Visit(newArray.Dimensions, p);
        AfterSyntax(newArray, p);
        return newArray;
    }

    public override J? VisitAliasQualifiedName(Cs.AliasQualifiedName node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.ALIAS_QUALIFIED_NAME_PREFIX, p);
        VisitRightPadded(node.Padding.Alias, CsRightPadded.Location.ALIAS_QUALIFIED_NAME_ALIAS, p);
        p.Append("::");
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypeParameter(Cs.TypeParameter node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.TYPE_PARAMETER_PREFIX, p);
        Visit(node.AttributeLists, p);
        VisitLeftPaddedEnum(node.Padding.Variance, CsLeftPadded.Location.TYPE_PARAMETER_VARIANCE, p);
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryExpression(Cs.QueryExpression node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.QUERY_EXPRESSION_PREFIX, p);
        Visit(node.FromClause, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryContinuation(Cs.QueryContinuation node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.QUERY_CONTINUATION_PREFIX, p);
        p.Append("into");
        Visit(node.Identifier, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFromClause(Cs.FromClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.FROM_CLAUSE_PREFIX, p);
        p.Append("from");
        Visit(node.TypeIdentifier, p);
        VisitRightPadded(node.Padding.Identifier, CsRightPadded.Location.FROM_CLAUSE_IDENTIFIER, p);
        p.Append("in");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitQueryBody(Cs.QueryBody node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.QUERY_BODY_PREFIX, p);
        foreach (var clause in node.Clauses)
        {
            Visit(clause, p);
        }
        Visit(node.SelectOrGroup, p);
        Visit(node.Continuation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLetClause(Cs.LetClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.LET_CLAUSE_PREFIX, p);
        p.Append("let");
        VisitRightPadded(node.Padding.Identifier, CsRightPadded.Location.LET_CLAUSE_IDENTIFIER, p);
        p.Append("=");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitJoinClause(Cs.JoinClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.JOIN_CLAUSE_PREFIX, p);
        p.Append("join");
        VisitRightPadded(node.Padding.Identifier, CsRightPadded.Location.JOIN_CLAUSE_IDENTIFIER, p);
        p.Append("in");
        VisitRightPadded(node.Padding.InExpression, CsRightPadded.Location.JOIN_CLAUSE_IN_EXPRESSION, p);
        p.Append("on");
        VisitRightPadded(node.Padding.LeftExpression, CsRightPadded.Location.JOIN_CLAUSE_LEFT_EXPRESSION, p);
        p.Append("equals");
        Visit(node.RightExpression, p);
        VisitLeftPadded("into", node.Padding.Into, CsLeftPadded.Location.JOIN_CLAUSE_INTO, p);
        Visit(node.Into, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitWhereClause(Cs.WhereClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.WHERE_CLAUSE_PREFIX, p);
        p.Append("where");
        Visit(node.Condition, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitJoinIntoClause(Cs.JoinIntoClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.JOIN_INTO_CLAUSE_PREFIX, p);
        p.Append("into");
        Visit(node.Identifier, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitOrderByClause(Cs.OrderByClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.JOIN_INTO_CLAUSE_PREFIX, p);
        p.Append("orderby");
        VisitRightPadded(node.Padding.Orderings, CsRightPadded.Location.ORDER_BY_CLAUSE_ORDERINGS, ",", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J VisitForEachVariableLoop(Cs.ForEachVariableLoop forEachLoop, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
        p.Append("foreach");
        var ctrl = forEachLoop.ControlElement;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Variable, CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_VARIABLE, "in", p);
        VisitRightPadded(ctrl.Padding.Iterable, CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_ITERABLE, "", p);
        p.Append(')');
        VisitStatement(forEachLoop.Padding.Body, CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_BODY, p);
        AfterSyntax(forEachLoop, p);
        return forEachLoop;
    }

    public override Cs VisitGroupClause(Cs.GroupClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.GROUP_CLAUSE_PREFIX, p);
        p.Append("group");
        VisitRightPadded(node.Padding.GroupExpression, CsRightPadded.Location.GROUP_CLAUSE_GROUP_EXPRESSION, "by", p);
        Visit(node.Key,  p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSelectClause(Cs.SelectClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SELECT_CLAUSE_PREFIX, p);
        p.Append("select");
        Visit(node.Expression,  p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitOrdering(Cs.Ordering node, PrintOutputCapture<TState> p)
    {
        var direction = node.Direction.ToString()?.ToLower() ?? "";
        BeforeSyntax(node, CsSpace.Location.ORDERING_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, CsRightPadded.Location.ORDERING_EXPRESSION,  p);
        p.Append(direction);
        AfterSyntax(node, p);
        return node;
    }

    protected void VisitRightPadded<T>(JRightPadded<T>? rightPadded, CsRightPadded.Location location, string? suffix, PrintOutputCapture<TState> p) where T : J
    {
        if (rightPadded != null)
        {
            BeforeSyntax(Space.EMPTY, rightPadded.Markers, (CsSpace.Location?)null, p);
            Visit(rightPadded.Element, p);
            AfterSyntax(rightPadded.Markers, p);
            VisitSpace(rightPadded.After, location.AfterLocation, p);
            if (suffix != null)
            {
                p.Append(suffix);
            }
        }
    }

    public override J? VisitSwitchExpression(Cs.SwitchExpression node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SWITCH_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, CsRightPadded.Location.SWITCH_EXPRESSION_EXPRESSION, p);
        p.Append("switch");
        VisitContainer("{", node.Padding.Arms, CsContainer.Location.SWITCH_EXPRESSION_ARMS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchStatement(Cs.SwitchStatement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SWITCH_STATEMENT_PREFIX, p);
        p.Append("switch");
        VisitContainer("(", node.Padding.Expression, CsContainer.Location.SWITCH_STATEMENT_EXPRESSION, ",", ")", p);
        VisitContainer("{", node.Padding.Sections, CsContainer.Location.SWITCH_STATEMENT_SECTIONS, "", "}", p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchSection(Cs.SwitchSection node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SWITCH_SECTION_PREFIX, p);
        Visit(node.Labels, p);
        VisitStatements(node.Padding.Statements, CsRightPadded.Location.SWITCH_SECTION_STATEMENTS, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnsafeStatement(Cs.UnsafeStatement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.UNSAFE_STATEMENT_PREFIX, p);
        p.Append("unsafe");
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCheckedStatement(Cs.CheckedStatement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.CHECKED_STATEMENT_PREFIX, p);
        p.Append("checked");
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRangeExpression(Cs.RangeExpression node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.RANGE_EXPRESSION_PREFIX, p);
        VisitRightPadded(node.Padding.Start, CsRightPadded.Location.RANGE_EXPRESSION_START, p);
        p.Append("..");
        Visit(node.End, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFixedStatement(Cs.FixedStatement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.FIXED_STATEMENT_PREFIX, p);
        p.Append("fixed");
        Visit(node.Declarations, p);
        Visit(node.Block, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLockStatement(Cs.LockStatement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.LOCK_STATEMENT_PREFIX, p);
        p.Append("lock");
        Visit(node.Expression, p);
        VisitStatement(node.Padding.Statement, CsRightPadded.Location.LOCK_STATEMENT_STATEMENT, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_PREFIX, p);
        p.Append("case");
        Visit(node.Pattern, p);
        VisitLeftPadded("when", node.Padding.WhenClause, CsLeftPadded.Location.CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE, p);
        VisitSpace(node.ColonToken, CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.Append(":");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDefaultSwitchLabel(Cs.DefaultSwitchLabel node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.DEFAULT_SWITCH_LABEL_PREFIX, p);
        p.Append("default");
        VisitSpace(node.ColonToken, CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.Append(":");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchExpressionArm(Cs.SwitchExpressionArm node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SWITCH_EXPRESSION_ARM_PREFIX, p);
        Visit(node.Pattern, p);
        VisitLeftPadded("when", node.Padding.WhenExpression, CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION, p);
        VisitLeftPadded("=>", node.Padding.Expression, CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_EXPRESSION, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDefaultExpression(Cs.DefaultExpression node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.DEFAULT_EXPRESSION_PREFIX, p);
        p.Append("default");
        if (node.TypeOperator != null)
        {
            VisitContainer("(", node.Padding.TypeOperator, CsContainer.Location.DEFAULT_EXPRESSION_TYPE_OPERATOR, "", ")", p);
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitYield(Cs.Yield node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.YIELD_PREFIX, p);
        p.Append("yield");
        Visit(node.ReturnOrBreakKeyword, p);
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitImplicitElementAccess(Cs.ImplicitElementAccess node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.IMPLICIT_ELEMENT_ACCESS_PREFIX, p);
        VisitContainer("[", node.Padding.ArgumentList, CsContainer.Location.IMPLICIT_ELEMENT_ACCESS_ARGUMENT_LIST, ",", "]", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTupleExpression(Cs.TupleExpression tupleExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(tupleExpression, CsSpace.Location.TUPLE_EXPRESSION_PREFIX, p);
        VisitContainer("(", tupleExpression.Padding.Arguments, CsContainer.Location.TUPLE_EXPRESSION_ARGUMENTS, ",", ")", p);
        AfterSyntax(tupleExpression, p);
        return tupleExpression;
    }

    public override J? VisitTupleType(Cs.TupleType node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.TUPLE_TYPE_PREFIX, p);
        VisitContainer("(", node.Padding.Elements, CsContainer.Location.TUPLE_TYPE_ELEMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTupleElement(Cs.TupleElement node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.TUPLE_ELEMENT_PREFIX, p);
        Visit(node.Type, p);
        if (node.Name != null)
        {
            Visit(node.Name, p);
        }
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.PARENTHESIZED_VARIABLE_DESIGNATION_PREFIX, p);
        VisitContainer("(", node.Padding.Variables, CsContainer.Location.PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArgument(Cs.Argument argument, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(argument, CsSpace.Location.ARGUMENT_PREFIX, p);
        var padding = argument.Padding;

        if (argument.NameColumn != null)
        {
            VisitRightPadded(padding.NameColumn, CsRightPadded.Location.NAMED_ARGUMENT_NAME_COLUMN, p);
            p.Append(':');
        }

        if (argument.RefKindKeyword != null)
        {
            Visit(argument.RefKindKeyword, p);
        }

        Visit(argument.Expression, p);
        AfterSyntax(argument, p);
        return argument;
    }

    public override J? VisitKeyword(Cs.Keyword keyword, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(keyword, CsSpace.Location.KEYWORD_PREFIX, p);
        p.Append(keyword.Kind.ToString().ToLowerInvariant());
        AfterSyntax(keyword, p);
        return keyword;
    }

    public override J? VisitBinaryPattern(Cs.BinaryPattern node, PrintOutputCapture<TState> p)
    {
        var @operator = node.Operator switch
        {
            Cs.BinaryPattern.OperatorType.And => "and",
            Cs.BinaryPattern.OperatorType.Or => "or",
            _ => throw new ArgumentOutOfRangeException()
        };
        BeforeSyntax(node, CsSpace.Location.BINARY_PATTERN_PREFIX, p);
        Visit(node.Left, p);
        VisitSpace(node.Padding.Operator.Before, CsSpace.Location.BINARY_PATTERN_OPERATOR, p);
        p.Append(@operator);
        Visit(node.Right, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConstantPattern(Cs.ConstantPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.CONSTANT_PATTERN_PREFIX, p);
        Visit(node.Value, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDiscardPattern(Cs.DiscardPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.DISCARD_PATTERN_PREFIX, p);
        p.Append("_");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitListPattern(Cs.ListPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.LIST_PATTERN_PREFIX, p);
        VisitContainer("[", node.Padding.Patterns, CsContainer.Location.LIST_PATTERN_PATTERNS, ",", "]", p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParenthesizedPattern(Cs.ParenthesizedPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.PARENTHESIZED_PATTERN_PREFIX, p);
        VisitContainer("(", node.Padding.Pattern, CsContainer.Location.LIST_PATTERN_PATTERNS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRecursivePattern(Cs.RecursivePattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.RECURSIVE_PATTERN_PREFIX, p);
        Visit(node.TypeQualifier, p);
        Visit(node.PositionalPattern, p);
        Visit(node.PropertyPattern, p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitRelationalPattern(Cs.RelationalPattern node, PrintOutputCapture<TState> p)
    {
        var @operator = node.Operator switch
        {
            Cs.RelationalPattern.OperatorType.LessThan => "<",
            Cs.RelationalPattern.OperatorType.LessThanOrEqual => "<=",
            Cs.RelationalPattern.OperatorType.GreaterThan => ">",
            Cs.RelationalPattern.OperatorType.GreaterThanOrEqual => ">=",
            _ => throw new ArgumentOutOfRangeException()
        };
        BeforeSyntax(node, CsSpace.Location.RELATIONAL_PATTERN_PREFIX, p);
        VisitSpace(node.Padding.Operator.Before, CsSpace.Location.RELATIONAL_PATTERN_OPERATOR, p);
        p.Append(@operator);
        Visit(node.Value, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSlicePattern(Cs.SlicePattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SLICE_PATTERN_PREFIX, p);
        p.Append("..");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypePattern(Cs.TypePattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.TYPE_PATTERN_PREFIX, p);
        Visit(node.TypeIdentifier, p);
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnaryPattern(Cs.UnaryPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.UNARY_PATTERN_PREFIX, p);
        Visit(node.Operator, p);
        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitVarPattern(Cs.VarPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.VAR_PATTERN_PREFIX, p);
        p.Append("var");
        Visit(node.Designation, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPositionalPatternClause(Cs.PositionalPatternClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.POSITIONAL_PATTERN_CLAUSE_PREFIX, p);
        VisitContainer("(", node.Padding.Subpatterns, CsContainer.Location.POSITIONAL_PATTERN_CLAUSE_SUBPATTERNS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPropertyPatternClause(Cs.PropertyPatternClause node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.PROPERTY_PATTERN_CLAUSE_PREFIX, p);
        VisitContainer("{", node.Padding.Subpatterns, CsContainer.Location.PROPERTY_PATTERN_CLAUSE_SUBPATTERNS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIsPattern(Cs.IsPattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.IS_PATTERN_PREFIX, p);
        Visit(node.Expression, p);
        VisitSpace(node.Padding.Pattern.Before, CsSpace.Location.IS_PATTERN_PATTERN, p);
        p.Append("is");
        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSubpattern(Cs.Subpattern node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SUBPATTERN_PREFIX, p);
        if (node.Name != null)
        {
            Visit(node.Name, p);
            VisitSpace(node.Padding.Pattern.Before, CsSpace.Location.SUBPATTERN_PATTERN, p);
            p.Append(":");
        }

        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.DISCARD_VARIABLE_DESIGNATION_PREFIX, p);
        Visit(node.Discard, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSingleVariableDesignation(Cs.SingleVariableDesignation node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.SINGLE_VARIABLE_DESIGNATION_PREFIX, p);
        Visit(node.Name, p);
        AfterSyntax(node, p);
        return node;
    }



    public override J? VisitUsingStatement(Cs.UsingStatement usingStatement, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(usingStatement, CsSpace.Location.NAMED_ARGUMENT_PREFIX, p);
        if (usingStatement.AwaitKeyword != null)
        {
            Visit(usingStatement.AwaitKeyword, p);
        }

        VisitLeftPadded("using", usingStatement.Padding.Expression, CsLeftPadded.Location.USING_STATEMENT_EXPRESSION, p);
        Visit(usingStatement.Statement, p);
        AfterSyntax(usingStatement, p);

        return usingStatement;
    }


    public override J? VisitUnary(Cs.Unary unary, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(unary, Space.Location.UNARY_PREFIX, p);
        switch (unary.Operator)
        {
            case Cs.Unary.Types.FromEnd:
                p.Append("^");
                Visit(unary.Expression, p);
                break;
            case Cs.Unary.Types.PointerIndirection:
                Visit(unary.Expression, p);
                p.Append("*");
                break;
            case Cs.Unary.Types.AddressOf:
                p.Append("&");
                Visit(unary.Expression, p);
                break;
            case Cs.Unary.Types.SuppressNullableWarning:
                Visit(unary.Expression, p);
                p.Append("!");
                break;
        }

        AfterSyntax(unary, p);
        return unary;
    }

    public override Cs VisitCompilationUnit(Cs.CompilationUnit compilationUnit, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(compilationUnit, Space.Location.COMPILATION_UNIT_PREFIX, p);

        foreach (var externAlias in compilationUnit.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in compilationUnit.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.COMPILATION_UNIT_USINGS, p);
            p.Append(';');
        }

        foreach (var attributeList in compilationUnit.AttributeLists)
        {
            Visit(attributeList, p);
        }

        VisitStatements(compilationUnit.Padding.Members, CsRightPadded.Location.COMPILATION_UNIT_MEMBERS, p);
        VisitSpace(compilationUnit.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        AfterSyntax(compilationUnit, p);

        return compilationUnit;
    }

    public override J? VisitClassDeclaration(Cs.ClassDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, Space.Location.CLASS_DECLARATION_PREFIX, p);
        Visit(node.AttributeList, p);
        Visit(node.Modifiers, p);

        Visit(node.Kind, p);
        Visit(node.Name, p);
        VisitContainer("<", node.Padding.TypeParameters, CsContainer.Location.CLASS_DECLARATION_TYPE_PARAMETERS, ",", ">", p);
        VisitContainer("(", node.Padding.PrimaryConstructor, CsContainer.Location.CLASS_DECLARATION_PRIMARY_CONSTRUCTOR, ",", ")", p);
        VisitLeftPadded(":", node.Padding.Extendings, CsLeftPadded.Location.CLASS_DECLARATION_EXTENDINGS, p);
        VisitContainer(node.Padding.Extendings == null ? ":" : ",", node.Padding.Implementings, CsContainer.Location.CLASS_DECLARATION_IMPLEMENTINGS, ",", "", p);
        VisitContainer("", node.Padding.TypeParameterConstraintClauses, CsContainer.Location.CLASS_DECLARATION_TYPE_PARAMETERS, "", "", p);

        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMethodDeclaration(Cs.MethodDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);

        Visit(node.Modifiers, p);

        Visit(node.ReturnTypeExpression, p);
        VisitRightPadded(node.Padding.ExplicitInterfaceSpecifier, CsRightPadded.Location.METHOD_DECLARATION_EXPLICIT_INTERFACE_SPECIFIER, ".", p);
        Visit(node.Name, p);


        VisitContainer("<", node.Padding.TypeParameters, CsContainer.Location.METHOD_DECLARATION_TYPE_PARAMETERS, ",", ">", p);

        if (node.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
        {
            VisitContainer("(", node.Padding.Parameters, CsContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        VisitContainer(node.Padding.TypeParameterConstraintClauses, CsContainer.Location.METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(annotatedStatement, CsSpace.Location.ANNOTATED_STATEMENT_PREFIX, p);

        foreach (var attributeList in annotatedStatement.AttributeLists)
        {
            Visit(attributeList, p);
        }

        Visit(annotatedStatement.Statement, p);
        AfterSyntax(annotatedStatement, p);

        return annotatedStatement;
    }

    public override J? VisitAttributeList(Cs.AttributeList attributeList, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(attributeList, CsSpace.Location.ATTRIBUTE_LIST_PREFIX, p);
        p.Append('[');
        var padding = attributeList.Padding;
        if (padding.Target != null)
        {
            VisitRightPadded(padding.Target, CsRightPadded.Location.ATTRIBUTE_LIST_TARGET, p);
            p.Append(':');
        }

        VisitRightPadded(padding.Attributes, CsRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, ",", p);
        p.Append(']');
        AfterSyntax(attributeList, p);
        return attributeList;
    }

    public override J? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(arrayRankSpecifier, CsSpace.Location.ARRAY_RANK_SPECIFIER_PREFIX, p);
        VisitContainer("", arrayRankSpecifier.Padding.Sizes, CsContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, ",", "", p);
        AfterSyntax(arrayRankSpecifier, p);
        return arrayRankSpecifier;
    }

    public override J? VisitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(assignmentOperation, CsSpace.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        Visit(assignmentOperation.Variable, p);
        VisitLeftPadded(assignmentOperation.Padding.Operator, CsLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        if (assignmentOperation.Operator == Cs.AssignmentOperation.OperatorType.NullCoalescing)
        {
            p.Append("??=");
        }

        Visit(assignmentOperation.Assignment, p);
        AfterSyntax(assignmentOperation, p);
        return assignmentOperation;
    }

    public override J? VisitAwaitExpression(Cs.AwaitExpression awaitExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(awaitExpression, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        p.Append("await");
        Visit(awaitExpression.Expression, p);
        AfterSyntax(awaitExpression, p);
        return awaitExpression;
    }

    public override J? VisitBinary(Cs.Binary binary, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(binary, CsSpace.Location.BINARY_PREFIX, p);
        Visit(binary.Left, p);
        VisitSpace(binary.Padding.Operator.Before, Space.Location.BINARY_OPERATOR, p);
        if (binary.Operator == Cs.Binary.OperatorType.As)
        {
            p.Append("as");
        }
        else if (binary.Operator == Cs.Binary.OperatorType.NullCoalescing)
        {
            p.Append("??");
        }

        Visit(binary.Right, p);
        AfterSyntax(binary, p);
        return binary;
    }

    public override Cs VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<TState> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        VisitRightPadded(namespaceDeclaration.Padding.Name,
            CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.Append('{');

        foreach (var externAlias in namespaceDeclaration.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in namespaceDeclaration.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.Append(';');
        }

        VisitStatements(namespaceDeclaration.Padding.Members,
            CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        VisitSpace(namespaceDeclaration.End, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p);
        p.Append('}');
        AfterSyntax(namespaceDeclaration, p);
        return namespaceDeclaration;
    }

    public override J? VisitCollectionExpression(Cs.CollectionExpression collectionExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(collectionExpression, CsSpace.Location.COLLECTION_EXPRESSION_PREFIX, p);
        p.Append('[');
        VisitRightPadded(collectionExpression.Padding.Elements, CsRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS,
            ",", p);
        p.Append(']');
        AfterSyntax(collectionExpression, p);
        return collectionExpression;
    }

    public override J? VisitEnumDeclaration(Cs.EnumDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.ENUM_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Modifiers, p);
        VisitLeftPadded("enum", node.Padding.Name, CsLeftPadded.Location.ENUM_DECLARATION_NAME, p);
        if (node.BaseType != null)
        {
            VisitLeftPadded(":", node.Padding.BaseType, CsLeftPadded.Location.ENUM_DECLARATION_BASE_TYPE, p);
        }

        VisitContainer("{", node.Padding.Members, CsContainer.Location.ENUM_DECLARATION_MEMBERS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitExpressionStatement(Cs.ExpressionStatement expressionStatement, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(expressionStatement, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        VisitRightPadded(expressionStatement.Padding.Expression, CsRightPadded.Location.EXPRESSION_STATEMENT_EXPRESSION, p);
        p.Append(";");
        AfterSyntax(expressionStatement, p);
        return expressionStatement;
    }

    public override J? VisitExternAlias(Cs.ExternAlias externAlias, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(externAlias, CsSpace.Location.EXTERN_ALIAS_PREFIX, p);
        p.Append("extern");
        VisitLeftPadded("alias", externAlias.Padding.Identifier, CsLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p);
        AfterSyntax(externAlias, p);
        return externAlias;
    }

    public override Cs VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<TState> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        VisitRightPadded(namespaceDeclaration.Padding.Name,
            CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.Append(";");

        foreach (var externAlias in namespaceDeclaration.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in namespaceDeclaration.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.Append(';');
        }

        VisitStatements(namespaceDeclaration.Padding.Members, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        return namespaceDeclaration;
    }

    public override J? VisitInterpolatedString(Cs.InterpolatedString interpolatedString, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(interpolatedString, CsSpace.Location.INTERPOLATED_STRING_PREFIX, p);
        p.Append(interpolatedString.Start);
        VisitRightPadded(interpolatedString.Padding.Parts, CsRightPadded.Location.INTERPOLATED_STRING_PARTS, "", p);
        p.Append(interpolatedString.End);
        AfterSyntax(interpolatedString, p);
        return interpolatedString;
    }


    public override J? VisitInterpolation(Cs.Interpolation interpolation, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(interpolation, CsSpace.Location.INTERPOLATION_PREFIX, p);
        p.Append('{');
        VisitRightPadded(interpolation.Padding.Expression, CsRightPadded.Location.INTERPOLATION_EXPRESSION, p);

        if (interpolation.Alignment != null)
        {
            p.Append(',');
            VisitRightPadded(interpolation.Padding.Alignment, CsRightPadded.Location.INTERPOLATION_ALIGNMENT, p);
        }

        if (interpolation.Format != null)
        {
            p.Append(':');
            VisitRightPadded(interpolation.Padding.Format, CsRightPadded.Location.INTERPOLATION_FORMAT, p);
        }

        p.Append('}');
        AfterSyntax(interpolation, p);
        return interpolation;
    }


    public override J? VisitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(nullSafeExpression, CsSpace.Location.NULL_SAFE_EXPRESSION_PREFIX, p);
        VisitRightPadded(nullSafeExpression.Padding.Expression, CsRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION,
            p);
        p.Append("?");
        AfterSyntax(nullSafeExpression, p);
        return nullSafeExpression;
    }

    public override J? VisitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(propertyDeclaration, CsSpace.Location.PROPERTY_DECLARATION_PREFIX, p);
        Visit(propertyDeclaration.AttributeLists, p);

        foreach (var m in propertyDeclaration.Modifiers)
        {
            _delegate.VisitModifier(m, p);
        }

        Visit(propertyDeclaration.TypeExpression, p);

        if (propertyDeclaration.Padding.InterfaceSpecifier != null)
        {
            VisitRightPadded(propertyDeclaration.Padding.InterfaceSpecifier,
                CsRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, p);
            p.Append('.');
        }

        Visit(propertyDeclaration.Name, p);
        Visit(propertyDeclaration.Accessors, p);

        if (propertyDeclaration.Initializer != null)
        {
            VisitLeftPadded("=", propertyDeclaration.Padding.Initializer,
                CsLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p);
        }

        AfterSyntax(propertyDeclaration, p);
        return propertyDeclaration;
    }

    public override J? VisitUsingDirective(Cs.UsingDirective usingDirective, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(usingDirective, CsSpace.Location.USING_DIRECTIVE_PREFIX, p);

        if (usingDirective.Global)
        {
            p.Append("global");
            VisitRightPadded(usingDirective.Padding.Global, CsRightPadded.Location.USING_DIRECTIVE_GLOBAL, p);
        }

        p.Append("using");

        if (usingDirective.Static)
        {
            VisitLeftPadded(usingDirective.Padding.Static, CsLeftPadded.Location.USING_DIRECTIVE_STATIC, p);
            p.Append("static");
        }
        else if (usingDirective.Alias != null)
        {
            if (usingDirective.Unsafe)
            {
                VisitLeftPadded(usingDirective.Padding.Unsafe, CsLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p);
                p.Append("unsafe");
            }

            VisitRightPadded(usingDirective.Padding.Alias, CsRightPadded.Location.USING_DIRECTIVE_ALIAS, p);
            p.Append('=');
        }

        Visit(usingDirective.NamespaceOrType, p);
        AfterSyntax(usingDirective, p);
        return usingDirective;
    }

    public override J? VisitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.CONVERSION_OPERATOR_DECLARATION_PREFIX, p);
        foreach (var modifier in node.Modifiers)
        {
            Visit(modifier, p);
        }

        VisitLeftPadded(node.Padding.Kind, CsLeftPadded.Location.CONVERSION_OPERATOR_DECLARATION_KIND, p);
        p.Append(node.Kind.ToString().ToLower());
        VisitLeftPadded("operator", node.Padding.ReturnType, CsLeftPadded.Location.CONVERSION_OPERATOR_DECLARATION_RETURN_TYPE, p);
        VisitContainer("(", node.Padding.Parameters, CsContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ")", p);
        Visit(node.ExpressionBody, p);
        Visit(node.Body, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEnumMemberDeclaration(Cs.EnumMemberDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.ENUM_MEMBER_DECLARATION_PREFIX, p);
        Visit(node.AttributeLists, p);
        Visit(node.Name, p);
        VisitLeftPadded("=", node.Padding.Initializer, CsLeftPadded.Location.ENUM_MEMBER_DECLARATION_INITIALIZER, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIndexerDeclaration(Cs.IndexerDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.INDEXER_DECLARATION_PREFIX, p);
        foreach (var modifier in node.Modifiers)
        {
            Visit(modifier, p);
        }

        Visit(node.TypeExpression, p);
        Visit(node.Indexer, p);
        VisitContainer("[", node.Padding.Parameters, CsContainer.Location.INDEXER_DECLARATION_PARAMETERS, ",", "]", p);
        VisitLeftPadded("", node.Padding.ExpressionBody, CsLeftPadded.Location.INDEXER_DECLARATION_EXPRESSION_BODY, p); //todo: probably should be => as inner block is just wrong representation
        Visit(node.Accessors, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDelegateDeclaration(Cs.DelegateDeclaration node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, CsSpace.Location.DELEGATE_DECLARATION_PREFIX, p);
        foreach (var modifier in node.Modifiers)
        {
            Visit(modifier, p);
        }

        VisitLeftPadded("delegate", node.Padding.ReturnType, CsLeftPadded.Location.DELEGATE_DECLARATION_RETURN_TYPE, p);
        Visit(node.Identifier, p);
        VisitContainer("<", node.Padding.TypeParameters, CsContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ">", p);
        VisitContainer("(", node.Padding.Parameters, CsContainer.Location.CONVERSION_OPERATOR_DECLARATION_PARAMETERS, ",", ")", p);
        VisitContainer(node.Padding.TypeParameterConstraintClauses, CsContainer.Location.DELEGATE_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p);

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDestructorDeclaration(Cs.DestructorDeclaration node, PrintOutputCapture<TState> p)
    {
        var method = node.MethodCore;
        BeforeSyntax(method, CsSpace.Location.DESTRUCTOR_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(method.LeadingAnnotations, p);
        foreach (var modifier in method.Modifiers)
        {
            _delegate.Visit(modifier, p);
        }

        Visit(method.Annotations.Name.Annotations, p);
        p.Append("~");
        Visit(method.Name, p);

        VisitContainer("(", method.Padding.Parameters, CsContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);

        Visit(method.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitConstructor(Cs.Constructor constructor, PrintOutputCapture<TState> p)
    {
        var method = constructor.ConstructorCore;
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
            VisitContainer("(", method.Padding.Parameters, CsContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        Visit(constructor.Initializer, p);

        Visit(method.Body, p);
        AfterSyntax(constructor, p);
        return constructor;
    }

    public override J? VisitConstructorInitializer(Cs.ConstructorInitializer node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        p.Append(":");
        Visit(node.Keyword, p);
        VisitContainer("(", node.Padding.Arguments, CsContainer.Location.CONSTRUCTOR_INITIALIZER_ARGUMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override Cs VisitLambda(Cs.Lambda lambda, PrintOutputCapture<TState> p)
    {
        if (lambda.Modifiers.Any()) // only put space in front if current node actually has anything to contribute
        {
            BeforeSyntax(lambda, Space.Location.LAMBDA_PREFIX, p);
        }

        var javaLambda = lambda.LambdaExpression;

        VisitSpace(lambda.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
        VisitMarkers(lambda.Markers, p);
        foreach (var modifier in lambda.Modifiers)
        {
            _delegate.VisitModifier(modifier, p);
        }

        Visit(javaLambda, p);
        AfterSyntax(lambda, p);
        return lambda;
    }

    protected override Space VisitSpace(Space space, CsSpace.Location loc, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }

    public override Space VisitSpace(Space space, Space.Location? loc, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitSpace(space, loc, p);
    }

    protected void VisitLeftPaddedEnum<T>(JLeftPadded<T>? leftPadded, CsLeftPadded.Location location, PrintOutputCapture<TState> p) where T : Enum
    {
        if (leftPadded == null)
            return;
        VisitLeftPadded(leftPadded, location, p);
        p.Append(leftPadded.Element.ToString().ToLower());
    }
    protected void VisitLeftPadded<T>(string prefix, JLeftPadded<T>? leftPadded, CsLeftPadded.Location location, PrintOutputCapture<TState> p) where T : J
    {
        if (leftPadded != null)
        {
            BeforeSyntax(leftPadded.Before, leftPadded.Markers, location.BeforeLocation, p);

            if (prefix != null)
            {
                p.Append(prefix);
            }

            Visit(leftPadded.Element, p);
            AfterSyntax(leftPadded.Markers, p);
        }
    }

    protected void VisitContainer<T>(string before, JContainer<T>? container, CsContainer.Location location, string suffixBetween, string after, PrintOutputCapture<TState> p) where T : J
    {
        if (container == null)
        {
            return;
        }

        VisitSpace(container.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitRightPadded(container.Padding.Elements, location.ElementLocation, suffixBetween, p);
        p.Append(after);
    }

    protected void VisitRightPadded<T>(IList<JRightPadded<T>> nodes, CsRightPadded.Location location, string suffixBetween, PrintOutputCapture<TState> p) where T : J
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            Visit(node.Element, p);
            VisitSpace(node.After, location.AfterLocation, p);
            VisitMarkers(node.Markers, p);

            if (i < nodes.Count - 1)
            {
                p.Append(suffixBetween);
            }
        }
    }

    protected void VisitStatements(string before, JContainer<Statement>? container, CsContainer.Location location, string after, PrintOutputCapture<TState> p)
    {
        if (container == null)
        {
            return;
        }

        VisitSpace(container.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitStatements(container.Padding.Elements, location.ElementLocation, p);
        p.Append(after);
    }

    protected void VisitStatements(IList<JRightPadded<Statement>> statements, CsRightPadded.Location location, PrintOutputCapture<TState> p)
    {
        foreach (var paddedStat in statements)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement(JRightPadded<Statement>? paddedStat, CsRightPadded.Location location, PrintOutputCapture<TState> p)
    {
        if (paddedStat == null)
        {
            return;
        }

        Visit(paddedStat.Element, p);
        VisitSpace(paddedStat.After, location.AfterLocation, p);
        VisitMarkers(paddedStat.Markers, p);

        if (Cursor.Parent?.Value is J.Block && Cursor.Parent?.Parent?.Value is J.NewClass)
        {
            p.Append(',');
            return;
        }

        _delegate.PrintStatementTerminator(paddedStat.Element, p);
    }

    public override J? VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(typeParameterConstraintClause, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_CLAUSE_PREFIX, p);
        p.Append("where");
        VisitRightPadded(typeParameterConstraintClause.Padding.TypeParameter, CsRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER,  p);
        p.Append(":");
        VisitContainer("", typeParameterConstraintClause.Padding.TypeParameterConstraints, CsContainer.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS, ",", "", p);
        AfterSyntax(typeParameterConstraintClause, p);
        return typeParameterConstraintClause;
    }

    public override J? VisitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(classOrStructConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.Append(classOrStructConstraint.Kind == Cs.ClassOrStructConstraint.TypeKind.Class ? "class" : "struct");
        return classOrStructConstraint;
    }

    public override J? VisitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(constructorConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.Append("new()");
        return constructorConstraint;
    }

    public override J? VisitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(defaultConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.Append("default");
        return defaultConstraint;
    }

    public override J? VisitInitializerExpression(Cs.InitializerExpression node, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(node, Space.Location.BLOCK_PREFIX, p);
        VisitContainer("{", node.Padding.Expressions, CsContainer.Location.INITIALIZER_EXPRESSION_EXPRESSIONS, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }


    private class CSharpJavaPrinter(CSharpPrinter<TState> _parent) : JavaPrinter<TState>
    {
        public override J? PreVisit(Core.Tree? tree, PrintOutputCapture<TState> p)
        {
            return _parent.PreVisit(tree, p);
        }

        public override J? PostVisit(Core.Tree tree, PrintOutputCapture<TState> p)
        {
            return _parent.PostVisit(tree, p);
        }
#if DEBUG_VISITOR
        [DebuggerStepThrough]
#endif
        public override J? Visit(Rewrite.Core.Tree? tree, PrintOutputCapture<TState> p)
        {
            if (tree is Cs)
            {
                // Re-route printing back up to C#
                return _parent.Visit(tree, p);
            }
            else if(tree is null or J)
            {
                return base.Visit(tree, p);
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

            return base.Visit(tree, p);
        }

        public override J VisitNewArray(J.NewArray newArray, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(newArray, Space.Location.NEW_ARRAY_PREFIX, p);
            p.Append("new");

            Visit(newArray.TypeExpression, p);
            // VisitArrayDimension()
            Visit(newArray.Dimensions, p);
            VisitContainer("{", newArray.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
            AfterSyntax(newArray, p);
            return newArray;
        }



        public override J? VisitClassDeclarationKind(J.ClassDeclaration.Kind kind, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(kind, Space.Location.CLASS_KIND, p);
            string kindStr = kind.KindType switch
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
            AfterSyntax(kind, p);
            return kind;
        }

        public override J VisitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<TState> p)
        {
            var csClassDeclaration = _parent.Cursor.Value as Cs.ClassDeclaration;

            BeforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(classDecl.LeadingAnnotations, p);
            foreach (var modifier in classDecl.Modifiers)
            {
                Visit(modifier, p);
            }

            Visit(classDecl.Padding.DeclarationKind.Annotations, p);
            Visit(classDecl.Padding.DeclarationKind, p);
            Visit(classDecl.Name, p);
            VisitContainer("<", classDecl.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", classDecl.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
            VisitLeftPadded(":", classDecl.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
            VisitContainer(classDecl.Padding.Extends == null ? ":" : ",", classDecl.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
            foreach (var typeParameterClause in csClassDeclaration?.TypeParameterConstraintClauses ?? [])
            {
                _parent.Visit(typeParameterClause, p);
            }
            Visit(classDecl.Body, p);
            AfterSyntax(classDecl, p);
            return classDecl;
        }

        public override J VisitAnnotation(J.Annotation annotation, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(annotation, Space.Location.ANNOTATION_PREFIX, p);
            Visit(annotation.AnnotationType, p);
            VisitContainer("(", annotation.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(annotation, p);
            return annotation;
        }

        public override J VisitBlock(J.Block block, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(block, Space.Location.BLOCK_PREFIX, p);

            if (block.Static)
            {
                p.Append("static");
                VisitRightPadded(block.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
            }

            if (block.Markers.FirstOrDefault(m => m is SingleExpressionBlock) != null)
            {
                p.Append("=>");
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);

                VisitSpace(block.End, Space.Location.BLOCK_END, p);
            }
            else if (!block.Markers.OfType<OmitBraces>().Any() || block.Statements.Any())
            {
                p.Append('{');
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(block.End, Space.Location.BLOCK_END, p);
                p.Append('}');
            }
            else
            {
                if (block.Padding.Statements.Any())
                {
                    VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                }
                else
                {
                    p.Append(";");
                }

                VisitSpace(block.End, Space.Location.BLOCK_END, p);
            }

            AfterSyntax(block, p);
            return block;
        }


        protected override void VisitStatements(IList<JRightPadded<Statement>> statements, JRightPadded.Location location, PrintOutputCapture<TState> p)
        {

            for (int i = 0; i < statements.Count; i++)
            {
                var paddedStat = statements[i];
                VisitStatement(paddedStat, location, p);
                if (i < statements.Count - 1 &&
                    (Cursor.Parent?.Value is J.NewClass ||
                     (Cursor.Parent?.Value is J.Block &&
                      Cursor.GetParent(2)?.Value is J.NewClass)))
                {
                    p.Append(',');
                }
            }
        }

        public override J VisitMethodDeclaration(J.MethodDeclaration method, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(method.LeadingAnnotations, p);
            foreach (var modifier in method.Modifiers)
            {
                Visit(modifier, p);
            }

            Visit(method.ReturnTypeExpression, p);
            Visit(method.Annotations.Name.Annotations, p);
            Visit(method.Name, p);

            var typeParameters = method.Annotations.TypeParameters;
            if (typeParameters != null)
            {
                Visit(typeParameters.Annotations, p);
                VisitSpace(typeParameters.Prefix, Space.Location.TYPE_PARAMETERS, p);
                VisitMarkers(typeParameters.Markers, p);
                p.Append('<');
                VisitRightPadded(typeParameters.Padding.Parameters, JRightPadded.Location.TYPE_PARAMETER, ",", p);
                p.Append('>');
            }

            if (method.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
            {
                VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",",
                    ")", p);
            }

            var csMethod = _parent.Cursor.GetValue<Cs.MethodDeclaration>();
            if (csMethod != null)
            {
                Visit(csMethod.TypeParameterConstraintClauses, p);
            }

            VisitContainer("throws", method.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
            Visit(method.Body, p);
            VisitLeftPadded("default", method.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
            AfterSyntax(method, p);
            return method;
        }

        public override J VisitMethodInvocation(J.MethodInvocation method, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(method, Space.Location.METHOD_INVOCATION_PREFIX, p);
            var prefix = method.Name.SimpleName != "" ? "." : "";
            VisitRightPadded(method.Padding.Select, JRightPadded.Location.METHOD_SELECT, prefix, p);
            Visit(method.Name, p);
            VisitContainer("<", method.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", method.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(method, p);
            return method;
        }

        public override J VisitCatch(J.Try.Catch catch_, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(catch_, Space.Location.CATCH_PREFIX, p);
            p.Append("catch");
            if (catch_.Parameter.Tree.TypeExpression != null)
            {
                Visit(catch_.Parameter, p);
            }

            Visit(catch_.Body, p);
            AfterSyntax(catch_, p);
            return catch_;
        }

        public override J VisitForEachLoop(J.ForEachLoop forEachLoop, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
            p.Append("foreach");
            var ctrl = forEachLoop.LoopControl;
            VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
            p.Append('(');
            VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, "in", p);
            VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, "", p);
            p.Append(')');
            VisitStatement(forEachLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p);
            AfterSyntax(forEachLoop, p);
            return forEachLoop;
        }

        public override J VisitInstanceOf(J.InstanceOf instanceOf, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(instanceOf, Space.Location.INSTANCEOF_PREFIX, p);
            VisitRightPadded(instanceOf.Padding.Expression, JRightPadded.Location.INSTANCEOF, "is", p);
            Visit(instanceOf.Clazz, p);
            Visit(instanceOf.Pattern, p);
            AfterSyntax(instanceOf, p);
            return instanceOf;
        }

        public override J VisitLambda(J.Lambda lambda, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(lambda, Space.Location.LAMBDA_PREFIX, p);
            VisitSpace(lambda.Params.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
            VisitMarkers(lambda.Params.Markers, p);

            if (lambda.Params.Parenthesized)
            {
                p.Append('(');
                VisitRightPadded(lambda.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
                p.Append(')');
            }
            else
            {
                VisitRightPadded(lambda.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
            }

            VisitSpace(lambda.Arrow, Space.Location.LAMBDA_ARROW_PREFIX, p);
            p.Append("=>");
            Visit(lambda.Body, p);
            AfterSyntax(lambda, p);
            return lambda;
        }

        public override J VisitPrimitive(J.Primitive primitive, PrintOutputCapture<TState> p)
        {
            string keyword = primitive.Type.Kind switch
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

            BeforeSyntax(primitive, Space.Location.PRIMITIVE_PREFIX, p);
            p.Append(keyword);
            AfterSyntax(primitive, p);
            return primitive;
        }

        public override M VisitMarker<M>(M marker, PrintOutputCapture<TState> p)
        {
            if (marker is Semicolon)
            {
                p.Append(';');
            }
            else if (marker is TrailingComma trailingComma)
            {
                p.Append(',');
                VisitSpace(trailingComma.Suffix, Space.Location.LANGUAGE_EXTENSION, p);
            }

            return (M)marker;
        }

        // override print
        public override void PrintStatementTerminator(Statement s, PrintOutputCapture<TState> p)
        {
            if (s is
                Cs.ExpressionStatement or
                Cs.AwaitExpression { Expression: J.ForEachLoop { Body: J.Block } })
            {
                return;
            }

            if (s is
                Cs.AssignmentOperation or
                Cs.Yield or
                Cs.DelegateDeclaration or
                Cs.UsingStatement { Statement: not J.Block and not Cs.UsingStatement and not Cs.ExpressionStatement } or
                Cs.AwaitExpression { Expression: not J.ForEachLoop { Body: not J.Block}} or
                Cs.PropertyDeclaration { Initializer: not null }
                )
            {
                p.Append(';');
            }
            else if(Cursor.Parent?.Value is not Cs.ExpressionStatement)
            {
                base.PrintStatementTerminator(s, p);
            }
        }
    }

    public override Markers VisitMarkers(Markers? markers, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitMarkers(markers, p);
    }

    private static readonly Func<string, string> JAVA_MARKER_WRAPPER =
        o => "/*~~" + o + (string.IsNullOrEmpty(o) ? "" : "~~") + ">*/";

    private void BeforeSyntax(J cs, Space.Location loc, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(J cs, CsSpace.Location loc, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(Space prefix, Markers markers, CsSpace.Location? loc, PrintOutputCapture<TState> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc, p);
        }

        VisitMarkers(markers, p);

        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }

    private void BeforeSyntax(Space prefix, Markers markers, Space.Location? loc, PrintOutputCapture<TState> p)
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


    private void AfterSyntax(Cs g, PrintOutputCapture<TState> p)
    {
        AfterSyntax(g.Markers, p);
    }

    private void AfterSyntax(Markers markers, PrintOutputCapture<TState> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }
}
