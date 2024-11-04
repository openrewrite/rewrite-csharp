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
