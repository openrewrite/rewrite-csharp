/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Moderne Source Available License (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://docs.moderne.io/licensing/moderne-source-available-license
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/*
 * -------------------THIS FILE IS AUTO GENERATED--------------------------
 * Changes to this file may cause incorrect behavior and will be lost if
 * the code is regenerated.
*/

package org.openrewrite.csharp;

import org.jspecify.annotations.Nullable;
import org.openrewrite.*;
import org.openrewrite.internal.ListUtils;
import org.openrewrite.marker.Markers;
import org.openrewrite.tree.*;
import org.openrewrite.java.JavaVisitor;
import org.openrewrite.java.tree.*;
import org.openrewrite.csharp.tree.*;

import java.util.List;

public class CSharpVisitor<P> extends JavaVisitor<P>
{
    @Override
    public boolean isAcceptable(SourceFile sourceFile, P p) {
        return sourceFile instanceof Cs;
    }

    public J visitCompilationUnit(Cs.CompilationUnit compilationUnit, P p) {
        compilationUnit = compilationUnit.withPrefix(visitSpace(compilationUnit.getPrefix(), Space.Location.COMPILATION_UNIT_PREFIX, p));
        compilationUnit = compilationUnit.withMarkers(visitMarkers(compilationUnit.getMarkers(), p));
        compilationUnit = compilationUnit.getPadding().withExterns(ListUtils.map(compilationUnit.getPadding().getExterns(), el -> visitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p)));
        compilationUnit = compilationUnit.getPadding().withUsings(ListUtils.map(compilationUnit.getPadding().getUsings(), el -> visitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_USINGS, p)));
        compilationUnit = compilationUnit.withAttributeLists(ListUtils.map(compilationUnit.getAttributeLists(), el -> (Cs.AttributeList)visit(el, p)));
        compilationUnit = compilationUnit.getPadding().withMembers(ListUtils.map(compilationUnit.getPadding().getMembers(), el -> visitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_MEMBERS, p)));
        compilationUnit = compilationUnit.withEof(visitSpace(compilationUnit.getEof(), Space.Location.COMPILATION_UNIT_EOF, p));
        return compilationUnit;
    }

    public J visitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, P p) {
        forEachVariableLoop = forEachVariableLoop.withPrefix(visitSpace(forEachVariableLoop.getPrefix(), CsSpace.Location.FOR_EACH_VARIABLE_LOOP_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(forEachVariableLoop, p);
        if (!(tempStatement instanceof Cs.ForEachVariableLoop))
        {
            return tempStatement;
        }
        forEachVariableLoop = (Cs.ForEachVariableLoop) tempStatement;
        forEachVariableLoop = forEachVariableLoop.withMarkers(visitMarkers(forEachVariableLoop.getMarkers(), p));
        forEachVariableLoop = forEachVariableLoop.withControlElement(visitAndCast(forEachVariableLoop.getControlElement(), p));
        forEachVariableLoop = forEachVariableLoop.getPadding().withBody(visitRightPadded(forEachVariableLoop.getPadding().getBody(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_BODY, p));
        return forEachVariableLoop;
    }

    public J visitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, P p) {
        control = control.withPrefix(visitSpace(control.getPrefix(), CsSpace.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_PREFIX, p));
        control = control.withMarkers(visitMarkers(control.getMarkers(), p));
        control = control.getPadding().withVariable(visitRightPadded(control.getPadding().getVariable(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_VARIABLE, p));
        control = control.getPadding().withIterable(visitRightPadded(control.getPadding().getIterable(), CsRightPadded.Location.FOR_EACH_VARIABLE_LOOP_CONTROL_ITERABLE, p));
        return control;
    }

    public J visitArgument(Cs.Argument argument, P p) {
        argument = argument.withPrefix(visitSpace(argument.getPrefix(), CsSpace.Location.ARGUMENT_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(argument, p);
        if (!(tempExpression instanceof Cs.Argument))
        {
            return tempExpression;
        }
        argument = (Cs.Argument) tempExpression;
        argument = argument.withMarkers(visitMarkers(argument.getMarkers(), p));
        argument = argument.getPadding().withNameColumn(visitRightPadded(argument.getPadding().getNameColumn(), CsRightPadded.Location.ARGUMENT_NAME_COLUMN, p));
        argument = argument.withRefKindKeyword(visitAndCast(argument.getRefKindKeyword(), p));
        argument = argument.withExpression(visitAndCast(argument.getExpression(), p));
        return argument;
    }

    public J visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, P p) {
        annotatedStatement = annotatedStatement.withPrefix(visitSpace(annotatedStatement.getPrefix(), CsSpace.Location.ANNOTATED_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(annotatedStatement, p);
        if (!(tempStatement instanceof Cs.AnnotatedStatement))
        {
            return tempStatement;
        }
        annotatedStatement = (Cs.AnnotatedStatement) tempStatement;
        annotatedStatement = annotatedStatement.withMarkers(visitMarkers(annotatedStatement.getMarkers(), p));
        annotatedStatement = annotatedStatement.withAttributeLists(ListUtils.map(annotatedStatement.getAttributeLists(), el -> (Cs.AttributeList)visit(el, p)));
        annotatedStatement = annotatedStatement.withStatement(visitAndCast(annotatedStatement.getStatement(), p));
        return annotatedStatement;
    }

    public J visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, P p) {
        arrayRankSpecifier = arrayRankSpecifier.withPrefix(visitSpace(arrayRankSpecifier.getPrefix(), CsSpace.Location.ARRAY_RANK_SPECIFIER_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(arrayRankSpecifier, p);
        if (!(tempExpression instanceof Cs.ArrayRankSpecifier))
        {
            return tempExpression;
        }
        arrayRankSpecifier = (Cs.ArrayRankSpecifier) tempExpression;
        arrayRankSpecifier = arrayRankSpecifier.withMarkers(visitMarkers(arrayRankSpecifier.getMarkers(), p));
        arrayRankSpecifier = arrayRankSpecifier.getPadding().withSizes(visitContainer(arrayRankSpecifier.getPadding().getSizes(), CsContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, p));
        return arrayRankSpecifier;
    }

    public J visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, P p) {
        assignmentOperation = assignmentOperation.withPrefix(visitSpace(assignmentOperation.getPrefix(), CsSpace.Location.ASSIGNMENT_OPERATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(assignmentOperation, p);
        if (!(tempStatement instanceof Cs.AssignmentOperation))
        {
            return tempStatement;
        }
        assignmentOperation = (Cs.AssignmentOperation) tempStatement;
        Expression tempExpression = (Expression) visitExpression(assignmentOperation, p);
        if (!(tempExpression instanceof Cs.AssignmentOperation))
        {
            return tempExpression;
        }
        assignmentOperation = (Cs.AssignmentOperation) tempExpression;
        assignmentOperation = assignmentOperation.withMarkers(visitMarkers(assignmentOperation.getMarkers(), p));
        assignmentOperation = assignmentOperation.withVariable(visitAndCast(assignmentOperation.getVariable(), p));
        assignmentOperation = assignmentOperation.getPadding().withOperator(visitLeftPadded(assignmentOperation.getPadding().getOperator(), CsLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p));
        assignmentOperation = assignmentOperation.withAssignment(visitAndCast(assignmentOperation.getAssignment(), p));
        return assignmentOperation;
    }

    public J visitAttributeList(Cs.AttributeList attributeList, P p) {
        attributeList = attributeList.withPrefix(visitSpace(attributeList.getPrefix(), CsSpace.Location.ATTRIBUTE_LIST_PREFIX, p));
        attributeList = attributeList.withMarkers(visitMarkers(attributeList.getMarkers(), p));
        attributeList = attributeList.getPadding().withTarget(visitRightPadded(attributeList.getPadding().getTarget(), CsRightPadded.Location.ATTRIBUTE_LIST_TARGET, p));
        attributeList = attributeList.getPadding().withAttributes(ListUtils.map(attributeList.getPadding().getAttributes(), el -> visitRightPadded(el, CsRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, p)));
        return attributeList;
    }

    public J visitAwaitExpression(Cs.AwaitExpression awaitExpression, P p) {
        awaitExpression = awaitExpression.withPrefix(visitSpace(awaitExpression.getPrefix(), CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(awaitExpression, p);
        if (!(tempExpression instanceof Cs.AwaitExpression))
        {
            return tempExpression;
        }
        awaitExpression = (Cs.AwaitExpression) tempExpression;
        awaitExpression = awaitExpression.withMarkers(visitMarkers(awaitExpression.getMarkers(), p));
        awaitExpression = awaitExpression.withExpression(visitAndCast(awaitExpression.getExpression(), p));
        return awaitExpression;
    }

    public J visitBinary(Cs.Binary binary, P p) {
        binary = binary.withPrefix(visitSpace(binary.getPrefix(), CsSpace.Location.BINARY_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(binary, p);
        if (!(tempExpression instanceof Cs.Binary))
        {
            return tempExpression;
        }
        binary = (Cs.Binary) tempExpression;
        binary = binary.withMarkers(visitMarkers(binary.getMarkers(), p));
        binary = binary.withLeft(visitAndCast(binary.getLeft(), p));
        binary = binary.getPadding().withOperator(visitLeftPadded(binary.getPadding().getOperator(), CsLeftPadded.Location.BINARY_OPERATOR, p));
        binary = binary.withRight(visitAndCast(binary.getRight(), p));
        return binary;
    }

    public J visitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, P p) {
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withPrefix(visitSpace(blockScopeNamespaceDeclaration.getPrefix(), CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(blockScopeNamespaceDeclaration, p);
        if (!(tempStatement instanceof Cs.BlockScopeNamespaceDeclaration))
        {
            return tempStatement;
        }
        blockScopeNamespaceDeclaration = (Cs.BlockScopeNamespaceDeclaration) tempStatement;
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withMarkers(visitMarkers(blockScopeNamespaceDeclaration.getMarkers(), p));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withName(visitRightPadded(blockScopeNamespaceDeclaration.getPadding().getName(), CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withExterns(ListUtils.map(blockScopeNamespaceDeclaration.getPadding().getExterns(), el -> visitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_EXTERNS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withUsings(ListUtils.map(blockScopeNamespaceDeclaration.getPadding().getUsings(), el -> visitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withMembers(ListUtils.map(blockScopeNamespaceDeclaration.getPadding().getMembers(), el -> visitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withEnd(visitSpace(blockScopeNamespaceDeclaration.getEnd(), CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p));
        return blockScopeNamespaceDeclaration;
    }

    public J visitCollectionExpression(Cs.CollectionExpression collectionExpression, P p) {
        collectionExpression = collectionExpression.withPrefix(visitSpace(collectionExpression.getPrefix(), CsSpace.Location.COLLECTION_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(collectionExpression, p);
        if (!(tempExpression instanceof Cs.CollectionExpression))
        {
            return tempExpression;
        }
        collectionExpression = (Cs.CollectionExpression) tempExpression;
        collectionExpression = collectionExpression.withMarkers(visitMarkers(collectionExpression.getMarkers(), p));
        collectionExpression = collectionExpression.getPadding().withElements(ListUtils.map(collectionExpression.getPadding().getElements(), el -> visitRightPadded(el, CsRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS, p)));
        return collectionExpression;
    }

    public J visitExpressionStatement(Cs.ExpressionStatement expressionStatement, P p) {
        expressionStatement = expressionStatement.withPrefix(visitSpace(expressionStatement.getPrefix(), CsSpace.Location.EXPRESSION_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(expressionStatement, p);
        if (!(tempStatement instanceof Cs.ExpressionStatement))
        {
            return tempStatement;
        }
        expressionStatement = (Cs.ExpressionStatement) tempStatement;
        expressionStatement = expressionStatement.withMarkers(visitMarkers(expressionStatement.getMarkers(), p));
        expressionStatement = expressionStatement.withExpression(visitAndCast(expressionStatement.getExpression(), p));
        return expressionStatement;
    }

    public J visitExternAlias(Cs.ExternAlias externAlias, P p) {
        externAlias = externAlias.withPrefix(visitSpace(externAlias.getPrefix(), CsSpace.Location.EXTERN_ALIAS_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(externAlias, p);
        if (!(tempStatement instanceof Cs.ExternAlias))
        {
            return tempStatement;
        }
        externAlias = (Cs.ExternAlias) tempStatement;
        externAlias = externAlias.withMarkers(visitMarkers(externAlias.getMarkers(), p));
        externAlias = externAlias.getPadding().withIdentifier(visitLeftPadded(externAlias.getPadding().getIdentifier(), CsLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p));
        return externAlias;
    }

    public J visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, P p) {
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withPrefix(visitSpace(fileScopeNamespaceDeclaration.getPrefix(), CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(fileScopeNamespaceDeclaration, p);
        if (!(tempStatement instanceof Cs.FileScopeNamespaceDeclaration))
        {
            return tempStatement;
        }
        fileScopeNamespaceDeclaration = (Cs.FileScopeNamespaceDeclaration) tempStatement;
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withMarkers(visitMarkers(fileScopeNamespaceDeclaration.getMarkers(), p));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withName(visitRightPadded(fileScopeNamespaceDeclaration.getPadding().getName(), CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, p));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withExterns(ListUtils.map(fileScopeNamespaceDeclaration.getPadding().getExterns(), el -> visitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_EXTERNS, p)));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withUsings(ListUtils.map(fileScopeNamespaceDeclaration.getPadding().getUsings(), el -> visitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p)));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withMembers(ListUtils.map(fileScopeNamespaceDeclaration.getPadding().getMembers(), el -> visitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p)));
        return fileScopeNamespaceDeclaration;
    }

    public J visitInterpolatedString(Cs.InterpolatedString interpolatedString, P p) {
        interpolatedString = interpolatedString.withPrefix(visitSpace(interpolatedString.getPrefix(), CsSpace.Location.INTERPOLATED_STRING_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(interpolatedString, p);
        if (!(tempExpression instanceof Cs.InterpolatedString))
        {
            return tempExpression;
        }
        interpolatedString = (Cs.InterpolatedString) tempExpression;
        interpolatedString = interpolatedString.withMarkers(visitMarkers(interpolatedString.getMarkers(), p));
        interpolatedString = interpolatedString.getPadding().withParts(ListUtils.map(interpolatedString.getPadding().getParts(), el -> visitRightPadded(el, CsRightPadded.Location.INTERPOLATED_STRING_PARTS, p)));
        return interpolatedString;
    }

    public J visitInterpolation(Cs.Interpolation interpolation, P p) {
        interpolation = interpolation.withPrefix(visitSpace(interpolation.getPrefix(), CsSpace.Location.INTERPOLATION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(interpolation, p);
        if (!(tempExpression instanceof Cs.Interpolation))
        {
            return tempExpression;
        }
        interpolation = (Cs.Interpolation) tempExpression;
        interpolation = interpolation.withMarkers(visitMarkers(interpolation.getMarkers(), p));
        interpolation = interpolation.getPadding().withExpression(visitRightPadded(interpolation.getPadding().getExpression(), CsRightPadded.Location.INTERPOLATION_EXPRESSION, p));
        interpolation = interpolation.getPadding().withAlignment(visitRightPadded(interpolation.getPadding().getAlignment(), CsRightPadded.Location.INTERPOLATION_ALIGNMENT, p));
        interpolation = interpolation.getPadding().withFormat(visitRightPadded(interpolation.getPadding().getFormat(), CsRightPadded.Location.INTERPOLATION_FORMAT, p));
        return interpolation;
    }

    public J visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, P p) {
        nullSafeExpression = nullSafeExpression.withPrefix(visitSpace(nullSafeExpression.getPrefix(), CsSpace.Location.NULL_SAFE_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(nullSafeExpression, p);
        if (!(tempExpression instanceof Cs.NullSafeExpression))
        {
            return tempExpression;
        }
        nullSafeExpression = (Cs.NullSafeExpression) tempExpression;
        nullSafeExpression = nullSafeExpression.withMarkers(visitMarkers(nullSafeExpression.getMarkers(), p));
        nullSafeExpression = nullSafeExpression.getPadding().withExpression(visitRightPadded(nullSafeExpression.getPadding().getExpression(), CsRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION, p));
        return nullSafeExpression;
    }

    public J visitStatementExpression(Cs.StatementExpression statementExpression, P p) {
        statementExpression = statementExpression.withPrefix(visitSpace(statementExpression.getPrefix(), CsSpace.Location.STATEMENT_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(statementExpression, p);
        if (!(tempExpression instanceof Cs.StatementExpression))
        {
            return tempExpression;
        }
        statementExpression = (Cs.StatementExpression) tempExpression;
        statementExpression = statementExpression.withMarkers(visitMarkers(statementExpression.getMarkers(), p));
        statementExpression = statementExpression.withStatement(visitAndCast(statementExpression.getStatement(), p));
        return statementExpression;
    }

    public J visitUsingDirective(Cs.UsingDirective usingDirective, P p) {
        usingDirective = usingDirective.withPrefix(visitSpace(usingDirective.getPrefix(), CsSpace.Location.USING_DIRECTIVE_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(usingDirective, p);
        if (!(tempStatement instanceof Cs.UsingDirective))
        {
            return tempStatement;
        }
        usingDirective = (Cs.UsingDirective) tempStatement;
        usingDirective = usingDirective.withMarkers(visitMarkers(usingDirective.getMarkers(), p));
        usingDirective = usingDirective.getPadding().withGlobal(visitRightPadded(usingDirective.getPadding().getGlobal(), CsRightPadded.Location.USING_DIRECTIVE_GLOBAL, p));
        usingDirective = usingDirective.getPadding().withStatic(visitLeftPadded(usingDirective.getPadding().getStatic(), CsLeftPadded.Location.USING_DIRECTIVE_STATIC, p));
        usingDirective = usingDirective.getPadding().withUnsafe(visitLeftPadded(usingDirective.getPadding().getUnsafe(), CsLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p));
        usingDirective = usingDirective.getPadding().withAlias(visitRightPadded(usingDirective.getPadding().getAlias(), CsRightPadded.Location.USING_DIRECTIVE_ALIAS, p));
        usingDirective = usingDirective.withNamespaceOrType(visitAndCast(usingDirective.getNamespaceOrType(), p));
        return usingDirective;
    }

    public J visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, P p) {
        propertyDeclaration = propertyDeclaration.withPrefix(visitSpace(propertyDeclaration.getPrefix(), CsSpace.Location.PROPERTY_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(propertyDeclaration, p);
        if (!(tempStatement instanceof Cs.PropertyDeclaration))
        {
            return tempStatement;
        }
        propertyDeclaration = (Cs.PropertyDeclaration) tempStatement;
        propertyDeclaration = propertyDeclaration.withMarkers(visitMarkers(propertyDeclaration.getMarkers(), p));
        propertyDeclaration = propertyDeclaration.withAttributeLists(ListUtils.map(propertyDeclaration.getAttributeLists(), el -> (Cs.AttributeList)visit(el, p)));
        propertyDeclaration = propertyDeclaration.withModifiers(ListUtils.map(propertyDeclaration.getModifiers(), el -> (J.Modifier)visit(el, p)));
        propertyDeclaration = propertyDeclaration.withTypeExpression(visitAndCast(propertyDeclaration.getTypeExpression(), p));
        propertyDeclaration = propertyDeclaration.getPadding().withInterfaceSpecifier(visitRightPadded(propertyDeclaration.getPadding().getInterfaceSpecifier(), CsRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, p));
        propertyDeclaration = propertyDeclaration.withName(visitAndCast(propertyDeclaration.getName(), p));
        propertyDeclaration = propertyDeclaration.withAccessors(visitAndCast(propertyDeclaration.getAccessors(), p));
        propertyDeclaration = propertyDeclaration.getPadding().withInitializer(visitLeftPadded(propertyDeclaration.getPadding().getInitializer(), CsLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p));
        return propertyDeclaration;
    }

    public J visitKeyword(Cs.Keyword keyword, P p) {
        keyword = keyword.withPrefix(visitSpace(keyword.getPrefix(), CsSpace.Location.KEYWORD_PREFIX, p));
        keyword = keyword.withMarkers(visitMarkers(keyword.getMarkers(), p));
        return keyword;
    }

    public J visitLambda(Cs.Lambda lambda, P p) {
        lambda = lambda.withPrefix(visitSpace(lambda.getPrefix(), CsSpace.Location.LAMBDA_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(lambda, p);
        if (!(tempStatement instanceof Cs.Lambda))
        {
            return tempStatement;
        }
        lambda = (Cs.Lambda) tempStatement;
        Expression tempExpression = (Expression) visitExpression(lambda, p);
        if (!(tempExpression instanceof Cs.Lambda))
        {
            return tempExpression;
        }
        lambda = (Cs.Lambda) tempExpression;
        lambda = lambda.withMarkers(visitMarkers(lambda.getMarkers(), p));
        lambda = lambda.withLambdaExpression(visitAndCast(lambda.getLambdaExpression(), p));
        lambda = lambda.withModifiers(ListUtils.map(lambda.getModifiers(), el -> (J.Modifier)visit(el, p)));
        return lambda;
    }

    public J visitClassDeclaration(Cs.ClassDeclaration classDeclaration, P p) {
        classDeclaration = classDeclaration.withPrefix(visitSpace(classDeclaration.getPrefix(), CsSpace.Location.CLASS_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(classDeclaration, p);
        if (!(tempStatement instanceof Cs.ClassDeclaration))
        {
            return tempStatement;
        }
        classDeclaration = (Cs.ClassDeclaration) tempStatement;
        classDeclaration = classDeclaration.withMarkers(visitMarkers(classDeclaration.getMarkers(), p));
        classDeclaration = classDeclaration.withClassDeclarationCore(visitAndCast(classDeclaration.getClassDeclarationCore(), p));
        classDeclaration = classDeclaration.getPadding().withTypeParameterConstraintClauses(visitContainer(classDeclaration.getPadding().getTypeParameterConstraintClauses(), CsContainer.Location.CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p));
        return classDeclaration;
    }

    public J visitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, P p) {
        methodDeclaration = methodDeclaration.withPrefix(visitSpace(methodDeclaration.getPrefix(), CsSpace.Location.METHOD_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(methodDeclaration, p);
        if (!(tempStatement instanceof Cs.MethodDeclaration))
        {
            return tempStatement;
        }
        methodDeclaration = (Cs.MethodDeclaration) tempStatement;
        methodDeclaration = methodDeclaration.withMarkers(visitMarkers(methodDeclaration.getMarkers(), p));
        methodDeclaration = methodDeclaration.withMethodDeclarationCore(visitAndCast(methodDeclaration.getMethodDeclarationCore(), p));
        methodDeclaration = methodDeclaration.getPadding().withTypeParameterConstraintClauses(visitContainer(methodDeclaration.getPadding().getTypeParameterConstraintClauses(), CsContainer.Location.METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, p));
        return methodDeclaration;
    }

    public J visitUsingStatement(Cs.UsingStatement usingStatement, P p) {
        usingStatement = usingStatement.withPrefix(visitSpace(usingStatement.getPrefix(), CsSpace.Location.USING_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(usingStatement, p);
        if (!(tempStatement instanceof Cs.UsingStatement))
        {
            return tempStatement;
        }
        usingStatement = (Cs.UsingStatement) tempStatement;
        usingStatement = usingStatement.withMarkers(visitMarkers(usingStatement.getMarkers(), p));
        usingStatement = usingStatement.withAwaitKeyword(visitAndCast(usingStatement.getAwaitKeyword(), p));
        usingStatement = usingStatement.getPadding().withExpression(visitContainer(usingStatement.getPadding().getExpression(), CsContainer.Location.USING_STATEMENT_EXPRESSION, p));
        usingStatement = usingStatement.withStatement(visitAndCast(usingStatement.getStatement(), p));
        return usingStatement;
    }

    public J visitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, P p) {
        typeParameterConstraintClause = typeParameterConstraintClause.withPrefix(visitSpace(typeParameterConstraintClause.getPrefix(), CsSpace.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_PREFIX, p));
        typeParameterConstraintClause = typeParameterConstraintClause.withMarkers(visitMarkers(typeParameterConstraintClause.getMarkers(), p));
        typeParameterConstraintClause = typeParameterConstraintClause.getPadding().withTypeParameter(visitRightPadded(typeParameterConstraintClause.getPadding().getTypeParameter(), CsRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER, p));
        typeParameterConstraintClause = typeParameterConstraintClause.getPadding().withTypeParameterConstraints(visitContainer(typeParameterConstraintClause.getPadding().getTypeParameterConstraints(), CsContainer.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS, p));
        return typeParameterConstraintClause;
    }

    public J visitTypeConstraint(Cs.TypeConstraint typeConstraint, P p) {
        typeConstraint = typeConstraint.withPrefix(visitSpace(typeConstraint.getPrefix(), CsSpace.Location.TYPE_CONSTRAINT_PREFIX, p));
        typeConstraint = typeConstraint.withMarkers(visitMarkers(typeConstraint.getMarkers(), p));
        typeConstraint = typeConstraint.withTypeExpression(visitAndCast(typeConstraint.getTypeExpression(), p));
        return typeConstraint;
    }

    public J visitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, P p) {
        allowsConstraintClause = allowsConstraintClause.withPrefix(visitSpace(allowsConstraintClause.getPrefix(), CsSpace.Location.ALLOWS_CONSTRAINT_CLAUSE_PREFIX, p));
        allowsConstraintClause = allowsConstraintClause.withMarkers(visitMarkers(allowsConstraintClause.getMarkers(), p));
        allowsConstraintClause = allowsConstraintClause.getPadding().withExpressions(visitContainer(allowsConstraintClause.getPadding().getExpressions(), CsContainer.Location.ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS, p));
        return allowsConstraintClause;
    }

    public J visitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, P p) {
        refStructConstraint = refStructConstraint.withPrefix(visitSpace(refStructConstraint.getPrefix(), CsSpace.Location.REF_STRUCT_CONSTRAINT_PREFIX, p));
        refStructConstraint = refStructConstraint.withMarkers(visitMarkers(refStructConstraint.getMarkers(), p));
        return refStructConstraint;
    }

    public J visitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, P p) {
        classOrStructConstraint = classOrStructConstraint.withPrefix(visitSpace(classOrStructConstraint.getPrefix(), CsSpace.Location.CLASS_OR_STRUCT_CONSTRAINT_PREFIX, p));
        classOrStructConstraint = classOrStructConstraint.withMarkers(visitMarkers(classOrStructConstraint.getMarkers(), p));
        return classOrStructConstraint;
    }

    public J visitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, P p) {
        constructorConstraint = constructorConstraint.withPrefix(visitSpace(constructorConstraint.getPrefix(), CsSpace.Location.CONSTRUCTOR_CONSTRAINT_PREFIX, p));
        constructorConstraint = constructorConstraint.withMarkers(visitMarkers(constructorConstraint.getMarkers(), p));
        return constructorConstraint;
    }

    public J visitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, P p) {
        defaultConstraint = defaultConstraint.withPrefix(visitSpace(defaultConstraint.getPrefix(), CsSpace.Location.DEFAULT_CONSTRAINT_PREFIX, p));
        defaultConstraint = defaultConstraint.withMarkers(visitMarkers(defaultConstraint.getMarkers(), p));
        return defaultConstraint;
    }

    public J visitDeclarationExpression(Cs.DeclarationExpression declarationExpression, P p) {
        declarationExpression = declarationExpression.withPrefix(visitSpace(declarationExpression.getPrefix(), CsSpace.Location.DECLARATION_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(declarationExpression, p);
        if (!(tempExpression instanceof Cs.DeclarationExpression))
        {
            return tempExpression;
        }
        declarationExpression = (Cs.DeclarationExpression) tempExpression;
        declarationExpression = declarationExpression.withMarkers(visitMarkers(declarationExpression.getMarkers(), p));
        declarationExpression = declarationExpression.withTypeExpression(visitAndCast(declarationExpression.getTypeExpression(), p));
        declarationExpression = declarationExpression.withVariables(visitAndCast(declarationExpression.getVariables(), p));
        return declarationExpression;
    }

    public J visitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, P p) {
        singleVariableDesignation = singleVariableDesignation.withPrefix(visitSpace(singleVariableDesignation.getPrefix(), CsSpace.Location.SINGLE_VARIABLE_DESIGNATION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(singleVariableDesignation, p);
        if (!(tempExpression instanceof Cs.SingleVariableDesignation))
        {
            return tempExpression;
        }
        singleVariableDesignation = (Cs.SingleVariableDesignation) tempExpression;
        singleVariableDesignation = singleVariableDesignation.withMarkers(visitMarkers(singleVariableDesignation.getMarkers(), p));
        singleVariableDesignation = singleVariableDesignation.withName(visitAndCast(singleVariableDesignation.getName(), p));
        return singleVariableDesignation;
    }

    public J visitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, P p) {
        parenthesizedVariableDesignation = parenthesizedVariableDesignation.withPrefix(visitSpace(parenthesizedVariableDesignation.getPrefix(), CsSpace.Location.PARENTHESIZED_VARIABLE_DESIGNATION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(parenthesizedVariableDesignation, p);
        if (!(tempExpression instanceof Cs.ParenthesizedVariableDesignation))
        {
            return tempExpression;
        }
        parenthesizedVariableDesignation = (Cs.ParenthesizedVariableDesignation) tempExpression;
        parenthesizedVariableDesignation = parenthesizedVariableDesignation.withMarkers(visitMarkers(parenthesizedVariableDesignation.getMarkers(), p));
        parenthesizedVariableDesignation = parenthesizedVariableDesignation.getPadding().withVariables(visitContainer(parenthesizedVariableDesignation.getPadding().getVariables(), CsContainer.Location.PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES, p));
        return parenthesizedVariableDesignation;
    }

    public J visitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, P p) {
        discardVariableDesignation = discardVariableDesignation.withPrefix(visitSpace(discardVariableDesignation.getPrefix(), CsSpace.Location.DISCARD_VARIABLE_DESIGNATION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(discardVariableDesignation, p);
        if (!(tempExpression instanceof Cs.DiscardVariableDesignation))
        {
            return tempExpression;
        }
        discardVariableDesignation = (Cs.DiscardVariableDesignation) tempExpression;
        discardVariableDesignation = discardVariableDesignation.withMarkers(visitMarkers(discardVariableDesignation.getMarkers(), p));
        discardVariableDesignation = discardVariableDesignation.withDiscard(visitAndCast(discardVariableDesignation.getDiscard(), p));
        return discardVariableDesignation;
    }

    public J visitTupleExpression(Cs.TupleExpression tupleExpression, P p) {
        tupleExpression = tupleExpression.withPrefix(visitSpace(tupleExpression.getPrefix(), CsSpace.Location.TUPLE_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(tupleExpression, p);
        if (!(tempExpression instanceof Cs.TupleExpression))
        {
            return tempExpression;
        }
        tupleExpression = (Cs.TupleExpression) tempExpression;
        tupleExpression = tupleExpression.withMarkers(visitMarkers(tupleExpression.getMarkers(), p));
        tupleExpression = tupleExpression.getPadding().withArguments(visitContainer(tupleExpression.getPadding().getArguments(), CsContainer.Location.TUPLE_EXPRESSION_ARGUMENTS, p));
        return tupleExpression;
    }

    public J visitConstructor(Cs.Constructor constructor, P p) {
        constructor = constructor.withPrefix(visitSpace(constructor.getPrefix(), CsSpace.Location.CONSTRUCTOR_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(constructor, p);
        if (!(tempStatement instanceof Cs.Constructor))
        {
            return tempStatement;
        }
        constructor = (Cs.Constructor) tempStatement;
        constructor = constructor.withMarkers(visitMarkers(constructor.getMarkers(), p));
        constructor = constructor.withInitializer(visitAndCast(constructor.getInitializer(), p));
        constructor = constructor.withConstructorCore(visitAndCast(constructor.getConstructorCore(), p));
        return constructor;
    }

    public J visitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, P p) {
        destructorDeclaration = destructorDeclaration.withPrefix(visitSpace(destructorDeclaration.getPrefix(), CsSpace.Location.DESTRUCTOR_DECLARATION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(destructorDeclaration, p);
        if (!(tempStatement instanceof Cs.DestructorDeclaration))
        {
            return tempStatement;
        }
        destructorDeclaration = (Cs.DestructorDeclaration) tempStatement;
        destructorDeclaration = destructorDeclaration.withMarkers(visitMarkers(destructorDeclaration.getMarkers(), p));
        destructorDeclaration = destructorDeclaration.withInitializer(visitAndCast(destructorDeclaration.getInitializer(), p));
        destructorDeclaration = destructorDeclaration.withConstructorCore(visitAndCast(destructorDeclaration.getConstructorCore(), p));
        return destructorDeclaration;
    }

    public J visitUnary(Cs.Unary unary, P p) {
        unary = unary.withPrefix(visitSpace(unary.getPrefix(), CsSpace.Location.UNARY_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(unary, p);
        if (!(tempStatement instanceof Cs.Unary))
        {
            return tempStatement;
        }
        unary = (Cs.Unary) tempStatement;
        Expression tempExpression = (Expression) visitExpression(unary, p);
        if (!(tempExpression instanceof Cs.Unary))
        {
            return tempExpression;
        }
        unary = (Cs.Unary) tempExpression;
        unary = unary.withMarkers(visitMarkers(unary.getMarkers(), p));
        unary = unary.getPadding().withOperator(visitLeftPadded(unary.getPadding().getOperator(), CsLeftPadded.Location.UNARY_OPERATOR, p));
        unary = unary.withExpression(visitAndCast(unary.getExpression(), p));
        return unary;
    }

    public J visitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, P p) {
        constructorInitializer = constructorInitializer.withPrefix(visitSpace(constructorInitializer.getPrefix(), CsSpace.Location.CONSTRUCTOR_INITIALIZER_PREFIX, p));
        constructorInitializer = constructorInitializer.withMarkers(visitMarkers(constructorInitializer.getMarkers(), p));
        constructorInitializer = constructorInitializer.withKeyword(visitAndCast(constructorInitializer.getKeyword(), p));
        constructorInitializer = constructorInitializer.getPadding().withArguments(visitContainer(constructorInitializer.getPadding().getArguments(), CsContainer.Location.CONSTRUCTOR_INITIALIZER_ARGUMENTS, p));
        return constructorInitializer;
    }

    public J visitTupleType(Cs.TupleType tupleType, P p) {
        tupleType = tupleType.withPrefix(visitSpace(tupleType.getPrefix(), CsSpace.Location.TUPLE_TYPE_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(tupleType, p);
        if (!(tempExpression instanceof Cs.TupleType))
        {
            return tempExpression;
        }
        tupleType = (Cs.TupleType) tempExpression;
        tupleType = tupleType.withMarkers(visitMarkers(tupleType.getMarkers(), p));
        tupleType = tupleType.getPadding().withElements(visitContainer(tupleType.getPadding().getElements(), CsContainer.Location.TUPLE_TYPE_ELEMENTS, p));
        return tupleType;
    }

    public J visitTupleElement(Cs.TupleElement tupleElement, P p) {
        tupleElement = tupleElement.withPrefix(visitSpace(tupleElement.getPrefix(), CsSpace.Location.TUPLE_ELEMENT_PREFIX, p));
        tupleElement = tupleElement.withMarkers(visitMarkers(tupleElement.getMarkers(), p));
        tupleElement = tupleElement.withType(visitAndCast(tupleElement.getType(), p));
        tupleElement = tupleElement.withName(visitAndCast(tupleElement.getName(), p));
        return tupleElement;
    }

    public J visitNewClass(Cs.NewClass newClass, P p) {
        newClass = newClass.withPrefix(visitSpace(newClass.getPrefix(), CsSpace.Location.NEW_CLASS_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(newClass, p);
        if (!(tempStatement instanceof Cs.NewClass))
        {
            return tempStatement;
        }
        newClass = (Cs.NewClass) tempStatement;
        Expression tempExpression = (Expression) visitExpression(newClass, p);
        if (!(tempExpression instanceof Cs.NewClass))
        {
            return tempExpression;
        }
        newClass = (Cs.NewClass) tempExpression;
        newClass = newClass.withMarkers(visitMarkers(newClass.getMarkers(), p));
        newClass = newClass.withNewClassCore(visitAndCast(newClass.getNewClassCore(), p));
        newClass = newClass.withInitializer(visitAndCast(newClass.getInitializer(), p));
        return newClass;
    }

    public J visitInitializerExpression(Cs.InitializerExpression initializerExpression, P p) {
        initializerExpression = initializerExpression.withPrefix(visitSpace(initializerExpression.getPrefix(), CsSpace.Location.INITIALIZER_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(initializerExpression, p);
        if (!(tempExpression instanceof Cs.InitializerExpression))
        {
            return tempExpression;
        }
        initializerExpression = (Cs.InitializerExpression) tempExpression;
        initializerExpression = initializerExpression.withMarkers(visitMarkers(initializerExpression.getMarkers(), p));
        initializerExpression = initializerExpression.getPadding().withExpressions(visitContainer(initializerExpression.getPadding().getExpressions(), CsContainer.Location.INITIALIZER_EXPRESSION_EXPRESSIONS, p));
        return initializerExpression;
    }

    public J visitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, P p) {
        implicitElementAccess = implicitElementAccess.withPrefix(visitSpace(implicitElementAccess.getPrefix(), CsSpace.Location.IMPLICIT_ELEMENT_ACCESS_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(implicitElementAccess, p);
        if (!(tempExpression instanceof Cs.ImplicitElementAccess))
        {
            return tempExpression;
        }
        implicitElementAccess = (Cs.ImplicitElementAccess) tempExpression;
        implicitElementAccess = implicitElementAccess.withMarkers(visitMarkers(implicitElementAccess.getMarkers(), p));
        implicitElementAccess = implicitElementAccess.getPadding().withArgumentList(visitContainer(implicitElementAccess.getPadding().getArgumentList(), CsContainer.Location.IMPLICIT_ELEMENT_ACCESS_ARGUMENT_LIST, p));
        return implicitElementAccess;
    }

    public J visitYield(Cs.Yield yield, P p) {
        yield = yield.withPrefix(visitSpace(yield.getPrefix(), CsSpace.Location.YIELD_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(yield, p);
        if (!(tempStatement instanceof Cs.Yield))
        {
            return tempStatement;
        }
        yield = (Cs.Yield) tempStatement;
        yield = yield.withMarkers(visitMarkers(yield.getMarkers(), p));
        yield = yield.withReturnOrBreakKeyword(visitAndCast(yield.getReturnOrBreakKeyword(), p));
        yield = yield.withExpression(visitAndCast(yield.getExpression(), p));
        return yield;
    }

    public J visitDefaultExpression(Cs.DefaultExpression defaultExpression, P p) {
        defaultExpression = defaultExpression.withPrefix(visitSpace(defaultExpression.getPrefix(), CsSpace.Location.DEFAULT_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(defaultExpression, p);
        if (!(tempExpression instanceof Cs.DefaultExpression))
        {
            return tempExpression;
        }
        defaultExpression = (Cs.DefaultExpression) tempExpression;
        defaultExpression = defaultExpression.withMarkers(visitMarkers(defaultExpression.getMarkers(), p));
        defaultExpression = defaultExpression.getPadding().withTypeOperator(visitContainer(defaultExpression.getPadding().getTypeOperator(), CsContainer.Location.DEFAULT_EXPRESSION_TYPE_OPERATOR, p));
        return defaultExpression;
    }

    public J visitIsPattern(Cs.IsPattern isPattern, P p) {
        isPattern = isPattern.withPrefix(visitSpace(isPattern.getPrefix(), CsSpace.Location.IS_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(isPattern, p);
        if (!(tempExpression instanceof Cs.IsPattern))
        {
            return tempExpression;
        }
        isPattern = (Cs.IsPattern) tempExpression;
        isPattern = isPattern.withMarkers(visitMarkers(isPattern.getMarkers(), p));
        isPattern = isPattern.withExpression(visitAndCast(isPattern.getExpression(), p));
        isPattern = isPattern.getPadding().withPattern(visitLeftPadded(isPattern.getPadding().getPattern(), CsLeftPadded.Location.IS_PATTERN_PATTERN, p));
        return isPattern;
    }

    public J visitUnaryPattern(Cs.UnaryPattern unaryPattern, P p) {
        unaryPattern = unaryPattern.withPrefix(visitSpace(unaryPattern.getPrefix(), CsSpace.Location.UNARY_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(unaryPattern, p);
        if (!(tempExpression instanceof Cs.UnaryPattern))
        {
            return tempExpression;
        }
        unaryPattern = (Cs.UnaryPattern) tempExpression;
        unaryPattern = unaryPattern.withMarkers(visitMarkers(unaryPattern.getMarkers(), p));
        unaryPattern = unaryPattern.withOperator(visitAndCast(unaryPattern.getOperator(), p));
        unaryPattern = unaryPattern.withPattern(visitAndCast(unaryPattern.getPattern(), p));
        return unaryPattern;
    }

    public J visitTypePattern(Cs.TypePattern typePattern, P p) {
        typePattern = typePattern.withPrefix(visitSpace(typePattern.getPrefix(), CsSpace.Location.TYPE_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(typePattern, p);
        if (!(tempExpression instanceof Cs.TypePattern))
        {
            return tempExpression;
        }
        typePattern = (Cs.TypePattern) tempExpression;
        typePattern = typePattern.withMarkers(visitMarkers(typePattern.getMarkers(), p));
        typePattern = typePattern.withTypeIdentifier(visitAndCast(typePattern.getTypeIdentifier(), p));
        typePattern = typePattern.withDesignation(visitAndCast(typePattern.getDesignation(), p));
        return typePattern;
    }

    public J visitBinaryPattern(Cs.BinaryPattern binaryPattern, P p) {
        binaryPattern = binaryPattern.withPrefix(visitSpace(binaryPattern.getPrefix(), CsSpace.Location.BINARY_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(binaryPattern, p);
        if (!(tempExpression instanceof Cs.BinaryPattern))
        {
            return tempExpression;
        }
        binaryPattern = (Cs.BinaryPattern) tempExpression;
        binaryPattern = binaryPattern.withMarkers(visitMarkers(binaryPattern.getMarkers(), p));
        binaryPattern = binaryPattern.withLeft(visitAndCast(binaryPattern.getLeft(), p));
        binaryPattern = binaryPattern.getPadding().withOperator(visitLeftPadded(binaryPattern.getPadding().getOperator(), CsLeftPadded.Location.BINARY_PATTERN_OPERATOR, p));
        binaryPattern = binaryPattern.withRight(visitAndCast(binaryPattern.getRight(), p));
        return binaryPattern;
    }

    public J visitConstantPattern(Cs.ConstantPattern constantPattern, P p) {
        constantPattern = constantPattern.withPrefix(visitSpace(constantPattern.getPrefix(), CsSpace.Location.CONSTANT_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(constantPattern, p);
        if (!(tempExpression instanceof Cs.ConstantPattern))
        {
            return tempExpression;
        }
        constantPattern = (Cs.ConstantPattern) tempExpression;
        constantPattern = constantPattern.withMarkers(visitMarkers(constantPattern.getMarkers(), p));
        constantPattern = constantPattern.withValue(visitAndCast(constantPattern.getValue(), p));
        return constantPattern;
    }

    public J visitDiscardPattern(Cs.DiscardPattern discardPattern, P p) {
        discardPattern = discardPattern.withPrefix(visitSpace(discardPattern.getPrefix(), CsSpace.Location.DISCARD_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(discardPattern, p);
        if (!(tempExpression instanceof Cs.DiscardPattern))
        {
            return tempExpression;
        }
        discardPattern = (Cs.DiscardPattern) tempExpression;
        discardPattern = discardPattern.withMarkers(visitMarkers(discardPattern.getMarkers(), p));
        return discardPattern;
    }

    public J visitListPattern(Cs.ListPattern listPattern, P p) {
        listPattern = listPattern.withPrefix(visitSpace(listPattern.getPrefix(), CsSpace.Location.LIST_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(listPattern, p);
        if (!(tempExpression instanceof Cs.ListPattern))
        {
            return tempExpression;
        }
        listPattern = (Cs.ListPattern) tempExpression;
        listPattern = listPattern.withMarkers(visitMarkers(listPattern.getMarkers(), p));
        listPattern = listPattern.getPadding().withPatterns(visitContainer(listPattern.getPadding().getPatterns(), CsContainer.Location.LIST_PATTERN_PATTERNS, p));
        listPattern = listPattern.withDesignation(visitAndCast(listPattern.getDesignation(), p));
        return listPattern;
    }

    public J visitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, P p) {
        parenthesizedPattern = parenthesizedPattern.withPrefix(visitSpace(parenthesizedPattern.getPrefix(), CsSpace.Location.PARENTHESIZED_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(parenthesizedPattern, p);
        if (!(tempExpression instanceof Cs.ParenthesizedPattern))
        {
            return tempExpression;
        }
        parenthesizedPattern = (Cs.ParenthesizedPattern) tempExpression;
        parenthesizedPattern = parenthesizedPattern.withMarkers(visitMarkers(parenthesizedPattern.getMarkers(), p));
        parenthesizedPattern = parenthesizedPattern.getPadding().withPattern(visitContainer(parenthesizedPattern.getPadding().getPattern(), CsContainer.Location.PARENTHESIZED_PATTERN_PATTERN, p));
        return parenthesizedPattern;
    }

    public J visitRecursivePattern(Cs.RecursivePattern recursivePattern, P p) {
        recursivePattern = recursivePattern.withPrefix(visitSpace(recursivePattern.getPrefix(), CsSpace.Location.RECURSIVE_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(recursivePattern, p);
        if (!(tempExpression instanceof Cs.RecursivePattern))
        {
            return tempExpression;
        }
        recursivePattern = (Cs.RecursivePattern) tempExpression;
        recursivePattern = recursivePattern.withMarkers(visitMarkers(recursivePattern.getMarkers(), p));
        recursivePattern = recursivePattern.withTypeQualifier(visitAndCast(recursivePattern.getTypeQualifier(), p));
        recursivePattern = recursivePattern.withPositionalPattern(visitAndCast(recursivePattern.getPositionalPattern(), p));
        recursivePattern = recursivePattern.withPropertyPattern(visitAndCast(recursivePattern.getPropertyPattern(), p));
        recursivePattern = recursivePattern.withDesignation(visitAndCast(recursivePattern.getDesignation(), p));
        return recursivePattern;
    }

    public J visitVarPattern(Cs.VarPattern varPattern, P p) {
        varPattern = varPattern.withPrefix(visitSpace(varPattern.getPrefix(), CsSpace.Location.VAR_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(varPattern, p);
        if (!(tempExpression instanceof Cs.VarPattern))
        {
            return tempExpression;
        }
        varPattern = (Cs.VarPattern) tempExpression;
        varPattern = varPattern.withMarkers(visitMarkers(varPattern.getMarkers(), p));
        varPattern = varPattern.withDesignation(visitAndCast(varPattern.getDesignation(), p));
        return varPattern;
    }

    public J visitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, P p) {
        positionalPatternClause = positionalPatternClause.withPrefix(visitSpace(positionalPatternClause.getPrefix(), CsSpace.Location.POSITIONAL_PATTERN_CLAUSE_PREFIX, p));
        positionalPatternClause = positionalPatternClause.withMarkers(visitMarkers(positionalPatternClause.getMarkers(), p));
        positionalPatternClause = positionalPatternClause.getPadding().withSubpatterns(visitContainer(positionalPatternClause.getPadding().getSubpatterns(), CsContainer.Location.POSITIONAL_PATTERN_CLAUSE_SUBPATTERNS, p));
        return positionalPatternClause;
    }

    public J visitRelationalPattern(Cs.RelationalPattern relationalPattern, P p) {
        relationalPattern = relationalPattern.withPrefix(visitSpace(relationalPattern.getPrefix(), CsSpace.Location.RELATIONAL_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(relationalPattern, p);
        if (!(tempExpression instanceof Cs.RelationalPattern))
        {
            return tempExpression;
        }
        relationalPattern = (Cs.RelationalPattern) tempExpression;
        relationalPattern = relationalPattern.withMarkers(visitMarkers(relationalPattern.getMarkers(), p));
        relationalPattern = relationalPattern.getPadding().withOperator(visitLeftPadded(relationalPattern.getPadding().getOperator(), CsLeftPadded.Location.RELATIONAL_PATTERN_OPERATOR, p));
        relationalPattern = relationalPattern.withValue(visitAndCast(relationalPattern.getValue(), p));
        return relationalPattern;
    }

    public J visitSlicePattern(Cs.SlicePattern slicePattern, P p) {
        slicePattern = slicePattern.withPrefix(visitSpace(slicePattern.getPrefix(), CsSpace.Location.SLICE_PATTERN_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(slicePattern, p);
        if (!(tempExpression instanceof Cs.SlicePattern))
        {
            return tempExpression;
        }
        slicePattern = (Cs.SlicePattern) tempExpression;
        slicePattern = slicePattern.withMarkers(visitMarkers(slicePattern.getMarkers(), p));
        return slicePattern;
    }

    public J visitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, P p) {
        propertyPatternClause = propertyPatternClause.withPrefix(visitSpace(propertyPatternClause.getPrefix(), CsSpace.Location.PROPERTY_PATTERN_CLAUSE_PREFIX, p));
        propertyPatternClause = propertyPatternClause.withMarkers(visitMarkers(propertyPatternClause.getMarkers(), p));
        propertyPatternClause = propertyPatternClause.getPadding().withSubpatterns(visitContainer(propertyPatternClause.getPadding().getSubpatterns(), CsContainer.Location.PROPERTY_PATTERN_CLAUSE_SUBPATTERNS, p));
        return propertyPatternClause;
    }

    public J visitSubpattern(Cs.Subpattern subpattern, P p) {
        subpattern = subpattern.withPrefix(visitSpace(subpattern.getPrefix(), CsSpace.Location.SUBPATTERN_PREFIX, p));
        subpattern = subpattern.withMarkers(visitMarkers(subpattern.getMarkers(), p));
        subpattern = subpattern.withName(visitAndCast(subpattern.getName(), p));
        subpattern = subpattern.getPadding().withPattern(visitLeftPadded(subpattern.getPadding().getPattern(), CsLeftPadded.Location.SUBPATTERN_PATTERN, p));
        return subpattern;
    }

    public J visitSwitchExpression(Cs.SwitchExpression switchExpression, P p) {
        switchExpression = switchExpression.withPrefix(visitSpace(switchExpression.getPrefix(), CsSpace.Location.SWITCH_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(switchExpression, p);
        if (!(tempExpression instanceof Cs.SwitchExpression))
        {
            return tempExpression;
        }
        switchExpression = (Cs.SwitchExpression) tempExpression;
        switchExpression = switchExpression.withMarkers(visitMarkers(switchExpression.getMarkers(), p));
        switchExpression = switchExpression.getPadding().withExpression(visitRightPadded(switchExpression.getPadding().getExpression(), CsRightPadded.Location.SWITCH_EXPRESSION_EXPRESSION, p));
        switchExpression = switchExpression.getPadding().withArms(visitContainer(switchExpression.getPadding().getArms(), CsContainer.Location.SWITCH_EXPRESSION_ARMS, p));
        return switchExpression;
    }

    public J visitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, P p) {
        switchExpressionArm = switchExpressionArm.withPrefix(visitSpace(switchExpressionArm.getPrefix(), CsSpace.Location.SWITCH_EXPRESSION_ARM_PREFIX, p));
        switchExpressionArm = switchExpressionArm.withMarkers(visitMarkers(switchExpressionArm.getMarkers(), p));
        switchExpressionArm = switchExpressionArm.withPattern(visitAndCast(switchExpressionArm.getPattern(), p));
        switchExpressionArm = switchExpressionArm.getPadding().withWhenExpression(visitLeftPadded(switchExpressionArm.getPadding().getWhenExpression(), CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION, p));
        switchExpressionArm = switchExpressionArm.getPadding().withExpression(visitLeftPadded(switchExpressionArm.getPadding().getExpression(), CsLeftPadded.Location.SWITCH_EXPRESSION_ARM_EXPRESSION, p));
        return switchExpressionArm;
    }

    public J visitSwitchSection(Cs.SwitchSection switchSection, P p) {
        switchSection = switchSection.withPrefix(visitSpace(switchSection.getPrefix(), CsSpace.Location.SWITCH_SECTION_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(switchSection, p);
        if (!(tempStatement instanceof Cs.SwitchSection))
        {
            return tempStatement;
        }
        switchSection = (Cs.SwitchSection) tempStatement;
        switchSection = switchSection.withMarkers(visitMarkers(switchSection.getMarkers(), p));
        switchSection = switchSection.withLabels(ListUtils.map(switchSection.getLabels(), el -> (Cs.SwitchLabel)visit(el, p)));
        switchSection = switchSection.getPadding().withStatements(ListUtils.map(switchSection.getPadding().getStatements(), el -> visitRightPadded(el, CsRightPadded.Location.SWITCH_SECTION_STATEMENTS, p)));
        return switchSection;
    }

    public J visitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, P p) {
        defaultSwitchLabel = defaultSwitchLabel.withPrefix(visitSpace(defaultSwitchLabel.getPrefix(), CsSpace.Location.DEFAULT_SWITCH_LABEL_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(defaultSwitchLabel, p);
        if (!(tempExpression instanceof Cs.DefaultSwitchLabel))
        {
            return tempExpression;
        }
        defaultSwitchLabel = (Cs.DefaultSwitchLabel) tempExpression;
        defaultSwitchLabel = defaultSwitchLabel.withMarkers(visitMarkers(defaultSwitchLabel.getMarkers(), p));
        defaultSwitchLabel = defaultSwitchLabel.withColonToken(visitSpace(defaultSwitchLabel.getColonToken(), CsSpace.Location.DEFAULT_SWITCH_LABEL_COLON_TOKEN, p));
        return defaultSwitchLabel;
    }

    public J visitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, P p) {
        casePatternSwitchLabel = casePatternSwitchLabel.withPrefix(visitSpace(casePatternSwitchLabel.getPrefix(), CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(casePatternSwitchLabel, p);
        if (!(tempExpression instanceof Cs.CasePatternSwitchLabel))
        {
            return tempExpression;
        }
        casePatternSwitchLabel = (Cs.CasePatternSwitchLabel) tempExpression;
        casePatternSwitchLabel = casePatternSwitchLabel.withMarkers(visitMarkers(casePatternSwitchLabel.getMarkers(), p));
        casePatternSwitchLabel = casePatternSwitchLabel.withPattern(visitAndCast(casePatternSwitchLabel.getPattern(), p));
        casePatternSwitchLabel = casePatternSwitchLabel.getPadding().withWhenClause(visitLeftPadded(casePatternSwitchLabel.getPadding().getWhenClause(), CsLeftPadded.Location.CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE, p));
        casePatternSwitchLabel = casePatternSwitchLabel.withColonToken(visitSpace(casePatternSwitchLabel.getColonToken(), CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, p));
        return casePatternSwitchLabel;
    }

    public J visitSwitchStatement(Cs.SwitchStatement switchStatement, P p) {
        switchStatement = switchStatement.withPrefix(visitSpace(switchStatement.getPrefix(), CsSpace.Location.SWITCH_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(switchStatement, p);
        if (!(tempStatement instanceof Cs.SwitchStatement))
        {
            return tempStatement;
        }
        switchStatement = (Cs.SwitchStatement) tempStatement;
        switchStatement = switchStatement.withMarkers(visitMarkers(switchStatement.getMarkers(), p));
        switchStatement = switchStatement.getPadding().withExpression(visitContainer(switchStatement.getPadding().getExpression(), CsContainer.Location.SWITCH_STATEMENT_EXPRESSION, p));
        switchStatement = switchStatement.getPadding().withSections(visitContainer(switchStatement.getPadding().getSections(), CsContainer.Location.SWITCH_STATEMENT_SECTIONS, p));
        return switchStatement;
    }

    public J visitLockStatement(Cs.LockStatement lockStatement, P p) {
        lockStatement = lockStatement.withPrefix(visitSpace(lockStatement.getPrefix(), CsSpace.Location.LOCK_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(lockStatement, p);
        if (!(tempStatement instanceof Cs.LockStatement))
        {
            return tempStatement;
        }
        lockStatement = (Cs.LockStatement) tempStatement;
        lockStatement = lockStatement.withMarkers(visitMarkers(lockStatement.getMarkers(), p));
        lockStatement = lockStatement.withExpression(visitAndCast(lockStatement.getExpression(), p));
        lockStatement = lockStatement.getPadding().withStatement(visitRightPadded(lockStatement.getPadding().getStatement(), CsRightPadded.Location.LOCK_STATEMENT_STATEMENT, p));
        return lockStatement;
    }

    public J visitFixedStatement(Cs.FixedStatement fixedStatement, P p) {
        fixedStatement = fixedStatement.withPrefix(visitSpace(fixedStatement.getPrefix(), CsSpace.Location.FIXED_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(fixedStatement, p);
        if (!(tempStatement instanceof Cs.FixedStatement))
        {
            return tempStatement;
        }
        fixedStatement = (Cs.FixedStatement) tempStatement;
        fixedStatement = fixedStatement.withMarkers(visitMarkers(fixedStatement.getMarkers(), p));
        fixedStatement = fixedStatement.withDeclarations(visitAndCast(fixedStatement.getDeclarations(), p));
        fixedStatement = fixedStatement.withBlock(visitAndCast(fixedStatement.getBlock(), p));
        return fixedStatement;
    }

    public J visitCheckedStatement(Cs.CheckedStatement checkedStatement, P p) {
        checkedStatement = checkedStatement.withPrefix(visitSpace(checkedStatement.getPrefix(), CsSpace.Location.CHECKED_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(checkedStatement, p);
        if (!(tempStatement instanceof Cs.CheckedStatement))
        {
            return tempStatement;
        }
        checkedStatement = (Cs.CheckedStatement) tempStatement;
        checkedStatement = checkedStatement.withMarkers(visitMarkers(checkedStatement.getMarkers(), p));
        checkedStatement = checkedStatement.withBlock(visitAndCast(checkedStatement.getBlock(), p));
        return checkedStatement;
    }

    public J visitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, P p) {
        unsafeStatement = unsafeStatement.withPrefix(visitSpace(unsafeStatement.getPrefix(), CsSpace.Location.UNSAFE_STATEMENT_PREFIX, p));
        Statement tempStatement = (Statement) visitStatement(unsafeStatement, p);
        if (!(tempStatement instanceof Cs.UnsafeStatement))
        {
            return tempStatement;
        }
        unsafeStatement = (Cs.UnsafeStatement) tempStatement;
        unsafeStatement = unsafeStatement.withMarkers(visitMarkers(unsafeStatement.getMarkers(), p));
        unsafeStatement = unsafeStatement.withBlock(visitAndCast(unsafeStatement.getBlock(), p));
        return unsafeStatement;
    }

    public J visitRangeExpression(Cs.RangeExpression rangeExpression, P p) {
        rangeExpression = rangeExpression.withPrefix(visitSpace(rangeExpression.getPrefix(), CsSpace.Location.RANGE_EXPRESSION_PREFIX, p));
        Expression tempExpression = (Expression) visitExpression(rangeExpression, p);
        if (!(tempExpression instanceof Cs.RangeExpression))
        {
            return tempExpression;
        }
        rangeExpression = (Cs.RangeExpression) tempExpression;
        rangeExpression = rangeExpression.withMarkers(visitMarkers(rangeExpression.getMarkers(), p));
        rangeExpression = rangeExpression.getPadding().withStart(visitRightPadded(rangeExpression.getPadding().getStart(), CsRightPadded.Location.RANGE_EXPRESSION_START, p));
        rangeExpression = rangeExpression.withEnd(visitAndCast(rangeExpression.getEnd(), p));
        return rangeExpression;
    }

    public <J2 extends J> JContainer<J2> visitContainer(@Nullable JContainer<J2> container,
                                                        CsContainer.Location loc, P p) {
        if (container == null) {
            //noinspection ConstantConditions
            return null;
        }
        setCursor(new Cursor(getCursor(), container));

        Space before = visitSpace(container.getBefore(), loc.getBeforeLocation(), p);
        List<JRightPadded<J2>> js = ListUtils.map(container.getPadding().getElements(), t -> visitRightPadded(t, loc.getElementLocation(), p));

        setCursor(getCursor().getParent());

        return js == container.getPadding().getElements() && before == container.getBefore() ?
                container :
                JContainer.build(before, js, container.getMarkers());
    }

    public <T> JLeftPadded<T> visitLeftPadded(@Nullable JLeftPadded<T> left, CsLeftPadded.Location loc, P p) {
        if (left == null) {
            //noinspection ConstantConditions
            return null;
        }

        setCursor(new Cursor(getCursor(), left));

        Space before = visitSpace(left.getBefore(), loc.getBeforeLocation(), p);
        T t = left.getElement();

        if (t instanceof J) {
            //noinspection unchecked
            t = visitAndCast((J) left.getElement(), p);
        }

        setCursor(getCursor().getParent());
        if (t == null) {
            // If nothing changed leave AST node the same
            if (left.getElement() == null && before == left.getBefore()) {
                return left;
            }
            //noinspection ConstantConditions
            return null;
        }

        return (before == left.getBefore() && t == left.getElement()) ? left : new JLeftPadded<>(before, t, left.getMarkers());
    }

    public <T> JRightPadded<T> visitRightPadded(@Nullable JRightPadded<T> right, CsRightPadded.Location loc, P p) {
        if (right == null) {
            //noinspection ConstantConditions
            return null;
        }

        setCursor(new Cursor(getCursor(), right));

        T t = right.getElement();
        if (t instanceof J) {
            //noinspection unchecked
            t = visitAndCast((J) right.getElement(), p);
        }

        setCursor(getCursor().getParent());
        if (t == null) {
            //noinspection ConstantConditions
            return null;
        }

        Space after = visitSpace(right.getAfter(), loc.getAfterLocation(), p);
        Markers markers = visitMarkers(right.getMarkers(), p);
        return (after == right.getAfter() && t == right.getElement() && markers == right.getMarkers()) ?
                right : new JRightPadded<>(t, after, markers);
    }

    public Space visitSpace(Space space, CsSpace.Location loc, P p) {
        return visitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }
}
