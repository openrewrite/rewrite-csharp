/*
 * Copyright 2024 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

// using Rewrite.RewriteJava.Tree;
using Tree = Rewrite.Core.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpPrinter<TState> : CSharpVisitor<PrintOutputCapture<TState>>
{
    private readonly CSharpJavaPrinter _delegate;

    public CSharpPrinter()
    {
        _delegate = new CSharpJavaPrinter(this);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public override J? Visit(Rewrite.Core.Tree? tree, PrintOutputCapture<TState> p)
    {
        if (!(tree is Cs))
        {
            // Re-route printing to the Java printer
            return _delegate.Visit(tree, p);
        }
        else
        {
            return base.Visit(tree, p);
        }
    }

    public override J? VisitNamedArgument(Cs.NamedArgument namedArgument, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(namedArgument, CsSpace.Location.NAMED_ARGUMENT_PREFIX, p);
        var padding = namedArgument.Padding;

        VisitRightPadded(padding.NameColumn, CsRightPadded.Location.NAMED_ARGUMENT_NAME_COLUMN, p);
        p.Append(':');
        Visit(namedArgument.Expression, p);
        AfterSyntax(namedArgument, p);
        return namedArgument;
    }

    public override Cs VisitCompilationUnit(Cs.CompilationUnit compilationUnit, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(compilationUnit, Space.Location.COMPILATION_UNIT_PREFIX, p);

        foreach (var externAlias in compilationUnit.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in compilationUnit.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.COMPILATION_UNIT_USINGS, p);
            p.Append(';');
        }

        foreach (var attributeList in compilationUnit.AttributeLists)
        {
            Visit(attributeList, p);
        }

        VisitStatements(compilationUnit.Padding.Members, CsRightPadded.Location.COMPILATION_UNIT_MEMBERS, p);
        VisitSpace(compilationUnit.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        AfterSyntax(compilationUnit, p);

        return compilationUnit;
    }

    public override J? VisitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(annotatedStatement, CsSpace.Location.ANNOTATED_STATEMENT_PREFIX, p);

        foreach (var attributeList in annotatedStatement.AttributeLists)
        {
            Visit(attributeList, p);
        }

        Visit(annotatedStatement.Statement, p);
        AfterSyntax(annotatedStatement, p);

        return annotatedStatement;
    }

    public override J? VisitAttributeList(Cs.AttributeList attributeList, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(attributeList, CsSpace.Location.ATTRIBUTE_LIST_PREFIX, p);
        p.Append('[');
        var padding = attributeList.Padding;
        if (padding.Target != null)
        {
            VisitRightPadded(padding.Target, CsRightPadded.Location.ATTRIBUTE_LIST_TARGET, p);
            p.Append(':');
        }

        VisitRightPadded(padding.Attributes, CsRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, ",", p);
        p.Append(']');
        AfterSyntax(attributeList, p);
        return attributeList;
    }

    public override J? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(arrayRankSpecifier, CsSpace.Location.ARRAY_RANK_SPECIFIER_PREFIX, p);
        VisitContainer("", arrayRankSpecifier.Padding.Sizes, CsContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, ",", "",
            p);
        AfterSyntax(arrayRankSpecifier, p);
        return arrayRankSpecifier;
    }

    public override J? VisitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(assignmentOperation, CsSpace.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        Visit(assignmentOperation.Variable, p);
        VisitLeftPadded(assignmentOperation.Padding.Operator, CsLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        if (assignmentOperation.Operator == Cs.AssignmentOperation.OperatorType.NullCoalescing)
        {
            p.Append("??=");
        }

        Visit(assignmentOperation.Assignment, p);
        AfterSyntax(assignmentOperation, p);
        return assignmentOperation;
    }

    public override J? VisitAwaitExpression(Cs.AwaitExpression awaitExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(awaitExpression, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        p.Append("await");
        Visit(awaitExpression.Expression, p);
        AfterSyntax(awaitExpression, p);
        return awaitExpression;
    }

    public override J? VisitBinary(Cs.Binary binary, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(binary, CsSpace.Location.BINARY_PREFIX, p);
        Visit(binary.Left, p);
        VisitSpace(binary.Padding.Operator.Before, Space.Location.BINARY_OPERATOR, p);
        if (binary.Operator == Cs.Binary.OperatorType.As)
        {
            p.Append("as");
        }
        else if (binary.Operator == Cs.Binary.OperatorType.NullCoalescing)
        {
            p.Append("??");
        }

        Visit(binary.Right, p);
        AfterSyntax(binary, p);
        return binary;
    }

    public override Cs VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<TState> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        VisitRightPadded(namespaceDeclaration.Padding.Name,
            CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.Append('{');

        foreach (var externAlias in namespaceDeclaration.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in namespaceDeclaration.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.Append(';');
        }

        VisitStatements(namespaceDeclaration.Padding.Members,
            CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        VisitSpace(namespaceDeclaration.End, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p);
        p.Append('}');
        AfterSyntax(namespaceDeclaration, p);
        return namespaceDeclaration;
    }

    public override J? VisitCollectionExpression(Cs.CollectionExpression collectionExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(collectionExpression, CsSpace.Location.COLLECTION_EXPRESSION_PREFIX, p);
        p.Append('[');
        VisitRightPadded(collectionExpression.Padding.Elements, CsRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS,
            ",", p);
        p.Append(']');
        AfterSyntax(collectionExpression, p);
        return collectionExpression;
    }

    public override J? VisitExpressionStatement(Cs.ExpressionStatement expressionStatement, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(expressionStatement, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p);
        Visit(expressionStatement.Expression, p);
        AfterSyntax(expressionStatement, p);
        return expressionStatement;
    }

    public override J? VisitExternAlias(Cs.ExternAlias externAlias, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(externAlias, CsSpace.Location.EXTERN_ALIAS_PREFIX, p);
        p.Append("extern");
        VisitLeftPadded("alias", externAlias.Padding.Identifier, CsLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p);
        AfterSyntax(externAlias, p);
        return externAlias;
    }

    public override Cs VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<TState> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        VisitRightPadded(namespaceDeclaration.Padding.Name,
            CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, p);
        p.Append(";");

        foreach (var externAlias in namespaceDeclaration.Padding.Externs)
        {
            VisitRightPadded(externAlias, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p);
            p.Append(';');
        }

        foreach (var usingDirective in namespaceDeclaration.Padding.Usings)
        {
            VisitRightPadded(usingDirective, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p);
            p.Append(';');
        }

        VisitStatements(namespaceDeclaration.Padding.Members,
            CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p);
        return namespaceDeclaration;
    }

    public override J? VisitInterpolatedString(Cs.InterpolatedString interpolatedString, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(interpolatedString, CsSpace.Location.INTERPOLATED_STRING_PREFIX, p);
        p.Append(interpolatedString.Start);
        VisitRightPadded(interpolatedString.Padding.Parts, CsRightPadded.Location.INTERPOLATED_STRING_PARTS, "", p);
        p.Append(interpolatedString.End);
        AfterSyntax(interpolatedString, p);
        return interpolatedString;
    }

    public override J? VisitInterpolation(Cs.Interpolation interpolation, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(interpolation, CsSpace.Location.INTERPOLATION_PREFIX, p);
        p.Append('{');
        VisitRightPadded(interpolation.Padding.Expression, CsRightPadded.Location.INTERPOLATION_EXPRESSION, p);

        if (interpolation.Alignment != null)
        {
            p.Append(',');
            VisitRightPadded(interpolation.Padding.Alignment, CsRightPadded.Location.INTERPOLATION_ALIGNMENT, p);
        }

        if (interpolation.Format != null)
        {
            p.Append(':');
            VisitRightPadded(interpolation.Padding.Format, CsRightPadded.Location.INTERPOLATION_FORMAT, p);
        }

        p.Append('}');
        AfterSyntax(interpolation, p);
        return interpolation;
    }

    public override J? VisitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(nullSafeExpression, CsSpace.Location.NULL_SAFE_EXPRESSION_PREFIX, p);
        VisitRightPadded(nullSafeExpression.Padding.Expression, CsRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION,
            p);
        p.Append("?");
        AfterSyntax(nullSafeExpression, p);
        return nullSafeExpression;
    }

    public override J? VisitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(propertyDeclaration, CsSpace.Location.PROPERTY_DECLARATION_PREFIX, p);
        Visit(propertyDeclaration.AttributeLists, p);

        foreach (var m in propertyDeclaration.Modifiers)
        {
            _delegate.VisitModifier(m, p);
        }

        Visit(propertyDeclaration.TypeExpression, p);

        if (propertyDeclaration.Padding.InterfaceSpecifier != null)
        {
            VisitRightPadded(propertyDeclaration.Padding.InterfaceSpecifier,
                CsRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, p);
            p.Append('.');
        }

        Visit(propertyDeclaration.Name, p);
        Visit(propertyDeclaration.Accessors, p);

        if (propertyDeclaration.Initializer != null)
        {
            VisitLeftPadded("=", propertyDeclaration.Padding.Initializer,
                CsLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p);
        }

        AfterSyntax(propertyDeclaration, p);
        return propertyDeclaration;
    }

    public override J? VisitUsingDirective(Cs.UsingDirective usingDirective, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(usingDirective, CsSpace.Location.USING_DIRECTIVE_PREFIX, p);

        if (usingDirective.Global)
        {
            p.Append("global");
            VisitRightPadded(usingDirective.Padding.Global, CsRightPadded.Location.USING_DIRECTIVE_GLOBAL, p);
        }

        p.Append("using");

        if (usingDirective.Static)
        {
            VisitLeftPadded(usingDirective.Padding.Static, CsLeftPadded.Location.USING_DIRECTIVE_STATIC, p);
            p.Append("static");
        }
        else if (usingDirective.Alias != null)
        {
            if (usingDirective.Unsafe)
            {
                VisitLeftPadded(usingDirective.Padding.Unsafe, CsLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p);
                p.Append("unsafe");
            }

            VisitRightPadded(usingDirective.Padding.Alias, CsRightPadded.Location.USING_DIRECTIVE_ALIAS, p);
            p.Append('=');
        }

        Visit(usingDirective.NamespaceOrType, p);
        AfterSyntax(usingDirective, p);
        return usingDirective;
    }

    public override Cs VisitLambda(Cs.Lambda lambda, PrintOutputCapture<TState> p)
    {
        var javaLambda = lambda.LambdaExpression;
        BeforeSyntax(javaLambda, Space.Location.LAMBDA_PREFIX, p);
        VisitSpace(javaLambda.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
        VisitMarkers(javaLambda.Markers, p);
        // _delegate.VisitContainer(lambda.Modifiers, JContainer.Location.ANY, p);
        foreach (var modifier in lambda.Modifiers)
        {
            _delegate.VisitModifier(modifier, p);
        }

        Visit(javaLambda, p);
        AfterSyntax(lambda, p);
        return lambda;
    }

    protected override Space VisitSpace(Space space, CsSpace.Location loc, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }

    public override Space VisitSpace(Space space, Space.Location? loc, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitSpace(space, loc, p);
    }

    protected void VisitLeftPadded<T>(string prefix, JLeftPadded<T>? leftPadded, CsLeftPadded.Location location,
        PrintOutputCapture<TState> p) where T : J
    {
        if (leftPadded != null)
        {
            BeforeSyntax(leftPadded.Before, leftPadded.Markers, location.BeforeLocation, p);

            if (prefix != null)
            {
                p.Append(prefix);
            }

            Visit(leftPadded.Element, p);
            AfterSyntax(leftPadded.Markers, p);
        }
    }

    protected void VisitContainer<T>(string before, JContainer<T> container, CsContainer.Location location,
        string suffixBetween, string after, PrintOutputCapture<TState> p) where T : J
    {
        if (container == null)
        {
            return;
        }

        VisitSpace(container.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitRightPadded(container.Padding.Elements, location.ElementLocation, suffixBetween, p);
        p.Append(after);
    }

    protected void VisitRightPadded<T>(IList<JRightPadded<T>> nodes, CsRightPadded.Location location,
        string suffixBetween, PrintOutputCapture<TState> p) where T : J
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            Visit(node.Element, p);
            VisitSpace(node.After, location.AfterLocation, p);
            VisitMarkers(node.Markers, p);

            if (i < nodes.Count - 1)
            {
                p.Append(suffixBetween);
            }
        }
    }

    protected void VisitStatements(string before, JContainer<Statement> container, CsContainer.Location location,
        string after, PrintOutputCapture<TState> p)
    {
        if (container == null)
        {
            return;
        }

        VisitSpace(container.Before, location.BeforeLocation, p);
        p.Append(before);
        VisitStatements(container.Padding.Elements, location.ElementLocation, p);
        p.Append(after);
    }

    protected void VisitStatements(IList<JRightPadded<Statement>> statements, CsRightPadded.Location location,
        PrintOutputCapture<TState> p)
    {
        foreach (var paddedStat in statements)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement(JRightPadded<Statement> paddedStat, CsRightPadded.Location location,
        PrintOutputCapture<TState> p)
    {
        if (paddedStat == null)
        {
            return;
        }

        Visit(paddedStat.Element, p);
        VisitSpace(paddedStat.After, location.AfterLocation, p);
        VisitMarkers(paddedStat.Markers, p);

        if (Cursor.Parent?.Value is J.Block && Cursor.Parent?.Parent?.Value is J.NewClass)
        {
            p.Append(',');
            return;
        }

        _delegate.PrintStatementTerminator(paddedStat.Element, p);
    }


    private class CSharpJavaPrinter(CSharpPrinter<TState> _parent) : JavaPrinter<TState>
    {
#if DEBUG_VISITOR
        [DebuggerStepThrough]
#endif
        public override J? Visit(Rewrite.Core.Tree? tree, PrintOutputCapture<TState> p)
        {
            if (tree is Cs)
            {
                // Re-route printing back up to C#
                return _parent.Visit(tree, p);
            }
            else if(tree is null or J)
            {
                return base.Visit(tree, p);
            }
            else if (tree is ParseError parseError)
            {
                p.Out.Append("/* LST PARSER ERROR\n");
                var parseExceptionResult = (ParseExceptionResult)parseError.Markers.First();
                p.Out.Append(parseExceptionResult.Message);
                p.Out.Append('\n');
                p.Out.Append("*/\n");
                p.Out.Append(parseError.Text);
            }

            return base.Visit(tree, p);
        }

        public override J VisitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<TState> p)
        {
            string kind = classDecl.Padding.DeclarationKind.KindType switch
            {
                J.ClassDeclaration.Kind.Type.Class => "class",
                J.ClassDeclaration.Kind.Type.Annotation => "class",
                J.ClassDeclaration.Kind.Type.Enum => "enum",
                J.ClassDeclaration.Kind.Type.Interface => "interface",
                J.ClassDeclaration.Kind.Type.Record => "record",
                J.ClassDeclaration.Kind.Type.Value => "struct",
                _ => ""
            };

            BeforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(classDecl.LeadingAnnotations, p);
            foreach (var modifier in classDecl.Modifiers)
            {
                VisitModifier(modifier, p);
            }

            Visit(classDecl.Padding.DeclarationKind.Annotations, p);
            VisitSpace(classDecl.Padding.DeclarationKind.Prefix, Space.Location.CLASS_KIND, p);
            p.Append(kind);
            Visit(classDecl.Name, p);
            VisitContainer("<", classDecl.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", classDecl.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")",
                p);
            VisitLeftPadded(":", classDecl.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
            VisitContainer(classDecl.Padding.Extends == null ? ":" : ",",
                classDecl.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
            VisitContainer("permits", classDecl.Padding.Permits, JContainer.Location.PERMITS, ",", null, p);
            Visit(classDecl.Body, p);
            AfterSyntax(classDecl, p);
            return classDecl;
        }

        public override J VisitAnnotation(J.Annotation annotation, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(annotation, Space.Location.ANNOTATION_PREFIX, p);
            Visit(annotation.AnnotationType, p);
            VisitContainer("(", annotation.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(annotation, p);
            return annotation;
        }


        public override J VisitBlock(J.Block block, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(block, Space.Location.BLOCK_PREFIX, p);

            if (block.Static)
            {
                p.Append("static");
                VisitRightPadded(block.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
            }

            if (block.Markers.FirstOrDefault(m => m is SingleExpressionBlock) != null)
            {
                p.Append("=>");
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(block.End, Space.Location.BLOCK_END, p);
            }
            else if (block.Markers.FirstOrDefault(m => m is OmitBraces) == null)
            {
                p.Append('{');
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(block.End, Space.Location.BLOCK_END, p);
                p.Append('}');
            }
            else
            {
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(block.End, Space.Location.BLOCK_END, p);
            }

            AfterSyntax(block, p);
            return block;
        }


        protected override void VisitStatements(IList<JRightPadded<Statement>> statements,
            JRightPadded.Location location,
            PrintOutputCapture<TState> p)
        {
            for (int i = 0; i < statements.Count; i++)
            {
                var paddedStat = statements[i];
                VisitStatement(paddedStat, location, p);
                if (i < statements.Count - 1 &&
                    (Cursor.Parent?.Value is J.NewClass ||
                     (Cursor.Parent?.Value is J.Block &&
                      Cursor.GetParent(2)?.Value is J.NewClass)))
                {
                    p.Append(',');
                }
            }
        }

        public override J VisitMethodDeclaration(J.MethodDeclaration method, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(method.LeadingAnnotations, p);
            foreach (var modifier in method.Modifiers)
            {
                VisitModifier(modifier, p);
            }

            Visit(method.ReturnTypeExpression, p);
            Visit(method.Annotations.Name.Annotations, p);
            Visit(method.Name, p);

            var typeParameters = method.Annotations.TypeParameters;
            if (typeParameters != null)
            {
                Visit(typeParameters.Annotations, p);
                VisitSpace(typeParameters.Prefix, Space.Location.TYPE_PARAMETERS, p);
                VisitMarkers(typeParameters.Markers, p);
                p.Append('<');
                VisitRightPadded(typeParameters.Padding.Parameters, JRightPadded.Location.TYPE_PARAMETER, ",", p);
                p.Append('>');
            }

            if (method.Markers.FirstOrDefault(m => m is CompactConstructor) == null)
            {
                VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",",
                    ")", p);
            }

            VisitContainer("throws", method.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
            Visit(method.Body, p);
            VisitLeftPadded("default", method.Padding.DefaultValue,
                JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
            AfterSyntax(method, p);
            return method;
        }

        public override J VisitMethodInvocation(J.MethodInvocation method, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(method, Space.Location.METHOD_INVOCATION_PREFIX, p);
            var prefix = method.Name.SimpleName != "" ? "." : "";
            VisitRightPadded(method.Padding.Select, JRightPadded.Location.METHOD_SELECT, prefix, p);
            Visit(method.Name, p);
            VisitContainer("<", method.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", method.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
            AfterSyntax(method, p);
            return method;
        }

        public override J VisitCatch(J.Try.Catch catch_, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(catch_, Space.Location.CATCH_PREFIX, p);
            p.Append("catch");
            if (catch_.Parameter.Tree.TypeExpression != null)
            {
                Visit(catch_.Parameter, p);
            }

            Visit(catch_.Body, p);
            AfterSyntax(catch_, p);
            return catch_;
        }

        public override J VisitForEachLoop(J.ForEachLoop forEachLoop, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
            p.Append("foreach");
            var ctrl = forEachLoop.LoopControl;
            VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
            p.Append('(');
            VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, "in", p);
            VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, "", p);
            p.Append(')');
            VisitStatement(forEachLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p);
            AfterSyntax(forEachLoop, p);
            return forEachLoop;
        }

        public override J VisitInstanceOf(J.InstanceOf instanceOf, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(instanceOf, Space.Location.INSTANCEOF_PREFIX, p);
            VisitRightPadded(instanceOf.Padding.Expression, JRightPadded.Location.INSTANCEOF, "is", p);
            Visit(instanceOf.Clazz, p);
            Visit(instanceOf.Pattern, p);
            AfterSyntax(instanceOf, p);
            return instanceOf;
        }

        public override J VisitLambda(J.Lambda lambda, PrintOutputCapture<TState> p)
        {
            BeforeSyntax(lambda, Space.Location.LAMBDA_PREFIX, p);
            VisitSpace(lambda.Params.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
            VisitMarkers(lambda.Params.Markers, p);

            if (lambda.Params.Parenthesized)
            {
                p.Append('(');
                VisitRightPadded(lambda.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
                p.Append(')');
            }
            else
            {
                VisitRightPadded(lambda.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
            }

            VisitSpace(lambda.Arrow, Space.Location.LAMBDA_ARROW_PREFIX, p);
            p.Append("=>");
            Visit(lambda.Body, p);
            AfterSyntax(lambda, p);
            return lambda;
        }

        public override J VisitPrimitive(J.Primitive primitive, PrintOutputCapture<TState> p)
        {
            string keyword = primitive.Type.Kind switch
            {
                JavaType.Primitive.PrimitiveType.Boolean => "bool",
                JavaType.Primitive.PrimitiveType.Byte => "byte",
                JavaType.Primitive.PrimitiveType.Char => "char",
                JavaType.Primitive.PrimitiveType.Double => "double",
                JavaType.Primitive.PrimitiveType.Float => "float",
                JavaType.Primitive.PrimitiveType.Int => "int",
                JavaType.Primitive.PrimitiveType.Long => "long",
                JavaType.Primitive.PrimitiveType.Short => "short",
                JavaType.Primitive.PrimitiveType.Void => "void",
                JavaType.Primitive.PrimitiveType.String => "string",
                JavaType.Primitive.PrimitiveType.None => throw new InvalidOperationException(
                    "Unable to print None primitive"),
                JavaType.Primitive.PrimitiveType.Null => throw new InvalidOperationException(
                    "Unable to print Null primitive"),
                _ => throw new InvalidOperationException("Unable to print non-primitive type")
            };

            BeforeSyntax(primitive, Space.Location.PRIMITIVE_PREFIX, p);
            p.Append(keyword);
            AfterSyntax(primitive, p);
            return primitive;
        }

        public override J VisitTry(J.Try tryable, PrintOutputCapture<TState> p)
        {
            if (tryable.Padding.Resources != null)
            {
                // this is a `using` statement
                BeforeSyntax(tryable, Space.Location.TRY_PREFIX, p);
                p.Append("using");

                // Note: we do not call VisitContainer here because the last resource may or may not be semicolon-terminated.
                // Doing this means that VisitTryResource is not called, therefore this logic must visit the resources.
                VisitSpace(tryable.Padding.Resources.Before, Space.Location.TRY_RESOURCES, p);
                p.Append('(');
                var resources = tryable.Padding.Resources.Padding.Elements;

                foreach (var resource in resources)
                {
                    VisitSpace(resource.Element.Prefix, Space.Location.TRY_RESOURCE, p);
                    VisitMarkers(resource.Element.Markers, p);
                    Visit(resource.Element.VariableDeclarations, p);

                    if (resource.Element.TerminatedWithSemicolon)
                    {
                        p.Append(';');
                    }

                    VisitSpace(resource.After, Space.Location.TRY_RESOURCE_SUFFIX, p);
                }

                p.Append(')');
                Visit(tryable.Body, p);
                AfterSyntax(tryable, p);
                return tryable;
            }

            return base.VisitTry(tryable, p);
        }

        public override J? VisitModifier(J.Modifier mod, PrintOutputCapture<TState> p)
        {
            return base.VisitModifier(mod, p);
        }


        public override M VisitMarker<M>(M marker, PrintOutputCapture<TState> p)
        {
            if (marker is Semicolon)
            {
                p.Append(';');
            }
            else if (marker is TrailingComma trailingComma)
            {
                p.Append(',');
                VisitSpace(trailingComma.Suffix, Space.Location.LANGUAGE_EXTENSION, p);
            }

            return (M)marker;
        }

        // override print
        public override void PrintStatementTerminator(Statement s, PrintOutputCapture<TState> p)
        {
            var parent = Cursor.Parent;
            if (parent != null && parent.Value is J.NewClass ||
                (parent != null && parent.Value is J.Block &&
                 Cursor.GetParent(2)?.Value is J.NewClass))
            {
                // do nothing, comma is printed at the block level
            }
            else if (s is Cs.ExpressionStatement || s is Cs.AssignmentOperation)
            {
                p.Append(';');
            }
            else if (s is Cs.PropertyDeclaration propertyDeclaration &&
                     (propertyDeclaration.Initializer != null ||
                      propertyDeclaration.Accessors.Markers.FirstOrDefault(m => m is SingleExpressionBlock) != null))
            {
                p.Append(';');
            }
            else if (s is J.ClassDeclaration classDeclaration &&
                     classDeclaration.Body.Markers.FirstOrDefault(m => m is OmitBraces) != null)
            {
                // class declaration without braces always requires a semicolon
                p.Append(';');
            }
            else if (s is Cs.AnnotatedStatement annotatedStatement &&
                     annotatedStatement.Statement is J.ClassDeclaration innerClassDeclaration &&
                     innerClassDeclaration.Body.Markers.FirstOrDefault(m => m is OmitBraces) != null)
            {
                // class declaration without braces always requires a semicolon
                p.Append(';');
            }
            else
            {
                base.PrintStatementTerminator(s, p);
            }
        }
    }

    public override Markers VisitMarkers(Markers? markers, PrintOutputCapture<TState> p)
    {
        return _delegate.VisitMarkers(markers, p);
    }

    private static readonly Func<string, string> JAVA_MARKER_WRAPPER =
        o => "/*~~" + o + (string.IsNullOrEmpty(o) ? "" : "~~") + ">*/";

    private void BeforeSyntax(J cs, Space.Location loc, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(J cs, CsSpace.Location loc, PrintOutputCapture<TState> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(Space prefix, Markers markers, CsSpace.Location? loc, PrintOutputCapture<TState> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc, p);
        }

        VisitMarkers(markers, p);

        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }

    private void BeforeSyntax(Space prefix, Markers markers, Space.Location? loc, PrintOutputCapture<TState> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc.Value, p);
        }

        VisitMarkers(markers, p);

        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }


    private void AfterSyntax(Cs g, PrintOutputCapture<TState> p)
    {
        AfterSyntax(g.Markers, p);
    }

    private void AfterSyntax(Markers markers, PrintOutputCapture<TState> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }
}
