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

package org.openrewrite.csharp.remote;

import lombok.Value;
import org.jspecify.annotations.Nullable;
import org.openrewrite.Checksum;
import org.openrewrite.Cursor;
import org.openrewrite.FileAttributes;
import org.openrewrite.Tree;
import org.openrewrite.csharp.CSharpVisitor;
import org.openrewrite.csharp.tree.Cs;
import org.openrewrite.java.tree.J;
import org.openrewrite.java.tree.JContainer;
import org.openrewrite.java.tree.JLeftPadded;
import org.openrewrite.java.tree.JRightPadded;
import org.openrewrite.java.tree.JavaType;
import org.openrewrite.java.tree.Space;
import org.openrewrite.remote.Receiver;
import org.openrewrite.remote.ReceiverContext;
import org.openrewrite.remote.ReceiverFactory;

import java.nio.charset.Charset;
import java.nio.file.Path;
import java.util.UUID;
import java.util.function.Function;

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
            return compilationUnit.withEof(ctx.receiveNonNullNode(compilationUnit.getEof(), CSharpReceiver::receiveSpace));
        }

        @Override
        public Cs.OperatorDeclaration visitOperatorDeclaration(Cs.OperatorDeclaration operatorDeclaration, ReceiverContext ctx) {
            operatorDeclaration = operatorDeclaration.withId(ctx.receiveNonNullValue(operatorDeclaration.getId(), UUID.class));
            operatorDeclaration = operatorDeclaration.withPrefix(ctx.receiveNonNullNode(operatorDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            operatorDeclaration = operatorDeclaration.withMarkers(ctx.receiveNonNullNode(operatorDeclaration.getMarkers(), ctx::receiveMarkers));
            operatorDeclaration = operatorDeclaration.withAttributeLists(ctx.receiveNonNullNodes(operatorDeclaration.getAttributeLists(), ctx::receiveTree));
            operatorDeclaration = operatorDeclaration.withModifiers(ctx.receiveNonNullNodes(operatorDeclaration.getModifiers(), ctx::receiveTree));
            operatorDeclaration = operatorDeclaration.getPadding().withExplicitInterfaceSpecifier(ctx.receiveNode(operatorDeclaration.getPadding().getExplicitInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            operatorDeclaration = operatorDeclaration.withOperatorKeyword(ctx.receiveNonNullNode(operatorDeclaration.getOperatorKeyword(), ctx::receiveTree));
            operatorDeclaration = operatorDeclaration.withCheckedKeyword(ctx.receiveNode(operatorDeclaration.getCheckedKeyword(), ctx::receiveTree));
            operatorDeclaration = operatorDeclaration.getPadding().withOperatorToken(ctx.receiveNonNullNode(operatorDeclaration.getPadding().getOperatorToken(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.OperatorDeclaration.Operator.class)));
            operatorDeclaration = operatorDeclaration.withReturnType(ctx.receiveNonNullNode(operatorDeclaration.getReturnType(), ctx::receiveTree));
            operatorDeclaration = operatorDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(operatorDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            operatorDeclaration = operatorDeclaration.withBody(ctx.receiveNonNullNode(operatorDeclaration.getBody(), ctx::receiveTree));
            return operatorDeclaration.withMethodType(ctx.receiveValue(operatorDeclaration.getMethodType(), JavaType.Method.class));
        }

        @Override
        public Cs.RefExpression visitRefExpression(Cs.RefExpression refExpression, ReceiverContext ctx) {
            refExpression = refExpression.withId(ctx.receiveNonNullValue(refExpression.getId(), UUID.class));
            refExpression = refExpression.withPrefix(ctx.receiveNonNullNode(refExpression.getPrefix(), CSharpReceiver::receiveSpace));
            refExpression = refExpression.withMarkers(ctx.receiveNonNullNode(refExpression.getMarkers(), ctx::receiveMarkers));
            return refExpression.withExpression(ctx.receiveNonNullNode(refExpression.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.PointerType visitPointerType(Cs.PointerType pointerType, ReceiverContext ctx) {
            pointerType = pointerType.withId(ctx.receiveNonNullValue(pointerType.getId(), UUID.class));
            pointerType = pointerType.withPrefix(ctx.receiveNonNullNode(pointerType.getPrefix(), CSharpReceiver::receiveSpace));
            pointerType = pointerType.withMarkers(ctx.receiveNonNullNode(pointerType.getMarkers(), ctx::receiveMarkers));
            return pointerType.getPadding().withElementType(ctx.receiveNonNullNode(pointerType.getPadding().getElementType(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.RefType visitRefType(Cs.RefType refType, ReceiverContext ctx) {
            refType = refType.withId(ctx.receiveNonNullValue(refType.getId(), UUID.class));
            refType = refType.withPrefix(ctx.receiveNonNullNode(refType.getPrefix(), CSharpReceiver::receiveSpace));
            refType = refType.withMarkers(ctx.receiveNonNullNode(refType.getMarkers(), ctx::receiveMarkers));
            refType = refType.withReadonlyKeyword(ctx.receiveNode(refType.getReadonlyKeyword(), ctx::receiveTree));
            refType = refType.withTypeIdentifier(ctx.receiveNonNullNode(refType.getTypeIdentifier(), ctx::receiveTree));
            return refType.withType(ctx.receiveValue(refType.getType(), JavaType.class));
        }

        @Override
        public Cs.ForEachVariableLoop visitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, ReceiverContext ctx) {
            forEachVariableLoop = forEachVariableLoop.withId(ctx.receiveNonNullValue(forEachVariableLoop.getId(), UUID.class));
            forEachVariableLoop = forEachVariableLoop.withPrefix(ctx.receiveNonNullNode(forEachVariableLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forEachVariableLoop = forEachVariableLoop.withMarkers(ctx.receiveNonNullNode(forEachVariableLoop.getMarkers(), ctx::receiveMarkers));
            forEachVariableLoop = forEachVariableLoop.withControlElement(ctx.receiveNonNullNode(forEachVariableLoop.getControlElement(), ctx::receiveTree));
            return forEachVariableLoop.getPadding().withBody(ctx.receiveNonNullNode(forEachVariableLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.ForEachVariableLoop.Control visitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withVariable(ctx.receiveNonNullNode(control.getPadding().getVariable(), CSharpReceiver::receiveRightPaddedTree));
            return control.getPadding().withIterable(ctx.receiveNonNullNode(control.getPadding().getIterable(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.NameColon visitNameColon(Cs.NameColon nameColon, ReceiverContext ctx) {
            nameColon = nameColon.withId(ctx.receiveNonNullValue(nameColon.getId(), UUID.class));
            nameColon = nameColon.withPrefix(ctx.receiveNonNullNode(nameColon.getPrefix(), CSharpReceiver::receiveSpace));
            nameColon = nameColon.withMarkers(ctx.receiveNonNullNode(nameColon.getMarkers(), ctx::receiveMarkers));
            return nameColon.getPadding().withName(ctx.receiveNonNullNode(nameColon.getPadding().getName(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.Argument visitArgument(Cs.Argument argument, ReceiverContext ctx) {
            argument = argument.withId(ctx.receiveNonNullValue(argument.getId(), UUID.class));
            argument = argument.withPrefix(ctx.receiveNonNullNode(argument.getPrefix(), CSharpReceiver::receiveSpace));
            argument = argument.withMarkers(ctx.receiveNonNullNode(argument.getMarkers(), ctx::receiveMarkers));
            argument = argument.getPadding().withNameColumn(ctx.receiveNode(argument.getPadding().getNameColumn(), CSharpReceiver::receiveRightPaddedTree));
            argument = argument.withRefKindKeyword(ctx.receiveNode(argument.getRefKindKeyword(), ctx::receiveTree));
            return argument.withExpression(ctx.receiveNonNullNode(argument.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.AnnotatedStatement visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, ReceiverContext ctx) {
            annotatedStatement = annotatedStatement.withId(ctx.receiveNonNullValue(annotatedStatement.getId(), UUID.class));
            annotatedStatement = annotatedStatement.withPrefix(ctx.receiveNonNullNode(annotatedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            annotatedStatement = annotatedStatement.withMarkers(ctx.receiveNonNullNode(annotatedStatement.getMarkers(), ctx::receiveMarkers));
            annotatedStatement = annotatedStatement.withAttributeLists(ctx.receiveNonNullNodes(annotatedStatement.getAttributeLists(), ctx::receiveTree));
            return annotatedStatement.withStatement(ctx.receiveNonNullNode(annotatedStatement.getStatement(), ctx::receiveTree));
        }

        @Override
        public Cs.ArrayRankSpecifier visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, ReceiverContext ctx) {
            arrayRankSpecifier = arrayRankSpecifier.withId(ctx.receiveNonNullValue(arrayRankSpecifier.getId(), UUID.class));
            arrayRankSpecifier = arrayRankSpecifier.withPrefix(ctx.receiveNonNullNode(arrayRankSpecifier.getPrefix(), CSharpReceiver::receiveSpace));
            arrayRankSpecifier = arrayRankSpecifier.withMarkers(ctx.receiveNonNullNode(arrayRankSpecifier.getMarkers(), ctx::receiveMarkers));
            return arrayRankSpecifier.getPadding().withSizes(ctx.receiveNonNullNode(arrayRankSpecifier.getPadding().getSizes(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.AssignmentOperation visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, ReceiverContext ctx) {
            assignmentOperation = assignmentOperation.withId(ctx.receiveNonNullValue(assignmentOperation.getId(), UUID.class));
            assignmentOperation = assignmentOperation.withPrefix(ctx.receiveNonNullNode(assignmentOperation.getPrefix(), CSharpReceiver::receiveSpace));
            assignmentOperation = assignmentOperation.withMarkers(ctx.receiveNonNullNode(assignmentOperation.getMarkers(), ctx::receiveMarkers));
            assignmentOperation = assignmentOperation.withVariable(ctx.receiveNonNullNode(assignmentOperation.getVariable(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.getPadding().withOperator(ctx.receiveNonNullNode(assignmentOperation.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AssignmentOperation.OperatorType.class)));
            assignmentOperation = assignmentOperation.withAssignment(ctx.receiveNonNullNode(assignmentOperation.getAssignment(), ctx::receiveTree));
            return assignmentOperation.withType(ctx.receiveValue(assignmentOperation.getType(), JavaType.class));
        }

        @Override
        public Cs.AttributeList visitAttributeList(Cs.AttributeList attributeList, ReceiverContext ctx) {
            attributeList = attributeList.withId(ctx.receiveNonNullValue(attributeList.getId(), UUID.class));
            attributeList = attributeList.withPrefix(ctx.receiveNonNullNode(attributeList.getPrefix(), CSharpReceiver::receiveSpace));
            attributeList = attributeList.withMarkers(ctx.receiveNonNullNode(attributeList.getMarkers(), ctx::receiveMarkers));
            attributeList = attributeList.getPadding().withTarget(ctx.receiveNode(attributeList.getPadding().getTarget(), CSharpReceiver::receiveRightPaddedTree));
            return attributeList.getPadding().withAttributes(ctx.receiveNonNullNodes(attributeList.getPadding().getAttributes(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.AwaitExpression visitAwaitExpression(Cs.AwaitExpression awaitExpression, ReceiverContext ctx) {
            awaitExpression = awaitExpression.withId(ctx.receiveNonNullValue(awaitExpression.getId(), UUID.class));
            awaitExpression = awaitExpression.withPrefix(ctx.receiveNonNullNode(awaitExpression.getPrefix(), CSharpReceiver::receiveSpace));
            awaitExpression = awaitExpression.withMarkers(ctx.receiveNonNullNode(awaitExpression.getMarkers(), ctx::receiveMarkers));
            awaitExpression = awaitExpression.withExpression(ctx.receiveNonNullNode(awaitExpression.getExpression(), ctx::receiveTree));
            return awaitExpression.withType(ctx.receiveValue(awaitExpression.getType(), JavaType.class));
        }

        @Override
        public Cs.StackAllocExpression visitStackAllocExpression(Cs.StackAllocExpression stackAllocExpression, ReceiverContext ctx) {
            stackAllocExpression = stackAllocExpression.withId(ctx.receiveNonNullValue(stackAllocExpression.getId(), UUID.class));
            stackAllocExpression = stackAllocExpression.withPrefix(ctx.receiveNonNullNode(stackAllocExpression.getPrefix(), CSharpReceiver::receiveSpace));
            stackAllocExpression = stackAllocExpression.withMarkers(ctx.receiveNonNullNode(stackAllocExpression.getMarkers(), ctx::receiveMarkers));
            return stackAllocExpression.withExpression(ctx.receiveNonNullNode(stackAllocExpression.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.GotoStatement visitGotoStatement(Cs.GotoStatement gotoStatement, ReceiverContext ctx) {
            gotoStatement = gotoStatement.withId(ctx.receiveNonNullValue(gotoStatement.getId(), UUID.class));
            gotoStatement = gotoStatement.withPrefix(ctx.receiveNonNullNode(gotoStatement.getPrefix(), CSharpReceiver::receiveSpace));
            gotoStatement = gotoStatement.withMarkers(ctx.receiveNonNullNode(gotoStatement.getMarkers(), ctx::receiveMarkers));
            gotoStatement = gotoStatement.withCaseOrDefaultKeyword(ctx.receiveNode(gotoStatement.getCaseOrDefaultKeyword(), ctx::receiveTree));
            return gotoStatement.withTarget(ctx.receiveNode(gotoStatement.getTarget(), ctx::receiveTree));
        }

        @Override
        public Cs.EventDeclaration visitEventDeclaration(Cs.EventDeclaration eventDeclaration, ReceiverContext ctx) {
            eventDeclaration = eventDeclaration.withId(ctx.receiveNonNullValue(eventDeclaration.getId(), UUID.class));
            eventDeclaration = eventDeclaration.withPrefix(ctx.receiveNonNullNode(eventDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            eventDeclaration = eventDeclaration.withMarkers(ctx.receiveNonNullNode(eventDeclaration.getMarkers(), ctx::receiveMarkers));
            eventDeclaration = eventDeclaration.withAttributeLists(ctx.receiveNonNullNodes(eventDeclaration.getAttributeLists(), ctx::receiveTree));
            eventDeclaration = eventDeclaration.withModifiers(ctx.receiveNonNullNodes(eventDeclaration.getModifiers(), ctx::receiveTree));
            eventDeclaration = eventDeclaration.getPadding().withTypeExpression(ctx.receiveNonNullNode(eventDeclaration.getPadding().getTypeExpression(), CSharpReceiver::receiveLeftPaddedTree));
            eventDeclaration = eventDeclaration.getPadding().withInterfaceSpecifier(ctx.receiveNode(eventDeclaration.getPadding().getInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            eventDeclaration = eventDeclaration.withName(ctx.receiveNonNullNode(eventDeclaration.getName(), ctx::receiveTree));
            return eventDeclaration.getPadding().withAccessors(ctx.receiveNode(eventDeclaration.getPadding().getAccessors(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.Binary visitBinary(Cs.Binary binary, ReceiverContext ctx) {
            binary = binary.withId(ctx.receiveNonNullValue(binary.getId(), UUID.class));
            binary = binary.withPrefix(ctx.receiveNonNullNode(binary.getPrefix(), CSharpReceiver::receiveSpace));
            binary = binary.withMarkers(ctx.receiveNonNullNode(binary.getMarkers(), ctx::receiveMarkers));
            binary = binary.withLeft(ctx.receiveNonNullNode(binary.getLeft(), ctx::receiveTree));
            binary = binary.getPadding().withOperator(ctx.receiveNonNullNode(binary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Binary.OperatorType.class)));
            binary = binary.withRight(ctx.receiveNonNullNode(binary.getRight(), ctx::receiveTree));
            return binary.withType(ctx.receiveValue(binary.getType(), JavaType.class));
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
            return blockScopeNamespaceDeclaration.withEnd(ctx.receiveNonNullNode(blockScopeNamespaceDeclaration.getEnd(), CSharpReceiver::receiveSpace));
        }

        @Override
        public Cs.CollectionExpression visitCollectionExpression(Cs.CollectionExpression collectionExpression, ReceiverContext ctx) {
            collectionExpression = collectionExpression.withId(ctx.receiveNonNullValue(collectionExpression.getId(), UUID.class));
            collectionExpression = collectionExpression.withPrefix(ctx.receiveNonNullNode(collectionExpression.getPrefix(), CSharpReceiver::receiveSpace));
            collectionExpression = collectionExpression.withMarkers(ctx.receiveNonNullNode(collectionExpression.getMarkers(), ctx::receiveMarkers));
            collectionExpression = collectionExpression.getPadding().withElements(ctx.receiveNonNullNodes(collectionExpression.getPadding().getElements(), CSharpReceiver::receiveRightPaddedTree));
            return collectionExpression.withType(ctx.receiveValue(collectionExpression.getType(), JavaType.class));
        }

        @Override
        public Cs.ExpressionStatement visitExpressionStatement(Cs.ExpressionStatement expressionStatement, ReceiverContext ctx) {
            expressionStatement = expressionStatement.withId(ctx.receiveNonNullValue(expressionStatement.getId(), UUID.class));
            expressionStatement = expressionStatement.withPrefix(ctx.receiveNonNullNode(expressionStatement.getPrefix(), CSharpReceiver::receiveSpace));
            expressionStatement = expressionStatement.withMarkers(ctx.receiveNonNullNode(expressionStatement.getMarkers(), ctx::receiveMarkers));
            return expressionStatement.getPadding().withExpression(ctx.receiveNonNullNode(expressionStatement.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.ExternAlias visitExternAlias(Cs.ExternAlias externAlias, ReceiverContext ctx) {
            externAlias = externAlias.withId(ctx.receiveNonNullValue(externAlias.getId(), UUID.class));
            externAlias = externAlias.withPrefix(ctx.receiveNonNullNode(externAlias.getPrefix(), CSharpReceiver::receiveSpace));
            externAlias = externAlias.withMarkers(ctx.receiveNonNullNode(externAlias.getMarkers(), ctx::receiveMarkers));
            return externAlias.getPadding().withIdentifier(ctx.receiveNonNullNode(externAlias.getPadding().getIdentifier(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.FileScopeNamespaceDeclaration visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, ReceiverContext ctx) {
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withId(ctx.receiveNonNullValue(fileScopeNamespaceDeclaration.getId(), UUID.class));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withPrefix(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.withMarkers(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getMarkers(), ctx::receiveMarkers));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withName(ctx.receiveNonNullNode(fileScopeNamespaceDeclaration.getPadding().getName(), CSharpReceiver::receiveRightPaddedTree));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withExterns(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getExterns(), CSharpReceiver::receiveRightPaddedTree));
            fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.getPadding().withUsings(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getUsings(), CSharpReceiver::receiveRightPaddedTree));
            return fileScopeNamespaceDeclaration.getPadding().withMembers(ctx.receiveNonNullNodes(fileScopeNamespaceDeclaration.getPadding().getMembers(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.InterpolatedString visitInterpolatedString(Cs.InterpolatedString interpolatedString, ReceiverContext ctx) {
            interpolatedString = interpolatedString.withId(ctx.receiveNonNullValue(interpolatedString.getId(), UUID.class));
            interpolatedString = interpolatedString.withPrefix(ctx.receiveNonNullNode(interpolatedString.getPrefix(), CSharpReceiver::receiveSpace));
            interpolatedString = interpolatedString.withMarkers(ctx.receiveNonNullNode(interpolatedString.getMarkers(), ctx::receiveMarkers));
            interpolatedString = interpolatedString.withStart(ctx.receiveNonNullValue(interpolatedString.getStart(), String.class));
            interpolatedString = interpolatedString.getPadding().withParts(ctx.receiveNonNullNodes(interpolatedString.getPadding().getParts(), CSharpReceiver::receiveRightPaddedTree));
            return interpolatedString.withEnd(ctx.receiveNonNullValue(interpolatedString.getEnd(), String.class));
        }

        @Override
        public Cs.Interpolation visitInterpolation(Cs.Interpolation interpolation, ReceiverContext ctx) {
            interpolation = interpolation.withId(ctx.receiveNonNullValue(interpolation.getId(), UUID.class));
            interpolation = interpolation.withPrefix(ctx.receiveNonNullNode(interpolation.getPrefix(), CSharpReceiver::receiveSpace));
            interpolation = interpolation.withMarkers(ctx.receiveNonNullNode(interpolation.getMarkers(), ctx::receiveMarkers));
            interpolation = interpolation.getPadding().withExpression(ctx.receiveNonNullNode(interpolation.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            interpolation = interpolation.getPadding().withAlignment(ctx.receiveNode(interpolation.getPadding().getAlignment(), CSharpReceiver::receiveRightPaddedTree));
            return interpolation.getPadding().withFormat(ctx.receiveNode(interpolation.getPadding().getFormat(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.NullSafeExpression visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, ReceiverContext ctx) {
            nullSafeExpression = nullSafeExpression.withId(ctx.receiveNonNullValue(nullSafeExpression.getId(), UUID.class));
            nullSafeExpression = nullSafeExpression.withPrefix(ctx.receiveNonNullNode(nullSafeExpression.getPrefix(), CSharpReceiver::receiveSpace));
            nullSafeExpression = nullSafeExpression.withMarkers(ctx.receiveNonNullNode(nullSafeExpression.getMarkers(), ctx::receiveMarkers));
            return nullSafeExpression.getPadding().withExpression(ctx.receiveNonNullNode(nullSafeExpression.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.StatementExpression visitStatementExpression(Cs.StatementExpression statementExpression, ReceiverContext ctx) {
            statementExpression = statementExpression.withId(ctx.receiveNonNullValue(statementExpression.getId(), UUID.class));
            statementExpression = statementExpression.withPrefix(ctx.receiveNonNullNode(statementExpression.getPrefix(), CSharpReceiver::receiveSpace));
            statementExpression = statementExpression.withMarkers(ctx.receiveNonNullNode(statementExpression.getMarkers(), ctx::receiveMarkers));
            return statementExpression.withStatement(ctx.receiveNonNullNode(statementExpression.getStatement(), ctx::receiveTree));
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
            return usingDirective.withNamespaceOrType(ctx.receiveNonNullNode(usingDirective.getNamespaceOrType(), ctx::receiveTree));
        }

        @Override
        public Cs.PropertyDeclaration visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, ReceiverContext ctx) {
            propertyDeclaration = propertyDeclaration.withId(ctx.receiveNonNullValue(propertyDeclaration.getId(), UUID.class));
            propertyDeclaration = propertyDeclaration.withPrefix(ctx.receiveNonNullNode(propertyDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            propertyDeclaration = propertyDeclaration.withMarkers(ctx.receiveNonNullNode(propertyDeclaration.getMarkers(), ctx::receiveMarkers));
            propertyDeclaration = propertyDeclaration.withAttributeLists(ctx.receiveNonNullNodes(propertyDeclaration.getAttributeLists(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withModifiers(ctx.receiveNonNullNodes(propertyDeclaration.getModifiers(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withTypeExpression(ctx.receiveNonNullNode(propertyDeclaration.getTypeExpression(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.getPadding().withInterfaceSpecifier(ctx.receiveNode(propertyDeclaration.getPadding().getInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            propertyDeclaration = propertyDeclaration.withName(ctx.receiveNonNullNode(propertyDeclaration.getName(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withAccessors(ctx.receiveNode(propertyDeclaration.getAccessors(), ctx::receiveTree));
            propertyDeclaration = propertyDeclaration.withExpressionBody(ctx.receiveNode(propertyDeclaration.getExpressionBody(), ctx::receiveTree));
            return propertyDeclaration.getPadding().withInitializer(ctx.receiveNode(propertyDeclaration.getPadding().getInitializer(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.Keyword visitKeyword(Cs.Keyword keyword, ReceiverContext ctx) {
            keyword = keyword.withId(ctx.receiveNonNullValue(keyword.getId(), UUID.class));
            keyword = keyword.withPrefix(ctx.receiveNonNullNode(keyword.getPrefix(), CSharpReceiver::receiveSpace));
            keyword = keyword.withMarkers(ctx.receiveNonNullNode(keyword.getMarkers(), ctx::receiveMarkers));
            return keyword.withKind(ctx.receiveNonNullValue(keyword.getKind(), Cs.Keyword.KeywordKind.class));
        }

        @Override
        public Cs.Lambda visitLambda(Cs.Lambda lambda, ReceiverContext ctx) {
            lambda = lambda.withId(ctx.receiveNonNullValue(lambda.getId(), UUID.class));
            lambda = lambda.withPrefix(ctx.receiveNonNullNode(lambda.getPrefix(), CSharpReceiver::receiveSpace));
            lambda = lambda.withMarkers(ctx.receiveNonNullNode(lambda.getMarkers(), ctx::receiveMarkers));
            lambda = lambda.withLambdaExpression(ctx.receiveNonNullNode(lambda.getLambdaExpression(), ctx::receiveTree));
            lambda = lambda.withReturnType(ctx.receiveNode(lambda.getReturnType(), ctx::receiveTree));
            return lambda.withModifiers(ctx.receiveNonNullNodes(lambda.getModifiers(), ctx::receiveTree));
        }

        @Override
        public Cs.ClassDeclaration visitClassDeclaration(Cs.ClassDeclaration classDeclaration, ReceiverContext ctx) {
            classDeclaration = classDeclaration.withId(ctx.receiveNonNullValue(classDeclaration.getId(), UUID.class));
            classDeclaration = classDeclaration.withPrefix(ctx.receiveNonNullNode(classDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            classDeclaration = classDeclaration.withMarkers(ctx.receiveNonNullNode(classDeclaration.getMarkers(), ctx::receiveMarkers));
            classDeclaration = classDeclaration.withAttributeList(ctx.receiveNonNullNodes(classDeclaration.getAttributeList(), ctx::receiveTree));
            classDeclaration = classDeclaration.withModifiers(ctx.receiveNonNullNodes(classDeclaration.getModifiers(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withKind(ctx.receiveNonNullNode(classDeclaration.getPadding().getKind(), CSharpReceiver::receiveClassDeclarationKind));
            classDeclaration = classDeclaration.withName(ctx.receiveNonNullNode(classDeclaration.getName(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withTypeParameters(ctx.receiveNode(classDeclaration.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withPrimaryConstructor(ctx.receiveNode(classDeclaration.getPadding().getPrimaryConstructor(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withExtendings(ctx.receiveNode(classDeclaration.getPadding().getExtendings(), CSharpReceiver::receiveLeftPaddedTree));
            classDeclaration = classDeclaration.getPadding().withImplementings(ctx.receiveNode(classDeclaration.getPadding().getImplementings(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.withBody(ctx.receiveNode(classDeclaration.getBody(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withTypeParameterConstraintClauses(ctx.receiveNode(classDeclaration.getPadding().getTypeParameterConstraintClauses(), CSharpReceiver::receiveContainer));
            return classDeclaration.withType(ctx.receiveValue(classDeclaration.getType(), JavaType.FullyQualified.class));
        }

        @Override
        public Cs.MethodDeclaration visitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, ReceiverContext ctx) {
            methodDeclaration = methodDeclaration.withId(ctx.receiveNonNullValue(methodDeclaration.getId(), UUID.class));
            methodDeclaration = methodDeclaration.withPrefix(ctx.receiveNonNullNode(methodDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            methodDeclaration = methodDeclaration.withMarkers(ctx.receiveNonNullNode(methodDeclaration.getMarkers(), ctx::receiveMarkers));
            methodDeclaration = methodDeclaration.withAttributes(ctx.receiveNonNullNodes(methodDeclaration.getAttributes(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.withModifiers(ctx.receiveNonNullNodes(methodDeclaration.getModifiers(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withTypeParameters(ctx.receiveNode(methodDeclaration.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.withReturnTypeExpression(ctx.receiveNonNullNode(methodDeclaration.getReturnTypeExpression(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withExplicitInterfaceSpecifier(ctx.receiveNode(methodDeclaration.getPadding().getExplicitInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            methodDeclaration = methodDeclaration.withName(ctx.receiveNonNullNode(methodDeclaration.getName(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(methodDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.withBody(ctx.receiveNode(methodDeclaration.getBody(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.withMethodType(ctx.receiveValue(methodDeclaration.getMethodType(), JavaType.Method.class));
            return methodDeclaration.getPadding().withTypeParameterConstraintClauses(ctx.receiveNonNullNode(methodDeclaration.getPadding().getTypeParameterConstraintClauses(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.UsingStatement visitUsingStatement(Cs.UsingStatement usingStatement, ReceiverContext ctx) {
            usingStatement = usingStatement.withId(ctx.receiveNonNullValue(usingStatement.getId(), UUID.class));
            usingStatement = usingStatement.withPrefix(ctx.receiveNonNullNode(usingStatement.getPrefix(), CSharpReceiver::receiveSpace));
            usingStatement = usingStatement.withMarkers(ctx.receiveNonNullNode(usingStatement.getMarkers(), ctx::receiveMarkers));
            usingStatement = usingStatement.withAwaitKeyword(ctx.receiveNode(usingStatement.getAwaitKeyword(), ctx::receiveTree));
            usingStatement = usingStatement.getPadding().withExpression(ctx.receiveNonNullNode(usingStatement.getPadding().getExpression(), CSharpReceiver::receiveLeftPaddedTree));
            return usingStatement.withStatement(ctx.receiveNonNullNode(usingStatement.getStatement(), ctx::receiveTree));
        }

        @Override
        public Cs.TypeParameterConstraintClause visitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, ReceiverContext ctx) {
            typeParameterConstraintClause = typeParameterConstraintClause.withId(ctx.receiveNonNullValue(typeParameterConstraintClause.getId(), UUID.class));
            typeParameterConstraintClause = typeParameterConstraintClause.withPrefix(ctx.receiveNonNullNode(typeParameterConstraintClause.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameterConstraintClause = typeParameterConstraintClause.withMarkers(ctx.receiveNonNullNode(typeParameterConstraintClause.getMarkers(), ctx::receiveMarkers));
            typeParameterConstraintClause = typeParameterConstraintClause.getPadding().withTypeParameter(ctx.receiveNonNullNode(typeParameterConstraintClause.getPadding().getTypeParameter(), CSharpReceiver::receiveRightPaddedTree));
            return typeParameterConstraintClause.getPadding().withTypeParameterConstraints(ctx.receiveNonNullNode(typeParameterConstraintClause.getPadding().getTypeParameterConstraints(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.TypeConstraint visitTypeConstraint(Cs.TypeConstraint typeConstraint, ReceiverContext ctx) {
            typeConstraint = typeConstraint.withId(ctx.receiveNonNullValue(typeConstraint.getId(), UUID.class));
            typeConstraint = typeConstraint.withPrefix(ctx.receiveNonNullNode(typeConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            typeConstraint = typeConstraint.withMarkers(ctx.receiveNonNullNode(typeConstraint.getMarkers(), ctx::receiveMarkers));
            return typeConstraint.withTypeExpression(ctx.receiveNonNullNode(typeConstraint.getTypeExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.AllowsConstraintClause visitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, ReceiverContext ctx) {
            allowsConstraintClause = allowsConstraintClause.withId(ctx.receiveNonNullValue(allowsConstraintClause.getId(), UUID.class));
            allowsConstraintClause = allowsConstraintClause.withPrefix(ctx.receiveNonNullNode(allowsConstraintClause.getPrefix(), CSharpReceiver::receiveSpace));
            allowsConstraintClause = allowsConstraintClause.withMarkers(ctx.receiveNonNullNode(allowsConstraintClause.getMarkers(), ctx::receiveMarkers));
            return allowsConstraintClause.getPadding().withExpressions(ctx.receiveNonNullNode(allowsConstraintClause.getPadding().getExpressions(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.RefStructConstraint visitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, ReceiverContext ctx) {
            refStructConstraint = refStructConstraint.withId(ctx.receiveNonNullValue(refStructConstraint.getId(), UUID.class));
            refStructConstraint = refStructConstraint.withPrefix(ctx.receiveNonNullNode(refStructConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            return refStructConstraint.withMarkers(ctx.receiveNonNullNode(refStructConstraint.getMarkers(), ctx::receiveMarkers));
        }

        @Override
        public Cs.ClassOrStructConstraint visitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, ReceiverContext ctx) {
            classOrStructConstraint = classOrStructConstraint.withId(ctx.receiveNonNullValue(classOrStructConstraint.getId(), UUID.class));
            classOrStructConstraint = classOrStructConstraint.withPrefix(ctx.receiveNonNullNode(classOrStructConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            classOrStructConstraint = classOrStructConstraint.withMarkers(ctx.receiveNonNullNode(classOrStructConstraint.getMarkers(), ctx::receiveMarkers));
            return classOrStructConstraint.withKind(ctx.receiveNonNullValue(classOrStructConstraint.getKind(), Cs.ClassOrStructConstraint.TypeKind.class));
        }

        @Override
        public Cs.ConstructorConstraint visitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, ReceiverContext ctx) {
            constructorConstraint = constructorConstraint.withId(ctx.receiveNonNullValue(constructorConstraint.getId(), UUID.class));
            constructorConstraint = constructorConstraint.withPrefix(ctx.receiveNonNullNode(constructorConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            return constructorConstraint.withMarkers(ctx.receiveNonNullNode(constructorConstraint.getMarkers(), ctx::receiveMarkers));
        }

        @Override
        public Cs.DefaultConstraint visitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, ReceiverContext ctx) {
            defaultConstraint = defaultConstraint.withId(ctx.receiveNonNullValue(defaultConstraint.getId(), UUID.class));
            defaultConstraint = defaultConstraint.withPrefix(ctx.receiveNonNullNode(defaultConstraint.getPrefix(), CSharpReceiver::receiveSpace));
            return defaultConstraint.withMarkers(ctx.receiveNonNullNode(defaultConstraint.getMarkers(), ctx::receiveMarkers));
        }

        @Override
        public Cs.DeclarationExpression visitDeclarationExpression(Cs.DeclarationExpression declarationExpression, ReceiverContext ctx) {
            declarationExpression = declarationExpression.withId(ctx.receiveNonNullValue(declarationExpression.getId(), UUID.class));
            declarationExpression = declarationExpression.withPrefix(ctx.receiveNonNullNode(declarationExpression.getPrefix(), CSharpReceiver::receiveSpace));
            declarationExpression = declarationExpression.withMarkers(ctx.receiveNonNullNode(declarationExpression.getMarkers(), ctx::receiveMarkers));
            declarationExpression = declarationExpression.withTypeExpression(ctx.receiveNode(declarationExpression.getTypeExpression(), ctx::receiveTree));
            return declarationExpression.withVariables(ctx.receiveNonNullNode(declarationExpression.getVariables(), ctx::receiveTree));
        }

        @Override
        public Cs.SingleVariableDesignation visitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, ReceiverContext ctx) {
            singleVariableDesignation = singleVariableDesignation.withId(ctx.receiveNonNullValue(singleVariableDesignation.getId(), UUID.class));
            singleVariableDesignation = singleVariableDesignation.withPrefix(ctx.receiveNonNullNode(singleVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            singleVariableDesignation = singleVariableDesignation.withMarkers(ctx.receiveNonNullNode(singleVariableDesignation.getMarkers(), ctx::receiveMarkers));
            return singleVariableDesignation.withName(ctx.receiveNonNullNode(singleVariableDesignation.getName(), ctx::receiveTree));
        }

        @Override
        public Cs.ParenthesizedVariableDesignation visitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, ReceiverContext ctx) {
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withId(ctx.receiveNonNullValue(parenthesizedVariableDesignation.getId(), UUID.class));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withPrefix(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.withMarkers(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getMarkers(), ctx::receiveMarkers));
            parenthesizedVariableDesignation = parenthesizedVariableDesignation.getPadding().withVariables(ctx.receiveNonNullNode(parenthesizedVariableDesignation.getPadding().getVariables(), CSharpReceiver::receiveContainer));
            return parenthesizedVariableDesignation.withType(ctx.receiveValue(parenthesizedVariableDesignation.getType(), JavaType.class));
        }

        @Override
        public Cs.DiscardVariableDesignation visitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, ReceiverContext ctx) {
            discardVariableDesignation = discardVariableDesignation.withId(ctx.receiveNonNullValue(discardVariableDesignation.getId(), UUID.class));
            discardVariableDesignation = discardVariableDesignation.withPrefix(ctx.receiveNonNullNode(discardVariableDesignation.getPrefix(), CSharpReceiver::receiveSpace));
            discardVariableDesignation = discardVariableDesignation.withMarkers(ctx.receiveNonNullNode(discardVariableDesignation.getMarkers(), ctx::receiveMarkers));
            return discardVariableDesignation.withDiscard(ctx.receiveNonNullNode(discardVariableDesignation.getDiscard(), ctx::receiveTree));
        }

        @Override
        public Cs.TupleExpression visitTupleExpression(Cs.TupleExpression tupleExpression, ReceiverContext ctx) {
            tupleExpression = tupleExpression.withId(ctx.receiveNonNullValue(tupleExpression.getId(), UUID.class));
            tupleExpression = tupleExpression.withPrefix(ctx.receiveNonNullNode(tupleExpression.getPrefix(), CSharpReceiver::receiveSpace));
            tupleExpression = tupleExpression.withMarkers(ctx.receiveNonNullNode(tupleExpression.getMarkers(), ctx::receiveMarkers));
            return tupleExpression.getPadding().withArguments(ctx.receiveNonNullNode(tupleExpression.getPadding().getArguments(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.Constructor visitConstructor(Cs.Constructor constructor, ReceiverContext ctx) {
            constructor = constructor.withId(ctx.receiveNonNullValue(constructor.getId(), UUID.class));
            constructor = constructor.withPrefix(ctx.receiveNonNullNode(constructor.getPrefix(), CSharpReceiver::receiveSpace));
            constructor = constructor.withMarkers(ctx.receiveNonNullNode(constructor.getMarkers(), ctx::receiveMarkers));
            constructor = constructor.withInitializer(ctx.receiveNode(constructor.getInitializer(), ctx::receiveTree));
            return constructor.withConstructorCore(ctx.receiveNonNullNode(constructor.getConstructorCore(), ctx::receiveTree));
        }

        @Override
        public Cs.DestructorDeclaration visitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, ReceiverContext ctx) {
            destructorDeclaration = destructorDeclaration.withId(ctx.receiveNonNullValue(destructorDeclaration.getId(), UUID.class));
            destructorDeclaration = destructorDeclaration.withPrefix(ctx.receiveNonNullNode(destructorDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            destructorDeclaration = destructorDeclaration.withMarkers(ctx.receiveNonNullNode(destructorDeclaration.getMarkers(), ctx::receiveMarkers));
            return destructorDeclaration.withMethodCore(ctx.receiveNonNullNode(destructorDeclaration.getMethodCore(), ctx::receiveTree));
        }

        @Override
        public Cs.Unary visitUnary(Cs.Unary unary, ReceiverContext ctx) {
            unary = unary.withId(ctx.receiveNonNullValue(unary.getId(), UUID.class));
            unary = unary.withPrefix(ctx.receiveNonNullNode(unary.getPrefix(), CSharpReceiver::receiveSpace));
            unary = unary.withMarkers(ctx.receiveNonNullNode(unary.getMarkers(), ctx::receiveMarkers));
            unary = unary.getPadding().withOperator(ctx.receiveNonNullNode(unary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Unary.Type.class)));
            unary = unary.withExpression(ctx.receiveNonNullNode(unary.getExpression(), ctx::receiveTree));
            return unary.withType(ctx.receiveValue(unary.getType(), JavaType.class));
        }

        @Override
        public Cs.ConstructorInitializer visitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, ReceiverContext ctx) {
            constructorInitializer = constructorInitializer.withId(ctx.receiveNonNullValue(constructorInitializer.getId(), UUID.class));
            constructorInitializer = constructorInitializer.withPrefix(ctx.receiveNonNullNode(constructorInitializer.getPrefix(), CSharpReceiver::receiveSpace));
            constructorInitializer = constructorInitializer.withMarkers(ctx.receiveNonNullNode(constructorInitializer.getMarkers(), ctx::receiveMarkers));
            constructorInitializer = constructorInitializer.withKeyword(ctx.receiveNonNullNode(constructorInitializer.getKeyword(), ctx::receiveTree));
            return constructorInitializer.getPadding().withArguments(ctx.receiveNonNullNode(constructorInitializer.getPadding().getArguments(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.TupleType visitTupleType(Cs.TupleType tupleType, ReceiverContext ctx) {
            tupleType = tupleType.withId(ctx.receiveNonNullValue(tupleType.getId(), UUID.class));
            tupleType = tupleType.withPrefix(ctx.receiveNonNullNode(tupleType.getPrefix(), CSharpReceiver::receiveSpace));
            tupleType = tupleType.withMarkers(ctx.receiveNonNullNode(tupleType.getMarkers(), ctx::receiveMarkers));
            tupleType = tupleType.getPadding().withElements(ctx.receiveNonNullNode(tupleType.getPadding().getElements(), CSharpReceiver::receiveContainer));
            return tupleType.withType(ctx.receiveValue(tupleType.getType(), JavaType.class));
        }

        @Override
        public Cs.TupleElement visitTupleElement(Cs.TupleElement tupleElement, ReceiverContext ctx) {
            tupleElement = tupleElement.withId(ctx.receiveNonNullValue(tupleElement.getId(), UUID.class));
            tupleElement = tupleElement.withPrefix(ctx.receiveNonNullNode(tupleElement.getPrefix(), CSharpReceiver::receiveSpace));
            tupleElement = tupleElement.withMarkers(ctx.receiveNonNullNode(tupleElement.getMarkers(), ctx::receiveMarkers));
            tupleElement = tupleElement.withType(ctx.receiveNonNullNode(tupleElement.getType(), ctx::receiveTree));
            return tupleElement.withName(ctx.receiveNode(tupleElement.getName(), ctx::receiveTree));
        }

        @Override
        public Cs.NewClass visitNewClass(Cs.NewClass newClass, ReceiverContext ctx) {
            newClass = newClass.withId(ctx.receiveNonNullValue(newClass.getId(), UUID.class));
            newClass = newClass.withPrefix(ctx.receiveNonNullNode(newClass.getPrefix(), CSharpReceiver::receiveSpace));
            newClass = newClass.withMarkers(ctx.receiveNonNullNode(newClass.getMarkers(), ctx::receiveMarkers));
            newClass = newClass.withNewClassCore(ctx.receiveNonNullNode(newClass.getNewClassCore(), ctx::receiveTree));
            return newClass.withInitializer(ctx.receiveNode(newClass.getInitializer(), ctx::receiveTree));
        }

        @Override
        public Cs.InitializerExpression visitInitializerExpression(Cs.InitializerExpression initializerExpression, ReceiverContext ctx) {
            initializerExpression = initializerExpression.withId(ctx.receiveNonNullValue(initializerExpression.getId(), UUID.class));
            initializerExpression = initializerExpression.withPrefix(ctx.receiveNonNullNode(initializerExpression.getPrefix(), CSharpReceiver::receiveSpace));
            initializerExpression = initializerExpression.withMarkers(ctx.receiveNonNullNode(initializerExpression.getMarkers(), ctx::receiveMarkers));
            return initializerExpression.getPadding().withExpressions(ctx.receiveNonNullNode(initializerExpression.getPadding().getExpressions(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.ImplicitElementAccess visitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, ReceiverContext ctx) {
            implicitElementAccess = implicitElementAccess.withId(ctx.receiveNonNullValue(implicitElementAccess.getId(), UUID.class));
            implicitElementAccess = implicitElementAccess.withPrefix(ctx.receiveNonNullNode(implicitElementAccess.getPrefix(), CSharpReceiver::receiveSpace));
            implicitElementAccess = implicitElementAccess.withMarkers(ctx.receiveNonNullNode(implicitElementAccess.getMarkers(), ctx::receiveMarkers));
            return implicitElementAccess.getPadding().withArgumentList(ctx.receiveNonNullNode(implicitElementAccess.getPadding().getArgumentList(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.Yield visitYield(Cs.Yield yield, ReceiverContext ctx) {
            yield = yield.withId(ctx.receiveNonNullValue(yield.getId(), UUID.class));
            yield = yield.withPrefix(ctx.receiveNonNullNode(yield.getPrefix(), CSharpReceiver::receiveSpace));
            yield = yield.withMarkers(ctx.receiveNonNullNode(yield.getMarkers(), ctx::receiveMarkers));
            yield = yield.withReturnOrBreakKeyword(ctx.receiveNonNullNode(yield.getReturnOrBreakKeyword(), ctx::receiveTree));
            return yield.withExpression(ctx.receiveNode(yield.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.DefaultExpression visitDefaultExpression(Cs.DefaultExpression defaultExpression, ReceiverContext ctx) {
            defaultExpression = defaultExpression.withId(ctx.receiveNonNullValue(defaultExpression.getId(), UUID.class));
            defaultExpression = defaultExpression.withPrefix(ctx.receiveNonNullNode(defaultExpression.getPrefix(), CSharpReceiver::receiveSpace));
            defaultExpression = defaultExpression.withMarkers(ctx.receiveNonNullNode(defaultExpression.getMarkers(), ctx::receiveMarkers));
            return defaultExpression.getPadding().withTypeOperator(ctx.receiveNode(defaultExpression.getPadding().getTypeOperator(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.IsPattern visitIsPattern(Cs.IsPattern isPattern, ReceiverContext ctx) {
            isPattern = isPattern.withId(ctx.receiveNonNullValue(isPattern.getId(), UUID.class));
            isPattern = isPattern.withPrefix(ctx.receiveNonNullNode(isPattern.getPrefix(), CSharpReceiver::receiveSpace));
            isPattern = isPattern.withMarkers(ctx.receiveNonNullNode(isPattern.getMarkers(), ctx::receiveMarkers));
            isPattern = isPattern.withExpression(ctx.receiveNonNullNode(isPattern.getExpression(), ctx::receiveTree));
            return isPattern.getPadding().withPattern(ctx.receiveNonNullNode(isPattern.getPadding().getPattern(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.UnaryPattern visitUnaryPattern(Cs.UnaryPattern unaryPattern, ReceiverContext ctx) {
            unaryPattern = unaryPattern.withId(ctx.receiveNonNullValue(unaryPattern.getId(), UUID.class));
            unaryPattern = unaryPattern.withPrefix(ctx.receiveNonNullNode(unaryPattern.getPrefix(), CSharpReceiver::receiveSpace));
            unaryPattern = unaryPattern.withMarkers(ctx.receiveNonNullNode(unaryPattern.getMarkers(), ctx::receiveMarkers));
            unaryPattern = unaryPattern.withOperator(ctx.receiveNonNullNode(unaryPattern.getOperator(), ctx::receiveTree));
            return unaryPattern.withPattern(ctx.receiveNonNullNode(unaryPattern.getPattern(), ctx::receiveTree));
        }

        @Override
        public Cs.TypePattern visitTypePattern(Cs.TypePattern typePattern, ReceiverContext ctx) {
            typePattern = typePattern.withId(ctx.receiveNonNullValue(typePattern.getId(), UUID.class));
            typePattern = typePattern.withPrefix(ctx.receiveNonNullNode(typePattern.getPrefix(), CSharpReceiver::receiveSpace));
            typePattern = typePattern.withMarkers(ctx.receiveNonNullNode(typePattern.getMarkers(), ctx::receiveMarkers));
            typePattern = typePattern.withTypeIdentifier(ctx.receiveNonNullNode(typePattern.getTypeIdentifier(), ctx::receiveTree));
            return typePattern.withDesignation(ctx.receiveNode(typePattern.getDesignation(), ctx::receiveTree));
        }

        @Override
        public Cs.BinaryPattern visitBinaryPattern(Cs.BinaryPattern binaryPattern, ReceiverContext ctx) {
            binaryPattern = binaryPattern.withId(ctx.receiveNonNullValue(binaryPattern.getId(), UUID.class));
            binaryPattern = binaryPattern.withPrefix(ctx.receiveNonNullNode(binaryPattern.getPrefix(), CSharpReceiver::receiveSpace));
            binaryPattern = binaryPattern.withMarkers(ctx.receiveNonNullNode(binaryPattern.getMarkers(), ctx::receiveMarkers));
            binaryPattern = binaryPattern.withLeft(ctx.receiveNonNullNode(binaryPattern.getLeft(), ctx::receiveTree));
            binaryPattern = binaryPattern.getPadding().withOperator(ctx.receiveNonNullNode(binaryPattern.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.BinaryPattern.OperatorType.class)));
            return binaryPattern.withRight(ctx.receiveNonNullNode(binaryPattern.getRight(), ctx::receiveTree));
        }

        @Override
        public Cs.ConstantPattern visitConstantPattern(Cs.ConstantPattern constantPattern, ReceiverContext ctx) {
            constantPattern = constantPattern.withId(ctx.receiveNonNullValue(constantPattern.getId(), UUID.class));
            constantPattern = constantPattern.withPrefix(ctx.receiveNonNullNode(constantPattern.getPrefix(), CSharpReceiver::receiveSpace));
            constantPattern = constantPattern.withMarkers(ctx.receiveNonNullNode(constantPattern.getMarkers(), ctx::receiveMarkers));
            return constantPattern.withValue(ctx.receiveNonNullNode(constantPattern.getValue(), ctx::receiveTree));
        }

        @Override
        public Cs.DiscardPattern visitDiscardPattern(Cs.DiscardPattern discardPattern, ReceiverContext ctx) {
            discardPattern = discardPattern.withId(ctx.receiveNonNullValue(discardPattern.getId(), UUID.class));
            discardPattern = discardPattern.withPrefix(ctx.receiveNonNullNode(discardPattern.getPrefix(), CSharpReceiver::receiveSpace));
            discardPattern = discardPattern.withMarkers(ctx.receiveNonNullNode(discardPattern.getMarkers(), ctx::receiveMarkers));
            return discardPattern.withType(ctx.receiveValue(discardPattern.getType(), JavaType.class));
        }

        @Override
        public Cs.ListPattern visitListPattern(Cs.ListPattern listPattern, ReceiverContext ctx) {
            listPattern = listPattern.withId(ctx.receiveNonNullValue(listPattern.getId(), UUID.class));
            listPattern = listPattern.withPrefix(ctx.receiveNonNullNode(listPattern.getPrefix(), CSharpReceiver::receiveSpace));
            listPattern = listPattern.withMarkers(ctx.receiveNonNullNode(listPattern.getMarkers(), ctx::receiveMarkers));
            listPattern = listPattern.getPadding().withPatterns(ctx.receiveNonNullNode(listPattern.getPadding().getPatterns(), CSharpReceiver::receiveContainer));
            return listPattern.withDesignation(ctx.receiveNode(listPattern.getDesignation(), ctx::receiveTree));
        }

        @Override
        public Cs.ParenthesizedPattern visitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, ReceiverContext ctx) {
            parenthesizedPattern = parenthesizedPattern.withId(ctx.receiveNonNullValue(parenthesizedPattern.getId(), UUID.class));
            parenthesizedPattern = parenthesizedPattern.withPrefix(ctx.receiveNonNullNode(parenthesizedPattern.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedPattern = parenthesizedPattern.withMarkers(ctx.receiveNonNullNode(parenthesizedPattern.getMarkers(), ctx::receiveMarkers));
            return parenthesizedPattern.getPadding().withPattern(ctx.receiveNonNullNode(parenthesizedPattern.getPadding().getPattern(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.RecursivePattern visitRecursivePattern(Cs.RecursivePattern recursivePattern, ReceiverContext ctx) {
            recursivePattern = recursivePattern.withId(ctx.receiveNonNullValue(recursivePattern.getId(), UUID.class));
            recursivePattern = recursivePattern.withPrefix(ctx.receiveNonNullNode(recursivePattern.getPrefix(), CSharpReceiver::receiveSpace));
            recursivePattern = recursivePattern.withMarkers(ctx.receiveNonNullNode(recursivePattern.getMarkers(), ctx::receiveMarkers));
            recursivePattern = recursivePattern.withTypeQualifier(ctx.receiveNode(recursivePattern.getTypeQualifier(), ctx::receiveTree));
            recursivePattern = recursivePattern.withPositionalPattern(ctx.receiveNode(recursivePattern.getPositionalPattern(), ctx::receiveTree));
            recursivePattern = recursivePattern.withPropertyPattern(ctx.receiveNode(recursivePattern.getPropertyPattern(), ctx::receiveTree));
            return recursivePattern.withDesignation(ctx.receiveNode(recursivePattern.getDesignation(), ctx::receiveTree));
        }

        @Override
        public Cs.VarPattern visitVarPattern(Cs.VarPattern varPattern, ReceiverContext ctx) {
            varPattern = varPattern.withId(ctx.receiveNonNullValue(varPattern.getId(), UUID.class));
            varPattern = varPattern.withPrefix(ctx.receiveNonNullNode(varPattern.getPrefix(), CSharpReceiver::receiveSpace));
            varPattern = varPattern.withMarkers(ctx.receiveNonNullNode(varPattern.getMarkers(), ctx::receiveMarkers));
            return varPattern.withDesignation(ctx.receiveNonNullNode(varPattern.getDesignation(), ctx::receiveTree));
        }

        @Override
        public Cs.PositionalPatternClause visitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, ReceiverContext ctx) {
            positionalPatternClause = positionalPatternClause.withId(ctx.receiveNonNullValue(positionalPatternClause.getId(), UUID.class));
            positionalPatternClause = positionalPatternClause.withPrefix(ctx.receiveNonNullNode(positionalPatternClause.getPrefix(), CSharpReceiver::receiveSpace));
            positionalPatternClause = positionalPatternClause.withMarkers(ctx.receiveNonNullNode(positionalPatternClause.getMarkers(), ctx::receiveMarkers));
            return positionalPatternClause.getPadding().withSubpatterns(ctx.receiveNonNullNode(positionalPatternClause.getPadding().getSubpatterns(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.RelationalPattern visitRelationalPattern(Cs.RelationalPattern relationalPattern, ReceiverContext ctx) {
            relationalPattern = relationalPattern.withId(ctx.receiveNonNullValue(relationalPattern.getId(), UUID.class));
            relationalPattern = relationalPattern.withPrefix(ctx.receiveNonNullNode(relationalPattern.getPrefix(), CSharpReceiver::receiveSpace));
            relationalPattern = relationalPattern.withMarkers(ctx.receiveNonNullNode(relationalPattern.getMarkers(), ctx::receiveMarkers));
            relationalPattern = relationalPattern.getPadding().withOperator(ctx.receiveNonNullNode(relationalPattern.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.RelationalPattern.OperatorType.class)));
            return relationalPattern.withValue(ctx.receiveNonNullNode(relationalPattern.getValue(), ctx::receiveTree));
        }

        @Override
        public Cs.SlicePattern visitSlicePattern(Cs.SlicePattern slicePattern, ReceiverContext ctx) {
            slicePattern = slicePattern.withId(ctx.receiveNonNullValue(slicePattern.getId(), UUID.class));
            slicePattern = slicePattern.withPrefix(ctx.receiveNonNullNode(slicePattern.getPrefix(), CSharpReceiver::receiveSpace));
            return slicePattern.withMarkers(ctx.receiveNonNullNode(slicePattern.getMarkers(), ctx::receiveMarkers));
        }

        @Override
        public Cs.PropertyPatternClause visitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, ReceiverContext ctx) {
            propertyPatternClause = propertyPatternClause.withId(ctx.receiveNonNullValue(propertyPatternClause.getId(), UUID.class));
            propertyPatternClause = propertyPatternClause.withPrefix(ctx.receiveNonNullNode(propertyPatternClause.getPrefix(), CSharpReceiver::receiveSpace));
            propertyPatternClause = propertyPatternClause.withMarkers(ctx.receiveNonNullNode(propertyPatternClause.getMarkers(), ctx::receiveMarkers));
            return propertyPatternClause.getPadding().withSubpatterns(ctx.receiveNonNullNode(propertyPatternClause.getPadding().getSubpatterns(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.Subpattern visitSubpattern(Cs.Subpattern subpattern, ReceiverContext ctx) {
            subpattern = subpattern.withId(ctx.receiveNonNullValue(subpattern.getId(), UUID.class));
            subpattern = subpattern.withPrefix(ctx.receiveNonNullNode(subpattern.getPrefix(), CSharpReceiver::receiveSpace));
            subpattern = subpattern.withMarkers(ctx.receiveNonNullNode(subpattern.getMarkers(), ctx::receiveMarkers));
            subpattern = subpattern.withName(ctx.receiveNode(subpattern.getName(), ctx::receiveTree));
            return subpattern.getPadding().withPattern(ctx.receiveNonNullNode(subpattern.getPadding().getPattern(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.SwitchExpression visitSwitchExpression(Cs.SwitchExpression switchExpression, ReceiverContext ctx) {
            switchExpression = switchExpression.withId(ctx.receiveNonNullValue(switchExpression.getId(), UUID.class));
            switchExpression = switchExpression.withPrefix(ctx.receiveNonNullNode(switchExpression.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpression = switchExpression.withMarkers(ctx.receiveNonNullNode(switchExpression.getMarkers(), ctx::receiveMarkers));
            switchExpression = switchExpression.getPadding().withExpression(ctx.receiveNonNullNode(switchExpression.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            return switchExpression.getPadding().withArms(ctx.receiveNonNullNode(switchExpression.getPadding().getArms(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.SwitchExpressionArm visitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, ReceiverContext ctx) {
            switchExpressionArm = switchExpressionArm.withId(ctx.receiveNonNullValue(switchExpressionArm.getId(), UUID.class));
            switchExpressionArm = switchExpressionArm.withPrefix(ctx.receiveNonNullNode(switchExpressionArm.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpressionArm = switchExpressionArm.withMarkers(ctx.receiveNonNullNode(switchExpressionArm.getMarkers(), ctx::receiveMarkers));
            switchExpressionArm = switchExpressionArm.withPattern(ctx.receiveNonNullNode(switchExpressionArm.getPattern(), ctx::receiveTree));
            switchExpressionArm = switchExpressionArm.getPadding().withWhenExpression(ctx.receiveNode(switchExpressionArm.getPadding().getWhenExpression(), CSharpReceiver::receiveLeftPaddedTree));
            return switchExpressionArm.getPadding().withExpression(ctx.receiveNonNullNode(switchExpressionArm.getPadding().getExpression(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.SwitchSection visitSwitchSection(Cs.SwitchSection switchSection, ReceiverContext ctx) {
            switchSection = switchSection.withId(ctx.receiveNonNullValue(switchSection.getId(), UUID.class));
            switchSection = switchSection.withPrefix(ctx.receiveNonNullNode(switchSection.getPrefix(), CSharpReceiver::receiveSpace));
            switchSection = switchSection.withMarkers(ctx.receiveNonNullNode(switchSection.getMarkers(), ctx::receiveMarkers));
            switchSection = switchSection.withLabels(ctx.receiveNonNullNodes(switchSection.getLabels(), ctx::receiveTree));
            return switchSection.getPadding().withStatements(ctx.receiveNonNullNodes(switchSection.getPadding().getStatements(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.DefaultSwitchLabel visitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, ReceiverContext ctx) {
            defaultSwitchLabel = defaultSwitchLabel.withId(ctx.receiveNonNullValue(defaultSwitchLabel.getId(), UUID.class));
            defaultSwitchLabel = defaultSwitchLabel.withPrefix(ctx.receiveNonNullNode(defaultSwitchLabel.getPrefix(), CSharpReceiver::receiveSpace));
            defaultSwitchLabel = defaultSwitchLabel.withMarkers(ctx.receiveNonNullNode(defaultSwitchLabel.getMarkers(), ctx::receiveMarkers));
            return defaultSwitchLabel.withColonToken(ctx.receiveNonNullNode(defaultSwitchLabel.getColonToken(), CSharpReceiver::receiveSpace));
        }

        @Override
        public Cs.CasePatternSwitchLabel visitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, ReceiverContext ctx) {
            casePatternSwitchLabel = casePatternSwitchLabel.withId(ctx.receiveNonNullValue(casePatternSwitchLabel.getId(), UUID.class));
            casePatternSwitchLabel = casePatternSwitchLabel.withPrefix(ctx.receiveNonNullNode(casePatternSwitchLabel.getPrefix(), CSharpReceiver::receiveSpace));
            casePatternSwitchLabel = casePatternSwitchLabel.withMarkers(ctx.receiveNonNullNode(casePatternSwitchLabel.getMarkers(), ctx::receiveMarkers));
            casePatternSwitchLabel = casePatternSwitchLabel.withPattern(ctx.receiveNonNullNode(casePatternSwitchLabel.getPattern(), ctx::receiveTree));
            casePatternSwitchLabel = casePatternSwitchLabel.getPadding().withWhenClause(ctx.receiveNode(casePatternSwitchLabel.getPadding().getWhenClause(), CSharpReceiver::receiveLeftPaddedTree));
            return casePatternSwitchLabel.withColonToken(ctx.receiveNonNullNode(casePatternSwitchLabel.getColonToken(), CSharpReceiver::receiveSpace));
        }

        @Override
        public Cs.SwitchStatement visitSwitchStatement(Cs.SwitchStatement switchStatement, ReceiverContext ctx) {
            switchStatement = switchStatement.withId(ctx.receiveNonNullValue(switchStatement.getId(), UUID.class));
            switchStatement = switchStatement.withPrefix(ctx.receiveNonNullNode(switchStatement.getPrefix(), CSharpReceiver::receiveSpace));
            switchStatement = switchStatement.withMarkers(ctx.receiveNonNullNode(switchStatement.getMarkers(), ctx::receiveMarkers));
            switchStatement = switchStatement.getPadding().withExpression(ctx.receiveNonNullNode(switchStatement.getPadding().getExpression(), CSharpReceiver::receiveContainer));
            return switchStatement.getPadding().withSections(ctx.receiveNonNullNode(switchStatement.getPadding().getSections(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.LockStatement visitLockStatement(Cs.LockStatement lockStatement, ReceiverContext ctx) {
            lockStatement = lockStatement.withId(ctx.receiveNonNullValue(lockStatement.getId(), UUID.class));
            lockStatement = lockStatement.withPrefix(ctx.receiveNonNullNode(lockStatement.getPrefix(), CSharpReceiver::receiveSpace));
            lockStatement = lockStatement.withMarkers(ctx.receiveNonNullNode(lockStatement.getMarkers(), ctx::receiveMarkers));
            lockStatement = lockStatement.withExpression(ctx.receiveNonNullNode(lockStatement.getExpression(), ctx::receiveTree));
            return lockStatement.getPadding().withStatement(ctx.receiveNonNullNode(lockStatement.getPadding().getStatement(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.FixedStatement visitFixedStatement(Cs.FixedStatement fixedStatement, ReceiverContext ctx) {
            fixedStatement = fixedStatement.withId(ctx.receiveNonNullValue(fixedStatement.getId(), UUID.class));
            fixedStatement = fixedStatement.withPrefix(ctx.receiveNonNullNode(fixedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            fixedStatement = fixedStatement.withMarkers(ctx.receiveNonNullNode(fixedStatement.getMarkers(), ctx::receiveMarkers));
            fixedStatement = fixedStatement.withDeclarations(ctx.receiveNonNullNode(fixedStatement.getDeclarations(), ctx::receiveTree));
            return fixedStatement.withBlock(ctx.receiveNonNullNode(fixedStatement.getBlock(), ctx::receiveTree));
        }

        @Override
        public Cs.CheckedExpression visitCheckedExpression(Cs.CheckedExpression checkedExpression, ReceiverContext ctx) {
            checkedExpression = checkedExpression.withId(ctx.receiveNonNullValue(checkedExpression.getId(), UUID.class));
            checkedExpression = checkedExpression.withPrefix(ctx.receiveNonNullNode(checkedExpression.getPrefix(), CSharpReceiver::receiveSpace));
            checkedExpression = checkedExpression.withMarkers(ctx.receiveNonNullNode(checkedExpression.getMarkers(), ctx::receiveMarkers));
            checkedExpression = checkedExpression.withCheckedOrUncheckedKeyword(ctx.receiveNonNullNode(checkedExpression.getCheckedOrUncheckedKeyword(), ctx::receiveTree));
            return checkedExpression.withExpression(ctx.receiveNonNullNode(checkedExpression.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.CheckedStatement visitCheckedStatement(Cs.CheckedStatement checkedStatement, ReceiverContext ctx) {
            checkedStatement = checkedStatement.withId(ctx.receiveNonNullValue(checkedStatement.getId(), UUID.class));
            checkedStatement = checkedStatement.withPrefix(ctx.receiveNonNullNode(checkedStatement.getPrefix(), CSharpReceiver::receiveSpace));
            checkedStatement = checkedStatement.withMarkers(ctx.receiveNonNullNode(checkedStatement.getMarkers(), ctx::receiveMarkers));
            checkedStatement = checkedStatement.withKeyword(ctx.receiveNonNullNode(checkedStatement.getKeyword(), ctx::receiveTree));
            return checkedStatement.withBlock(ctx.receiveNonNullNode(checkedStatement.getBlock(), ctx::receiveTree));
        }

        @Override
        public Cs.UnsafeStatement visitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, ReceiverContext ctx) {
            unsafeStatement = unsafeStatement.withId(ctx.receiveNonNullValue(unsafeStatement.getId(), UUID.class));
            unsafeStatement = unsafeStatement.withPrefix(ctx.receiveNonNullNode(unsafeStatement.getPrefix(), CSharpReceiver::receiveSpace));
            unsafeStatement = unsafeStatement.withMarkers(ctx.receiveNonNullNode(unsafeStatement.getMarkers(), ctx::receiveMarkers));
            return unsafeStatement.withBlock(ctx.receiveNonNullNode(unsafeStatement.getBlock(), ctx::receiveTree));
        }

        @Override
        public Cs.RangeExpression visitRangeExpression(Cs.RangeExpression rangeExpression, ReceiverContext ctx) {
            rangeExpression = rangeExpression.withId(ctx.receiveNonNullValue(rangeExpression.getId(), UUID.class));
            rangeExpression = rangeExpression.withPrefix(ctx.receiveNonNullNode(rangeExpression.getPrefix(), CSharpReceiver::receiveSpace));
            rangeExpression = rangeExpression.withMarkers(ctx.receiveNonNullNode(rangeExpression.getMarkers(), ctx::receiveMarkers));
            rangeExpression = rangeExpression.getPadding().withStart(ctx.receiveNode(rangeExpression.getPadding().getStart(), CSharpReceiver::receiveRightPaddedTree));
            return rangeExpression.withEnd(ctx.receiveNode(rangeExpression.getEnd(), ctx::receiveTree));
        }

        @Override
        public Cs.QueryExpression visitQueryExpression(Cs.QueryExpression queryExpression, ReceiverContext ctx) {
            queryExpression = queryExpression.withId(ctx.receiveNonNullValue(queryExpression.getId(), UUID.class));
            queryExpression = queryExpression.withPrefix(ctx.receiveNonNullNode(queryExpression.getPrefix(), CSharpReceiver::receiveSpace));
            queryExpression = queryExpression.withMarkers(ctx.receiveNonNullNode(queryExpression.getMarkers(), ctx::receiveMarkers));
            queryExpression = queryExpression.withFromClause(ctx.receiveNonNullNode(queryExpression.getFromClause(), ctx::receiveTree));
            return queryExpression.withBody(ctx.receiveNonNullNode(queryExpression.getBody(), ctx::receiveTree));
        }

        @Override
        public Cs.QueryBody visitQueryBody(Cs.QueryBody queryBody, ReceiverContext ctx) {
            queryBody = queryBody.withId(ctx.receiveNonNullValue(queryBody.getId(), UUID.class));
            queryBody = queryBody.withPrefix(ctx.receiveNonNullNode(queryBody.getPrefix(), CSharpReceiver::receiveSpace));
            queryBody = queryBody.withMarkers(ctx.receiveNonNullNode(queryBody.getMarkers(), ctx::receiveMarkers));
            queryBody = queryBody.withClauses(ctx.receiveNonNullNodes(queryBody.getClauses(), ctx::receiveTree));
            queryBody = queryBody.withSelectOrGroup(ctx.receiveNode(queryBody.getSelectOrGroup(), ctx::receiveTree));
            return queryBody.withContinuation(ctx.receiveNode(queryBody.getContinuation(), ctx::receiveTree));
        }

        @Override
        public Cs.FromClause visitFromClause(Cs.FromClause fromClause, ReceiverContext ctx) {
            fromClause = fromClause.withId(ctx.receiveNonNullValue(fromClause.getId(), UUID.class));
            fromClause = fromClause.withPrefix(ctx.receiveNonNullNode(fromClause.getPrefix(), CSharpReceiver::receiveSpace));
            fromClause = fromClause.withMarkers(ctx.receiveNonNullNode(fromClause.getMarkers(), ctx::receiveMarkers));
            fromClause = fromClause.withTypeIdentifier(ctx.receiveNode(fromClause.getTypeIdentifier(), ctx::receiveTree));
            fromClause = fromClause.getPadding().withIdentifier(ctx.receiveNonNullNode(fromClause.getPadding().getIdentifier(), CSharpReceiver::receiveRightPaddedTree));
            return fromClause.withExpression(ctx.receiveNonNullNode(fromClause.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.LetClause visitLetClause(Cs.LetClause letClause, ReceiverContext ctx) {
            letClause = letClause.withId(ctx.receiveNonNullValue(letClause.getId(), UUID.class));
            letClause = letClause.withPrefix(ctx.receiveNonNullNode(letClause.getPrefix(), CSharpReceiver::receiveSpace));
            letClause = letClause.withMarkers(ctx.receiveNonNullNode(letClause.getMarkers(), ctx::receiveMarkers));
            letClause = letClause.getPadding().withIdentifier(ctx.receiveNonNullNode(letClause.getPadding().getIdentifier(), CSharpReceiver::receiveRightPaddedTree));
            return letClause.withExpression(ctx.receiveNonNullNode(letClause.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.JoinClause visitJoinClause(Cs.JoinClause joinClause, ReceiverContext ctx) {
            joinClause = joinClause.withId(ctx.receiveNonNullValue(joinClause.getId(), UUID.class));
            joinClause = joinClause.withPrefix(ctx.receiveNonNullNode(joinClause.getPrefix(), CSharpReceiver::receiveSpace));
            joinClause = joinClause.withMarkers(ctx.receiveNonNullNode(joinClause.getMarkers(), ctx::receiveMarkers));
            joinClause = joinClause.getPadding().withIdentifier(ctx.receiveNonNullNode(joinClause.getPadding().getIdentifier(), CSharpReceiver::receiveRightPaddedTree));
            joinClause = joinClause.getPadding().withInExpression(ctx.receiveNonNullNode(joinClause.getPadding().getInExpression(), CSharpReceiver::receiveRightPaddedTree));
            joinClause = joinClause.getPadding().withLeftExpression(ctx.receiveNonNullNode(joinClause.getPadding().getLeftExpression(), CSharpReceiver::receiveRightPaddedTree));
            joinClause = joinClause.withRightExpression(ctx.receiveNonNullNode(joinClause.getRightExpression(), ctx::receiveTree));
            return joinClause.getPadding().withInto(ctx.receiveNode(joinClause.getPadding().getInto(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.JoinIntoClause visitJoinIntoClause(Cs.JoinIntoClause joinIntoClause, ReceiverContext ctx) {
            joinIntoClause = joinIntoClause.withId(ctx.receiveNonNullValue(joinIntoClause.getId(), UUID.class));
            joinIntoClause = joinIntoClause.withPrefix(ctx.receiveNonNullNode(joinIntoClause.getPrefix(), CSharpReceiver::receiveSpace));
            joinIntoClause = joinIntoClause.withMarkers(ctx.receiveNonNullNode(joinIntoClause.getMarkers(), ctx::receiveMarkers));
            return joinIntoClause.withIdentifier(ctx.receiveNonNullNode(joinIntoClause.getIdentifier(), ctx::receiveTree));
        }

        @Override
        public Cs.WhereClause visitWhereClause(Cs.WhereClause whereClause, ReceiverContext ctx) {
            whereClause = whereClause.withId(ctx.receiveNonNullValue(whereClause.getId(), UUID.class));
            whereClause = whereClause.withPrefix(ctx.receiveNonNullNode(whereClause.getPrefix(), CSharpReceiver::receiveSpace));
            whereClause = whereClause.withMarkers(ctx.receiveNonNullNode(whereClause.getMarkers(), ctx::receiveMarkers));
            return whereClause.withCondition(ctx.receiveNonNullNode(whereClause.getCondition(), ctx::receiveTree));
        }

        @Override
        public Cs.OrderByClause visitOrderByClause(Cs.OrderByClause orderByClause, ReceiverContext ctx) {
            orderByClause = orderByClause.withId(ctx.receiveNonNullValue(orderByClause.getId(), UUID.class));
            orderByClause = orderByClause.withPrefix(ctx.receiveNonNullNode(orderByClause.getPrefix(), CSharpReceiver::receiveSpace));
            orderByClause = orderByClause.withMarkers(ctx.receiveNonNullNode(orderByClause.getMarkers(), ctx::receiveMarkers));
            return orderByClause.getPadding().withOrderings(ctx.receiveNonNullNodes(orderByClause.getPadding().getOrderings(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.QueryContinuation visitQueryContinuation(Cs.QueryContinuation queryContinuation, ReceiverContext ctx) {
            queryContinuation = queryContinuation.withId(ctx.receiveNonNullValue(queryContinuation.getId(), UUID.class));
            queryContinuation = queryContinuation.withPrefix(ctx.receiveNonNullNode(queryContinuation.getPrefix(), CSharpReceiver::receiveSpace));
            queryContinuation = queryContinuation.withMarkers(ctx.receiveNonNullNode(queryContinuation.getMarkers(), ctx::receiveMarkers));
            queryContinuation = queryContinuation.withIdentifier(ctx.receiveNonNullNode(queryContinuation.getIdentifier(), ctx::receiveTree));
            return queryContinuation.withBody(ctx.receiveNonNullNode(queryContinuation.getBody(), ctx::receiveTree));
        }

        @Override
        public Cs.Ordering visitOrdering(Cs.Ordering ordering, ReceiverContext ctx) {
            ordering = ordering.withId(ctx.receiveNonNullValue(ordering.getId(), UUID.class));
            ordering = ordering.withPrefix(ctx.receiveNonNullNode(ordering.getPrefix(), CSharpReceiver::receiveSpace));
            ordering = ordering.withMarkers(ctx.receiveNonNullNode(ordering.getMarkers(), ctx::receiveMarkers));
            ordering = ordering.getPadding().withExpression(ctx.receiveNonNullNode(ordering.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            return ordering.withDirection(ctx.receiveValue(ordering.getDirection(), Cs.Ordering.DirectionKind.class));
        }

        @Override
        public Cs.SelectClause visitSelectClause(Cs.SelectClause selectClause, ReceiverContext ctx) {
            selectClause = selectClause.withId(ctx.receiveNonNullValue(selectClause.getId(), UUID.class));
            selectClause = selectClause.withPrefix(ctx.receiveNonNullNode(selectClause.getPrefix(), CSharpReceiver::receiveSpace));
            selectClause = selectClause.withMarkers(ctx.receiveNonNullNode(selectClause.getMarkers(), ctx::receiveMarkers));
            return selectClause.withExpression(ctx.receiveNonNullNode(selectClause.getExpression(), ctx::receiveTree));
        }

        @Override
        public Cs.GroupClause visitGroupClause(Cs.GroupClause groupClause, ReceiverContext ctx) {
            groupClause = groupClause.withId(ctx.receiveNonNullValue(groupClause.getId(), UUID.class));
            groupClause = groupClause.withPrefix(ctx.receiveNonNullNode(groupClause.getPrefix(), CSharpReceiver::receiveSpace));
            groupClause = groupClause.withMarkers(ctx.receiveNonNullNode(groupClause.getMarkers(), ctx::receiveMarkers));
            groupClause = groupClause.getPadding().withGroupExpression(ctx.receiveNonNullNode(groupClause.getPadding().getGroupExpression(), CSharpReceiver::receiveRightPaddedTree));
            return groupClause.withKey(ctx.receiveNonNullNode(groupClause.getKey(), ctx::receiveTree));
        }

        @Override
        public Cs.IndexerDeclaration visitIndexerDeclaration(Cs.IndexerDeclaration indexerDeclaration, ReceiverContext ctx) {
            indexerDeclaration = indexerDeclaration.withId(ctx.receiveNonNullValue(indexerDeclaration.getId(), UUID.class));
            indexerDeclaration = indexerDeclaration.withPrefix(ctx.receiveNonNullNode(indexerDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            indexerDeclaration = indexerDeclaration.withMarkers(ctx.receiveNonNullNode(indexerDeclaration.getMarkers(), ctx::receiveMarkers));
            indexerDeclaration = indexerDeclaration.withModifiers(ctx.receiveNonNullNodes(indexerDeclaration.getModifiers(), ctx::receiveTree));
            indexerDeclaration = indexerDeclaration.withTypeExpression(ctx.receiveNonNullNode(indexerDeclaration.getTypeExpression(), ctx::receiveTree));
            indexerDeclaration = indexerDeclaration.getPadding().withExplicitInterfaceSpecifier(ctx.receiveNode(indexerDeclaration.getPadding().getExplicitInterfaceSpecifier(), CSharpReceiver::receiveRightPaddedTree));
            indexerDeclaration = indexerDeclaration.withIndexer(ctx.receiveNonNullNode(indexerDeclaration.getIndexer(), ctx::receiveTree));
            indexerDeclaration = indexerDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(indexerDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            indexerDeclaration = indexerDeclaration.getPadding().withExpressionBody(ctx.receiveNode(indexerDeclaration.getPadding().getExpressionBody(), CSharpReceiver::receiveLeftPaddedTree));
            return indexerDeclaration.withAccessors(ctx.receiveNode(indexerDeclaration.getAccessors(), ctx::receiveTree));
        }

        @Override
        public Cs.DelegateDeclaration visitDelegateDeclaration(Cs.DelegateDeclaration delegateDeclaration, ReceiverContext ctx) {
            delegateDeclaration = delegateDeclaration.withId(ctx.receiveNonNullValue(delegateDeclaration.getId(), UUID.class));
            delegateDeclaration = delegateDeclaration.withPrefix(ctx.receiveNonNullNode(delegateDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            delegateDeclaration = delegateDeclaration.withMarkers(ctx.receiveNonNullNode(delegateDeclaration.getMarkers(), ctx::receiveMarkers));
            delegateDeclaration = delegateDeclaration.withAttributes(ctx.receiveNonNullNodes(delegateDeclaration.getAttributes(), ctx::receiveTree));
            delegateDeclaration = delegateDeclaration.withModifiers(ctx.receiveNonNullNodes(delegateDeclaration.getModifiers(), ctx::receiveTree));
            delegateDeclaration = delegateDeclaration.getPadding().withReturnType(ctx.receiveNonNullNode(delegateDeclaration.getPadding().getReturnType(), CSharpReceiver::receiveLeftPaddedTree));
            delegateDeclaration = delegateDeclaration.withIdentifier(ctx.receiveNonNullNode(delegateDeclaration.getIdentifier(), ctx::receiveTree));
            delegateDeclaration = delegateDeclaration.getPadding().withTypeParameters(ctx.receiveNode(delegateDeclaration.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            delegateDeclaration = delegateDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(delegateDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            return delegateDeclaration.getPadding().withTypeParameterConstraintClauses(ctx.receiveNode(delegateDeclaration.getPadding().getTypeParameterConstraintClauses(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.ConversionOperatorDeclaration visitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration conversionOperatorDeclaration, ReceiverContext ctx) {
            conversionOperatorDeclaration = conversionOperatorDeclaration.withId(ctx.receiveNonNullValue(conversionOperatorDeclaration.getId(), UUID.class));
            conversionOperatorDeclaration = conversionOperatorDeclaration.withPrefix(ctx.receiveNonNullNode(conversionOperatorDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            conversionOperatorDeclaration = conversionOperatorDeclaration.withMarkers(ctx.receiveNonNullNode(conversionOperatorDeclaration.getMarkers(), ctx::receiveMarkers));
            conversionOperatorDeclaration = conversionOperatorDeclaration.withModifiers(ctx.receiveNonNullNodes(conversionOperatorDeclaration.getModifiers(), ctx::receiveTree));
            conversionOperatorDeclaration = conversionOperatorDeclaration.getPadding().withKind(ctx.receiveNonNullNode(conversionOperatorDeclaration.getPadding().getKind(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.ConversionOperatorDeclaration.ExplicitImplicit.class)));
            conversionOperatorDeclaration = conversionOperatorDeclaration.getPadding().withReturnType(ctx.receiveNonNullNode(conversionOperatorDeclaration.getPadding().getReturnType(), CSharpReceiver::receiveLeftPaddedTree));
            conversionOperatorDeclaration = conversionOperatorDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(conversionOperatorDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            conversionOperatorDeclaration = conversionOperatorDeclaration.getPadding().withExpressionBody(ctx.receiveNode(conversionOperatorDeclaration.getPadding().getExpressionBody(), CSharpReceiver::receiveLeftPaddedTree));
            return conversionOperatorDeclaration.withBody(ctx.receiveNode(conversionOperatorDeclaration.getBody(), ctx::receiveTree));
        }

        @Override
        public Cs.TypeParameter visitTypeParameter(Cs.TypeParameter typeParameter, ReceiverContext ctx) {
            typeParameter = typeParameter.withId(ctx.receiveNonNullValue(typeParameter.getId(), UUID.class));
            typeParameter = typeParameter.withPrefix(ctx.receiveNonNullNode(typeParameter.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameter = typeParameter.withMarkers(ctx.receiveNonNullNode(typeParameter.getMarkers(), ctx::receiveMarkers));
            typeParameter = typeParameter.withAttributeLists(ctx.receiveNonNullNodes(typeParameter.getAttributeLists(), ctx::receiveTree));
            typeParameter = typeParameter.getPadding().withVariance(ctx.receiveNode(typeParameter.getPadding().getVariance(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.TypeParameter.VarianceKind.class)));
            return typeParameter.withName(ctx.receiveNonNullNode(typeParameter.getName(), ctx::receiveTree));
        }

        @Override
        public Cs.EnumDeclaration visitEnumDeclaration(Cs.EnumDeclaration enumDeclaration, ReceiverContext ctx) {
            enumDeclaration = enumDeclaration.withId(ctx.receiveNonNullValue(enumDeclaration.getId(), UUID.class));
            enumDeclaration = enumDeclaration.withPrefix(ctx.receiveNonNullNode(enumDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            enumDeclaration = enumDeclaration.withMarkers(ctx.receiveNonNullNode(enumDeclaration.getMarkers(), ctx::receiveMarkers));
            enumDeclaration = enumDeclaration.withAttributeLists(ctx.receiveNodes(enumDeclaration.getAttributeLists(), ctx::receiveTree));
            enumDeclaration = enumDeclaration.withModifiers(ctx.receiveNonNullNodes(enumDeclaration.getModifiers(), ctx::receiveTree));
            enumDeclaration = enumDeclaration.getPadding().withName(ctx.receiveNonNullNode(enumDeclaration.getPadding().getName(), CSharpReceiver::receiveLeftPaddedTree));
            enumDeclaration = enumDeclaration.getPadding().withBaseType(ctx.receiveNode(enumDeclaration.getPadding().getBaseType(), CSharpReceiver::receiveLeftPaddedTree));
            return enumDeclaration.getPadding().withMembers(ctx.receiveNode(enumDeclaration.getPadding().getMembers(), CSharpReceiver::receiveContainer));
        }

        @Override
        public Cs.EnumMemberDeclaration visitEnumMemberDeclaration(Cs.EnumMemberDeclaration enumMemberDeclaration, ReceiverContext ctx) {
            enumMemberDeclaration = enumMemberDeclaration.withId(ctx.receiveNonNullValue(enumMemberDeclaration.getId(), UUID.class));
            enumMemberDeclaration = enumMemberDeclaration.withPrefix(ctx.receiveNonNullNode(enumMemberDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            enumMemberDeclaration = enumMemberDeclaration.withMarkers(ctx.receiveNonNullNode(enumMemberDeclaration.getMarkers(), ctx::receiveMarkers));
            enumMemberDeclaration = enumMemberDeclaration.withAttributeLists(ctx.receiveNonNullNodes(enumMemberDeclaration.getAttributeLists(), ctx::receiveTree));
            enumMemberDeclaration = enumMemberDeclaration.withName(ctx.receiveNonNullNode(enumMemberDeclaration.getName(), ctx::receiveTree));
            return enumMemberDeclaration.getPadding().withInitializer(ctx.receiveNode(enumMemberDeclaration.getPadding().getInitializer(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.AliasQualifiedName visitAliasQualifiedName(Cs.AliasQualifiedName aliasQualifiedName, ReceiverContext ctx) {
            aliasQualifiedName = aliasQualifiedName.withId(ctx.receiveNonNullValue(aliasQualifiedName.getId(), UUID.class));
            aliasQualifiedName = aliasQualifiedName.withPrefix(ctx.receiveNonNullNode(aliasQualifiedName.getPrefix(), CSharpReceiver::receiveSpace));
            aliasQualifiedName = aliasQualifiedName.withMarkers(ctx.receiveNonNullNode(aliasQualifiedName.getMarkers(), ctx::receiveMarkers));
            aliasQualifiedName = aliasQualifiedName.getPadding().withAlias(ctx.receiveNonNullNode(aliasQualifiedName.getPadding().getAlias(), CSharpReceiver::receiveRightPaddedTree));
            return aliasQualifiedName.withName(ctx.receiveNonNullNode(aliasQualifiedName.getName(), ctx::receiveTree));
        }

        @Override
        public Cs.ArrayType visitArrayType(Cs.ArrayType arrayType, ReceiverContext ctx) {
            arrayType = arrayType.withId(ctx.receiveNonNullValue(arrayType.getId(), UUID.class));
            arrayType = arrayType.withPrefix(ctx.receiveNonNullNode(arrayType.getPrefix(), CSharpReceiver::receiveSpace));
            arrayType = arrayType.withMarkers(ctx.receiveNonNullNode(arrayType.getMarkers(), ctx::receiveMarkers));
            arrayType = arrayType.withTypeExpression(ctx.receiveNode(arrayType.getTypeExpression(), ctx::receiveTree));
            arrayType = arrayType.withDimensions(ctx.receiveNonNullNodes(arrayType.getDimensions(), ctx::receiveTree));
            return arrayType.withType(ctx.receiveValue(arrayType.getType(), JavaType.class));
        }

        @Override
        public Cs.Try visitTry(Cs.Try try_, ReceiverContext ctx) {
            try_ = try_.withId(ctx.receiveNonNullValue(try_.getId(), UUID.class));
            try_ = try_.withPrefix(ctx.receiveNonNullNode(try_.getPrefix(), CSharpReceiver::receiveSpace));
            try_ = try_.withMarkers(ctx.receiveNonNullNode(try_.getMarkers(), ctx::receiveMarkers));
            try_ = try_.withBody(ctx.receiveNonNullNode(try_.getBody(), ctx::receiveTree));
            try_ = try_.withCatches(ctx.receiveNonNullNodes(try_.getCatches(), ctx::receiveTree));
            return try_.getPadding().withFinally(ctx.receiveNode(try_.getPadding().getFinally(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public Cs.Try.Catch visitTryCatch(Cs.Try.Catch catch_, ReceiverContext ctx) {
            catch_ = catch_.withId(ctx.receiveNonNullValue(catch_.getId(), UUID.class));
            catch_ = catch_.withPrefix(ctx.receiveNonNullNode(catch_.getPrefix(), CSharpReceiver::receiveSpace));
            catch_ = catch_.withMarkers(ctx.receiveNonNullNode(catch_.getMarkers(), ctx::receiveMarkers));
            catch_ = catch_.withParameter(ctx.receiveNonNullNode(catch_.getParameter(), ctx::receiveTree));
            catch_ = catch_.getPadding().withFilterExpression(ctx.receiveNode(catch_.getPadding().getFilterExpression(), CSharpReceiver::receiveLeftPaddedTree));
            return catch_.withBody(ctx.receiveNonNullNode(catch_.getBody(), ctx::receiveTree));
        }

        @Override
        public Cs.ArrowExpressionClause visitArrowExpressionClause(Cs.ArrowExpressionClause arrowExpressionClause, ReceiverContext ctx) {
            arrowExpressionClause = arrowExpressionClause.withId(ctx.receiveNonNullValue(arrowExpressionClause.getId(), UUID.class));
            arrowExpressionClause = arrowExpressionClause.withPrefix(ctx.receiveNonNullNode(arrowExpressionClause.getPrefix(), CSharpReceiver::receiveSpace));
            arrowExpressionClause = arrowExpressionClause.withMarkers(ctx.receiveNonNullNode(arrowExpressionClause.getMarkers(), ctx::receiveMarkers));
            return arrowExpressionClause.getPadding().withExpression(ctx.receiveNonNullNode(arrowExpressionClause.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public Cs.AccessorDeclaration visitAccessorDeclaration(Cs.AccessorDeclaration accessorDeclaration, ReceiverContext ctx) {
            accessorDeclaration = accessorDeclaration.withId(ctx.receiveNonNullValue(accessorDeclaration.getId(), UUID.class));
            accessorDeclaration = accessorDeclaration.withPrefix(ctx.receiveNonNullNode(accessorDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            accessorDeclaration = accessorDeclaration.withMarkers(ctx.receiveNonNullNode(accessorDeclaration.getMarkers(), ctx::receiveMarkers));
            accessorDeclaration = accessorDeclaration.withAttributes(ctx.receiveNonNullNodes(accessorDeclaration.getAttributes(), ctx::receiveTree));
            accessorDeclaration = accessorDeclaration.withModifiers(ctx.receiveNonNullNodes(accessorDeclaration.getModifiers(), ctx::receiveTree));
            accessorDeclaration = accessorDeclaration.getPadding().withKind(ctx.receiveNonNullNode(accessorDeclaration.getPadding().getKind(), leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AccessorDeclaration.AccessorKinds.class)));
            accessorDeclaration = accessorDeclaration.withExpressionBody(ctx.receiveNode(accessorDeclaration.getExpressionBody(), ctx::receiveTree));
            return accessorDeclaration.withBody(ctx.receiveNode(accessorDeclaration.getBody(), ctx::receiveTree));
        }

        @Override
        public Cs.PointerFieldAccess visitPointerFieldAccess(Cs.PointerFieldAccess pointerFieldAccess, ReceiverContext ctx) {
            pointerFieldAccess = pointerFieldAccess.withId(ctx.receiveNonNullValue(pointerFieldAccess.getId(), UUID.class));
            pointerFieldAccess = pointerFieldAccess.withPrefix(ctx.receiveNonNullNode(pointerFieldAccess.getPrefix(), CSharpReceiver::receiveSpace));
            pointerFieldAccess = pointerFieldAccess.withMarkers(ctx.receiveNonNullNode(pointerFieldAccess.getMarkers(), ctx::receiveMarkers));
            pointerFieldAccess = pointerFieldAccess.withTarget(ctx.receiveNonNullNode(pointerFieldAccess.getTarget(), ctx::receiveTree));
            pointerFieldAccess = pointerFieldAccess.getPadding().withName(ctx.receiveNonNullNode(pointerFieldAccess.getPadding().getName(), CSharpReceiver::receiveLeftPaddedTree));
            return pointerFieldAccess.withType(ctx.receiveValue(pointerFieldAccess.getType(), JavaType.class));
        }

        @Override
        public J.AnnotatedType visitAnnotatedType(J.AnnotatedType annotatedType, ReceiverContext ctx) {
            annotatedType = annotatedType.withId(ctx.receiveNonNullValue(annotatedType.getId(), UUID.class));
            annotatedType = annotatedType.withPrefix(ctx.receiveNonNullNode(annotatedType.getPrefix(), CSharpReceiver::receiveSpace));
            annotatedType = annotatedType.withMarkers(ctx.receiveNonNullNode(annotatedType.getMarkers(), ctx::receiveMarkers));
            annotatedType = annotatedType.withAnnotations(ctx.receiveNonNullNodes(annotatedType.getAnnotations(), ctx::receiveTree));
            return annotatedType.withTypeExpression(ctx.receiveNonNullNode(annotatedType.getTypeExpression(), ctx::receiveTree));
        }

        @Override
        public J.Annotation visitAnnotation(J.Annotation annotation, ReceiverContext ctx) {
            annotation = annotation.withId(ctx.receiveNonNullValue(annotation.getId(), UUID.class));
            annotation = annotation.withPrefix(ctx.receiveNonNullNode(annotation.getPrefix(), CSharpReceiver::receiveSpace));
            annotation = annotation.withMarkers(ctx.receiveNonNullNode(annotation.getMarkers(), ctx::receiveMarkers));
            annotation = annotation.withAnnotationType(ctx.receiveNonNullNode(annotation.getAnnotationType(), ctx::receiveTree));
            return annotation.getPadding().withArguments(ctx.receiveNode(annotation.getPadding().getArguments(), CSharpReceiver::receiveContainer));
        }

        @Override
        public J.ArrayAccess visitArrayAccess(J.ArrayAccess arrayAccess, ReceiverContext ctx) {
            arrayAccess = arrayAccess.withId(ctx.receiveNonNullValue(arrayAccess.getId(), UUID.class));
            arrayAccess = arrayAccess.withPrefix(ctx.receiveNonNullNode(arrayAccess.getPrefix(), CSharpReceiver::receiveSpace));
            arrayAccess = arrayAccess.withMarkers(ctx.receiveNonNullNode(arrayAccess.getMarkers(), ctx::receiveMarkers));
            arrayAccess = arrayAccess.withIndexed(ctx.receiveNonNullNode(arrayAccess.getIndexed(), ctx::receiveTree));
            arrayAccess = arrayAccess.withDimension(ctx.receiveNonNullNode(arrayAccess.getDimension(), ctx::receiveTree));
            return arrayAccess.withType(ctx.receiveValue(arrayAccess.getType(), JavaType.class));
        }

        @Override
        public J.ArrayType visitArrayType(J.ArrayType arrayType, ReceiverContext ctx) {
            arrayType = arrayType.withId(ctx.receiveNonNullValue(arrayType.getId(), UUID.class));
            arrayType = arrayType.withPrefix(ctx.receiveNonNullNode(arrayType.getPrefix(), CSharpReceiver::receiveSpace));
            arrayType = arrayType.withMarkers(ctx.receiveNonNullNode(arrayType.getMarkers(), ctx::receiveMarkers));
            arrayType = arrayType.withElementType(ctx.receiveNonNullNode(arrayType.getElementType(), ctx::receiveTree));
            arrayType = arrayType.withAnnotations(ctx.receiveNodes(arrayType.getAnnotations(), ctx::receiveTree));
            arrayType = arrayType.withDimension(ctx.receiveNode(arrayType.getDimension(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            return arrayType.withType(ctx.receiveValue(arrayType.getType(), JavaType.class));
        }

        @Override
        public J.Assert visitAssert(J.Assert assert_, ReceiverContext ctx) {
            assert_ = assert_.withId(ctx.receiveNonNullValue(assert_.getId(), UUID.class));
            assert_ = assert_.withPrefix(ctx.receiveNonNullNode(assert_.getPrefix(), CSharpReceiver::receiveSpace));
            assert_ = assert_.withMarkers(ctx.receiveNonNullNode(assert_.getMarkers(), ctx::receiveMarkers));
            assert_ = assert_.withCondition(ctx.receiveNonNullNode(assert_.getCondition(), ctx::receiveTree));
            return assert_.withDetail(ctx.receiveNode(assert_.getDetail(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public J.Assignment visitAssignment(J.Assignment assignment, ReceiverContext ctx) {
            assignment = assignment.withId(ctx.receiveNonNullValue(assignment.getId(), UUID.class));
            assignment = assignment.withPrefix(ctx.receiveNonNullNode(assignment.getPrefix(), CSharpReceiver::receiveSpace));
            assignment = assignment.withMarkers(ctx.receiveNonNullNode(assignment.getMarkers(), ctx::receiveMarkers));
            assignment = assignment.withVariable(ctx.receiveNonNullNode(assignment.getVariable(), ctx::receiveTree));
            assignment = assignment.getPadding().withAssignment(ctx.receiveNonNullNode(assignment.getPadding().getAssignment(), CSharpReceiver::receiveLeftPaddedTree));
            return assignment.withType(ctx.receiveValue(assignment.getType(), JavaType.class));
        }

        @Override
        public J.AssignmentOperation visitAssignmentOperation(J.AssignmentOperation assignmentOperation, ReceiverContext ctx) {
            assignmentOperation = assignmentOperation.withId(ctx.receiveNonNullValue(assignmentOperation.getId(), UUID.class));
            assignmentOperation = assignmentOperation.withPrefix(ctx.receiveNonNullNode(assignmentOperation.getPrefix(), CSharpReceiver::receiveSpace));
            assignmentOperation = assignmentOperation.withMarkers(ctx.receiveNonNullNode(assignmentOperation.getMarkers(), ctx::receiveMarkers));
            assignmentOperation = assignmentOperation.withVariable(ctx.receiveNonNullNode(assignmentOperation.getVariable(), ctx::receiveTree));
            assignmentOperation = assignmentOperation.getPadding().withOperator(ctx.receiveNonNullNode(assignmentOperation.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.AssignmentOperation.Type.class)));
            assignmentOperation = assignmentOperation.withAssignment(ctx.receiveNonNullNode(assignmentOperation.getAssignment(), ctx::receiveTree));
            return assignmentOperation.withType(ctx.receiveValue(assignmentOperation.getType(), JavaType.class));
        }

        @Override
        public J.Binary visitBinary(J.Binary binary, ReceiverContext ctx) {
            binary = binary.withId(ctx.receiveNonNullValue(binary.getId(), UUID.class));
            binary = binary.withPrefix(ctx.receiveNonNullNode(binary.getPrefix(), CSharpReceiver::receiveSpace));
            binary = binary.withMarkers(ctx.receiveNonNullNode(binary.getMarkers(), ctx::receiveMarkers));
            binary = binary.withLeft(ctx.receiveNonNullNode(binary.getLeft(), ctx::receiveTree));
            binary = binary.getPadding().withOperator(ctx.receiveNonNullNode(binary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Binary.Type.class)));
            binary = binary.withRight(ctx.receiveNonNullNode(binary.getRight(), ctx::receiveTree));
            return binary.withType(ctx.receiveValue(binary.getType(), JavaType.class));
        }

        @Override
        public J.Block visitBlock(J.Block block, ReceiverContext ctx) {
            block = block.withId(ctx.receiveNonNullValue(block.getId(), UUID.class));
            block = block.withPrefix(ctx.receiveNonNullNode(block.getPrefix(), CSharpReceiver::receiveSpace));
            block = block.withMarkers(ctx.receiveNonNullNode(block.getMarkers(), ctx::receiveMarkers));
            block = block.getPadding().withStatic(ctx.receiveNonNullNode(block.getPadding().getStatic(), rightPaddedValueReceiver(java.lang.Boolean.class)));
            block = block.getPadding().withStatements(ctx.receiveNonNullNodes(block.getPadding().getStatements(), CSharpReceiver::receiveRightPaddedTree));
            return block.withEnd(ctx.receiveNonNullNode(block.getEnd(), CSharpReceiver::receiveSpace));
        }

        @Override
        public J.Break visitBreak(J.Break break_, ReceiverContext ctx) {
            break_ = break_.withId(ctx.receiveNonNullValue(break_.getId(), UUID.class));
            break_ = break_.withPrefix(ctx.receiveNonNullNode(break_.getPrefix(), CSharpReceiver::receiveSpace));
            break_ = break_.withMarkers(ctx.receiveNonNullNode(break_.getMarkers(), ctx::receiveMarkers));
            return break_.withLabel(ctx.receiveNode(break_.getLabel(), ctx::receiveTree));
        }

        @Override
        public J.Case visitCase(J.Case case_, ReceiverContext ctx) {
            case_ = case_.withId(ctx.receiveNonNullValue(case_.getId(), UUID.class));
            case_ = case_.withPrefix(ctx.receiveNonNullNode(case_.getPrefix(), CSharpReceiver::receiveSpace));
            case_ = case_.withMarkers(ctx.receiveNonNullNode(case_.getMarkers(), ctx::receiveMarkers));
            case_ = case_.withType(ctx.receiveNonNullValue(case_.getType(), J.Case.Type.class));
            case_ = case_.getPadding().withCaseLabels(ctx.receiveNonNullNode(case_.getPadding().getCaseLabels(), CSharpReceiver::receiveContainer));
            case_ = case_.getPadding().withStatements(ctx.receiveNonNullNode(case_.getPadding().getStatements(), CSharpReceiver::receiveContainer));
            case_ = case_.getPadding().withBody(ctx.receiveNode(case_.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return case_.withGuard(ctx.receiveNode(case_.getGuard(), ctx::receiveTree));
        }

        @Override
        public J.ClassDeclaration visitClassDeclaration(J.ClassDeclaration classDeclaration, ReceiverContext ctx) {
            classDeclaration = classDeclaration.withId(ctx.receiveNonNullValue(classDeclaration.getId(), UUID.class));
            classDeclaration = classDeclaration.withPrefix(ctx.receiveNonNullNode(classDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            classDeclaration = classDeclaration.withMarkers(ctx.receiveNonNullNode(classDeclaration.getMarkers(), ctx::receiveMarkers));
            classDeclaration = classDeclaration.withLeadingAnnotations(ctx.receiveNonNullNodes(classDeclaration.getLeadingAnnotations(), ctx::receiveTree));
            classDeclaration = classDeclaration.withModifiers(ctx.receiveNonNullNodes(classDeclaration.getModifiers(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withKind(ctx.receiveNonNullNode(classDeclaration.getPadding().getKind(), CSharpReceiver::receiveClassDeclarationKind));
            classDeclaration = classDeclaration.withName(ctx.receiveNonNullNode(classDeclaration.getName(), ctx::receiveTree));
            classDeclaration = classDeclaration.getPadding().withTypeParameters(ctx.receiveNode(classDeclaration.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withPrimaryConstructor(ctx.receiveNode(classDeclaration.getPadding().getPrimaryConstructor(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withExtends(ctx.receiveNode(classDeclaration.getPadding().getExtends(), CSharpReceiver::receiveLeftPaddedTree));
            classDeclaration = classDeclaration.getPadding().withImplements(ctx.receiveNode(classDeclaration.getPadding().getImplements(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.getPadding().withPermits(ctx.receiveNode(classDeclaration.getPadding().getPermits(), CSharpReceiver::receiveContainer));
            classDeclaration = classDeclaration.withBody(ctx.receiveNonNullNode(classDeclaration.getBody(), ctx::receiveTree));
            return classDeclaration.withType(ctx.receiveValue(classDeclaration.getType(), JavaType.FullyQualified.class));
        }

        @Override
        public J.Continue visitContinue(J.Continue continue_, ReceiverContext ctx) {
            continue_ = continue_.withId(ctx.receiveNonNullValue(continue_.getId(), UUID.class));
            continue_ = continue_.withPrefix(ctx.receiveNonNullNode(continue_.getPrefix(), CSharpReceiver::receiveSpace));
            continue_ = continue_.withMarkers(ctx.receiveNonNullNode(continue_.getMarkers(), ctx::receiveMarkers));
            return continue_.withLabel(ctx.receiveNode(continue_.getLabel(), ctx::receiveTree));
        }

        @Override
        public J.DoWhileLoop visitDoWhileLoop(J.DoWhileLoop doWhileLoop, ReceiverContext ctx) {
            doWhileLoop = doWhileLoop.withId(ctx.receiveNonNullValue(doWhileLoop.getId(), UUID.class));
            doWhileLoop = doWhileLoop.withPrefix(ctx.receiveNonNullNode(doWhileLoop.getPrefix(), CSharpReceiver::receiveSpace));
            doWhileLoop = doWhileLoop.withMarkers(ctx.receiveNonNullNode(doWhileLoop.getMarkers(), ctx::receiveMarkers));
            doWhileLoop = doWhileLoop.getPadding().withBody(ctx.receiveNonNullNode(doWhileLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
            return doWhileLoop.getPadding().withWhileCondition(ctx.receiveNonNullNode(doWhileLoop.getPadding().getWhileCondition(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public J.Empty visitEmpty(J.Empty empty, ReceiverContext ctx) {
            empty = empty.withId(ctx.receiveNonNullValue(empty.getId(), UUID.class));
            empty = empty.withPrefix(ctx.receiveNonNullNode(empty.getPrefix(), CSharpReceiver::receiveSpace));
            return empty.withMarkers(ctx.receiveNonNullNode(empty.getMarkers(), ctx::receiveMarkers));
        }

        @Override
        public J.EnumValue visitEnumValue(J.EnumValue enumValue, ReceiverContext ctx) {
            enumValue = enumValue.withId(ctx.receiveNonNullValue(enumValue.getId(), UUID.class));
            enumValue = enumValue.withPrefix(ctx.receiveNonNullNode(enumValue.getPrefix(), CSharpReceiver::receiveSpace));
            enumValue = enumValue.withMarkers(ctx.receiveNonNullNode(enumValue.getMarkers(), ctx::receiveMarkers));
            enumValue = enumValue.withAnnotations(ctx.receiveNonNullNodes(enumValue.getAnnotations(), ctx::receiveTree));
            enumValue = enumValue.withName(ctx.receiveNonNullNode(enumValue.getName(), ctx::receiveTree));
            return enumValue.withInitializer(ctx.receiveNode(enumValue.getInitializer(), ctx::receiveTree));
        }

        @Override
        public J.EnumValueSet visitEnumValueSet(J.EnumValueSet enumValueSet, ReceiverContext ctx) {
            enumValueSet = enumValueSet.withId(ctx.receiveNonNullValue(enumValueSet.getId(), UUID.class));
            enumValueSet = enumValueSet.withPrefix(ctx.receiveNonNullNode(enumValueSet.getPrefix(), CSharpReceiver::receiveSpace));
            enumValueSet = enumValueSet.withMarkers(ctx.receiveNonNullNode(enumValueSet.getMarkers(), ctx::receiveMarkers));
            enumValueSet = enumValueSet.getPadding().withEnums(ctx.receiveNonNullNodes(enumValueSet.getPadding().getEnums(), CSharpReceiver::receiveRightPaddedTree));
            return enumValueSet.withTerminatedWithSemicolon(ctx.receiveNonNullValue(enumValueSet.isTerminatedWithSemicolon(), boolean.class));
        }

        @Override
        public J.FieldAccess visitFieldAccess(J.FieldAccess fieldAccess, ReceiverContext ctx) {
            fieldAccess = fieldAccess.withId(ctx.receiveNonNullValue(fieldAccess.getId(), UUID.class));
            fieldAccess = fieldAccess.withPrefix(ctx.receiveNonNullNode(fieldAccess.getPrefix(), CSharpReceiver::receiveSpace));
            fieldAccess = fieldAccess.withMarkers(ctx.receiveNonNullNode(fieldAccess.getMarkers(), ctx::receiveMarkers));
            fieldAccess = fieldAccess.withTarget(ctx.receiveNonNullNode(fieldAccess.getTarget(), ctx::receiveTree));
            fieldAccess = fieldAccess.getPadding().withName(ctx.receiveNonNullNode(fieldAccess.getPadding().getName(), CSharpReceiver::receiveLeftPaddedTree));
            return fieldAccess.withType(ctx.receiveValue(fieldAccess.getType(), JavaType.class));
        }

        @Override
        public J.ForEachLoop visitForEachLoop(J.ForEachLoop forEachLoop, ReceiverContext ctx) {
            forEachLoop = forEachLoop.withId(ctx.receiveNonNullValue(forEachLoop.getId(), UUID.class));
            forEachLoop = forEachLoop.withPrefix(ctx.receiveNonNullNode(forEachLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forEachLoop = forEachLoop.withMarkers(ctx.receiveNonNullNode(forEachLoop.getMarkers(), ctx::receiveMarkers));
            forEachLoop = forEachLoop.withControl(ctx.receiveNonNullNode(forEachLoop.getControl(), ctx::receiveTree));
            return forEachLoop.getPadding().withBody(ctx.receiveNonNullNode(forEachLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.ForEachLoop.Control visitForEachControl(J.ForEachLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withVariable(ctx.receiveNonNullNode(control.getPadding().getVariable(), CSharpReceiver::receiveRightPaddedTree));
            return control.getPadding().withIterable(ctx.receiveNonNullNode(control.getPadding().getIterable(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.ForLoop visitForLoop(J.ForLoop forLoop, ReceiverContext ctx) {
            forLoop = forLoop.withId(ctx.receiveNonNullValue(forLoop.getId(), UUID.class));
            forLoop = forLoop.withPrefix(ctx.receiveNonNullNode(forLoop.getPrefix(), CSharpReceiver::receiveSpace));
            forLoop = forLoop.withMarkers(ctx.receiveNonNullNode(forLoop.getMarkers(), ctx::receiveMarkers));
            forLoop = forLoop.withControl(ctx.receiveNonNullNode(forLoop.getControl(), ctx::receiveTree));
            return forLoop.getPadding().withBody(ctx.receiveNonNullNode(forLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.ForLoop.Control visitForControl(J.ForLoop.Control control, ReceiverContext ctx) {
            control = control.withId(ctx.receiveNonNullValue(control.getId(), UUID.class));
            control = control.withPrefix(ctx.receiveNonNullNode(control.getPrefix(), CSharpReceiver::receiveSpace));
            control = control.withMarkers(ctx.receiveNonNullNode(control.getMarkers(), ctx::receiveMarkers));
            control = control.getPadding().withInit(ctx.receiveNonNullNodes(control.getPadding().getInit(), CSharpReceiver::receiveRightPaddedTree));
            control = control.getPadding().withCondition(ctx.receiveNonNullNode(control.getPadding().getCondition(), CSharpReceiver::receiveRightPaddedTree));
            return control.getPadding().withUpdate(ctx.receiveNonNullNodes(control.getPadding().getUpdate(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.ParenthesizedTypeTree visitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, ReceiverContext ctx) {
            parenthesizedTypeTree = parenthesizedTypeTree.withId(ctx.receiveNonNullValue(parenthesizedTypeTree.getId(), UUID.class));
            parenthesizedTypeTree = parenthesizedTypeTree.withPrefix(ctx.receiveNonNullNode(parenthesizedTypeTree.getPrefix(), CSharpReceiver::receiveSpace));
            parenthesizedTypeTree = parenthesizedTypeTree.withMarkers(ctx.receiveNonNullNode(parenthesizedTypeTree.getMarkers(), ctx::receiveMarkers));
            parenthesizedTypeTree = parenthesizedTypeTree.withAnnotations(ctx.receiveNonNullNodes(parenthesizedTypeTree.getAnnotations(), ctx::receiveTree));
            return parenthesizedTypeTree.withParenthesizedType(ctx.receiveNonNullNode(parenthesizedTypeTree.getParenthesizedType(), ctx::receiveTree));
        }

        @Override
        public J.Identifier visitIdentifier(J.Identifier identifier, ReceiverContext ctx) {
            identifier = identifier.withId(ctx.receiveNonNullValue(identifier.getId(), UUID.class));
            identifier = identifier.withPrefix(ctx.receiveNonNullNode(identifier.getPrefix(), CSharpReceiver::receiveSpace));
            identifier = identifier.withMarkers(ctx.receiveNonNullNode(identifier.getMarkers(), ctx::receiveMarkers));
            identifier = identifier.withAnnotations(ctx.receiveNonNullNodes(identifier.getAnnotations(), ctx::receiveTree));
            identifier = identifier.withSimpleName(ctx.receiveNonNullValue(identifier.getSimpleName(), String.class));
            identifier = identifier.withType(ctx.receiveValue(identifier.getType(), JavaType.class));
            return identifier.withFieldType(ctx.receiveValue(identifier.getFieldType(), JavaType.Variable.class));
        }

        @Override
        public J.If visitIf(J.If if_, ReceiverContext ctx) {
            if_ = if_.withId(ctx.receiveNonNullValue(if_.getId(), UUID.class));
            if_ = if_.withPrefix(ctx.receiveNonNullNode(if_.getPrefix(), CSharpReceiver::receiveSpace));
            if_ = if_.withMarkers(ctx.receiveNonNullNode(if_.getMarkers(), ctx::receiveMarkers));
            if_ = if_.withIfCondition(ctx.receiveNonNullNode(if_.getIfCondition(), ctx::receiveTree));
            if_ = if_.getPadding().withThenPart(ctx.receiveNonNullNode(if_.getPadding().getThenPart(), CSharpReceiver::receiveRightPaddedTree));
            return if_.withElsePart(ctx.receiveNode(if_.getElsePart(), ctx::receiveTree));
        }

        @Override
        public J.If.Else visitElse(J.If.Else else_, ReceiverContext ctx) {
            else_ = else_.withId(ctx.receiveNonNullValue(else_.getId(), UUID.class));
            else_ = else_.withPrefix(ctx.receiveNonNullNode(else_.getPrefix(), CSharpReceiver::receiveSpace));
            else_ = else_.withMarkers(ctx.receiveNonNullNode(else_.getMarkers(), ctx::receiveMarkers));
            return else_.getPadding().withBody(ctx.receiveNonNullNode(else_.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.Import visitImport(J.Import import_, ReceiverContext ctx) {
            import_ = import_.withId(ctx.receiveNonNullValue(import_.getId(), UUID.class));
            import_ = import_.withPrefix(ctx.receiveNonNullNode(import_.getPrefix(), CSharpReceiver::receiveSpace));
            import_ = import_.withMarkers(ctx.receiveNonNullNode(import_.getMarkers(), ctx::receiveMarkers));
            import_ = import_.getPadding().withStatic(ctx.receiveNonNullNode(import_.getPadding().getStatic(), leftPaddedValueReceiver(java.lang.Boolean.class)));
            import_ = import_.withQualid(ctx.receiveNonNullNode(import_.getQualid(), ctx::receiveTree));
            return import_.getPadding().withAlias(ctx.receiveNode(import_.getPadding().getAlias(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public J.InstanceOf visitInstanceOf(J.InstanceOf instanceOf, ReceiverContext ctx) {
            instanceOf = instanceOf.withId(ctx.receiveNonNullValue(instanceOf.getId(), UUID.class));
            instanceOf = instanceOf.withPrefix(ctx.receiveNonNullNode(instanceOf.getPrefix(), CSharpReceiver::receiveSpace));
            instanceOf = instanceOf.withMarkers(ctx.receiveNonNullNode(instanceOf.getMarkers(), ctx::receiveMarkers));
            instanceOf = instanceOf.getPadding().withExpression(ctx.receiveNonNullNode(instanceOf.getPadding().getExpression(), CSharpReceiver::receiveRightPaddedTree));
            instanceOf = instanceOf.withClazz(ctx.receiveNonNullNode(instanceOf.getClazz(), ctx::receiveTree));
            instanceOf = instanceOf.withPattern(ctx.receiveNode(instanceOf.getPattern(), ctx::receiveTree));
            return instanceOf.withType(ctx.receiveValue(instanceOf.getType(), JavaType.class));
        }

        @Override
        public J.DeconstructionPattern visitDeconstructionPattern(J.DeconstructionPattern deconstructionPattern, ReceiverContext ctx) {
            deconstructionPattern = deconstructionPattern.withId(ctx.receiveNonNullValue(deconstructionPattern.getId(), UUID.class));
            deconstructionPattern = deconstructionPattern.withPrefix(ctx.receiveNonNullNode(deconstructionPattern.getPrefix(), CSharpReceiver::receiveSpace));
            deconstructionPattern = deconstructionPattern.withMarkers(ctx.receiveNonNullNode(deconstructionPattern.getMarkers(), ctx::receiveMarkers));
            deconstructionPattern = deconstructionPattern.withDeconstructor(ctx.receiveNonNullNode(deconstructionPattern.getDeconstructor(), ctx::receiveTree));
            deconstructionPattern = deconstructionPattern.getPadding().withNested(ctx.receiveNonNullNode(deconstructionPattern.getPadding().getNested(), CSharpReceiver::receiveContainer));
            return deconstructionPattern.withType(ctx.receiveValue(deconstructionPattern.getType(), JavaType.class));
        }

        @Override
        public J.IntersectionType visitIntersectionType(J.IntersectionType intersectionType, ReceiverContext ctx) {
            intersectionType = intersectionType.withId(ctx.receiveNonNullValue(intersectionType.getId(), UUID.class));
            intersectionType = intersectionType.withPrefix(ctx.receiveNonNullNode(intersectionType.getPrefix(), CSharpReceiver::receiveSpace));
            intersectionType = intersectionType.withMarkers(ctx.receiveNonNullNode(intersectionType.getMarkers(), ctx::receiveMarkers));
            return intersectionType.getPadding().withBounds(ctx.receiveNonNullNode(intersectionType.getPadding().getBounds(), CSharpReceiver::receiveContainer));
        }

        @Override
        public J.Label visitLabel(J.Label label, ReceiverContext ctx) {
            label = label.withId(ctx.receiveNonNullValue(label.getId(), UUID.class));
            label = label.withPrefix(ctx.receiveNonNullNode(label.getPrefix(), CSharpReceiver::receiveSpace));
            label = label.withMarkers(ctx.receiveNonNullNode(label.getMarkers(), ctx::receiveMarkers));
            label = label.getPadding().withLabel(ctx.receiveNonNullNode(label.getPadding().getLabel(), CSharpReceiver::receiveRightPaddedTree));
            return label.withStatement(ctx.receiveNonNullNode(label.getStatement(), ctx::receiveTree));
        }

        @Override
        public J.Lambda visitLambda(J.Lambda lambda, ReceiverContext ctx) {
            lambda = lambda.withId(ctx.receiveNonNullValue(lambda.getId(), UUID.class));
            lambda = lambda.withPrefix(ctx.receiveNonNullNode(lambda.getPrefix(), CSharpReceiver::receiveSpace));
            lambda = lambda.withMarkers(ctx.receiveNonNullNode(lambda.getMarkers(), ctx::receiveMarkers));
            lambda = lambda.withParameters(ctx.receiveNonNullNode(lambda.getParameters(), CSharpReceiver::receiveLambdaParameters));
            lambda = lambda.withArrow(ctx.receiveNonNullNode(lambda.getArrow(), CSharpReceiver::receiveSpace));
            lambda = lambda.withBody(ctx.receiveNonNullNode(lambda.getBody(), ctx::receiveTree));
            return lambda.withType(ctx.receiveValue(lambda.getType(), JavaType.class));
        }

        @Override
        public J.Literal visitLiteral(J.Literal literal, ReceiverContext ctx) {
            literal = literal.withId(ctx.receiveNonNullValue(literal.getId(), UUID.class));
            literal = literal.withPrefix(ctx.receiveNonNullNode(literal.getPrefix(), CSharpReceiver::receiveSpace));
            literal = literal.withMarkers(ctx.receiveNonNullNode(literal.getMarkers(), ctx::receiveMarkers));
            literal = literal.withValue(ctx.receiveValue(literal.getValue(), Object.class));
            literal = literal.withValueSource(ctx.receiveValue(literal.getValueSource(), String.class));
            literal = literal.withUnicodeEscapes(ctx.receiveValues(literal.getUnicodeEscapes(), J.Literal.UnicodeEscape.class));
            return literal.withType(ctx.receiveValue(literal.getType(), JavaType.Primitive.class));
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
            return memberReference.withVariableType(ctx.receiveValue(memberReference.getVariableType(), JavaType.Variable.class));
        }

        @Override
        public J.MethodDeclaration visitMethodDeclaration(J.MethodDeclaration methodDeclaration, ReceiverContext ctx) {
            methodDeclaration = methodDeclaration.withId(ctx.receiveNonNullValue(methodDeclaration.getId(), UUID.class));
            methodDeclaration = methodDeclaration.withPrefix(ctx.receiveNonNullNode(methodDeclaration.getPrefix(), CSharpReceiver::receiveSpace));
            methodDeclaration = methodDeclaration.withMarkers(ctx.receiveNonNullNode(methodDeclaration.getMarkers(), ctx::receiveMarkers));
            methodDeclaration = methodDeclaration.withLeadingAnnotations(ctx.receiveNonNullNodes(methodDeclaration.getLeadingAnnotations(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.withModifiers(ctx.receiveNonNullNodes(methodDeclaration.getModifiers(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getAnnotations().withTypeParameters(ctx.receiveNode(methodDeclaration.getAnnotations().getTypeParameters(), CSharpReceiver::receiveMethodTypeParameters));
            methodDeclaration = methodDeclaration.withReturnTypeExpression(ctx.receiveNode(methodDeclaration.getReturnTypeExpression(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getAnnotations().withName(ctx.receiveNonNullNode(methodDeclaration.getAnnotations().getName(), CSharpReceiver::receiveMethodIdentifierWithAnnotations));
            methodDeclaration = methodDeclaration.getPadding().withParameters(ctx.receiveNonNullNode(methodDeclaration.getPadding().getParameters(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.getPadding().withThrows(ctx.receiveNode(methodDeclaration.getPadding().getThrows(), CSharpReceiver::receiveContainer));
            methodDeclaration = methodDeclaration.withBody(ctx.receiveNode(methodDeclaration.getBody(), ctx::receiveTree));
            methodDeclaration = methodDeclaration.getPadding().withDefaultValue(ctx.receiveNode(methodDeclaration.getPadding().getDefaultValue(), CSharpReceiver::receiveLeftPaddedTree));
            return methodDeclaration.withMethodType(ctx.receiveValue(methodDeclaration.getMethodType(), JavaType.Method.class));
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
            return methodInvocation.withMethodType(ctx.receiveValue(methodInvocation.getMethodType(), JavaType.Method.class));
        }

        @Override
        public J.Modifier visitModifier(J.Modifier modifier, ReceiverContext ctx) {
            modifier = modifier.withId(ctx.receiveNonNullValue(modifier.getId(), UUID.class));
            modifier = modifier.withPrefix(ctx.receiveNonNullNode(modifier.getPrefix(), CSharpReceiver::receiveSpace));
            modifier = modifier.withMarkers(ctx.receiveNonNullNode(modifier.getMarkers(), ctx::receiveMarkers));
            modifier = modifier.withKeyword(ctx.receiveValue(modifier.getKeyword(), String.class));
            modifier = modifier.withType(ctx.receiveNonNullValue(modifier.getType(), J.Modifier.Type.class));
            return modifier.withAnnotations(ctx.receiveNonNullNodes(modifier.getAnnotations(), ctx::receiveTree));
        }

        @Override
        public J.MultiCatch visitMultiCatch(J.MultiCatch multiCatch, ReceiverContext ctx) {
            multiCatch = multiCatch.withId(ctx.receiveNonNullValue(multiCatch.getId(), UUID.class));
            multiCatch = multiCatch.withPrefix(ctx.receiveNonNullNode(multiCatch.getPrefix(), CSharpReceiver::receiveSpace));
            multiCatch = multiCatch.withMarkers(ctx.receiveNonNullNode(multiCatch.getMarkers(), ctx::receiveMarkers));
            return multiCatch.getPadding().withAlternatives(ctx.receiveNonNullNodes(multiCatch.getPadding().getAlternatives(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.NewArray visitNewArray(J.NewArray newArray, ReceiverContext ctx) {
            newArray = newArray.withId(ctx.receiveNonNullValue(newArray.getId(), UUID.class));
            newArray = newArray.withPrefix(ctx.receiveNonNullNode(newArray.getPrefix(), CSharpReceiver::receiveSpace));
            newArray = newArray.withMarkers(ctx.receiveNonNullNode(newArray.getMarkers(), ctx::receiveMarkers));
            newArray = newArray.withTypeExpression(ctx.receiveNode(newArray.getTypeExpression(), ctx::receiveTree));
            newArray = newArray.withDimensions(ctx.receiveNonNullNodes(newArray.getDimensions(), ctx::receiveTree));
            newArray = newArray.getPadding().withInitializer(ctx.receiveNode(newArray.getPadding().getInitializer(), CSharpReceiver::receiveContainer));
            return newArray.withType(ctx.receiveValue(newArray.getType(), JavaType.class));
        }

        @Override
        public J.ArrayDimension visitArrayDimension(J.ArrayDimension arrayDimension, ReceiverContext ctx) {
            arrayDimension = arrayDimension.withId(ctx.receiveNonNullValue(arrayDimension.getId(), UUID.class));
            arrayDimension = arrayDimension.withPrefix(ctx.receiveNonNullNode(arrayDimension.getPrefix(), CSharpReceiver::receiveSpace));
            arrayDimension = arrayDimension.withMarkers(ctx.receiveNonNullNode(arrayDimension.getMarkers(), ctx::receiveMarkers));
            return arrayDimension.getPadding().withIndex(ctx.receiveNonNullNode(arrayDimension.getPadding().getIndex(), CSharpReceiver::receiveRightPaddedTree));
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
            return newClass.withConstructorType(ctx.receiveValue(newClass.getConstructorType(), JavaType.Method.class));
        }

        @Override
        public J.NullableType visitNullableType(J.NullableType nullableType, ReceiverContext ctx) {
            nullableType = nullableType.withId(ctx.receiveNonNullValue(nullableType.getId(), UUID.class));
            nullableType = nullableType.withPrefix(ctx.receiveNonNullNode(nullableType.getPrefix(), CSharpReceiver::receiveSpace));
            nullableType = nullableType.withMarkers(ctx.receiveNonNullNode(nullableType.getMarkers(), ctx::receiveMarkers));
            nullableType = nullableType.withAnnotations(ctx.receiveNonNullNodes(nullableType.getAnnotations(), ctx::receiveTree));
            return nullableType.getPadding().withTypeTree(ctx.receiveNonNullNode(nullableType.getPadding().getTypeTree(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.Package visitPackage(J.Package package_, ReceiverContext ctx) {
            package_ = package_.withId(ctx.receiveNonNullValue(package_.getId(), UUID.class));
            package_ = package_.withPrefix(ctx.receiveNonNullNode(package_.getPrefix(), CSharpReceiver::receiveSpace));
            package_ = package_.withMarkers(ctx.receiveNonNullNode(package_.getMarkers(), ctx::receiveMarkers));
            package_ = package_.withExpression(ctx.receiveNonNullNode(package_.getExpression(), ctx::receiveTree));
            return package_.withAnnotations(ctx.receiveNonNullNodes(package_.getAnnotations(), ctx::receiveTree));
        }

        @Override
        public J.ParameterizedType visitParameterizedType(J.ParameterizedType parameterizedType, ReceiverContext ctx) {
            parameterizedType = parameterizedType.withId(ctx.receiveNonNullValue(parameterizedType.getId(), UUID.class));
            parameterizedType = parameterizedType.withPrefix(ctx.receiveNonNullNode(parameterizedType.getPrefix(), CSharpReceiver::receiveSpace));
            parameterizedType = parameterizedType.withMarkers(ctx.receiveNonNullNode(parameterizedType.getMarkers(), ctx::receiveMarkers));
            parameterizedType = parameterizedType.withClazz(ctx.receiveNonNullNode(parameterizedType.getClazz(), ctx::receiveTree));
            parameterizedType = parameterizedType.getPadding().withTypeParameters(ctx.receiveNode(parameterizedType.getPadding().getTypeParameters(), CSharpReceiver::receiveContainer));
            return parameterizedType.withType(ctx.receiveValue(parameterizedType.getType(), JavaType.class));
        }

        @Override
        public <J2 extends J> J.Parentheses<J2> visitParentheses(J.Parentheses<J2> parentheses, ReceiverContext ctx) {
            parentheses = parentheses.withId(ctx.receiveNonNullValue(parentheses.getId(), UUID.class));
            parentheses = parentheses.withPrefix(ctx.receiveNonNullNode(parentheses.getPrefix(), CSharpReceiver::receiveSpace));
            parentheses = parentheses.withMarkers(ctx.receiveNonNullNode(parentheses.getMarkers(), ctx::receiveMarkers));
            return parentheses.getPadding().withTree(ctx.receiveNonNullNode(parentheses.getPadding().getTree(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public <J2 extends J> J.ControlParentheses<J2> visitControlParentheses(J.ControlParentheses<J2> controlParentheses, ReceiverContext ctx) {
            controlParentheses = controlParentheses.withId(ctx.receiveNonNullValue(controlParentheses.getId(), UUID.class));
            controlParentheses = controlParentheses.withPrefix(ctx.receiveNonNullNode(controlParentheses.getPrefix(), CSharpReceiver::receiveSpace));
            controlParentheses = controlParentheses.withMarkers(ctx.receiveNonNullNode(controlParentheses.getMarkers(), ctx::receiveMarkers));
            return controlParentheses.getPadding().withTree(ctx.receiveNonNullNode(controlParentheses.getPadding().getTree(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.Primitive visitPrimitive(J.Primitive primitive, ReceiverContext ctx) {
            primitive = primitive.withId(ctx.receiveNonNullValue(primitive.getId(), UUID.class));
            primitive = primitive.withPrefix(ctx.receiveNonNullNode(primitive.getPrefix(), CSharpReceiver::receiveSpace));
            primitive = primitive.withMarkers(ctx.receiveNonNullNode(primitive.getMarkers(), ctx::receiveMarkers));
            return primitive.withType(ctx.receiveValue(primitive.getType(), JavaType.Primitive.class));
        }

        @Override
        public J.Return visitReturn(J.Return return_, ReceiverContext ctx) {
            return_ = return_.withId(ctx.receiveNonNullValue(return_.getId(), UUID.class));
            return_ = return_.withPrefix(ctx.receiveNonNullNode(return_.getPrefix(), CSharpReceiver::receiveSpace));
            return_ = return_.withMarkers(ctx.receiveNonNullNode(return_.getMarkers(), ctx::receiveMarkers));
            return return_.withExpression(ctx.receiveNode(return_.getExpression(), ctx::receiveTree));
        }

        @Override
        public J.Switch visitSwitch(J.Switch switch_, ReceiverContext ctx) {
            switch_ = switch_.withId(ctx.receiveNonNullValue(switch_.getId(), UUID.class));
            switch_ = switch_.withPrefix(ctx.receiveNonNullNode(switch_.getPrefix(), CSharpReceiver::receiveSpace));
            switch_ = switch_.withMarkers(ctx.receiveNonNullNode(switch_.getMarkers(), ctx::receiveMarkers));
            switch_ = switch_.withSelector(ctx.receiveNonNullNode(switch_.getSelector(), ctx::receiveTree));
            return switch_.withCases(ctx.receiveNonNullNode(switch_.getCases(), ctx::receiveTree));
        }

        @Override
        public J.SwitchExpression visitSwitchExpression(J.SwitchExpression switchExpression, ReceiverContext ctx) {
            switchExpression = switchExpression.withId(ctx.receiveNonNullValue(switchExpression.getId(), UUID.class));
            switchExpression = switchExpression.withPrefix(ctx.receiveNonNullNode(switchExpression.getPrefix(), CSharpReceiver::receiveSpace));
            switchExpression = switchExpression.withMarkers(ctx.receiveNonNullNode(switchExpression.getMarkers(), ctx::receiveMarkers));
            switchExpression = switchExpression.withSelector(ctx.receiveNonNullNode(switchExpression.getSelector(), ctx::receiveTree));
            switchExpression = switchExpression.withCases(ctx.receiveNonNullNode(switchExpression.getCases(), ctx::receiveTree));
            return switchExpression.withType(ctx.receiveValue(switchExpression.getType(), JavaType.class));
        }

        @Override
        public J.Synchronized visitSynchronized(J.Synchronized synchronized_, ReceiverContext ctx) {
            synchronized_ = synchronized_.withId(ctx.receiveNonNullValue(synchronized_.getId(), UUID.class));
            synchronized_ = synchronized_.withPrefix(ctx.receiveNonNullNode(synchronized_.getPrefix(), CSharpReceiver::receiveSpace));
            synchronized_ = synchronized_.withMarkers(ctx.receiveNonNullNode(synchronized_.getMarkers(), ctx::receiveMarkers));
            synchronized_ = synchronized_.withLock(ctx.receiveNonNullNode(synchronized_.getLock(), ctx::receiveTree));
            return synchronized_.withBody(ctx.receiveNonNullNode(synchronized_.getBody(), ctx::receiveTree));
        }

        @Override
        public J.Ternary visitTernary(J.Ternary ternary, ReceiverContext ctx) {
            ternary = ternary.withId(ctx.receiveNonNullValue(ternary.getId(), UUID.class));
            ternary = ternary.withPrefix(ctx.receiveNonNullNode(ternary.getPrefix(), CSharpReceiver::receiveSpace));
            ternary = ternary.withMarkers(ctx.receiveNonNullNode(ternary.getMarkers(), ctx::receiveMarkers));
            ternary = ternary.withCondition(ctx.receiveNonNullNode(ternary.getCondition(), ctx::receiveTree));
            ternary = ternary.getPadding().withTruePart(ctx.receiveNonNullNode(ternary.getPadding().getTruePart(), CSharpReceiver::receiveLeftPaddedTree));
            ternary = ternary.getPadding().withFalsePart(ctx.receiveNonNullNode(ternary.getPadding().getFalsePart(), CSharpReceiver::receiveLeftPaddedTree));
            return ternary.withType(ctx.receiveValue(ternary.getType(), JavaType.class));
        }

        @Override
        public J.Throw visitThrow(J.Throw throw_, ReceiverContext ctx) {
            throw_ = throw_.withId(ctx.receiveNonNullValue(throw_.getId(), UUID.class));
            throw_ = throw_.withPrefix(ctx.receiveNonNullNode(throw_.getPrefix(), CSharpReceiver::receiveSpace));
            throw_ = throw_.withMarkers(ctx.receiveNonNullNode(throw_.getMarkers(), ctx::receiveMarkers));
            return throw_.withException(ctx.receiveNonNullNode(throw_.getException(), ctx::receiveTree));
        }

        @Override
        public J.Try visitTry(J.Try try_, ReceiverContext ctx) {
            try_ = try_.withId(ctx.receiveNonNullValue(try_.getId(), UUID.class));
            try_ = try_.withPrefix(ctx.receiveNonNullNode(try_.getPrefix(), CSharpReceiver::receiveSpace));
            try_ = try_.withMarkers(ctx.receiveNonNullNode(try_.getMarkers(), ctx::receiveMarkers));
            try_ = try_.getPadding().withResources(ctx.receiveNode(try_.getPadding().getResources(), CSharpReceiver::receiveContainer));
            try_ = try_.withBody(ctx.receiveNonNullNode(try_.getBody(), ctx::receiveTree));
            try_ = try_.withCatches(ctx.receiveNonNullNodes(try_.getCatches(), ctx::receiveTree));
            return try_.getPadding().withFinally(ctx.receiveNode(try_.getPadding().getFinally(), CSharpReceiver::receiveLeftPaddedTree));
        }

        @Override
        public J.Try.Resource visitTryResource(J.Try.Resource resource, ReceiverContext ctx) {
            resource = resource.withId(ctx.receiveNonNullValue(resource.getId(), UUID.class));
            resource = resource.withPrefix(ctx.receiveNonNullNode(resource.getPrefix(), CSharpReceiver::receiveSpace));
            resource = resource.withMarkers(ctx.receiveNonNullNode(resource.getMarkers(), ctx::receiveMarkers));
            resource = resource.withVariableDeclarations(ctx.receiveNonNullNode(resource.getVariableDeclarations(), ctx::receiveTree));
            return resource.withTerminatedWithSemicolon(ctx.receiveNonNullValue(resource.isTerminatedWithSemicolon(), boolean.class));
        }

        @Override
        public J.Try.Catch visitCatch(J.Try.Catch catch_, ReceiverContext ctx) {
            catch_ = catch_.withId(ctx.receiveNonNullValue(catch_.getId(), UUID.class));
            catch_ = catch_.withPrefix(ctx.receiveNonNullNode(catch_.getPrefix(), CSharpReceiver::receiveSpace));
            catch_ = catch_.withMarkers(ctx.receiveNonNullNode(catch_.getMarkers(), ctx::receiveMarkers));
            catch_ = catch_.withParameter(ctx.receiveNonNullNode(catch_.getParameter(), ctx::receiveTree));
            return catch_.withBody(ctx.receiveNonNullNode(catch_.getBody(), ctx::receiveTree));
        }

        @Override
        public J.TypeCast visitTypeCast(J.TypeCast typeCast, ReceiverContext ctx) {
            typeCast = typeCast.withId(ctx.receiveNonNullValue(typeCast.getId(), UUID.class));
            typeCast = typeCast.withPrefix(ctx.receiveNonNullNode(typeCast.getPrefix(), CSharpReceiver::receiveSpace));
            typeCast = typeCast.withMarkers(ctx.receiveNonNullNode(typeCast.getMarkers(), ctx::receiveMarkers));
            typeCast = typeCast.withClazz(ctx.receiveNonNullNode(typeCast.getClazz(), ctx::receiveTree));
            return typeCast.withExpression(ctx.receiveNonNullNode(typeCast.getExpression(), ctx::receiveTree));
        }

        @Override
        public J.TypeParameter visitTypeParameter(J.TypeParameter typeParameter, ReceiverContext ctx) {
            typeParameter = typeParameter.withId(ctx.receiveNonNullValue(typeParameter.getId(), UUID.class));
            typeParameter = typeParameter.withPrefix(ctx.receiveNonNullNode(typeParameter.getPrefix(), CSharpReceiver::receiveSpace));
            typeParameter = typeParameter.withMarkers(ctx.receiveNonNullNode(typeParameter.getMarkers(), ctx::receiveMarkers));
            typeParameter = typeParameter.withAnnotations(ctx.receiveNonNullNodes(typeParameter.getAnnotations(), ctx::receiveTree));
            typeParameter = typeParameter.withModifiers(ctx.receiveNonNullNodes(typeParameter.getModifiers(), ctx::receiveTree));
            typeParameter = typeParameter.withName(ctx.receiveNonNullNode(typeParameter.getName(), ctx::receiveTree));
            return typeParameter.getPadding().withBounds(ctx.receiveNode(typeParameter.getPadding().getBounds(), CSharpReceiver::receiveContainer));
        }

        @Override
        public J.Unary visitUnary(J.Unary unary, ReceiverContext ctx) {
            unary = unary.withId(ctx.receiveNonNullValue(unary.getId(), UUID.class));
            unary = unary.withPrefix(ctx.receiveNonNullNode(unary.getPrefix(), CSharpReceiver::receiveSpace));
            unary = unary.withMarkers(ctx.receiveNonNullNode(unary.getMarkers(), ctx::receiveMarkers));
            unary = unary.getPadding().withOperator(ctx.receiveNonNullNode(unary.getPadding().getOperator(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Unary.Type.class)));
            unary = unary.withExpression(ctx.receiveNonNullNode(unary.getExpression(), ctx::receiveTree));
            return unary.withType(ctx.receiveValue(unary.getType(), JavaType.class));
        }

        @Override
        public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations variableDeclarations, ReceiverContext ctx) {
            variableDeclarations = variableDeclarations.withId(ctx.receiveNonNullValue(variableDeclarations.getId(), UUID.class));
            variableDeclarations = variableDeclarations.withPrefix(ctx.receiveNonNullNode(variableDeclarations.getPrefix(), CSharpReceiver::receiveSpace));
            variableDeclarations = variableDeclarations.withMarkers(ctx.receiveNonNullNode(variableDeclarations.getMarkers(), ctx::receiveMarkers));
            variableDeclarations = variableDeclarations.withLeadingAnnotations(ctx.receiveNonNullNodes(variableDeclarations.getLeadingAnnotations(), ctx::receiveTree));
            variableDeclarations = variableDeclarations.withModifiers(ctx.receiveNonNullNodes(variableDeclarations.getModifiers(), ctx::receiveTree));
            variableDeclarations = variableDeclarations.withTypeExpression(ctx.receiveNode(variableDeclarations.getTypeExpression(), ctx::receiveTree));
            variableDeclarations = variableDeclarations.withVarargs(ctx.receiveNode(variableDeclarations.getVarargs(), CSharpReceiver::receiveSpace));
            variableDeclarations = variableDeclarations.withDimensionsBeforeName(ctx.receiveNonNullNodes(variableDeclarations.getDimensionsBeforeName(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            return variableDeclarations.getPadding().withVariables(ctx.receiveNonNullNodes(variableDeclarations.getPadding().getVariables(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.VariableDeclarations.NamedVariable visitVariable(J.VariableDeclarations.NamedVariable namedVariable, ReceiverContext ctx) {
            namedVariable = namedVariable.withId(ctx.receiveNonNullValue(namedVariable.getId(), UUID.class));
            namedVariable = namedVariable.withPrefix(ctx.receiveNonNullNode(namedVariable.getPrefix(), CSharpReceiver::receiveSpace));
            namedVariable = namedVariable.withMarkers(ctx.receiveNonNullNode(namedVariable.getMarkers(), ctx::receiveMarkers));
            namedVariable = namedVariable.withName(ctx.receiveNonNullNode(namedVariable.getName(), ctx::receiveTree));
            namedVariable = namedVariable.withDimensionsAfterName(ctx.receiveNonNullNodes(namedVariable.getDimensionsAfterName(), leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)));
            namedVariable = namedVariable.getPadding().withInitializer(ctx.receiveNode(namedVariable.getPadding().getInitializer(), CSharpReceiver::receiveLeftPaddedTree));
            return namedVariable.withVariableType(ctx.receiveValue(namedVariable.getVariableType(), JavaType.Variable.class));
        }

        @Override
        public J.WhileLoop visitWhileLoop(J.WhileLoop whileLoop, ReceiverContext ctx) {
            whileLoop = whileLoop.withId(ctx.receiveNonNullValue(whileLoop.getId(), UUID.class));
            whileLoop = whileLoop.withPrefix(ctx.receiveNonNullNode(whileLoop.getPrefix(), CSharpReceiver::receiveSpace));
            whileLoop = whileLoop.withMarkers(ctx.receiveNonNullNode(whileLoop.getMarkers(), ctx::receiveMarkers));
            whileLoop = whileLoop.withCondition(ctx.receiveNonNullNode(whileLoop.getCondition(), ctx::receiveTree));
            return whileLoop.getPadding().withBody(ctx.receiveNonNullNode(whileLoop.getPadding().getBody(), CSharpReceiver::receiveRightPaddedTree));
        }

        @Override
        public J.Wildcard visitWildcard(J.Wildcard wildcard, ReceiverContext ctx) {
            wildcard = wildcard.withId(ctx.receiveNonNullValue(wildcard.getId(), UUID.class));
            wildcard = wildcard.withPrefix(ctx.receiveNonNullNode(wildcard.getPrefix(), CSharpReceiver::receiveSpace));
            wildcard = wildcard.withMarkers(ctx.receiveNonNullNode(wildcard.getMarkers(), ctx::receiveMarkers));
            wildcard = wildcard.getPadding().withBound(ctx.receiveNode(wildcard.getPadding().getBound(), leftPaddedValueReceiver(org.openrewrite.java.tree.J.Wildcard.Bound.class)));
            return wildcard.withBoundedType(ctx.receiveNode(wildcard.getBoundedType(), ctx::receiveTree));
        }

        @Override
        public J.Yield visitYield(J.Yield yield, ReceiverContext ctx) {
            yield = yield.withId(ctx.receiveNonNullValue(yield.getId(), UUID.class));
            yield = yield.withPrefix(ctx.receiveNonNullNode(yield.getPrefix(), CSharpReceiver::receiveSpace));
            yield = yield.withMarkers(ctx.receiveNonNullNode(yield.getMarkers(), ctx::receiveMarkers));
            yield = yield.withImplicit(ctx.receiveNonNullValue(yield.isImplicit(), boolean.class));
            return yield.withValue(ctx.receiveNonNullNode(yield.getValue(), ctx::receiveTree));
        }

        @Override
        public J.Unknown visitUnknown(J.Unknown unknown, ReceiverContext ctx) {
            unknown = unknown.withId(ctx.receiveNonNullValue(unknown.getId(), UUID.class));
            unknown = unknown.withPrefix(ctx.receiveNonNullNode(unknown.getPrefix(), CSharpReceiver::receiveSpace));
            unknown = unknown.withMarkers(ctx.receiveNonNullNode(unknown.getMarkers(), ctx::receiveMarkers));
            return unknown.withSource(ctx.receiveNonNullNode(unknown.getSource(), ctx::receiveTree));
        }

        @Override
        public J.Unknown.Source visitUnknownSource(J.Unknown.Source source, ReceiverContext ctx) {
            source = source.withId(ctx.receiveNonNullValue(source.getId(), UUID.class));
            source = source.withPrefix(ctx.receiveNonNullNode(source.getPrefix(), CSharpReceiver::receiveSpace));
            source = source.withMarkers(ctx.receiveNonNullNode(source.getMarkers(), ctx::receiveMarkers));
            return source.withText(ctx.receiveNonNullValue(source.getText(), String.class));
        }

        @Override
        public J.Erroneous visitErroneous(J.Erroneous erroneous, ReceiverContext ctx) {
            erroneous = erroneous.withId(ctx.receiveNonNullValue(erroneous.getId(), UUID.class));
            erroneous = erroneous.withPrefix(ctx.receiveNonNullNode(erroneous.getPrefix(), CSharpReceiver::receiveSpace));
            erroneous = erroneous.withMarkers(ctx.receiveNonNullNode(erroneous.getMarkers(), ctx::receiveMarkers));
            return erroneous.withText(ctx.receiveNonNullValue(erroneous.getText(), String.class));
        }

    }

    private static class Factory implements ReceiverFactory {

        private final ClassValue<Function<ReceiverContext, Object>> factories = new ClassValue<Function<ReceiverContext, Object>>() {
            @Override
            protected Function<ReceiverContext, Object> computeValue(Class type) {
                if (type == Cs.CompilationUnit.class) return Factory::createCsCompilationUnit;
                if (type == Cs.OperatorDeclaration.class) return Factory::createCsOperatorDeclaration;
                if (type == Cs.RefExpression.class) return Factory::createCsRefExpression;
                if (type == Cs.PointerType.class) return Factory::createCsPointerType;
                if (type == Cs.RefType.class) return Factory::createCsRefType;
                if (type == Cs.ForEachVariableLoop.class) return Factory::createCsForEachVariableLoop;
                if (type == Cs.ForEachVariableLoop.Control.class) return Factory::createCsForEachVariableLoopControl;
                if (type == Cs.NameColon.class) return Factory::createCsNameColon;
                if (type == Cs.Argument.class) return Factory::createCsArgument;
                if (type == Cs.AnnotatedStatement.class) return Factory::createCsAnnotatedStatement;
                if (type == Cs.ArrayRankSpecifier.class) return Factory::createCsArrayRankSpecifier;
                if (type == Cs.AssignmentOperation.class) return Factory::createCsAssignmentOperation;
                if (type == Cs.AttributeList.class) return Factory::createCsAttributeList;
                if (type == Cs.AwaitExpression.class) return Factory::createCsAwaitExpression;
                if (type == Cs.StackAllocExpression.class) return Factory::createCsStackAllocExpression;
                if (type == Cs.GotoStatement.class) return Factory::createCsGotoStatement;
                if (type == Cs.EventDeclaration.class) return Factory::createCsEventDeclaration;
                if (type == Cs.Binary.class) return Factory::createCsBinary;
                if (type == Cs.BlockScopeNamespaceDeclaration.class) return Factory::createCsBlockScopeNamespaceDeclaration;
                if (type == Cs.CollectionExpression.class) return Factory::createCsCollectionExpression;
                if (type == Cs.ExpressionStatement.class) return Factory::createCsExpressionStatement;
                if (type == Cs.ExternAlias.class) return Factory::createCsExternAlias;
                if (type == Cs.FileScopeNamespaceDeclaration.class) return Factory::createCsFileScopeNamespaceDeclaration;
                if (type == Cs.InterpolatedString.class) return Factory::createCsInterpolatedString;
                if (type == Cs.Interpolation.class) return Factory::createCsInterpolation;
                if (type == Cs.NullSafeExpression.class) return Factory::createCsNullSafeExpression;
                if (type == Cs.StatementExpression.class) return Factory::createCsStatementExpression;
                if (type == Cs.UsingDirective.class) return Factory::createCsUsingDirective;
                if (type == Cs.PropertyDeclaration.class) return Factory::createCsPropertyDeclaration;
                if (type == Cs.Keyword.class) return Factory::createCsKeyword;
                if (type == Cs.Lambda.class) return Factory::createCsLambda;
                if (type == Cs.ClassDeclaration.class) return Factory::createCsClassDeclaration;
                if (type == Cs.MethodDeclaration.class) return Factory::createCsMethodDeclaration;
                if (type == Cs.UsingStatement.class) return Factory::createCsUsingStatement;
                if (type == Cs.TypeParameterConstraintClause.class) return Factory::createCsTypeParameterConstraintClause;
                if (type == Cs.TypeConstraint.class) return Factory::createCsTypeConstraint;
                if (type == Cs.AllowsConstraintClause.class) return Factory::createCsAllowsConstraintClause;
                if (type == Cs.RefStructConstraint.class) return Factory::createCsRefStructConstraint;
                if (type == Cs.ClassOrStructConstraint.class) return Factory::createCsClassOrStructConstraint;
                if (type == Cs.ConstructorConstraint.class) return Factory::createCsConstructorConstraint;
                if (type == Cs.DefaultConstraint.class) return Factory::createCsDefaultConstraint;
                if (type == Cs.DeclarationExpression.class) return Factory::createCsDeclarationExpression;
                if (type == Cs.SingleVariableDesignation.class) return Factory::createCsSingleVariableDesignation;
                if (type == Cs.ParenthesizedVariableDesignation.class) return Factory::createCsParenthesizedVariableDesignation;
                if (type == Cs.DiscardVariableDesignation.class) return Factory::createCsDiscardVariableDesignation;
                if (type == Cs.TupleExpression.class) return Factory::createCsTupleExpression;
                if (type == Cs.Constructor.class) return Factory::createCsConstructor;
                if (type == Cs.DestructorDeclaration.class) return Factory::createCsDestructorDeclaration;
                if (type == Cs.Unary.class) return Factory::createCsUnary;
                if (type == Cs.ConstructorInitializer.class) return Factory::createCsConstructorInitializer;
                if (type == Cs.TupleType.class) return Factory::createCsTupleType;
                if (type == Cs.TupleElement.class) return Factory::createCsTupleElement;
                if (type == Cs.NewClass.class) return Factory::createCsNewClass;
                if (type == Cs.InitializerExpression.class) return Factory::createCsInitializerExpression;
                if (type == Cs.ImplicitElementAccess.class) return Factory::createCsImplicitElementAccess;
                if (type == Cs.Yield.class) return Factory::createCsYield;
                if (type == Cs.DefaultExpression.class) return Factory::createCsDefaultExpression;
                if (type == Cs.IsPattern.class) return Factory::createCsIsPattern;
                if (type == Cs.UnaryPattern.class) return Factory::createCsUnaryPattern;
                if (type == Cs.TypePattern.class) return Factory::createCsTypePattern;
                if (type == Cs.BinaryPattern.class) return Factory::createCsBinaryPattern;
                if (type == Cs.ConstantPattern.class) return Factory::createCsConstantPattern;
                if (type == Cs.DiscardPattern.class) return Factory::createCsDiscardPattern;
                if (type == Cs.ListPattern.class) return Factory::createCsListPattern;
                if (type == Cs.ParenthesizedPattern.class) return Factory::createCsParenthesizedPattern;
                if (type == Cs.RecursivePattern.class) return Factory::createCsRecursivePattern;
                if (type == Cs.VarPattern.class) return Factory::createCsVarPattern;
                if (type == Cs.PositionalPatternClause.class) return Factory::createCsPositionalPatternClause;
                if (type == Cs.RelationalPattern.class) return Factory::createCsRelationalPattern;
                if (type == Cs.SlicePattern.class) return Factory::createCsSlicePattern;
                if (type == Cs.PropertyPatternClause.class) return Factory::createCsPropertyPatternClause;
                if (type == Cs.Subpattern.class) return Factory::createCsSubpattern;
                if (type == Cs.SwitchExpression.class) return Factory::createCsSwitchExpression;
                if (type == Cs.SwitchExpressionArm.class) return Factory::createCsSwitchExpressionArm;
                if (type == Cs.SwitchSection.class) return Factory::createCsSwitchSection;
                if (type == Cs.DefaultSwitchLabel.class) return Factory::createCsDefaultSwitchLabel;
                if (type == Cs.CasePatternSwitchLabel.class) return Factory::createCsCasePatternSwitchLabel;
                if (type == Cs.SwitchStatement.class) return Factory::createCsSwitchStatement;
                if (type == Cs.LockStatement.class) return Factory::createCsLockStatement;
                if (type == Cs.FixedStatement.class) return Factory::createCsFixedStatement;
                if (type == Cs.CheckedExpression.class) return Factory::createCsCheckedExpression;
                if (type == Cs.CheckedStatement.class) return Factory::createCsCheckedStatement;
                if (type == Cs.UnsafeStatement.class) return Factory::createCsUnsafeStatement;
                if (type == Cs.RangeExpression.class) return Factory::createCsRangeExpression;
                if (type == Cs.QueryExpression.class) return Factory::createCsQueryExpression;
                if (type == Cs.QueryBody.class) return Factory::createCsQueryBody;
                if (type == Cs.FromClause.class) return Factory::createCsFromClause;
                if (type == Cs.LetClause.class) return Factory::createCsLetClause;
                if (type == Cs.JoinClause.class) return Factory::createCsJoinClause;
                if (type == Cs.JoinIntoClause.class) return Factory::createCsJoinIntoClause;
                if (type == Cs.WhereClause.class) return Factory::createCsWhereClause;
                if (type == Cs.OrderByClause.class) return Factory::createCsOrderByClause;
                if (type == Cs.QueryContinuation.class) return Factory::createCsQueryContinuation;
                if (type == Cs.Ordering.class) return Factory::createCsOrdering;
                if (type == Cs.SelectClause.class) return Factory::createCsSelectClause;
                if (type == Cs.GroupClause.class) return Factory::createCsGroupClause;
                if (type == Cs.IndexerDeclaration.class) return Factory::createCsIndexerDeclaration;
                if (type == Cs.DelegateDeclaration.class) return Factory::createCsDelegateDeclaration;
                if (type == Cs.ConversionOperatorDeclaration.class) return Factory::createCsConversionOperatorDeclaration;
                if (type == Cs.TypeParameter.class) return Factory::createCsTypeParameter;
                if (type == Cs.EnumDeclaration.class) return Factory::createCsEnumDeclaration;
                if (type == Cs.EnumMemberDeclaration.class) return Factory::createCsEnumMemberDeclaration;
                if (type == Cs.AliasQualifiedName.class) return Factory::createCsAliasQualifiedName;
                if (type == Cs.ArrayType.class) return Factory::createCsArrayType;
                if (type == Cs.Try.class) return Factory::createCsTry;
                if (type == Cs.Try.Catch.class) return Factory::createCsTryCatch;
                if (type == Cs.ArrowExpressionClause.class) return Factory::createCsArrowExpressionClause;
                if (type == Cs.AccessorDeclaration.class) return Factory::createCsAccessorDeclaration;
                if (type == Cs.PointerFieldAccess.class) return Factory::createCsPointerFieldAccess;
                if (type == J.AnnotatedType.class) return Factory::createJAnnotatedType;
                if (type == J.Annotation.class) return Factory::createJAnnotation;
                if (type == J.ArrayAccess.class) return Factory::createJArrayAccess;
                if (type == J.ArrayType.class) return Factory::createJArrayType;
                if (type == J.Assert.class) return Factory::createJAssert;
                if (type == J.Assignment.class) return Factory::createJAssignment;
                if (type == J.AssignmentOperation.class) return Factory::createJAssignmentOperation;
                if (type == J.Binary.class) return Factory::createJBinary;
                if (type == J.Block.class) return Factory::createJBlock;
                if (type == J.Break.class) return Factory::createJBreak;
                if (type == J.Case.class) return Factory::createJCase;
                if (type == J.ClassDeclaration.class) return Factory::createJClassDeclaration;
                if (type == J.ClassDeclaration.Kind.class) return Factory::createJClassDeclarationKind;
                if (type == J.Continue.class) return Factory::createJContinue;
                if (type == J.DoWhileLoop.class) return Factory::createJDoWhileLoop;
                if (type == J.Empty.class) return Factory::createJEmpty;
                if (type == J.EnumValue.class) return Factory::createJEnumValue;
                if (type == J.EnumValueSet.class) return Factory::createJEnumValueSet;
                if (type == J.FieldAccess.class) return Factory::createJFieldAccess;
                if (type == J.ForEachLoop.class) return Factory::createJForEachLoop;
                if (type == J.ForEachLoop.Control.class) return Factory::createJForEachLoopControl;
                if (type == J.ForLoop.class) return Factory::createJForLoop;
                if (type == J.ForLoop.Control.class) return Factory::createJForLoopControl;
                if (type == J.ParenthesizedTypeTree.class) return Factory::createJParenthesizedTypeTree;
                if (type == J.Identifier.class) return Factory::createJIdentifier;
                if (type == J.If.class) return Factory::createJIf;
                if (type == J.If.Else.class) return Factory::createJIfElse;
                if (type == J.Import.class) return Factory::createJImport;
                if (type == J.InstanceOf.class) return Factory::createJInstanceOf;
                if (type == J.DeconstructionPattern.class) return Factory::createJDeconstructionPattern;
                if (type == J.IntersectionType.class) return Factory::createJIntersectionType;
                if (type == J.Label.class) return Factory::createJLabel;
                if (type == J.Lambda.class) return Factory::createJLambda;
                if (type == J.Lambda.Parameters.class) return Factory::createJLambdaParameters;
                if (type == J.Literal.class) return Factory::createJLiteral;
                if (type == J.MemberReference.class) return Factory::createJMemberReference;
                if (type == J.MethodDeclaration.class) return Factory::createJMethodDeclaration;
                if (type == J.MethodInvocation.class) return Factory::createJMethodInvocation;
                if (type == J.Modifier.class) return Factory::createJModifier;
                if (type == J.MultiCatch.class) return Factory::createJMultiCatch;
                if (type == J.NewArray.class) return Factory::createJNewArray;
                if (type == J.ArrayDimension.class) return Factory::createJArrayDimension;
                if (type == J.NewClass.class) return Factory::createJNewClass;
                if (type == J.NullableType.class) return Factory::createJNullableType;
                if (type == J.Package.class) return Factory::createJPackage;
                if (type == J.ParameterizedType.class) return Factory::createJParameterizedType;
                if (type == J.Parentheses.class) return Factory::createJParentheses;
                if (type == J.ControlParentheses.class) return Factory::createJControlParentheses;
                if (type == J.Primitive.class) return Factory::createJPrimitive;
                if (type == J.Return.class) return Factory::createJReturn;
                if (type == J.Switch.class) return Factory::createJSwitch;
                if (type == J.SwitchExpression.class) return Factory::createJSwitchExpression;
                if (type == J.Synchronized.class) return Factory::createJSynchronized;
                if (type == J.Ternary.class) return Factory::createJTernary;
                if (type == J.Throw.class) return Factory::createJThrow;
                if (type == J.Try.class) return Factory::createJTry;
                if (type == J.Try.Resource.class) return Factory::createJTryResource;
                if (type == J.Try.Catch.class) return Factory::createJTryCatch;
                if (type == J.TypeCast.class) return Factory::createJTypeCast;
                if (type == J.TypeParameter.class) return Factory::createJTypeParameter;
                if (type == J.TypeParameters.class) return Factory::createJTypeParameters;
                if (type == J.Unary.class) return Factory::createJUnary;
                if (type == J.VariableDeclarations.class) return Factory::createJVariableDeclarations;
                if (type == J.VariableDeclarations.NamedVariable.class) return Factory::createJVariableDeclarationsNamedVariable;
                if (type == J.WhileLoop.class) return Factory::createJWhileLoop;
                if (type == J.Wildcard.class) return Factory::createJWildcard;
                if (type == J.Yield.class) return Factory::createJYield;
                if (type == J.Unknown.class) return Factory::createJUnknown;
                if (type == J.Unknown.Source.class) return Factory::createJUnknownSource;
                if (type == J.Erroneous.class) return Factory::createJErroneous;
                throw new IllegalArgumentException("Unknown type: " + type);
            }
        };

        @Override
        @SuppressWarnings("unchecked")
        public <T> T create(Class<T> type, ReceiverContext ctx) {
            return (T) factories.get(type).apply(ctx);
        }

        private static Cs.CompilationUnit createCsCompilationUnit(ReceiverContext ctx) {
            return new Cs.CompilationUnit(
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

        private static Cs.OperatorDeclaration createCsOperatorDeclaration(ReceiverContext ctx) {
            return new Cs.OperatorDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.OperatorDeclaration.Operator.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.Method.class)
            );
        }

        private static Cs.RefExpression createCsRefExpression(ReceiverContext ctx) {
            return new Cs.RefExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.PointerType createCsPointerType(ReceiverContext ctx) {
            return new Cs.PointerType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.RefType createCsRefType(ReceiverContext ctx) {
            return new Cs.RefType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.ForEachVariableLoop createCsForEachVariableLoop(ReceiverContext ctx) {
            return new Cs.ForEachVariableLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.ForEachVariableLoop.Control createCsForEachVariableLoopControl(ReceiverContext ctx) {
            return new Cs.ForEachVariableLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.NameColon createCsNameColon(ReceiverContext ctx) {
            return new Cs.NameColon(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.Argument createCsArgument(ReceiverContext ctx) {
            return new Cs.Argument(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.AnnotatedStatement createCsAnnotatedStatement(ReceiverContext ctx) {
            return new Cs.AnnotatedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ArrayRankSpecifier createCsArrayRankSpecifier(ReceiverContext ctx) {
            return new Cs.ArrayRankSpecifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.AssignmentOperation createCsAssignmentOperation(ReceiverContext ctx) {
            return new Cs.AssignmentOperation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AssignmentOperation.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.AttributeList createCsAttributeList(ReceiverContext ctx) {
            return new Cs.AttributeList(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.AwaitExpression createCsAwaitExpression(ReceiverContext ctx) {
            return new Cs.AwaitExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.StackAllocExpression createCsStackAllocExpression(ReceiverContext ctx) {
            return new Cs.StackAllocExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.GotoStatement createCsGotoStatement(ReceiverContext ctx) {
            return new Cs.GotoStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.EventDeclaration createCsEventDeclaration(ReceiverContext ctx) {
            return new Cs.EventDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.Binary createCsBinary(ReceiverContext ctx) {
            return new Cs.Binary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Binary.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.BlockScopeNamespaceDeclaration createCsBlockScopeNamespaceDeclaration(ReceiverContext ctx) {
            return new Cs.BlockScopeNamespaceDeclaration(
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

        private static Cs.CollectionExpression createCsCollectionExpression(ReceiverContext ctx) {
            return new Cs.CollectionExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.ExpressionStatement createCsExpressionStatement(ReceiverContext ctx) {
            return new Cs.ExpressionStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.ExternAlias createCsExternAlias(ReceiverContext ctx) {
            return new Cs.ExternAlias(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.FileScopeNamespaceDeclaration createCsFileScopeNamespaceDeclaration(ReceiverContext ctx) {
            return new Cs.FileScopeNamespaceDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.InterpolatedString createCsInterpolatedString(ReceiverContext ctx) {
            return new Cs.InterpolatedString(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, String.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullValue(null, String.class)
            );
        }

        private static Cs.Interpolation createCsInterpolation(ReceiverContext ctx) {
            return new Cs.Interpolation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.NullSafeExpression createCsNullSafeExpression(ReceiverContext ctx) {
            return new Cs.NullSafeExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.StatementExpression createCsStatementExpression(ReceiverContext ctx) {
            return new Cs.StatementExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.UsingDirective createCsUsingDirective(ReceiverContext ctx) {
            return new Cs.UsingDirective(
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

        private static Cs.PropertyDeclaration createCsPropertyDeclaration(ReceiverContext ctx) {
            return new Cs.PropertyDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.Keyword createCsKeyword(ReceiverContext ctx) {
            return new Cs.Keyword(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, Cs.Keyword.KeywordKind.class)
            );
        }

        private static Cs.Lambda createCsLambda(ReceiverContext ctx) {
            return new Cs.Lambda(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
            );
        }

        private static Cs.ClassDeclaration createCsClassDeclaration(ReceiverContext ctx) {
            return new Cs.ClassDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveClassDeclarationKind),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.FullyQualified.class)
            );
        }

        private static Cs.MethodDeclaration createCsMethodDeclaration(ReceiverContext ctx) {
            return new Cs.MethodDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.Method.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.UsingStatement createCsUsingStatement(ReceiverContext ctx) {
            return new Cs.UsingStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.TypeParameterConstraintClause createCsTypeParameterConstraintClause(ReceiverContext ctx) {
            return new Cs.TypeParameterConstraintClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.TypeConstraint createCsTypeConstraint(ReceiverContext ctx) {
            return new Cs.TypeConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.AllowsConstraintClause createCsAllowsConstraintClause(ReceiverContext ctx) {
            return new Cs.AllowsConstraintClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.RefStructConstraint createCsRefStructConstraint(ReceiverContext ctx) {
            return new Cs.RefStructConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
            );
        }

        private static Cs.ClassOrStructConstraint createCsClassOrStructConstraint(ReceiverContext ctx) {
            return new Cs.ClassOrStructConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, Cs.ClassOrStructConstraint.TypeKind.class)
            );
        }

        private static Cs.ConstructorConstraint createCsConstructorConstraint(ReceiverContext ctx) {
            return new Cs.ConstructorConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
            );
        }

        private static Cs.DefaultConstraint createCsDefaultConstraint(ReceiverContext ctx) {
            return new Cs.DefaultConstraint(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
            );
        }

        private static Cs.DeclarationExpression createCsDeclarationExpression(ReceiverContext ctx) {
            return new Cs.DeclarationExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.SingleVariableDesignation createCsSingleVariableDesignation(ReceiverContext ctx) {
            return new Cs.SingleVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ParenthesizedVariableDesignation createCsParenthesizedVariableDesignation(ReceiverContext ctx) {
            return new Cs.ParenthesizedVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.DiscardVariableDesignation createCsDiscardVariableDesignation(ReceiverContext ctx) {
            return new Cs.DiscardVariableDesignation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.TupleExpression createCsTupleExpression(ReceiverContext ctx) {
            return new Cs.TupleExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.Constructor createCsConstructor(ReceiverContext ctx) {
            return new Cs.Constructor(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.DestructorDeclaration createCsDestructorDeclaration(ReceiverContext ctx) {
            return new Cs.DestructorDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.Unary createCsUnary(ReceiverContext ctx) {
            return new Cs.Unary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.Unary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.ConstructorInitializer createCsConstructorInitializer(ReceiverContext ctx) {
            return new Cs.ConstructorInitializer(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.TupleType createCsTupleType(ReceiverContext ctx) {
            return new Cs.TupleType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.TupleElement createCsTupleElement(ReceiverContext ctx) {
            return new Cs.TupleElement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.NewClass createCsNewClass(ReceiverContext ctx) {
            return new Cs.NewClass(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.InitializerExpression createCsInitializerExpression(ReceiverContext ctx) {
            return new Cs.InitializerExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.ImplicitElementAccess createCsImplicitElementAccess(ReceiverContext ctx) {
            return new Cs.ImplicitElementAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.Yield createCsYield(ReceiverContext ctx) {
            return new Cs.Yield(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.DefaultExpression createCsDefaultExpression(ReceiverContext ctx) {
            return new Cs.DefaultExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.IsPattern createCsIsPattern(ReceiverContext ctx) {
            return new Cs.IsPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.UnaryPattern createCsUnaryPattern(ReceiverContext ctx) {
            return new Cs.UnaryPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.TypePattern createCsTypePattern(ReceiverContext ctx) {
            return new Cs.TypePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.BinaryPattern createCsBinaryPattern(ReceiverContext ctx) {
            return new Cs.BinaryPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.BinaryPattern.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ConstantPattern createCsConstantPattern(ReceiverContext ctx) {
            return new Cs.ConstantPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.DiscardPattern createCsDiscardPattern(ReceiverContext ctx) {
            return new Cs.DiscardPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.ListPattern createCsListPattern(ReceiverContext ctx) {
            return new Cs.ListPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ParenthesizedPattern createCsParenthesizedPattern(ReceiverContext ctx) {
            return new Cs.ParenthesizedPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.RecursivePattern createCsRecursivePattern(ReceiverContext ctx) {
            return new Cs.RecursivePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.VarPattern createCsVarPattern(ReceiverContext ctx) {
            return new Cs.VarPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.PositionalPatternClause createCsPositionalPatternClause(ReceiverContext ctx) {
            return new Cs.PositionalPatternClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.RelationalPattern createCsRelationalPattern(ReceiverContext ctx) {
            return new Cs.RelationalPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.RelationalPattern.OperatorType.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.SlicePattern createCsSlicePattern(ReceiverContext ctx) {
            return new Cs.SlicePattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
            );
        }

        private static Cs.PropertyPatternClause createCsPropertyPatternClause(ReceiverContext ctx) {
            return new Cs.PropertyPatternClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.Subpattern createCsSubpattern(ReceiverContext ctx) {
            return new Cs.Subpattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.SwitchExpression createCsSwitchExpression(ReceiverContext ctx) {
            return new Cs.SwitchExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.SwitchExpressionArm createCsSwitchExpressionArm(ReceiverContext ctx) {
            return new Cs.SwitchExpressionArm(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.SwitchSection createCsSwitchSection(ReceiverContext ctx) {
            return new Cs.SwitchSection(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.DefaultSwitchLabel createCsDefaultSwitchLabel(ReceiverContext ctx) {
            return new Cs.DefaultSwitchLabel(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
            );
        }

        private static Cs.CasePatternSwitchLabel createCsCasePatternSwitchLabel(ReceiverContext ctx) {
            return new Cs.CasePatternSwitchLabel(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
            );
        }

        private static Cs.SwitchStatement createCsSwitchStatement(ReceiverContext ctx) {
            return new Cs.SwitchStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.LockStatement createCsLockStatement(ReceiverContext ctx) {
            return new Cs.LockStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.FixedStatement createCsFixedStatement(ReceiverContext ctx) {
            return new Cs.FixedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.CheckedExpression createCsCheckedExpression(ReceiverContext ctx) {
            return new Cs.CheckedExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.CheckedStatement createCsCheckedStatement(ReceiverContext ctx) {
            return new Cs.CheckedStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.UnsafeStatement createCsUnsafeStatement(ReceiverContext ctx) {
            return new Cs.UnsafeStatement(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.RangeExpression createCsRangeExpression(ReceiverContext ctx) {
            return new Cs.RangeExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.QueryExpression createCsQueryExpression(ReceiverContext ctx) {
            return new Cs.QueryExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.QueryBody createCsQueryBody(ReceiverContext ctx) {
            return new Cs.QueryBody(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.FromClause createCsFromClause(ReceiverContext ctx) {
            return new Cs.FromClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.LetClause createCsLetClause(ReceiverContext ctx) {
            return new Cs.LetClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.JoinClause createCsJoinClause(ReceiverContext ctx) {
            return new Cs.JoinClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.JoinIntoClause createCsJoinIntoClause(ReceiverContext ctx) {
            return new Cs.JoinIntoClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.WhereClause createCsWhereClause(ReceiverContext ctx) {
            return new Cs.WhereClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.OrderByClause createCsOrderByClause(ReceiverContext ctx) {
            return new Cs.OrderByClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.QueryContinuation createCsQueryContinuation(ReceiverContext ctx) {
            return new Cs.QueryContinuation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.Ordering createCsOrdering(ReceiverContext ctx) {
            return new Cs.Ordering(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveValue(null, Cs.Ordering.DirectionKind.class)
            );
        }

        private static Cs.SelectClause createCsSelectClause(ReceiverContext ctx) {
            return new Cs.SelectClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.GroupClause createCsGroupClause(ReceiverContext ctx) {
            return new Cs.GroupClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.IndexerDeclaration createCsIndexerDeclaration(ReceiverContext ctx) {
            return new Cs.IndexerDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.DelegateDeclaration createCsDelegateDeclaration(ReceiverContext ctx) {
            return new Cs.DelegateDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.ConversionOperatorDeclaration createCsConversionOperatorDeclaration(ReceiverContext ctx) {
            return new Cs.ConversionOperatorDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.ConversionOperatorDeclaration.ExplicitImplicit.class)),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.TypeParameter createCsTypeParameter(ReceiverContext ctx) {
            return new Cs.TypeParameter(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.TypeParameter.VarianceKind.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.EnumDeclaration createCsEnumDeclaration(ReceiverContext ctx) {
            return new Cs.EnumDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static Cs.EnumMemberDeclaration createCsEnumMemberDeclaration(ReceiverContext ctx) {
            return new Cs.EnumMemberDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.AliasQualifiedName createCsAliasQualifiedName(ReceiverContext ctx) {
            return new Cs.AliasQualifiedName(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ArrayType createCsArrayType(ReceiverContext ctx) {
            return new Cs.ArrayType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static Cs.Try createCsTry(ReceiverContext ctx) {
            return new Cs.Try(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static Cs.Try.Catch createCsTryCatch(ReceiverContext ctx) {
            return new Cs.Try.Catch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static Cs.ArrowExpressionClause createCsArrowExpressionClause(ReceiverContext ctx) {
            return new Cs.ArrowExpressionClause(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static Cs.AccessorDeclaration createCsAccessorDeclaration(ReceiverContext ctx) {
            return new Cs.AccessorDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.csharp.tree.Cs.AccessorDeclaration.AccessorKinds.class)),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static Cs.PointerFieldAccess createCsPointerFieldAccess(ReceiverContext ctx) {
            return new Cs.PointerFieldAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.AnnotatedType createJAnnotatedType(ReceiverContext ctx) {
            return new J.AnnotatedType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Annotation createJAnnotation(ReceiverContext ctx) {
            return new J.Annotation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static J.ArrayAccess createJArrayAccess(ReceiverContext ctx) {
            return new J.ArrayAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.ArrayType createJArrayType(ReceiverContext ctx) {
            return new J.ArrayType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Assert createJAssert(ReceiverContext ctx) {
            return new J.Assert(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static J.Assignment createJAssignment(ReceiverContext ctx) {
            return new J.Assignment(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.AssignmentOperation createJAssignmentOperation(ReceiverContext ctx) {
            return new J.AssignmentOperation(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.AssignmentOperation.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Binary createJBinary(ReceiverContext ctx) {
            return new J.Binary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Binary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Block createJBlock(ReceiverContext ctx) {
            return new J.Block(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, rightPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace)
            );
        }

        private static J.Break createJBreak(ReceiverContext ctx) {
            return new J.Break(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.Case createJCase(ReceiverContext ctx) {
            return new J.Case(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, J.Case.Type.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.ClassDeclaration createJClassDeclaration(ReceiverContext ctx) {
            return new J.ClassDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
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

        private static J.ClassDeclaration.Kind createJClassDeclarationKind(ReceiverContext ctx) {
            return new J.ClassDeclaration.Kind(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, J.ClassDeclaration.Kind.Type.class)
            );
        }

        private static J.Continue createJContinue(ReceiverContext ctx) {
            return new J.Continue(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.DoWhileLoop createJDoWhileLoop(ReceiverContext ctx) {
            return new J.DoWhileLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static J.Empty createJEmpty(ReceiverContext ctx) {
            return new J.Empty(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers)
            );
        }

        private static J.EnumValue createJEnumValue(ReceiverContext ctx) {
            return new J.EnumValue(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.EnumValueSet createJEnumValueSet(ReceiverContext ctx) {
            return new J.EnumValueSet(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullValue(null, boolean.class)
            );
        }

        private static J.FieldAccess createJFieldAccess(ReceiverContext ctx) {
            return new J.FieldAccess(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.ForEachLoop createJForEachLoop(ReceiverContext ctx) {
            return new J.ForEachLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.ForEachLoop.Control createJForEachLoopControl(ReceiverContext ctx) {
            return new J.ForEachLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.ForLoop createJForLoop(ReceiverContext ctx) {
            return new J.ForLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.ForLoop.Control createJForLoopControl(ReceiverContext ctx) {
            return new J.ForLoop.Control(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.ParenthesizedTypeTree createJParenthesizedTypeTree(ReceiverContext ctx) {
            return new J.ParenthesizedTypeTree(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Identifier createJIdentifier(ReceiverContext ctx) {
            return new J.Identifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, String.class),
                    ctx.receiveValue(null, JavaType.class),
                    ctx.receiveValue(null, JavaType.Variable.class)
            );
        }

        private static J.If createJIf(ReceiverContext ctx) {
            return new J.If(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.If.Else createJIfElse(ReceiverContext ctx) {
            return new J.If.Else(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Import createJImport(ReceiverContext ctx) {
            return new J.Import(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(java.lang.Boolean.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static J.InstanceOf createJInstanceOf(ReceiverContext ctx) {
            return new J.InstanceOf(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.DeconstructionPattern createJDeconstructionPattern(ReceiverContext ctx) {
            return new J.DeconstructionPattern(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.IntersectionType createJIntersectionType(ReceiverContext ctx) {
            return new J.IntersectionType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static J.Label createJLabel(ReceiverContext ctx) {
            return new J.Label(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Lambda createJLambda(ReceiverContext ctx) {
            return new J.Lambda(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLambdaParameters),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Lambda.Parameters createJLambdaParameters(ReceiverContext ctx) {
            return new J.Lambda.Parameters(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Literal createJLiteral(ReceiverContext ctx) {
            return new J.Literal(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, Object.class),
                    ctx.receiveValue(null, String.class),
                    ctx.receiveValues(null, J.Literal.UnicodeEscape.class),
                    ctx.receiveValue(null, JavaType.Primitive.class)
            );
        }

        private static J.MemberReference createJMemberReference(ReceiverContext ctx) {
            return new J.MemberReference(
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

        private static J.MethodDeclaration createJMethodDeclaration(ReceiverContext ctx) {
            return new J.MethodDeclaration(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
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

        private static J.MethodInvocation createJMethodInvocation(ReceiverContext ctx) {
            return new J.MethodInvocation(
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

        private static J.Modifier createJModifier(ReceiverContext ctx) {
            return new J.Modifier(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, String.class),
                    ctx.receiveNonNullValue(null, J.Modifier.Type.class),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
            );
        }

        private static J.MultiCatch createJMultiCatch(ReceiverContext ctx) {
            return new J.MultiCatch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.NewArray createJNewArray(ReceiverContext ctx) {
            return new J.NewArray(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.ArrayDimension createJArrayDimension(ReceiverContext ctx) {
            return new J.ArrayDimension(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.NewClass createJNewClass(ReceiverContext ctx) {
            return new J.NewClass(
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

        private static J.NullableType createJNullableType(ReceiverContext ctx) {
            return new J.NullableType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Package createJPackage(ReceiverContext ctx) {
            return new J.Package(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree)
            );
        }

        private static J.ParameterizedType createJParameterizedType(ReceiverContext ctx) {
            return new J.ParameterizedType(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Parentheses createJParentheses(ReceiverContext ctx) {
            return new J.Parentheses(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.ControlParentheses createJControlParentheses(ReceiverContext ctx) {
            return new J.ControlParentheses(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Primitive createJPrimitive(ReceiverContext ctx) {
            return new J.Primitive(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveValue(null, JavaType.Primitive.class)
            );
        }

        private static J.Return createJReturn(ReceiverContext ctx) {
            return new J.Return(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.Switch createJSwitch(ReceiverContext ctx) {
            return new J.Switch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.SwitchExpression createJSwitchExpression(ReceiverContext ctx) {
            return new J.SwitchExpression(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Synchronized createJSynchronized(ReceiverContext ctx) {
            return new J.Synchronized(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Ternary createJTernary(ReceiverContext ctx) {
            return new J.Ternary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.Throw createJThrow(ReceiverContext ctx) {
            return new J.Throw(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Try createJTry(ReceiverContext ctx) {
            return new J.Try(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree)
            );
        }

        private static J.Try.Resource createJTryResource(ReceiverContext ctx) {
            return new J.Try.Resource(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullValue(null, boolean.class)
            );
        }

        private static J.Try.Catch createJTryCatch(ReceiverContext ctx) {
            return new J.Try.Catch(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.TypeCast createJTypeCast(ReceiverContext ctx) {
            return new J.TypeCast(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.TypeParameter createJTypeParameter(ReceiverContext ctx) {
            return new J.TypeParameter(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveContainer)
            );
        }

        private static J.TypeParameters createJTypeParameters(ReceiverContext ctx) {
            return new J.TypeParameters(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Unary createJUnary(ReceiverContext ctx) {
            return new J.Unary(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Unary.Type.class)),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveValue(null, JavaType.class)
            );
        }

        private static J.VariableDeclarations createJVariableDeclarations(ReceiverContext ctx) {
            return new J.VariableDeclarations(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, ctx::receiveTree),
                    ctx.receiveNode(null, ctx::receiveTree),
                    ctx.receiveNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNodes(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveNonNullNodes(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.VariableDeclarations.NamedVariable createJVariableDeclarationsNamedVariable(ReceiverContext ctx) {
            return new J.VariableDeclarations.NamedVariable(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNodes(null, leftPaddedNodeReceiver(org.openrewrite.java.tree.Space.class)),
                    ctx.receiveNode(null, CSharpReceiver::receiveLeftPaddedTree),
                    ctx.receiveValue(null, JavaType.Variable.class)
            );
        }

        private static J.WhileLoop createJWhileLoop(ReceiverContext ctx) {
            return new J.WhileLoop(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveRightPaddedTree)
            );
        }

        private static J.Wildcard createJWildcard(ReceiverContext ctx) {
            return new J.Wildcard(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNode(null, leftPaddedValueReceiver(org.openrewrite.java.tree.J.Wildcard.Bound.class)),
                    ctx.receiveNode(null, ctx::receiveTree)
            );
        }

        private static J.Yield createJYield(ReceiverContext ctx) {
            return new J.Yield(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, boolean.class),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Unknown createJUnknown(ReceiverContext ctx) {
            return new J.Unknown(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullNode(null, ctx::receiveTree)
            );
        }

        private static J.Unknown.Source createJUnknownSource(ReceiverContext ctx) {
            return new J.Unknown.Source(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, String.class)
            );
        }

        private static J.Erroneous createJErroneous(ReceiverContext ctx) {
            return new J.Erroneous(
                    ctx.receiveNonNullValue(null, UUID.class),
                    ctx.receiveNonNullNode(null, CSharpReceiver::receiveSpace),
                    ctx.receiveNonNullNode(null, ctx::receiveMarkers),
                    ctx.receiveNonNullValue(null, String.class)
            );
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

}
