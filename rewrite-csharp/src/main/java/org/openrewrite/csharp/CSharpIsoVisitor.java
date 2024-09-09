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

import org.openrewrite.csharp.tree.*;
import org.openrewrite.java.tree.*;

public class CSharpIsoVisitor<P> extends CSharpVisitor<P>
{
    @Override
    public Cs.CompilationUnit visitCompilationUnit(Cs.CompilationUnit compilationUnit, P p) {
        return (Cs.CompilationUnit) super.visitCompilationUnit(compilationUnit, p);
    }

    @Override
    public Cs.AnnotatedStatement visitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, P p) {
        return (Cs.AnnotatedStatement) super.visitAnnotatedStatement(annotatedStatement, p);
    }

    @Override
    public Cs.ArrayRankSpecifier visitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, P p) {
        return (Cs.ArrayRankSpecifier) super.visitArrayRankSpecifier(arrayRankSpecifier, p);
    }

    @Override
    public Cs.AssignmentOperation visitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, P p) {
        return (Cs.AssignmentOperation) super.visitAssignmentOperation(assignmentOperation, p);
    }

    @Override
    public Cs.AttributeList visitAttributeList(Cs.AttributeList attributeList, P p) {
        return (Cs.AttributeList) super.visitAttributeList(attributeList, p);
    }

    @Override
    public Cs.AwaitExpression visitAwaitExpression(Cs.AwaitExpression awaitExpression, P p) {
        return (Cs.AwaitExpression) super.visitAwaitExpression(awaitExpression, p);
    }

    @Override
    public Cs.Binary visitBinary(Cs.Binary binary, P p) {
        return (Cs.Binary) super.visitBinary(binary, p);
    }

    @Override
    public Cs.BlockScopeNamespaceDeclaration visitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, P p) {
        return (Cs.BlockScopeNamespaceDeclaration) super.visitBlockScopeNamespaceDeclaration(blockScopeNamespaceDeclaration, p);
    }

    @Override
    public Cs.CollectionExpression visitCollectionExpression(Cs.CollectionExpression collectionExpression, P p) {
        return (Cs.CollectionExpression) super.visitCollectionExpression(collectionExpression, p);
    }

    @Override
    public Cs.ExpressionStatement visitExpressionStatement(Cs.ExpressionStatement expressionStatement, P p) {
        return (Cs.ExpressionStatement) super.visitExpressionStatement(expressionStatement, p);
    }

    @Override
    public Cs.ExternAlias visitExternAlias(Cs.ExternAlias externAlias, P p) {
        return (Cs.ExternAlias) super.visitExternAlias(externAlias, p);
    }

    @Override
    public Cs.FileScopeNamespaceDeclaration visitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, P p) {
        return (Cs.FileScopeNamespaceDeclaration) super.visitFileScopeNamespaceDeclaration(fileScopeNamespaceDeclaration, p);
    }

    @Override
    public Cs.InterpolatedString visitInterpolatedString(Cs.InterpolatedString interpolatedString, P p) {
        return (Cs.InterpolatedString) super.visitInterpolatedString(interpolatedString, p);
    }

    @Override
    public Cs.Interpolation visitInterpolation(Cs.Interpolation interpolation, P p) {
        return (Cs.Interpolation) super.visitInterpolation(interpolation, p);
    }

    @Override
    public Cs.NullSafeExpression visitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, P p) {
        return (Cs.NullSafeExpression) super.visitNullSafeExpression(nullSafeExpression, p);
    }

    @Override
    public Cs.StatementExpression visitStatementExpression(Cs.StatementExpression statementExpression, P p) {
        return (Cs.StatementExpression) super.visitStatementExpression(statementExpression, p);
    }

    @Override
    public Cs.UsingDirective visitUsingDirective(Cs.UsingDirective usingDirective, P p) {
        return (Cs.UsingDirective) super.visitUsingDirective(usingDirective, p);
    }

    @Override
    public Cs.PropertyDeclaration visitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, P p) {
        return (Cs.PropertyDeclaration) super.visitPropertyDeclaration(propertyDeclaration, p);
    }

    @Override
    public J.AnnotatedType visitAnnotatedType(J.AnnotatedType annotatedType, P p) {
        return (J.AnnotatedType) super.visitAnnotatedType(annotatedType, p);
    }

    @Override
    public J.Annotation visitAnnotation(J.Annotation annotation, P p) {
        return (J.Annotation) super.visitAnnotation(annotation, p);
    }

    @Override
    public J.ArrayAccess visitArrayAccess(J.ArrayAccess arrayAccess, P p) {
        return (J.ArrayAccess) super.visitArrayAccess(arrayAccess, p);
    }

    @Override
    public J.ArrayType visitArrayType(J.ArrayType arrayType, P p) {
        return (J.ArrayType) super.visitArrayType(arrayType, p);
    }

    @Override
    public J.Assert visitAssert(J.Assert assert_, P p) {
        return (J.Assert) super.visitAssert(assert_, p);
    }

    @Override
    public J.Assignment visitAssignment(J.Assignment assignment, P p) {
        return (J.Assignment) super.visitAssignment(assignment, p);
    }

    @Override
    public J.AssignmentOperation visitAssignmentOperation(J.AssignmentOperation assignmentOperation, P p) {
        return (J.AssignmentOperation) super.visitAssignmentOperation(assignmentOperation, p);
    }

    @Override
    public J.Binary visitBinary(J.Binary binary, P p) {
        return (J.Binary) super.visitBinary(binary, p);
    }

    @Override
    public J.Block visitBlock(J.Block block, P p) {
        return (J.Block) super.visitBlock(block, p);
    }

    @Override
    public J.Break visitBreak(J.Break break_, P p) {
        return (J.Break) super.visitBreak(break_, p);
    }

    @Override
    public J.Case visitCase(J.Case case_, P p) {
        return (J.Case) super.visitCase(case_, p);
    }

    @Override
    public J.ClassDeclaration visitClassDeclaration(J.ClassDeclaration classDeclaration, P p) {
        return (J.ClassDeclaration) super.visitClassDeclaration(classDeclaration, p);
    }

    @Override
    public J.Continue visitContinue(J.Continue continue_, P p) {
        return (J.Continue) super.visitContinue(continue_, p);
    }

    @Override
    public J.DoWhileLoop visitDoWhileLoop(J.DoWhileLoop doWhileLoop, P p) {
        return (J.DoWhileLoop) super.visitDoWhileLoop(doWhileLoop, p);
    }

    @Override
    public J.Empty visitEmpty(J.Empty empty, P p) {
        return (J.Empty) super.visitEmpty(empty, p);
    }

    @Override
    public J.EnumValue visitEnumValue(J.EnumValue enumValue, P p) {
        return (J.EnumValue) super.visitEnumValue(enumValue, p);
    }

    @Override
    public J.EnumValueSet visitEnumValueSet(J.EnumValueSet enumValueSet, P p) {
        return (J.EnumValueSet) super.visitEnumValueSet(enumValueSet, p);
    }

    @Override
    public J.FieldAccess visitFieldAccess(J.FieldAccess fieldAccess, P p) {
        return (J.FieldAccess) super.visitFieldAccess(fieldAccess, p);
    }

    @Override
    public J.ForEachLoop visitForEachLoop(J.ForEachLoop forEachLoop, P p) {
        return (J.ForEachLoop) super.visitForEachLoop(forEachLoop, p);
    }

    @Override
    public J.ForLoop visitForLoop(J.ForLoop forLoop, P p) {
        return (J.ForLoop) super.visitForLoop(forLoop, p);
    }

    @Override
    public J.ParenthesizedTypeTree visitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, P p) {
        return (J.ParenthesizedTypeTree) super.visitParenthesizedTypeTree(parenthesizedTypeTree, p);
    }

    @Override
    public J.Identifier visitIdentifier(J.Identifier identifier, P p) {
        return (J.Identifier) super.visitIdentifier(identifier, p);
    }

    @Override
    public J.If visitIf(J.If if_, P p) {
        return (J.If) super.visitIf(if_, p);
    }

    @Override
    public J.Import visitImport(J.Import import_, P p) {
        return (J.Import) super.visitImport(import_, p);
    }

    @Override
    public J.InstanceOf visitInstanceOf(J.InstanceOf instanceOf, P p) {
        return (J.InstanceOf) super.visitInstanceOf(instanceOf, p);
    }

    @Override
    public J.IntersectionType visitIntersectionType(J.IntersectionType intersectionType, P p) {
        return (J.IntersectionType) super.visitIntersectionType(intersectionType, p);
    }

    @Override
    public J.Label visitLabel(J.Label label, P p) {
        return (J.Label) super.visitLabel(label, p);
    }

    @Override
    public J.Lambda visitLambda(J.Lambda lambda, P p) {
        return (J.Lambda) super.visitLambda(lambda, p);
    }

    @Override
    public J.Literal visitLiteral(J.Literal literal, P p) {
        return (J.Literal) super.visitLiteral(literal, p);
    }

    @Override
    public J.MemberReference visitMemberReference(J.MemberReference memberReference, P p) {
        return (J.MemberReference) super.visitMemberReference(memberReference, p);
    }

    @Override
    public J.MethodDeclaration visitMethodDeclaration(J.MethodDeclaration methodDeclaration, P p) {
        return (J.MethodDeclaration) super.visitMethodDeclaration(methodDeclaration, p);
    }

    @Override
    public J.MethodInvocation visitMethodInvocation(J.MethodInvocation methodInvocation, P p) {
        return (J.MethodInvocation) super.visitMethodInvocation(methodInvocation, p);
    }

    @Override
    public J.MultiCatch visitMultiCatch(J.MultiCatch multiCatch, P p) {
        return (J.MultiCatch) super.visitMultiCatch(multiCatch, p);
    }

    @Override
    public J.NewArray visitNewArray(J.NewArray newArray, P p) {
        return (J.NewArray) super.visitNewArray(newArray, p);
    }

    @Override
    public J.ArrayDimension visitArrayDimension(J.ArrayDimension arrayDimension, P p) {
        return (J.ArrayDimension) super.visitArrayDimension(arrayDimension, p);
    }

    @Override
    public J.NewClass visitNewClass(J.NewClass newClass, P p) {
        return (J.NewClass) super.visitNewClass(newClass, p);
    }

    @Override
    public J.NullableType visitNullableType(J.NullableType nullableType, P p) {
        return (J.NullableType) super.visitNullableType(nullableType, p);
    }

    @Override
    public J.Package visitPackage(J.Package package_, P p) {
        return (J.Package) super.visitPackage(package_, p);
    }

    @Override
    public J.ParameterizedType visitParameterizedType(J.ParameterizedType parameterizedType, P p) {
        return (J.ParameterizedType) super.visitParameterizedType(parameterizedType, p);
    }

    @Override
    public <J2 extends J> J.Parentheses<J2> visitParentheses(J.Parentheses<J2> parentheses, P p) {
        return (J.Parentheses<J2>) super.visitParentheses(parentheses, p);
    }

    @Override
    public <J2 extends J> J.ControlParentheses<J2> visitControlParentheses(J.ControlParentheses<J2> controlParentheses, P p) {
        return (J.ControlParentheses<J2>) super.visitControlParentheses(controlParentheses, p);
    }

    @Override
    public J.Primitive visitPrimitive(J.Primitive primitive, P p) {
        return (J.Primitive) super.visitPrimitive(primitive, p);
    }

    @Override
    public J.Return visitReturn(J.Return return_, P p) {
        return (J.Return) super.visitReturn(return_, p);
    }

    @Override
    public J.Switch visitSwitch(J.Switch switch_, P p) {
        return (J.Switch) super.visitSwitch(switch_, p);
    }

    @Override
    public J.SwitchExpression visitSwitchExpression(J.SwitchExpression switchExpression, P p) {
        return (J.SwitchExpression) super.visitSwitchExpression(switchExpression, p);
    }

    @Override
    public J.Synchronized visitSynchronized(J.Synchronized synchronized_, P p) {
        return (J.Synchronized) super.visitSynchronized(synchronized_, p);
    }

    @Override
    public J.Ternary visitTernary(J.Ternary ternary, P p) {
        return (J.Ternary) super.visitTernary(ternary, p);
    }

    @Override
    public J.Throw visitThrow(J.Throw throw_, P p) {
        return (J.Throw) super.visitThrow(throw_, p);
    }

    @Override
    public J.Try visitTry(J.Try try_, P p) {
        return (J.Try) super.visitTry(try_, p);
    }

    @Override
    public J.TypeCast visitTypeCast(J.TypeCast typeCast, P p) {
        return (J.TypeCast) super.visitTypeCast(typeCast, p);
    }

    @Override
    public J.TypeParameter visitTypeParameter(J.TypeParameter typeParameter, P p) {
        return (J.TypeParameter) super.visitTypeParameter(typeParameter, p);
    }

    @Override
    public J.Unary visitUnary(J.Unary unary, P p) {
        return (J.Unary) super.visitUnary(unary, p);
    }

    @Override
    public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations variableDeclarations, P p) {
        return (J.VariableDeclarations) super.visitVariableDeclarations(variableDeclarations, p);
    }

    @Override
    public J.WhileLoop visitWhileLoop(J.WhileLoop whileLoop, P p) {
        return (J.WhileLoop) super.visitWhileLoop(whileLoop, p);
    }

    @Override
    public J.Wildcard visitWildcard(J.Wildcard wildcard, P p) {
        return (J.Wildcard) super.visitWildcard(wildcard, p);
    }

    @Override
    public J.Yield visitYield(J.Yield yield, P p) {
        return (J.Yield) super.visitYield(yield, p);
    }

    @Override
    public J.Unknown visitUnknown(J.Unknown unknown, P p) {
        return (J.Unknown) super.visitUnknown(unknown, p);
    }

}
