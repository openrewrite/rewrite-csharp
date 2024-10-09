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
        public Cs.NamedArgument visitNamedArgument(Cs.NamedArgument namedArgument, SenderContext ctx) {
            ctx.sendValue(namedArgument, Cs.NamedArgument::getId);
            ctx.sendNode(namedArgument, Cs.NamedArgument::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(namedArgument, Cs.NamedArgument::getMarkers, ctx::sendMarkers);
            ctx.sendNode(namedArgument, e -> e.getPadding().getNameColumn(), CSharpSender::sendRightPadded);
            ctx.sendNode(namedArgument, Cs.NamedArgument::getExpression, ctx::sendTree);
            return namedArgument;
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
            ctx.sendNode(expressionStatement, Cs.ExpressionStatement::getExpression, ctx::sendTree);
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
            ctx.sendNodes(propertyDeclaration, Cs.PropertyDeclaration::getModifiers, this::sendModifier, Tree::getId);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getTypeExpression, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, e -> e.getPadding().getInterfaceSpecifier(), CSharpSender::sendRightPadded);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getName, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, Cs.PropertyDeclaration::getAccessors, ctx::sendTree);
            ctx.sendNode(propertyDeclaration, e -> e.getPadding().getInitializer(), CSharpSender::sendLeftPadded);
            return propertyDeclaration;
        }

        @Override
        public Cs.Lambda visitLambda(Cs.Lambda lambda, SenderContext ctx) {
            ctx.sendValue(lambda, Cs.Lambda::getId);
            ctx.sendNode(lambda, Cs.Lambda::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(lambda, Cs.Lambda::getMarkers, ctx::sendMarkers);
            ctx.sendNode(lambda, Cs.Lambda::getLambdaExpression, ctx::sendTree);
            ctx.sendNodes(lambda, Cs.Lambda::getModifiers, this::sendModifier, Tree::getId);
            return lambda;
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
            ctx.sendNodes(classDeclaration, J.ClassDeclaration::getModifiers, this::sendModifier, Tree::getId);
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
            ctx.sendNodes(methodDeclaration, J.MethodDeclaration::getModifiers, this::sendModifier, Tree::getId);
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

        private void sendModifier(J.Modifier modifier, SenderContext ctx) {
            ctx.sendValue(modifier, J.Modifier::getId);
            ctx.sendNode(modifier, J.Modifier::getPrefix, CSharpSender::sendSpace);
            ctx.sendNode(modifier, J.Modifier::getMarkers, ctx::sendMarkers);
            ctx.sendValue(modifier, J.Modifier::getKeyword);
            ctx.sendValue(modifier, J.Modifier::getType);
            ctx.sendNodes(modifier, J.Modifier::getAnnotations, ctx::sendTree, Tree::getId);
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
            ctx.sendNodes(typeParameter, J.TypeParameter::getModifiers, this::sendModifier, Tree::getId);
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
            ctx.sendNodes(variableDeclarations, J.VariableDeclarations::getModifiers, this::sendModifier, Tree::getId);
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