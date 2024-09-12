using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
public class JavaVisitor<P> : TreeVisitor<J, P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is J;
    }

    public virtual J VisitExpression(Expression expression, P p) {
        return expression;
    }

    public virtual J VisitStatement(Statement statement, P p) {
        return statement;
    }

    public virtual J? VisitAnnotatedType(J.AnnotatedType annotatedType, P p)
    {
        annotatedType = annotatedType.WithPrefix(VisitSpace(annotatedType.Prefix, Space.Location.ANNOTATED_TYPE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(annotatedType, p);
        if (tempExpression is not J.AnnotatedType)
        {
            return tempExpression;
        }
        annotatedType = (J.AnnotatedType) tempExpression;
        annotatedType = annotatedType.WithMarkers(VisitMarkers(annotatedType.Markers, p));
        annotatedType = annotatedType.WithAnnotations(ListUtils.Map(annotatedType.Annotations, el => (J.Annotation?)Visit(el, p)));
        annotatedType = annotatedType.WithTypeExpression(VisitAndCast<TypeTree>(annotatedType.TypeExpression, p)!);
        return annotatedType;
    }

    public virtual J? VisitAnnotation(J.Annotation annotation, P p)
    {
        annotation = annotation.WithPrefix(VisitSpace(annotation.Prefix, Space.Location.ANNOTATION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(annotation, p);
        if (tempExpression is not J.Annotation)
        {
            return tempExpression;
        }
        annotation = (J.Annotation) tempExpression;
        annotation = annotation.WithMarkers(VisitMarkers(annotation.Markers, p));
        annotation = annotation.WithAnnotationType(VisitAndCast<NameTree>(annotation.AnnotationType, p)!);
        annotation = annotation.Padding.WithArguments(VisitContainer(annotation.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, p));
        return annotation;
    }

    public virtual J? VisitArrayAccess(J.ArrayAccess arrayAccess, P p)
    {
        arrayAccess = arrayAccess.WithPrefix(VisitSpace(arrayAccess.Prefix, Space.Location.ARRAY_ACCESS_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(arrayAccess, p);
        if (tempExpression is not J.ArrayAccess)
        {
            return tempExpression;
        }
        arrayAccess = (J.ArrayAccess) tempExpression;
        arrayAccess = arrayAccess.WithMarkers(VisitMarkers(arrayAccess.Markers, p));
        arrayAccess = arrayAccess.WithIndexed(VisitAndCast<Expression>(arrayAccess.Indexed, p)!);
        arrayAccess = arrayAccess.WithDimension(VisitAndCast<J.ArrayDimension>(arrayAccess.Dimension, p)!);
        return arrayAccess;
    }

    public virtual J? VisitArrayType(J.ArrayType arrayType, P p)
    {
        arrayType = arrayType.WithPrefix(VisitSpace(arrayType.Prefix, Space.Location.ARRAY_TYPE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(arrayType, p);
        if (tempExpression is not J.ArrayType)
        {
            return tempExpression;
        }
        arrayType = (J.ArrayType) tempExpression;
        arrayType = arrayType.WithMarkers(VisitMarkers(arrayType.Markers, p));
        arrayType = arrayType.WithElementType(VisitAndCast<TypeTree>(arrayType.ElementType, p)!);
        arrayType = arrayType.WithAnnotations(arrayType.Annotations?.Map(el => (J.Annotation?)Visit(el, p)));
        arrayType = arrayType.WithDimension(arrayType.Dimension?.WithBefore(VisitSpace(arrayType.Dimension.Before, Space.Location.DIMENSION_PREFIX, p)).WithElement(VisitSpace(arrayType.Dimension.Element, Space.Location.DIMENSION, p)));
        return arrayType;
    }

    public virtual J? VisitAssert(J.Assert assert, P p)
    {
        assert = assert.WithPrefix(VisitSpace(assert.Prefix, Space.Location.ASSERT_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(assert, p);
        if (tempStatement is not J.Assert)
        {
            return tempStatement;
        }
        assert = (J.Assert) tempStatement;
        assert = assert.WithMarkers(VisitMarkers(assert.Markers, p));
        assert = assert.WithCondition(VisitAndCast<Expression>(assert.Condition, p)!);
        assert = assert.WithDetail(VisitLeftPadded(assert.Detail, JLeftPadded.Location.ASSERT_DETAIL, p));
        return assert;
    }

    public virtual J? VisitAssignment(J.Assignment assignment, P p)
    {
        assignment = assignment.WithPrefix(VisitSpace(assignment.Prefix, Space.Location.ASSIGNMENT_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(assignment, p);
        if (tempStatement is not J.Assignment)
        {
            return tempStatement;
        }
        assignment = (J.Assignment) tempStatement;
        var tempExpression = (Expression) VisitExpression(assignment, p);
        if (tempExpression is not J.Assignment)
        {
            return tempExpression;
        }
        assignment = (J.Assignment) tempExpression;
        assignment = assignment.WithMarkers(VisitMarkers(assignment.Markers, p));
        assignment = assignment.WithVariable(VisitAndCast<Expression>(assignment.Variable, p)!);
        assignment = assignment.Padding.WithExpression(VisitLeftPadded(assignment.Padding.Expression, JLeftPadded.Location.ASSIGNMENT, p)!);
        return assignment;
    }

    public virtual J? VisitAssignmentOperation(J.AssignmentOperation assignmentOperation, P p)
    {
        assignmentOperation = assignmentOperation.WithPrefix(VisitSpace(assignmentOperation.Prefix, Space.Location.ASSIGNMENT_OPERATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(assignmentOperation, p);
        if (tempStatement is not J.AssignmentOperation)
        {
            return tempStatement;
        }
        assignmentOperation = (J.AssignmentOperation) tempStatement;
        var tempExpression = (Expression) VisitExpression(assignmentOperation, p);
        if (tempExpression is not J.AssignmentOperation)
        {
            return tempExpression;
        }
        assignmentOperation = (J.AssignmentOperation) tempExpression;
        assignmentOperation = assignmentOperation.WithMarkers(VisitMarkers(assignmentOperation.Markers, p));
        assignmentOperation = assignmentOperation.WithVariable(VisitAndCast<Expression>(assignmentOperation.Variable, p)!);
        assignmentOperation = assignmentOperation.Padding.WithOperator(VisitLeftPadded(assignmentOperation.Padding.Operator, JLeftPadded.Location.ASSIGNMENT_OPERATION_OPERATOR, p)!);
        assignmentOperation = assignmentOperation.WithAssignment(VisitAndCast<Expression>(assignmentOperation.Assignment, p)!);
        return assignmentOperation;
    }

    public virtual J? VisitBinary(J.Binary binary, P p)
    {
        binary = binary.WithPrefix(VisitSpace(binary.Prefix, Space.Location.BINARY_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(binary, p);
        if (tempExpression is not J.Binary)
        {
            return tempExpression;
        }
        binary = (J.Binary) tempExpression;
        binary = binary.WithMarkers(VisitMarkers(binary.Markers, p));
        binary = binary.WithLeft(VisitAndCast<Expression>(binary.Left, p)!);
        binary = binary.Padding.WithOperator(VisitLeftPadded(binary.Padding.Operator, JLeftPadded.Location.BINARY_OPERATOR, p)!);
        binary = binary.WithRight(VisitAndCast<Expression>(binary.Right, p)!);
        return binary;
    }

    public virtual J? VisitBlock(J.Block block, P p)
    {
        block = block.WithPrefix(VisitSpace(block.Prefix, Space.Location.BLOCK_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(block, p);
        if (tempStatement is not J.Block)
        {
            return tempStatement;
        }
        block = (J.Block) tempStatement;
        block = block.WithMarkers(VisitMarkers(block.Markers, p));
        block = block.Padding.WithStatic(VisitRightPadded(block.Padding.Static, JRightPadded.Location.STATIC_INIT, p)!);
        block = block.Padding.WithStatements(ListUtils.Map(block.Padding.Statements, el => VisitRightPadded(el, JRightPadded.Location.BLOCK_STATEMENT, p)));
        block = block.WithEnd(VisitSpace(block.End, Space.Location.BLOCK_END, p)!);
        return block;
    }

    public virtual J? VisitBreak(J.Break @break, P p)
    {
        @break = @break.WithPrefix(VisitSpace(@break.Prefix, Space.Location.BREAK_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@break, p);
        if (tempStatement is not J.Break)
        {
            return tempStatement;
        }
        @break = (J.Break) tempStatement;
        @break = @break.WithMarkers(VisitMarkers(@break.Markers, p));
        @break = @break.WithLabel(VisitAndCast<J.Identifier>(@break.Label, p));
        return @break;
    }

    public virtual J? VisitCase(J.Case @case, P p)
    {
        @case = @case.WithPrefix(VisitSpace(@case.Prefix, Space.Location.CASE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@case, p);
        if (tempStatement is not J.Case)
        {
            return tempStatement;
        }
        @case = (J.Case) tempStatement;
        @case = @case.WithMarkers(VisitMarkers(@case.Markers, p));
        @case = @case.Padding.WithExpressions(VisitContainer(@case.Padding.Expressions, JContainer.Location.CASE_EXPRESSION, p)!);
        @case = @case.Padding.WithStatements(VisitContainer(@case.Padding.Statements, JContainer.Location.CASE, p)!);
        @case = @case.Padding.WithBody(VisitRightPadded(@case.Padding.Body, JRightPadded.Location.CASE_BODY, p));
        return @case;
    }

    public virtual J? VisitClassDeclaration(J.ClassDeclaration classDeclaration, P p)
    {
        classDeclaration = classDeclaration.WithPrefix(VisitSpace(classDeclaration.Prefix, Space.Location.CLASS_DECLARATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(classDeclaration, p);
        if (tempStatement is not J.ClassDeclaration)
        {
            return tempStatement;
        }
        classDeclaration = (J.ClassDeclaration) tempStatement;
        classDeclaration = classDeclaration.WithMarkers(VisitMarkers(classDeclaration.Markers, p));
        classDeclaration = classDeclaration.WithLeadingAnnotations(ListUtils.Map(classDeclaration.LeadingAnnotations, el => (J.Annotation?)Visit(el, p)));
        classDeclaration = classDeclaration.WithModifiers(ListUtils.Map(classDeclaration.Modifiers, el => (J.Modifier?)Visit(el, p)));
        classDeclaration = classDeclaration.Padding.WithDeclarationKind(VisitAndCast<J.ClassDeclaration.Kind>(classDeclaration.Padding.DeclarationKind, p)!);
        classDeclaration = classDeclaration.WithName(VisitAndCast<J.Identifier>(classDeclaration.Name, p)!);
        classDeclaration = classDeclaration.Padding.WithTypeParameters(VisitContainer(classDeclaration.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, p));
        classDeclaration = classDeclaration.Padding.WithPrimaryConstructor(VisitContainer(classDeclaration.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, p));
        classDeclaration = classDeclaration.Padding.WithExtends(VisitLeftPadded(classDeclaration.Padding.Extends, JLeftPadded.Location.EXTENDS, p));
        classDeclaration = classDeclaration.Padding.WithImplements(VisitContainer(classDeclaration.Padding.Implements, JContainer.Location.IMPLEMENTS, p));
        classDeclaration = classDeclaration.Padding.WithPermits(VisitContainer(classDeclaration.Padding.Permits, JContainer.Location.PERMITS, p));
        classDeclaration = classDeclaration.WithBody(VisitAndCast<J.Block>(classDeclaration.Body, p)!);
        return classDeclaration;
    }

    public virtual J? VisitClassDeclarationKind(J.ClassDeclaration.Kind kind, P p)
    {
        kind = kind.WithPrefix(VisitSpace(kind.Prefix, Space.Location.CLASS_KIND, p)!);
        kind = kind.WithMarkers(VisitMarkers(kind.Markers, p));
        kind = kind.WithAnnotations(ListUtils.Map(kind.Annotations, el => (J.Annotation?)Visit(el, p)));
        return kind;
    }

    public virtual J? VisitCompilationUnit(J.CompilationUnit compilationUnit, P p)
    {
        compilationUnit = compilationUnit.WithPrefix(VisitSpace(compilationUnit.Prefix, Space.Location.COMPILATION_UNIT_PREFIX, p)!);
        compilationUnit = compilationUnit.WithMarkers(VisitMarkers(compilationUnit.Markers, p));
        compilationUnit = compilationUnit.Padding.WithPackageDeclaration(VisitRightPadded(compilationUnit.Padding.PackageDeclaration, JRightPadded.Location.PACKAGE, p));
        compilationUnit = compilationUnit.Padding.WithImports(ListUtils.Map(compilationUnit.Padding.Imports, el => VisitRightPadded(el, JRightPadded.Location.IMPORT, p)));
        compilationUnit = compilationUnit.WithClasses(ListUtils.Map(compilationUnit.Classes, el => (J.ClassDeclaration?)Visit(el, p)));
        compilationUnit = compilationUnit.WithEof(VisitSpace(compilationUnit.Eof, Space.Location.COMPILATION_UNIT_EOF, p)!);
        return compilationUnit;
    }

    public virtual J? VisitContinue(J.Continue @continue, P p)
    {
        @continue = @continue.WithPrefix(VisitSpace(@continue.Prefix, Space.Location.CONTINUE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@continue, p);
        if (tempStatement is not J.Continue)
        {
            return tempStatement;
        }
        @continue = (J.Continue) tempStatement;
        @continue = @continue.WithMarkers(VisitMarkers(@continue.Markers, p));
        @continue = @continue.WithLabel(VisitAndCast<J.Identifier>(@continue.Label, p));
        return @continue;
    }

    public virtual J? VisitDoWhileLoop(J.DoWhileLoop doWhileLoop, P p)
    {
        doWhileLoop = doWhileLoop.WithPrefix(VisitSpace(doWhileLoop.Prefix, Space.Location.DO_WHILE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(doWhileLoop, p);
        if (tempStatement is not J.DoWhileLoop)
        {
            return tempStatement;
        }
        doWhileLoop = (J.DoWhileLoop) tempStatement;
        doWhileLoop = doWhileLoop.WithMarkers(VisitMarkers(doWhileLoop.Markers, p));
        doWhileLoop = doWhileLoop.Padding.WithBody(VisitRightPadded(doWhileLoop.Padding.Body, JRightPadded.Location.WHILE_BODY, p)!);
        doWhileLoop = doWhileLoop.Padding.WithWhileCondition(VisitLeftPadded(doWhileLoop.Padding.WhileCondition, JLeftPadded.Location.WHILE_CONDITION, p)!);
        return doWhileLoop;
    }

    public virtual J? VisitEmpty(J.Empty empty, P p)
    {
        empty = empty.WithPrefix(VisitSpace(empty.Prefix, Space.Location.EMPTY_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(empty, p);
        if (tempStatement is not J.Empty)
        {
            return tempStatement;
        }
        empty = (J.Empty) tempStatement;
        var tempExpression = (Expression) VisitExpression(empty, p);
        if (tempExpression is not J.Empty)
        {
            return tempExpression;
        }
        empty = (J.Empty) tempExpression;
        empty = empty.WithMarkers(VisitMarkers(empty.Markers, p));
        return empty;
    }

    public virtual J? VisitEnumValue(J.EnumValue enumValue, P p)
    {
        enumValue = enumValue.WithPrefix(VisitSpace(enumValue.Prefix, Space.Location.ENUM_VALUE_PREFIX, p)!);
        enumValue = enumValue.WithMarkers(VisitMarkers(enumValue.Markers, p));
        enumValue = enumValue.WithAnnotations(ListUtils.Map(enumValue.Annotations, el => (J.Annotation?)Visit(el, p)));
        enumValue = enumValue.WithName(VisitAndCast<J.Identifier>(enumValue.Name, p)!);
        enumValue = enumValue.WithInitializer(VisitAndCast<J.NewClass>(enumValue.Initializer, p));
        return enumValue;
    }

    public virtual J? VisitEnumValueSet(J.EnumValueSet enumValueSet, P p)
    {
        enumValueSet = enumValueSet.WithPrefix(VisitSpace(enumValueSet.Prefix, Space.Location.ENUM_VALUE_SET_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(enumValueSet, p);
        if (tempStatement is not J.EnumValueSet)
        {
            return tempStatement;
        }
        enumValueSet = (J.EnumValueSet) tempStatement;
        enumValueSet = enumValueSet.WithMarkers(VisitMarkers(enumValueSet.Markers, p));
        enumValueSet = enumValueSet.Padding.WithEnums(ListUtils.Map(enumValueSet.Padding.Enums, el => VisitRightPadded(el, JRightPadded.Location.ENUM_VALUE, p)));
        return enumValueSet;
    }

    public virtual J? VisitFieldAccess(J.FieldAccess fieldAccess, P p)
    {
        fieldAccess = fieldAccess.WithPrefix(VisitSpace(fieldAccess.Prefix, Space.Location.FIELD_ACCESS_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(fieldAccess, p);
        if (tempStatement is not J.FieldAccess)
        {
            return tempStatement;
        }
        fieldAccess = (J.FieldAccess) tempStatement;
        var tempExpression = (Expression) VisitExpression(fieldAccess, p);
        if (tempExpression is not J.FieldAccess)
        {
            return tempExpression;
        }
        fieldAccess = (J.FieldAccess) tempExpression;
        fieldAccess = fieldAccess.WithMarkers(VisitMarkers(fieldAccess.Markers, p));
        fieldAccess = fieldAccess.WithTarget(VisitAndCast<Expression>(fieldAccess.Target, p)!);
        fieldAccess = fieldAccess.Padding.WithName(VisitLeftPadded(fieldAccess.Padding.Name, JLeftPadded.Location.FIELD_ACCESS_NAME, p)!);
        return fieldAccess;
    }

    public virtual J? VisitForEachLoop(J.ForEachLoop forEachLoop, P p)
    {
        forEachLoop = forEachLoop.WithPrefix(VisitSpace(forEachLoop.Prefix, Space.Location.FOR_EACH_LOOP_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(forEachLoop, p);
        if (tempStatement is not J.ForEachLoop)
        {
            return tempStatement;
        }
        forEachLoop = (J.ForEachLoop) tempStatement;
        forEachLoop = forEachLoop.WithMarkers(VisitMarkers(forEachLoop.Markers, p));
        forEachLoop = forEachLoop.WithLoopControl(VisitAndCast<J.ForEachLoop.Control>(forEachLoop.LoopControl, p)!);
        forEachLoop = forEachLoop.Padding.WithBody(VisitRightPadded(forEachLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p)!);
        return forEachLoop;
    }

    public virtual J? VisitForEachControl(J.ForEachLoop.Control control, P p)
    {
        control = control.WithPrefix(VisitSpace(control.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p)!);
        control = control.WithMarkers(VisitMarkers(control.Markers, p));
        control = control.Padding.WithVariable(VisitRightPadded(control.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, p)!);
        control = control.Padding.WithIterable(VisitRightPadded(control.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, p)!);
        return control;
    }

    public virtual J? VisitForLoop(J.ForLoop forLoop, P p)
    {
        forLoop = forLoop.WithPrefix(VisitSpace(forLoop.Prefix, Space.Location.FOR_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(forLoop, p);
        if (tempStatement is not J.ForLoop)
        {
            return tempStatement;
        }
        forLoop = (J.ForLoop) tempStatement;
        forLoop = forLoop.WithMarkers(VisitMarkers(forLoop.Markers, p));
        forLoop = forLoop.WithLoopControl(VisitAndCast<J.ForLoop.Control>(forLoop.LoopControl, p)!);
        forLoop = forLoop.Padding.WithBody(VisitRightPadded(forLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p)!);
        return forLoop;
    }

    public virtual J? VisitForControl(J.ForLoop.Control control, P p)
    {
        control = control.WithPrefix(VisitSpace(control.Prefix, Space.Location.FOR_CONTROL_PREFIX, p)!);
        control = control.WithMarkers(VisitMarkers(control.Markers, p));
        control = control.Padding.WithInit(ListUtils.Map(control.Padding.Init, el => VisitRightPadded(el, JRightPadded.Location.FOR_INIT, p)));
        control = control.Padding.WithCondition(VisitRightPadded(control.Padding.Condition, JRightPadded.Location.FOR_CONDITION, p)!);
        control = control.Padding.WithUpdate(ListUtils.Map(control.Padding.Update, el => VisitRightPadded(el, JRightPadded.Location.FOR_UPDATE, p)));
        return control;
    }

    public virtual J? VisitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, P p)
    {
        parenthesizedTypeTree = parenthesizedTypeTree.WithPrefix(VisitSpace(parenthesizedTypeTree.Prefix, Space.Location.PARENTHESES_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(parenthesizedTypeTree, p);
        if (tempExpression is not J.ParenthesizedTypeTree)
        {
            return tempExpression;
        }
        parenthesizedTypeTree = (J.ParenthesizedTypeTree) tempExpression;
        parenthesizedTypeTree = parenthesizedTypeTree.WithMarkers(VisitMarkers(parenthesizedTypeTree.Markers, p));
        parenthesizedTypeTree = parenthesizedTypeTree.WithAnnotations(ListUtils.Map(parenthesizedTypeTree.Annotations, el => (J.Annotation?)Visit(el, p)));
        parenthesizedTypeTree = parenthesizedTypeTree.WithParenthesizedType(VisitAndCast<J.Parentheses<TypeTree>>(parenthesizedTypeTree.ParenthesizedType, p)!);
        return parenthesizedTypeTree;
    }

    public virtual J? VisitIdentifier(J.Identifier identifier, P p)
    {
        identifier = identifier.WithPrefix(VisitSpace(identifier.Prefix, Space.Location.IDENTIFIER_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(identifier, p);
        if (tempExpression is not J.Identifier)
        {
            return tempExpression;
        }
        identifier = (J.Identifier) tempExpression;
        identifier = identifier.WithMarkers(VisitMarkers(identifier.Markers, p));
        identifier = identifier.WithAnnotations(ListUtils.Map(identifier.Annotations, el => (J.Annotation?)Visit(el, p)));
        return identifier;
    }

    public virtual J? VisitIf(J.If @if, P p)
    {
        @if = @if.WithPrefix(VisitSpace(@if.Prefix, Space.Location.IF_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@if, p);
        if (tempStatement is not J.If)
        {
            return tempStatement;
        }
        @if = (J.If) tempStatement;
        @if = @if.WithMarkers(VisitMarkers(@if.Markers, p));
        @if = @if.WithIfCondition(VisitAndCast<J.ControlParentheses<Expression>>(@if.IfCondition, p)!);
        @if = @if.Padding.WithThenPart(VisitRightPadded(@if.Padding.ThenPart, JRightPadded.Location.IF_THEN, p)!);
        @if = @if.WithElsePart(VisitAndCast<J.If.Else>(@if.ElsePart, p));
        return @if;
    }

    public virtual J? VisitElse(J.If.Else @else, P p)
    {
        @else = @else.WithPrefix(VisitSpace(@else.Prefix, Space.Location.ELSE_PREFIX, p)!);
        @else = @else.WithMarkers(VisitMarkers(@else.Markers, p));
        @else = @else.Padding.WithBody(VisitRightPadded(@else.Padding.Body, JRightPadded.Location.IF_ELSE, p)!);
        return @else;
    }

    public virtual J? VisitImport(J.Import import, P p)
    {
        import = import.WithPrefix(VisitSpace(import.Prefix, Space.Location.IMPORT_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(import, p);
        if (tempStatement is not J.Import)
        {
            return tempStatement;
        }
        import = (J.Import) tempStatement;
        import = import.WithMarkers(VisitMarkers(import.Markers, p));
        import = import.Padding.WithStatic(VisitLeftPadded(import.Padding.Static, JLeftPadded.Location.STATIC_IMPORT, p)!);
        import = import.WithQualid(VisitAndCast<J.FieldAccess>(import.Qualid, p)!);
        import = import.Padding.WithAlias(VisitLeftPadded(import.Padding.Alias, JLeftPadded.Location.IMPORT_ALIAS_PREFIX, p));
        return import;
    }

    public virtual J? VisitInstanceOf(J.InstanceOf instanceOf, P p)
    {
        instanceOf = instanceOf.WithPrefix(VisitSpace(instanceOf.Prefix, Space.Location.INSTANCEOF_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(instanceOf, p);
        if (tempExpression is not J.InstanceOf)
        {
            return tempExpression;
        }
        instanceOf = (J.InstanceOf) tempExpression;
        instanceOf = instanceOf.WithMarkers(VisitMarkers(instanceOf.Markers, p));
        instanceOf = instanceOf.Padding.WithExpression(VisitRightPadded(instanceOf.Padding.Expression, JRightPadded.Location.INSTANCEOF, p)!);
        instanceOf = instanceOf.WithClazz(VisitAndCast<J>(instanceOf.Clazz, p)!);
        instanceOf = instanceOf.WithPattern(VisitAndCast<J>(instanceOf.Pattern, p));
        return instanceOf;
    }

    public virtual J? VisitIntersectionType(J.IntersectionType intersectionType, P p)
    {
        intersectionType = intersectionType.WithPrefix(VisitSpace(intersectionType.Prefix, Space.Location.INTERSECTION_TYPE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(intersectionType, p);
        if (tempExpression is not J.IntersectionType)
        {
            return tempExpression;
        }
        intersectionType = (J.IntersectionType) tempExpression;
        intersectionType = intersectionType.WithMarkers(VisitMarkers(intersectionType.Markers, p));
        intersectionType = intersectionType.Padding.WithBounds(VisitContainer(intersectionType.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, p)!);
        return intersectionType;
    }

    public virtual J? VisitLabel(J.Label label, P p)
    {
        label = label.WithPrefix(VisitSpace(label.Prefix, Space.Location.LABEL_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(label, p);
        if (tempStatement is not J.Label)
        {
            return tempStatement;
        }
        label = (J.Label) tempStatement;
        label = label.WithMarkers(VisitMarkers(label.Markers, p));
        label = label.Padding.WithName(VisitRightPadded(label.Padding.Name, JRightPadded.Location.LABEL, p)!);
        label = label.WithStatement(VisitAndCast<Statement>(label.Statement, p)!);
        return label;
    }

    public virtual J? VisitLambda(J.Lambda lambda, P p)
    {
        lambda = lambda.WithPrefix(VisitSpace(lambda.Prefix, Space.Location.LAMBDA_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(lambda, p);
        if (tempStatement is not J.Lambda)
        {
            return tempStatement;
        }
        lambda = (J.Lambda) tempStatement;
        var tempExpression = (Expression) VisitExpression(lambda, p);
        if (tempExpression is not J.Lambda)
        {
            return tempExpression;
        }
        lambda = (J.Lambda) tempExpression;
        lambda = lambda.WithMarkers(VisitMarkers(lambda.Markers, p));
        lambda = lambda.WithParams(VisitAndCast<J.Lambda.Parameters>(lambda.Params, p)!);
        lambda = lambda.WithArrow(VisitSpace(lambda.Arrow, Space.Location.LAMBDA_ARROW_PREFIX, p)!);
        lambda = lambda.WithBody(VisitAndCast<J>(lambda.Body, p)!);
        return lambda;
    }

    public virtual J? VisitLambdaParameters(J.Lambda.Parameters parameters, P p)
    {
        parameters = parameters.WithPrefix(VisitSpace(parameters.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p)!);
        parameters = parameters.WithMarkers(VisitMarkers(parameters.Markers, p));
        parameters = parameters.Padding.WithElements(ListUtils.Map(parameters.Padding.Elements, el => VisitRightPadded(el, JRightPadded.Location.LAMBDA_PARAM, p)));
        return parameters;
    }

    public virtual J? VisitLiteral(J.Literal literal, P p)
    {
        literal = literal.WithPrefix(VisitSpace(literal.Prefix, Space.Location.LITERAL_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(literal, p);
        if (tempExpression is not J.Literal)
        {
            return tempExpression;
        }
        literal = (J.Literal) tempExpression;
        literal = literal.WithMarkers(VisitMarkers(literal.Markers, p));
        return literal;
    }

    public virtual J? VisitMemberReference(J.MemberReference memberReference, P p)
    {
        memberReference = memberReference.WithPrefix(VisitSpace(memberReference.Prefix, Space.Location.MEMBER_REFERENCE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(memberReference, p);
        if (tempExpression is not J.MemberReference)
        {
            return tempExpression;
        }
        memberReference = (J.MemberReference) tempExpression;
        memberReference = memberReference.WithMarkers(VisitMarkers(memberReference.Markers, p));
        memberReference = memberReference.Padding.WithContaining(VisitRightPadded(memberReference.Padding.Containing, JRightPadded.Location.MEMBER_REFERENCE_CONTAINING, p)!);
        memberReference = memberReference.Padding.WithTypeParameters(VisitContainer(memberReference.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, p));
        memberReference = memberReference.Padding.WithReference(VisitLeftPadded(memberReference.Padding.Reference, JLeftPadded.Location.MEMBER_REFERENCE_NAME, p)!);
        return memberReference;
    }

    public virtual J? VisitMethodDeclaration(J.MethodDeclaration methodDeclaration, P p)
    {
        methodDeclaration = methodDeclaration.WithPrefix(VisitSpace(methodDeclaration.Prefix, Space.Location.METHOD_DECLARATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(methodDeclaration, p);
        if (tempStatement is not J.MethodDeclaration)
        {
            return tempStatement;
        }
        methodDeclaration = (J.MethodDeclaration) tempStatement;
        methodDeclaration = methodDeclaration.WithMarkers(VisitMarkers(methodDeclaration.Markers, p));
        methodDeclaration = methodDeclaration.WithLeadingAnnotations(ListUtils.Map(methodDeclaration.LeadingAnnotations, el => (J.Annotation?)Visit(el, p)));
        methodDeclaration = methodDeclaration.WithModifiers(ListUtils.Map(methodDeclaration.Modifiers, el => (J.Modifier?)Visit(el, p)));
        methodDeclaration = methodDeclaration.Annotations.WithTypeParameters(VisitAndCast<J.TypeParameters>(methodDeclaration.Annotations.TypeParameters, p));
        methodDeclaration = methodDeclaration.WithReturnTypeExpression(VisitAndCast<TypeTree>(methodDeclaration.ReturnTypeExpression, p));
        methodDeclaration = methodDeclaration.Annotations.WithName(methodDeclaration.Annotations.Name.WithAnnotations(ListUtils.Map(methodDeclaration.Annotations.Name.Annotations, el => (J.Annotation?)Visit(el, p))).WithIdentifier(VisitAndCast<J.Identifier>(methodDeclaration.Annotations.Name.Identifier, p)!));
        methodDeclaration = methodDeclaration.Padding.WithParameters(VisitContainer(methodDeclaration.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, p)!);
        methodDeclaration = methodDeclaration.Padding.WithThrows(VisitContainer(methodDeclaration.Padding.Throws, JContainer.Location.THROWS, p));
        methodDeclaration = methodDeclaration.WithBody(VisitAndCast<J.Block>(methodDeclaration.Body, p));
        methodDeclaration = methodDeclaration.Padding.WithDefaultValue(VisitLeftPadded(methodDeclaration.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p));
        return methodDeclaration;
    }

    public virtual J? VisitMethodInvocation(J.MethodInvocation methodInvocation, P p)
    {
        methodInvocation = methodInvocation.WithPrefix(VisitSpace(methodInvocation.Prefix, Space.Location.METHOD_INVOCATION_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(methodInvocation, p);
        if (tempStatement is not J.MethodInvocation)
        {
            return tempStatement;
        }
        methodInvocation = (J.MethodInvocation) tempStatement;
        var tempExpression = (Expression) VisitExpression(methodInvocation, p);
        if (tempExpression is not J.MethodInvocation)
        {
            return tempExpression;
        }
        methodInvocation = (J.MethodInvocation) tempExpression;
        methodInvocation = methodInvocation.WithMarkers(VisitMarkers(methodInvocation.Markers, p));
        methodInvocation = methodInvocation.Padding.WithSelect(VisitRightPadded(methodInvocation.Padding.Select, JRightPadded.Location.METHOD_SELECT, p));
        methodInvocation = methodInvocation.Padding.WithTypeParameters(VisitContainer(methodInvocation.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, p));
        methodInvocation = methodInvocation.WithName(VisitAndCast<J.Identifier>(methodInvocation.Name, p)!);
        methodInvocation = methodInvocation.Padding.WithArguments(VisitContainer(methodInvocation.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, p)!);
        return methodInvocation;
    }

    public virtual J? VisitModifier(J.Modifier modifier, P p)
    {
        modifier = modifier.WithPrefix(VisitSpace(modifier.Prefix, Space.Location.MODIFIER_PREFIX, p)!);
        modifier = modifier.WithMarkers(VisitMarkers(modifier.Markers, p));
        modifier = modifier.WithAnnotations(ListUtils.Map(modifier.Annotations, el => (J.Annotation?)Visit(el, p)));
        return modifier;
    }

    public virtual J? VisitMultiCatch(J.MultiCatch multiCatch, P p)
    {
        multiCatch = multiCatch.WithPrefix(VisitSpace(multiCatch.Prefix, Space.Location.MULTI_CATCH_PREFIX, p)!);
        multiCatch = multiCatch.WithMarkers(VisitMarkers(multiCatch.Markers, p));
        multiCatch = multiCatch.Padding.WithAlternatives(ListUtils.Map(multiCatch.Padding.Alternatives, el => VisitRightPadded(el, JRightPadded.Location.CATCH_ALTERNATIVE, p)));
        return multiCatch;
    }

    public virtual J? VisitNewArray(J.NewArray newArray, P p)
    {
        newArray = newArray.WithPrefix(VisitSpace(newArray.Prefix, Space.Location.NEW_ARRAY_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(newArray, p);
        if (tempExpression is not J.NewArray)
        {
            return tempExpression;
        }
        newArray = (J.NewArray) tempExpression;
        newArray = newArray.WithMarkers(VisitMarkers(newArray.Markers, p));
        newArray = newArray.WithTypeExpression(VisitAndCast<TypeTree>(newArray.TypeExpression, p));
        newArray = newArray.WithDimensions(ListUtils.Map(newArray.Dimensions, el => (J.ArrayDimension?)Visit(el, p)));
        newArray = newArray.Padding.WithInitializer(VisitContainer(newArray.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, p));
        return newArray;
    }

    public virtual J? VisitArrayDimension(J.ArrayDimension arrayDimension, P p)
    {
        arrayDimension = arrayDimension.WithPrefix(VisitSpace(arrayDimension.Prefix, Space.Location.DIMENSION_PREFIX, p)!);
        arrayDimension = arrayDimension.WithMarkers(VisitMarkers(arrayDimension.Markers, p));
        arrayDimension = arrayDimension.Padding.WithIndex(VisitRightPadded(arrayDimension.Padding.Index, JRightPadded.Location.ARRAY_INDEX, p)!);
        return arrayDimension;
    }

    public virtual J? VisitNewClass(J.NewClass newClass, P p)
    {
        newClass = newClass.WithPrefix(VisitSpace(newClass.Prefix, Space.Location.NEW_CLASS_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(newClass, p);
        if (tempStatement is not J.NewClass)
        {
            return tempStatement;
        }
        newClass = (J.NewClass) tempStatement;
        var tempExpression = (Expression) VisitExpression(newClass, p);
        if (tempExpression is not J.NewClass)
        {
            return tempExpression;
        }
        newClass = (J.NewClass) tempExpression;
        newClass = newClass.WithMarkers(VisitMarkers(newClass.Markers, p));
        newClass = newClass.Padding.WithEnclosing(VisitRightPadded(newClass.Padding.Enclosing, JRightPadded.Location.NEW_CLASS_ENCLOSING, p));
        newClass = newClass.WithNew(VisitSpace(newClass.New, Space.Location.NEW_PREFIX, p)!);
        newClass = newClass.WithClazz(VisitAndCast<TypeTree>(newClass.Clazz, p));
        newClass = newClass.Padding.WithArguments(VisitContainer(newClass.Padding.Arguments, JContainer.Location.NEW_CLASS_ARGUMENTS, p)!);
        newClass = newClass.WithBody(VisitAndCast<J.Block>(newClass.Body, p));
        return newClass;
    }

    public virtual J? VisitNullableType(J.NullableType nullableType, P p)
    {
        nullableType = nullableType.WithPrefix(VisitSpace(nullableType.Prefix, Space.Location.NULLABLE_TYPE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(nullableType, p);
        if (tempExpression is not J.NullableType)
        {
            return tempExpression;
        }
        nullableType = (J.NullableType) tempExpression;
        nullableType = nullableType.WithMarkers(VisitMarkers(nullableType.Markers, p));
        nullableType = nullableType.WithAnnotations(ListUtils.Map(nullableType.Annotations, el => (J.Annotation?)Visit(el, p)));
        nullableType = nullableType.Padding.WithTypeTree(VisitRightPadded(nullableType.Padding.TypeTree, JRightPadded.Location.NULLABLE, p)!);
        return nullableType;
    }

    public virtual J? VisitPackage(J.Package package, P p)
    {
        package = package.WithPrefix(VisitSpace(package.Prefix, Space.Location.PACKAGE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(package, p);
        if (tempStatement is not J.Package)
        {
            return tempStatement;
        }
        package = (J.Package) tempStatement;
        package = package.WithMarkers(VisitMarkers(package.Markers, p));
        package = package.WithExpression(VisitAndCast<Expression>(package.Expression, p)!);
        package = package.WithAnnotations(ListUtils.Map(package.Annotations, el => (J.Annotation?)Visit(el, p)));
        return package;
    }

    public virtual J? VisitParameterizedType(J.ParameterizedType parameterizedType, P p)
    {
        parameterizedType = parameterizedType.WithPrefix(VisitSpace(parameterizedType.Prefix, Space.Location.PARAMETERIZED_TYPE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(parameterizedType, p);
        if (tempExpression is not J.ParameterizedType)
        {
            return tempExpression;
        }
        parameterizedType = (J.ParameterizedType) tempExpression;
        parameterizedType = parameterizedType.WithMarkers(VisitMarkers(parameterizedType.Markers, p));
        parameterizedType = parameterizedType.WithClazz(VisitAndCast<NameTree>(parameterizedType.Clazz, p)!);
        parameterizedType = parameterizedType.Padding.WithTypeParameters(VisitContainer(parameterizedType.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, p));
        return parameterizedType;
    }

    public virtual J? VisitParentheses<J2>(J.Parentheses<J2> parentheses, P p) where J2 : J
    {
        parentheses = parentheses.WithPrefix(VisitSpace(parentheses.Prefix, Space.Location.PARENTHESES_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(parentheses, p);
        if (tempExpression is not J.Parentheses<J2>)
        {
            return tempExpression;
        }
        parentheses = (J.Parentheses<J2>) tempExpression;
        parentheses = parentheses.WithMarkers(VisitMarkers(parentheses.Markers, p));
        parentheses = parentheses.Padding.WithTree(VisitRightPadded(parentheses.Padding.Tree, JRightPadded.Location.PARENTHESES, p)!);
        return parentheses;
    }

    public virtual J? VisitControlParentheses<J2>(J.ControlParentheses<J2> controlParentheses, P p) where J2 : J
    {
        controlParentheses = controlParentheses.WithPrefix(VisitSpace(controlParentheses.Prefix, Space.Location.CONTROL_PARENTHESES_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(controlParentheses, p);
        if (tempExpression is not J.ControlParentheses<J2>)
        {
            return tempExpression;
        }
        controlParentheses = (J.ControlParentheses<J2>) tempExpression;
        controlParentheses = controlParentheses.WithMarkers(VisitMarkers(controlParentheses.Markers, p));
        controlParentheses = controlParentheses.Padding.WithTree(VisitRightPadded(controlParentheses.Padding.Tree, JRightPadded.Location.PARENTHESES, p)!);
        return controlParentheses;
    }

    public virtual J? VisitPrimitive(J.Primitive primitive, P p)
    {
        primitive = primitive.WithPrefix(VisitSpace(primitive.Prefix, Space.Location.PRIMITIVE_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(primitive, p);
        if (tempExpression is not J.Primitive)
        {
            return tempExpression;
        }
        primitive = (J.Primitive) tempExpression;
        primitive = primitive.WithMarkers(VisitMarkers(primitive.Markers, p));
        return primitive;
    }

    public virtual J? VisitReturn(J.Return @return, P p)
    {
        @return = @return.WithPrefix(VisitSpace(@return.Prefix, Space.Location.RETURN_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@return, p);
        if (tempStatement is not J.Return)
        {
            return tempStatement;
        }
        @return = (J.Return) tempStatement;
        @return = @return.WithMarkers(VisitMarkers(@return.Markers, p));
        @return = @return.WithExpression(VisitAndCast<Expression>(@return.Expression, p));
        return @return;
    }

    public virtual J? VisitSwitch(J.Switch @switch, P p)
    {
        @switch = @switch.WithPrefix(VisitSpace(@switch.Prefix, Space.Location.SWITCH_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@switch, p);
        if (tempStatement is not J.Switch)
        {
            return tempStatement;
        }
        @switch = (J.Switch) tempStatement;
        @switch = @switch.WithMarkers(VisitMarkers(@switch.Markers, p));
        @switch = @switch.WithSelector(VisitAndCast<J.ControlParentheses<Expression>>(@switch.Selector, p)!);
        @switch = @switch.WithCases(VisitAndCast<J.Block>(@switch.Cases, p)!);
        return @switch;
    }

    public virtual J? VisitSwitchExpression(J.SwitchExpression switchExpression, P p)
    {
        switchExpression = switchExpression.WithPrefix(VisitSpace(switchExpression.Prefix, Space.Location.SWITCH_EXPRESSION_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(switchExpression, p);
        if (tempExpression is not J.SwitchExpression)
        {
            return tempExpression;
        }
        switchExpression = (J.SwitchExpression) tempExpression;
        switchExpression = switchExpression.WithMarkers(VisitMarkers(switchExpression.Markers, p));
        switchExpression = switchExpression.WithSelector(VisitAndCast<J.ControlParentheses<Expression>>(switchExpression.Selector, p)!);
        switchExpression = switchExpression.WithCases(VisitAndCast<J.Block>(switchExpression.Cases, p)!);
        return switchExpression;
    }

    public virtual J? VisitSynchronized(J.Synchronized synchronized, P p)
    {
        synchronized = synchronized.WithPrefix(VisitSpace(synchronized.Prefix, Space.Location.SYNCHRONIZED_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(synchronized, p);
        if (tempStatement is not J.Synchronized)
        {
            return tempStatement;
        }
        synchronized = (J.Synchronized) tempStatement;
        synchronized = synchronized.WithMarkers(VisitMarkers(synchronized.Markers, p));
        synchronized = synchronized.WithLock(VisitAndCast<J.ControlParentheses<Expression>>(synchronized.Lock, p)!);
        synchronized = synchronized.WithBody(VisitAndCast<J.Block>(synchronized.Body, p)!);
        return synchronized;
    }

    public virtual J? VisitTernary(J.Ternary ternary, P p)
    {
        ternary = ternary.WithPrefix(VisitSpace(ternary.Prefix, Space.Location.TERNARY_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(ternary, p);
        if (tempStatement is not J.Ternary)
        {
            return tempStatement;
        }
        ternary = (J.Ternary) tempStatement;
        var tempExpression = (Expression) VisitExpression(ternary, p);
        if (tempExpression is not J.Ternary)
        {
            return tempExpression;
        }
        ternary = (J.Ternary) tempExpression;
        ternary = ternary.WithMarkers(VisitMarkers(ternary.Markers, p));
        ternary = ternary.WithCondition(VisitAndCast<Expression>(ternary.Condition, p)!);
        ternary = ternary.Padding.WithTruePart(VisitLeftPadded(ternary.Padding.TruePart, JLeftPadded.Location.TERNARY_TRUE, p)!);
        ternary = ternary.Padding.WithFalsePart(VisitLeftPadded(ternary.Padding.FalsePart, JLeftPadded.Location.TERNARY_FALSE, p)!);
        return ternary;
    }

    public virtual J? VisitThrow(J.Throw @throw, P p)
    {
        @throw = @throw.WithPrefix(VisitSpace(@throw.Prefix, Space.Location.THROW_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@throw, p);
        if (tempStatement is not J.Throw)
        {
            return tempStatement;
        }
        @throw = (J.Throw) tempStatement;
        @throw = @throw.WithMarkers(VisitMarkers(@throw.Markers, p));
        @throw = @throw.WithException(VisitAndCast<Expression>(@throw.Exception, p)!);
        return @throw;
    }

    public virtual J? VisitTry(J.Try @try, P p)
    {
        @try = @try.WithPrefix(VisitSpace(@try.Prefix, Space.Location.TRY_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(@try, p);
        if (tempStatement is not J.Try)
        {
            return tempStatement;
        }
        @try = (J.Try) tempStatement;
        @try = @try.WithMarkers(VisitMarkers(@try.Markers, p));
        @try = @try.Padding.WithResources(VisitContainer(@try.Padding.Resources, JContainer.Location.TRY_RESOURCES, p));
        @try = @try.WithBody(VisitAndCast<J.Block>(@try.Body, p)!);
        @try = @try.WithCatches(ListUtils.Map(@try.Catches, el => (J.Try.Catch?)Visit(el, p)));
        @try = @try.Padding.WithFinally(VisitLeftPadded(@try.Padding.Finally, JLeftPadded.Location.TRY_FINALLY, p));
        return @try;
    }

    public virtual J? VisitTryResource(J.Try.Resource resource, P p)
    {
        resource = resource.WithPrefix(VisitSpace(resource.Prefix, Space.Location.TRY_RESOURCE, p)!);
        resource = resource.WithMarkers(VisitMarkers(resource.Markers, p));
        resource = resource.WithVariableDeclarations(VisitAndCast<TypedTree>(resource.VariableDeclarations, p)!);
        return resource;
    }

    public virtual J? VisitCatch(J.Try.Catch @catch, P p)
    {
        @catch = @catch.WithPrefix(VisitSpace(@catch.Prefix, Space.Location.CATCH_PREFIX, p)!);
        @catch = @catch.WithMarkers(VisitMarkers(@catch.Markers, p));
        @catch = @catch.WithParameter(VisitAndCast<J.ControlParentheses<J.VariableDeclarations>>(@catch.Parameter, p)!);
        @catch = @catch.WithBody(VisitAndCast<J.Block>(@catch.Body, p)!);
        return @catch;
    }

    public virtual J? VisitTypeCast(J.TypeCast typeCast, P p)
    {
        typeCast = typeCast.WithPrefix(VisitSpace(typeCast.Prefix, Space.Location.TYPE_CAST_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(typeCast, p);
        if (tempExpression is not J.TypeCast)
        {
            return tempExpression;
        }
        typeCast = (J.TypeCast) tempExpression;
        typeCast = typeCast.WithMarkers(VisitMarkers(typeCast.Markers, p));
        typeCast = typeCast.WithClazz(VisitAndCast<J.ControlParentheses<TypeTree>>(typeCast.Clazz, p)!);
        typeCast = typeCast.WithExpression(VisitAndCast<Expression>(typeCast.Expression, p)!);
        return typeCast;
    }

    public virtual J? VisitTypeParameter(J.TypeParameter typeParameter, P p)
    {
        typeParameter = typeParameter.WithPrefix(VisitSpace(typeParameter.Prefix, Space.Location.TYPE_PARAMETERS_PREFIX, p)!);
        typeParameter = typeParameter.WithMarkers(VisitMarkers(typeParameter.Markers, p));
        typeParameter = typeParameter.WithAnnotations(ListUtils.Map(typeParameter.Annotations, el => (J.Annotation?)Visit(el, p)));
        typeParameter = typeParameter.WithModifiers(ListUtils.Map(typeParameter.Modifiers, el => (J.Modifier?)Visit(el, p)));
        typeParameter = typeParameter.WithName(VisitAndCast<Expression>(typeParameter.Name, p)!);
        typeParameter = typeParameter.Padding.WithBounds(VisitContainer(typeParameter.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, p));
        return typeParameter;
    }

    public virtual J? VisitTypeParameters(J.TypeParameters typeParameters, P p)
    {
        typeParameters = typeParameters.WithPrefix(VisitSpace(typeParameters.Prefix, Space.Location.TYPE_PARAMETERS_PREFIX, p)!);
        typeParameters = typeParameters.WithMarkers(VisitMarkers(typeParameters.Markers, p));
        typeParameters = typeParameters.WithAnnotations(ListUtils.Map(typeParameters.Annotations, el => (J.Annotation?)Visit(el, p)));
        typeParameters = typeParameters.Padding.WithParameters(ListUtils.Map(typeParameters.Padding.Parameters, el => VisitRightPadded(el, JRightPadded.Location.TYPE_PARAMETER, p)));
        return typeParameters;
    }

    public virtual J? VisitUnary(J.Unary unary, P p)
    {
        unary = unary.WithPrefix(VisitSpace(unary.Prefix, Space.Location.UNARY_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(unary, p);
        if (tempStatement is not J.Unary)
        {
            return tempStatement;
        }
        unary = (J.Unary) tempStatement;
        var tempExpression = (Expression) VisitExpression(unary, p);
        if (tempExpression is not J.Unary)
        {
            return tempExpression;
        }
        unary = (J.Unary) tempExpression;
        unary = unary.WithMarkers(VisitMarkers(unary.Markers, p));
        unary = unary.Padding.WithOperator(VisitLeftPadded(unary.Padding.Operator, JLeftPadded.Location.UNARY_OPERATOR, p)!);
        unary = unary.WithExpression(VisitAndCast<Expression>(unary.Expression, p)!);
        return unary;
    }

    public virtual J? VisitVariableDeclarations(J.VariableDeclarations variableDeclarations, P p)
    {
        variableDeclarations = variableDeclarations.WithPrefix(VisitSpace(variableDeclarations.Prefix, Space.Location.VARIABLE_DECLARATIONS_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(variableDeclarations, p);
        if (tempStatement is not J.VariableDeclarations)
        {
            return tempStatement;
        }
        variableDeclarations = (J.VariableDeclarations) tempStatement;
        variableDeclarations = variableDeclarations.WithMarkers(VisitMarkers(variableDeclarations.Markers, p));
        variableDeclarations = variableDeclarations.WithLeadingAnnotations(ListUtils.Map(variableDeclarations.LeadingAnnotations, el => (J.Annotation?)Visit(el, p)));
        variableDeclarations = variableDeclarations.WithModifiers(ListUtils.Map(variableDeclarations.Modifiers, el => (J.Modifier?)Visit(el, p)));
        variableDeclarations = variableDeclarations.WithTypeExpression(VisitAndCast<TypeTree>(variableDeclarations.TypeExpression, p));
        variableDeclarations = variableDeclarations.WithVarargs(VisitSpace(variableDeclarations.Varargs!, Space.Location.VARARGS, p));
        variableDeclarations = variableDeclarations.WithDimensionsBeforeName(ListUtils.Map(variableDeclarations.DimensionsBeforeName, el => el.WithBefore(VisitSpace(el.Before, Space.Location.DIMENSION_PREFIX, p)).WithElement(VisitSpace(el.Element, Space.Location.DIMENSION, p))));
        variableDeclarations = variableDeclarations.Padding.WithVariables(ListUtils.Map(variableDeclarations.Padding.Variables, el => VisitRightPadded(el, JRightPadded.Location.NAMED_VARIABLE, p)));
        return variableDeclarations;
    }

    public virtual J? VisitVariable(J.VariableDeclarations.NamedVariable namedVariable, P p)
    {
        namedVariable = namedVariable.WithPrefix(VisitSpace(namedVariable.Prefix, Space.Location.VARIABLE_PREFIX, p)!);
        namedVariable = namedVariable.WithMarkers(VisitMarkers(namedVariable.Markers, p));
        namedVariable = namedVariable.WithName(VisitAndCast<J.Identifier>(namedVariable.Name, p)!);
        namedVariable = namedVariable.WithDimensionsAfterName(ListUtils.Map(namedVariable.DimensionsAfterName, el => el.WithBefore(VisitSpace(el.Before, Space.Location.DIMENSION_PREFIX, p)).WithElement(VisitSpace(el.Element, Space.Location.DIMENSION, p))));
        namedVariable = namedVariable.Padding.WithInitializer(VisitLeftPadded(namedVariable.Padding.Initializer, JLeftPadded.Location.VARIABLE_INITIALIZER, p));
        return namedVariable;
    }

    public virtual J? VisitWhileLoop(J.WhileLoop whileLoop, P p)
    {
        whileLoop = whileLoop.WithPrefix(VisitSpace(whileLoop.Prefix, Space.Location.WHILE_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(whileLoop, p);
        if (tempStatement is not J.WhileLoop)
        {
            return tempStatement;
        }
        whileLoop = (J.WhileLoop) tempStatement;
        whileLoop = whileLoop.WithMarkers(VisitMarkers(whileLoop.Markers, p));
        whileLoop = whileLoop.WithCondition(VisitAndCast<J.ControlParentheses<Expression>>(whileLoop.Condition, p)!);
        whileLoop = whileLoop.Padding.WithBody(VisitRightPadded(whileLoop.Padding.Body, JRightPadded.Location.WHILE_BODY, p)!);
        return whileLoop;
    }

    public virtual J? VisitWildcard(J.Wildcard wildcard, P p)
    {
        wildcard = wildcard.WithPrefix(VisitSpace(wildcard.Prefix, Space.Location.WILDCARD_PREFIX, p)!);
        var tempExpression = (Expression) VisitExpression(wildcard, p);
        if (tempExpression is not J.Wildcard)
        {
            return tempExpression;
        }
        wildcard = (J.Wildcard) tempExpression;
        wildcard = wildcard.WithMarkers(VisitMarkers(wildcard.Markers, p));
        wildcard = wildcard.Padding.WithWildcardBound(VisitLeftPadded(wildcard.Padding.WildcardBound, JLeftPadded.Location.WILDCARD_BOUND, p));
        wildcard = wildcard.WithBoundedType(VisitAndCast<NameTree>(wildcard.BoundedType, p));
        return wildcard;
    }

    public virtual J? VisitYield(J.Yield yield, P p)
    {
        yield = yield.WithPrefix(VisitSpace(yield.Prefix, Space.Location.YIELD_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(yield, p);
        if (tempStatement is not J.Yield)
        {
            return tempStatement;
        }
        yield = (J.Yield) tempStatement;
        yield = yield.WithMarkers(VisitMarkers(yield.Markers, p));
        yield = yield.WithValue(VisitAndCast<Expression>(yield.Value, p)!);
        return yield;
    }

    public virtual J? VisitUnknown(J.Unknown unknown, P p)
    {
        unknown = unknown.WithPrefix(VisitSpace(unknown.Prefix, Space.Location.UNKNOWN_PREFIX, p)!);
        var tempStatement = (Statement) VisitStatement(unknown, p);
        if (tempStatement is not J.Unknown)
        {
            return tempStatement;
        }
        unknown = (J.Unknown) tempStatement;
        var tempExpression = (Expression) VisitExpression(unknown, p);
        if (tempExpression is not J.Unknown)
        {
            return tempExpression;
        }
        unknown = (J.Unknown) tempExpression;
        unknown = unknown.WithMarkers(VisitMarkers(unknown.Markers, p));
        unknown = unknown.WithUnknownSource(VisitAndCast<J.Unknown.Source>(unknown.UnknownSource, p)!);
        return unknown;
    }

    public virtual J? VisitUnknownSource(J.Unknown.Source source, P p)
    {
        source = source.WithPrefix(VisitSpace(source.Prefix, Space.Location.UNKNOWN_SOURCE_PREFIX, p)!);
        source = source.WithMarkers(VisitMarkers(source.Markers, p));
        return source;
    }

    public virtual JContainer<T>? VisitContainer<T>(JContainer<T>? container, JContainer.Location loc, P p)
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
            JContainer<T>.Build(before!, js, container.Markers);
    }

    public virtual JLeftPadded<T>? VisitLeftPadded<T>(JLeftPadded<T>? left, JLeftPadded.Location loc, P p)
    {
        if (left == null)
        {
            return null;
        }

        Cursor = new Cursor(Cursor, left);

        var before = VisitSpace(left.Before, loc.BeforeLocation, p)!;
        var t = left.Element;
        if (left.Element is Core.Tree)
        {
            t = (T?)Visit(t as Core.Tree, p);
        }
        Cursor = Cursor.Parent!;

        // If nothing changed leave AST node the same
        if (ReferenceEquals(left.Element, t) && before == left.Before) {
            return left;
        }

        return t == null ? null : new JLeftPadded<T>(before, t, left.Markers);
    }

    public virtual JRightPadded<T>? VisitRightPadded<T>(JRightPadded<T>? right, JRightPadded.Location loc, P p)
    {
        if (right == null)
        {
            return null;
        }

        var t = right.Element;
        if (t is J)
        {
            Cursor = new Cursor(Cursor, right);
            t = (T?)Visit(t as J, p);
            Cursor = Cursor.Parent!;
        }

        if (t == null)
        {
            return null;
        }

        right = right.WithElement(t);
        right = right.WithAfter(VisitSpace(right.After, loc.AfterLocation, p)!);
        right = right.WithMarkers(VisitMarkers(right.Markers, p));
        return right;
    }

    public virtual Space VisitSpace(Space space, Space.Location? loc, P p)
    {
        // FIXME add Javadoc support
        return space;
    }

}
