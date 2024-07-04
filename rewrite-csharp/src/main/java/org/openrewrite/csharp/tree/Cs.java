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
package org.openrewrite.csharp.tree;

import lombok.*;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;
import org.openrewrite.*;
import org.openrewrite.csharp.CSharpPrinter;
import org.openrewrite.csharp.CSharpVisitor;
import org.openrewrite.internal.lang.Nullable;
import org.openrewrite.java.JavaPrinter;
import org.openrewrite.java.tree.*;
import org.openrewrite.marker.Markers;

import java.beans.Transient;
import java.lang.ref.WeakReference;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static java.util.Collections.singletonList;

public interface Cs extends J {

    @SuppressWarnings("unchecked")
    @Override
    default <R extends Tree, P> R accept(TreeVisitor<R, P> v, P p) {
        return (R) acceptCSharp(v.adapt(CSharpVisitor.class), p);
    }

    @Override
    default <P> boolean isAcceptable(TreeVisitor<?, P> v, P p) {
        return v.isAdaptableTo(CSharpVisitor.class);
    }

    @Nullable
    default <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
        return v.defaultValue(this, p);
    }

    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    final class CompilationUnit implements Cs, SourceFile {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        @Getter
        @With
        Path sourcePath;

        @Getter
        @With
        @Nullable
        FileAttributes fileAttributes;

        @Getter
        @Nullable
        @With(AccessLevel.PRIVATE)
        String charsetName;

        @Getter
        @With
        boolean charsetBomMarked;

        @Getter
        @With
        @Nullable
        Checksum checksum;

        @Override
        public Charset getCharset() {
            return charsetName == null ? StandardCharsets.UTF_8 : Charset.forName(charsetName);
        }

        @SuppressWarnings("unchecked")
        @Override
        public SourceFile withCharset(Charset charset) {
            return withCharsetName(charset.name());
        }

        List<JRightPadded<Cs.UsingDirective>> usings;

        public List<Cs.UsingDirective> getUsings() {
            return JRightPadded.getElements(usings);
        }

        public Cs.CompilationUnit withUsings(List<Cs.UsingDirective> usings) {
            return getPadding().withUsings(JRightPadded.withElements(this.usings, usings));
        }

        @Getter
        @With
        List<Cs.AttributeList> attributeLists;

        List<JRightPadded<Statement>> members;

        public List<Statement> getMembers() {
            return JRightPadded.getElements(members);
        }

        public Cs.CompilationUnit withMembers(List<Statement> members) {
            return getPadding().withMembers(JRightPadded.withElements(this.members, members));
        }

        @Getter
        @With
        Space eof;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitCompilationUnit(this, p);
        }

        @Override
        public <P> TreeVisitor<?, PrintOutputCapture<P>> printer(Cursor cursor) {
            return new CSharpPrinter<>();
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.CompilationUnit t;

            public List<JRightPadded<Statement>> getMembers() {
                return t.members;
            }

            public Cs.CompilationUnit withMembers(List<JRightPadded<Statement>> members) {
                return t.members == members ? t : new Cs.CompilationUnit(t.id, t.prefix, t.markers, t.sourcePath, t.fileAttributes, t.charsetName, t.charsetBomMarked, t.checksum, t.usings, t.attributeLists, members, t.eof);
            }

            public List<JRightPadded<UsingDirective>> getUsings() {
                return t.usings;
            }

            public Cs.CompilationUnit withUsings(List<JRightPadded<UsingDirective>> usings) {
                return t.usings == usings ? t : new Cs.CompilationUnit(t.id, t.prefix, t.markers, t.sourcePath, t.fileAttributes, t.charsetName, t.charsetBomMarked, t.checksum, usings, t.attributeLists, t.members, t.eof);
            }
        }
    }

    @Getter
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    final class AnnotatedStatement implements Cs, Statement {
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @With
        Space prefix;

        @With
        Markers markers;

        @With
        List<AttributeList> attributeLists;

        @With
        Statement statement;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitAnnotatedStatement(this, p);
        }

        @Override
        @Transient
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }

        @Override
        public String toString() {
            return withPrefix(Space.EMPTY).printTrimmed(new CSharpPrinter<>());
        }
    }

    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    final class ArrayRankSpecifier implements Cs, Expression {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @EqualsAndHashCode.Include
        @Getter
        @With
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        JContainer<Expression> sizes;

        public List<Expression> getSizes() {
            return sizes.getElements();
        }

        public ArrayRankSpecifier withSizes(List<Expression> sizes) {
            return getPadding().withSizes(JContainer.withElements(this.sizes, sizes));
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitArrayRankSpecifier(this, p);
        }

        @Override
        public @Nullable JavaType getType() {
            return sizes.getElements().isEmpty() ? null : sizes.getPadding().getElements().get(0).getElement().getType();
        }

        @Override
        public <T extends J> T withType(@Nullable JavaType type) {
            throw new IllegalArgumentException("Cannot set type on " + getClass());
        }

        @Override
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final ArrayRankSpecifier t;

            @Nullable
            public JContainer<Expression> getSizes() {
                return t.sizes;
            }

            public ArrayRankSpecifier withSizes(@Nullable JContainer<Expression> sizes) {
                return t.sizes == sizes ? t : new ArrayRankSpecifier(t.id, t.prefix, t.markers, sizes);
            }
        }
    }

    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    final class AssignmentOperation implements Cs, Statement, Expression, TypedTree {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @With
        @EqualsAndHashCode.Include
        @Getter
        UUID id;

        @With
        @Getter
        Space prefix;

        @With
        @Getter
        Markers markers;

        @With
        @Getter
        Expression variable;

        JLeftPadded<OperatorType> operator;

        public OperatorType getOperator() {
            return operator.getElement();
        }

        public Cs.AssignmentOperation withOperator(OperatorType operator) {
            return getPadding().withOperator(this.operator.withElement(operator));
        }

        @With
        @Getter
        Expression assignment;

        @With
        @Nullable
        @Getter
        JavaType type;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitAssignmentOperation(this, p);
        }

        @Override
        @Transient
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }

        @Override
        @Transient
        public List<J> getSideEffects() {
            return singletonList(this);
        }

        public enum OperatorType {
            NullCoalescing
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @Override
        public String toString() {
            return withPrefix(Space.EMPTY).printTrimmed(new CSharpPrinter<>());
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.AssignmentOperation t;

            public JLeftPadded<OperatorType> getOperator() {
                return t.operator;
            }

            public Cs.AssignmentOperation withOperator(JLeftPadded<OperatorType> operator) {
                return t.operator == operator ? t : new Cs.AssignmentOperation(t.id, t.prefix, t.markers, t.variable, operator, t.assignment, t.type);
            }
        }
    }

    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    final class AttributeList implements Cs {
        @Nullable
        @NonFinal
        transient WeakReference<AttributeList.Padding> padding;

        @With
        @EqualsAndHashCode.Include
        @Getter
        UUID id;

        @With
        @Getter
        Space prefix;

        @With
        @Getter
        Markers markers;

        @Nullable
        JRightPadded<J.Identifier> target;

        @Nullable
        public J.Identifier getTarget() {
            return target == null ? null : target.getElement();
        }

        public AttributeList withTarget(@Nullable J.Identifier target) {
            return getPadding().withTarget(JRightPadded.withElement(this.target, target));
        }

        List<JRightPadded<J.Annotation>> attributes;

        public List<J.Annotation> getAttributes() {
            return JRightPadded.getElements(attributes);
        }

        public AttributeList withAttributes(List<J.Annotation> attributes) {
            return getPadding().withAttributes(JRightPadded.withElements(this.attributes, attributes));
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitAttributeList(this, p);
        }

        public AttributeList.Padding getPadding() {
            AttributeList.Padding p;
            if (this.padding == null) {
                p = new AttributeList.Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new AttributeList.Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @Override
        public String toString() {
            return withPrefix(Space.EMPTY).printTrimmed(new JavaPrinter<>());
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.AttributeList t;

            public @Nullable JRightPadded<Identifier> getTarget() {
                return t.target;
            }

            public AttributeList withTarget(@Nullable JRightPadded<J.Identifier> target) {
                return t.target == target ? t : new AttributeList(t.id, t.prefix, t.markers, target, t.attributes);
            }

            public List<JRightPadded<Annotation>> getAttributes() {
                return t.attributes;
            }

            public AttributeList withAttributes(List<JRightPadded<Annotation>> attributes) {
                return t.attributes == attributes ? t : new AttributeList(t.id, t.prefix, t.markers, t.target, attributes);
            }
        }
    }

    @Getter
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @With
    final class AwaitExpression implements Cs, Expression {

        @EqualsAndHashCode.Include
        UUID id;

        Space prefix;
        Markers markers;

        Expression expression;

        @Nullable
        JavaType type;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitAwaitExpression(this, p);
        }

        @Override
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }
    }

    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    @Data
    final class Binary implements Cs, Expression, TypedTree {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @With
        @EqualsAndHashCode.Include
        UUID id;

        @With
        Space prefix;

        @With
        Markers markers;

        @With
        Expression left;

        JLeftPadded<OperatorType> operator;

        public OperatorType getOperator() {
            return operator.getElement();
        }

        public Cs.Binary withOperator(Cs.Binary.OperatorType operator) {
            return getPadding().withOperator(this.operator.withElement(operator));
        }

        @With
        Expression right;

        @With
        @Nullable
        JavaType type;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitBinary(this, p);
        }

        @Override
        @Transient
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }

        @Transient
        @Override
        public List<J> getSideEffects() {
            List<J> sideEffects = new ArrayList<>(2);
            sideEffects.addAll(left.getSideEffects());
            sideEffects.addAll(right.getSideEffects());
            return sideEffects;
        }

        public enum OperatorType {
            As,
            NullCoalescing
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @Override
        public String toString() {
            return withPrefix(Space.EMPTY).printTrimmed(new CSharpPrinter<>());
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.Binary t;

            public JLeftPadded<Cs.Binary.OperatorType> getOperator() {
                return t.operator;
            }

            public Cs.Binary withOperator(JLeftPadded<Cs.Binary.OperatorType> operator) {
                return t.operator == operator ? t : new Cs.Binary(t.id, t.prefix, t.markers, t.left, operator, t.right, t.type);
            }
        }
    }

    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    class BlockScopeNamespaceDeclaration implements Cs, Statement {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        JRightPadded<Expression> name;

        public Expression getName() {
            return name.getElement();
        }

        public BlockScopeNamespaceDeclaration withName(Expression name) {
            return getPadding().withName(JRightPadded.withElement(this.name, name));
        }

        List<JRightPadded<Cs.UsingDirective>> usings;

        public List<Cs.UsingDirective> getUsings() {
            return JRightPadded.getElements(usings);
        }

        public Cs.BlockScopeNamespaceDeclaration withUsings(List<Cs.UsingDirective> usings) {
            return getPadding().withUsings(JRightPadded.withElements(this.usings, usings));
        }

        List<JRightPadded<Statement>> members;

        public List<Statement> getMembers() {
            return JRightPadded.getElements(members);
        }

        public Cs.BlockScopeNamespaceDeclaration withMembers(List<Statement> members) {
            return getPadding().withMembers(JRightPadded.withElements(this.members, members));
        }

        @Getter
        @With
        Space end;

        @Override
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitBlockScopeNamespaceDeclaration(this, p);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.BlockScopeNamespaceDeclaration t;

            public JRightPadded<Expression> getName() {
                return t.name;
            }

            public BlockScopeNamespaceDeclaration withName(JRightPadded<Expression> name) {
                return t.name == name ? t : new Cs.BlockScopeNamespaceDeclaration(t.id, t.prefix, t.markers, name, t.usings, t.members, t.end);
            }

            public List<JRightPadded<UsingDirective>> getUsings() {
                return t.usings;
            }

            public Cs.BlockScopeNamespaceDeclaration withUsings(List<JRightPadded<UsingDirective>> usings) {
                return t.usings == usings ? t : new Cs.BlockScopeNamespaceDeclaration(t.id, t.prefix, t.markers, t.name, usings, t.members, t.end);
            }

            public List<JRightPadded<Statement>> getMembers() {
                return t.members;
            }

            public BlockScopeNamespaceDeclaration withMembers(List<JRightPadded<Statement>> members) {
                return t.members == members ? t : new Cs.BlockScopeNamespaceDeclaration(t.id, t.prefix, t.markers, t.name, t.usings, members, t.end);
            }
        }
    }

    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    final class CollectionExpression implements Cs, Expression, TypedTree {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        List<JRightPadded<Expression>> elements;

        public List<Expression> getElements() {
            return JRightPadded.getElements(elements);
        }

        public CollectionExpression withElements(List<Expression> elements) {
            return getPadding().withElements(JRightPadded.withElements(this.elements, elements));
        }

        @Getter
        @With
        JavaType type;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitCollectionExpression(this, p);
        }

        @Transient
        @Override
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.CollectionExpression t;

            public List<JRightPadded<Expression>> getElements() {
                return t.elements;
            }

            public CollectionExpression withElements(List<JRightPadded<Expression>> elements) {
                return t.elements == elements ? t : new Cs.CollectionExpression(t.id, t.prefix, t.markers, elements, t.type);
            }
        }
    }

    @Getter
    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @With
    final class ExpressionStatement implements Cs, Statement {

        @EqualsAndHashCode.Include
        UUID id;
        Space prefix;
        Markers markers;

        Expression expression;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitExpressionStatement(this, p);
        }

        @Transient
        @Override
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }
    }

    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    class FileScopeNamespaceDeclaration implements Cs, Statement {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        JRightPadded<Expression> name;

        public Expression getName() {
            return name.getElement();
        }

        public FileScopeNamespaceDeclaration withName(Expression name) {
            return getPadding().withName(JRightPadded.withElement(this.name, name));
        }

        List<JRightPadded<Cs.UsingDirective>> usings;

        public List<Cs.UsingDirective> getUsings() {
            return JRightPadded.getElements(usings);
        }

        public Cs.FileScopeNamespaceDeclaration withUsings(List<Cs.UsingDirective> usings) {
            return getPadding().withUsings(JRightPadded.withElements(this.usings, usings));
        }

        List<JRightPadded<Statement>> members;

        public List<Statement> getMembers() {
            return JRightPadded.getElements(members);
        }

        public Cs.FileScopeNamespaceDeclaration withMembers(List<Statement> members) {
            return getPadding().withMembers(JRightPadded.withElements(this.members, members));
        }

        @Override
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitFileScopeNamespaceDeclaration(this, p);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final Cs.FileScopeNamespaceDeclaration t;

            public JRightPadded<Expression> getName() {
                return t.name;
            }

            public FileScopeNamespaceDeclaration withName(JRightPadded<Expression> name) {
                return t.name == name ? t : new Cs.FileScopeNamespaceDeclaration(t.id, t.prefix, t.markers, name, t.usings, t.members);
            }

            public List<JRightPadded<UsingDirective>> getUsings() {
                return t.usings;
            }

            public Cs.FileScopeNamespaceDeclaration withUsings(List<JRightPadded<UsingDirective>> usings) {
                return t.usings == usings ? t : new Cs.FileScopeNamespaceDeclaration(t.id, t.prefix, t.markers, t.name, usings, t.members);
            }

            public List<JRightPadded<Statement>> getMembers() {
                return t.members;
            }

            public FileScopeNamespaceDeclaration withMembers(List<JRightPadded<Statement>> members) {
                return t.members == members ? t : new Cs.FileScopeNamespaceDeclaration(t.id, t.prefix, t.markers, t.name, t.usings, members);
            }
        }
    }

    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    class NullSafeExpression implements Cs, Expression {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        JRightPadded<Expression> expression;

        public Expression getExpression() {
            return expression.getElement();
        }

        public NullSafeExpression withExpression(Expression expression) {
            return getPadding().withExpression(JRightPadded.withElement(this.expression, expression));
        }

        @Override
        public @Nullable JavaType getType() {
            return expression.getElement().getType();
        }

        @Override
        public <T extends J> T withType(@Nullable JavaType type) {
            Expression newExpression = expression.getElement().withType(type);
            if (newExpression == expression.getElement()) {
                return (T) this;
            }
            return (T) new NullSafeExpression(id, prefix, markers, JRightPadded.withElement(expression, newExpression));
        }

        @Override
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitNullSafeExpression(this, p);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final NullSafeExpression t;

            public JRightPadded<Expression> getExpression() {
                return t.expression;
            }

            public NullSafeExpression withExpression(JRightPadded<Expression> expression) {
                return t.expression == expression ? t : new NullSafeExpression(t.id, t.prefix, t.markers, expression);
            }
        }
    }

    @Getter
    @ToString
    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @With
    final class StatementExpression implements Cs, Expression {

        @EqualsAndHashCode.Include
        UUID id;
        Space prefix;
        Markers markers;

        Statement statement;

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitStatementExpression(this, p);
        }

        @Override
        public @Nullable JavaType getType() {
            return null;
        }

        @Override
        public <T extends J> T withType(@Nullable JavaType type) {
            return (T) this;
        }

        @Transient
        @Override
        public CoordinateBuilder.Expression getCoordinates() {
            return new CoordinateBuilder.Expression(this);
        }
    }

    @FieldDefaults(makeFinal = true, level = AccessLevel.PRIVATE)
    @EqualsAndHashCode(callSuper = false, onlyExplicitlyIncluded = true)
    @RequiredArgsConstructor
    @AllArgsConstructor(access = AccessLevel.PRIVATE)
    class UsingDirective implements Cs, Statement {
        @Nullable
        @NonFinal
        transient WeakReference<Padding> padding;

        @Getter
        @With
        @EqualsAndHashCode.Include
        UUID id;

        @Getter
        @With
        Space prefix;

        @Getter
        @With
        Markers markers;

        JRightPadded<Boolean> global;

        public boolean isGlobal() {
            return global.getElement();
        }

        public UsingDirective withGlobal(boolean global) {
            return getPadding().withGlobal(JRightPadded.withElement(this.global, global));
        }

        JLeftPadded<Boolean> statik;

        public boolean isStatic() {
            return statik.getElement();
        }

        public UsingDirective withStatic(boolean statik) {
            return getPadding().withStatic(JLeftPadded.withElement(this.statik, statik));
        }

        JLeftPadded<Boolean> unsafe;

        public boolean isUnsafe() {
            return unsafe.getElement();
        }

        public UsingDirective withUnsafe(boolean unsafe) {
            return getPadding().withUnsafe(JLeftPadded.withElement(this.unsafe, unsafe));
        }

        @Nullable
        JRightPadded<J.Identifier> alias;

        @Nullable
        public J.Identifier getAlias() {
            return alias != null ? alias.getElement() : null;
        }

        public UsingDirective withAlias(@Nullable J.Identifier alias) {
            return getPadding().withAlias(JRightPadded.withElement(this.alias, alias));
        }

        @Getter
        @With
        TypeTree namespaceOrType;

        @Override
        public CoordinateBuilder.Statement getCoordinates() {
            return new CoordinateBuilder.Statement(this);
        }

        @Override
        public <P> J acceptCSharp(CSharpVisitor<P> v, P p) {
            return v.visitUsingDirective(this, p);
        }

        public Padding getPadding() {
            Padding p;
            if (this.padding == null) {
                p = new Padding(this);
                this.padding = new WeakReference<>(p);
            } else {
                p = this.padding.get();
                if (p == null || p.t != this) {
                    p = new Padding(this);
                    this.padding = new WeakReference<>(p);
                }
            }
            return p;
        }

        @RequiredArgsConstructor
        public static class Padding {
            private final UsingDirective t;

            public JRightPadded<Boolean> getGlobal() {
                return t.global;
            }

            public UsingDirective withGlobal(JRightPadded<Boolean> global) {
                return t.global == global ? t : new UsingDirective(t.id, t.prefix, t.markers, global, t.statik, t.unsafe, t.alias, t.namespaceOrType);
            }

            public JLeftPadded<Boolean> getStatic() {
                return t.statik;
            }

            public UsingDirective withStatic(JLeftPadded<Boolean> statik) {
                return t.statik == statik ? t : new UsingDirective(t.id, t.prefix, t.markers, t.global, statik, t.unsafe, t.alias, t.namespaceOrType);
            }

            public JLeftPadded<Boolean> getUnsafe() {
                return t.unsafe;
            }

            public UsingDirective withUnsafe(JLeftPadded<Boolean> unsafe) {
                return t.unsafe == unsafe ? t : new UsingDirective(t.id, t.prefix, t.markers, t.global, t.statik, unsafe, t.alias, t.namespaceOrType);
            }

            @Nullable
            public JRightPadded<J.Identifier> getAlias() {
                return t.alias;
            }

            public UsingDirective withAlias(JRightPadded<J.Identifier> alias) {
                return t.alias == alias ? t : new UsingDirective(t.id, t.prefix, t.markers, t.global, t.statik, t.unsafe, t.alias, t.namespaceOrType);
            }
        }
    }
}
