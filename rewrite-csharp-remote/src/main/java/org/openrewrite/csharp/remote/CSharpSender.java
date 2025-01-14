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
import org.openrewrite.Cursor;
import org.openrewrite.Tree;
import org.openrewrite.csharp.CSharpVisitor;
import org.openrewrite.csharp.tree.*;
import org.openrewrite.java.*;
import org.openrewrite.java.tree.*;
import org.openrewrite.remote.Sender;
import org.openrewrite.remote.SenderContext;

import java.util.function.Function;

@Value
public class CSharpSender implements Sender<Cs> {

    @Override
    public void send(Cs after, @Nullable Cs before, SenderContext ctx) {
        Visitor visitor = new Visitor();
        visitor.visit(after, ctx.fork(visitor, before));
        ctx.flush();
    }

    private static class Visitor extends CSharpVisitor<SenderContext> {

        @Override
        public @Nullable J visit(@Nullable Tree tree, SenderContext ctx) {
            setCursor(new Cursor(getCursor(), tree));
            ctx.sendNode(tree, Function.identity(), ctx::sendTree);
            setCursor(getCursor().getParent());

            return (J) tree;
        }

        @Override
        public Cs.CompilationUnit visitCompilationUnit(Cs.CompilationUnit compilationUnit, SenderContext ctx) {
            ctx.sendValue(compilationUnit, Cs.CompilationUnit::getId);
            ctx.sendNode(compilationUnit, Cs.CompilationUnit::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(compilationUnit, Cs.CompilationUnit::getMarkers, ctx::sendMarkers);
            ctx.sendValue(compilationUnit, Cs.CompilationUnit::getSourcePath);
            ctx.sendTypedValue(compilationUnit, Cs.CompilationUnit::getFileAttributes);
            ctx.sendValue(compilationUnit, e -> e.getCharset() != null ? e.getCharset().name() : "UTF-8");
            ctx.sendValue(compilationUnit, Cs.CompilationUnit::isCharsetBomMarked);
            ctx.sendTypedValue(compilationUnit, Cs.CompilationUnit::getChecksum);
            ctx.sendNodes(compilationUnit, e -> e.getPadding().getExterns(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(compilationUnit, e -> e.getPadding().getUsings(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(compilationUnit, Cs.CompilationUnit::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNodes(compilationUnit, e -> e.getPadding().getMembers(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNode(compilationUnit, Cs.CompilationUnit::getEof, CSharpSender::sendSpace);
            return compilationUnit;
        }

        @Override
        public Cs.RefExpression visitRefExpression(Cs.RefExpression refExpression, SenderContext ctx) {
            ctx.sendValue(refExpression, Cs.RefExpression::getId);
            ctx.sendNode(refExpression, Cs.RefExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(refExpression, Cs.RefExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(refExpression, Cs.RefExpression::getExpression, ctx::sendTree);
            return refExpression;
        }

        @Override
        public Cs.PointerType visitPointerType(Cs.PointerType pointerType, SenderContext ctx) {
            ctx.sendValue(pointerType, Cs.PointerType::getId);
            ctx.sendNode(pointerType, Cs.PointerType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(pointerType, Cs.PointerType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(pointerType, e -> e.getPadding().getElementType(), CSharpSender::sendRightPadded);
            return pointerType;
        }

        @Override
        public Cs.RefType visitRefType(Cs.RefType refType, SenderContext ctx) {
            ctx.sendValue(refType, Cs.RefType::getId);
            ctx.sendNode(refType, Cs.RefType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(refType, Cs.RefType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(refType, Cs.RefType::getReadonlyKeyword, ctx::sendTree);
            ctx.sendNode(refType, Cs.RefType::getTypeIdentifier, ctx::sendTree);
            ctx.sendTypedValue(refType, Cs.RefType::getType);
            return refType;
        }

        @Override
        public Cs.ForEachVariableLoop visitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, SenderContext ctx) {
            ctx.sendValue(forEachVariableLoop, Cs.ForEachVariableLoop::getId);
            ctx.sendNode(forEachVariableLoop, Cs.ForEachVariableLoop::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(forEachVariableLoop, Cs.ForEachVariableLoop::getMarkers, ctx::sendMarkers);
            ctx.sendNode(forEachVariableLoop, Cs.ForEachVariableLoop::getControlElement, ctx::sendTree);
            ctx.sendNode(forEachVariableLoop, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return forEachVariableLoop;
        }

        @Override
        public Cs.ForEachVariableLoop.Control visitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, SenderContext ctx) {
            ctx.sendValue(control, Cs.ForEachVariableLoop.Control::getId);
            ctx.sendNode(control, Cs.ForEachVariableLoop.Control::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(control, Cs.ForEachVariableLoop.Control::getMarkers, ctx::sendMarkers);
            ctx.sendNode(control, e -> e.getPadding().getVariable(), CSharpSender::sendRightPadded);
            ctx.sendNode(control, e -> e.getPadding().getIterable(), CSharpSender::sendRightPadded);
            return control;
        }

        @Override
        public Cs.Argument visitArgument(Cs.Argument argument, SenderContext ctx) {
            ctx.sendValue(argument, Cs.Argument::getId);
            ctx.sendNode(argument, Cs.Argument::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(argument, Cs.Argument::getMarkers, ctx::sendMarkers);
            ctx.sendNode(argument, e -> e.getPadding().getNameColumn(), CSharpSender::sendRightPadded);
            ctx.sendNode(argument, Cs.Argument::getRefKindKeyword, ctx::sendTree);
            ctx.sendNode(argument, Cs.Argument::getExpression, ctx::sendTree);
            return argument;
        }

        @Override
        public Cs.AnnotatedStatement visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, SenderContext ctx) {
            ctx.sendValue(annotatedStatement, Cs.AnnotatedStatement::getId);
            ctx.sendNode(annotatedStatement, Cs.AnnotatedStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(annotatedStatement, Cs.AnnotatedStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(annotatedStatement, Cs.AnnotatedStatement::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNode(annotatedStatement, Cs.AnnotatedStatement::getStatement, ctx::sendTree);
            return annotatedStatement;
        }

        @Override
        public Cs.ArrayRankSpecifier visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, SenderContext ctx) {
            ctx.sendValue(arrayRankSpecifier, Cs.ArrayRankSpecifier::getId);
            ctx.sendNode(arrayRankSpecifier, Cs.ArrayRankSpecifier::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrayRankSpecifier, Cs.ArrayRankSpecifier::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrayRankSpecifier, e -> e.getPadding().getSizes(), CSharpSender::sendContainer);
            return arrayRankSpecifier;
        }

        @Override
        public Cs.AssignmentOperation visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, SenderContext ctx) {
            ctx.sendValue(assignmentOperation, Cs.AssignmentOperation::getId);
            ctx.sendNode(assignmentOperation, Cs.AssignmentOperation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(assignmentOperation, Cs.AssignmentOperation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(assignmentOperation, Cs.AssignmentOperation::getVariable, ctx::sendTree);
            ctx.sendNode(assignmentOperation, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(assignmentOperation, Cs.AssignmentOperation::getAssignment, ctx::sendTree);
            ctx.sendTypedValue(assignmentOperation, Cs.AssignmentOperation::getType);
            return assignmentOperation;
        }

        @Override
        public Cs.AttributeList visitAttributeList(Cs.AttributeList attributeList, SenderContext ctx) {
            ctx.sendValue(attributeList, Cs.AttributeList::getId);
            ctx.sendNode(attributeList, Cs.AttributeList::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(attributeList, Cs.AttributeList::getMarkers, ctx::sendMarkers);
            ctx.sendNode(attributeList, e -> e.getPadding().getTarget(), CSharpSender::sendRightPadded);
            ctx.sendNodes(attributeList, e -> e.getPadding().getAttributes(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return attributeList;
        }

        @Override
        public Cs.AwaitExpression visitAwaitExpression(Cs.AwaitExpression awaitExpression, SenderContext ctx) {
            ctx.sendValue(awaitExpression, Cs.AwaitExpression::getId);
            ctx.sendNode(awaitExpression, Cs.AwaitExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(awaitExpression, Cs.AwaitExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(awaitExpression, Cs.AwaitExpression::getExpression, ctx::sendTree);
            ctx.sendTypedValue(awaitExpression, Cs.AwaitExpression::getType);
            return awaitExpression;
        }

        @Override
        public Cs.StackAllocExpression visitStackAllocExpression(Cs.StackAllocExpression stackAllocExpression, SenderContext ctx) {
            ctx.sendValue(stackAllocExpression, Cs.StackAllocExpression::getId);
            ctx.sendNode(stackAllocExpression, Cs.StackAllocExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(stackAllocExpression, Cs.StackAllocExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(stackAllocExpression, Cs.StackAllocExpression::getExpression, ctx::sendTree);
            return stackAllocExpression;
        }

        @Override
        public Cs.GotoStatement visitGotoStatement(Cs.GotoStatement gotoStatement, SenderContext ctx) {
            ctx.sendValue(gotoStatement, Cs.GotoStatement::getId);
            ctx.sendNode(gotoStatement, Cs.GotoStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(gotoStatement, Cs.GotoStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(gotoStatement, Cs.GotoStatement::getCaseOrDefaultKeyword, ctx::sendTree);
            ctx.sendNode(gotoStatement, Cs.GotoStatement::getTarget, ctx::sendTree);
            return gotoStatement;
        }

        @Override
        public Cs.EventDeclaration visitEventDeclaration(Cs.EventDeclaration eventDeclaration, SenderContext ctx) {
            ctx.sendValue(eventDeclaration, Cs.EventDeclaration::getId);
            ctx.sendNode(eventDeclaration, Cs.EventDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(eventDeclaration, Cs.EventDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(eventDeclaration, Cs.EventDeclaration::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNodes(eventDeclaration, Cs.EventDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(eventDeclaration, e -> e.getPadding().getTypeExpression(), CSharpSender::sendLeftPadded);
            ctx.sendNode(eventDeclaration, e -> e.getPadding().getInterfaceSpecifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(eventDeclaration, Cs.EventDeclaration::getName, ctx::sendTree);
            ctx.sendNode(eventDeclaration, e -> e.getPadding().getAccessors(), CSharpSender::sendContainer);
            return eventDeclaration;
        }

        @Override
        public Cs.Binary visitBinary(Cs.Binary binary, SenderContext ctx) {
            ctx.sendValue(binary, Cs.Binary::getId);
            ctx.sendNode(binary, Cs.Binary::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(binary, Cs.Binary::getMarkers, ctx::sendMarkers);
            ctx.sendNode(binary, Cs.Binary::getLeft, ctx::sendTree);
            ctx.sendNode(binary, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(binary, Cs.Binary::getRight, ctx::sendTree);
            ctx.sendTypedValue(binary, Cs.Binary::getType);
            return binary;
        }

        @Override
        public Cs.BlockScopeNamespaceDeclaration visitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, SenderContext ctx) {
            ctx.sendValue(blockScopeNamespaceDeclaration, Cs.BlockScopeNamespaceDeclaration::getId);
            ctx.sendNode(blockScopeNamespaceDeclaration, Cs.BlockScopeNamespaceDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(blockScopeNamespaceDeclaration, Cs.BlockScopeNamespaceDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNode(blockScopeNamespaceDeclaration, e -> e.getPadding().getName(), CSharpSender::sendRightPadded);
            ctx.sendNodes(blockScopeNamespaceDeclaration, e -> e.getPadding().getExterns(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(blockScopeNamespaceDeclaration, e -> e.getPadding().getUsings(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(blockScopeNamespaceDeclaration, e -> e.getPadding().getMembers(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNode(blockScopeNamespaceDeclaration, Cs.BlockScopeNamespaceDeclaration::getEnd, CSharpSender::sendSpace);
            return blockScopeNamespaceDeclaration;
        }

        @Override
        public Cs.CollectionExpression visitCollectionExpression(Cs.CollectionExpression collectionExpression, SenderContext ctx) {
            ctx.sendValue(collectionExpression, Cs.CollectionExpression::getId);
            ctx.sendNode(collectionExpression, Cs.CollectionExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(collectionExpression, Cs.CollectionExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(collectionExpression, e -> e.getPadding().getElements(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendTypedValue(collectionExpression, Cs.CollectionExpression::getType);
            return collectionExpression;
        }

        @Override
        public Cs.ExpressionStatement visitExpressionStatement(Cs.ExpressionStatement expressionStatement, SenderContext ctx) {
            ctx.sendValue(expressionStatement, Cs.ExpressionStatement::getId);
            ctx.sendNode(expressionStatement, Cs.ExpressionStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(expressionStatement, Cs.ExpressionStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(expressionStatement, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            return expressionStatement;
        }

        @Override
        public Cs.ExternAlias visitExternAlias(Cs.ExternAlias externAlias, SenderContext ctx) {
            ctx.sendValue(externAlias, Cs.ExternAlias::getId);
            ctx.sendNode(externAlias, Cs.ExternAlias::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(externAlias, Cs.ExternAlias::getMarkers, ctx::sendMarkers);
            ctx.sendNode(externAlias, e -> e.getPadding().getIdentifier(), CSharpSender::sendLeftPadded);
            return externAlias;
        }

        @Override
        public Cs.FileScopeNamespaceDeclaration visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, SenderContext ctx) {
            ctx.sendValue(fileScopeNamespaceDeclaration, Cs.FileScopeNamespaceDeclaration::getId);
            ctx.sendNode(fileScopeNamespaceDeclaration, Cs.FileScopeNamespaceDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(fileScopeNamespaceDeclaration, Cs.FileScopeNamespaceDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNode(fileScopeNamespaceDeclaration, e -> e.getPadding().getName(), CSharpSender::sendRightPadded);
            ctx.sendNodes(fileScopeNamespaceDeclaration, e -> e.getPadding().getExterns(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(fileScopeNamespaceDeclaration, e -> e.getPadding().getUsings(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNodes(fileScopeNamespaceDeclaration, e -> e.getPadding().getMembers(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return fileScopeNamespaceDeclaration;
        }

        @Override
        public Cs.InterpolatedString visitInterpolatedString(Cs.InterpolatedString interpolatedString, SenderContext ctx) {
            ctx.sendValue(interpolatedString, Cs.InterpolatedString::getId);
            ctx.sendNode(interpolatedString, Cs.InterpolatedString::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(interpolatedString, Cs.InterpolatedString::getMarkers, ctx::sendMarkers);
            ctx.sendValue(interpolatedString, Cs.InterpolatedString::getStart);
            ctx.sendNodes(interpolatedString, e -> e.getPadding().getParts(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendValue(interpolatedString, Cs.InterpolatedString::getEnd);
            return interpolatedString;
        }

        @Override
        public Cs.Interpolation visitInterpolation(Cs.Interpolation interpolation, SenderContext ctx) {
            ctx.sendValue(interpolation, Cs.Interpolation::getId);
            ctx.sendNode(interpolation, Cs.Interpolation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(interpolation, Cs.Interpolation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(interpolation, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(interpolation, e -> e.getPadding().getAlignment(), CSharpSender::sendRightPadded);
            ctx.sendNode(interpolation, e -> e.getPadding().getFormat(), CSharpSender::sendRightPadded);
            return interpolation;
        }

        @Override
        public Cs.NullSafeExpression visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, SenderContext ctx) {
            ctx.sendValue(nullSafeExpression, Cs.NullSafeExpression::getId);
            ctx.sendNode(nullSafeExpression, Cs.NullSafeExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(nullSafeExpression, Cs.NullSafeExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(nullSafeExpression, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            return nullSafeExpression;
        }

        @Override
        public Cs.StatementExpression visitStatementExpression(Cs.StatementExpression statementExpression, SenderContext ctx) {
            ctx.sendValue(statementExpression, Cs.StatementExpression::getId);
            ctx.sendNode(statementExpression, Cs.StatementExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(statementExpression, Cs.StatementExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(statementExpression, Cs.StatementExpression::getStatement, ctx::sendTree);
            return statementExpression;
        }

        @Override
        public Cs.UsingDirective visitUsingDirective(Cs.UsingDirective usingDirective, SenderContext ctx) {
            ctx.sendValue(usingDirective, Cs.UsingDirective::getId);
            ctx.sendNode(usingDirective, Cs.UsingDirective::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(usingDirective, Cs.UsingDirective::getMarkers, ctx::sendMarkers);
            ctx.sendNode(usingDirective, e -> e.getPadding().getGlobal(), CSharpSender::sendRightPadded);
            ctx.sendNode(usingDirective, e -> e.getPadding().getStatic(), CSharpSender::sendLeftPadded);
            ctx.sendNode(usingDirective, e -> e.getPadding().getUnsafe(), CSharpSender::sendLeftPadded);
            ctx.sendNode(usingDirective, e -> e.getPadding().getAlias(), CSharpSender::sendRightPadded);
            ctx.sendNode(usingDirective, Cs.UsingDirective::getNamespaceOrType, ctx::sendTree);
            return usingDirective;
        }

        @Override
        public Cs.PropertyDeclaration visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, SenderContext ctx) {
            ctx.sendValue(propertyDeclaration, Cs.PropertyDeclaration::getId);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(propertyDeclaration, Cs.PropertyDeclaration::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNodes(propertyDeclaration, Cs.PropertyDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getTypeExpression, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, e -> e.getPadding().getInterfaceSpecifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getName, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getAccessors, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getExpressionBody, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, e -> e.getPadding().getInitializer(), CSharpSender::sendLeftPadded);
            return propertyDeclaration;
        }

        @Override
        public Cs.Keyword visitKeyword(Cs.Keyword keyword, SenderContext ctx) {
            ctx.sendValue(keyword, Cs.Keyword::getId);
            ctx.sendNode(keyword, Cs.Keyword::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(keyword, Cs.Keyword::getMarkers, ctx::sendMarkers);
            ctx.sendValue(keyword, Cs.Keyword::getKind);
            return keyword;
        }

        @Override
        public Cs.Lambda visitLambda(Cs.Lambda lambda, SenderContext ctx) {
            ctx.sendValue(lambda, Cs.Lambda::getId);
            ctx.sendNode(lambda, Cs.Lambda::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(lambda, Cs.Lambda::getMarkers, ctx::sendMarkers);
            ctx.sendNode(lambda, Cs.Lambda::getLambdaExpression, ctx::sendTree);
            ctx.sendNodes(lambda, Cs.Lambda::getModifiers, ctx::sendTree, Tree::getId);
            return lambda;
        }

        @Override
        public Cs.ClassDeclaration visitClassDeclaration(Cs.ClassDeclaration classDeclaration, SenderContext ctx) {
            ctx.sendValue(classDeclaration, Cs.ClassDeclaration::getId);
            ctx.sendNode(classDeclaration, Cs.ClassDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(classDeclaration, Cs.ClassDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(classDeclaration, Cs.ClassDeclaration::getAttributeList, ctx::sendTree, Tree::getId);
            ctx.sendNodes(classDeclaration, Cs.ClassDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getKind(), this::sendClassDeclarationKind);
            ctx.sendNode(classDeclaration, Cs.ClassDeclaration::getName, ctx::sendTree);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getPrimaryConstructor(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getExtendings(), CSharpSender::sendLeftPadded);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getImplementings(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, Cs.ClassDeclaration::getBody, ctx::sendTree);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getTypeParameterConstraintClauses(), CSharpSender::sendContainer);
            ctx.sendTypedValue(classDeclaration, Cs.ClassDeclaration::getType);
            return classDeclaration;
        }

        @Override
        public Cs.MethodDeclaration visitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, SenderContext ctx) {
            ctx.sendValue(methodDeclaration, Cs.MethodDeclaration::getId);
            ctx.sendNode(methodDeclaration, Cs.MethodDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(methodDeclaration, Cs.MethodDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(methodDeclaration, Cs.MethodDeclaration::getAttributes, ctx::sendTree, Tree::getId);
            ctx.sendNodes(methodDeclaration, Cs.MethodDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(methodDeclaration, Cs.MethodDeclaration::getReturnTypeExpression, ctx::sendTree);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getExplicitInterfaceSpecifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(methodDeclaration, Cs.MethodDeclaration::getName, ctx::sendTree);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getParameters(), CSharpSender::sendContainer);
            ctx.sendNode(methodDeclaration, Cs.MethodDeclaration::getBody, ctx::sendTree);
            ctx.sendTypedValue(methodDeclaration, Cs.MethodDeclaration::getMethodType);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getTypeParameterConstraintClauses(), CSharpSender::sendContainer);
            return methodDeclaration;
        }

        @Override
        public Cs.UsingStatement visitUsingStatement(Cs.UsingStatement usingStatement, SenderContext ctx) {
            ctx.sendValue(usingStatement, Cs.UsingStatement::getId);
            ctx.sendNode(usingStatement, Cs.UsingStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(usingStatement, Cs.UsingStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(usingStatement, Cs.UsingStatement::getAwaitKeyword, ctx::sendTree);
            ctx.sendNode(usingStatement, e -> e.getPadding().getExpression(), CSharpSender::sendLeftPadded);
            ctx.sendNode(usingStatement, Cs.UsingStatement::getStatement, ctx::sendTree);
            return usingStatement;
        }

        @Override
        public Cs.TypeParameterConstraintClause visitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, SenderContext ctx) {
            ctx.sendValue(typeParameterConstraintClause, Cs.TypeParameterConstraintClause::getId);
            ctx.sendNode(typeParameterConstraintClause, Cs.TypeParameterConstraintClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeParameterConstraintClause, Cs.TypeParameterConstraintClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(typeParameterConstraintClause, e -> e.getPadding().getTypeParameter(), CSharpSender::sendRightPadded);
            ctx.sendNode(typeParameterConstraintClause, e -> e.getPadding().getTypeParameterConstraints(), CSharpSender::sendContainer);
            return typeParameterConstraintClause;
        }

        @Override
        public Cs.TypeConstraint visitTypeConstraint(Cs.TypeConstraint typeConstraint, SenderContext ctx) {
            ctx.sendValue(typeConstraint, Cs.TypeConstraint::getId);
            ctx.sendNode(typeConstraint, Cs.TypeConstraint::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeConstraint, Cs.TypeConstraint::getMarkers, ctx::sendMarkers);
            ctx.sendNode(typeConstraint, Cs.TypeConstraint::getTypeExpression, ctx::sendTree);
            return typeConstraint;
        }

        @Override
        public Cs.AllowsConstraintClause visitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, SenderContext ctx) {
            ctx.sendValue(allowsConstraintClause, Cs.AllowsConstraintClause::getId);
            ctx.sendNode(allowsConstraintClause, Cs.AllowsConstraintClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(allowsConstraintClause, Cs.AllowsConstraintClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(allowsConstraintClause, e -> e.getPadding().getExpressions(), CSharpSender::sendContainer);
            return allowsConstraintClause;
        }

        @Override
        public Cs.RefStructConstraint visitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, SenderContext ctx) {
            ctx.sendValue(refStructConstraint, Cs.RefStructConstraint::getId);
            ctx.sendNode(refStructConstraint, Cs.RefStructConstraint::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(refStructConstraint, Cs.RefStructConstraint::getMarkers, ctx::sendMarkers);
            return refStructConstraint;
        }

        @Override
        public Cs.ClassOrStructConstraint visitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, SenderContext ctx) {
            ctx.sendValue(classOrStructConstraint, Cs.ClassOrStructConstraint::getId);
            ctx.sendNode(classOrStructConstraint, Cs.ClassOrStructConstraint::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(classOrStructConstraint, Cs.ClassOrStructConstraint::getMarkers, ctx::sendMarkers);
            ctx.sendValue(classOrStructConstraint, Cs.ClassOrStructConstraint::getKind);
            return classOrStructConstraint;
        }

        @Override
        public Cs.ConstructorConstraint visitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, SenderContext ctx) {
            ctx.sendValue(constructorConstraint, Cs.ConstructorConstraint::getId);
            ctx.sendNode(constructorConstraint, Cs.ConstructorConstraint::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(constructorConstraint, Cs.ConstructorConstraint::getMarkers, ctx::sendMarkers);
            return constructorConstraint;
        }

        @Override
        public Cs.DefaultConstraint visitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, SenderContext ctx) {
            ctx.sendValue(defaultConstraint, Cs.DefaultConstraint::getId);
            ctx.sendNode(defaultConstraint, Cs.DefaultConstraint::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(defaultConstraint, Cs.DefaultConstraint::getMarkers, ctx::sendMarkers);
            return defaultConstraint;
        }

        @Override
        public Cs.DeclarationExpression visitDeclarationExpression(Cs.DeclarationExpression declarationExpression, SenderContext ctx) {
            ctx.sendValue(declarationExpression, Cs.DeclarationExpression::getId);
            ctx.sendNode(declarationExpression, Cs.DeclarationExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(declarationExpression, Cs.DeclarationExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(declarationExpression, Cs.DeclarationExpression::getTypeExpression, ctx::sendTree);
            ctx.sendNode(declarationExpression, Cs.DeclarationExpression::getVariables, ctx::sendTree);
            return declarationExpression;
        }

        @Override
        public Cs.SingleVariableDesignation visitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, SenderContext ctx) {
            ctx.sendValue(singleVariableDesignation, Cs.SingleVariableDesignation::getId);
            ctx.sendNode(singleVariableDesignation, Cs.SingleVariableDesignation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(singleVariableDesignation, Cs.SingleVariableDesignation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(singleVariableDesignation, Cs.SingleVariableDesignation::getName, ctx::sendTree);
            return singleVariableDesignation;
        }

        @Override
        public Cs.ParenthesizedVariableDesignation visitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, SenderContext ctx) {
            ctx.sendValue(parenthesizedVariableDesignation, Cs.ParenthesizedVariableDesignation::getId);
            ctx.sendNode(parenthesizedVariableDesignation, Cs.ParenthesizedVariableDesignation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parenthesizedVariableDesignation, Cs.ParenthesizedVariableDesignation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(parenthesizedVariableDesignation, e -> e.getPadding().getVariables(), CSharpSender::sendContainer);
            ctx.sendTypedValue(parenthesizedVariableDesignation, Cs.ParenthesizedVariableDesignation::getType);
            return parenthesizedVariableDesignation;
        }

        @Override
        public Cs.DiscardVariableDesignation visitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, SenderContext ctx) {
            ctx.sendValue(discardVariableDesignation, Cs.DiscardVariableDesignation::getId);
            ctx.sendNode(discardVariableDesignation, Cs.DiscardVariableDesignation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(discardVariableDesignation, Cs.DiscardVariableDesignation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(discardVariableDesignation, Cs.DiscardVariableDesignation::getDiscard, ctx::sendTree);
            return discardVariableDesignation;
        }

        @Override
        public Cs.TupleExpression visitTupleExpression(Cs.TupleExpression tupleExpression, SenderContext ctx) {
            ctx.sendValue(tupleExpression, Cs.TupleExpression::getId);
            ctx.sendNode(tupleExpression, Cs.TupleExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(tupleExpression, Cs.TupleExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(tupleExpression, e -> e.getPadding().getArguments(), CSharpSender::sendContainer);
            return tupleExpression;
        }

        @Override
        public Cs.Constructor visitConstructor(Cs.Constructor constructor, SenderContext ctx) {
            ctx.sendValue(constructor, Cs.Constructor::getId);
            ctx.sendNode(constructor, Cs.Constructor::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(constructor, Cs.Constructor::getMarkers, ctx::sendMarkers);
            ctx.sendNode(constructor, Cs.Constructor::getInitializer, ctx::sendTree);
            ctx.sendNode(constructor, Cs.Constructor::getConstructorCore, ctx::sendTree);
            return constructor;
        }

        @Override
        public Cs.DestructorDeclaration visitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, SenderContext ctx) {
            ctx.sendValue(destructorDeclaration, Cs.DestructorDeclaration::getId);
            ctx.sendNode(destructorDeclaration, Cs.DestructorDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(destructorDeclaration, Cs.DestructorDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNode(destructorDeclaration, Cs.DestructorDeclaration::getMethodCore, ctx::sendTree);
            return destructorDeclaration;
        }

        @Override
        public Cs.Unary visitUnary(Cs.Unary unary, SenderContext ctx) {
            ctx.sendValue(unary, Cs.Unary::getId);
            ctx.sendNode(unary, Cs.Unary::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(unary, Cs.Unary::getMarkers, ctx::sendMarkers);
            ctx.sendNode(unary, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(unary, Cs.Unary::getExpression, ctx::sendTree);
            ctx.sendTypedValue(unary, Cs.Unary::getType);
            return unary;
        }

        @Override
        public Cs.ConstructorInitializer visitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, SenderContext ctx) {
            ctx.sendValue(constructorInitializer, Cs.ConstructorInitializer::getId);
            ctx.sendNode(constructorInitializer, Cs.ConstructorInitializer::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(constructorInitializer, Cs.ConstructorInitializer::getMarkers, ctx::sendMarkers);
            ctx.sendNode(constructorInitializer, Cs.ConstructorInitializer::getKeyword, ctx::sendTree);
            ctx.sendNode(constructorInitializer, e -> e.getPadding().getArguments(), CSharpSender::sendContainer);
            return constructorInitializer;
        }

        @Override
        public Cs.TupleType visitTupleType(Cs.TupleType tupleType, SenderContext ctx) {
            ctx.sendValue(tupleType, Cs.TupleType::getId);
            ctx.sendNode(tupleType, Cs.TupleType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(tupleType, Cs.TupleType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(tupleType, e -> e.getPadding().getElements(), CSharpSender::sendContainer);
            ctx.sendTypedValue(tupleType, Cs.TupleType::getType);
            return tupleType;
        }

        @Override
        public Cs.TupleElement visitTupleElement(Cs.TupleElement tupleElement, SenderContext ctx) {
            ctx.sendValue(tupleElement, Cs.TupleElement::getId);
            ctx.sendNode(tupleElement, Cs.TupleElement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(tupleElement, Cs.TupleElement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(tupleElement, Cs.TupleElement::getType, ctx::sendTree);
            ctx.sendNode(tupleElement, Cs.TupleElement::getName, ctx::sendTree);
            return tupleElement;
        }

        @Override
        public Cs.NewClass visitNewClass(Cs.NewClass newClass, SenderContext ctx) {
            ctx.sendValue(newClass, Cs.NewClass::getId);
            ctx.sendNode(newClass, Cs.NewClass::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(newClass, Cs.NewClass::getMarkers, ctx::sendMarkers);
            ctx.sendNode(newClass, Cs.NewClass::getNewClassCore, ctx::sendTree);
            ctx.sendNode(newClass, Cs.NewClass::getInitializer, ctx::sendTree);
            return newClass;
        }

        @Override
        public Cs.InitializerExpression visitInitializerExpression(Cs.InitializerExpression initializerExpression, SenderContext ctx) {
            ctx.sendValue(initializerExpression, Cs.InitializerExpression::getId);
            ctx.sendNode(initializerExpression, Cs.InitializerExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(initializerExpression, Cs.InitializerExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(initializerExpression, e -> e.getPadding().getExpressions(), CSharpSender::sendContainer);
            return initializerExpression;
        }

        @Override
        public Cs.ImplicitElementAccess visitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, SenderContext ctx) {
            ctx.sendValue(implicitElementAccess, Cs.ImplicitElementAccess::getId);
            ctx.sendNode(implicitElementAccess, Cs.ImplicitElementAccess::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(implicitElementAccess, Cs.ImplicitElementAccess::getMarkers, ctx::sendMarkers);
            ctx.sendNode(implicitElementAccess, e -> e.getPadding().getArgumentList(), CSharpSender::sendContainer);
            return implicitElementAccess;
        }

        @Override
        public Cs.Yield visitYield(Cs.Yield yield, SenderContext ctx) {
            ctx.sendValue(yield, Cs.Yield::getId);
            ctx.sendNode(yield, Cs.Yield::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(yield, Cs.Yield::getMarkers, ctx::sendMarkers);
            ctx.sendNode(yield, Cs.Yield::getReturnOrBreakKeyword, ctx::sendTree);
            ctx.sendNode(yield, Cs.Yield::getExpression, ctx::sendTree);
            return yield;
        }

        @Override
        public Cs.DefaultExpression visitDefaultExpression(Cs.DefaultExpression defaultExpression, SenderContext ctx) {
            ctx.sendValue(defaultExpression, Cs.DefaultExpression::getId);
            ctx.sendNode(defaultExpression, Cs.DefaultExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(defaultExpression, Cs.DefaultExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(defaultExpression, e -> e.getPadding().getTypeOperator(), CSharpSender::sendContainer);
            return defaultExpression;
        }

        @Override
        public Cs.IsPattern visitIsPattern(Cs.IsPattern isPattern, SenderContext ctx) {
            ctx.sendValue(isPattern, Cs.IsPattern::getId);
            ctx.sendNode(isPattern, Cs.IsPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(isPattern, Cs.IsPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(isPattern, Cs.IsPattern::getExpression, ctx::sendTree);
            ctx.sendNode(isPattern, e -> e.getPadding().getPattern(), CSharpSender::sendLeftPadded);
            return isPattern;
        }

        @Override
        public Cs.UnaryPattern visitUnaryPattern(Cs.UnaryPattern unaryPattern, SenderContext ctx) {
            ctx.sendValue(unaryPattern, Cs.UnaryPattern::getId);
            ctx.sendNode(unaryPattern, Cs.UnaryPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(unaryPattern, Cs.UnaryPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(unaryPattern, Cs.UnaryPattern::getOperator, ctx::sendTree);
            ctx.sendNode(unaryPattern, Cs.UnaryPattern::getPattern, ctx::sendTree);
            return unaryPattern;
        }

        @Override
        public Cs.TypePattern visitTypePattern(Cs.TypePattern typePattern, SenderContext ctx) {
            ctx.sendValue(typePattern, Cs.TypePattern::getId);
            ctx.sendNode(typePattern, Cs.TypePattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typePattern, Cs.TypePattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(typePattern, Cs.TypePattern::getTypeIdentifier, ctx::sendTree);
            ctx.sendNode(typePattern, Cs.TypePattern::getDesignation, ctx::sendTree);
            return typePattern;
        }

        @Override
        public Cs.BinaryPattern visitBinaryPattern(Cs.BinaryPattern binaryPattern, SenderContext ctx) {
            ctx.sendValue(binaryPattern, Cs.BinaryPattern::getId);
            ctx.sendNode(binaryPattern, Cs.BinaryPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(binaryPattern, Cs.BinaryPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(binaryPattern, Cs.BinaryPattern::getLeft, ctx::sendTree);
            ctx.sendNode(binaryPattern, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(binaryPattern, Cs.BinaryPattern::getRight, ctx::sendTree);
            return binaryPattern;
        }

        @Override
        public Cs.ConstantPattern visitConstantPattern(Cs.ConstantPattern constantPattern, SenderContext ctx) {
            ctx.sendValue(constantPattern, Cs.ConstantPattern::getId);
            ctx.sendNode(constantPattern, Cs.ConstantPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(constantPattern, Cs.ConstantPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(constantPattern, Cs.ConstantPattern::getValue, ctx::sendTree);
            return constantPattern;
        }

        @Override
        public Cs.DiscardPattern visitDiscardPattern(Cs.DiscardPattern discardPattern, SenderContext ctx) {
            ctx.sendValue(discardPattern, Cs.DiscardPattern::getId);
            ctx.sendNode(discardPattern, Cs.DiscardPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(discardPattern, Cs.DiscardPattern::getMarkers, ctx::sendMarkers);
            ctx.sendTypedValue(discardPattern, Cs.DiscardPattern::getType);
            return discardPattern;
        }

        @Override
        public Cs.ListPattern visitListPattern(Cs.ListPattern listPattern, SenderContext ctx) {
            ctx.sendValue(listPattern, Cs.ListPattern::getId);
            ctx.sendNode(listPattern, Cs.ListPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(listPattern, Cs.ListPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(listPattern, e -> e.getPadding().getPatterns(), CSharpSender::sendContainer);
            ctx.sendNode(listPattern, Cs.ListPattern::getDesignation, ctx::sendTree);
            return listPattern;
        }

        @Override
        public Cs.ParenthesizedPattern visitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, SenderContext ctx) {
            ctx.sendValue(parenthesizedPattern, Cs.ParenthesizedPattern::getId);
            ctx.sendNode(parenthesizedPattern, Cs.ParenthesizedPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parenthesizedPattern, Cs.ParenthesizedPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(parenthesizedPattern, e -> e.getPadding().getPattern(), CSharpSender::sendContainer);
            return parenthesizedPattern;
        }

        @Override
        public Cs.RecursivePattern visitRecursivePattern(Cs.RecursivePattern recursivePattern, SenderContext ctx) {
            ctx.sendValue(recursivePattern, Cs.RecursivePattern::getId);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getTypeQualifier, ctx::sendTree);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getPositionalPattern, ctx::sendTree);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getPropertyPattern, ctx::sendTree);
            ctx.sendNode(recursivePattern, Cs.RecursivePattern::getDesignation, ctx::sendTree);
            return recursivePattern;
        }

        @Override
        public Cs.VarPattern visitVarPattern(Cs.VarPattern varPattern, SenderContext ctx) {
            ctx.sendValue(varPattern, Cs.VarPattern::getId);
            ctx.sendNode(varPattern, Cs.VarPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(varPattern, Cs.VarPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(varPattern, Cs.VarPattern::getDesignation, ctx::sendTree);
            return varPattern;
        }

        @Override
        public Cs.PositionalPatternClause visitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, SenderContext ctx) {
            ctx.sendValue(positionalPatternClause, Cs.PositionalPatternClause::getId);
            ctx.sendNode(positionalPatternClause, Cs.PositionalPatternClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(positionalPatternClause, Cs.PositionalPatternClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(positionalPatternClause, e -> e.getPadding().getSubpatterns(), CSharpSender::sendContainer);
            return positionalPatternClause;
        }

        @Override
        public Cs.RelationalPattern visitRelationalPattern(Cs.RelationalPattern relationalPattern, SenderContext ctx) {
            ctx.sendValue(relationalPattern, Cs.RelationalPattern::getId);
            ctx.sendNode(relationalPattern, Cs.RelationalPattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(relationalPattern, Cs.RelationalPattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(relationalPattern, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(relationalPattern, Cs.RelationalPattern::getValue, ctx::sendTree);
            return relationalPattern;
        }

        @Override
        public Cs.SlicePattern visitSlicePattern(Cs.SlicePattern slicePattern, SenderContext ctx) {
            ctx.sendValue(slicePattern, Cs.SlicePattern::getId);
            ctx.sendNode(slicePattern, Cs.SlicePattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(slicePattern, Cs.SlicePattern::getMarkers, ctx::sendMarkers);
            return slicePattern;
        }

        @Override
        public Cs.PropertyPatternClause visitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, SenderContext ctx) {
            ctx.sendValue(propertyPatternClause, Cs.PropertyPatternClause::getId);
            ctx.sendNode(propertyPatternClause, Cs.PropertyPatternClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(propertyPatternClause, Cs.PropertyPatternClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(propertyPatternClause, e -> e.getPadding().getSubpatterns(), CSharpSender::sendContainer);
            return propertyPatternClause;
        }

        @Override
        public Cs.Subpattern visitSubpattern(Cs.Subpattern subpattern, SenderContext ctx) {
            ctx.sendValue(subpattern, Cs.Subpattern::getId);
            ctx.sendNode(subpattern, Cs.Subpattern::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(subpattern, Cs.Subpattern::getMarkers, ctx::sendMarkers);
            ctx.sendNode(subpattern, Cs.Subpattern::getName, ctx::sendTree);
            ctx.sendNode(subpattern, e -> e.getPadding().getPattern(), CSharpSender::sendLeftPadded);
            return subpattern;
        }

        @Override
        public Cs.SwitchExpression visitSwitchExpression(Cs.SwitchExpression switchExpression, SenderContext ctx) {
            ctx.sendValue(switchExpression, Cs.SwitchExpression::getId);
            ctx.sendNode(switchExpression, Cs.SwitchExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switchExpression, Cs.SwitchExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(switchExpression, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(switchExpression, e -> e.getPadding().getArms(), CSharpSender::sendContainer);
            return switchExpression;
        }

        @Override
        public Cs.SwitchExpressionArm visitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, SenderContext ctx) {
            ctx.sendValue(switchExpressionArm, Cs.SwitchExpressionArm::getId);
            ctx.sendNode(switchExpressionArm, Cs.SwitchExpressionArm::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switchExpressionArm, Cs.SwitchExpressionArm::getMarkers, ctx::sendMarkers);
            ctx.sendNode(switchExpressionArm, Cs.SwitchExpressionArm::getPattern, ctx::sendTree);
            ctx.sendNode(switchExpressionArm, e -> e.getPadding().getWhenExpression(), CSharpSender::sendLeftPadded);
            ctx.sendNode(switchExpressionArm, e -> e.getPadding().getExpression(), CSharpSender::sendLeftPadded);
            return switchExpressionArm;
        }

        @Override
        public Cs.SwitchSection visitSwitchSection(Cs.SwitchSection switchSection, SenderContext ctx) {
            ctx.sendValue(switchSection, Cs.SwitchSection::getId);
            ctx.sendNode(switchSection, Cs.SwitchSection::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switchSection, Cs.SwitchSection::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(switchSection, Cs.SwitchSection::getLabels, ctx::sendTree, Tree::getId);
            ctx.sendNodes(switchSection, e -> e.getPadding().getStatements(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return switchSection;
        }

        @Override
        public Cs.DefaultSwitchLabel visitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, SenderContext ctx) {
            ctx.sendValue(defaultSwitchLabel, Cs.DefaultSwitchLabel::getId);
            ctx.sendNode(defaultSwitchLabel, Cs.DefaultSwitchLabel::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(defaultSwitchLabel, Cs.DefaultSwitchLabel::getMarkers, ctx::sendMarkers);
            ctx.sendNode(defaultSwitchLabel, Cs.DefaultSwitchLabel::getColonToken, CSharpSender::sendSpace);
            return defaultSwitchLabel;
        }

        @Override
        public Cs.CasePatternSwitchLabel visitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, SenderContext ctx) {
            ctx.sendValue(casePatternSwitchLabel, Cs.CasePatternSwitchLabel::getId);
            ctx.sendNode(casePatternSwitchLabel, Cs.CasePatternSwitchLabel::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(casePatternSwitchLabel, Cs.CasePatternSwitchLabel::getMarkers, ctx::sendMarkers);
            ctx.sendNode(casePatternSwitchLabel, Cs.CasePatternSwitchLabel::getPattern, ctx::sendTree);
            ctx.sendNode(casePatternSwitchLabel, e -> e.getPadding().getWhenClause(), CSharpSender::sendLeftPadded);
            ctx.sendNode(casePatternSwitchLabel, Cs.CasePatternSwitchLabel::getColonToken, CSharpSender::sendSpace);
            return casePatternSwitchLabel;
        }

        @Override
        public Cs.SwitchStatement visitSwitchStatement(Cs.SwitchStatement switchStatement, SenderContext ctx) {
            ctx.sendValue(switchStatement, Cs.SwitchStatement::getId);
            ctx.sendNode(switchStatement, Cs.SwitchStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switchStatement, Cs.SwitchStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(switchStatement, e -> e.getPadding().getExpression(), CSharpSender::sendContainer);
            ctx.sendNode(switchStatement, e -> e.getPadding().getSections(), CSharpSender::sendContainer);
            return switchStatement;
        }

        @Override
        public Cs.LockStatement visitLockStatement(Cs.LockStatement lockStatement, SenderContext ctx) {
            ctx.sendValue(lockStatement, Cs.LockStatement::getId);
            ctx.sendNode(lockStatement, Cs.LockStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(lockStatement, Cs.LockStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(lockStatement, Cs.LockStatement::getExpression, ctx::sendTree);
            ctx.sendNode(lockStatement, e -> e.getPadding().getStatement(), CSharpSender::sendRightPadded);
            return lockStatement;
        }

        @Override
        public Cs.FixedStatement visitFixedStatement(Cs.FixedStatement fixedStatement, SenderContext ctx) {
            ctx.sendValue(fixedStatement, Cs.FixedStatement::getId);
            ctx.sendNode(fixedStatement, Cs.FixedStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(fixedStatement, Cs.FixedStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(fixedStatement, Cs.FixedStatement::getDeclarations, ctx::sendTree);
            ctx.sendNode(fixedStatement, Cs.FixedStatement::getBlock, ctx::sendTree);
            return fixedStatement;
        }

        @Override
        public Cs.CheckedExpression visitCheckedExpression(Cs.CheckedExpression checkedExpression, SenderContext ctx) {
            ctx.sendValue(checkedExpression, Cs.CheckedExpression::getId);
            ctx.sendNode(checkedExpression, Cs.CheckedExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(checkedExpression, Cs.CheckedExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(checkedExpression, Cs.CheckedExpression::getCheckedOrUncheckedKeyword, ctx::sendTree);
            ctx.sendNode(checkedExpression, Cs.CheckedExpression::getExpression, ctx::sendTree);
            return checkedExpression;
        }

        @Override
        public Cs.CheckedStatement visitCheckedStatement(Cs.CheckedStatement checkedStatement, SenderContext ctx) {
            ctx.sendValue(checkedStatement, Cs.CheckedStatement::getId);
            ctx.sendNode(checkedStatement, Cs.CheckedStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(checkedStatement, Cs.CheckedStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(checkedStatement, Cs.CheckedStatement::getKeyword, ctx::sendTree);
            ctx.sendNode(checkedStatement, Cs.CheckedStatement::getBlock, ctx::sendTree);
            return checkedStatement;
        }

        @Override
        public Cs.UnsafeStatement visitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, SenderContext ctx) {
            ctx.sendValue(unsafeStatement, Cs.UnsafeStatement::getId);
            ctx.sendNode(unsafeStatement, Cs.UnsafeStatement::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(unsafeStatement, Cs.UnsafeStatement::getMarkers, ctx::sendMarkers);
            ctx.sendNode(unsafeStatement, Cs.UnsafeStatement::getBlock, ctx::sendTree);
            return unsafeStatement;
        }

        @Override
        public Cs.RangeExpression visitRangeExpression(Cs.RangeExpression rangeExpression, SenderContext ctx) {
            ctx.sendValue(rangeExpression, Cs.RangeExpression::getId);
            ctx.sendNode(rangeExpression, Cs.RangeExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(rangeExpression, Cs.RangeExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(rangeExpression, e -> e.getPadding().getStart(), CSharpSender::sendRightPadded);
            ctx.sendNode(rangeExpression, Cs.RangeExpression::getEnd, ctx::sendTree);
            return rangeExpression;
        }

        @Override
        public Cs.QueryExpression visitQueryExpression(Cs.QueryExpression queryExpression, SenderContext ctx) {
            ctx.sendValue(queryExpression, Cs.QueryExpression::getId);
            ctx.sendNode(queryExpression, Cs.QueryExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(queryExpression, Cs.QueryExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(queryExpression, Cs.QueryExpression::getFromClause, ctx::sendTree);
            ctx.sendNode(queryExpression, Cs.QueryExpression::getBody, ctx::sendTree);
            return queryExpression;
        }

        @Override
        public Cs.QueryBody visitQueryBody(Cs.QueryBody queryBody, SenderContext ctx) {
            ctx.sendValue(queryBody, Cs.QueryBody::getId);
            ctx.sendNode(queryBody, Cs.QueryBody::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(queryBody, Cs.QueryBody::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(queryBody, Cs.QueryBody::getClauses, ctx::sendTree, Tree::getId);
            ctx.sendNode(queryBody, Cs.QueryBody::getSelectOrGroup, ctx::sendTree);
            ctx.sendNode(queryBody, Cs.QueryBody::getContinuation, ctx::sendTree);
            return queryBody;
        }

        @Override
        public Cs.FromClause visitFromClause(Cs.FromClause fromClause, SenderContext ctx) {
            ctx.sendValue(fromClause, Cs.FromClause::getId);
            ctx.sendNode(fromClause, Cs.FromClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(fromClause, Cs.FromClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(fromClause, Cs.FromClause::getTypeIdentifier, ctx::sendTree);
            ctx.sendNode(fromClause, e -> e.getPadding().getIdentifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(fromClause, Cs.FromClause::getExpression, ctx::sendTree);
            return fromClause;
        }

        @Override
        public Cs.LetClause visitLetClause(Cs.LetClause letClause, SenderContext ctx) {
            ctx.sendValue(letClause, Cs.LetClause::getId);
            ctx.sendNode(letClause, Cs.LetClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(letClause, Cs.LetClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(letClause, e -> e.getPadding().getIdentifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(letClause, Cs.LetClause::getExpression, ctx::sendTree);
            return letClause;
        }

        @Override
        public Cs.JoinClause visitJoinClause(Cs.JoinClause joinClause, SenderContext ctx) {
            ctx.sendValue(joinClause, Cs.JoinClause::getId);
            ctx.sendNode(joinClause, Cs.JoinClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(joinClause, Cs.JoinClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(joinClause, e -> e.getPadding().getIdentifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(joinClause, e -> e.getPadding().getInExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(joinClause, e -> e.getPadding().getLeftExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(joinClause, Cs.JoinClause::getRightExpression, ctx::sendTree);
            ctx.sendNode(joinClause, e -> e.getPadding().getInto(), CSharpSender::sendLeftPadded);
            return joinClause;
        }

        @Override
        public Cs.JoinIntoClause visitJoinIntoClause(Cs.JoinIntoClause joinIntoClause, SenderContext ctx) {
            ctx.sendValue(joinIntoClause, Cs.JoinIntoClause::getId);
            ctx.sendNode(joinIntoClause, Cs.JoinIntoClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(joinIntoClause, Cs.JoinIntoClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(joinIntoClause, Cs.JoinIntoClause::getIdentifier, ctx::sendTree);
            return joinIntoClause;
        }

        @Override
        public Cs.WhereClause visitWhereClause(Cs.WhereClause whereClause, SenderContext ctx) {
            ctx.sendValue(whereClause, Cs.WhereClause::getId);
            ctx.sendNode(whereClause, Cs.WhereClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(whereClause, Cs.WhereClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(whereClause, Cs.WhereClause::getCondition, ctx::sendTree);
            return whereClause;
        }

        @Override
        public Cs.OrderByClause visitOrderByClause(Cs.OrderByClause orderByClause, SenderContext ctx) {
            ctx.sendValue(orderByClause, Cs.OrderByClause::getId);
            ctx.sendNode(orderByClause, Cs.OrderByClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(orderByClause, Cs.OrderByClause::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(orderByClause, e -> e.getPadding().getOrderings(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return orderByClause;
        }

        @Override
        public Cs.QueryContinuation visitQueryContinuation(Cs.QueryContinuation queryContinuation, SenderContext ctx) {
            ctx.sendValue(queryContinuation, Cs.QueryContinuation::getId);
            ctx.sendNode(queryContinuation, Cs.QueryContinuation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(queryContinuation, Cs.QueryContinuation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(queryContinuation, Cs.QueryContinuation::getIdentifier, ctx::sendTree);
            ctx.sendNode(queryContinuation, Cs.QueryContinuation::getBody, ctx::sendTree);
            return queryContinuation;
        }

        @Override
        public Cs.Ordering visitOrdering(Cs.Ordering ordering, SenderContext ctx) {
            ctx.sendValue(ordering, Cs.Ordering::getId);
            ctx.sendNode(ordering, Cs.Ordering::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(ordering, Cs.Ordering::getMarkers, ctx::sendMarkers);
            ctx.sendNode(ordering, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            ctx.sendValue(ordering, Cs.Ordering::getDirection);
            return ordering;
        }

        @Override
        public Cs.SelectClause visitSelectClause(Cs.SelectClause selectClause, SenderContext ctx) {
            ctx.sendValue(selectClause, Cs.SelectClause::getId);
            ctx.sendNode(selectClause, Cs.SelectClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(selectClause, Cs.SelectClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(selectClause, Cs.SelectClause::getExpression, ctx::sendTree);
            return selectClause;
        }

        @Override
        public Cs.GroupClause visitGroupClause(Cs.GroupClause groupClause, SenderContext ctx) {
            ctx.sendValue(groupClause, Cs.GroupClause::getId);
            ctx.sendNode(groupClause, Cs.GroupClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(groupClause, Cs.GroupClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(groupClause, e -> e.getPadding().getGroupExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(groupClause, Cs.GroupClause::getKey, ctx::sendTree);
            return groupClause;
        }

        @Override
        public Cs.IndexerDeclaration visitIndexerDeclaration(Cs.IndexerDeclaration indexerDeclaration, SenderContext ctx) {
            ctx.sendValue(indexerDeclaration, Cs.IndexerDeclaration::getId);
            ctx.sendNode(indexerDeclaration, Cs.IndexerDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(indexerDeclaration, Cs.IndexerDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(indexerDeclaration, Cs.IndexerDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(indexerDeclaration, Cs.IndexerDeclaration::getTypeExpression, ctx::sendTree);
            ctx.sendNode(indexerDeclaration, Cs.IndexerDeclaration::getIndexer, ctx::sendTree);
            ctx.sendNode(indexerDeclaration, e -> e.getPadding().getParameters(), CSharpSender::sendContainer);
            ctx.sendNode(indexerDeclaration, e -> e.getPadding().getExpressionBody(), CSharpSender::sendLeftPadded);
            ctx.sendNode(indexerDeclaration, Cs.IndexerDeclaration::getAccessors, ctx::sendTree);
            return indexerDeclaration;
        }

        @Override
        public Cs.DelegateDeclaration visitDelegateDeclaration(Cs.DelegateDeclaration delegateDeclaration, SenderContext ctx) {
            ctx.sendValue(delegateDeclaration, Cs.DelegateDeclaration::getId);
            ctx.sendNode(delegateDeclaration, Cs.DelegateDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(delegateDeclaration, Cs.DelegateDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(delegateDeclaration, Cs.DelegateDeclaration::getAttributes, ctx::sendTree, Tree::getId);
            ctx.sendNodes(delegateDeclaration, Cs.DelegateDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(delegateDeclaration, e -> e.getPadding().getReturnType(), CSharpSender::sendLeftPadded);
            ctx.sendNode(delegateDeclaration, Cs.DelegateDeclaration::getIdentifier, ctx::sendTree);
            ctx.sendNode(delegateDeclaration, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(delegateDeclaration, e -> e.getPadding().getParameters(), CSharpSender::sendContainer);
            ctx.sendNode(delegateDeclaration, e -> e.getPadding().getTypeParameterConstraintClauses(), CSharpSender::sendContainer);
            return delegateDeclaration;
        }

        @Override
        public Cs.ConversionOperatorDeclaration visitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration conversionOperatorDeclaration, SenderContext ctx) {
            ctx.sendValue(conversionOperatorDeclaration, Cs.ConversionOperatorDeclaration::getId);
            ctx.sendNode(conversionOperatorDeclaration, Cs.ConversionOperatorDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(conversionOperatorDeclaration, Cs.ConversionOperatorDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(conversionOperatorDeclaration, Cs.ConversionOperatorDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(conversionOperatorDeclaration, e -> e.getPadding().getKind(), CSharpSender::sendLeftPadded);
            ctx.sendNode(conversionOperatorDeclaration, e -> e.getPadding().getReturnType(), CSharpSender::sendLeftPadded);
            ctx.sendNode(conversionOperatorDeclaration, e -> e.getPadding().getParameters(), CSharpSender::sendContainer);
            ctx.sendNode(conversionOperatorDeclaration, e -> e.getPadding().getExpressionBody(), CSharpSender::sendLeftPadded);
            ctx.sendNode(conversionOperatorDeclaration, Cs.ConversionOperatorDeclaration::getBody, ctx::sendTree);
            return conversionOperatorDeclaration;
        }

        @Override
        public Cs.TypeParameter visitTypeParameter(Cs.TypeParameter typeParameter, SenderContext ctx) {
            ctx.sendValue(typeParameter, Cs.TypeParameter::getId);
            ctx.sendNode(typeParameter, Cs.TypeParameter::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeParameter, Cs.TypeParameter::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(typeParameter, Cs.TypeParameter::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNode(typeParameter, e -> e.getPadding().getVariance(), CSharpSender::sendLeftPadded);
            ctx.sendNode(typeParameter, Cs.TypeParameter::getName, ctx::sendTree);
            return typeParameter;
        }

        @Override
        public Cs.EnumDeclaration visitEnumDeclaration(Cs.EnumDeclaration enumDeclaration, SenderContext ctx) {
            ctx.sendValue(enumDeclaration, Cs.EnumDeclaration::getId);
            ctx.sendNode(enumDeclaration, Cs.EnumDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(enumDeclaration, Cs.EnumDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(enumDeclaration, Cs.EnumDeclaration::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNodes(enumDeclaration, Cs.EnumDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(enumDeclaration, e -> e.getPadding().getName(), CSharpSender::sendLeftPadded);
            ctx.sendNode(enumDeclaration, e -> e.getPadding().getBaseType(), CSharpSender::sendLeftPadded);
            ctx.sendNode(enumDeclaration, e -> e.getPadding().getMembers(), CSharpSender::sendContainer);
            return enumDeclaration;
        }

        @Override
        public Cs.EnumMemberDeclaration visitEnumMemberDeclaration(Cs.EnumMemberDeclaration enumMemberDeclaration, SenderContext ctx) {
            ctx.sendValue(enumMemberDeclaration, Cs.EnumMemberDeclaration::getId);
            ctx.sendNode(enumMemberDeclaration, Cs.EnumMemberDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(enumMemberDeclaration, Cs.EnumMemberDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(enumMemberDeclaration, Cs.EnumMemberDeclaration::getAttributeLists, ctx::sendTree, Tree::getId);
            ctx.sendNode(enumMemberDeclaration, Cs.EnumMemberDeclaration::getName, ctx::sendTree);
            ctx.sendNode(enumMemberDeclaration, e -> e.getPadding().getInitializer(), CSharpSender::sendLeftPadded);
            return enumMemberDeclaration;
        }

        @Override
        public Cs.AliasQualifiedName visitAliasQualifiedName(Cs.AliasQualifiedName aliasQualifiedName, SenderContext ctx) {
            ctx.sendValue(aliasQualifiedName, Cs.AliasQualifiedName::getId);
            ctx.sendNode(aliasQualifiedName, Cs.AliasQualifiedName::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(aliasQualifiedName, Cs.AliasQualifiedName::getMarkers, ctx::sendMarkers);
            ctx.sendNode(aliasQualifiedName, e -> e.getPadding().getAlias(), CSharpSender::sendRightPadded);
            ctx.sendNode(aliasQualifiedName, Cs.AliasQualifiedName::getName, ctx::sendTree);
            return aliasQualifiedName;
        }

        @Override
        public Cs.ArrayType visitArrayType(Cs.ArrayType arrayType, SenderContext ctx) {
            ctx.sendValue(arrayType, Cs.ArrayType::getId);
            ctx.sendNode(arrayType, Cs.ArrayType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrayType, Cs.ArrayType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrayType, Cs.ArrayType::getTypeExpression, ctx::sendTree);
            ctx.sendNodes(arrayType, Cs.ArrayType::getDimensions, ctx::sendTree, Tree::getId);
            ctx.sendTypedValue(arrayType, Cs.ArrayType::getType);
            return arrayType;
        }

        @Override
        public Cs.Try visitTry(Cs.Try try_, SenderContext ctx) {
            ctx.sendValue(try_, Cs.Try::getId);
            ctx.sendNode(try_, Cs.Try::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(try_, Cs.Try::getMarkers, ctx::sendMarkers);
            ctx.sendNode(try_, Cs.Try::getBody, ctx::sendTree);
            ctx.sendNodes(try_, Cs.Try::getCatches, ctx::sendTree, Tree::getId);
            ctx.sendNode(try_, e -> e.getPadding().getFinally(), CSharpSender::sendLeftPadded);
            return try_;
        }

        @Override
        public Cs.Try.Catch visitTryCatch(Cs.Try.Catch catch_, SenderContext ctx) {
            ctx.sendValue(catch_, Cs.Try.Catch::getId);
            ctx.sendNode(catch_, Cs.Try.Catch::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(catch_, Cs.Try.Catch::getMarkers, ctx::sendMarkers);
            ctx.sendNode(catch_, Cs.Try.Catch::getParameter, ctx::sendTree);
            ctx.sendNode(catch_, e -> e.getPadding().getFilterExpression(), CSharpSender::sendLeftPadded);
            ctx.sendNode(catch_, Cs.Try.Catch::getBody, ctx::sendTree);
            return catch_;
        }

        @Override
        public Cs.ArrowExpressionClause visitArrowExpressionClause(Cs.ArrowExpressionClause arrowExpressionClause, SenderContext ctx) {
            ctx.sendValue(arrowExpressionClause, Cs.ArrowExpressionClause::getId);
            ctx.sendNode(arrowExpressionClause, Cs.ArrowExpressionClause::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrowExpressionClause, Cs.ArrowExpressionClause::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrowExpressionClause, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            return arrowExpressionClause;
        }

        @Override
        public Cs.AccessorDeclaration visitAccessorDeclaration(Cs.AccessorDeclaration accessorDeclaration, SenderContext ctx) {
            ctx.sendValue(accessorDeclaration, Cs.AccessorDeclaration::getId);
            ctx.sendNode(accessorDeclaration, Cs.AccessorDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(accessorDeclaration, Cs.AccessorDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(accessorDeclaration, Cs.AccessorDeclaration::getAttributes, ctx::sendTree, Tree::getId);
            ctx.sendNodes(accessorDeclaration, Cs.AccessorDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(accessorDeclaration, e -> e.getPadding().getKind(), CSharpSender::sendLeftPadded);
            ctx.sendNode(accessorDeclaration, Cs.AccessorDeclaration::getExpressionBody, ctx::sendTree);
            ctx.sendNode(accessorDeclaration, Cs.AccessorDeclaration::getBody, ctx::sendTree);
            return accessorDeclaration;
        }

        @Override
        public Cs.PointerFieldAccess visitPointerFieldAccess(Cs.PointerFieldAccess pointerFieldAccess, SenderContext ctx) {
            ctx.sendValue(pointerFieldAccess, Cs.PointerFieldAccess::getId);
            ctx.sendNode(pointerFieldAccess, Cs.PointerFieldAccess::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(pointerFieldAccess, Cs.PointerFieldAccess::getMarkers, ctx::sendMarkers);
            ctx.sendNode(pointerFieldAccess, Cs.PointerFieldAccess::getTarget, ctx::sendTree);
            ctx.sendNode(pointerFieldAccess, e -> e.getPadding().getName(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(pointerFieldAccess, Cs.PointerFieldAccess::getType);
            return pointerFieldAccess;
        }

        @Override
        public J.AnnotatedType visitAnnotatedType(J.AnnotatedType annotatedType, SenderContext ctx) {
            ctx.sendValue(annotatedType, J.AnnotatedType::getId);
            ctx.sendNode(annotatedType, J.AnnotatedType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(annotatedType, J.AnnotatedType::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(annotatedType, J.AnnotatedType::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNode(annotatedType, J.AnnotatedType::getTypeExpression, ctx::sendTree);
            return annotatedType;
        }

        @Override
        public J.Annotation visitAnnotation(J.Annotation annotation, SenderContext ctx) {
            ctx.sendValue(annotation, J.Annotation::getId);
            ctx.sendNode(annotation, J.Annotation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(annotation, J.Annotation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(annotation, J.Annotation::getAnnotationType, ctx::sendTree);
            ctx.sendNode(annotation, e -> e.getPadding().getArguments(), CSharpSender::sendContainer);
            return annotation;
        }

        @Override
        public J.ArrayAccess visitArrayAccess(J.ArrayAccess arrayAccess, SenderContext ctx) {
            ctx.sendValue(arrayAccess, J.ArrayAccess::getId);
            ctx.sendNode(arrayAccess, J.ArrayAccess::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrayAccess, J.ArrayAccess::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrayAccess, J.ArrayAccess::getIndexed, ctx::sendTree);
            ctx.sendNode(arrayAccess, J.ArrayAccess::getDimension, ctx::sendTree);
            ctx.sendTypedValue(arrayAccess, J.ArrayAccess::getType);
            return arrayAccess;
        }

        @Override
        public J.ArrayType visitArrayType(J.ArrayType arrayType, SenderContext ctx) {
            ctx.sendValue(arrayType, J.ArrayType::getId);
            ctx.sendNode(arrayType, J.ArrayType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrayType, J.ArrayType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrayType, J.ArrayType::getElementType, ctx::sendTree);
            ctx.sendNodes(arrayType, J.ArrayType::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNode(arrayType, J.ArrayType::getDimension, CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(arrayType, J.ArrayType::getType);
            return arrayType;
        }

        @Override
        public J.Assert visitAssert(J.Assert assert_, SenderContext ctx) {
            ctx.sendValue(assert_, J.Assert::getId);
            ctx.sendNode(assert_, J.Assert::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(assert_, J.Assert::getMarkers, ctx::sendMarkers);
            ctx.sendNode(assert_, J.Assert::getCondition, ctx::sendTree);
            ctx.sendNode(assert_, J.Assert::getDetail, CSharpSender::sendLeftPadded);
            return assert_;
        }

        @Override
        public J.Assignment visitAssignment(J.Assignment assignment, SenderContext ctx) {
            ctx.sendValue(assignment, J.Assignment::getId);
            ctx.sendNode(assignment, J.Assignment::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(assignment, J.Assignment::getMarkers, ctx::sendMarkers);
            ctx.sendNode(assignment, J.Assignment::getVariable, ctx::sendTree);
            ctx.sendNode(assignment, e -> e.getPadding().getAssignment(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(assignment, J.Assignment::getType);
            return assignment;
        }

        @Override
        public J.AssignmentOperation visitAssignmentOperation(J.AssignmentOperation assignmentOperation, SenderContext ctx) {
            ctx.sendValue(assignmentOperation, J.AssignmentOperation::getId);
            ctx.sendNode(assignmentOperation, J.AssignmentOperation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(assignmentOperation, J.AssignmentOperation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(assignmentOperation, J.AssignmentOperation::getVariable, ctx::sendTree);
            ctx.sendNode(assignmentOperation, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(assignmentOperation, J.AssignmentOperation::getAssignment, ctx::sendTree);
            ctx.sendTypedValue(assignmentOperation, J.AssignmentOperation::getType);
            return assignmentOperation;
        }

        @Override
        public J.Binary visitBinary(J.Binary binary, SenderContext ctx) {
            ctx.sendValue(binary, J.Binary::getId);
            ctx.sendNode(binary, J.Binary::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(binary, J.Binary::getMarkers, ctx::sendMarkers);
            ctx.sendNode(binary, J.Binary::getLeft, ctx::sendTree);
            ctx.sendNode(binary, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(binary, J.Binary::getRight, ctx::sendTree);
            ctx.sendTypedValue(binary, J.Binary::getType);
            return binary;
        }

        @Override
        public J.Block visitBlock(J.Block block, SenderContext ctx) {
            ctx.sendValue(block, J.Block::getId);
            ctx.sendNode(block, J.Block::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(block, J.Block::getMarkers, ctx::sendMarkers);
            ctx.sendNode(block, e -> e.getPadding().getStatic(), CSharpSender::sendRightPadded);
            ctx.sendNodes(block, e -> e.getPadding().getStatements(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNode(block, J.Block::getEnd, CSharpSender::sendSpace);
            return block;
        }

        @Override
        public J.Break visitBreak(J.Break break_, SenderContext ctx) {
            ctx.sendValue(break_, J.Break::getId);
            ctx.sendNode(break_, J.Break::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(break_, J.Break::getMarkers, ctx::sendMarkers);
            ctx.sendNode(break_, J.Break::getLabel, ctx::sendTree);
            return break_;
        }

        @Override
        public J.Case visitCase(J.Case case_, SenderContext ctx) {
            ctx.sendValue(case_, J.Case::getId);
            ctx.sendNode(case_, J.Case::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(case_, J.Case::getMarkers, ctx::sendMarkers);
            ctx.sendValue(case_, J.Case::getType);
            ctx.sendNode(case_, e -> e.getPadding().getExpressions(), CSharpSender::sendContainer);
            ctx.sendNode(case_, e -> e.getPadding().getStatements(), CSharpSender::sendContainer);
            ctx.sendNode(case_, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return case_;
        }

        @Override
        public J.ClassDeclaration visitClassDeclaration(J.ClassDeclaration classDeclaration, SenderContext ctx) {
            ctx.sendValue(classDeclaration, J.ClassDeclaration::getId);
            ctx.sendNode(classDeclaration, J.ClassDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(classDeclaration, J.ClassDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(classDeclaration, J.ClassDeclaration::getLeadingAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNodes(classDeclaration, J.ClassDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getKind(), this::sendClassDeclarationKind);
            ctx.sendNode(classDeclaration, J.ClassDeclaration::getName, ctx::sendTree);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getPrimaryConstructor(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getExtends(), CSharpSender::sendLeftPadded);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getImplements(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, e -> e.getPadding().getPermits(), CSharpSender::sendContainer);
            ctx.sendNode(classDeclaration, J.ClassDeclaration::getBody, ctx::sendTree);
            ctx.sendTypedValue(classDeclaration, J.ClassDeclaration::getType);
            return classDeclaration;
        }

        private void sendClassDeclarationKind(J.ClassDeclaration.Kind kind, SenderContext ctx) {
            ctx.sendValue(kind, J.ClassDeclaration.Kind::getId);
            ctx.sendNode(kind, J.ClassDeclaration.Kind::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(kind, J.ClassDeclaration.Kind::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(kind, J.ClassDeclaration.Kind::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendValue(kind, J.ClassDeclaration.Kind::getType);
        }

        @Override
        public J.Continue visitContinue(J.Continue continue_, SenderContext ctx) {
            ctx.sendValue(continue_, J.Continue::getId);
            ctx.sendNode(continue_, J.Continue::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(continue_, J.Continue::getMarkers, ctx::sendMarkers);
            ctx.sendNode(continue_, J.Continue::getLabel, ctx::sendTree);
            return continue_;
        }

        @Override
        public J.DoWhileLoop visitDoWhileLoop(J.DoWhileLoop doWhileLoop, SenderContext ctx) {
            ctx.sendValue(doWhileLoop, J.DoWhileLoop::getId);
            ctx.sendNode(doWhileLoop, J.DoWhileLoop::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(doWhileLoop, J.DoWhileLoop::getMarkers, ctx::sendMarkers);
            ctx.sendNode(doWhileLoop, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            ctx.sendNode(doWhileLoop, e -> e.getPadding().getWhileCondition(), CSharpSender::sendLeftPadded);
            return doWhileLoop;
        }

        @Override
        public J.Empty visitEmpty(J.Empty empty, SenderContext ctx) {
            ctx.sendValue(empty, J.Empty::getId);
            ctx.sendNode(empty, J.Empty::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(empty, J.Empty::getMarkers, ctx::sendMarkers);
            return empty;
        }

        @Override
        public J.EnumValue visitEnumValue(J.EnumValue enumValue, SenderContext ctx) {
            ctx.sendValue(enumValue, J.EnumValue::getId);
            ctx.sendNode(enumValue, J.EnumValue::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(enumValue, J.EnumValue::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(enumValue, J.EnumValue::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNode(enumValue, J.EnumValue::getName, ctx::sendTree);
            ctx.sendNode(enumValue, J.EnumValue::getInitializer, ctx::sendTree);
            return enumValue;
        }

        @Override
        public J.EnumValueSet visitEnumValueSet(J.EnumValueSet enumValueSet, SenderContext ctx) {
            ctx.sendValue(enumValueSet, J.EnumValueSet::getId);
            ctx.sendNode(enumValueSet, J.EnumValueSet::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(enumValueSet, J.EnumValueSet::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(enumValueSet, e -> e.getPadding().getEnums(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendValue(enumValueSet, J.EnumValueSet::isTerminatedWithSemicolon);
            return enumValueSet;
        }

        @Override
        public J.FieldAccess visitFieldAccess(J.FieldAccess fieldAccess, SenderContext ctx) {
            ctx.sendValue(fieldAccess, J.FieldAccess::getId);
            ctx.sendNode(fieldAccess, J.FieldAccess::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(fieldAccess, J.FieldAccess::getMarkers, ctx::sendMarkers);
            ctx.sendNode(fieldAccess, J.FieldAccess::getTarget, ctx::sendTree);
            ctx.sendNode(fieldAccess, e -> e.getPadding().getName(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(fieldAccess, J.FieldAccess::getType);
            return fieldAccess;
        }

        @Override
        public J.ForEachLoop visitForEachLoop(J.ForEachLoop forEachLoop, SenderContext ctx) {
            ctx.sendValue(forEachLoop, J.ForEachLoop::getId);
            ctx.sendNode(forEachLoop, J.ForEachLoop::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(forEachLoop, J.ForEachLoop::getMarkers, ctx::sendMarkers);
            ctx.sendNode(forEachLoop, J.ForEachLoop::getControl, ctx::sendTree);
            ctx.sendNode(forEachLoop, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return forEachLoop;
        }

        @Override
        public J.ForEachLoop.Control visitForEachControl(J.ForEachLoop.Control control, SenderContext ctx) {
            ctx.sendValue(control, J.ForEachLoop.Control::getId);
            ctx.sendNode(control, J.ForEachLoop.Control::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(control, J.ForEachLoop.Control::getMarkers, ctx::sendMarkers);
            ctx.sendNode(control, e -> e.getPadding().getVariable(), CSharpSender::sendRightPadded);
            ctx.sendNode(control, e -> e.getPadding().getIterable(), CSharpSender::sendRightPadded);
            return control;
        }

        @Override
        public J.ForLoop visitForLoop(J.ForLoop forLoop, SenderContext ctx) {
            ctx.sendValue(forLoop, J.ForLoop::getId);
            ctx.sendNode(forLoop, J.ForLoop::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(forLoop, J.ForLoop::getMarkers, ctx::sendMarkers);
            ctx.sendNode(forLoop, J.ForLoop::getControl, ctx::sendTree);
            ctx.sendNode(forLoop, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return forLoop;
        }

        @Override
        public J.ForLoop.Control visitForControl(J.ForLoop.Control control, SenderContext ctx) {
            ctx.sendValue(control, J.ForLoop.Control::getId);
            ctx.sendNode(control, J.ForLoop.Control::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(control, J.ForLoop.Control::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(control, e -> e.getPadding().getInit(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            ctx.sendNode(control, e -> e.getPadding().getCondition(), CSharpSender::sendRightPadded);
            ctx.sendNodes(control, e -> e.getPadding().getUpdate(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return control;
        }

        @Override
        public J.ParenthesizedTypeTree visitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, SenderContext ctx) {
            ctx.sendValue(parenthesizedTypeTree, J.ParenthesizedTypeTree::getId);
            ctx.sendNode(parenthesizedTypeTree, J.ParenthesizedTypeTree::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parenthesizedTypeTree, J.ParenthesizedTypeTree::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(parenthesizedTypeTree, J.ParenthesizedTypeTree::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNode(parenthesizedTypeTree, J.ParenthesizedTypeTree::getParenthesizedType, ctx::sendTree);
            return parenthesizedTypeTree;
        }

        @Override
        public J.Identifier visitIdentifier(J.Identifier identifier, SenderContext ctx) {
            ctx.sendValue(identifier, J.Identifier::getId);
            ctx.sendNode(identifier, J.Identifier::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(identifier, J.Identifier::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(identifier, J.Identifier::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendValue(identifier, J.Identifier::getSimpleName);
            ctx.sendTypedValue(identifier, J.Identifier::getType);
            ctx.sendTypedValue(identifier, J.Identifier::getFieldType);
            return identifier;
        }

        @Override
        public J.If visitIf(J.If if_, SenderContext ctx) {
            ctx.sendValue(if_, J.If::getId);
            ctx.sendNode(if_, J.If::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(if_, J.If::getMarkers, ctx::sendMarkers);
            ctx.sendNode(if_, J.If::getIfCondition, ctx::sendTree);
            ctx.sendNode(if_, e -> e.getPadding().getThenPart(), CSharpSender::sendRightPadded);
            ctx.sendNode(if_, J.If::getElsePart, ctx::sendTree);
            return if_;
        }

        @Override
        public J.If.Else visitElse(J.If.Else else_, SenderContext ctx) {
            ctx.sendValue(else_, J.If.Else::getId);
            ctx.sendNode(else_, J.If.Else::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(else_, J.If.Else::getMarkers, ctx::sendMarkers);
            ctx.sendNode(else_, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return else_;
        }

        @Override
        public J.Import visitImport(J.Import import_, SenderContext ctx) {
            ctx.sendValue(import_, J.Import::getId);
            ctx.sendNode(import_, J.Import::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(import_, J.Import::getMarkers, ctx::sendMarkers);
            ctx.sendNode(import_, e -> e.getPadding().getStatic(), CSharpSender::sendLeftPadded);
            ctx.sendNode(import_, J.Import::getQualid, ctx::sendTree);
            ctx.sendNode(import_, e -> e.getPadding().getAlias(), CSharpSender::sendLeftPadded);
            return import_;
        }

        @Override
        public J.InstanceOf visitInstanceOf(J.InstanceOf instanceOf, SenderContext ctx) {
            ctx.sendValue(instanceOf, J.InstanceOf::getId);
            ctx.sendNode(instanceOf, J.InstanceOf::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(instanceOf, J.InstanceOf::getMarkers, ctx::sendMarkers);
            ctx.sendNode(instanceOf, e -> e.getPadding().getExpression(), CSharpSender::sendRightPadded);
            ctx.sendNode(instanceOf, J.InstanceOf::getClazz, ctx::sendTree);
            ctx.sendNode(instanceOf, J.InstanceOf::getPattern, ctx::sendTree);
            ctx.sendTypedValue(instanceOf, J.InstanceOf::getType);
            return instanceOf;
        }

        @Override
        public J.IntersectionType visitIntersectionType(J.IntersectionType intersectionType, SenderContext ctx) {
            ctx.sendValue(intersectionType, J.IntersectionType::getId);
            ctx.sendNode(intersectionType, J.IntersectionType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(intersectionType, J.IntersectionType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(intersectionType, e -> e.getPadding().getBounds(), CSharpSender::sendContainer);
            return intersectionType;
        }

        @Override
        public J.Label visitLabel(J.Label label, SenderContext ctx) {
            ctx.sendValue(label, J.Label::getId);
            ctx.sendNode(label, J.Label::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(label, J.Label::getMarkers, ctx::sendMarkers);
            ctx.sendNode(label, e -> e.getPadding().getLabel(), CSharpSender::sendRightPadded);
            ctx.sendNode(label, J.Label::getStatement, ctx::sendTree);
            return label;
        }

        @Override
        public J.Lambda visitLambda(J.Lambda lambda, SenderContext ctx) {
            ctx.sendValue(lambda, J.Lambda::getId);
            ctx.sendNode(lambda, J.Lambda::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(lambda, J.Lambda::getMarkers, ctx::sendMarkers);
            ctx.sendNode(lambda, J.Lambda::getParameters, this::sendLambdaParameters);
            ctx.sendNode(lambda, J.Lambda::getArrow, CSharpSender::sendSpace);
            ctx.sendNode(lambda, J.Lambda::getBody, ctx::sendTree);
            ctx.sendTypedValue(lambda, J.Lambda::getType);
            return lambda;
        }

        private void sendLambdaParameters(J.Lambda.Parameters parameters, SenderContext ctx) {
            ctx.sendValue(parameters, J.Lambda.Parameters::getId);
            ctx.sendNode(parameters, J.Lambda.Parameters::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parameters, J.Lambda.Parameters::getMarkers, ctx::sendMarkers);
            ctx.sendValue(parameters, J.Lambda.Parameters::isParenthesized);
            ctx.sendNodes(parameters, e -> e.getPadding().getParameters(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
        }

        @Override
        public J.Literal visitLiteral(J.Literal literal, SenderContext ctx) {
            ctx.sendValue(literal, J.Literal::getId);
            ctx.sendNode(literal, J.Literal::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(literal, J.Literal::getMarkers, ctx::sendMarkers);
            ctx.sendTypedValue(literal, J.Literal::getValue);
            ctx.sendValue(literal, J.Literal::getValueSource);
            ctx.sendValues(literal, J.Literal::getUnicodeEscapes, Function.identity());
            ctx.sendValue(literal, J.Literal::getType);
            return literal;
        }

        @Override
        public J.MemberReference visitMemberReference(J.MemberReference memberReference, SenderContext ctx) {
            ctx.sendValue(memberReference, J.MemberReference::getId);
            ctx.sendNode(memberReference, J.MemberReference::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(memberReference, J.MemberReference::getMarkers, ctx::sendMarkers);
            ctx.sendNode(memberReference, e -> e.getPadding().getContaining(), CSharpSender::sendRightPadded);
            ctx.sendNode(memberReference, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(memberReference, e -> e.getPadding().getReference(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(memberReference, J.MemberReference::getType);
            ctx.sendTypedValue(memberReference, J.MemberReference::getMethodType);
            ctx.sendTypedValue(memberReference, J.MemberReference::getVariableType);
            return memberReference;
        }

        @Override
        public J.MethodDeclaration visitMethodDeclaration(J.MethodDeclaration methodDeclaration, SenderContext ctx) {
            ctx.sendValue(methodDeclaration, J.MethodDeclaration::getId);
            ctx.sendNode(methodDeclaration, J.MethodDeclaration::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(methodDeclaration, J.MethodDeclaration::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(methodDeclaration, J.MethodDeclaration::getLeadingAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNodes(methodDeclaration, J.MethodDeclaration::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(methodDeclaration, e -> e.getAnnotations().getTypeParameters(), this::sendMethodTypeParameters);
            ctx.sendNode(methodDeclaration, J.MethodDeclaration::getReturnTypeExpression, ctx::sendTree);
            ctx.sendNode(methodDeclaration, e -> e.getAnnotations().getName(), this::sendMethodIdentifierWithAnnotations);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getParameters(), CSharpSender::sendContainer);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getThrows(), CSharpSender::sendContainer);
            ctx.sendNode(methodDeclaration, J.MethodDeclaration::getBody, ctx::sendTree);
            ctx.sendNode(methodDeclaration, e -> e.getPadding().getDefaultValue(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(methodDeclaration, J.MethodDeclaration::getMethodType);
            return methodDeclaration;
        }

        private void sendMethodIdentifierWithAnnotations(J.MethodDeclaration.IdentifierWithAnnotations identifierWithAnnotations, SenderContext ctx) {
            ctx.sendNode(identifierWithAnnotations, J.MethodDeclaration.IdentifierWithAnnotations::getIdentifier, ctx::sendTree);
            ctx.sendNodes(identifierWithAnnotations, J.MethodDeclaration.IdentifierWithAnnotations::getAnnotations, ctx::sendTree, Tree::getId);
        }

        @Override
        public J.MethodInvocation visitMethodInvocation(J.MethodInvocation methodInvocation, SenderContext ctx) {
            ctx.sendValue(methodInvocation, J.MethodInvocation::getId);
            ctx.sendNode(methodInvocation, J.MethodInvocation::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(methodInvocation, J.MethodInvocation::getMarkers, ctx::sendMarkers);
            ctx.sendNode(methodInvocation, e -> e.getPadding().getSelect(), CSharpSender::sendRightPadded);
            ctx.sendNode(methodInvocation, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendNode(methodInvocation, J.MethodInvocation::getName, ctx::sendTree);
            ctx.sendNode(methodInvocation, e -> e.getPadding().getArguments(), CSharpSender::sendContainer);
            ctx.sendTypedValue(methodInvocation, J.MethodInvocation::getMethodType);
            return methodInvocation;
        }

        @Override
        public J.Modifier visitModifier(J.Modifier modifier, SenderContext ctx) {
            ctx.sendValue(modifier, J.Modifier::getId);
            ctx.sendNode(modifier, J.Modifier::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(modifier, J.Modifier::getMarkers, ctx::sendMarkers);
            ctx.sendValue(modifier, J.Modifier::getKeyword);
            ctx.sendValue(modifier, J.Modifier::getType);
            ctx.sendNodes(modifier, J.Modifier::getAnnotations, ctx::sendTree, Tree::getId);
            return modifier;
        }

        @Override
        public J.MultiCatch visitMultiCatch(J.MultiCatch multiCatch, SenderContext ctx) {
            ctx.sendValue(multiCatch, J.MultiCatch::getId);
            ctx.sendNode(multiCatch, J.MultiCatch::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(multiCatch, J.MultiCatch::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(multiCatch, e -> e.getPadding().getAlternatives(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return multiCatch;
        }

        @Override
        public J.NewArray visitNewArray(J.NewArray newArray, SenderContext ctx) {
            ctx.sendValue(newArray, J.NewArray::getId);
            ctx.sendNode(newArray, J.NewArray::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(newArray, J.NewArray::getMarkers, ctx::sendMarkers);
            ctx.sendNode(newArray, J.NewArray::getTypeExpression, ctx::sendTree);
            ctx.sendNodes(newArray, J.NewArray::getDimensions, ctx::sendTree, Tree::getId);
            ctx.sendNode(newArray, e -> e.getPadding().getInitializer(), CSharpSender::sendContainer);
            ctx.sendTypedValue(newArray, J.NewArray::getType);
            return newArray;
        }

        @Override
        public J.ArrayDimension visitArrayDimension(J.ArrayDimension arrayDimension, SenderContext ctx) {
            ctx.sendValue(arrayDimension, J.ArrayDimension::getId);
            ctx.sendNode(arrayDimension, J.ArrayDimension::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(arrayDimension, J.ArrayDimension::getMarkers, ctx::sendMarkers);
            ctx.sendNode(arrayDimension, e -> e.getPadding().getIndex(), CSharpSender::sendRightPadded);
            return arrayDimension;
        }

        @Override
        public J.NewClass visitNewClass(J.NewClass newClass, SenderContext ctx) {
            ctx.sendValue(newClass, J.NewClass::getId);
            ctx.sendNode(newClass, J.NewClass::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(newClass, J.NewClass::getMarkers, ctx::sendMarkers);
            ctx.sendNode(newClass, e -> e.getPadding().getEnclosing(), CSharpSender::sendRightPadded);
            ctx.sendNode(newClass, J.NewClass::getNew, CSharpSender::sendSpace);
            ctx.sendNode(newClass, J.NewClass::getClazz, ctx::sendTree);
            ctx.sendNode(newClass, e -> e.getPadding().getArguments(), CSharpSender::sendContainer);
            ctx.sendNode(newClass, J.NewClass::getBody, ctx::sendTree);
            ctx.sendTypedValue(newClass, J.NewClass::getConstructorType);
            return newClass;
        }

        @Override
        public J.NullableType visitNullableType(J.NullableType nullableType, SenderContext ctx) {
            ctx.sendValue(nullableType, J.NullableType::getId);
            ctx.sendNode(nullableType, J.NullableType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(nullableType, J.NullableType::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(nullableType, J.NullableType::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNode(nullableType, e -> e.getPadding().getTypeTree(), CSharpSender::sendRightPadded);
            return nullableType;
        }

        @Override
        public J.Package visitPackage(J.Package package_, SenderContext ctx) {
            ctx.sendValue(package_, J.Package::getId);
            ctx.sendNode(package_, J.Package::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(package_, J.Package::getMarkers, ctx::sendMarkers);
            ctx.sendNode(package_, J.Package::getExpression, ctx::sendTree);
            ctx.sendNodes(package_, J.Package::getAnnotations, ctx::sendTree, Tree::getId);
            return package_;
        }

        @Override
        public J.ParameterizedType visitParameterizedType(J.ParameterizedType parameterizedType, SenderContext ctx) {
            ctx.sendValue(parameterizedType, J.ParameterizedType::getId);
            ctx.sendNode(parameterizedType, J.ParameterizedType::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parameterizedType, J.ParameterizedType::getMarkers, ctx::sendMarkers);
            ctx.sendNode(parameterizedType, J.ParameterizedType::getClazz, ctx::sendTree);
            ctx.sendNode(parameterizedType, e -> e.getPadding().getTypeParameters(), CSharpSender::sendContainer);
            ctx.sendTypedValue(parameterizedType, J.ParameterizedType::getType);
            return parameterizedType;
        }

        @Override
        public <J2 extends J> J.Parentheses<J2> visitParentheses(J.Parentheses<J2> parentheses, SenderContext ctx) {
            ctx.sendValue(parentheses, J.Parentheses::getId);
            ctx.sendNode(parentheses, J.Parentheses::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(parentheses, J.Parentheses::getMarkers, ctx::sendMarkers);
            ctx.sendNode(parentheses, e -> e.getPadding().getTree(), CSharpSender::sendRightPadded);
            return parentheses;
        }

        @Override
        public <J2 extends J> J.ControlParentheses<J2> visitControlParentheses(J.ControlParentheses<J2> controlParentheses, SenderContext ctx) {
            ctx.sendValue(controlParentheses, J.ControlParentheses::getId);
            ctx.sendNode(controlParentheses, J.ControlParentheses::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(controlParentheses, J.ControlParentheses::getMarkers, ctx::sendMarkers);
            ctx.sendNode(controlParentheses, e -> e.getPadding().getTree(), CSharpSender::sendRightPadded);
            return controlParentheses;
        }

        @Override
        public J.Primitive visitPrimitive(J.Primitive primitive, SenderContext ctx) {
            ctx.sendValue(primitive, J.Primitive::getId);
            ctx.sendNode(primitive, J.Primitive::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(primitive, J.Primitive::getMarkers, ctx::sendMarkers);
            ctx.sendValue(primitive, J.Primitive::getType);
            return primitive;
        }

        @Override
        public J.Return visitReturn(J.Return return_, SenderContext ctx) {
            ctx.sendValue(return_, J.Return::getId);
            ctx.sendNode(return_, J.Return::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(return_, J.Return::getMarkers, ctx::sendMarkers);
            ctx.sendNode(return_, J.Return::getExpression, ctx::sendTree);
            return return_;
        }

        @Override
        public J.Switch visitSwitch(J.Switch switch_, SenderContext ctx) {
            ctx.sendValue(switch_, J.Switch::getId);
            ctx.sendNode(switch_, J.Switch::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switch_, J.Switch::getMarkers, ctx::sendMarkers);
            ctx.sendNode(switch_, J.Switch::getSelector, ctx::sendTree);
            ctx.sendNode(switch_, J.Switch::getCases, ctx::sendTree);
            return switch_;
        }

        @Override
        public J.SwitchExpression visitSwitchExpression(J.SwitchExpression switchExpression, SenderContext ctx) {
            ctx.sendValue(switchExpression, J.SwitchExpression::getId);
            ctx.sendNode(switchExpression, J.SwitchExpression::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(switchExpression, J.SwitchExpression::getMarkers, ctx::sendMarkers);
            ctx.sendNode(switchExpression, J.SwitchExpression::getSelector, ctx::sendTree);
            ctx.sendNode(switchExpression, J.SwitchExpression::getCases, ctx::sendTree);
            return switchExpression;
        }

        @Override
        public J.Synchronized visitSynchronized(J.Synchronized synchronized_, SenderContext ctx) {
            ctx.sendValue(synchronized_, J.Synchronized::getId);
            ctx.sendNode(synchronized_, J.Synchronized::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(synchronized_, J.Synchronized::getMarkers, ctx::sendMarkers);
            ctx.sendNode(synchronized_, J.Synchronized::getLock, ctx::sendTree);
            ctx.sendNode(synchronized_, J.Synchronized::getBody, ctx::sendTree);
            return synchronized_;
        }

        @Override
        public J.Ternary visitTernary(J.Ternary ternary, SenderContext ctx) {
            ctx.sendValue(ternary, J.Ternary::getId);
            ctx.sendNode(ternary, J.Ternary::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(ternary, J.Ternary::getMarkers, ctx::sendMarkers);
            ctx.sendNode(ternary, J.Ternary::getCondition, ctx::sendTree);
            ctx.sendNode(ternary, e -> e.getPadding().getTruePart(), CSharpSender::sendLeftPadded);
            ctx.sendNode(ternary, e -> e.getPadding().getFalsePart(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(ternary, J.Ternary::getType);
            return ternary;
        }

        @Override
        public J.Throw visitThrow(J.Throw throw_, SenderContext ctx) {
            ctx.sendValue(throw_, J.Throw::getId);
            ctx.sendNode(throw_, J.Throw::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(throw_, J.Throw::getMarkers, ctx::sendMarkers);
            ctx.sendNode(throw_, J.Throw::getException, ctx::sendTree);
            return throw_;
        }

        @Override
        public J.Try visitTry(J.Try try_, SenderContext ctx) {
            ctx.sendValue(try_, J.Try::getId);
            ctx.sendNode(try_, J.Try::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(try_, J.Try::getMarkers, ctx::sendMarkers);
            ctx.sendNode(try_, e -> e.getPadding().getResources(), CSharpSender::sendContainer);
            ctx.sendNode(try_, J.Try::getBody, ctx::sendTree);
            ctx.sendNodes(try_, J.Try::getCatches, ctx::sendTree, Tree::getId);
            ctx.sendNode(try_, e -> e.getPadding().getFinally(), CSharpSender::sendLeftPadded);
            return try_;
        }

        @Override
        public J.Try.Resource visitTryResource(J.Try.Resource resource, SenderContext ctx) {
            ctx.sendValue(resource, J.Try.Resource::getId);
            ctx.sendNode(resource, J.Try.Resource::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(resource, J.Try.Resource::getMarkers, ctx::sendMarkers);
            ctx.sendNode(resource, J.Try.Resource::getVariableDeclarations, ctx::sendTree);
            ctx.sendValue(resource, J.Try.Resource::isTerminatedWithSemicolon);
            return resource;
        }

        @Override
        public J.Try.Catch visitCatch(J.Try.Catch catch_, SenderContext ctx) {
            ctx.sendValue(catch_, J.Try.Catch::getId);
            ctx.sendNode(catch_, J.Try.Catch::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(catch_, J.Try.Catch::getMarkers, ctx::sendMarkers);
            ctx.sendNode(catch_, J.Try.Catch::getParameter, ctx::sendTree);
            ctx.sendNode(catch_, J.Try.Catch::getBody, ctx::sendTree);
            return catch_;
        }

        @Override
        public J.TypeCast visitTypeCast(J.TypeCast typeCast, SenderContext ctx) {
            ctx.sendValue(typeCast, J.TypeCast::getId);
            ctx.sendNode(typeCast, J.TypeCast::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeCast, J.TypeCast::getMarkers, ctx::sendMarkers);
            ctx.sendNode(typeCast, J.TypeCast::getClazz, ctx::sendTree);
            ctx.sendNode(typeCast, J.TypeCast::getExpression, ctx::sendTree);
            return typeCast;
        }

        @Override
        public J.TypeParameter visitTypeParameter(J.TypeParameter typeParameter, SenderContext ctx) {
            ctx.sendValue(typeParameter, J.TypeParameter::getId);
            ctx.sendNode(typeParameter, J.TypeParameter::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeParameter, J.TypeParameter::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(typeParameter, J.TypeParameter::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNodes(typeParameter, J.TypeParameter::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(typeParameter, J.TypeParameter::getName, ctx::sendTree);
            ctx.sendNode(typeParameter, e -> e.getPadding().getBounds(), CSharpSender::sendContainer);
            return typeParameter;
        }

        private void sendMethodTypeParameters(J.TypeParameters typeParameters, SenderContext ctx) {
            ctx.sendValue(typeParameters, J.TypeParameters::getId);
            ctx.sendNode(typeParameters, J.TypeParameters::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(typeParameters, J.TypeParameters::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(typeParameters, J.TypeParameters::getAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNodes(typeParameters, e -> e.getPadding().getTypeParameters(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
        }

        @Override
        public J.Unary visitUnary(J.Unary unary, SenderContext ctx) {
            ctx.sendValue(unary, J.Unary::getId);
            ctx.sendNode(unary, J.Unary::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(unary, J.Unary::getMarkers, ctx::sendMarkers);
            ctx.sendNode(unary, e -> e.getPadding().getOperator(), CSharpSender::sendLeftPadded);
            ctx.sendNode(unary, J.Unary::getExpression, ctx::sendTree);
            ctx.sendTypedValue(unary, J.Unary::getType);
            return unary;
        }

        @Override
        public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations variableDeclarations, SenderContext ctx) {
            ctx.sendValue(variableDeclarations, J.VariableDeclarations::getId);
            ctx.sendNode(variableDeclarations, J.VariableDeclarations::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(variableDeclarations, J.VariableDeclarations::getMarkers, ctx::sendMarkers);
            ctx.sendNodes(variableDeclarations, J.VariableDeclarations::getLeadingAnnotations, ctx::sendTree, Tree::getId);
            ctx.sendNodes(variableDeclarations, J.VariableDeclarations::getModifiers, ctx::sendTree, Tree::getId);
            ctx.sendNode(variableDeclarations, J.VariableDeclarations::getTypeExpression, ctx::sendTree);
            ctx.sendNode(variableDeclarations, J.VariableDeclarations::getVarargs, CSharpSender::sendSpace);
            ctx.sendNodes(variableDeclarations, J.VariableDeclarations::getDimensionsBeforeName, CSharpSender::sendLeftPadded, Function.identity());
            ctx.sendNodes(variableDeclarations, e -> e.getPadding().getVariables(), CSharpSender::sendRightPadded, e -> e.getElement().getId());
            return variableDeclarations;
        }

        @Override
        public J.VariableDeclarations.NamedVariable visitVariable(J.VariableDeclarations.NamedVariable namedVariable, SenderContext ctx) {
            ctx.sendValue(namedVariable, J.VariableDeclarations.NamedVariable::getId);
            ctx.sendNode(namedVariable, J.VariableDeclarations.NamedVariable::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(namedVariable, J.VariableDeclarations.NamedVariable::getMarkers, ctx::sendMarkers);
            ctx.sendNode(namedVariable, J.VariableDeclarations.NamedVariable::getName, ctx::sendTree);
            ctx.sendNodes(namedVariable, J.VariableDeclarations.NamedVariable::getDimensionsAfterName, CSharpSender::sendLeftPadded, Function.identity());
            ctx.sendNode(namedVariable, e -> e.getPadding().getInitializer(), CSharpSender::sendLeftPadded);
            ctx.sendTypedValue(namedVariable, J.VariableDeclarations.NamedVariable::getVariableType);
            return namedVariable;
        }

        @Override
        public J.WhileLoop visitWhileLoop(J.WhileLoop whileLoop, SenderContext ctx) {
            ctx.sendValue(whileLoop, J.WhileLoop::getId);
            ctx.sendNode(whileLoop, J.WhileLoop::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(whileLoop, J.WhileLoop::getMarkers, ctx::sendMarkers);
            ctx.sendNode(whileLoop, J.WhileLoop::getCondition, ctx::sendTree);
            ctx.sendNode(whileLoop, e -> e.getPadding().getBody(), CSharpSender::sendRightPadded);
            return whileLoop;
        }

        @Override
        public J.Wildcard visitWildcard(J.Wildcard wildcard, SenderContext ctx) {
            ctx.sendValue(wildcard, J.Wildcard::getId);
            ctx.sendNode(wildcard, J.Wildcard::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(wildcard, J.Wildcard::getMarkers, ctx::sendMarkers);
            ctx.sendNode(wildcard, e -> e.getPadding().getBound(), CSharpSender::sendLeftPadded);
            ctx.sendNode(wildcard, J.Wildcard::getBoundedType, ctx::sendTree);
            return wildcard;
        }

        @Override
        public J.Yield visitYield(J.Yield yield, SenderContext ctx) {
            ctx.sendValue(yield, J.Yield::getId);
            ctx.sendNode(yield, J.Yield::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(yield, J.Yield::getMarkers, ctx::sendMarkers);
            ctx.sendValue(yield, J.Yield::isImplicit);
            ctx.sendNode(yield, J.Yield::getValue, ctx::sendTree);
            return yield;
        }

        @Override
        public J.Unknown visitUnknown(J.Unknown unknown, SenderContext ctx) {
            ctx.sendValue(unknown, J.Unknown::getId);
            ctx.sendNode(unknown, J.Unknown::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(unknown, J.Unknown::getMarkers, ctx::sendMarkers);
            ctx.sendNode(unknown, J.Unknown::getSource, ctx::sendTree);
            return unknown;
        }

        @Override
        public J.Unknown.Source visitUnknownSource(J.Unknown.Source source, SenderContext ctx) {
            ctx.sendValue(source, J.Unknown.Source::getId);
            ctx.sendNode(source, J.Unknown.Source::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(source, J.Unknown.Source::getMarkers, ctx::sendMarkers);
            ctx.sendValue(source, J.Unknown.Source::getText);
            return source;
        }

    }

    private static <T extends J> void sendContainer(JContainer<T> container, SenderContext ctx) {
        Extensions.sendContainer(container, ctx);
    }

    private static <T> void sendLeftPadded(JLeftPadded<T> leftPadded, SenderContext ctx) {
        Extensions.sendLeftPadded(leftPadded, ctx);
    }

    private static <T> void sendRightPadded(JRightPadded<T> rightPadded, SenderContext ctx) {
        Extensions.sendRightPadded(rightPadded, ctx);
    }

    private static void sendSpace(Space space, SenderContext ctx) {
        Extensions.sendSpace(space, ctx);
    }

    private static void sendComment(Comment comment, SenderContext ctx) {
        Extensions.sendComment(comment, ctx);
    }

}
