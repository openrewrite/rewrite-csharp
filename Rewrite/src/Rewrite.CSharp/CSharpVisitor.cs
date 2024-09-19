using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
public class CSharpVisitor<P> : JavaVisitor<P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is Cs;
    }

    public virtual J? VisitCompilationUnit(Cs.CompilationUnit compilationUnit, P p)
    {
        compilationUnit = compilationUnit.WithPrefix(VisitSpace(compilationUnit.Prefix, Space.Location.COMPILATION_UNIT_PREFIX, p)!);
        compilationUnit = compilationUnit.WithMarkers(VisitMarkers(compilationUnit.Markers, p));
        compilationUnit = compilationUnit.Padding.WithExterns(ListUtils.Map(compilationUnit.Padding.Externs, el => VisitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_EXTERNS, p)));
        compilationUnit = compilationUnit.Padding.WithUsings(ListUtils.Map(compilationUnit.Padding.Usings, el => VisitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_USINGS, p)));
        compilationUnit = compilationUnit.WithAttributeLists(ListUtils.Map(compilationUnit.AttributeLists, el => (Cs.AttributeList?)Visit(el, p)));
        compilationUnit = compilationUnit.Padding.WithMembers(ListUtils.Map(compilationUnit.Padding.Members, el => VisitRightPadded(el, CsRightPadded.Location.COMPILATION_UNIT_MEMBERS, p)));
        compilationUnit = compilationUnit.WithEof(VisitSpace(compilationUnit.Eof, Space.Location.COMPILATION_UNIT_EOF, p)!);
        return compilationUnit;
    }

    public virtual J? VisitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, P p)
    {
        annotatedStatement = annotatedStatement.WithPrefix(VisitSpace(annotatedStatement.Prefix, CsSpace.Location.ANNOTATED_STATEMENT_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(annotatedStatement, p);
        if (tempStatement is not Cs.AnnotatedStatement)
        {
            return tempStatement;
        }
        annotatedStatement = (Cs.AnnotatedStatement) tempStatement;
        annotatedStatement = annotatedStatement.WithMarkers(VisitMarkers(annotatedStatement.Markers, p));
        annotatedStatement = annotatedStatement.WithAttributeLists(ListUtils.Map(annotatedStatement.AttributeLists, el => (Cs.AttributeList?)Visit(el, p)));
        annotatedStatement = annotatedStatement.WithStatement(VisitAndCast<Statement>(annotatedStatement.Statement, p)!);
        return annotatedStatement;
    }

    public virtual J? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, P p)
    {
        arrayRankSpecifier = arrayRankSpecifier.WithPrefix(VisitSpace(arrayRankSpecifier.Prefix, CsSpace.Location.ARRAY_RANK_SPECIFIER_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(arrayRankSpecifier, p);
        if (tempExpression is not Cs.ArrayRankSpecifier)
        {
            return tempExpression;
        }
        arrayRankSpecifier = (Cs.ArrayRankSpecifier) tempExpression;
        arrayRankSpecifier = arrayRankSpecifier.WithMarkers(VisitMarkers(arrayRankSpecifier.Markers, p));
        arrayRankSpecifier = arrayRankSpecifier.Padding.WithSizes(VisitContainer(arrayRankSpecifier.Padding.Sizes, CsContainer.Location.ARRAY_RANK_SPECIFIER_SIZES, p)!);
        return arrayRankSpecifier;
    }

    public virtual J? VisitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, P p)
    {
        assignmentOperation = assignmentOperation.WithPrefix(VisitSpace(assignmentOperation.Prefix, CsSpace.Location.ASSIGNMENT_OPERATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(assignmentOperation, p);
        if (tempStatement is not Cs.AssignmentOperation)
        {
            return tempStatement;
        }
        assignmentOperation = (Cs.AssignmentOperation) tempStatement;
        var tempExpression = (Expression) VisitExpression(assignmentOperation, p);
        if (tempExpression is not Cs.AssignmentOperation)
        {
            return tempExpression;
        }
        assignmentOperation = (Cs.AssignmentOperation) tempExpression;
        assignmentOperation = assignmentOperation.WithMarkers(VisitMarkers(assignmentOperation.Markers, p));
        assignmentOperation = assignmentOperation.WithVariable(VisitAndCast<Expression>(assignmentOperation.Variable, p)!);
        assignmentOperation = assignmentOperation.Padding.WithOperator(VisitLeftPadded(assignmentOperation.Padding.Operator, CsLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p)!);
        assignmentOperation = assignmentOperation.WithAssignment(VisitAndCast<Expression>(assignmentOperation.Assignment, p)!);
        return assignmentOperation;
    }

    public virtual J? VisitAttributeList(Cs.AttributeList attributeList, P p)
    {
        attributeList = attributeList.WithPrefix(VisitSpace(attributeList.Prefix, CsSpace.Location.ATTRIBUTE_LIST_PREFIX, p)!);
        attributeList = attributeList.WithMarkers(VisitMarkers(attributeList.Markers, p));
        attributeList = attributeList.Padding.WithTarget(VisitRightPadded(attributeList.Padding.Target, CsRightPadded.Location.ATTRIBUTE_LIST_TARGET, p));
        attributeList = attributeList.Padding.WithAttributes(ListUtils.Map(attributeList.Padding.Attributes, el => VisitRightPadded(el, CsRightPadded.Location.ATTRIBUTE_LIST_ATTRIBUTES, p)));
        return attributeList;
    }

    public virtual J? VisitAwaitExpression(Cs.AwaitExpression awaitExpression, P p)
    {
        awaitExpression = awaitExpression.WithPrefix(VisitSpace(awaitExpression.Prefix, CsSpace.Location.AWAIT_EXPRESSION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(awaitExpression, p);
        if (tempExpression is not Cs.AwaitExpression)
        {
            return tempExpression;
        }
        awaitExpression = (Cs.AwaitExpression) tempExpression;
        awaitExpression = awaitExpression.WithMarkers(VisitMarkers(awaitExpression.Markers, p));
        awaitExpression = awaitExpression.WithExpression(VisitAndCast<Expression>(awaitExpression.Expression, p)!);
        return awaitExpression;
    }

    public virtual J? VisitBinary(Cs.Binary binary, P p)
    {
        binary = binary.WithPrefix(VisitSpace(binary.Prefix, CsSpace.Location.BINARY_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(binary, p);
        if (tempExpression is not Cs.Binary)
        {
            return tempExpression;
        }
        binary = (Cs.Binary) tempExpression;
        binary = binary.WithMarkers(VisitMarkers(binary.Markers, p));
        binary = binary.WithLeft(VisitAndCast<Expression>(binary.Left, p)!);
        binary = binary.Padding.WithOperator(VisitLeftPadded(binary.Padding.Operator, CsLeftPadded.Location.BINARY_OPERATOR, p)!);
        binary = binary.WithRight(VisitAndCast<Expression>(binary.Right, p)!);
        return binary;
    }

    public virtual J? VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, P p)
    {
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.WithPrefix(VisitSpace(blockScopeNamespaceDeclaration.Prefix, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(blockScopeNamespaceDeclaration, p);
        if (tempStatement is not Cs.BlockScopeNamespaceDeclaration)
        {
            return tempStatement;
        }
        blockScopeNamespaceDeclaration = (Cs.BlockScopeNamespaceDeclaration) tempStatement;
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.WithMarkers(VisitMarkers(blockScopeNamespaceDeclaration.Markers, p));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.Padding.WithName(VisitRightPadded(blockScopeNamespaceDeclaration.Padding.Name, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME, p)!);
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.Padding.WithExterns(ListUtils.Map(blockScopeNamespaceDeclaration.Padding.Externs, el => VisitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_EXTERNS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.Padding.WithUsings(ListUtils.Map(blockScopeNamespaceDeclaration.Padding.Usings, el => VisitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.Padding.WithMembers(ListUtils.Map(blockScopeNamespaceDeclaration.Padding.Members, el => VisitRightPadded(el, CsRightPadded.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p)));
        blockScopeNamespaceDeclaration = blockScopeNamespaceDeclaration.WithEnd(VisitSpace(blockScopeNamespaceDeclaration.End, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_END, p)!);
        return blockScopeNamespaceDeclaration;
    }

    public virtual J? VisitCollectionExpression(Cs.CollectionExpression collectionExpression, P p)
    {
        collectionExpression = collectionExpression.WithPrefix(VisitSpace(collectionExpression.Prefix, CsSpace.Location.COLLECTION_EXPRESSION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(collectionExpression, p);
        if (tempExpression is not Cs.CollectionExpression)
        {
            return tempExpression;
        }
        collectionExpression = (Cs.CollectionExpression) tempExpression;
        collectionExpression = collectionExpression.WithMarkers(VisitMarkers(collectionExpression.Markers, p));
        collectionExpression = collectionExpression.Padding.WithElements(ListUtils.Map(collectionExpression.Padding.Elements, el => VisitRightPadded(el, CsRightPadded.Location.COLLECTION_EXPRESSION_ELEMENTS, p)));
        return collectionExpression;
    }

    public virtual J? VisitExpressionStatement(Cs.ExpressionStatement expressionStatement, P p)
    {
        expressionStatement = expressionStatement.WithPrefix(VisitSpace(expressionStatement.Prefix, CsSpace.Location.EXPRESSION_STATEMENT_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(expressionStatement, p);
        if (tempStatement is not Cs.ExpressionStatement)
        {
            return tempStatement;
        }
        expressionStatement = (Cs.ExpressionStatement) tempStatement;
        expressionStatement = expressionStatement.WithMarkers(VisitMarkers(expressionStatement.Markers, p));
        expressionStatement = expressionStatement.WithExpression(VisitAndCast<Expression>(expressionStatement.Expression, p)!);
        return expressionStatement;
    }

    public virtual J? VisitExternAlias(Cs.ExternAlias externAlias, P p)
    {
        externAlias = externAlias.WithPrefix(VisitSpace(externAlias.Prefix, CsSpace.Location.EXTERN_ALIAS_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(externAlias, p);
        if (tempStatement is not Cs.ExternAlias)
        {
            return tempStatement;
        }
        externAlias = (Cs.ExternAlias) tempStatement;
        externAlias = externAlias.WithMarkers(VisitMarkers(externAlias.Markers, p));
        externAlias = externAlias.Padding.WithIdentifier(VisitLeftPadded(externAlias.Padding.Identifier, CsLeftPadded.Location.EXTERN_ALIAS_IDENTIFIER, p)!);
        return externAlias;
    }

    public virtual J? VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, P p)
    {
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.WithPrefix(VisitSpace(fileScopeNamespaceDeclaration.Prefix, CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(fileScopeNamespaceDeclaration, p);
        if (tempStatement is not Cs.FileScopeNamespaceDeclaration)
        {
            return tempStatement;
        }
        fileScopeNamespaceDeclaration = (Cs.FileScopeNamespaceDeclaration) tempStatement;
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.WithMarkers(VisitMarkers(fileScopeNamespaceDeclaration.Markers, p));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.Padding.WithName(VisitRightPadded(fileScopeNamespaceDeclaration.Padding.Name, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME, p)!);
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.Padding.WithExterns(ListUtils.Map(fileScopeNamespaceDeclaration.Padding.Externs, el => VisitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_EXTERNS, p)));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.Padding.WithUsings(ListUtils.Map(fileScopeNamespaceDeclaration.Padding.Usings, el => VisitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS, p)));
        fileScopeNamespaceDeclaration = fileScopeNamespaceDeclaration.Padding.WithMembers(ListUtils.Map(fileScopeNamespaceDeclaration.Padding.Members, el => VisitRightPadded(el, CsRightPadded.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS, p)));
        return fileScopeNamespaceDeclaration;
    }

    public virtual J? VisitInterpolatedString(Cs.InterpolatedString interpolatedString, P p)
    {
        interpolatedString = interpolatedString.WithPrefix(VisitSpace(interpolatedString.Prefix, CsSpace.Location.INTERPOLATED_STRING_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(interpolatedString, p);
        if (tempExpression is not Cs.InterpolatedString)
        {
            return tempExpression;
        }
        interpolatedString = (Cs.InterpolatedString) tempExpression;
        interpolatedString = interpolatedString.WithMarkers(VisitMarkers(interpolatedString.Markers, p));
        interpolatedString = interpolatedString.Padding.WithParts(ListUtils.Map(interpolatedString.Padding.Parts, el => VisitRightPadded(el, CsRightPadded.Location.INTERPOLATED_STRING_PARTS, p)));
        return interpolatedString;
    }

    public virtual J? VisitInterpolation(Cs.Interpolation interpolation, P p)
    {
        interpolation = interpolation.WithPrefix(VisitSpace(interpolation.Prefix, CsSpace.Location.INTERPOLATION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(interpolation, p);
        if (tempExpression is not Cs.Interpolation)
        {
            return tempExpression;
        }
        interpolation = (Cs.Interpolation) tempExpression;
        interpolation = interpolation.WithMarkers(VisitMarkers(interpolation.Markers, p));
        interpolation = interpolation.Padding.WithExpression(VisitRightPadded(interpolation.Padding.Expression, CsRightPadded.Location.INTERPOLATION_EXPRESSION, p)!);
        interpolation = interpolation.Padding.WithAlignment(VisitRightPadded(interpolation.Padding.Alignment, CsRightPadded.Location.INTERPOLATION_ALIGNMENT, p));
        interpolation = interpolation.Padding.WithFormat(VisitRightPadded(interpolation.Padding.Format, CsRightPadded.Location.INTERPOLATION_FORMAT, p));
        return interpolation;
    }

    public virtual J? VisitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, P p)
    {
        nullSafeExpression = nullSafeExpression.WithPrefix(VisitSpace(nullSafeExpression.Prefix, CsSpace.Location.NULL_SAFE_EXPRESSION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(nullSafeExpression, p);
        if (tempExpression is not Cs.NullSafeExpression)
        {
            return tempExpression;
        }
        nullSafeExpression = (Cs.NullSafeExpression) tempExpression;
        nullSafeExpression = nullSafeExpression.WithMarkers(VisitMarkers(nullSafeExpression.Markers, p));
        nullSafeExpression = nullSafeExpression.Padding.WithExpression(VisitRightPadded(nullSafeExpression.Padding.Expression, CsRightPadded.Location.NULL_SAFE_EXPRESSION_EXPRESSION, p)!);
        return nullSafeExpression;
    }

    public virtual J? VisitStatementExpression(Cs.StatementExpression statementExpression, P p)
    {
        statementExpression = statementExpression.WithPrefix(VisitSpace(statementExpression.Prefix, CsSpace.Location.STATEMENT_EXPRESSION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(statementExpression, p);
        if (tempExpression is not Cs.StatementExpression)
        {
            return tempExpression;
        }
        statementExpression = (Cs.StatementExpression) tempExpression;
        statementExpression = statementExpression.WithMarkers(VisitMarkers(statementExpression.Markers, p));
        statementExpression = statementExpression.WithStatement(VisitAndCast<Statement>(statementExpression.Statement, p)!);
        return statementExpression;
    }

    public virtual J? VisitUsingDirective(Cs.UsingDirective usingDirective, P p)
    {
        usingDirective = usingDirective.WithPrefix(VisitSpace(usingDirective.Prefix, CsSpace.Location.USING_DIRECTIVE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(usingDirective, p);
        if (tempStatement is not Cs.UsingDirective)
        {
            return tempStatement;
        }
        usingDirective = (Cs.UsingDirective) tempStatement;
        usingDirective = usingDirective.WithMarkers(VisitMarkers(usingDirective.Markers, p));
        usingDirective = usingDirective.Padding.WithGlobal(VisitRightPadded(usingDirective.Padding.Global, CsRightPadded.Location.USING_DIRECTIVE_GLOBAL, p)!);
        usingDirective = usingDirective.Padding.WithStatic(VisitLeftPadded(usingDirective.Padding.Static, CsLeftPadded.Location.USING_DIRECTIVE_STATIC, p)!);
        usingDirective = usingDirective.Padding.WithUnsafe(VisitLeftPadded(usingDirective.Padding.Unsafe, CsLeftPadded.Location.USING_DIRECTIVE_UNSAFE, p)!);
        usingDirective = usingDirective.Padding.WithAlias(VisitRightPadded(usingDirective.Padding.Alias, CsRightPadded.Location.USING_DIRECTIVE_ALIAS, p));
        usingDirective = usingDirective.WithNamespaceOrType(VisitAndCast<TypeTree>(usingDirective.NamespaceOrType, p)!);
        return usingDirective;
    }

    public virtual J? VisitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, P p)
    {
        propertyDeclaration = propertyDeclaration.WithPrefix(VisitSpace(propertyDeclaration.Prefix, CsSpace.Location.PROPERTY_DECLARATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(propertyDeclaration, p);
        if (tempStatement is not Cs.PropertyDeclaration)
        {
            return tempStatement;
        }
        propertyDeclaration = (Cs.PropertyDeclaration) tempStatement;
        propertyDeclaration = propertyDeclaration.WithMarkers(VisitMarkers(propertyDeclaration.Markers, p));
        propertyDeclaration = propertyDeclaration.WithAttributeLists(ListUtils.Map(propertyDeclaration.AttributeLists, el => (Cs.AttributeList?)Visit(el, p)));
        propertyDeclaration = propertyDeclaration.WithModifiers(ListUtils.Map(propertyDeclaration.Modifiers, el => (J.Modifier?)Visit(el, p)));
        propertyDeclaration = propertyDeclaration.WithTypeExpression(VisitAndCast<TypeTree>(propertyDeclaration.TypeExpression, p)!);
        propertyDeclaration = propertyDeclaration.Padding.WithInterfaceSpecifier(VisitRightPadded(propertyDeclaration.Padding.InterfaceSpecifier, CsRightPadded.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER, p));
        propertyDeclaration = propertyDeclaration.WithName(VisitAndCast<J.Identifier>(propertyDeclaration.Name, p)!);
        propertyDeclaration = propertyDeclaration.WithAccessors(VisitAndCast<J.Block>(propertyDeclaration.Accessors, p)!);
        propertyDeclaration = propertyDeclaration.Padding.WithInitializer(VisitLeftPadded(propertyDeclaration.Padding.Initializer, CsLeftPadded.Location.PROPERTY_DECLARATION_INITIALIZER, p));
        return propertyDeclaration;
    }

    protected JContainer<J2>? VisitContainer<J2>(JContainer<J2>? container, CsContainer.Location loc, P p) where J2 : J
    {
        if (container == null) {
            return null;
        }

        Cursor = new Cursor(Cursor, container);

        var before = VisitSpace(container.Before, loc.BeforeLocation, p);
        var js = ListUtils.Map(container.Padding.Elements, t => VisitRightPadded(t, loc.ElementLocation, p));

        Cursor = Cursor.Parent!;

        return js == container.Padding.Elements && before == container.Before ?
            container :
            JContainer<J2>.Build(before!, js, container.Markers);
    }

    protected JLeftPadded<J2>? VisitLeftPadded<J2>(JLeftPadded<J2>? left, CsLeftPadded.Location loc, P p)
    {
        if (left == null)
        {
            return null;
        }

        Cursor = new Cursor(Cursor, left);

        var before = VisitSpace(left.Before, loc.BeforeLocation, p)!;
        var t = left.Element;
        if (t is Core.Tree)
        {
            t = (J2?)Visit((Core.Tree)t, p);
        }
        Cursor = Cursor.Parent!;

        // If nothing changed leave AST node the same
        if (ReferenceEquals(left.Element, t) && before == left.Before) {
            return left;
        }

        return t == null ? null : new JLeftPadded<J2>(before, t, left.Markers);
    }

    protected JRightPadded<J2>? VisitRightPadded<J2>(JRightPadded<J2>? right, CsRightPadded.Location loc, P p)
    {
        if (right == null)
        {
            return null;
        }

        var t = right.Element;
        if (t is J)
        {
            Cursor = new Cursor(Cursor, right);
            t = (J2?)Visit((J)t, p);
            Cursor = Cursor.Parent!;
        }

        if (t == null)
        {
            return null;
        }

        right = right.WithElement(t);
        right = right.WithAfter(VisitSpace(right.After, loc.AfterLocation, p));
        right = right.WithMarkers(VisitMarkers(right.Markers, p));
        return right;
    }

    protected virtual Space VisitSpace(Space space, CsSpace.Location loc, P p)
    {
        return VisitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }
}
