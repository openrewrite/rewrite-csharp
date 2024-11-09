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

package org.openrewrite.csharp.remote;

import lombok.Value;
import org.jspecify.annotations.Nullable;
import org.openrewrite.Checksum;
import org.openrewrite.Cursor;
import org.openrewrite.FileAttributes;
import org.openrewrite.Tree;
import org.openrewrite.marker.Markers;
import org.openrewrite.csharp.CSharpVisitor;
import org.openrewrite.csharp.tree.*;
import org.openrewrite.java.*;
import org.openrewrite.java.tree.*;
import org.openrewrite.remote.Receiver;
import org.openrewrite.remote.ReceiverContext;
import org.openrewrite.remote.ReceiverFactory;

import java.nio.charset.Charset;
import java.nio.file.Path;
import java.util.List;
import java.util.UUID;

@Value
public class CSharpReceiver implements Receiver<Cs> {

    @Override
    public Cs receive(@Nullable Cs before, ReceiverContext ctx) {
        ReceiverContext forked = fork(ctx);
        //noinspection DataFlowIssue
        return (Cs) forked.getVisitor().visit(before, forked);
    }

    @Override
    public ReceiverContext fork(ReceiverContext ctx) {
        return ctx.fork(new Visitor(), new Factory());
    }

    private static class Visitor extends CSharpVisitor<ReceiverContext> {

        public @Nullable J visit(@Nullable Tree tree, ReceiverContext ctx) {
            //noinspection DataFlowIssue
            Cursor cursor = new Cursor(getCursor(), tree);
            setCursor(cursor);

            tree = ctx.receiveNode((J) tree, ctx::receiveTree);

            setCursor(cursor.getParent());
            return (J) tree;
        }

        @Override
        public Cs.CompilationUnit visitCompilationUnit(Cs.CompilationUnit compilationUnit, ReceiverContext ctx) {
            compilationUnit = compilationUnit.withId(ctx.receiveNonNullValue(compilationUnit.getId(), UUID.class));
            compilationUnit = compilationUnit.withPrefix(ctx.receiveNonNullNode(compilationUnit.getPrefix(), CSharpReceiver::receiveSpace));
            compilationUnit = compilationUnit.withMarkers(ctx.receiveNonNullNode(compilationUnit.getMarkers(), ctx::receiveMarkers));
            compilationUnit = compilationUnit.withSourcePath(ctx.receiveNonNullValue(compilationUnit.getSourcePath(), Path.class));
            compilationUnit = compilationUnit.withFileAttributes(ctx.receiveValue(compilationUnit.getFileAttributes(), FileAttributes.class));
            String charsetName = ctx.receiveValue(compilationUnit.getCharset().name(), String.class);
            if (charsetName != null) {
                compilationUnit = (Cs.CompilationUnit) compilationUnit.withCharset(Charset.forName(charsetName));
            }
            compilationUnit = compilationUnit.withCharsetBomMarked(ctx.receiveNonNullValue(compilationUnit.isCharsetBomMarked(), boolean.class));
            compilationUnit = compilationUnit.withChecksum(ctx.receiveValue(compilationUnit.getChecksum(), Checksum.class));
            compilationUnit = compilationUnit.getPadding().withExterns(ctx.receiveNonNullNodes(compilationUnit.getPadding().getExterns(), CSharpReceiver::receiveRightPaddedTree));
            compilationUnit = compilationUnit.getPadding().withUsings(ctx.receiveNonNullNodes(compilationUnit.getPadding().getUsings(), CSharpReceiver::receiveRightPaddedTree));
            compilationUnit = compilationUnit.withAttributeLists(ctx.receiveNonNullNodes(compilationUnit.getAttributeLists(), ctx::receiveTree));
            compilationUnit = compilationUnit.getPadding().withMembers(ctx.receiveNonNullNodes(compilationUnit.getPadding().getMembers(), CSharpReceiver::receiveRightPaddedTree));
            compilationUnit = compilationUnit.withEof(ctx.receiveNonNullNode(compilationUnit.getEof(), CSharpReceiver::receiveSpace));
            return compilationUnit;
        }

        @Override
        public Cs.ForEachVariableLoop visitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, ReceiverContext ctx) {
            forEachVariableLoop = forEachVariableLoop.withId(ctx.receiveNonNullValue(forEachVariableLoop.getId(), UUID.class));
            forEachVariableLoop = forEachVariableLoop.withPrefix(ctx.receiveNonNullNode(forEachVariableLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forEachVariableLoop = forEachVariableLoop.withMarkers(ctx.receiveNonNullNode(forEachVariableLoop.getMarkers(), ctx::receiveMarkers));
            forEachVariableLoop = forEachVariableLoop.withControlElement(ctx.receiveNonNullNode(forEachVariableLoop.getControlElement(), ctx::receiveTree));
            forEachVariableLoop = forEachVariableLoop.getPadding().withBody(ctx.receiveNonNullNode(forEachVariableLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return forEachVariableLoop;
        }

        @Override
        public Cs.ForEachVariableLoop.Control visitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withVariable(ctx.receiveNonNullNode(control.getPadding().getVariable(), CSharpReceiver::receiveRightPaddedTree));
            control = control.getPadding().withIterable(ctx.receiveNonNullNode(control.getPadding().getIterable(), CSharpReceiver::receiveRightPaddedTree));
            return control;
        }

        @Override
        public Cs.Argument visitArgument(Cs.Argument argument, ReceiverContext ctx) {
            argument = argument.withId(ctx.receiveNonNullValue(argument.getId(), UUID.class));
            argument = argument.withPrefix(ctx.receiveNonNullNode(argument.getPrefix(), CSharpReceiver::receiveSpace));
            argument = argument.withMarkers(ctx.receiveNonNullNode(argument.getMarkers(), ctx::receiveMarkers));
            argument = argument.getPadding().withNameColumn(ctx.receiveNode(argument.getPadding().getNameColumn(), CSharpReceiver::receiveRightPaddedTree));
            argument = argument.withRefKindKeyword(ctx.receiveNode(argument.getRefKindKeyword(), ctx::receiveTree));
            argument = argument.withExpression(ctx.receiveNonNullNode(argument.getExpression(), ctx::receiveTree));
            return argument;
        }

        @Override
        public Cs.AnnotatedStatement visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, ReceiverContext ctx) {
            annotatedStatement = annotatedStatement.withId(ctx.receiveNonNullValue(annotatedStatement.getId(), UUID.class));
            annotatedStatement = annotatedStatement.withPrefix(ctx.receiveNonNullNode(annotatedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            annotatedStatement = annotatedStatement.withMarkers(ctx.receiveNonNullNode(annotatedStatement.getMarkers(), ctx::receiveMarkers));
            annotatedStatement = annotatedStatement.withAttributeLists(ctx.receiveNonNullNodes(annotatedStatement.getAttributeLists(), ctx::receiveTree));
            annotatedStatement = annotatedStatement.withStatement(ctx.receiveNonNullNode(annotatedStatement.getStatement(), ctx::receiveTree));
            return annotatedStatement;
        }

        @Override
        public Cs.ArrayRankSpecifier visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, ReceiverContext ctx) {
            arrayRankSpecifier = arrayRankSpecifier.withId(ctx.receiveNonNullValue(arrayRankSpecifier.getId(), UUID.class));
            arrayRankSpecifier = arrayRankSpecifier.withPrefix(ctx.receiveNonNullNode(arrayRankSpecifier.getPrefix(), CSharpReceiver::receiveSpace));
            arrayRankSpecifier = arrayRankSpecifier.withMarkers(ctx.receiveNonNullNode(arrayRankSpecifier.getMarkers(), ctx::receiveMarkers));
            arrayRankSpecifier = arrayRankSpecifier.getPadding().withSizes(ctx.receiveNonNullNode(arrayRankSpecifier.getPadding().getSizes(), CSharpReceiver::receiveContainer));
            return arrayRankSpecifier;
        }

        @Override
        public Cs.AssignmentOperation visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, ReceiverContext ctx) {
            assignmentOperation = assignmentOperation.withId(ctx.receiveNonNullValue(assignmentOperation.getId(), UUID.class));
            assignmentOperation = assignmentOperation.withPrefix(ctx.receiveNonNullNode(assignmentOperation.getPrefix(), CSharpReceiver::receiveSpace));
            assignmentOperation = assignmentOperation.withMarkers(ctx.receiveNonNullNode(assignmentOperation.getMarkers(), ctx::receiveMarkers));
            assignmentOperation = assignmentOperation.withVariable(ctx.receiveNonNullNode(assignmentOperation.getVariable(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.getPadding().withOperator(ctx.receiveNonNullNode(assignmentOperation.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AssignmentOperation.OperatorType.class)));
            assignmentOperation = assignmentOperation.withAssignment(ctx.receiveNonNullNode(assignmentOperation.getAssignment(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.withType(ctx.receiveValue(assignmentOperation.getType(), JavaType.class));
            return assignmentOperation;
        }

        @Override
        public Cs.AttributeList visitAttributeList(Cs.AttributeList attributeList, ReceiverContext ctx) {
            attributeList = attributeList.withId(ctx.receiveNonNullValue(attributeList.getId(), UUID.class));
            attributeList = attributeList.withPrefix(ctx.receiveNonNullNode(attributeList.getPrefix(), CSharpReceiver::receiveSpace));
            attributeList = attributeList.withMarkers(ctx.receiveNonNullNode(attributeList.getMarkers(), ctx::receiveMarkers));
            attributeList = attributeList.getPadding().withTarget(ctx.receiveNode(attributeList.getPadding().getTarget(), CSharpReceiver::receiveRightPaddedTree));
            attributeList = attributeList.getPadding().withAttributes(ctx.receiveNonNullNodes(attributeList.getPadding().getAttributes(), CSharpReceiver::receiveRightPaddedTree));
            return attributeList;
        }

        @Override
        public Cs.AwaitExpression visitAwaitExpression(Cs.AwaitExpression awaitExpression, ReceiverContext ctx) {
            awaitExpression = awaitExpression.withId(ctx.receiveNonNullValue(awaitExpression.getId(), UUID.class));
            awaitExpression = awaitExpression.withPrefix(ctx.receiveNonNullNode(awaitExpression.getPrefix(), CSharpReceiver::receiveSpace));
            awaitExpression = awaitExpression.withMarkers(ctx.receiveNonNullNode(awaitExpression.getMarkers(), ctx::receiveMarkers));
            awaitExpression = awaitExpression.withExpression(ctx.receiveNonNullNode(awaitExpression.getExpression(), ctx::receiveTree));
            awaitExpression = awaitExpression.withType(ctx.receiveValue(awaitExpression.getType(), JavaType.class));
            return awaitExpression;
        }

        @Override
        public Cs.Binary visitBinary(Cs.Binary binary, ReceiverContext ctx) {
            binary = binary.withId(ctx.receiveNonNullValue(binary.getId(), UUID.class));
            binary = binary.withPrefix(ctx.receiveNonNullNode(binary.getPrefix(), CSharpReceiver::receiveSpace));
            binary = binary.withMarkers(ctx.receiveNonNullNode(binary.getMarkers(), ctx::receiveMarkers));
            binary = binary.withLeft(ctx.receiveNonNullNode(binary.getLeft(), ctx::receiveTree));
            binary = binary.getPadding().withOperator(ctx.receiveNonNullNode(binary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Binary.OperatorType.class)));
            binary = binary.withRight(ctx.receiveNonNullNode(binary.getRight(), ctx::receiveTree));
            binary = binary.withType(ctx.receiveValue(binary.getType(), JavaType.class));
            return binary;
        }

        @Override
        public Cs.BlockScopeNamespaceDeclaration visitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, ReceiverContext ctx) {
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withId(ctx.receiveNonNullValue(blockScopeNamespaceDeclaration.getId(), UUID.class));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withPrefix(ctx.receiveNonNullNode(blockScopeNamespaceDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withMarkers(ctx.receiveNonNullNode(blockScopeNamespaceDeclaration.getMarkers(), ctx::receiveMarkers));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withName(ctx.receiveNonNullNode(blockScopeNamespaceDeclaration.getPadding().getName(), CSharpReceiver::receiveRightPaddedTree));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withExterns(ctx.receiveNonNullNodes(blockScopeNamespaceDeclaration.getPadding().getExterns(), CSharpReceiver::receiveRightPaddedTree));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withUsings(ctx.receiveNonNullNodes(blockScopeNamespaceDeclaration.getPadding().getUsings(), CSharpReceiver::receiveRightPaddedTree));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.getPadding().withMembers(ctx.receiveNonNullNodes(blockScopeNamespaceDeclaration.getPadding().getMembers(), CSharpReceiver::receiveRightPaddedTree));
            blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.withEnd(ctx.receiveNonNullNode(blockScopeNamespaceDeclaration.getEnd(), CSharpReceiver::receiveSpace));
            return blockScopeNamespaceDeclaration;
        }

        @Override
        public Cs.CollectionExpression visitCollectionExpression(Cs.CollectionExpression collectionExpression, ReceiverContext ctx) {
            collectionExpression = collectionExpression.withId(ctx.receiveNonNullValue(collectionExpression.getId(), UUID.class));
            collectionExpression = collectionExpression.withPrefix(ctx.receiveNonNullNode(collectionExpression.getPrefix(), CSharpReceiver::receiveSpace));
            collectionExpression = collectionExpression.withMarkers(ctx.receiveNonNullNode(collectionExpression.getMarkers(), ctx::receiveMarkers));
            collectionExpression = collectionExpression.getPadding().withElements(ctx.receiveNonNullNodes(collectionExpression.getPadding().getElements(), CSharpReceiver::receiveRightPaddedTree));
            collectionExpression = collectionExpression.withType(ctx.receiveValue(collectionExpression.getType(), JavaType.class));
            return collectionExpression;
        }

        @Override
        public Cs.ExpressionStatement visitExpressionStatement(Cs.ExpressionStatement expressionStatement, ReceiverContext ctx) {
            expressionStatement = expressionStatement.withId(ctx.receiveNonNullValue(expressionStatement.getId(), UUID.class));
            expressionStatement = expressionStatement.withPrefix(ctx.receiveNonNullNode(expressionStatement.getPrefix(), CSharpReceiver::receiveSpace));
            expressionStatement = expressionStatement.withMarkers(ctx.receiveNonNullNode(expressionStatement.getMarkers(), ctx::receiveMarkers));
            expressionStatement = expressionStatement.withExpression(ctx.receiveNonNullNode(expressionStatement.getExpression(), ctx::receiveTree));
            return expressionStatement;
        }

        @Override
        public Cs.ExternAlias visitExternAlias(Cs.ExternAlias externAlias, ReceiverContext ctx) {
            externAlias = externAlias.withId(ctx.receiveNonNullValue(externAlias.getId(), UUID.class));
            externAlias = externAlias.withPrefix(ctx.receiveNonNullNode(externAlias.getPrefix(), CSharpReceiver::receiveSpace));
            externAlias = externAlias.withMarkers(ctx.receiveNonNullNode(externAlias.getMarkers(), ctx::receiveMarkers));
            externAlias = externAlias.getPadding().withIdentifier(ctx.receiveNonNullNode(externAlias.getPadding().getIdentifier(), CSharpReceiver::receiveLeftPaddedTree));
            return externAlias;
        }

        @Override
        public Cs.FileScopeNamespaceDeclaration visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, ReceiverContext ctx) {
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withId(ctx.receiveNonNullValue(fileScopeNamespaceDeclaration.getId(), UUID.class));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withPrefix(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withMarkers(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getMarkers(), ctx::receiveMarkers));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withName(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getPadding().getName(), CSharpReceiver::receiveRightPaddedTree));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withExterns(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getExterns(), CSharpReceiver::receiveRightPaddedTree));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withUsings(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getUsings(), CSharpReceiver::receiveRightPaddedTree));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withMembers(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getMembers(), CSharpReceiver::receiveRightPaddedTree));
            return fileScopeNamespaceDeclaration;
        }

        @Override
        public Cs.InterpolatedString visitInterpolatedString(Cs.InterpolatedString interpolatedString, ReceiverContext ctx) {
            interpolatedString = interpolatedString.withId(ctx.receiveNonNullValue(interpolatedString.getId(), UUID.class));
            interpolatedString = interpolatedString.withPrefix(ctx.receiveNonNullNode(interpolatedString.getPrefix(), CSharpReceiver::receiveSpace));
            interpolatedString = interpolatedString.withMarkers(ctx.receiveNonNullNode(interpolatedString.getMarkers(), ctx::receiveMarkers));
            interpolatedString = interpolatedString.withStart(ctx.receiveNonNullValue(interpolatedString.getStart(), String.class));
            interpolatedString = interpolatedString.getPadding().withParts(ctx.receiveNonNullNodes(interpolatedString.getPadding().getParts(), CSharpReceiver::receiveRightPaddedTree));
            interpolatedString = interpolatedString.withEnd(ctx.receiveNonNullValue(interpolatedString.getEnd(), String.class));
            return interpolatedString;
        }

        @Override
        public Cs.Interpolation visitInterpolation(Cs.Interpolation interpolation, ReceiverContext ctx) {
            interpolation = interpolation.withId(ctx.receiveNonNullValue(interpolation.getId(), UUID.class));
            interpolation = interpolation.withPrefix(ctx.receiveNonNullNode(interpolation.getPrefix(), CSharpReceiver::receiveSpace));
            interpolation = interpolation.withMarkers(ctx.receiveNonNullNode(interpolation.getMarkers(), ctx::receiveMarkers));
            interpolation = interpolation.getPadding().withExpression(ctx.receiveNonNullNode(interpolation.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            interpolation = interpolation.getPadding().withAlignment(ctx.receiveNode(interpolation.getPadding().getAlignment(), CSharpReceiver::receiveRightPaddedTree));
            interpolation = interpolation.getPadding().withFormat(ctx.receiveNode(interpolation.getPadding().getFormat(), CSharpReceiver::receiveRightPaddedTree));
            return interpolation;
        }

        @Override
        public Cs.NullSafeExpression visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, ReceiverContext ctx) {
            nullSafeExpression = nullSafeExpression.withId(ctx.receiveNonNullValue(nullSafeExpression.getId(), UUID.class));
            nullSafeExpression = nullSafeExpression.withPrefix(ctx.receiveNonNullNode(nullSafeExpression.getPrefix(), CSharpReceiver::receiveSpace));
            nullSafeExpression = nullSafeExpression.withMarkers(ctx.receiveNonNullNode(nullSafeExpression.getMarkers(), ctx::receiveMarkers));
            nullSafeExpression = nullSafeExpression.getPadding().withExpression(ctx.receiveNonNullNode(nullSafeExpression.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            return nullSafeExpression;
        }

        @Override
        public Cs.StatementExpression visitStatementExpression(Cs.StatementExpression statementExpression, ReceiverContext ctx) {
            statementExpression = statementExpression.withId(ctx.receiveNonNullValue(statementExpression.getId(), UUID.class));
            statementExpression = statementExpression.withPrefix(ctx.receiveNonNullNode(statementExpression.getPrefix(), CSharpReceiver::receiveSpace));
            statementExpression = statementExpression.withMarkers(ctx.receiveNonNullNode(statementExpression.getMarkers(), ctx::receiveMarkers));
            statementExpression = statementExpression.withStatement(ctx.receiveNonNullNode(statementExpression.getStatement(), ctx::receiveTree));
            return statementExpression;
        }

        @Override
        public Cs.UsingDirective visitUsingDirective(Cs.UsingDirective usingDirective, ReceiverContext ctx) {
            usingDirective = usingDirective.withId(ctx.receiveNonNullValue(usingDirective.getId(), UUID.class));
            usingDirective = usingDirective.withPrefix(ctx.receiveNonNullNode(usingDirective.getPrefix(), CSharpReceiver::receiveSpace));
            usingDirective = usingDirective.withMarkers(ctx.receiveNonNullNode(usingDirective.getMarkers(), ctx::receiveMarkers));
            usingDirective = usingDirective.getPadding().withGlobal(ctx.receiveNonNullNode(usingDirective.getPadding().getGlobal(), rightPaddedValueReceiver(java.lang.Boolean.class)));
            usingDirective = usingDirective.getPadding().withStatic(ctx.receiveNonNullNode(usingDirective.getPadding().getStatic(), leftPaddedValueReceiver(java.lang.Boolean.class)));
            usingDirective = usingDirective.getPadding().withUnsafe(ctx.receiveNonNullNode(usingDirective.getPadding().getUnsafe(), leftPaddedValueReceiver(java.lang.Boolean.class)));
            usingDirective = usingDirective.getPadding().withAlias(ctx.receiveNode(usingDirective.getPadding().getAlias(), CSharpReceiver::receiveRightPaddedTree));
            usingDirective = usingDirective.withNamespaceOrType(ctx.receiveNonNullNode(usingDirective.getNamespaceOrType(), ctx::receiveTree));
            return usingDirective;
        }

        @Override
        public Cs.PropertyDeclaration visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, ReceiverContext ctx) {
            propertyDeclaration = propertyDeclaration.withId(ctx.receiveNonNullValue(propertyDeclaration.getId(), UUID.class));
            propertyDeclaration = propertyDeclaration.withPrefix(ctx.receiveNonNullNode(propertyDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            propertyDeclaration = propertyDeclaration.withMarkers(ctx.receiveNonNullNode(propertyDeclaration.getMarkers(), ctx::receiveMarkers));
            propertyDeclaration = propertyDeclaration.withAttributeLists(ctx.receiveNonNullNodes(propertyDeclaration.getAttributeLists(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withModifiers(ctx.receiveNonNullNodes(propertyDeclaration.getModifiers(), CSharpReceiver::receiveModifier));
            propertyDeclaration = propertyDeclaration.withTypeExpression(ctx.receiveNonNullNode(propertyDeclaration.getTypeExpression(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.getPadding().withInterfaceSpecifier(ctx.receiveNode(propertyDeclaration.getPadding().getInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            propertyDeclaration = propertyDeclaration.withName(ctx.receiveNonNullNode(propertyDeclaration.getName(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withAccessors(ctx.receiveNonNullNode(propertyDeclaration.getAccessors(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.getPadding().withInitializer(ctx.receiveNode(propertyDeclaration.getPadding().getInitializer(), CSharpReceiver::receiveLeftPaddedTree));
            return propertyDeclaration;
        }

        @Override
        public Cs.Keyword visitKeyword(Cs.Keyword keyword, ReceiverContext ctx) {
            keyword = keyword.withId(ctx.receiveNonNullValue(keyword.getId(), UUID.class));
            keyword = keyword.withPrefix(ctx.receiveNonNullNode(keyword.getPrefix(), CSharpReceiver::receiveSpace));
            keyword = keyword.withMarkers(ctx.receiveNonNullNode(keyword.getMarkers(), ctx::receiveMarkers));
            keyword = keyword.withKind(ctx.receiveNonNullValue(keyword.getKind(), Cs.Keyword.KeywordKind.class));
            return keyword;
        }

        @Override
        public Cs.Lambda visitLambda(Cs.Lambda lambda, ReceiverContext ctx) {
            lambda = lambda.withId(ctx.receiveNonNullValue(lambda.getId(), UUID.class));
            lambda = lambda.withPrefix(ctx.receiveNonNullNode(lambda.getPrefix(), CSharpReceiver::receiveSpace));
            lambda = lambda.withMarkers(ctx.receiveNonNullNode(lambda.getMarkers(), ctx::receiveMarkers));
            lambda = lambda.withLambdaExpression(ctx.receiveNonNullNode(lambda.getLambdaExpression(), ctx::receiveTree));
            lambda = lambda.withModifiers(ctx.receiveNonNullNodes(lambda.getModifiers(), CSharpReceiver::receiveModifier));
            return lambda;
        }

        @Override
        public Cs.ClassDeclaration visitClassDeclaration(Cs.ClassDeclaration classDeclaration, ReceiverContext ctx) {
            classDeclaration = classDeclaration.withId(ctx.receiveNonNullValue(classDeclaration.getId(), UUID.class));
            classDeclaration = classDeclaration.withPrefix(ctx.receiveNonNullNode(classDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            classDeclaration = classDeclaration.withMarkers(ctx.receiveNonNullNode(classDeclaration.getMarkers(), ctx::receiveMarkers));
            classDeclaration = classDeclaration.withClassDeclarationCore(ctx.receiveNonNullNode(classDeclaration.getClassDeclarationCore(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withTypeParameterConstraintClauses(ctx.receiveNonNullNode(classDeclaration.getPadding().getTypeParameterConstraintClauses(), CSharpReceiver::receiveContainer));
            return classDeclaration;
        }

        @Override
        public Cs.MethodDeclaration visitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, ReceiverContext ctx) {
            methodDeclaration = methodDeclaration.withId(ctx.receiveNonNullValue(methodDeclaration.getId(), UUID.class));
            methodDeclaration = methodDeclaration.withPrefix(ctx.receiveNonNullNode(methodDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            methodDeclaration = methodDeclaration.withMarkers(ctx.receiveNonNullNode(methodDeclaration.getMarkers(), ctx::receiveMarkers));
            methodDeclaration = methodDeclaration.withMethodDeclarationCore(ctx.receiveNonNullNode(methodDeclaration.getMethodDeclarationCore(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withTypeParameterConstraintClauses(ctx.receiveNonNullNode(methodDeclaration.getPadding().getTypeParameterConstraintClauses(), CSharpReceiver::receiveContainer));
            return methodDeclaration;
        }

        @Override
        public Cs.UsingStatement visitUsingStatement(Cs.UsingStatement usingStatement, ReceiverContext ctx) {
            usingStatement = usingStatement.withId(ctx.receiveNonNullValue(usingStatement.getId(), UUID.class));
            usingStatement = usingStatement.withPrefix(ctx.receiveNonNullNode(usingStatement.getPrefix(), CSharpReceiver::receiveSpace));
            usingStatement = usingStatement.withMarkers(ctx.receiveNonNullNode(usingStatement.getMarkers(), ctx::receiveMarkers));
            usingStatement = usingStatement.withAwaitKeyword(ctx.receiveNode(usingStatement.getAwaitKeyword(), ctx::receiveTree));
            usingStatement = usingStatement.getPadding().withExpression(ctx.receiveNonNullNode(usingStatement.getPadding().getExpression(), CSharpReceiver::receiveContainer));
            usingStatement = usingStatement.withStatement(ctx.receiveNonNullNode(usingStatement.getStatement(), ctx::receiveTree));
            return usingStatement;
        }

        @Override
        public Cs.TypeParameterConstraintClause visitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, ReceiverContext ctx) {
            typeParameterConstraintClause = typeParameterConstraintClause.withId(ctx.receiveNonNullValue(typeParameterConstraintClause.getId(), UUID.class));
            typeParameterConstraintClause = typeParameterConstraintClause.withPrefix(ctx.receiveNonNullNode(typeParameterConstraintClause.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameterConstraintClause = typeParameterConstraintClause.withMarkers(ctx.receiveNonNullNode(typeParameterConstraintClause.getMarkers(), ctx::receiveMarkers));
            typeParameterConstraintClause = typeParameterConstraintClause.getPadding().withTypeParameter(ctx.receiveNonNullNode(typeParameterConstraintClause.getPadding().getTypeParameter(), CSharpReceiver::receiveRightPaddedTree));
            typeParameterConstraintClause = typeParameterConstraintClause.getPadding().withTypeParameterConstraints(ctx.receiveNonNullNode(typeParameterConstraintClause.getPadding().getTypeParameterConstraints(), CSharpReceiver::receiveContainer));
            return typeParameterConstraintClause;
        }

        @Override
        public Cs.TypeConstraint visitTypeConstraint(Cs.TypeConstraint typeConstraint, ReceiverContext ctx) {
            typeConstraint = typeConstraint.withId(ctx.receiveNonNullValue(typeConstraint.getId(), UUID.class));
            typeConstraint = typeConstraint.withPrefix(ctx.receiveNonNullNode(typeConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            typeConstraint = typeConstraint.withMarkers(ctx.receiveNonNullNode(typeConstraint.getMarkers(), ctx::receiveMarkers));
            typeConstraint = typeConstraint.withTypeExpression(ctx.receiveNonNullNode(typeConstraint.getTypeExpression(), ctx::receiveTree));
            return typeConstraint;
        }

        @Override
        public Cs.AllowsConstraintClause visitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, ReceiverContext ctx) {
            allowsConstraintClause = allowsConstraintClause.withId(ctx.receiveNonNullValue(allowsConstraintClause.getId(), UUID.class));
            allowsConstraintClause = allowsConstraintClause.withPrefix(ctx.receiveNonNullNode(allowsConstraintClause.getPrefix(), CSharpReceiver::receiveSpace));
            allowsConstraintClause = allowsConstraintClause.withMarkers(ctx.receiveNonNullNode(allowsConstraintClause.getMarkers(), ctx::receiveMarkers));
            allowsConstraintClause = allowsConstraintClause.getPadding().withExpressions(ctx.receiveNonNullNode(allowsConstraintClause.getPadding().getExpressions(), CSharpReceiver::receiveContainer));
            return allowsConstraintClause;
        }

        @Override
        public Cs.RefStructConstraint visitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, ReceiverContext ctx) {
            refStructConstraint = refStructConstraint.withId(ctx.receiveNonNullValue(refStructConstraint.getId(), UUID.class));
            refStructConstraint = refStructConstraint.withPrefix(ctx.receiveNonNullNode(refStructConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            refStructConstraint = refStructConstraint.withMarkers(ctx.receiveNonNullNode(refStructConstraint.getMarkers(), ctx::receiveMarkers));
            return refStructConstraint;
        }

        @Override
        public Cs.ClassOrStructConstraint visitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, ReceiverContext ctx) {
            classOrStructConstraint = classOrStructConstraint.withId(ctx.receiveNonNullValue(classOrStructConstraint.getId(), UUID.class));
            classOrStructConstraint = classOrStructConstraint.withPrefix(ctx.receiveNonNullNode(classOrStructConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            classOrStructConstraint = classOrStructConstraint.withMarkers(ctx.receiveNonNullNode(classOrStructConstraint.getMarkers(), ctx::receiveMarkers));
            classOrStructConstraint = classOrStructConstraint.withKind(ctx.receiveNonNullValue(classOrStructConstraint.getKind(), Cs.ClassOrStructConstraint.TypeKind.class));
            return classOrStructConstraint;
        }

        @Override
        public Cs.ConstructorConstraint visitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, ReceiverContext ctx) {
            constructorConstraint = constructorConstraint.withId(ctx.receiveNonNullValue(constructorConstraint.getId(), UUID.class));
            constructorConstraint = constructorConstraint.withPrefix(ctx.receiveNonNullNode(constructorConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            constructorConstraint = constructorConstraint.withMarkers(ctx.receiveNonNullNode(constructorConstraint.getMarkers(), ctx::receiveMarkers));
            return constructorConstraint;
        }

        @Override
        public Cs.DefaultConstraint visitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, ReceiverContext ctx) {
            defaultConstraint = defaultConstraint.withId(ctx.receiveNonNullValue(defaultConstraint.getId(), UUID.class));
            defaultConstraint = defaultConstraint.withPrefix(ctx.receiveNonNullNode(defaultConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            defaultConstraint = defaultConstraint.withMarkers(ctx.receiveNonNullNode(defaultConstraint.getMarkers(), ctx::receiveMarkers));
            return defaultConstraint;
        }

        @Override
        public Cs.DeclarationExpression visitDeclarationExpression(Cs.DeclarationExpression declarationExpression, ReceiverContext ctx) {
            declarationExpression = declarationExpression.withId(ctx.receiveNonNullValue(declarationExpression.getId(), UUID.class));
            declarationExpression = declarationExpression.withPrefix(ctx.receiveNonNullNode(declarationExpression.getPrefix(), CSharpReceiver::receiveSpace));
            declarationExpression = declarationExpression.withMarkers(ctx.receiveNonNullNode(declarationExpression.getMarkers(), ctx::receiveMarkers));
            declarationExpression = declarationExpression.withTypeExpression(ctx.receiveNode(declarationExpression.getTypeExpression(), ctx::receiveTree));
            declarationExpression = declarationExpression.withVariables(ctx.receiveNonNullNode(declarationExpression.getVariables(), ctx::receiveTree));
            return declarationExpression;
        }

        @Override
        public Cs.SingleVariableDesignation visitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, ReceiverContext ctx) {
            singleVariableDesignation = singleVariableDesignation.withId(ctx.receiveNonNullValue(singleVariableDesignation.getId(), UUID.class));
            singleVariableDesignation = singleVariableDesignation.withPrefix(ctx.receiveNonNullNode(singleVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            singleVariableDesignation = singleVariableDesignation.withMarkers(ctx.receiveNonNullNode(singleVariableDesignation.getMarkers(), ctx::receiveMarkers));
            singleVariableDesignation = singleVariableDesignation.withName(ctx.receiveNonNullNode(singleVariableDesignation.getName(), ctx::receiveTree));
            return singleVariableDesignation;
        }

        @Override
        public Cs.ParenthesizedVariableDesignation visitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, ReceiverContext ctx) {
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withId(ctx.receiveNonNullValue(parenthesizedVariableDesignation.getId(), UUID.class));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withPrefix(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withMarkers(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getMarkers(), ctx::receiveMarkers));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.getPadding().withVariables(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getPadding().getVariables(), CSharpReceiver::receiveContainer));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withType(ctx.receiveValue(parenthesizedVariableDesignation.getType(), JavaType.class));
            return parenthesizedVariableDesignation;
        }

        @Override
        public Cs.DiscardVariableDesignation visitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, ReceiverContext ctx) {
            discardVariableDesignation = discardVariableDesignation.withId(ctx.receiveNonNullValue(discardVariableDesignation.getId(), UUID.class));
            discardVariableDesignation = discardVariableDesignation.withPrefix(ctx.receiveNonNullNode(discardVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            discardVariableDesignation = discardVariableDesignation.withMarkers(ctx.receiveNonNullNode(discardVariableDesignation.getMarkers(), ctx::receiveMarkers));
            discardVariableDesignation = discardVariableDesignation.withDiscard(ctx.receiveNonNullNode(discardVariableDesignation.getDiscard(), ctx::receiveTree));
            return discardVariableDesignation;
        }

        @Override
        public Cs.TupleExpression visitTupleExpression(Cs.TupleExpression tupleExpression, ReceiverContext ctx) {
            tupleExpression = tupleExpression.withId(ctx.receiveNonNullValue(tupleExpression.getId(), UUID.class));
            tupleExpression = tupleExpression.withPrefix(ctx.receiveNonNullNode(tupleExpression.getPrefix(), CSharpReceiver::receiveSpace));
            tupleExpression = tupleExpression.withMarkers(ctx.receiveNonNullNode(tupleExpression.getMarkers(), ctx::receiveMarkers));
            tupleExpression = tupleExpression.getPadding().withArguments(ctx.receiveNonNullNode(tupleExpression.getPadding().getArguments(), CSharpReceiver::receiveContainer));
            return tupleExpression;
        }

        @Override
        public Cs.Constructor visitConstructor(Cs.Constructor constructor, ReceiverContext ctx) {
            constructor = constructor.withId(ctx.receiveNonNullValue(constructor.getId(), UUID.class));
            constructor = constructor.withPrefix(ctx.receiveNonNullNode(constructor.getPrefix(), CSharpReceiver::receiveSpace));
            constructor = constructor.withMarkers(ctx.receiveNonNullNode(constructor.getMarkers(), ctx::receiveMarkers));
            constructor = constructor.withInitializer(ctx.receiveNode(constructor.getInitializer(), ctx::receiveTree));
            constructor = constructor.withConstructorCore(ctx.receiveNonNullNode(constructor.getConstructorCore(), ctx::receiveTree));
            return constructor;
        }

        @Override
        public Cs.DestructorDeclaration visitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, ReceiverContext ctx) {
            destructorDeclaration = destructorDeclaration.withId(ctx.receiveNonNullValue(destructorDeclaration.getId(), UUID.class));
            destructorDeclaration = destructorDeclaration.withPrefix(ctx.receiveNonNullNode(destructorDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            destructorDeclaration = destructorDeclaration.withMarkers(ctx.receiveNonNullNode(destructorDeclaration.getMarkers(), ctx::receiveMarkers));
            destructorDeclaration = destructorDeclaration.withInitializer(ctx.receiveNode(destructorDeclaration.getInitializer(), ctx::receiveTree));
            destructorDeclaration = destructorDeclaration.withConstructorCore(ctx.receiveNonNullNode(destructorDeclaration.getConstructorCore(), ctx::receiveTree));
            return destructorDeclaration;
        }

        @Override
        public Cs.Unary visitUnary(Cs.Unary unary, ReceiverContext ctx) {
            unary = unary.withId(ctx.receiveNonNullValue(unary.getId(), UUID.class));
            unary = unary.withPrefix(ctx.receiveNonNullNode(unary.getPrefix(), CSharpReceiver::receiveSpace));
            unary = unary.withMarkers(ctx.receiveNonNullNode(unary.getMarkers(), ctx::receiveMarkers));
            unary = unary.getPadding().withOperator(ctx.receiveNonNullNode(unary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Unary.Type.class)));
            unary = unary.withExpression(ctx.receiveNonNullNode(unary.getExpression(), ctx::receiveTree));
            unary = unary.withType(ctx.receiveValue(unary.getType(), JavaType.class));
            return unary;
        }

        @Override
        public Cs.ConstructorInitializer visitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, ReceiverContext ctx) {
            constructorInitializer = constructorInitializer.withId(ctx.receiveNonNullValue(constructorInitializer.getId(), UUID.class));
            constructorInitializer = constructorInitializer.withPrefix(ctx.receiveNonNullNode(constructorInitializer.getPrefix(), CSharpReceiver::receiveSpace));
            constructorInitializer = constructorInitializer.withMarkers(ctx.receiveNonNullNode(constructorInitializer.getMarkers(), ctx::receiveMarkers));
            constructorInitializer = constructorInitializer.withKeyword(ctx.receiveNonNullNode(constructorInitializer.getKeyword(), ctx::receiveTree));
            constructorInitializer = constructorInitializer.getPadding().withArguments(ctx.receiveNonNullNode(constructorInitializer.getPadding().getArguments(), CSharpReceiver::receiveContainer));
            return constructorInitializer;
        }

        @Override
        public Cs.TupleType visitTupleType(Cs.TupleType tupleType, ReceiverContext ctx) {
            tupleType = tupleType.withId(ctx.receiveNonNullValue(tupleType.getId(), UUID.class));
            tupleType = tupleType.withPrefix(ctx.receiveNonNullNode(tupleType.getPrefix(), CSharpReceiver::receiveSpace));
            tupleType = tupleType.withMarkers(ctx.receiveNonNullNode(tupleType.getMarkers(), ctx::receiveMarkers));
            tupleType = tupleType.getPadding().withElements(ctx.receiveNonNullNode(tupleType.getPadding().getElements(), CSharpReceiver::receiveContainer));
            tupleType = tupleType.withType(ctx.receiveValue(tupleType.getType(), JavaType.class));
            return tupleType;
        }

        @Override
        public Cs.TupleElement visitTupleElement(Cs.TupleElement tupleElement, ReceiverContext ctx) {
            tupleElement = tupleElement.withId(ctx.receiveNonNullValue(tupleElement.getId(), UUID.class));
            tupleElement = tupleElement.withPrefix(ctx.receiveNonNullNode(tupleElement.getPrefix(), CSharpReceiver::receiveSpace));
            tupleElement = tupleElement.withMarkers(ctx.receiveNonNullNode(tupleElement.getMarkers(), ctx::receiveMarkers));
            tupleElement = tupleElement.withType(ctx.receiveNonNullNode(tupleElement.getType(), ctx::receiveTree));
            tupleElement = tupleElement.withName(ctx.receiveNode(tupleElement.getName(), ctx::receiveTree));
            return tupleElement;
        }

        @Override
        public Cs.NewClass visitNewClass(Cs.NewClass newClass, ReceiverContext ctx) {
            newClass = newClass.withId(ctx.receiveNonNullValue(newClass.getId(), UUID.class));
            newClass = newClass.withPrefix(ctx.receiveNonNullNode(newClass.getPrefix(), CSharpReceiver::receiveSpace));
            newClass = newClass.withMarkers(ctx.receiveNonNullNode(newClass.getMarkers(), ctx::receiveMarkers));
            newClass = newClass.withNewClassCore(ctx.receiveNonNullNode(newClass.getNewClassCore(), ctx::receiveTree));
            newClass = newClass.withInitializer(ctx.receiveNode(newClass.getInitializer(), ctx::receiveTree));
            return newClass;
        }

        @Override
        public Cs.InitializerExpression visitInitializerExpression(Cs.InitializerExpression initializerExpression, ReceiverContext ctx) {
            initializerExpression = initializerExpression.withId(ctx.receiveNonNullValue(initializerExpression.getId(), UUID.class));
            initializerExpression = initializerExpression.withPrefix(ctx.receiveNonNullNode(initializerExpression.getPrefix(), CSharpReceiver::receiveSpace));
            initializerExpression = initializerExpression.withMarkers(ctx.receiveNonNullNode(initializerExpression.getMarkers(), ctx::receiveMarkers));
            initializerExpression = initializerExpression.getPadding().withExpressions(ctx.receiveNonNullNode(initializerExpression.getPadding().getExpressions(), CSharpReceiver::receiveContainer));
            return initializerExpression;
        }

        @Override
        public Cs.ImplicitElementAccess visitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, ReceiverContext ctx) {
            implicitElementAccess = implicitElementAccess.withId(ctx.receiveNonNullValue(implicitElementAccess.getId(), UUID.class));
            implicitElementAccess = implicitElementAccess.withPrefix(ctx.receiveNonNullNode(implicitElementAccess.getPrefix(), CSharpReceiver::receiveSpace));
            implicitElementAccess = implicitElementAccess.withMarkers(ctx.receiveNonNullNode(implicitElementAccess.getMarkers(), ctx::receiveMarkers));
            implicitElementAccess = implicitElementAccess.getPadding().withArgumentList(ctx.receiveNonNullNode(implicitElementAccess.getPadding().getArgumentList(), CSharpReceiver::receiveContainer));
            return implicitElementAccess;
        }

        @Override
        public Cs.Yield visitYield(Cs.Yield yield, ReceiverContext ctx) {
            yield = yield.withId(ctx.receiveNonNullValue(yield.getId(), UUID.class));
            yield = yield.withPrefix(ctx.receiveNonNullNode(yield.getPrefix(), CSharpReceiver::receiveSpace));
            yield = yield.withMarkers(ctx.receiveNonNullNode(yield.getMarkers(), ctx::receiveMarkers));
            yield = yield.withReturnOrBreakKeyword(ctx.receiveNonNullNode(yield.getReturnOrBreakKeyword(), ctx::receiveTree));
            yield = yield.withExpression(ctx.receiveNode(yield.getExpression(), ctx::receiveTree));
            return yield;
        }

        @Override
        public Cs.DefaultExpression visitDefaultExpression(Cs.DefaultExpression defaultExpression, ReceiverContext ctx) {
            defaultExpression = defaultExpression.withId(ctx.receiveNonNullValue(defaultExpression.getId(), UUID.class));
            defaultExpression = defaultExpression.withPrefix(ctx.receiveNonNullNode(defaultExpression.getPrefix(), CSharpReceiver::receiveSpace));
            defaultExpression = defaultExpression.withMarkers(ctx.receiveNonNullNode(defaultExpression.getMarkers(), ctx::receiveMarkers));
            defaultExpression = defaultExpression.getPadding().withTypeOperator(ctx.receiveNode(defaultExpression.getPadding().getTypeOperator(), CSharpReceiver::receiveContainer));
            return defaultExpression;
        }

        @Override
        public Cs.IsPattern visitIsPattern(Cs.IsPattern isPattern, ReceiverContext ctx) {
            isPattern = isPattern.withId(ctx.receiveNonNullValue(isPattern.getId(), UUID.class));
            isPattern = isPattern.withPrefix(ctx.receiveNonNullNode(isPattern.getPrefix(), CSharpReceiver::receiveSpace));
            isPattern = isPattern.withMarkers(ctx.receiveNonNullNode(isPattern.getMarkers(), ctx::receiveMarkers));
            isPattern = isPattern.withExpression(ctx.receiveNonNullNode(isPattern.getExpression(), ctx::receiveTree));
            isPattern = isPattern.getPadding().withPattern(ctx.receiveNonNullNode(isPattern.getPadding().getPattern(), CSharpReceiver::receiveLeftPaddedTree));
            return isPattern;
        }

        @Override
        public Cs.UnaryPattern visitUnaryPattern(Cs.UnaryPattern unaryPattern, ReceiverContext ctx) {
            unaryPattern = unaryPattern.withId(ctx.receiveNonNullValue(unaryPattern.getId(), UUID.class));
            unaryPattern = unaryPattern.withPrefix(ctx.receiveNonNullNode(unaryPattern.getPrefix(), CSharpReceiver::receiveSpace));
            unaryPattern = unaryPattern.withMarkers(ctx.receiveNonNullNode(unaryPattern.getMarkers(), ctx::receiveMarkers));
            unaryPattern = unaryPattern.withOperator(ctx.receiveNonNullNode(unaryPattern.getOperator(), ctx::receiveTree));
            unaryPattern = unaryPattern.withPattern(ctx.receiveNonNullNode(unaryPattern.getPattern(), ctx::receiveTree));
            return unaryPattern;
        }

        @Override
        public Cs.TypePattern visitTypePattern(Cs.TypePattern typePattern, ReceiverContext ctx) {
            typePattern = typePattern.withId(ctx.receiveNonNullValue(typePattern.getId(), UUID.class));
            typePattern = typePattern.withPrefix(ctx.receiveNonNullNode(typePattern.getPrefix(), CSharpReceiver::receiveSpace));
            typePattern = typePattern.withMarkers(ctx.receiveNonNullNode(typePattern.getMarkers(), ctx::receiveMarkers));
            typePattern = typePattern.withTypeIdentifier(ctx.receiveNonNullNode(typePattern.getTypeIdentifier(), ctx::receiveTree));
            typePattern = typePattern.withDesignation(ctx.receiveNode(typePattern.getDesignation(), ctx::receiveTree));
            return typePattern;
        }

        @Override
        public Cs.BinaryPattern visitBinaryPattern(Cs.BinaryPattern binaryPattern, ReceiverContext ctx) {
            binaryPattern = binaryPattern.withId(ctx.receiveNonNullValue(binaryPattern.getId(), UUID.class));
            binaryPattern = binaryPattern.withPrefix(ctx.receiveNonNullNode(binaryPattern.getPrefix(), CSharpReceiver::receiveSpace));
            binaryPattern = binaryPattern.withMarkers(ctx.receiveNonNullNode(binaryPattern.getMarkers(), ctx::receiveMarkers));
            binaryPattern = binaryPattern.withLeft(ctx.receiveNonNullNode(binaryPattern.getLeft(), ctx::receiveTree));
            binaryPattern = binaryPattern.getPadding().withOperator(ctx.receiveNonNullNode(binaryPattern.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.BinaryPattern.OperatorType.class)));
            binaryPattern = binaryPattern.withRight(ctx.receiveNonNullNode(binaryPattern.getRight(), ctx::receiveTree));
            return binaryPattern;
        }

        @Override
        public Cs.ConstantPattern visitConstantPattern(Cs.ConstantPattern constantPattern, ReceiverContext ctx) {
            constantPattern = constantPattern.withId(ctx.receiveNonNullValue(constantPattern.getId(), UUID.class));
            constantPattern = constantPattern.withPrefix(ctx.receiveNonNullNode(constantPattern.getPrefix(), CSharpReceiver::receiveSpace));
            constantPattern = constantPattern.withMarkers(ctx.receiveNonNullNode(constantPattern.getMarkers(), ctx::receiveMarkers));
            constantPattern = constantPattern.withValue(ctx.receiveNonNullNode(constantPattern.getValue(), ctx::receiveTree));
            return constantPattern;
        }

        @Override
        public Cs.DiscardPattern visitDiscardPattern(Cs.DiscardPattern discardPattern, ReceiverContext ctx) {
            discardPattern = discardPattern.withId(ctx.receiveNonNullValue(discardPattern.getId(), UUID.class));
            discardPattern = discardPattern.withPrefix(ctx.receiveNonNullNode(discardPattern.getPrefix(), CSharpReceiver::receiveSpace));
            discardPattern = discardPattern.withMarkers(ctx.receiveNonNullNode(discardPattern.getMarkers(), ctx::receiveMarkers));
            discardPattern = discardPattern.withType(ctx.receiveValue(discardPattern.getType(), JavaType.class));
            return discardPattern;
        }

        @Override
        public Cs.ListPattern visitListPattern(Cs.ListPattern listPattern, ReceiverContext ctx) {
            listPattern = listPattern.withId(ctx.receiveNonNullValue(listPattern.getId(), UUID.class));
            listPattern = listPattern.withPrefix(ctx.receiveNonNullNode(listPattern.getPrefix(), CSharpReceiver::receiveSpace));
            listPattern = listPattern.withMarkers(ctx.receiveNonNullNode(listPattern.getMarkers(), ctx::receiveMarkers));
            listPattern = listPattern.getPadding().withPatterns(ctx.receiveNonNullNode(listPattern.getPadding().getPatterns(), CSharpReceiver::receiveContainer));
            listPattern = listPattern.withDesignation(ctx.receiveNode(listPattern.getDesignation(), ctx::receiveTree));
            return listPattern;
        }

        @Override
        public Cs.ParenthesizedPattern visitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, ReceiverContext ctx) {
            parenthesizedPattern = parenthesizedPattern.withId(ctx.receiveNonNullValue(parenthesizedPattern.getId(), UUID.class));
            parenthesizedPattern = parenthesizedPattern.withPrefix(ctx.receiveNonNullNode(parenthesizedPattern.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedPattern = parenthesizedPattern.withMarkers(ctx.receiveNonNullNode(parenthesizedPattern.getMarkers(), ctx::receiveMarkers));
            parenthesizedPattern = parenthesizedPattern.getPadding().withPattern(ctx.receiveNonNullNode(parenthesizedPattern.getPadding().getPattern(), CSharpReceiver::receiveContainer));
            return parenthesizedPattern;
        }

        @Override
        public Cs.RecursivePattern visitRecursivePattern(Cs.RecursivePattern recursivePattern, ReceiverContext ctx) {
            recursivePattern = recursivePattern.withId(ctx.receiveNonNullValue(recursivePattern.getId(), UUID.class));
            recursivePattern = recursivePattern.withPrefix(ctx.receiveNonNullNode(recursivePattern.getPrefix(), CSharpReceiver::receiveSpace));
            recursivePattern = recursivePattern.withMarkers(ctx.receiveNonNullNode(recursivePattern.getMarkers(), ctx::receiveMarkers));
            recursivePattern = recursivePattern.withTypeQualifier(ctx.receiveNode(recursivePattern.getTypeQualifier(), ctx::receiveTree));
            recursivePattern = recursivePattern.withPositionalPattern(ctx.receiveNode(recursivePattern.getPositionalPattern(), ctx::receiveTree));
            recursivePattern = recursivePattern.withPropertyPattern(ctx.receiveNode(recursivePattern.getPropertyPattern(), ctx::receiveTree));
            recursivePattern = recursivePattern.withDesignation(ctx.receiveNode(recursivePattern.getDesignation(), ctx::receiveTree));
            return recursivePattern;
        }

        @Override
        public Cs.VarPattern visitVarPattern(Cs.VarPattern varPattern, ReceiverContext ctx) {
            varPattern = varPattern.withId(ctx.receiveNonNullValue(varPattern.getId(), UUID.class));
            varPattern = varPattern.withPrefix(ctx.receiveNonNullNode(varPattern.getPrefix(), CSharpReceiver::receiveSpace));
            varPattern = varPattern.withMarkers(ctx.receiveNonNullNode(varPattern.getMarkers(), ctx::receiveMarkers));
            varPattern = varPattern.withDesignation(ctx.receiveNonNullNode(varPattern.getDesignation(), ctx::receiveTree));
            return varPattern;
        }

        @Override
        public Cs.PositionalPatternClause visitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, ReceiverContext ctx) {
            positionalPatternClause = positionalPatternClause.withId(ctx.receiveNonNullValue(positionalPatternClause.getId(), UUID.class));
            positionalPatternClause = positionalPatternClause.withPrefix(ctx.receiveNonNullNode(positionalPatternClause.getPrefix(), CSharpReceiver::receiveSpace));
            positionalPatternClause = positionalPatternClause.withMarkers(ctx.receiveNonNullNode(positionalPatternClause.getMarkers(), ctx::receiveMarkers));
            positionalPatternClause = positionalPatternClause.getPadding().withSubpatterns(ctx.receiveNonNullNode(positionalPatternClause.getPadding().getSubpatterns(), CSharpReceiver::receiveContainer));
            return positionalPatternClause;
        }

        @Override
        public Cs.RelationalPattern visitRelationalPattern(Cs.RelationalPattern relationalPattern, ReceiverContext ctx) {
            relationalPattern = relationalPattern.withId(ctx.receiveNonNullValue(relationalPattern.getId(), UUID.class));
            relationalPattern = relationalPattern.withPrefix(ctx.receiveNonNullNode(relationalPattern.getPrefix(), CSharpReceiver::receiveSpace));
            relationalPattern = relationalPattern.withMarkers(ctx.receiveNonNullNode(relationalPattern.getMarkers(), ctx::receiveMarkers));
            relationalPattern = relationalPattern.getPadding().withOperator(ctx.receiveNonNullNode(relationalPattern.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.RelationalPattern.OperatorType.class)));
            relationalPattern = relationalPattern.withValue(ctx.receiveNonNullNode(relationalPattern.getValue(), ctx::receiveTree));
            return relationalPattern;
        }

        @Override
        public Cs.SlicePattern visitSlicePattern(Cs.SlicePattern slicePattern, ReceiverContext ctx) {
            slicePattern = slicePattern.withId(ctx.receiveNonNullValue(slicePattern.getId(), UUID.class));
            slicePattern = slicePattern.withPrefix(ctx.receiveNonNullNode(slicePattern.getPrefix(), CSharpReceiver::receiveSpace));
            slicePattern = slicePattern.withMarkers(ctx.receiveNonNullNode(slicePattern.getMarkers(), ctx::receiveMarkers));
            return slicePattern;
        }

        @Override
        public Cs.PropertyPatternClause visitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, ReceiverContext ctx) {
            propertyPatternClause = propertyPatternClause.withId(ctx.receiveNonNullValue(propertyPatternClause.getId(), UUID.class));
            propertyPatternClause = propertyPatternClause.withPrefix(ctx.receiveNonNullNode(propertyPatternClause.getPrefix(), CSharpReceiver::receiveSpace));
            propertyPatternClause = propertyPatternClause.withMarkers(ctx.receiveNonNullNode(propertyPatternClause.getMarkers(), ctx::receiveMarkers));
            propertyPatternClause = propertyPatternClause.getPadding().withSubpatterns(ctx.receiveNonNullNode(propertyPatternClause.getPadding().getSubpatterns(), CSharpReceiver::receiveContainer));
            return propertyPatternClause;
        }

        @Override
        public Cs.Subpattern visitSubpattern(Cs.Subpattern subpattern, ReceiverContext ctx) {
            subpattern = subpattern.withId(ctx.receiveNonNullValue(subpattern.getId(), UUID.class));
            subpattern = subpattern.withPrefix(ctx.receiveNonNullNode(subpattern.getPrefix(), CSharpReceiver::receiveSpace));
            subpattern = subpattern.withMarkers(ctx.receiveNonNullNode(subpattern.getMarkers(), ctx::receiveMarkers));
            subpattern = subpattern.withName(ctx.receiveNode(subpattern.getName(), ctx::receiveTree));
            subpattern = subpattern.getPadding().withPattern(ctx.receiveNonNullNode(subpattern.getPadding().getPattern(), CSharpReceiver::receiveLeftPaddedTree));
            return subpattern;
        }

        @Override
        public Cs.SwitchExpression visitSwitchExpression(Cs.SwitchExpression switchExpression, ReceiverContext ctx) {
            switchExpression = switchExpression.withId(ctx.receiveNonNullValue(switchExpression.getId(), UUID.class));
            switchExpression = switchExpression.withPrefix(ctx.receiveNonNullNode(switchExpression.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpression = switchExpression.withMarkers(ctx.receiveNonNullNode(switchExpression.getMarkers(), ctx::receiveMarkers));
            switchExpression = switchExpression.getPadding().withExpression(ctx.receiveNonNullNode(switchExpression.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            switchExpression = switchExpression.getPadding().withArms(ctx.receiveNonNullNode(switchExpression.getPadding().getArms(), CSharpReceiver::receiveContainer));
            return switchExpression;
        }

        @Override
        public Cs.SwitchExpressionArm visitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, ReceiverContext ctx) {
            switchExpressionArm = switchExpressionArm.withId(ctx.receiveNonNullValue(switchExpressionArm.getId(), UUID.class));
            switchExpressionArm = switchExpressionArm.withPrefix(ctx.receiveNonNullNode(switchExpressionArm.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpressionArm = switchExpressionArm.withMarkers(ctx.receiveNonNullNode(switchExpressionArm.getMarkers(), ctx::receiveMarkers));
            switchExpressionArm = switchExpressionArm.withPattern(ctx.receiveNonNullNode(switchExpressionArm.getPattern(), ctx::receiveTree));
            switchExpressionArm = switchExpressionArm.getPadding().withWhenExpression(ctx.receiveNode(switchExpressionArm.getPadding().getWhenExpression(), CSharpReceiver::receiveLeftPaddedTree));
            switchExpressionArm = switchExpressionArm.getPadding().withExpression(ctx.receiveNonNullNode(switchExpressionArm.getPadding().getExpression(), CSharpReceiver::receiveLeftPaddedTree));
            return switchExpressionArm;
        }

        @Override
        public Cs.SwitchSection visitSwitchSection(Cs.SwitchSection switchSection, ReceiverContext ctx) {
            switchSection = switchSection.withId(ctx.receiveNonNullValue(switchSection.getId(), UUID.class));
            switchSection = switchSection.withPrefix(ctx.receiveNonNullNode(switchSection.getPrefix(), CSharpReceiver::receiveSpace));
            switchSection = switchSection.withMarkers(ctx.receiveNonNullNode(switchSection.getMarkers(), ctx::receiveMarkers));
            switchSection = switchSection.withLabels(ctx.receiveNonNullNodes(switchSection.getLabels(), ctx::receiveTree));
            switchSection = switchSection.getPadding().withStatements(ctx.receiveNonNullNodes(switchSection.getPadding().getStatements(), CSharpReceiver::receiveRightPaddedTree));
            return switchSection;
        }

        @Override
        public Cs.DefaultSwitchLabel visitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, ReceiverContext ctx) {
            defaultSwitchLabel = defaultSwitchLabel.withId(ctx.receiveNonNullValue(defaultSwitchLabel.getId(), UUID.class));
            defaultSwitchLabel = defaultSwitchLabel.withPrefix(ctx.receiveNonNullNode(defaultSwitchLabel.getPrefix(), CSharpReceiver::receiveSpace));
            defaultSwitchLabel = defaultSwitchLabel.withMarkers(ctx.receiveNonNullNode(defaultSwitchLabel.getMarkers(), ctx::receiveMarkers));
            defaultSwitchLabel = defaultSwitchLabel.withColonToken(ctx.receiveNonNullNode(defaultSwitchLabel.getColonToken(), CSharpReceiver::receiveSpace));
            return defaultSwitchLabel;
        }

        @Override
        public Cs.CasePatternSwitchLabel visitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, ReceiverContext ctx) {
            casePatternSwitchLabel = casePatternSwitchLabel.withId(ctx.receiveNonNullValue(casePatternSwitchLabel.getId(), UUID.class));
            casePatternSwitchLabel = casePatternSwitchLabel.withPrefix(ctx.receiveNonNullNode(casePatternSwitchLabel.getPrefix(), CSharpReceiver::receiveSpace));
            casePatternSwitchLabel = casePatternSwitchLabel.withMarkers(ctx.receiveNonNullNode(casePatternSwitchLabel.getMarkers(), ctx::receiveMarkers));
            casePatternSwitchLabel = casePatternSwitchLabel.withPattern(ctx.receiveNonNullNode(casePatternSwitchLabel.getPattern(), ctx::receiveTree));
            casePatternSwitchLabel = casePatternSwitchLabel.getPadding().withWhenClause(ctx.receiveNode(casePatternSwitchLabel.getPadding().getWhenClause(), CSharpReceiver::receiveLeftPaddedTree));
            casePatternSwitchLabel = casePatternSwitchLabel.withColonToken(ctx.receiveNonNullNode(casePatternSwitchLabel.getColonToken(), CSharpReceiver::receiveSpace));
            return casePatternSwitchLabel;
        }

        @Override
        public Cs.SwitchStatement visitSwitchStatement(Cs.SwitchStatement switchStatement, ReceiverContext ctx) {
            switchStatement = switchStatement.withId(ctx.receiveNonNullValue(switchStatement.getId(), UUID.class));
            switchStatement = switchStatement.withPrefix(ctx.receiveNonNullNode(switchStatement.getPrefix(), CSharpReceiver::receiveSpace));
            switchStatement = switchStatement.withMarkers(ctx.receiveNonNullNode(switchStatement.getMarkers(), ctx::receiveMarkers));
            switchStatement = switchStatement.getPadding().withExpression(ctx.receiveNonNullNode(switchStatement.getPadding().getExpression(), CSharpReceiver::receiveContainer));
            switchStatement = switchStatement.getPadding().withSections(ctx.receiveNonNullNode(switchStatement.getPadding().getSections(), CSharpReceiver::receiveContainer));
            return switchStatement;
        }

        @Override
        public Cs.LockStatement visitLockStatement(Cs.LockStatement lockStatement, ReceiverContext ctx) {
            lockStatement = lockStatement.withId(ctx.receiveNonNullValue(lockStatement.getId(), UUID.class));
            lockStatement = lockStatement.withPrefix(ctx.receiveNonNullNode(lockStatement.getPrefix(), CSharpReceiver::receiveSpace));
            lockStatement = lockStatement.withMarkers(ctx.receiveNonNullNode(lockStatement.getMarkers(), ctx::receiveMarkers));
            lockStatement = lockStatement.withExpression(ctx.receiveNonNullNode(lockStatement.getExpression(), ctx::receiveTree));
            lockStatement = lockStatement.getPadding().withStatement(ctx.receiveNonNullNode(lockStatement.getPadding().getStatement(), CSharpReceiver::receiveRightPaddedTree));
            return lockStatement;
        }

        @Override
        public Cs.FixedStatement visitFixedStatement(Cs.FixedStatement fixedStatement, ReceiverContext ctx) {
            fixedStatement = fixedStatement.withId(ctx.receiveNonNullValue(fixedStatement.getId(), UUID.class));
            fixedStatement = fixedStatement.withPrefix(ctx.receiveNonNullNode(fixedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            fixedStatement = fixedStatement.withMarkers(ctx.receiveNonNullNode(fixedStatement.getMarkers(), ctx::receiveMarkers));
            fixedStatement = fixedStatement.withDeclarations(ctx.receiveNonNullNode(fixedStatement.getDeclarations(), ctx::receiveTree));
            fixedStatement = fixedStatement.withBlock(ctx.receiveNonNullNode(fixedStatement.getBlock(), ctx::receiveTree));
            return fixedStatement;
        }

        @Override
        public Cs.CheckedStatement visitCheckedStatement(Cs.CheckedStatement checkedStatement, ReceiverContext ctx) {
            checkedStatement = checkedStatement.withId(ctx.receiveNonNullValue(checkedStatement.getId(), UUID.class));
            checkedStatement = checkedStatement.withPrefix(ctx.receiveNonNullNode(checkedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            checkedStatement = checkedStatement.withMarkers(ctx.receiveNonNullNode(checkedStatement.getMarkers(), ctx::receiveMarkers));
            checkedStatement = checkedStatement.withBlock(ctx.receiveNonNullNode(checkedStatement.getBlock(), ctx::receiveTree));
            return checkedStatement;
        }

        @Override
        public Cs.UnsafeStatement visitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, ReceiverContext ctx) {
            unsafeStatement = unsafeStatement.withId(ctx.receiveNonNullValue(unsafeStatement.getId(), UUID.class));
            unsafeStatement = unsafeStatement.withPrefix(ctx.receiveNonNullNode(unsafeStatement.getPrefix(), CSharpReceiver::receiveSpace));
            unsafeStatement = unsafeStatement.withMarkers(ctx.receiveNonNullNode(unsafeStatement.getMarkers(), ctx::receiveMarkers));
            unsafeStatement = unsafeStatement.withBlock(ctx.receiveNonNullNode(unsafeStatement.getBlock(), ctx::receiveTree));
            return unsafeStatement;
        }

        @Override
        public Cs.RangeExpression visitRangeExpression(Cs.RangeExpression rangeExpression, ReceiverContext ctx) {
            rangeExpression = rangeExpression.withId(ctx.receiveNonNullValue(rangeExpression.getId(), UUID.class));
            rangeExpression = rangeExpression.withPrefix(ctx.receiveNonNullNode(rangeExpression.getPrefix(), CSharpReceiver::receiveSpace));
            rangeExpression = rangeExpression.withMarkers(ctx.receiveNonNullNode(rangeExpression.getMarkers(), ctx::receiveMarkers));
            rangeExpression = rangeExpression.getPadding().withStart(ctx.receiveNode(rangeExpression.getPadding().getStart(), CSharpReceiver::receiveRightPaddedTree));
            rangeExpression = rangeExpression.withEnd(ctx.receiveNode(rangeExpression.getEnd(), ctx::receiveTree));
            return rangeExpression;
        }

        @Override
        public J.AnnotatedType visitAnnotatedType(J.AnnotatedType annotatedType, ReceiverContext ctx) {
            annotatedType = annotatedType.withId(ctx.receiveNonNullValue(annotatedType.getId(), UUID.class));
            annotatedType = annotatedType.withPrefix(ctx.receiveNonNullNode(annotatedType.getPrefix(), CSharpReceiver::receiveSpace));
            annotatedType = annotatedType.withMarkers(ctx.receiveNonNullNode(annotatedType.getMarkers(), ctx::receiveMarkers));
            annotatedType = annotatedType.withAnnotations(ctx.receiveNonNullNodes(annotatedType.getAnnotations(), ctx::receiveTree));
            annotatedType = annotatedType.withTypeExpression(ctx.receiveNonNullNode(annotatedType.getTypeExpression(), ctx::receiveTree));
            return annotatedType;
        }

        @Override
        public J.Annotation visitAnnotation(J.Annotation annotation, ReceiverContext ctx) {
            annotation = annotation.withId(ctx.receiveNonNullValue(annotation.getId(), UUID.class));
            annotation = annotation.withPrefix(ctx.receiveNonNullNode(annotation.getPrefix(), CSharpReceiver::receiveSpace));
            annotation = annotation.withMarkers(ctx.receiveNonNullNode(annotation.getMarkers(), ctx::receiveMarkers));
            annotation = annotation.withAnnotationType(ctx.receiveNonNullNode(annotation.getAnnotationType(), ctx::receiveTree));
            annotation = annotation.getPadding().withArguments(ctx.receiveNode(annotation.getPadding().getArguments(), CSharpReceiver::receiveContainer));
            return annotation;
        }

        @Override
        public J.ArrayAccess visitArrayAccess(J.ArrayAccess arrayAccess, ReceiverContext ctx) {
            arrayAccess = arrayAccess.withId(ctx.receiveNonNullValue(arrayAccess.getId(), UUID.class));
            arrayAccess = arrayAccess.withPrefix(ctx.receiveNonNullNode(arrayAccess.getPrefix(), CSharpReceiver::receiveSpace));
            arrayAccess = arrayAccess.withMarkers(ctx.receiveNonNullNode(arrayAccess.getMarkers(), ctx::receiveMarkers));
            arrayAccess = arrayAccess.withIndexed(ctx.receiveNonNullNode(arrayAccess.getIndexed(), ctx::receiveTree));
            arrayAccess = arrayAccess.withDimension(ctx.receiveNonNullNode(arrayAccess.getDimension(), ctx::receiveTree));
            arrayAccess = arrayAccess.withType(ctx.receiveValue(arrayAccess.getType(), JavaType.class));
            return arrayAccess;
        }

        @Override
        public J.ArrayType visitArrayType(J.ArrayType arrayType, ReceiverContext ctx) {
            arrayType = arrayType.withId(ctx.receiveNonNullValue(arrayType.getId(), UUID.class));
            arrayType = arrayType.withPrefix(ctx.receiveNonNullNode(arrayType.getPrefix(), CSharpReceiver::receiveSpace));
            arrayType = arrayType.withMarkers(ctx.receiveNonNullNode(arrayType.getMarkers(), ctx::receiveMarkers));
            arrayType = arrayType.withElementType(ctx.receiveNonNullNode(arrayType.getElementType(), ctx::receiveTree));
            arrayType = arrayType.withAnnotations(ctx.receiveNodes(arrayType.getAnnotations(), ctx::receiveTree));
            arrayType = arrayType.withDimension(ctx.receiveNode(arrayType.getDimension(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            arrayType = arrayType.withType(ctx.receiveValue(arrayType.getType(), JavaType.class));
            return arrayType;
        }

        @Override
        public J.Assert visitAssert(J.Assert assert_, ReceiverContext ctx) {
            assert_ = assert_.withId(ctx.receiveNonNullValue(assert_.getId(), UUID.class));
            assert_ = assert_.withPrefix(ctx.receiveNonNullNode(assert_.getPrefix(), CSharpReceiver::receiveSpace));
            assert_ = assert_.withMarkers(ctx.receiveNonNullNode(assert_.getMarkers(), ctx::receiveMarkers));
            assert_ = assert_.withCondition(ctx.receiveNonNullNode(assert_.getCondition(), ctx::receiveTree));
            assert_ = assert_.withDetail(ctx.receiveNode(assert_.getDetail(), CSharpReceiver::receiveLeftPaddedTree));
            return assert_;
        }

        @Override
        public J.Assignment visitAssignment(J.Assignment assignment, ReceiverContext ctx) {
            assignment = assignment.withId(ctx.receiveNonNullValue(assignment.getId(), UUID.class));
            assignment = assignment.withPrefix(ctx.receiveNonNullNode(assignment.getPrefix(), CSharpReceiver::receiveSpace));
            assignment = assignment.withMarkers(ctx.receiveNonNullNode(assignment.getMarkers(), ctx::receiveMarkers));
            assignment = assignment.withVariable(ctx.receiveNonNullNode(assignment.getVariable(), ctx::receiveTree));
            assignment = assignment.getPadding().withAssignment(ctx.receiveNonNullNode(assignment.getPadding().getAssignment(), CSharpReceiver::receiveLeftPaddedTree));
            assignment = assignment.withType(ctx.receiveValue(assignment.getType(), JavaType.class));
            return assignment;
        }

        @Override
        public J.AssignmentOperation visitAssignmentOperation(J.AssignmentOperation assignmentOperation, ReceiverContext ctx) {
            assignmentOperation = assignmentOperation.withId(ctx.receiveNonNullValue(assignmentOperation.getId(), UUID.class));
            assignmentOperation = assignmentOperation.withPrefix(ctx.receiveNonNullNode(assignmentOperation.getPrefix(), CSharpReceiver::receiveSpace));
            assignmentOperation = assignmentOperation.withMarkers(ctx.receiveNonNullNode(assignmentOperation.getMarkers(), ctx::receiveMarkers));
            assignmentOperation = assignmentOperation.withVariable(ctx.receiveNonNullNode(assignmentOperation.getVariable(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.getPadding().withOperator(ctx.receiveNonNullNode(assignmentOperation.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.AssignmentOperation.Type.class)));
            assignmentOperation = assignmentOperation.withAssignment(ctx.receiveNonNullNode(assignmentOperation.getAssignment(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.withType(ctx.receiveValue(assignmentOperation.getType(), JavaType.class));
            return assignmentOperation;
        }

        @Override
        public J.Binary visitBinary(J.Binary binary, ReceiverContext ctx) {
            binary = binary.withId(ctx.receiveNonNullValue(binary.getId(), UUID.class));
            binary = binary.withPrefix(ctx.receiveNonNullNode(binary.getPrefix(), CSharpReceiver::receiveSpace));
            binary = binary.withMarkers(ctx.receiveNonNullNode(binary.getMarkers(), ctx::receiveMarkers));
            binary = binary.withLeft(ctx.receiveNonNullNode(binary.getLeft(), ctx::receiveTree));
            binary = binary.getPadding().withOperator(ctx.receiveNonNullNode(binary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Binary.Type.class)));
            binary = binary.withRight(ctx.receiveNonNullNode(binary.getRight(), ctx::receiveTree));
            binary = binary.withType(ctx.receiveValue(binary.getType(), JavaType.class));
            return binary;
        }

        @Override
        public J.Block visitBlock(J.Block block, ReceiverContext ctx) {
            block = block.withId(ctx.receiveNonNullValue(block.getId(), UUID.class));
            block = block.withPrefix(ctx.receiveNonNullNode(block.getPrefix(), CSharpReceiver::receiveSpace));
            block = block.withMarkers(ctx.receiveNonNullNode(block.getMarkers(), ctx::receiveMarkers));
            block = block.getPadding().withStatic(ctx.receiveNonNullNode(block.getPadding().getStatic(), rightPaddedValueReceiver(java.lang.Boolean.class)));
            block = block.getPadding().withStatements(ctx.receiveNonNullNodes(block.getPadding().getStatements(), CSharpReceiver::receiveRightPaddedTree));
            block = block.withEnd(ctx.receiveNonNullNode(block.getEnd(), CSharpReceiver::receiveSpace));
            return block;
        }

        @Override
        public J.Break visitBreak(J.Break break_, ReceiverContext ctx) {
            break_ = break_.withId(ctx.receiveNonNullValue(break_.getId(), UUID.class));
            break_ = break_.withPrefix(ctx.receiveNonNullNode(break_.getPrefix(), CSharpReceiver::receiveSpace));
            break_ = break_.withMarkers(ctx.receiveNonNullNode(break_.getMarkers(), ctx::receiveMarkers));
            break_ = break_.withLabel(ctx.receiveNode(break_.getLabel(), ctx::receiveTree));
            return break_;
        }

        @Override
        public J.Case visitCase(J.Case case_, ReceiverContext ctx) {
            case_ = case_.withId(ctx.receiveNonNullValue(case_.getId(), UUID.class));
            case_ = case_.withPrefix(ctx.receiveNonNullNode(case_.getPrefix(), CSharpReceiver::receiveSpace));
            case_ = case_.withMarkers(ctx.receiveNonNullNode(case_.getMarkers(), ctx::receiveMarkers));
            case_ = case_.withType(ctx.receiveNonNullValue(case_.getType(), J.Case.Type.class));
            case_ = case_.getPadding().withExpressions(ctx.receiveNonNullNode(case_.getPadding().getExpressions(), CSharpReceiver::receiveContainer));
            case_ = case_.getPadding().withStatements(ctx.receiveNonNullNode(case_.getPadding().getStatements(), CSharpReceiver::receiveContainer));
            case_ = case_.getPadding().withBody(ctx.receiveNode(case_.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return case_;
        }

        @Override
        public J.ClassDeclaration visitClassDeclaration(J.ClassDeclaration classDeclaration, ReceiverContext ctx) {
            classDeclaration = classDeclaration.withId(ctx.receiveNonNullValue(classDeclaration.getId(), UUID.class));
            classDeclaration = classDeclaration.withPrefix(ctx.receiveNonNullNode(classDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            classDeclaration = classDeclaration.withMarkers(ctx.receiveNonNullNode(classDeclaration.getMarkers(), ctx::receiveMarkers));
            classDeclaration = classDeclaration.withLeadingAnnotations(ctx.receiveNonNullNodes(classDeclaration.getLeadingAnnotations(), ctx::receiveTree));
            classDeclaration = classDeclaration.withModifiers(ctx.receiveNonNullNodes(classDeclaration.getModifiers(), CSharpReceiver::receiveModifier));
            classDeclaration = classDeclaration.getPadding().withKind(ctx.receiveNonNullNode(classDeclaration.getPadding().getKind(), CSharpReceiver::receiveClassDeclarationKind));
            classDeclaration = classDeclaration.withName(ctx.receiveNonNullNode(classDeclaration.getName(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withTypeParameters(ctx.receiveNode(classDeclaration.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withPrimaryConstructor(ctx.receiveNode(classDeclaration.getPadding().getPrimaryConstructor(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withExtends(ctx.receiveNode(classDeclaration.getPadding().getExtends(), CSharpReceiver::receiveLeftPaddedTree));
            classDeclaration = classDeclaration.getPadding().withImplements(ctx.receiveNode(classDeclaration.getPadding().getImplements(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withPermits(ctx.receiveNode(classDeclaration.getPadding().getPermits(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.withBody(ctx.receiveNonNullNode(classDeclaration.getBody(), ctx::receiveTree));
            classDeclaration = classDeclaration.withType(ctx.receiveValue(classDeclaration.getType(), JavaType.FullyQualified.class));
            return classDeclaration;
        }

        @Override
        public J.Continue visitContinue(J.Continue continue_, ReceiverContext ctx) {
            continue_ = continue_.withId(ctx.receiveNonNullValue(continue_.getId(), UUID.class));
            continue_ = continue_.withPrefix(ctx.receiveNonNullNode(continue_.getPrefix(), CSharpReceiver::receiveSpace));
            continue_ = continue_.withMarkers(ctx.receiveNonNullNode(continue_.getMarkers(), ctx::receiveMarkers));
            continue_ = continue_.withLabel(ctx.receiveNode(continue_.getLabel(), ctx::receiveTree));
            return continue_;
        }

        @Override
        public J.DoWhileLoop visitDoWhileLoop(J.DoWhileLoop doWhileLoop, ReceiverContext ctx) {
            doWhileLoop = doWhileLoop.withId(ctx.receiveNonNullValue(doWhileLoop.getId(), UUID.class));
            doWhileLoop = doWhileLoop.withPrefix(ctx.receiveNonNullNode(doWhileLoop.getPrefix(), CSharpReceiver::receiveSpace));
            doWhileLoop = doWhileLoop.withMarkers(ctx.receiveNonNullNode(doWhileLoop.getMarkers(), ctx::receiveMarkers));
            doWhileLoop = doWhileLoop.getPadding().withBody(ctx.receiveNonNullNode(doWhileLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            doWhileLoop = doWhileLoop.getPadding().withWhileCondition(ctx.receiveNonNullNode(doWhileLoop.getPadding().getWhileCondition(), CSharpReceiver::receiveLeftPaddedTree));
            return doWhileLoop;
        }

        @Override
        public J.Empty visitEmpty(J.Empty empty, ReceiverContext ctx) {
            empty = empty.withId(ctx.receiveNonNullValue(empty.getId(), UUID.class));
            empty = empty.withPrefix(ctx.receiveNonNullNode(empty.getPrefix(), CSharpReceiver::receiveSpace));
            empty = empty.withMarkers(ctx.receiveNonNullNode(empty.getMarkers(), ctx::receiveMarkers));
            return empty;
        }

        @Override
        public J.EnumValue visitEnumValue(J.EnumValue enumValue, ReceiverContext ctx) {
            enumValue = enumValue.withId(ctx.receiveNonNullValue(enumValue.getId(), UUID.class));
            enumValue = enumValue.withPrefix(ctx.receiveNonNullNode(enumValue.getPrefix(), CSharpReceiver::receiveSpace));
            enumValue = enumValue.withMarkers(ctx.receiveNonNullNode(enumValue.getMarkers(), ctx::receiveMarkers));
            enumValue = enumValue.withAnnotations(ctx.receiveNonNullNodes(enumValue.getAnnotations(), ctx::receiveTree));
            enumValue = enumValue.withName(ctx.receiveNonNullNode(enumValue.getName(), ctx::receiveTree));
            enumValue = enumValue.withInitializer(ctx.receiveNode(enumValue.getInitializer(), ctx::receiveTree));
            return enumValue;
        }

        @Override
        public J.EnumValueSet visitEnumValueSet(J.EnumValueSet enumValueSet, ReceiverContext ctx) {
            enumValueSet = enumValueSet.withId(ctx.receiveNonNullValue(enumValueSet.getId(), UUID.class));
            enumValueSet = enumValueSet.withPrefix(ctx.receiveNonNullNode(enumValueSet.getPrefix(), CSharpReceiver::receiveSpace));
            enumValueSet = enumValueSet.withMarkers(ctx.receiveNonNullNode(enumValueSet.getMarkers(), ctx::receiveMarkers));
            enumValueSet = enumValueSet.getPadding().withEnums(ctx.receiveNonNullNodes(enumValueSet.getPadding().getEnums(), CSharpReceiver::receiveRightPaddedTree));
            enumValueSet = enumValueSet.withTerminatedWithSemicolon(ctx.receiveNonNullValue(enumValueSet.isTerminatedWithSemicolon(), boolean.class));
            return enumValueSet;
        }

        @Override
        public J.FieldAccess visitFieldAccess(J.FieldAccess fieldAccess, ReceiverContext ctx) {
            fieldAccess = fieldAccess.withId(ctx.receiveNonNullValue(fieldAccess.getId(), UUID.class));
            fieldAccess = fieldAccess.withPrefix(ctx.receiveNonNullNode(fieldAccess.getPrefix(), CSharpReceiver::receiveSpace));
            fieldAccess = fieldAccess.withMarkers(ctx.receiveNonNullNode(fieldAccess.getMarkers(), ctx::receiveMarkers));
            fieldAccess = fieldAccess.withTarget(ctx.receiveNonNullNode(fieldAccess.getTarget(), ctx::receiveTree));
            fieldAccess = fieldAccess.getPadding().withName(ctx.receiveNonNullNode(fieldAccess.getPadding().getName(), CSharpReceiver::receiveLeftPaddedTree));
            fieldAccess = fieldAccess.withType(ctx.receiveValue(fieldAccess.getType(), JavaType.class));
            return fieldAccess;
        }

        @Override
        public J.ForEachLoop visitForEachLoop(J.ForEachLoop forEachLoop, ReceiverContext ctx) {
            forEachLoop = forEachLoop.withId(ctx.receiveNonNullValue(forEachLoop.getId(), UUID.class));
            forEachLoop = forEachLoop.withPrefix(ctx.receiveNonNullNode(forEachLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forEachLoop = forEachLoop.withMarkers(ctx.receiveNonNullNode(forEachLoop.getMarkers(), ctx::receiveMarkers));
            forEachLoop = forEachLoop.withControl(ctx.receiveNonNullNode(forEachLoop.getControl(), ctx::receiveTree));
            forEachLoop = forEachLoop.getPadding().withBody(ctx.receiveNonNullNode(forEachLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return forEachLoop;
        }

        @Override
        public J.ForEachLoop.Control visitForEachControl(J.ForEachLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withVariable(ctx.receiveNonNullNode(control.getPadding().getVariable(), CSharpReceiver::receiveRightPaddedTree));
            control = control.getPadding().withIterable(ctx.receiveNonNullNode(control.getPadding().getIterable(), CSharpReceiver::receiveRightPaddedTree));
            return control;
        }

        @Override
        public J.ForLoop visitForLoop(J.ForLoop forLoop, ReceiverContext ctx) {
            forLoop = forLoop.withId(ctx.receiveNonNullValue(forLoop.getId(), UUID.class));
            forLoop = forLoop.withPrefix(ctx.receiveNonNullNode(forLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forLoop = forLoop.withMarkers(ctx.receiveNonNullNode(forLoop.getMarkers(), ctx::receiveMarkers));
            forLoop = forLoop.withControl(ctx.receiveNonNullNode(forLoop.getControl(), ctx::receiveTree));
            forLoop = forLoop.getPadding().withBody(ctx.receiveNonNullNode(forLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return forLoop;
        }

        @Override
        public J.ForLoop.Control visitForControl(J.ForLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withInit(ctx.receiveNonNullNodes(control.getPadding().getInit(), CSharpReceiver::receiveRightPaddedTree));
            control = control.getPadding().withCondition(ctx.receiveNonNullNode(control.getPadding().getCondition(), CSharpReceiver::receiveRightPaddedTree));
            control = control.getPadding().withUpdate(ctx.receiveNonNullNodes(control.getPadding().getUpdate(), CSharpReceiver::receiveRightPaddedTree));
            return control;
        }

        @Override
        public J.ParenthesizedTypeTree visitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, ReceiverContext ctx) {
            parenthesizedTypeTree = parenthesizedTypeTree.withId(ctx.receiveNonNullValue(parenthesizedTypeTree.getId(), UUID.class));
            parenthesizedTypeTree = parenthesizedTypeTree.withPrefix(ctx.receiveNonNullNode(parenthesizedTypeTree.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedTypeTree = parenthesizedTypeTree.withMarkers(ctx.receiveNonNullNode(parenthesizedTypeTree.getMarkers(), ctx::receiveMarkers));
            parenthesizedTypeTree = parenthesizedTypeTree.withAnnotations(ctx.receiveNonNullNodes(parenthesizedTypeTree.getAnnotations(), ctx::receiveTree));
            parenthesizedTypeTree = parenthesizedTypeTree.withParenthesizedType(ctx.receiveNonNullNode(parenthesizedTypeTree.getParenthesizedType(), ctx::receiveTree));
            return parenthesizedTypeTree;
        }

        @Override
        public J.Identifier visitIdentifier(J.Identifier identifier, ReceiverContext ctx) {
            identifier = identifier.withId(ctx.receiveNonNullValue(identifier.getId(), UUID.class));
            identifier = identifier.withPrefix(ctx.receiveNonNullNode(identifier.getPrefix(), CSharpReceiver::receiveSpace));
            identifier = identifier.withMarkers(ctx.receiveNonNullNode(identifier.getMarkers(), ctx::receiveMarkers));
            identifier = identifier.withAnnotations(ctx.receiveNonNullNodes(identifier.getAnnotations(), ctx::receiveTree));
            identifier = identifier.withSimpleName(ctx.receiveNonNullValue(identifier.getSimpleName(), String.class));
            identifier = identifier.withType(ctx.receiveValue(identifier.getType(), JavaType.class));
            identifier = identifier.withFieldType(ctx.receiveValue(identifier.getFieldType(), JavaType.Variable.class));
            return identifier;
        }

        @Override
        public J.If visitIf(J.If if_, ReceiverContext ctx) {
            if_ = if_.withId(ctx.receiveNonNullValue(if_.getId(), UUID.class));
            if_ = if_.withPrefix(ctx.receiveNonNullNode(if_.getPrefix(), CSharpReceiver::receiveSpace));
            if_ = if_.withMarkers(ctx.receiveNonNullNode(if_.getMarkers(), ctx::receiveMarkers));
            if_ = if_.withIfCondition(ctx.receiveNonNullNode(if_.getIfCondition(), ctx::receiveTree));
            if_ = if_.getPadding().withThenPart(ctx.receiveNonNullNode(if_.getPadding().getThenPart(), CSharpReceiver::receiveRightPaddedTree));
            if_ = if_.withElsePart(ctx.receiveNode(if_.getElsePart(), ctx::receiveTree));
            return if_;
        }

        @Override
        public J.If.Else visitElse(J.If.Else else_, ReceiverContext ctx) {
            else_ = else_.withId(ctx.receiveNonNullValue(else_.getId(), UUID.class));
            else_ = else_.withPrefix(ctx.receiveNonNullNode(else_.getPrefix(), CSharpReceiver::receiveSpace));
            else_ = else_.withMarkers(ctx.receiveNonNullNode(else_.getMarkers(), ctx::receiveMarkers));
            else_ = else_.getPadding().withBody(ctx.receiveNonNullNode(else_.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return else_;
        }

        @Override
        public J.Import visitImport(J.Import import_, ReceiverContext ctx) {
            import_ = import_.withId(ctx.receiveNonNullValue(import_.getId(), UUID.class));
            import_ = import_.withPrefix(ctx.receiveNonNullNode(import_.getPrefix(), CSharpReceiver::receiveSpace));
            import_ = import_.withMarkers(ctx.receiveNonNullNode(import_.getMarkers(), ctx::receiveMarkers));
            import_ = import_.getPadding().withStatic(ctx.receiveNonNullNode(import_.getPadding().getStatic(), leftPaddedValueReceiver(java.lang.Boolean.class)));
            import_ = import_.withQualid(ctx.receiveNonNullNode(import_.getQualid(), ctx::receiveTree));
            import_ = import_.getPadding().withAlias(ctx.receiveNode(import_.getPadding().getAlias(), CSharpReceiver::receiveLeftPaddedTree));
            return import_;
        }

        @Override
        public J.InstanceOf visitInstanceOf(J.InstanceOf instanceOf, ReceiverContext ctx) {
            instanceOf = instanceOf.withId(ctx.receiveNonNullValue(instanceOf.getId(), UUID.class));
            instanceOf = instanceOf.withPrefix(ctx.receiveNonNullNode(instanceOf.getPrefix(), CSharpReceiver::receiveSpace));
            instanceOf = instanceOf.withMarkers(ctx.receiveNonNullNode(instanceOf.getMarkers(), ctx::receiveMarkers));
            instanceOf = instanceOf.getPadding().withExpression(ctx.receiveNonNullNode(instanceOf.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            instanceOf = instanceOf.withClazz(ctx.receiveNonNullNode(instanceOf.getClazz(), ctx::receiveTree));
            instanceOf = instanceOf.withPattern(ctx.receiveNode(instanceOf.getPattern(), ctx::receiveTree));
            instanceOf = instanceOf.withType(ctx.receiveValue(instanceOf.getType(), JavaType.class));
            return instanceOf;
        }

        @Override
        public J.IntersectionType visitIntersectionType(J.IntersectionType intersectionType, ReceiverContext ctx) {
            intersectionType = intersectionType.withId(ctx.receiveNonNullValue(intersectionType.getId(), UUID.class));
            intersectionType = intersectionType.withPrefix(ctx.receiveNonNullNode(intersectionType.getPrefix(), CSharpReceiver::receiveSpace));
            intersectionType = intersectionType.withMarkers(ctx.receiveNonNullNode(intersectionType.getMarkers(), ctx::receiveMarkers));
            intersectionType = intersectionType.getPadding().withBounds(ctx.receiveNonNullNode(intersectionType.getPadding().getBounds(), CSharpReceiver::receiveContainer));
            return intersectionType;
        }

        @Override
        public J.Label visitLabel(J.Label label, ReceiverContext ctx) {
            label = label.withId(ctx.receiveNonNullValue(label.getId(), UUID.class));
            label = label.withPrefix(ctx.receiveNonNullNode(label.getPrefix(), CSharpReceiver::receiveSpace));
            label = label.withMarkers(ctx.receiveNonNullNode(label.getMarkers(), ctx::receiveMarkers));
            label = label.getPadding().withLabel(ctx.receiveNonNullNode(label.getPadding().getLabel(), CSharpReceiver::receiveRightPaddedTree));
            label = label.withStatement(ctx.receiveNonNullNode(label.getStatement(), ctx::receiveTree));
            return label;
        }

        @Override
        public J.Lambda visitLambda(J.Lambda lambda, ReceiverContext ctx) {
            lambda = lambda.withId(ctx.receiveNonNullValue(lambda.getId(), UUID.class));
            lambda = lambda.withPrefix(ctx.receiveNonNullNode(lambda.getPrefix(), CSharpReceiver::receiveSpace));
            lambda = lambda.withMarkers(ctx.receiveNonNullNode(lambda.getMarkers(), ctx::receiveMarkers));
            lambda = lambda.withParameters(ctx.receiveNonNullNode(lambda.getParameters(), CSharpReceiver::receiveLambdaParameters));
            lambda = lambda.withArrow(ctx.receiveNonNullNode(lambda.getArrow(), CSharpReceiver::receiveSpace));
            lambda = lambda.withBody(ctx.receiveNonNullNode(lambda.getBody(), ctx::receiveTree));
            lambda = lambda.withType(ctx.receiveValue(lambda.getType(), JavaType.class));
            return lambda;
        }

        @Override
        public J.Literal visitLiteral(J.Literal literal, ReceiverContext ctx) {
            literal = literal.withId(ctx.receiveNonNullValue(literal.getId(), UUID.class));
            literal = literal.withPrefix(ctx.receiveNonNullNode(literal.getPrefix(), CSharpReceiver::receiveSpace));
            literal = literal.withMarkers(ctx.receiveNonNullNode(literal.getMarkers(), ctx::receiveMarkers));
            literal = literal.withValue(ctx.receiveValue(literal.getValue(), Object.class));
            literal = literal.withValueSource(ctx.receiveValue(literal.getValueSource(), String.class));
            literal = literal.withUnicodeEscapes(ctx.receiveValues(literal.getUnicodeEscapes(), J.Literal.UnicodeEscape.class));
            literal = literal.withType(ctx.receiveValue(literal.getType(), JavaType.Primitive.class));
            return literal;
        }

        @Override
        public J.MemberReference visitMemberReference(J.MemberReference memberReference, ReceiverContext ctx) {
            memberReference = memberReference.withId(ctx.receiveNonNullValue(memberReference.getId(), UUID.class));
            memberReference = memberReference.withPrefix(ctx.receiveNonNullNode(memberReference.getPrefix(), CSharpReceiver::receiveSpace));
            memberReference = memberReference.withMarkers(ctx.receiveNonNullNode(memberReference.getMarkers(), ctx::receiveMarkers));
            memberReference = memberReference.getPadding().withContaining(ctx.receiveNonNullNode(memberReference.getPadding().getContaining(), CSharpReceiver::receiveRightPaddedTree));
            memberReference = memberReference.getPadding().withTypeParameters(ctx.receiveNode(memberReference.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            memberReference = memberReference.getPadding().withReference(ctx.receiveNonNullNode(memberReference.getPadding().getReference(), CSharpReceiver::receiveLeftPaddedTree));
            memberReference = memberReference.withType(ctx.receiveValue(memberReference.getType(), JavaType.class));
            memberReference = memberReference.withMethodType(ctx.receiveValue(memberReference.getMethodType(), JavaType.Method.class));
            memberReference = memberReference.withVariableType(ctx.receiveValue(memberReference.getVariableType(), JavaType.Variable.class));
            return memberReference;
        }

        @Override
        public J.MethodDeclaration visitMethodDeclaration(J.MethodDeclaration methodDeclaration, ReceiverContext ctx) {
            methodDeclaration = methodDeclaration.withId(ctx.receiveNonNullValue(methodDeclaration.getId(), UUID.class));
            methodDeclaration = methodDeclaration.withPrefix(ctx.receiveNonNullNode(methodDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            methodDeclaration = methodDeclaration.withMarkers(ctx.receiveNonNullNode(methodDeclaration.getMarkers(), ctx::receiveMarkers));
            methodDeclaration = methodDeclaration.withLeadingAnnotations(ctx.receiveNonNullNodes(methodDeclaration.getLeadingAnnotations(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.withModifiers(ctx.receiveNonNullNodes(methodDeclaration.getModifiers(), CSharpReceiver::receiveModifier));
            methodDeclaration = methodDeclaration.getAnnotations().withTypeParameters(ctx.receiveNode(methodDeclaration.getAnnotations().getTypeParameters(), CSharpReceiver::receiveMethodTypeParameters));
            methodDeclaration = methodDeclaration.withReturnTypeExpression(ctx.receiveNode(methodDeclaration.getReturnTypeExpression(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getAnnotations().withName(ctx.receiveNonNullNode(methodDeclaration.getAnnotations().getName(), CSharpReceiver::receiveMethodIdentifierWithAnnotations));
            methodDeclaration = methodDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(methodDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.getPadding().withThrows(ctx.receiveNode(methodDeclaration.getPadding().getThrows(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.withBody(ctx.receiveNode(methodDeclaration.getBody(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withDefaultValue(ctx.receiveNode(methodDeclaration.getPadding().getDefaultValue(), CSharpReceiver::receiveLeftPaddedTree));
            methodDeclaration = methodDeclaration.withMethodType(ctx.receiveValue(methodDeclaration.getMethodType(), JavaType.Method.class));
            return methodDeclaration;
        }

        @Override
        public J.MethodInvocation visitMethodInvocation(J.MethodInvocation methodInvocation, ReceiverContext ctx) {
            methodInvocation = methodInvocation.withId(ctx.receiveNonNullValue(methodInvocation.getId(), UUID.class));
            methodInvocation = methodInvocation.withPrefix(ctx.receiveNonNullNode(methodInvocation.getPrefix(), CSharpReceiver::receiveSpace));
            methodInvocation = methodInvocation.withMarkers(ctx.receiveNonNullNode(methodInvocation.getMarkers(), ctx::receiveMarkers));
            methodInvocation = methodInvocation.getPadding().withSelect(ctx.receiveNode(methodInvocation.getPadding().getSelect(), CSharpReceiver::receiveRightPaddedTree));
            methodInvocation = methodInvocation.getPadding().withTypeParameters(ctx.receiveNode(methodInvocation.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            methodInvocation = methodInvocation.withName(ctx.receiveNonNullNode(methodInvocation.getName(), ctx::receiveTree));
            methodInvocation = methodInvocation.getPadding().withArguments(ctx.receiveNonNullNode(methodInvocation.getPadding().getArguments(), CSharpReceiver::receiveContainer));
            methodInvocation = methodInvocation.withMethodType(ctx.receiveValue(methodInvocation.getMethodType(), JavaType.Method.class));
            return methodInvocation;
        }

        @Override
        public J.MultiCatch visitMultiCatch(J.MultiCatch multiCatch, ReceiverContext ctx) {
            multiCatch = multiCatch.withId(ctx.receiveNonNullValue(multiCatch.getId(), UUID.class));
            multiCatch = multiCatch.withPrefix(ctx.receiveNonNullNode(multiCatch.getPrefix(), CSharpReceiver::receiveSpace));
            multiCatch = multiCatch.withMarkers(ctx.receiveNonNullNode(multiCatch.getMarkers(), ctx::receiveMarkers));
            multiCatch = multiCatch.getPadding().withAlternatives(ctx.receiveNonNullNodes(multiCatch.getPadding().getAlternatives(), CSharpReceiver::receiveRightPaddedTree));
            return multiCatch;
        }

        @Override
        public J.NewArray visitNewArray(J.NewArray newArray, ReceiverContext ctx) {
            newArray = newArray.withId(ctx.receiveNonNullValue(newArray.getId(), UUID.class));
            newArray = newArray.withPrefix(ctx.receiveNonNullNode(newArray.getPrefix(), CSharpReceiver::receiveSpace));
            newArray = newArray.withMarkers(ctx.receiveNonNullNode(newArray.getMarkers(), ctx::receiveMarkers));
            newArray = newArray.withTypeExpression(ctx.receiveNode(newArray.getTypeExpression(), ctx::receiveTree));
            newArray = newArray.withDimensions(ctx.receiveNonNullNodes(newArray.getDimensions(), ctx::receiveTree));
            newArray = newArray.getPadding().withInitializer(ctx.receiveNode(newArray.getPadding().getInitializer(), CSharpReceiver::receiveContainer));
            newArray = newArray.withType(ctx.receiveValue(newArray.getType(), JavaType.class));
            return newArray;
        }

        @Override
        public J.ArrayDimension visitArrayDimension(J.ArrayDimension arrayDimension, ReceiverContext ctx) {
            arrayDimension = arrayDimension.withId(ctx.receiveNonNullValue(arrayDimension.getId(), UUID.class));
            arrayDimension = arrayDimension.withPrefix(ctx.receiveNonNullNode(arrayDimension.getPrefix(), CSharpReceiver::receiveSpace));
            arrayDimension = arrayDimension.withMarkers(ctx.receiveNonNullNode(arrayDimension.getMarkers(), ctx::receiveMarkers));
            arrayDimension = arrayDimension.getPadding().withIndex(ctx.receiveNonNullNode(arrayDimension.getPadding().getIndex(), CSharpReceiver::receiveRightPaddedTree));
            return arrayDimension;
        }

        @Override
        public J.NewClass visitNewClass(J.NewClass newClass, ReceiverContext ctx) {
            newClass = newClass.withId(ctx.receiveNonNullValue(newClass.getId(), UUID.class));
            newClass = newClass.withPrefix(ctx.receiveNonNullNode(newClass.getPrefix(), CSharpReceiver::receiveSpace));
            newClass = newClass.withMarkers(ctx.receiveNonNullNode(newClass.getMarkers(), ctx::receiveMarkers));
            newClass = newClass.getPadding().withEnclosing(ctx.receiveNode(newClass.getPadding().getEnclosing(), CSharpReceiver::receiveRightPaddedTree));
            newClass = newClass.withNew(ctx.receiveNonNullNode(newClass.getNew(), CSharpReceiver::receiveSpace));
            newClass = newClass.withClazz(ctx.receiveNode(newClass.getClazz(), ctx::receiveTree));
            newClass = newClass.getPadding().withArguments(ctx.receiveNonNullNode(newClass.getPadding().getArguments(), CSharpReceiver::receiveContainer));
            newClass = newClass.withBody(ctx.receiveNode(newClass.getBody(), ctx::receiveTree));
            newClass = newClass.withConstructorType(ctx.receiveValue(newClass.getConstructorType(), JavaType.Method.class));
            return newClass;
        }

        @Override
        public J.NullableType visitNullableType(J.NullableType nullableType, ReceiverContext ctx) {
            nullableType = nullableType.withId(ctx.receiveNonNullValue(nullableType.getId(), UUID.class));
            nullableType = nullableType.withPrefix(ctx.receiveNonNullNode(nullableType.getPrefix(), CSharpReceiver::receiveSpace));
            nullableType = nullableType.withMarkers(ctx.receiveNonNullNode(nullableType.getMarkers(), ctx::receiveMarkers));
            nullableType = nullableType.withAnnotations(ctx.receiveNonNullNodes(nullableType.getAnnotations(), ctx::receiveTree));
            nullableType = nullableType.getPadding().withTypeTree(ctx.receiveNonNullNode(nullableType.getPadding().getTypeTree(), CSharpReceiver::receiveRightPaddedTree));
            return nullableType;
        }

        @Override
        public J.Package visitPackage(J.Package package_, ReceiverContext ctx) {
            package_ = package_.withId(ctx.receiveNonNullValue(package_.getId(), UUID.class));
            package_ = package_.withPrefix(ctx.receiveNonNullNode(package_.getPrefix(), CSharpReceiver::receiveSpace));
            package_ = package_.withMarkers(ctx.receiveNonNullNode(package_.getMarkers(), ctx::receiveMarkers));
            package_ = package_.withExpression(ctx.receiveNonNullNode(package_.getExpression(), ctx::receiveTree));
            package_ = package_.withAnnotations(ctx.receiveNonNullNodes(package_.getAnnotations(), ctx::receiveTree));
            return package_;
        }

        @Override
        public J.ParameterizedType visitParameterizedType(J.ParameterizedType parameterizedType, ReceiverContext ctx) {
            parameterizedType = parameterizedType.withId(ctx.receiveNonNullValue(parameterizedType.getId(), UUID.class));
            parameterizedType = parameterizedType.withPrefix(ctx.receiveNonNullNode(parameterizedType.getPrefix(), CSharpReceiver::receiveSpace));
            parameterizedType = parameterizedType.withMarkers(ctx.receiveNonNullNode(parameterizedType.getMarkers(), ctx::receiveMarkers));
            parameterizedType = parameterizedType.withClazz(ctx.receiveNonNullNode(parameterizedType.getClazz(), ctx::receiveTree));
            parameterizedType = parameterizedType.getPadding().withTypeParameters(ctx.receiveNode(parameterizedType.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            parameterizedType = parameterizedType.withType(ctx.receiveValue(parameterizedType.getType(), JavaType.class));
            return parameterizedType;
        }

        @Override
        public <J2 extends J> J.Parentheses<J2> visitParentheses(J.Parentheses<J2> parentheses, ReceiverContext ctx) {
            parentheses = parentheses.withId(ctx.receiveNonNullValue(parentheses.getId(), UUID.class));
            parentheses = parentheses.withPrefix(ctx.receiveNonNullNode(parentheses.getPrefix(), CSharpReceiver::receiveSpace));
            parentheses = parentheses.withMarkers(ctx.receiveNonNullNode(parentheses.getMarkers(), ctx::receiveMarkers));
            parentheses = parentheses.getPadding().withTree(ctx.receiveNonNullNode(parentheses.getPadding().getTree(), CSharpReceiver::receiveRightPaddedTree));
            return parentheses;
        }

        @Override
        public <J2 extends J> J.ControlParentheses<J2> visitControlParentheses(J.ControlParentheses<J2> controlParentheses, ReceiverContext ctx) {
            controlParentheses = controlParentheses.withId(ctx.receiveNonNullValue(controlParentheses.getId(), UUID.class));
            controlParentheses = controlParentheses.withPrefix(ctx.receiveNonNullNode(controlParentheses.getPrefix(), CSharpReceiver::receiveSpace));
            controlParentheses = controlParentheses.withMarkers(ctx.receiveNonNullNode(controlParentheses.getMarkers(), ctx::receiveMarkers));
            controlParentheses = controlParentheses.getPadding().withTree(ctx.receiveNonNullNode(controlParentheses.getPadding().getTree(), CSharpReceiver::receiveRightPaddedTree));
            return controlParentheses;
        }

        @Override
        public J.Primitive visitPrimitive(J.Primitive primitive, ReceiverContext ctx) {
            primitive = primitive.withId(ctx.receiveNonNullValue(primitive.getId(), UUID.class));
            primitive = primitive.withPrefix(ctx.receiveNonNullNode(primitive.getPrefix(), CSharpReceiver::receiveSpace));
            primitive = primitive.withMarkers(ctx.receiveNonNullNode(primitive.getMarkers(), ctx::receiveMarkers));
            primitive = primitive.withType(ctx.receiveValue(primitive.getType(), JavaType.Primitive.class));
            return primitive;
        }

        @Override
        public J.Return visitReturn(J.Return return_, ReceiverContext ctx) {
            return_ = return_.withId(ctx.receiveNonNullValue(return_.getId(), UUID.class));
            return_ = return_.withPrefix(ctx.receiveNonNullNode(return_.getPrefix(), CSharpReceiver::receiveSpace));
            return_ = return_.withMarkers(ctx.receiveNonNullNode(return_.getMarkers(), ctx::receiveMarkers));
            return_ = return_.withExpression(ctx.receiveNode(return_.getExpression(), ctx::receiveTree));
            return return_;
        }

        @Override
        public J.Switch visitSwitch(J.Switch switch_, ReceiverContext ctx) {
            switch_ = switch_.withId(ctx.receiveNonNullValue(switch_.getId(), UUID.class));
            switch_ = switch_.withPrefix(ctx.receiveNonNullNode(switch_.getPrefix(), CSharpReceiver::receiveSpace));
            switch_ = switch_.withMarkers(ctx.receiveNonNullNode(switch_.getMarkers(), ctx::receiveMarkers));
            switch_ = switch_.withSelector(ctx.receiveNonNullNode(switch_.getSelector(), ctx::receiveTree));
            switch_ = switch_.withCases(ctx.receiveNonNullNode(switch_.getCases(), ctx::receiveTree));
            return switch_;
        }

        @Override
        public J.SwitchExpression visitSwitchExpression(J.SwitchExpression switchExpression, ReceiverContext ctx) {
            switchExpression = switchExpression.withId(ctx.receiveNonNullValue(switchExpression.getId(), UUID.class));
            switchExpression = switchExpression.withPrefix(ctx.receiveNonNullNode(switchExpression.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpression = switchExpression.withMarkers(ctx.receiveNonNullNode(switchExpression.getMarkers(), ctx::receiveMarkers));
            switchExpression = switchExpression.withSelector(ctx.receiveNonNullNode(switchExpression.getSelector(), ctx::receiveTree));
            switchExpression = switchExpression.withCases(ctx.receiveNonNullNode(switchExpression.getCases(), ctx::receiveTree));
            return switchExpression;
        }

        @Override
        public J.Synchronized visitSynchronized(J.Synchronized synchronized_, ReceiverContext ctx) {
            synchronized_ = synchronized_.withId(ctx.receiveNonNullValue(synchronized_.getId(), UUID.class));
            synchronized_ = synchronized_.withPrefix(ctx.receiveNonNullNode(synchronized_.getPrefix(), CSharpReceiver::receiveSpace));
            synchronized_ = synchronized_.withMarkers(ctx.receiveNonNullNode(synchronized_.getMarkers(), ctx::receiveMarkers));
            synchronized_ = synchronized_.withLock(ctx.receiveNonNullNode(synchronized_.getLock(), ctx::receiveTree));
            synchronized_ = synchronized_.withBody(ctx.receiveNonNullNode(synchronized_.getBody(), ctx::receiveTree));
            return synchronized_;
        }

        @Override
        public J.Ternary visitTernary(J.Ternary ternary, ReceiverContext ctx) {
            ternary = ternary.withId(ctx.receiveNonNullValue(ternary.getId(), UUID.class));
            ternary = ternary.withPrefix(ctx.receiveNonNullNode(ternary.getPrefix(), CSharpReceiver::receiveSpace));
            ternary = ternary.withMarkers(ctx.receiveNonNullNode(ternary.getMarkers(), ctx::receiveMarkers));
            ternary = ternary.withCondition(ctx.receiveNonNullNode(ternary.getCondition(), ctx::receiveTree));
            ternary = ternary.getPadding().withTruePart(ctx.receiveNonNullNode(ternary.getPadding().getTruePart(), CSharpReceiver::receiveLeftPaddedTree));
            ternary = ternary.getPadding().withFalsePart(ctx.receiveNonNullNode(ternary.getPadding().getFalsePart(), CSharpReceiver::receiveLeftPaddedTree));
            ternary = ternary.withType(ctx.receiveValue(ternary.getType(), JavaType.class));
            return ternary;
        }

        @Override
        public J.Throw visitThrow(J.Throw throw_, ReceiverContext ctx) {
            throw_ = throw_.withId(ctx.receiveNonNullValue(throw_.getId(), UUID.class));
            throw_ = throw_.withPrefix(ctx.receiveNonNullNode(throw_.getPrefix(), CSharpReceiver::receiveSpace));
            throw_ = throw_.withMarkers(ctx.receiveNonNullNode(throw_.getMarkers(), ctx::receiveMarkers));
            throw_ = throw_.withException(ctx.receiveNonNullNode(throw_.getException(), ctx::receiveTree));
            return throw_;
        }

        @Override
        public J.Try visitTry(J.Try try_, ReceiverContext ctx) {
            try_ = try_.withId(ctx.receiveNonNullValue(try_.getId(), UUID.class));
            try_ = try_.withPrefix(ctx.receiveNonNullNode(try_.getPrefix(), CSharpReceiver::receiveSpace));
            try_ = try_.withMarkers(ctx.receiveNonNullNode(try_.getMarkers(), ctx::receiveMarkers));
            try_ = try_.getPadding().withResources(ctx.receiveNode(try_.getPadding().getResources(), CSharpReceiver::receiveContainer));
            try_ = try_.withBody(ctx.receiveNonNullNode(try_.getBody(), ctx::receiveTree));
            try_ = try_.withCatches(ctx.receiveNonNullNodes(try_.getCatches(), ctx::receiveTree));
            try_ = try_.getPadding().withFinally(ctx.receiveNode(try_.getPadding().getFinally(), CSharpReceiver::receiveLeftPaddedTree));
            return try_;
        }

        @Override
        public J.Try.Resource visitTryResource(J.Try.Resource resource, ReceiverContext ctx) {
            resource = resource.withId(ctx.receiveNonNullValue(resource.getId(), UUID.class));
            resource = resource.withPrefix(ctx.receiveNonNullNode(resource.getPrefix(), CSharpReceiver::receiveSpace));
            resource = resource.withMarkers(ctx.receiveNonNullNode(resource.getMarkers(), ctx::receiveMarkers));
            resource = resource.withVariableDeclarations(ctx.receiveNonNullNode(resource.getVariableDeclarations(), ctx::receiveTree));
            resource = resource.withTerminatedWithSemicolon(ctx.receiveNonNullValue(resource.isTerminatedWithSemicolon(), boolean.class));
            return resource;
        }

        @Override
        public J.Try.Catch visitCatch(J.Try.Catch catch_, ReceiverContext ctx) {
            catch_ = catch_.withId(ctx.receiveNonNullValue(catch_.getId(), UUID.class));
            catch_ = catch_.withPrefix(ctx.receiveNonNullNode(catch_.getPrefix(), CSharpReceiver::receiveSpace));
            catch_ = catch_.withMarkers(ctx.receiveNonNullNode(catch_.getMarkers(), ctx::receiveMarkers));
            catch_ = catch_.withParameter(ctx.receiveNonNullNode(catch_.getParameter(), ctx::receiveTree));
            catch_ = catch_.withBody(ctx.receiveNonNullNode(catch_.getBody(), ctx::receiveTree));
            return catch_;
        }

        @Override
        public J.TypeCast visitTypeCast(J.TypeCast typeCast, ReceiverContext ctx) {
            typeCast = typeCast.withId(ctx.receiveNonNullValue(typeCast.getId(), UUID.class));
            typeCast = typeCast.withPrefix(ctx.receiveNonNullNode(typeCast.getPrefix(), CSharpReceiver::receiveSpace));
            typeCast = typeCast.withMarkers(ctx.receiveNonNullNode(typeCast.getMarkers(), ctx::receiveMarkers));
            typeCast = typeCast.withClazz(ctx.receiveNonNullNode(typeCast.getClazz(), ctx::receiveTree));
            typeCast = typeCast.withExpression(ctx.receiveNonNullNode(typeCast.getExpression(), ctx::receiveTree));
            return typeCast;
        }

        @Override
        public J.TypeParameter visitTypeParameter(J.TypeParameter typeParameter, ReceiverContext ctx) {
            typeParameter = typeParameter.withId(ctx.receiveNonNullValue(typeParameter.getId(), UUID.class));
            typeParameter = typeParameter.withPrefix(ctx.receiveNonNullNode(typeParameter.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameter = typeParameter.withMarkers(ctx.receiveNonNullNode(typeParameter.getMarkers(), ctx::receiveMarkers));
            typeParameter = typeParameter.withAnnotations(ctx.receiveNonNullNodes(typeParameter.getAnnotations(), ctx::receiveTree));
            typeParameter = typeParameter.withModifiers(ctx.receiveNonNullNodes(typeParameter.getModifiers(), CSharpReceiver::receiveModifier));
            typeParameter = typeParameter.withName(ctx.receiveNonNullNode(typeParameter.getName(), ctx::receiveTree));
            typeParameter = typeParameter.getPadding().withBounds(ctx.receiveNode(typeParameter.getPadding().getBounds(), CSharpReceiver::receiveContainer));
            return typeParameter;
        }

        @Override
        public J.Unary visitUnary(J.Unary unary, ReceiverContext ctx) {
            unary = unary.withId(ctx.receiveNonNullValue(unary.getId(), UUID.class));
            unary = unary.withPrefix(ctx.receiveNonNullNode(unary.getPrefix(), CSharpReceiver::receiveSpace));
            unary = unary.withMarkers(ctx.receiveNonNullNode(unary.getMarkers(), ctx::receiveMarkers));
            unary = unary.getPadding().withOperator(ctx.receiveNonNullNode(unary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Unary.Type.class)));
            unary = unary.withExpression(ctx.receiveNonNullNode(unary.getExpression(), ctx::receiveTree));
            unary = unary.withType(ctx.receiveValue(unary.getType(), JavaType.class));
            return unary;
        }

        @Override
        public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations variableDeclarations, ReceiverContext ctx) {
            variableDeclarations = variableDeclarations.withId(ctx.receiveNonNullValue(variableDeclarations.getId(), UUID.class));
            variableDeclarations = variableDeclarations.withPrefix(ctx.receiveNonNullNode(variableDeclarations.getPrefix(), CSharpReceiver::receiveSpace));
            variableDeclarations = variableDeclarations.withMarkers(ctx.receiveNonNullNode(variableDeclarations.getMarkers(), ctx::receiveMarkers));
            variableDeclarations = variableDeclarations.withLeadingAnnotations(ctx.receiveNonNullNodes(variableDeclarations.getLeadingAnnotations(), ctx::receiveTree));
            variableDeclarations = variableDeclarations.withModifiers(ctx.receiveNonNullNodes(variableDeclarations.getModifiers(), CSharpReceiver::receiveModifier));
            variableDeclarations = variableDeclarations.withTypeExpression(ctx.receiveNode(variableDeclarations.getTypeExpression(), ctx::receiveTree));
            variableDeclarations = variableDeclarations.withVarargs(ctx.receiveNode(variableDeclarations.getVarargs(), CSharpReceiver::receiveSpace));
            variableDeclarations = variableDeclarations.withDimensionsBeforeName(ctx.receiveNonNullNodes(variableDeclarations.getDimensionsBeforeName(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            variableDeclarations = variableDeclarations.getPadding().withVariables(ctx.receiveNonNullNodes(variableDeclarations.getPadding().getVariables(), CSharpReceiver::receiveRightPaddedTree));
            return variableDeclarations;
        }

        @Override
        public J.VariableDeclarations.NamedVariable visitVariable(J.VariableDeclarations.NamedVariable namedVariable, ReceiverContext ctx) {
            namedVariable = namedVariable.withId(ctx.receiveNonNullValue(namedVariable.getId(), UUID.class));
            namedVariable = namedVariable.withPrefix(ctx.receiveNonNullNode(namedVariable.getPrefix(), CSharpReceiver::receiveSpace));
            namedVariable = namedVariable.withMarkers(ctx.receiveNonNullNode(namedVariable.getMarkers(), ctx::receiveMarkers));
            namedVariable = namedVariable.withName(ctx.receiveNonNullNode(namedVariable.getName(), ctx::receiveTree));
            namedVariable = namedVariable.withDimensionsAfterName(ctx.receiveNonNullNodes(namedVariable.getDimensionsAfterName(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            namedVariable = namedVariable.getPadding().withInitializer(ctx.receiveNode(namedVariable.getPadding().getInitializer(), CSharpReceiver::receiveLeftPaddedTree));
            namedVariable = namedVariable.withVariableType(ctx.receiveValue(namedVariable.getVariableType(), JavaType.Variable.class));
            return namedVariable;
        }

        @Override
        public J.WhileLoop visitWhileLoop(J.WhileLoop whileLoop, ReceiverContext ctx) {
            whileLoop = whileLoop.withId(ctx.receiveNonNullValue(whileLoop.getId(), UUID.class));
            whileLoop = whileLoop.withPrefix(ctx.receiveNonNullNode(whileLoop.getPrefix(), CSharpReceiver::receiveSpace));
            whileLoop = whileLoop.withMarkers(ctx.receiveNonNullNode(whileLoop.getMarkers(), ctx::receiveMarkers));
            whileLoop = whileLoop.withCondition(ctx.receiveNonNullNode(whileLoop.getCondition(), ctx::receiveTree));
            whileLoop = whileLoop.getPadding().withBody(ctx.receiveNonNullNode(whileLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return whileLoop;
        }

        @Override
        public J.Wildcard visitWildcard(J.Wildcard wildcard, ReceiverContext ctx) {
            wildcard = wildcard.withId(ctx.receiveNonNullValue(wildcard.getId(), UUID.class));
            wildcard = wildcard.withPrefix(ctx.receiveNonNullNode(wildcard.getPrefix(), CSharpReceiver::receiveSpace));
            wildcard = wildcard.withMarkers(ctx.receiveNonNullNode(wildcard.getMarkers(), ctx::receiveMarkers));
            wildcard = wildcard.getPadding().withBound(ctx.receiveNode(wildcard.getPadding().getBound(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Wildcard.Bound.class)));
            wildcard = wildcard.withBoundedType(ctx.receiveNode(wildcard.getBoundedType(), ctx::receiveTree));
            return wildcard;
        }

        @Override
        public J.Yield visitYield(J.Yield yield, ReceiverContext ctx) {
            yield = yield.withId(ctx.receiveNonNullValue(yield.getId(), UUID.class));
            yield = yield.withPrefix(ctx.receiveNonNullNode(yield.getPrefix(), CSharpReceiver::receiveSpace));
            yield = yield.withMarkers(ctx.receiveNonNullNode(yield.getMarkers(), ctx::receiveMarkers));
            yield = yield.withImplicit(ctx.receiveNonNullValue(yield.isImplicit(), boolean.class));
            yield = yield.withValue(ctx.receiveNonNullNode(yield.getValue(), ctx::receiveTree));
            return yield;
        }

        @Override
        public J.Unknown visitUnknown(J.Unknown unknown, ReceiverContext ctx) {
            unknown = unknown.withId(ctx.receiveNonNullValue(unknown.getId(), UUID.class));
            unknown = unknown.withPrefix(ctx.receiveNonNullNode(unknown.getPrefix(), CSharpReceiver::receiveSpace));
            unknown = unknown.withMarkers(ctx.receiveNonNullNode(unknown.getMarkers(), ctx::receiveMarkers));
            unknown = unknown.withSource(ctx.receiveNonNullNode(unknown.getSource(), ctx::receiveTree));
            return unknown;
        }

        @Override
        public J.Unknown.Source visitUnknownSource(J.Unknown.Source source, ReceiverContext ctx) {
            source = source.withId(ctx.receiveNonNullValue(source.getId(), UUID.class));
            source = source.withPrefix(ctx.receiveNonNullNode(source.getPrefix(), CSharpReceiver::receiveSpace));
            source = source.withMarkers(ctx.receiveNonNullNode(source.getMarkers(), ctx::receiveMarkers));
            source = source.withText(ctx.receiveNonNullValue(source.getText(), String.class));
            return source;
        }

    }

    private static class Factory implements ReceiverFactory {

        @Override
        @SuppressWarnings("unchecked")
        public <T> T create(Class<T> type, ReceiverContext ctx) {
            if (type == Cs.CompilationUnit.class) {
                return (T) new Cs.CompilationUnit(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, Path.class),
                    ctx.receiveValue(null, FileAttributes.class),
                    ctx.receiveValue(null, String.class),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveValue(null, Checksum.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
                );
            }

            if (type == Cs.ForEachVariableLoop.class) {
                return (T) new Cs.ForEachVariableLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.ForEachVariableLoop.Control.class) {
                return (T) new Cs.ForEachVariableLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.Argument.class) {
                return (T) new Cs.Argument(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.AnnotatedStatement.class) {
                return (T) new Cs.AnnotatedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.ArrayRankSpecifier.class) {
                return (T) new Cs.ArrayRankSpecifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.AssignmentOperation.class) {
                return (T) new Cs.AssignmentOperation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AssignmentOperation.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.AttributeList.class) {
                return (T) new Cs.AttributeList(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.AwaitExpression.class) {
                return (T) new Cs.AwaitExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.Binary.class) {
                return (T) new Cs.Binary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Binary.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.BlockScopeNamespaceDeclaration.class) {
                return (T) new Cs.BlockScopeNamespaceDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
                );
            }

            if (type == Cs.CollectionExpression.class) {
                return (T) new Cs.CollectionExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.ExpressionStatement.class) {
                return (T) new Cs.ExpressionStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.ExternAlias.class) {
                return (T) new Cs.ExternAlias(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == Cs.FileScopeNamespaceDeclaration.class) {
                return (T) new Cs.FileScopeNamespaceDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.InterpolatedString.class) {
                return (T) new Cs.InterpolatedString(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, String.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullValue(null, String.class)
                );
            }

            if (type == Cs.Interpolation.class) {
                return (T) new Cs.Interpolation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.NullSafeExpression.class) {
                return (T) new Cs.NullSafeExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.StatementExpression.class) {
                return (T) new Cs.StatementExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.UsingDirective.class) {
                return (T) new Cs.UsingDirective(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, rightPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.PropertyDeclaration.class) {
                return (T) new Cs.PropertyDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == Cs.Keyword.class) {
                return (T) new Cs.Keyword(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, Cs.Keyword.KeywordKind.class)
                );
            }

            if (type == Cs.Lambda.class) {
                return (T) new Cs.Lambda(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier)
                );
            }

            if (type == Cs.ClassDeclaration.class) {
                return (T) new Cs.ClassDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.MethodDeclaration.class) {
                return (T) new Cs.MethodDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.UsingStatement.class) {
                return (T) new Cs.UsingStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.TypeParameterConstraintClause.class) {
                return (T) new Cs.TypeParameterConstraintClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.TypeConstraint.class) {
                return (T) new Cs.TypeConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.AllowsConstraintClause.class) {
                return (T) new Cs.AllowsConstraintClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.RefStructConstraint.class) {
                return (T) new Cs.RefStructConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
                );
            }

            if (type == Cs.ClassOrStructConstraint.class) {
                return (T) new Cs.ClassOrStructConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, Cs.ClassOrStructConstraint.TypeKind.class)
                );
            }

            if (type == Cs.ConstructorConstraint.class) {
                return (T) new Cs.ConstructorConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
                );
            }

            if (type == Cs.DefaultConstraint.class) {
                return (T) new Cs.DefaultConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
                );
            }

            if (type == Cs.DeclarationExpression.class) {
                return (T) new Cs.DeclarationExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.SingleVariableDesignation.class) {
                return (T) new Cs.SingleVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.ParenthesizedVariableDesignation.class) {
                return (T) new Cs.ParenthesizedVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.DiscardVariableDesignation.class) {
                return (T) new Cs.DiscardVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.TupleExpression.class) {
                return (T) new Cs.TupleExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.Constructor.class) {
                return (T) new Cs.Constructor(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.DestructorDeclaration.class) {
                return (T) new Cs.DestructorDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.Unary.class) {
                return (T) new Cs.Unary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Unary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.ConstructorInitializer.class) {
                return (T) new Cs.ConstructorInitializer(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.TupleType.class) {
                return (T) new Cs.TupleType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.TupleElement.class) {
                return (T) new Cs.TupleElement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.NewClass.class) {
                return (T) new Cs.NewClass(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.InitializerExpression.class) {
                return (T) new Cs.InitializerExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.ImplicitElementAccess.class) {
                return (T) new Cs.ImplicitElementAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.Yield.class) {
                return (T) new Cs.Yield(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.DefaultExpression.class) {
                return (T) new Cs.DefaultExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.IsPattern.class) {
                return (T) new Cs.IsPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == Cs.UnaryPattern.class) {
                return (T) new Cs.UnaryPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.TypePattern.class) {
                return (T) new Cs.TypePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.BinaryPattern.class) {
                return (T) new Cs.BinaryPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.BinaryPattern.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.ConstantPattern.class) {
                return (T) new Cs.ConstantPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.DiscardPattern.class) {
                return (T) new Cs.DiscardPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == Cs.ListPattern.class) {
                return (T) new Cs.ListPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.ParenthesizedPattern.class) {
                return (T) new Cs.ParenthesizedPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.RecursivePattern.class) {
                return (T) new Cs.RecursivePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.VarPattern.class) {
                return (T) new Cs.VarPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.PositionalPatternClause.class) {
                return (T) new Cs.PositionalPatternClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.RelationalPattern.class) {
                return (T) new Cs.RelationalPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.RelationalPattern.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.SlicePattern.class) {
                return (T) new Cs.SlicePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
                );
            }

            if (type == Cs.PropertyPatternClause.class) {
                return (T) new Cs.PropertyPatternClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.Subpattern.class) {
                return (T) new Cs.Subpattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == Cs.SwitchExpression.class) {
                return (T) new Cs.SwitchExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.SwitchExpressionArm.class) {
                return (T) new Cs.SwitchExpressionArm(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == Cs.SwitchSection.class) {
                return (T) new Cs.SwitchSection(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.DefaultSwitchLabel.class) {
                return (T) new Cs.DefaultSwitchLabel(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
                );
            }

            if (type == Cs.CasePatternSwitchLabel.class) {
                return (T) new Cs.CasePatternSwitchLabel(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
                );
            }

            if (type == Cs.SwitchStatement.class) {
                return (T) new Cs.SwitchStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == Cs.LockStatement.class) {
                return (T) new Cs.LockStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == Cs.FixedStatement.class) {
                return (T) new Cs.FixedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.CheckedStatement.class) {
                return (T) new Cs.CheckedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.UnsafeStatement.class) {
                return (T) new Cs.UnsafeStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == Cs.RangeExpression.class) {
                return (T) new Cs.RangeExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.AnnotatedType.class) {
                return (T) new J.AnnotatedType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Annotation.class) {
                return (T) new J.Annotation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == J.ArrayAccess.class) {
                return (T) new J.ArrayAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.ArrayType.class) {
                return (T) new J.ArrayType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Assert.class) {
                return (T) new J.Assert(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == J.Assignment.class) {
                return (T) new J.Assignment(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.AssignmentOperation.class) {
                return (T) new J.AssignmentOperation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.AssignmentOperation.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Binary.class) {
                return (T) new J.Binary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Binary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Block.class) {
                return (T) new J.Block(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, rightPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
                );
            }

            if (type == J.Break.class) {
                return (T) new J.Break(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Case.class) {
                return (T) new J.Case(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, J.Case.Type.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ClassDeclaration.class) {
                return (T) new J.ClassDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveClassDeclarationKind),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.FullyQualified.class)
                );
            }

            if (type == J.ClassDeclaration.Kind.class) {
                return (T) new J.ClassDeclaration.Kind(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, J.ClassDeclaration.Kind.Type.class)
                );
            }

            if (type == J.Continue.class) {
                return (T) new J.Continue(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.DoWhileLoop.class) {
                return (T) new J.DoWhileLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == J.Empty.class) {
                return (T) new J.Empty(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
                );
            }

            if (type == J.EnumValue.class) {
                return (T) new J.EnumValue(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.EnumValueSet.class) {
                return (T) new J.EnumValueSet(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullValue(null, boolean.class)
                );
            }

            if (type == J.FieldAccess.class) {
                return (T) new J.FieldAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.ForEachLoop.class) {
                return (T) new J.ForEachLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ForEachLoop.Control.class) {
                return (T) new J.ForEachLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ForLoop.class) {
                return (T) new J.ForLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ForLoop.Control.class) {
                return (T) new J.ForLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ParenthesizedTypeTree.class) {
                return (T) new J.ParenthesizedTypeTree(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Identifier.class) {
                return (T) new J.Identifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, String.class),
                    ctx.receiveValue(null, JavaType.class),
                    ctx.receiveValue(null, JavaType.Variable.class)
                );
            }

            if (type == J.If.class) {
                return (T) new J.If(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.If.Else.class) {
                return (T) new J.If.Else(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Import.class) {
                return (T) new J.Import(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == J.InstanceOf.class) {
                return (T) new J.InstanceOf(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.IntersectionType.class) {
                return (T) new J.IntersectionType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == J.Label.class) {
                return (T) new J.Label(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Lambda.class) {
                return (T) new J.Lambda(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLambdaParameters),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Lambda.Parameters.class) {
                return (T) new J.Lambda.Parameters(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Literal.class) {
                return (T) new J.Literal(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, Object.class),
                    ctx.receiveValue(null, String.class),
                    ctx.receiveValues(null, J.Literal.UnicodeEscape.class),
                    ctx.receiveValue(null, JavaType.Primitive.class)
                );
            }

            if (type == J.MemberReference.class) {
                return (T) new J.MemberReference(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class),
                    ctx.receiveValue(null, JavaType.Method.class),
                    ctx.receiveValue(null, JavaType.Variable.class)
                );
            }

            if (type == J.MethodDeclaration.class) {
                return (T) new J.MethodDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier),
                    ctx.receiveNode(null, CSharpReceiver::receiveMethodTypeParameters),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveMethodIdentifierWithAnnotations),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.Method.class)
                );
            }

            if (type == J.MethodInvocation.class) {
                return (T) new J.MethodInvocation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.Method.class)
                );
            }

            if (type == J.Modifier.class) {
                return (T) new J.Modifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, String.class),
                    ctx.receiveNonNullValue(null, J.Modifier.Type.class),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
                );
            }

            if (type == J.MultiCatch.class) {
                return (T) new J.MultiCatch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.NewArray.class) {
                return (T) new J.NewArray(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.ArrayDimension.class) {
                return (T) new J.ArrayDimension(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.NewClass.class) {
                return (T) new J.NewClass(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.Method.class)
                );
            }

            if (type == J.NullableType.class) {
                return (T) new J.NullableType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Package.class) {
                return (T) new J.Package(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
                );
            }

            if (type == J.ParameterizedType.class) {
                return (T) new J.ParameterizedType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Parentheses.class) {
                return (T) new J.Parentheses(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.ControlParentheses.class) {
                return (T) new J.ControlParentheses(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Primitive.class) {
                return (T) new J.Primitive(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, JavaType.Primitive.class)
                );
            }

            if (type == J.Return.class) {
                return (T) new J.Return(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Switch.class) {
                return (T) new J.Switch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.SwitchExpression.class) {
                return (T) new J.SwitchExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Synchronized.class) {
                return (T) new J.Synchronized(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Ternary.class) {
                return (T) new J.Ternary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.Throw.class) {
                return (T) new J.Throw(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Try.class) {
                return (T) new J.Try(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
                );
            }

            if (type == J.Try.Resource.class) {
                return (T) new J.Try.Resource(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, boolean.class)
                );
            }

            if (type == J.Try.Catch.class) {
                return (T) new J.Try.Catch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.TypeCast.class) {
                return (T) new J.TypeCast(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.TypeParameter.class) {
                return (T) new J.TypeParameter(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
                );
            }

            if (type == J.TypeParameters.class) {
                return (T) new J.TypeParameters(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Unary.class) {
                return (T) new J.Unary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Unary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
                );
            }

            if (type == J.VariableDeclarations.class) {
                return (T) new J.VariableDeclarations(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveModifier),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNodes(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.VariableDeclarations.NamedVariable.class) {
                return (T) new J.VariableDeclarations.NamedVariable(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.Variable.class)
                );
            }

            if (type == J.WhileLoop.class) {
                return (T) new J.WhileLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
                );
            }

            if (type == J.Wildcard.class) {
                return (T) new J.Wildcard(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Wildcard.Bound.class)),
                    ctx.receiveNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Yield.class) {
                return (T) new J.Yield(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Unknown.class) {
                return (T) new J.Unknown(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
                );
            }

            if (type == J.Unknown.Source.class) {
                return (T) new J.Unknown.Source(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, String.class)
                );
            }

            throw new IllegalArgumentException("Unknown type: " + type);
        }
    }

    private static J.ClassDeclaration.Kind receiveClassDeclarationKind(J.ClassDeclaration.@Nullable Kind kind, @Nullable Class<?> type, ReceiverContext ctx) {
        if (kind != null) {
            kind = kind.withId(ctx.receiveNonNullValue(kind.getId(), UUID.class));
            kind = kind.withPrefix(ctx.receiveNonNullNode(kind.getPrefix(), CSharpReceiver::receiveSpace));
            kind = kind.withMarkers(ctx.receiveNonNullNode(kind.getMarkers(), ctx::receiveMarkers));
            kind = kind.withAnnotations(ctx.receiveNonNullNodes(kind.getAnnotations(), ctx::receiveTree));
            kind = kind.withType(ctx.receiveNonNullValue(kind.getType(), J.ClassDeclaration.Kind.Type.class));
        } else {
            kind = new J.ClassDeclaration.Kind(
                ctx.receiveNonNullValue(null, UUID.class),
                ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                ctx.receiveNonNullNodes(null, ctx::receiveTree),
                ctx.receiveNonNullValue(null, J.ClassDeclaration.Kind.Type.class)
            );
        }
        return kind;
    }

    private static J.Lambda.Parameters receiveLambdaParameters(J.Lambda.@Nullable Parameters parameters, @Nullable Class<?> type, ReceiverContext ctx) {
        if (parameters != null) {
            parameters = parameters.withId(ctx.receiveNonNullValue(parameters.getId(), UUID.class));
            parameters = parameters.withPrefix(ctx.receiveNonNullNode(parameters.getPrefix(), CSharpReceiver::receiveSpace));
            parameters = parameters.withMarkers(ctx.receiveNonNullNode(parameters.getMarkers(), ctx::receiveMarkers));
            parameters = parameters.withParenthesized(ctx.receiveNonNullValue(parameters.isParenthesized(), boolean.class));
            parameters = parameters.getPadding().withParameters(ctx.receiveNonNullNodes(parameters.getPadding().getParameters(), CSharpReceiver::receiveRightPaddedTree));
        } else {
            parameters = new J.Lambda.Parameters(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }
        return parameters;
    }

    private static J.MethodDeclaration.IdentifierWithAnnotations receiveMethodIdentifierWithAnnotations(J.MethodDeclaration.@Nullable IdentifierWithAnnotations identifierWithAnnotations, @Nullable Class<?> identifierWithAnnotationsClass, ReceiverContext ctx) {
        if (identifierWithAnnotations != null) {
            identifierWithAnnotations = identifierWithAnnotations.withIdentifier(ctx.receiveNonNullNode(identifierWithAnnotations.getIdentifier(), ctx::receiveTree));
            identifierWithAnnotations = identifierWithAnnotations.withAnnotations(ctx.receiveNonNullNodes(identifierWithAnnotations.getAnnotations(), ctx::receiveTree));
        } else {
            identifierWithAnnotations = new J.MethodDeclaration.IdentifierWithAnnotations(
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
            );
        }
        return identifierWithAnnotations;
    }

    private static J.Modifier receiveModifier(J.@Nullable Modifier modifier, @Nullable Class<?> type, ReceiverContext ctx) {
        if (modifier != null) {
            modifier = modifier.withId(ctx.receiveNonNullValue(modifier.getId(), UUID.class));
            modifier = modifier.withPrefix(ctx.receiveNonNullNode(modifier.getPrefix(), CSharpReceiver::receiveSpace));
            modifier = modifier.withMarkers(ctx.receiveNonNullNode(modifier.getMarkers(), ctx::receiveMarkers));
            modifier = modifier.withKeyword(ctx.receiveValue(modifier.getKeyword(), String.class));
            modifier = modifier.withType(ctx.receiveNonNullValue(modifier.getType(), J.Modifier.Type.class));
            modifier = modifier.withAnnotations(ctx.receiveNonNullNodes(modifier.getAnnotations(), ctx::receiveTree));
        } else {
            modifier = new J.Modifier(
                ctx.receiveNonNullValue(null, UUID.class),
                ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                ctx.receiveValue(null, String.class),
                ctx.receiveNonNullValue(null, J.Modifier.Type.class),
                ctx.receiveNonNullNodes(null, ctx::receiveTree)
            );
        }
        return modifier;
    }

    private static J.TypeParameters receiveMethodTypeParameters(J.@Nullable TypeParameters typeParameters, @Nullable Class<?> type, ReceiverContext ctx) {
        if (typeParameters != null) {
            typeParameters = typeParameters.withId(ctx.receiveNonNullValue(typeParameters.getId(), UUID.class));
            typeParameters = typeParameters.withPrefix(ctx.receiveNonNullNode(typeParameters.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameters = typeParameters.withMarkers(ctx.receiveNonNullNode(typeParameters.getMarkers(), ctx::receiveMarkers));
            typeParameters = typeParameters.withAnnotations(ctx.receiveNonNullNodes(typeParameters.getAnnotations(), ctx::receiveTree));
            typeParameters = typeParameters.getPadding().withTypeParameters(ctx.receiveNonNullNodes(typeParameters.getPadding().getTypeParameters(), CSharpReceiver::receiveRightPaddedTree));
        } else {
            typeParameters = new J.TypeParameters(
                ctx.receiveNonNullValue(null, UUID.class),
                ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                ctx.receiveNonNullNodes(null, ctx::receiveTree),
                ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }
        return typeParameters;
    }

    private static <T extends J> JContainer<T> receiveContainer(@Nullable JContainer<T> container, @Nullable Class<?> type, ReceiverContext ctx) {
        return Extensions.receiveContainer(container, type, ctx);
    }

    private static <T> ReceiverContext.DetailsReceiver<JLeftPadded<T>> leftPaddedValueReceiver(Class<T> valueType) {
        return Extensions.leftPaddedValueReceiver(valueType);
    }

    private static <T> ReceiverContext.DetailsReceiver<JLeftPadded<T>> leftPaddedNodeReceiver(Class<T> nodeType) {
        return Extensions.leftPaddedNodeReceiver(nodeType);
    }

    private static <T extends J> JLeftPadded<T> receiveLeftPaddedTree(@Nullable JLeftPadded<T> leftPadded, @Nullable Class<?> type, ReceiverContext ctx) {
        return Extensions.receiveLeftPaddedTree(leftPadded, type, ctx);
    }

    private static <T> ReceiverContext.DetailsReceiver<JRightPadded<T>> rightPaddedValueReceiver(Class<T> valueType) {
        return Extensions.rightPaddedValueReceiver(valueType);
    }

    private static <T> ReceiverContext.DetailsReceiver<JRightPadded<T>> rightPaddedNodeReceiver(Class<T> nodeType) {
        return Extensions.rightPaddedNodeReceiver(nodeType);
    }

    private static <T extends J> JRightPadded<T> receiveRightPaddedTree(@Nullable JRightPadded<T> rightPadded, @Nullable Class<?> type, ReceiverContext ctx) {
        return Extensions.receiveRightPaddedTree(rightPadded, type, ctx);
    }

    private static Space receiveSpace(@Nullable Space space, @Nullable Class<?> type, ReceiverContext ctx) {
        return Extensions.receiveSpace(space, type, ctx);
    }

    private static Comment receiveComment(@Nullable Comment comment, @Nullable Class<Comment> type, ReceiverContext ctx) {
        return Extensions.receiveComment(comment, type, ctx);
    }

}
