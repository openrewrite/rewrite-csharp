/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.openrewrite.csharp;

import org.jspecify.annotations.NonNull;
import org.jspecify.annotations.Nullable;
import org.openrewrite.Cursor;
import org.openrewrite.PrintOutputCapture;
import org.openrewrite.Tree;
import org.openrewrite.csharp.marker.OmitBraces;
import org.openrewrite.csharp.marker.SingleExpressionBlock;
import org.openrewrite.csharp.tree.*;
import org.openrewrite.java.JavaPrinter;
import org.openrewrite.java.marker.CompactConstructor;
import org.openrewrite.java.marker.Semicolon;
import org.openrewrite.java.marker.TrailingComma;
import org.openrewrite.java.tree.*;
import org.openrewrite.marker.Marker;
import org.openrewrite.marker.Markers;

import java.util.List;
import java.util.function.UnaryOperator;

public class CSharpPrinter<P> extends CSharpVisitor<PrintOutputCapture<P>> {
    private final CSharpJavaPrinter delegate = new CSharpJavaPrinter();

    @Override
    public J visit(@Nullable Tree tree, PrintOutputCapture<P> p) {
        if (!(tree instanceof Cs)) {
            // re-route printing to the java printer
            return delegate.visit(tree, p);
        } else {
            return super.visit(tree, p);
        }
    }

    public J visitForEachVariableLoop(Cs.ForEachVariableLoop forEachLoop, PrintOutputCapture<P> p) {
        beforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
        p.append("foreach");
        Cs.ForEachVariableLoop.Control ctrl = forEachLoop.getControlElement();
        visitSpace(ctrl.getPrefix(), Space.Location.FOR_EACH_CONTROL_PREFIX, p);
        p.append('(');
        visitRightPadded(ctrl.getPadding().getVariable(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_VARIABLE, "in", p);
        visitRightPadded(ctrl.getPadding().getIterable(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_ITERABLE, "", p);
        p.append(')');
        visitStatement(forEachLoop.getPadding().getBody(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_BODY, p);
        afterSyntax(forEachLoop, p);
        return forEachLoop;
    }

    protected <T extends J> void visitRightPadded(JRightPadded<T> rightPadded, CsRightPadded.Location location, String suffix, PrintOutputCapture<P> p) {
        if (rightPadded != null) {
            beforeSyntax(Space.EMPTY, rightPadded.getMarkers(), (CsSpace.Location) null, p);
            visit(rightPadded.getElement(), p);
            afterSyntax(rightPadded.getMarkers(), p);
            visitSpace(rightPadded.getAfter(), location.getAfterLocation(), p);
            if (suffix != null) {
                p.append(suffix);
            }
        }
    }

    public J visitSwitchExpression(Cs.SwitchExpression node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SWITCH_EXPRESSION_PREFIX, p);
        visitRightPadded(node.getPadding().getExpression(), CsRightPadded.Location.SWITCH_EXPRESSION_EXPRESSION, p);
        p.append("switch");
        visitContainer("{", node.getPadding().getArms(), CsContainer.Location.SWITCH_EXPRESSION_ARMS, ",", "}", p);
        afterSyntax(node, p);
        return node;
    }

    public J visitSwitchStatement(Cs.SwitchStatement node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SWITCH_STATEMENT_PREFIX, p);
        p.append("switch");
        visitContainer("(", node.getPadding().getExpression(), CsContainer.Location.SWITCH_STATEMENT_EXPRESSION, ",", ")", p);
        visitContainer("{", node.getPadding().getSections(), CsContainer.Location.SWITCH_STATEMENT_SECTIONS, "", "}", p);
        afterSyntax(node, p);
        return node;
    }

    public J visitSwitchSection(Cs.SwitchSection node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SWITCH_SECTION_PREFIX, p);
        visit(node.getLabels(), p);
        visitStatements(node.getPadding().getStatements(), CsRightPadded.Location.SWITCH_SECTION_STATEMENTS, p);
        afterSyntax(node, p);
        return node;
    }

    public J visitUnsafeStatement(Cs.UnsafeStatement node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.UNSAFE_STATEMENT_PREFIX, p);
        p.append("unsafe");
        visit(node.getBlock(), p);
        afterSyntax(node, p);
        return node;
    }

    public J visitCheckedStatement(Cs.CheckedStatement node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.CHECKED_STATEMENT_PREFIX, p);
        p.append("checked");
        visit(node.getBlock(), p);
        afterSyntax(node, p);
        return node;
    }

    public J visitRangeExpression(Cs.RangeExpression node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.RANGE_EXPRESSION_PREFIX, p);
        visitRightPadded(node.getPadding().getStart(), CsRightPadded.Location.RANGE_EXPRESSION_START, p);
        p.append("..");
        visit(node.getEnd(), p);
        afterSyntax(node, p);
        return node;
    }

    public J visitFixedStatement(Cs.FixedStatement node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.FIXED_STATEMENT_PREFIX, p);
        p.append("fixed");
        visit(node.getDeclarations(), p);
        visit(node.getBlock(), p);
        afterSyntax(node, p);
        return node;
    }

    public J visitLockStatement(Cs.LockStatement node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.LOCK_STATEMENT_PREFIX, p);
        p.append("lock");
        visit(node.getExpression(), p);
        visitStatement(node.getPadding().getStatement(), CsRightPadded.Location.LOCK_STATEMENT_STATEMENT, p);
        afterSyntax(node, p);
        return node;
    }

    public J visitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_PREFIX, p);
        p.append("case");
        visit(node.getPattern(), p);
        visitLeftPadded("when", node.getPadding().getWhenClause(), CsLeftPadded.Location.CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE, p);
        visitSpace(node.getColonToken(), CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.append(":");
        afterSyntax(node, p);
        return node;
    }

    public J visitDefaultSwitchLabel(Cs.DefaultSwitchLabel node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.DEFAULT_SWITCH_LABEL_PREFIX, p);
        p.append("default");
        visitSpace(node.getColonToken(), CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p);
        p.append(":");
        afterSyntax(node, p);
        return node;
    }

    public J visitSwitchExpressionArm(Cs.SwitchExpressionArm node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SWITCH_EXPRESSION_ARM_PREFIX, p);
        visit(node.getPattern(), p);
        visitLeftPadded("when", node.getPadding().getWhenExpression(), CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION, p);
        visitLeftPadded("=>", node.getPadding().getExpression(), CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_EXPRESSION, p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitDefaultExpression(Cs.DefaultExpression node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.YIELD_PREFIX, p);
        p.append("default");
        if (node.getTypeOperator() != null) {
            visitContainer("(", node.getPadding().getTypeOperator(), CsContainer.Location.DEFAULT_EXPRESSION_TYPE_OPERATOR, "", ")", p);
        }
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitYield(Cs.Yield node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.YIELD_PREFIX, p);
        p.append("yield");
        visitKeyword(node.getReturnOrBreakKeyword(), p);
        visit(node.getExpression(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitImplicitElementAccess(Cs.ImplicitElementAccess node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.IMPLICIT_ELEMENT_ACCESS_PREFIX, p);
        visitContainer("[", node.getPadding().getArgumentList(), CsContainer.Location.IMPLICIT_ELEMENT_ACCESS_ARGUMENT_LIST, ",", "]", p);
        afterSyntax(node, p);
        return node;
    }



    @Override
    public Cs visitTupleExpression(Cs.TupleExpression tupleExpression, PrintOutputCapture<P> p)
    {
        beforeSyntax(tupleExpression, CsSpace.Location.TUPLE_EXPRESSION_PREFIX, p);
        visitContainer("(", tupleExpression.getPadding().getArguments(), CsContainer.Location.TUPLE_EXPRESSION_ARGUMENTS, ",", ")", p);
        afterSyntax(tupleExpression, p);
        return tupleExpression;
    }

    @Override
    public Cs visitTupleType(Cs.TupleType node, PrintOutputCapture<P> p)
    {
        beforeSyntax(node, CsSpace.Location.TUPLE_TYPE_PREFIX, p);
        visitContainer("(", node.getPadding().getElements(), CsContainer.Location.TUPLE_TYPE_ELEMENTS, ",", ")", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public Cs visitTupleElement(Cs.TupleElement node, PrintOutputCapture<P> p)
    {
        beforeSyntax(node, CsSpace.Location.TUPLE_ELEMENT_PREFIX, p);
        visit(node.getType(), p);
        if (node.getName() != null)
        {
            visit(node.getName(), p);
        }
        afterSyntax(node, p);
        return node;
    }

    @Override
    public Cs visitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation node, PrintOutputCapture<P> p)
    {
        beforeSyntax(node, CsSpace.Location.PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES, p);
        visitContainer("(", node.getPadding().getVariables(), CsContainer.Location.PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES, ",", ")", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitArgument(Cs.Argument argument, PrintOutputCapture<P> p) {
        beforeSyntax(argument, CsSpace.Location.ARGUMENT_PREFIX, p);
        Cs.Argument.Padding padding = argument.getPadding();

        if (argument.getNameColumn() != null) {
            visitRightPadded(padding.getNameColumn(), CsRightPadded.Location.ARGUMENT_NAME_COLUMN, p);
            p.append(':');
        }
        if (argument.getRefKindKeyword() != null)
        {
            visit(argument.getRefKindKeyword(), p);
        }
        visit(argument.getExpression(), p);
        afterSyntax(argument, p);
        return argument;
    }

    @Override
    public Cs visitKeyword(Cs.Keyword keyword, PrintOutputCapture<P> p)
    {
        beforeSyntax(keyword, CsSpace.Location.KEYWORD_PREFIX, p);
        p.append(keyword.getKind().toString().toLowerCase());
        afterSyntax(keyword, p);
        return keyword;
    }

    @Override
    public J visitBinaryPattern(Cs.BinaryPattern node, PrintOutputCapture<P> p) {
        String operator;

        switch (node.getOperator()) {
            case And:
                operator = "and";
                break;
            case Or:
                operator = "or";
                break;
            default:
                throw new IllegalArgumentException();
        };
        beforeSyntax(node, CsSpace.Location.BINARY_PATTERN_PREFIX, p);
        visit(node.getLeft(), p);
        visitSpace(node.getPadding().getOperator().getBefore(), CsSpace.Location.BINARY_PATTERN_OPERATOR, p);
        p.append(operator);
        visit(node.getRight(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitConstantPattern(Cs.ConstantPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.CONSTANT_PATTERN_PREFIX, p);
        visit(node.getValue(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitDiscardPattern(Cs.DiscardPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.DISCARD_PATTERN_PREFIX, p);
        p.append("_");
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitListPattern(Cs.ListPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.LIST_PATTERN_PREFIX, p);
        visitContainer("[", node.getPadding().getPatterns(), CsContainer.Location.LIST_PATTERN_PATTERNS, ",", "]", p);
        visit(node.getDesignation(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitParenthesizedPattern(Cs.ParenthesizedPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.PARENTHESIZED_PATTERN_PREFIX, p);
        visitContainer("(", node.getPadding().getPattern(), CsContainer.Location.PARENTHESIZED_PATTERN_PREFIX, ",", ")", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitRecursivePattern(Cs.RecursivePattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.RECURSIVE_PATTERN_PREFIX, p);
        visit(node.getTypeQualifier(), p);
        visit(node.getPositionalPattern(), p);
        visit(node.getPropertyPattern(), p);
        visit(node.getDesignation(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitRelationalPattern(Cs.RelationalPattern node, PrintOutputCapture<P> p) {
        String operator;
        switch (node.getOperator()) {
            case LessThan:
                operator = "<";
                break;
            case LessThanOrEqual:
                operator = "<=";
                break;
            case GreaterThan:
                operator = ">";
                break;
            case GreaterThanOrEqual:
                operator = ">=";
                break;
            default:
                throw new IllegalArgumentException();
        };
        beforeSyntax(node, CsSpace.Location.RELATIONAL_PATTERN_PREFIX, p);
        visitSpace(node.getPadding().getOperator().getBefore(), CsSpace.Location.RELATIONAL_PATTERN_OPERATOR, p);
        p.append(operator);
        visit(node.getValue(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitSlicePattern(Cs.SlicePattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SLICE_PATTERN_PREFIX, p);
        p.append("..");
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitTypePattern(Cs.TypePattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.TYPE_PATTERN_PREFIX, p);
        visit(node.getTypeIdentifier(), p);
        visit(node.getDesignation(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitUnaryPattern(Cs.UnaryPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.UNARY_PATTERN_PREFIX, p);
        visit(node.getOperator(), p);
        visit(node.getPattern(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitVarPattern(Cs.VarPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.VAR_PATTERN_PREFIX, p);
        p.append("var");
        visit(node.getDesignation(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitPositionalPatternClause(Cs.PositionalPatternClause node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.POSITIONAL_PATTERN_CLAUSE_PREFIX, p);
        visitContainer("(", node.getPadding().getSubpatterns(), CsContainer.Location.POSITIONAL_PATTERN_CLAUSE_SUBPATTERNS, ",", ")", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitPropertyPatternClause(Cs.PropertyPatternClause node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.PROPERTY_PATTERN_CLAUSE_PREFIX, p);
        visitContainer("{", node.getPadding().getSubpatterns(), CsContainer.Location.PROPERTY_PATTERN_CLAUSE_SUBPATTERNS, ",", "}", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitIsPattern(Cs.IsPattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.IS_PATTERN_PREFIX, p);
        visit(node.getExpression(), p);
        visitSpace(node.getPadding().getPattern().getBefore(), CsSpace.Location.IS_PATTERN_PATTERN, p);
        p.append("is");
        visit(node.getPattern(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitSubpattern(Cs.Subpattern node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SUBPATTERN_PREFIX, p);
        if (node.getName() != null) {
            visit(node.getName(), p);
            visitSpace(node.getPadding().getPattern().getBefore(), CsSpace.Location.SUBPATTERN_PATTERN, p);
            p.append(":");
        }
        visit(node.getPattern(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitDiscardVariableDesignation(Cs.DiscardVariableDesignation node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.IS_PATTERN_PREFIX, p);
        visit(node.getDiscard(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitSingleVariableDesignation(Cs.SingleVariableDesignation node, PrintOutputCapture<P> p) {
        beforeSyntax(node, CsSpace.Location.SINGLE_VARIABLE_DESIGNATION_PREFIX, p);
        visit(node.getName(), p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitUsingStatement(Cs.UsingStatement usingStatement, PrintOutputCapture<P> p)
    {
        beforeSyntax(usingStatement, CsSpace.Location.NAMED_ARGUMENT_PREFIX, p);
        p.append("using");
        if (usingStatement.getAwaitKeyword() != null)
        {
            visit(usingStatement.getAwaitKeyword(), p);
        }

        visitContainer("(", usingStatement.getPadding().getExpression(), CsContainer.Location.USING_STATEMENT_EXPRESSION, "", ")", p);
        visit(usingStatement.getStatement(), p);
        afterSyntax(usingStatement, p);
        return usingStatement;
    }

    @Override
    public Cs visitUnary(Cs.Unary unary, PrintOutputCapture<P> p)
    {
        beforeSyntax(unary, Space.Location.UNARY_PREFIX, p);
        switch (unary.getOperator())
        {
            default:
            case SuppressNullableWarning:
                visit(unary.getExpression(), p);
                p.append("!");
                break;
        }

        afterSyntax(unary, p);
        return unary;
    }

    @Override
    public Cs visitCompilationUnit(Cs.CompilationUnit compilationUnit, PrintOutputCapture<P> p) {
        beforeSyntax(compilationUnit, Space.Location.COMPILATION_UNIT_PREFIX, p);
        for (JRightPadded<Cs.ExternAlias> extern : compilationUnit.getPadding().getExterns()) {
            visitRightPadded(extern, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.append(';');
        }
        for (JRightPadded<Cs.UsingDirective> using : compilationUnit.getPadding().getUsings()) {
            visitRightPadded(using, CsRightPadded.Location.COMPILATION_UNIT_USINGS, p);
            p.append(';');
        }
        for (Cs.AttributeList attributeList : compilationUnit.getAttributeLists()) {
            visit(attributeList, p);
        }
        visitStatements(compilationUnit.getPadding().getMembers(), CsRightPadded.Location.COMPILATION_UNIT_MEMBERS, p);
        visitSpace(compilationUnit.getEof(), Space.Location.COMPILATION_UNIT_EOF, p);
        afterSyntax(compilationUnit, p);
        return compilationUnit;
    }

    @Override
    public J visitClassDeclaration(Cs.ClassDeclaration classDeclaration, PrintOutputCapture<P> p)
    {
        delegate.visit(classDeclaration.getClassDeclarationCore(), p);
        return classDeclaration;
    }

    @Override
    public J visitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, PrintOutputCapture<P> p)
    {
        delegate.visitMethodDeclaration(methodDeclaration.getMethodDeclarationCore(), p);
        if (methodDeclaration.getMethodDeclarationCore().getBody() == null)
        {
            p.append(";");
        }
        return methodDeclaration;
    }

    @Override
    public J visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, PrintOutputCapture<P> p) {
        beforeSyntax(annotatedStatement, CsSpace.Location.ANNOTATED_STATEMENT_PREFIX, p);
        for (Cs.AttributeList attributeList : annotatedStatement.getAttributeLists()) {
            visit(attributeList, p);
        }
        visit(annotatedStatement.getStatement(), p);
        afterSyntax(annotatedStatement, p);
        return annotatedStatement;
    }

    @Override
    public J visitAttributeList(Cs.AttributeList attributeList, PrintOutputCapture<P> p) {
        beforeSyntax(attributeList, CsSpace.Location.ATTRIBUTE_LIST_PREFIX, p);
        p.append('[');
        Cs.AttributeList.Padding padding = attributeList.getPadding();
        if (padding.getTarget() != null) {
            visitRightPadded(padding.getTarget(), CsRightPadded.Location.ATTRIBUTE_LIST_TARGET, p);
            p.append(':');
        }
        visitRightPadded(padding.getAttributes(), CsRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, ",", p);
        p.append(']');
        afterSyntax(attributeList, p);
        return attributeList;
    }

    @Override
    public J visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, PrintOutputCapture<P> p) {
        beforeSyntax(arrayRankSpecifier, CsSpace.Location.ARRAY_RANK_SPECIFIER_PREFIX, p);
        visitContainer("", arrayRankSpecifier.getPadding().getSizes(), CsContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, ",", "", p);
        afterSyntax(arrayRankSpecifier, p);
        return arrayRankSpecifier;
    }

    @Override
    public J visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, PrintOutputCapture<P> p) {
        beforeSyntax(assignmentOperation, CsSpace.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        visit(assignmentOperation.getVariable(), p);
        visitLeftPadded(assignmentOperation.getPadding().getOperator(), CsLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        if (assignmentOperation.getOperator() == Cs.AssignmentOperation.OperatorType.NullCoalescing) {
            p.append("??=");
        }
        visit(assignmentOperation.getAssignment(), p);
        afterSyntax(assignmentOperation, p);
        return assignmentOperation;
    }

    @Override
    public J visitAwaitExpression(Cs.AwaitExpression awaitExpression, PrintOutputCapture<P> p) {
        beforeSyntax(awaitExpression, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        p.append("await");
        visit(awaitExpression.getExpression(), p);
        afterSyntax(awaitExpression, p);
        return awaitExpression;
    }

    @Override
    public J visitBinary(Cs.Binary binary, PrintOutputCapture<P> p) {
        beforeSyntax(binary, CsSpace.Location.BINARY_PREFIX, p);
        visit(binary.getLeft(), p);
        visitSpace(binary.getPadding().getOperator().getBefore(), Space.Location.BINARY_OPERATOR, p);
        if (binary.getOperator() == Cs.Binary.OperatorType.As) {
            p.append("as");
        } else if (binary.getOperator() == Cs.Binary.OperatorType.NullCoalescing) {
            p.append("??");
        }
        visit(binary.getRight(), p);
        afterSyntax(binary, p);
        return binary;
    }

    @Override
    public Cs visitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration namespaceDeclaration, PrintOutputCapture<P> p) {
        beforeSyntax(namespaceDeclaration, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.append("namespace");
        visitRightPadded(namespaceDeclaration.getPadding().getName(), CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.append('{');
        for (JRightPadded<Cs.ExternAlias> extern : namespaceDeclaration.getPadding().getExterns()) {
            visitRightPadded(extern, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.append(';');
        }
        for (JRightPadded<Cs.UsingDirective> using : namespaceDeclaration.getPadding().getUsings()) {
            visitRightPadded(using, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.append(';');
        }
        visitStatements(namespaceDeclaration.getPadding().getMembers(), CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        visitSpace(namespaceDeclaration.getEnd(), CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p);
        p.append('}');
        afterSyntax(namespaceDeclaration, p);
        return namespaceDeclaration;
    }

    @Override
    public J visitCollectionExpression(Cs.CollectionExpression collectionExpression, PrintOutputCapture<P> p) {
        beforeSyntax(collectionExpression, CsSpace.Location.COLLECTION_EXPRESSION_PREFIX, p);
        p.append('[');
        visitRightPadded(collectionExpression.getPadding().getElements(), CsRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS, ",", p);
        p.append(']');
        afterSyntax(collectionExpression, p);
        return collectionExpression;
    }

    @Override
    public J visitExpressionStatement(Cs.ExpressionStatement expressionStatement, PrintOutputCapture<P> p) {
        beforeSyntax(expressionStatement, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        visit(expressionStatement.getExpression(), p);
        afterSyntax(expressionStatement, p);
        return expressionStatement;
    }

    @Override
    public J visitExternAlias(Cs.ExternAlias externAlias, PrintOutputCapture<P> p) {
        beforeSyntax(externAlias, CsSpace.Location.EXTERN_ALIAS_PREFIX, p);
        p.append("extern");
        visitLeftPadded("alias", externAlias.getPadding().getIdentifier(), CsLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p);
        afterSyntax(externAlias, p);
        return externAlias;
    }

    @Override
    public Cs visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration namespaceDeclaration,
                                                 PrintOutputCapture<P> p) {
        beforeSyntax(namespaceDeclaration, CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.append("namespace");
        visitRightPadded(namespaceDeclaration.getPadding().getName(), CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.append(";");
        for (JRightPadded<Cs.ExternAlias> extern : namespaceDeclaration.getPadding().getExterns()) {
            visitRightPadded(extern, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.append(';');
        }
        for (JRightPadded<Cs.UsingDirective> using : namespaceDeclaration.getPadding().getUsings()) {
            visitRightPadded(using, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.append(';');
        }
        visitStatements(namespaceDeclaration.getPadding().getMembers(), CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        return namespaceDeclaration;
    }

    @Override
    public J visitInterpolatedString(Cs.InterpolatedString interpolatedString, PrintOutputCapture<P> p) {
        beforeSyntax(interpolatedString, CsSpace.Location.INTERPOLATED_STRING_PREFIX, p);
        p.append(interpolatedString.getStart());
        visitRightPadded(interpolatedString.getPadding().getParts(), CsRightPadded.Location.INTERPOLATED_STRING_PARTS, "", p);
        p.append(interpolatedString.getEnd());
        afterSyntax(interpolatedString, p);
        return interpolatedString;
    }

    @Override
    public J visitInterpolation(Cs.Interpolation interpolation, PrintOutputCapture<P> p) {
        beforeSyntax(interpolation, CsSpace.Location.INTERPOLATION_PREFIX, p);
        p.append('{');
        visitRightPadded(interpolation.getPadding().getExpression(), CsRightPadded.Location.INTERPOLATION_EXPRESSION, p);
        if (interpolation.getAlignment() != null) {
            p.append(',');
            visitRightPadded(interpolation.getPadding().getAlignment(), CsRightPadded.Location.INTERPOLATION_ALIGNMENT, p);
        }
        if (interpolation.getFormat() != null) {
            p.append(':');
            visitRightPadded(interpolation.getPadding().getFormat(), CsRightPadded.Location.INTERPOLATION_FORMAT, p);
        }
        p.append('}');
        afterSyntax(interpolation, p);
        return interpolation;
    }

    @Override
    public J visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, PrintOutputCapture<P> p) {
        beforeSyntax(nullSafeExpression, CsSpace.Location.NULL_SAFE_EXPRESSION_PREFIX, p);
        visitRightPadded(nullSafeExpression.getPadding().getExpression(), CsRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION, p);
        p.append("?");
        afterSyntax(nullSafeExpression, p);
        return nullSafeExpression;
    }

    @Override
    public Cs visitConstructor(Cs.Constructor constructor, PrintOutputCapture<P> p)
    {
        J.MethodDeclaration method = constructor.getConstructorCore();
        beforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
        visitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        visit(method.getLeadingAnnotations(), p);
        for (J.Modifier modifier : method.getModifiers())
        {
            delegate.visitModifier(modifier, p);
        }

        visit(method.getAnnotations().getName().getAnnotations(), p);
        visit(method.getName(), p);


        if (!method.getMarkers().findFirst(CompactConstructor.class).isPresent()) {
            visitContainer("(", method.getPadding().getParameters(), CsContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        visit(constructor.getInitializer(), p);

        visit(method.getBody(), p);
        afterSyntax(constructor, p);
        return constructor;
    }

    @Override
    public Cs visitConstructorInitializer(Cs.ConstructorInitializer node, PrintOutputCapture<P> p)
    {
        beforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        p.append(":");
        visit(node.getKeyword(), p);
        visitContainer("(", node.getPadding().getArguments(), CsContainer.Location.CONSTRUCTOR_INITIALIZER_ARGUMENTS, ",", ")", p);
        afterSyntax(node, p);
        return node;
    }

    @Override
    public J visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration,
            PrintOutputCapture<P> p) {
        beforeSyntax(propertyDeclaration, CsSpace.Location.PROPERTY_DECLARATION_PREFIX, p);
        visit(propertyDeclaration.getAttributeLists(), p);
        for (J.Modifier m : propertyDeclaration.getModifiers()) {
            delegate.visitModifier(m, p);
        }
        visit(propertyDeclaration.getTypeExpression(), p);
        if (propertyDeclaration.getPadding().getInterfaceSpecifier() != null) {
            visitRightPadded(propertyDeclaration.getPadding().getInterfaceSpecifier(), CsRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, p);
            p.append('.');
        }
        visit(propertyDeclaration.getName(), p);
        visit(propertyDeclaration.getAccessors(), p);
        if(propertyDeclaration.getInitializer() != null) {
            visitLeftPadded("=", propertyDeclaration.getPadding().getInitializer(), CsLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p);
        }
        afterSyntax(propertyDeclaration, p);
        return propertyDeclaration;
    }

    @Override
    public J visitUsingDirective(Cs.UsingDirective usingDirective, PrintOutputCapture<P> p) {
        beforeSyntax(usingDirective, CsSpace.Location.USING_DIRECTIVE_PREFIX, p);
        if (usingDirective.isGlobal()) {
            p.append("global");
            visitRightPadded(usingDirective.getPadding().getGlobal(), CsRightPadded.Location.USING_DIRECTIVE_GLOBAL, p);
        }
        p.append("using");
        if (usingDirective.isStatic()) {
            visitLeftPadded(usingDirective.getPadding().getStatic(), CsLeftPadded.Location.USING_DIRECTIVE_STATIC, p);
            p.append("static");
        } else if (usingDirective.getAlias() != null) {
            if (usingDirective.isUnsafe()) {
                visitLeftPadded(usingDirective.getPadding().getUnsafe(), CsLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p);
                p.append("unsafe");
            }
            visitRightPadded(usingDirective.getPadding().getAlias(), CsRightPadded.Location.USING_DIRECTIVE_ALIAS, p);
            p.append('=');
        }
        visit(usingDirective.getNamespaceOrType(), p);
        afterSyntax(usingDirective, p);
        return usingDirective;
    }

    @Override
    public Cs visitLambda(Cs.Lambda lambda, PrintOutputCapture<P> p)
    {
        if (!lambda.getModifiers().isEmpty()) // only put space in front if current node actually has anything to contribute
        {
            beforeSyntax(lambda, Space.Location.LAMBDA_PREFIX, p);
        }
        J.Lambda javaLambda = lambda.getLambdaExpression();
        visitSpace(lambda.getPrefix(), Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
        visitMarkers(lambda.getMarkers(), p);
        for(J.Modifier modifier : lambda.getModifiers())
        {
            delegate.visitModifier(modifier, p);
        }

        visit(javaLambda, p);
        afterSyntax(lambda, p);
        return lambda;
    }

    @Override
    public Space visitSpace(Space space, CsSpace.Location loc, PrintOutputCapture<P> p) {
        return delegate.visitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }

    @Override
    public Space visitSpace(Space space, Space.Location loc, PrintOutputCapture<P> p) {
        return delegate.visitSpace(space, loc, p);
    }

    protected void visitLeftPadded(@Nullable String prefix, @Nullable JLeftPadded<? extends J> leftPadded, CsLeftPadded.Location location, PrintOutputCapture<P> p) {
        if (leftPadded != null) {
            beforeSyntax(leftPadded.getBefore(), leftPadded.getMarkers(), location.getBeforeLocation(), p);
            if (prefix != null) {
                p.append(prefix);
            }
            visit(leftPadded.getElement(), p);
            afterSyntax(leftPadded.getMarkers(), p);
        }
    }

    protected void visitContainer(@Nullable String before, @Nullable JContainer<? extends J> container, CsContainer.Location location,
                                  String suffixBetween, @Nullable String after, PrintOutputCapture<P> p) {
        if (container == null) {
            return;
        }
        visitSpace(container.getBefore(), location.getBeforeLocation(), p);
        p.append(before);
        visitRightPadded(container.getPadding().getElements(), location.getElementLocation(), suffixBetween, p);
        p.append(after);
    }

    protected void visitRightPadded(List<? extends JRightPadded<? extends J>> nodes, CsRightPadded.Location location, String suffixBetween, PrintOutputCapture<P> p) {
        for (int i = 0; i < nodes.size(); i++) {
            JRightPadded<? extends J> node = nodes.get(i);
            visit(node.getElement(), p);
            visitSpace(node.getAfter(), location.getAfterLocation(), p);
            visitMarkers(node.getMarkers(), p);
            if (i < nodes.size() - 1) {
                p.append(suffixBetween);
            }
        }
    }

    protected void visitStatements(@Nullable String before, @Nullable JContainer<Statement> container, CsContainer.Location location,
                                   @Nullable String after, PrintOutputCapture<P> p) {
        if (container == null) {
            return;
        }
        visitSpace(container.getBefore(), location.getBeforeLocation(), p);
        p.append(before);
        visitStatements(container.getPadding().getElements(), location.getElementLocation(), p);
        p.append(after);
    }

    protected void visitStatements(List<JRightPadded<Statement>> statements, CsRightPadded.Location location, PrintOutputCapture<P> p) {
        for (JRightPadded<Statement> paddedStat : statements) {
            visitStatement(paddedStat, location, p);
        }
    }

    protected void visitStatement(@Nullable JRightPadded<Statement> paddedStat, CsRightPadded.Location location, PrintOutputCapture<P> p) {
        if (paddedStat == null) {
            return;
        }

        visit(paddedStat.getElement(), p);
        visitSpace(paddedStat.getAfter(), location.getAfterLocation(), p);
        visitMarkers(paddedStat.getMarkers(), p);
        if (getCursor().getParent().getValue() instanceof J.Block && getCursor().getParent().getParent().getValue() instanceof J.NewClass) {
            p.append(',');
            return;
        }
        delegate.printStatementTerminator(paddedStat.getElement(), p);
    }

    @Override
    public J visitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, PrintOutputCapture<P> p)
    {
        beforeSyntax(typeParameterConstraintClause, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_CLAUSE_PREFIX, p);
        p.append("where");
        visitRightPadded(typeParameterConstraintClause.getPadding().getTypeParameter(), CsRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER,  p);
        p.append(":");
        visitContainer("", typeParameterConstraintClause.getPadding().getTypeParameterConstraints(), CsContainer.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS, ",", "", p);
        afterSyntax(typeParameterConstraintClause, p);
        return typeParameterConstraintClause;
    }

    @Override
    public J visitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, PrintOutputCapture<P> p)
    {
        beforeSyntax(classOrStructConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.append(classOrStructConstraint.getKind().equals(Cs.ClassOrStructConstraint.TypeKind.Class) ? "class" : "struct");
        return classOrStructConstraint;
    }

    @Override
    public J visitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, PrintOutputCapture<P> p)
    {
        beforeSyntax(constructorConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.append("new()");
        return constructorConstraint;
    }

    @Override
    public J visitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, PrintOutputCapture<P> p)
    {
        beforeSyntax(defaultConstraint, CsSpace.Location.TYPE_PARAMETERS_CONSTRAINT_PREFIX, p);
        p.append("default");
        return defaultConstraint;
    }

    @Override
    public J visitInitializerExpression(Cs.InitializerExpression node, PrintOutputCapture<P> p)
    {
        beforeSyntax(node, Space.Location.BLOCK_PREFIX, p);
        visitContainer("{", node.getPadding().getExpressions(), CsContainer.Location.INITIALIZER_EXPRESSION_EXPRESSIONS, ",", "}", p);
        afterSyntax(node, p);
        return node;
    }

    private class CSharpJavaPrinter extends JavaPrinter<P> {

        @Override
        public @Nullable J preVisit(@NonNull J tree, PrintOutputCapture<P> pPrintOutputCapture) {
            return CSharpPrinter.this.preVisit(tree, pPrintOutputCapture);
        }

        @Override
        public @Nullable J postVisit(@NonNull J tree, PrintOutputCapture<P> pPrintOutputCapture) {
            return CSharpPrinter.this.postVisit(tree, pPrintOutputCapture);
        }

        @Override
        public J visit(@Nullable Tree tree, PrintOutputCapture<P> p) {
            if (tree instanceof Cs) {
                // re-route printing back up to groovy
                return CSharpPrinter.this.visit(tree, p);
            } else {
                return super.visit(tree, p);
            }
        }

        @Override
        public J visitNewArray(J.NewArray newArray, PrintOutputCapture<P> p)
        {
            beforeSyntax(newArray, Space.Location.NEW_ARRAY_PREFIX, p);
            p.append("new");

            visit(newArray.getTypeExpression(), p);
            visit(newArray.getDimensions(), p);
            visitContainer("{", newArray.getPadding().getInitializer(), JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
            afterSyntax(newArray, p);
            return newArray;
        }

        @Override
        public J visitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<P> p) {
            J csParent = CSharpPrinter.this.getCursor().getValue();
            Cs.ClassDeclaration csClassDeclaration = csParent instanceof Cs.ClassDeclaration ? (Cs.ClassDeclaration) csParent : null;
            String kind = "";
            switch (classDecl.getPadding().getKind().getType()) {
                case Class:
                case Annotation:
                    kind = "class";
                    break;
                case Enum:
                    kind = "enum";
                    break;
                case Interface:
                    kind = "interface";
                    break;
                case Record:
                    kind = "record";
                    break;
                case Value:
                    kind = "struct";
                    break;
            }

            beforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
            visitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            visit(classDecl.getLeadingAnnotations(), p);
            for (J.Modifier m : classDecl.getModifiers()) {
                visit(m, p);
            }
            visit(classDecl.getPadding().getKind().getAnnotations(), p);
            visitSpace(classDecl.getPadding().getKind().getPrefix(), Space.Location.CLASS_KIND, p);
            p.append(kind);
            visit(classDecl.getName(), p);
            visitContainer("<", classDecl.getPadding().getTypeParameters(), JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            visitContainer("(", classDecl.getPadding().getPrimaryConstructor(), JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
            visitLeftPadded(":", classDecl.getPadding().getExtends(), JLeftPadded.Location.EXTENDS, p);
            visitContainer(classDecl.getPadding().getExtends() == null ? ":" : ",",
                    classDecl.getPadding().getImplements(), JContainer.Location.IMPLEMENTS, ",", null, p);
            visitContainer("permits", classDecl.getPadding().getPermits(), JContainer.Location.PERMITS, ",", null, p);
            if(csClassDeclaration != null) {
                for (Cs.TypeParameterConstraintClause typeParameterClause : csClassDeclaration.getTypeParameterConstraintClauses())
                {
                    CSharpPrinter.this.visit(typeParameterClause, p);
                }
            }
            visit(classDecl.getBody(), p);
            afterSyntax(classDecl, p);
            return classDecl;
        }

        @Override
        public J visitAnnotation(J.Annotation annotation, PrintOutputCapture<P> p) {
            beforeSyntax(annotation, Space.Location.ANNOTATION_PREFIX, p);
            visit(annotation.getAnnotationType(), p);
            visitContainer("(", annotation.getPadding().getArguments(), JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
            afterSyntax(annotation, p);
            return annotation;
        }

        @Override
        public J visitBlock(J.Block block, PrintOutputCapture<P> p) {
            beforeSyntax(block, Space.Location.BLOCK_PREFIX, p);

            if (block.isStatic()) {
                p.append("static");
                visitRightPadded(block.getPadding().getStatic(), JRightPadded.Location.STATIC_INIT, p);
            }

            if (block.getMarkers().findFirst(SingleExpressionBlock.class).isPresent()) {
                p.append("=>");
                visitStatements(block.getPadding().getStatements(), JRightPadded.Location.BLOCK_STATEMENT, p);
                if (!(block.getStatements().get(0) instanceof Cs.ExpressionStatement)) // expression statements print their own semicolon
                {
                    p.append(";");
                }
                visitSpace(block.getEnd(), Space.Location.BLOCK_END, p);
            } else if (!block.getMarkers().findFirst(OmitBraces.class).isPresent() || !block.getStatements().isEmpty()) {
                p.append('{');
                visitStatements(block.getPadding().getStatements(), JRightPadded.Location.BLOCK_STATEMENT, p);
                visitSpace(block.getEnd(), Space.Location.BLOCK_END, p);
                p.append('}');
            } else {
                if (!block.getStatements().isEmpty()) {
                    visitStatements(block.getPadding().getStatements(), JRightPadded.Location.BLOCK_STATEMENT, p);
                }
                else {
                    p.append(";");
                }
                visitSpace(block.getEnd(), Space.Location.BLOCK_END, p);
            }


            afterSyntax(block, p);
            return block;
        }

        @Override
        protected void visitStatements(List<JRightPadded<Statement>> statements,
                                       JRightPadded.Location location,
                                       PrintOutputCapture<P> p) {
            for (int i = 0; i < statements.size(); i++) {
                JRightPadded<Statement> paddedStat = statements.get(i);
                visitStatement(paddedStat, location, p);
                if (i < statements.size() - 1 &&
                    (getCursor().getParent() != null && getCursor().getParent()
                            .getValue() instanceof J.NewClass ||
                     (getCursor().getParent() != null && getCursor().getParent()
                             .getValue() instanceof J.Block &&
                      getCursor().getParent(2) != null && getCursor().getParent(2).getValue() instanceof J.NewClass
                     )
                    )
                ) {
                    p.append(',');
                }
            }

        }

        @Override
        public J visitMethodDeclaration(J.MethodDeclaration method, PrintOutputCapture<P> p) {
            beforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
            visitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            visit(method.getLeadingAnnotations(), p);
            for (J.Modifier m : method.getModifiers()) {
                visit(m, p);
            }
            visit(method.getReturnTypeExpression(), p);
            visit(method.getAnnotations().getName().getAnnotations(), p);
            visit(method.getName(), p);
            J.TypeParameters typeParameters = method.getAnnotations().getTypeParameters();
            if (typeParameters != null) {
                visit(typeParameters.getAnnotations(), p);
                visitSpace(typeParameters.getPrefix(), Space.Location.TYPE_PARAMETERS, p);
                visitMarkers(typeParameters.getMarkers(), p);
                p.append('<');
                visitRightPadded(typeParameters.getPadding().getTypeParameters(), JRightPadded.Location.TYPE_PARAMETER, ",", p);
                p.append('>');
            }
            if (!method.getMarkers().findFirst(CompactConstructor.class).isPresent()) {
                visitContainer("(", method.getPadding().getParameters(), JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
            }

            if(CSharpPrinter.this.getCursor().getValue() instanceof Cs.MethodDeclaration) {
                Cs.MethodDeclaration csMethod = CSharpPrinter.this.getCursor().getValue();
                visit(csMethod.getTypeParameterConstraintClauses(), p);
            }

            visitContainer("throws", method.getPadding().getThrows(), JContainer.Location.THROWS, ",", null, p);
            visit(method.getBody(), p);
            visitLeftPadded("default", method.getPadding().getDefaultValue(), JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
            afterSyntax(method, p);
            return method;
        }

        @Override
        public J visitMethodInvocation(J.MethodInvocation method, PrintOutputCapture<P> p) {
            beforeSyntax(method, Space.Location.METHOD_INVOCATION_PREFIX, p);
            String prefix = !method.getSimpleName().isEmpty() ? "." : "";
            visitRightPadded(method.getPadding().getSelect(), JRightPadded.Location.METHOD_SELECT, prefix, p);
            visit(method.getName(), p);
            visitContainer("<", method.getPadding().getTypeParameters(), JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            visitContainer("(", method.getPadding().getArguments(), JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
            afterSyntax(method, p);
            return method;
        }

        @Override
        public J visitCatch(J.Try.Catch catch_, PrintOutputCapture<P> p) {
            beforeSyntax(catch_, Space.Location.CATCH_PREFIX, p);
            p.append("catch");
            if (catch_.getParameter().getTree().getTypeExpression() != null) {
                // this is the catch-all case `catch`
                visit(catch_.getParameter(), p);
            }
            visit(catch_.getBody(), p);
            afterSyntax(catch_, p);
            return catch_;
        }

        @Override
        public J visitForEachLoop(J.ForEachLoop forEachLoop, PrintOutputCapture<P> p) {
            this.beforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
            p.append("foreach");
            J.ForEachLoop.Control ctrl = forEachLoop.getControl();
            this.visitSpace(ctrl.getPrefix(), Space.Location.FOR_EACH_CONTROL_PREFIX, p);
            p.append('(');
            this.visitRightPadded(ctrl.getPadding().getVariable(),
                    JRightPadded.Location.FOREACH_VARIABLE,
                    "in", p);
            this.visitRightPadded(ctrl.getPadding().getIterable(), JRightPadded.Location.FOREACH_ITERABLE, "", p);
            p.append(')');
            this.visitStatement(forEachLoop.getPadding().getBody(), JRightPadded.Location.FOR_BODY, p);
            this.afterSyntax((J) forEachLoop, p);
            return forEachLoop;
        }

        @Override
        public J visitInstanceOf(J.InstanceOf instanceOf, PrintOutputCapture<P> p) {
            beforeSyntax(instanceOf, Space.Location.INSTANCEOF_PREFIX, p);
            visitRightPadded(instanceOf.getPadding().getExpression(), JRightPadded.Location.INSTANCEOF, "is", p);
            visit(instanceOf.getClazz(), p);
            visit(instanceOf.getPattern(), p);
            afterSyntax(instanceOf, p);
            return instanceOf;
        }

        @Override
        public J visitLambda(J.Lambda lambda, PrintOutputCapture<P> p) {
            beforeSyntax(lambda, Space.Location.LAMBDA_PREFIX, p);
            visitSpace(lambda.getParameters().getPrefix(), Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
            visitMarkers(lambda.getParameters().getMarkers(), p);
            if (lambda.getParameters().isParenthesized()) {
                p.append('(');
                visitRightPadded(lambda.getParameters().getPadding().getParameters(), JRightPadded.Location.LAMBDA_PARAM, ",", p);
                p.append(')');
            } else {
                visitRightPadded(lambda.getParameters().getPadding().getParameters(), JRightPadded.Location.LAMBDA_PARAM, ",", p);
            }
            visitSpace(lambda.getArrow(), Space.Location.LAMBDA_ARROW_PREFIX, p);
            p.append("=>");
            visit(lambda.getBody(), p);
            afterSyntax(lambda, p);
            return lambda;
        }

        @Override
        public J visitPrimitive(J.Primitive primitive, PrintOutputCapture<P> p) {
            String keyword;
            switch (primitive.getType()) {
                case Boolean:
                    keyword = "bool";
                    break;
                case Byte:
                    keyword = "byte";
                    break;
                case Char:
                    keyword = "char";
                    break;
                case Double:
                    keyword = "double";
                    break;
                case Float:
                    keyword = "float";
                    break;
                case Int:
                    keyword = "int";
                    break;
                case Long:
                    keyword = "long";
                    break;
                case Short:
                    keyword = "short";
                    break;
                case Void:
                    keyword = "void";
                    break;
                case String:
                    keyword = "string";
                    break;
                case None:
                    throw new IllegalStateException("Unable to print None primitive");
                case Null:
                    throw new IllegalStateException("Unable to print Null primitive");
                default:
                    throw new IllegalStateException("Unable to print non-primitive type");
            }
            beforeSyntax(primitive, Space.Location.PRIMITIVE_PREFIX, p);
            p.append(keyword);
            afterSyntax(primitive, p);
            return primitive;
        }

        @Override
        public <M extends Marker> M visitMarker(Marker marker, PrintOutputCapture<P> p) {
            if (marker instanceof Semicolon) {
                p.append(';');
            } else if (marker instanceof TrailingComma) {
                p.append(',');
                visitSpace(((TrailingComma) marker).getSuffix(), Space.Location.LANGUAGE_EXTENSION, p);
            }
            return (M) marker;
        }

        @Override
        protected void printStatementTerminator(Statement s, PrintOutputCapture<P> p) {
            if (s instanceof Cs.ExpressionStatement || s instanceof Cs.AssignmentOperation || s instanceof Cs.Yield) {
                p.append(';');
            } else if (s instanceof Cs.PropertyDeclaration && (((Cs.PropertyDeclaration) s).getInitializer() != null)) {
                p.append(';');

            } else {
                super.printStatementTerminator(s, p);
            }
        }
    }

    @Override
    public <M extends Marker> M visitMarker(Marker marker, PrintOutputCapture<P> p) {
        return delegate.visitMarker(marker, p);
    }

    private static final UnaryOperator<String> JAVA_MARKER_WRAPPER =
            out -> "/*~~" + out + (out.isEmpty() ? "" : "~~") + ">*/";

    private void beforeSyntax(J j, Space.Location loc, PrintOutputCapture<P> p) {
        beforeSyntax(j.getPrefix(), j.getMarkers(), loc, p);
    }

    private void beforeSyntax(J j, CsSpace.Location loc, PrintOutputCapture<P> p) {
        beforeSyntax(j.getPrefix(), j.getMarkers(), loc, p);
    }

    private void beforeSyntax(Space prefix, Markers markers, CsSpace.@Nullable Location loc, PrintOutputCapture<P> p) {
        for (Marker marker : markers.getMarkers()) {
            p.append(p.getMarkerPrinter().beforePrefix(marker, new Cursor(getCursor(), marker), JAVA_MARKER_WRAPPER));
        }
        if (loc != null) {
            visitSpace(prefix, loc, p);
        }
        visitMarkers(markers, p);
        for (Marker marker : markers.getMarkers()) {
            p.append(p.getMarkerPrinter().beforeSyntax(marker, new Cursor(getCursor(), marker), JAVA_MARKER_WRAPPER));
        }
    }

    private void beforeSyntax(Space prefix, Markers markers, Space.@Nullable Location loc, PrintOutputCapture<P> p) {
        for (Marker marker : markers.getMarkers()) {
            p.append(p.getMarkerPrinter().beforePrefix(marker, new Cursor(getCursor(), marker), JAVA_MARKER_WRAPPER));
        }
        if (loc != null) {
            visitSpace(prefix, loc, p);
        }
        visitMarkers(markers, p);
        for (Marker marker : markers.getMarkers()) {
            p.append(p.getMarkerPrinter().beforeSyntax(marker, new Cursor(getCursor(), marker), JAVA_MARKER_WRAPPER));
        }
    }

    private void afterSyntax(Cs g, PrintOutputCapture<P> p) {
        afterSyntax(g.getMarkers(), p);
    }

    private void afterSyntax(Markers markers, PrintOutputCapture<P> p) {
        for (Marker marker : markers.getMarkers()) {
            p.append(p.getMarkerPrinter().afterSyntax(marker, new Cursor(getCursor(), marker), JAVA_MARKER_WRAPPER));
        }
    }
}
