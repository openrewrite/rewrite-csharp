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

import org.openrewrite.Cursor;
import org.openrewrite.PrintOutputCapture;
import org.openrewrite.Tree;
import org.openrewrite.csharp.marker.OmitBraces;
import org.openrewrite.csharp.marker.SingleExpressionBlock;
import org.openrewrite.csharp.tree.*;
import org.openrewrite.internal.lang.Nullable;
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

    private class CSharpJavaPrinter extends JavaPrinter<P> {
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
        public J visitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<P> p) {
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
                visitModifier(m, p);
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
                visitSpace(block.getEnd(), Space.Location.BLOCK_END, p);
            } else if (!block.getMarkers().findFirst(OmitBraces.class).isPresent()) {
                p.append('{');
                visitStatements(block.getPadding().getStatements(), JRightPadded.Location.BLOCK_STATEMENT, p);
                visitSpace(block.getEnd(), Space.Location.BLOCK_END, p);
                p.append('}');
            } else {
                visitStatements(block.getPadding().getStatements(), JRightPadded.Location.BLOCK_STATEMENT, p);
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
                visitModifier(m, p);
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
            visitContainer("throws", method.getPadding().getThrows(), JContainer.Location.THROWS, ",", null, p);
            visit(method.getBody(), p);
            visitLeftPadded("default", method.getPadding().getDefaultValue(), JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
            afterSyntax(method, p);
            return method;
        }

        @Override
        public J visitMethodInvocation(J.MethodInvocation method, PrintOutputCapture<P> p) {
            beforeSyntax(method, Space.Location.METHOD_INVOCATION_PREFIX, p);
            visitRightPadded(method.getPadding().getSelect(), JRightPadded.Location.METHOD_SELECT, ".", p);
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
        public J visitTry(J.Try tryable, PrintOutputCapture<P> p) {
            if (tryable.getPadding().getResources() != null) {
                // this is a `using` statement
                beforeSyntax(tryable, Space.Location.TRY_PREFIX, p);
                p.append("using");

                //Note: we do not call visitContainer here because the last resource may or may not be semicolon terminated.
                //      Doing this means that visitTryResource is not called, therefore this logiJ must visit the resources.
                visitSpace(tryable.getPadding().getResources().getBefore(), Space.Location.TRY_RESOURCES, p);
                p.append('(');
                List<JRightPadded<J.Try.Resource>> resources = tryable.getPadding().getResources().getPadding().getElements();
                for (JRightPadded<J.Try.Resource> resource : resources) {
                    visitSpace(resource.getElement().getPrefix(), Space.Location.TRY_RESOURCE, p);
                    visitMarkers(resource.getElement().getMarkers(), p);
                    visit(resource.getElement().getVariableDeclarations(), p);

                    if (resource.getElement().isTerminatedWithSemicolon()) {
                        p.append(';');
                    }

                    visitSpace(resource.getAfter(), Space.Location.TRY_RESOURCE_SUFFIX, p);
                }
                p.append(')');

                visit(tryable.getBody(), p);
                afterSyntax(tryable, p);
                return tryable;
            }
            return super.visitTry(tryable, p);
        }

        @Override
        public void visitModifier(J.Modifier mod, PrintOutputCapture<P> p) {
            super.visitModifier(mod, p);
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
            if (getCursor().getParent() != null && getCursor().getParent()
                    .getValue() instanceof J.NewClass ||
                (getCursor().getParent() != null && getCursor().getParent()
                        .getValue() instanceof J.Block &&
                 getCursor().getParent(2) != null && getCursor().getParent(2).getValue() instanceof J.NewClass
                )
            ) {
                // do nothing, comma is printed at the block level
            } else if (s instanceof Cs.ExpressionStatement || s instanceof Cs.AssignmentOperation) {
                p.append(';');
            } else if (s instanceof Cs.PropertyDeclaration && (((Cs.PropertyDeclaration) s).getInitializer() != null || ((Cs.PropertyDeclaration) s).getAccessors().getMarkers().findFirst(SingleExpressionBlock.class).isPresent())) {
                p.append(';');
            } else if (s instanceof J.ClassDeclaration && ((J.ClassDeclaration) s).getBody().getMarkers().findFirst(OmitBraces.class).isPresent()) {
                // class declaration without braces always require a semicolon
                p.append(';');
            } else if (s instanceof Cs.AnnotatedStatement && ((Cs.AnnotatedStatement) s).getStatement() instanceof J.ClassDeclaration &&
                       ((J.ClassDeclaration) ((Cs.AnnotatedStatement) s).getStatement()).getBody().getMarkers().findFirst(OmitBraces.class).isPresent()) {
                // class declaration without braces always require a semicolon
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

    private void beforeSyntax(Cs cs, Space.Location loc, PrintOutputCapture<P> p) {
        beforeSyntax(cs.getPrefix(), cs.getMarkers(), loc, p);
    }

    private void beforeSyntax(Cs cs, CsSpace.Location loc, PrintOutputCapture<P> p) {
        beforeSyntax(cs.getPrefix(), cs.getMarkers(), loc, p);
    }

    private void beforeSyntax(Space prefix, Markers markers, @Nullable CsSpace.Location loc, PrintOutputCapture<P> p) {
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

    private void beforeSyntax(Space prefix, Markers markers, @Nullable Space.Location loc, PrintOutputCapture<P> p) {
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