using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava;

public class JavaPrinter<P> : JavaVisitor<PrintOutputCapture<P>>
{
    protected void VisitRightPadded<T>(IList<JRightPadded<T>> nodes, JRightPadded.Location location,
        string suffixBetween, PrintOutputCapture<P> p) where T : J
    {
        for (var i = 0; i < nodes.Count; i++)
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

    protected void VisitContainer<T>(string before, JContainer<T>? container, JContainer.Location location,
        string suffixBetween, string? after, PrintOutputCapture<P> p) where T : J
    {
        if (container == null)
        {
            return;
        }

        BeforeSyntax(container.Before, container.Markers, location.BeforeLocation, p);
        p.Append(before);
        VisitRightPadded(container.Padding.Elements, location.ElementLocation, suffixBetween, p);
        AfterSyntax(container.Markers, p);
        p.Append(after ?? "");
    }

    public override Space VisitSpace(Space? space, Space.Location? loc, PrintOutputCapture<P> p)
    {
        p.Append(space.Whitespace);

        var comments = space.Comments;
        for (var i = 0; i < comments.Count; i++)
        {
            Comment comment = comments[i];
            VisitMarkers(comment.Markers, p);
            comment.PrintComment(Cursor, p);
            p.Append(comment.Suffix);
        }

        return space;
    }

    protected void VisitLeftPadded<T>(string? prefix, JLeftPadded<T>? leftPadded, JLeftPadded.Location location,
        PrintOutputCapture<P> p) where T : J
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

    protected void VisitRightPadded<T>(JRightPadded<T>? rightPadded, JRightPadded.Location location, string? suffix,
        PrintOutputCapture<P> p) where T : J
    {
        if (rightPadded != null)
        {
            BeforeSyntax(Space.EMPTY, rightPadded.Markers, null, p);
            Visit(rightPadded.Element, p);
            AfterSyntax(rightPadded.Markers, p);
            VisitSpace(rightPadded.After, location.AfterLocation, p);
            if (suffix != null)
            {
                p.Append(suffix);
            }
        }
    }

    protected void VisitModifier(J.Modifier mod, PrintOutputCapture<P> p)
    {
        Visit(mod.Annotations, p);
        var keyword = "";
        switch (mod.ModifierType)
        {
            case J.Modifier.Type.Default:
                keyword = "default";
                break;
            case J.Modifier.Type.Public:
                keyword = "public";
                break;
            case J.Modifier.Type.Protected:
                keyword = "protected";
                break;
            case J.Modifier.Type.Private:
                keyword = "private";
                break;
            case J.Modifier.Type.Abstract:
                keyword = "abstract";
                break;
            case J.Modifier.Type.Static:
                keyword = "static";
                break;
            case J.Modifier.Type.Final:
                keyword = "sealed";
                break;
            case J.Modifier.Type.Native:
                keyword = "native";
                break;
            case J.Modifier.Type.NonSealed:
                keyword = "non-sealed";
                break;
            case J.Modifier.Type.Sealed:
                keyword = "sealed";
                break;
            case J.Modifier.Type.Strictfp:
                keyword = "strictfp";
                break;
            case J.Modifier.Type.Synchronized:
                keyword = "synchronized";
                break;
            case J.Modifier.Type.Transient:
                keyword = "transient";
                break;
            case J.Modifier.Type.Volatile:
                keyword = "volatile";
                break;
        }

        BeforeSyntax(mod, Space.Location.MODIFIER_PREFIX, p);
        p.Append(keyword);
        AfterSyntax(mod, p);
    }


    public override J VisitAnnotation(J.Annotation annotation, PrintOutputCapture<P> p)
    {
        BeforeSyntax(annotation, Space.Location.ANNOTATION_PREFIX, p);
        p.Append('@');
        Visit(annotation.AnnotationType, p);
        VisitContainer("(", annotation.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
        AfterSyntax(annotation, p);
        return annotation;
    }

    public override J VisitAnnotatedType(J.AnnotatedType annotatedType, PrintOutputCapture<P> p)
    {
        BeforeSyntax(annotatedType, Space.Location.ANNOTATED_TYPE_PREFIX, p);
        Visit(annotatedType.Annotations, p);
        Visit(annotatedType.TypeExpression, p);
        AfterSyntax(annotatedType, p);
        return annotatedType;
    }

    public override J VisitArrayDimension(J.ArrayDimension arrayDimension, PrintOutputCapture<P> p)
    {
        BeforeSyntax(arrayDimension, Space.Location.DIMENSION_PREFIX, p);
        p.Append('[');
        VisitRightPadded(arrayDimension.Padding.Index, JRightPadded.Location.ARRAY_INDEX, "]", p);
        AfterSyntax(arrayDimension, p);
        return arrayDimension;
    }

    public override J VisitArrayType(J.ArrayType arrayType, PrintOutputCapture<P> p)
    {
        BeforeSyntax(arrayType, Space.Location.ARRAY_TYPE_PREFIX, p);
        TypeTree type = arrayType;
        while (type is J.ArrayType)
        {
            type = ((J.ArrayType)type).ElementType;
        }

        Visit(type, p);
        Visit(arrayType.Annotations, p);
        if (arrayType.Dimension != null)
        {
            VisitSpace(arrayType.Dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(arrayType.Dimension.Element, Space.Location.DIMENSION, p);
            p.Append(']');

            if (arrayType.ElementType is J.ArrayType)
            {
                PrintDimensions((J.ArrayType)arrayType.ElementType, p);
            }
        }

        AfterSyntax(arrayType, p);
        return arrayType;
    }

    private void PrintDimensions(J.ArrayType arrayType, PrintOutputCapture<P> p)
    {
        BeforeSyntax(arrayType, Space.Location.ARRAY_TYPE_PREFIX, p);
        Visit(arrayType.Annotations, p);
        VisitSpace(arrayType.Dimension.Before, Space.Location.DIMENSION_PREFIX, p);
        p.Append('[');
        VisitSpace(arrayType.Dimension.Element, Space.Location.DIMENSION, p);
        p.Append(']');
        if (arrayType.ElementType is J.ArrayType)
        {
            PrintDimensions((J.ArrayType)arrayType.ElementType, p);
        }

        AfterSyntax(arrayType, p);
    }

    public override J VisitAssert(J.Assert assert_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(assert_, Space.Location.ASSERT_PREFIX, p);
        p.Append("assert");
        Visit(assert_.Condition, p);
        VisitLeftPadded(":", assert_.Detail, JLeftPadded.Location.ASSERT_DETAIL, p);
        AfterSyntax(assert_, p);
        return assert_;
    }

    public override J VisitAssignment(J.Assignment assignment, PrintOutputCapture<P> p)
    {
        BeforeSyntax(assignment, Space.Location.ASSIGNMENT_PREFIX, p);
        Visit(assignment.Variable, p);
        VisitLeftPadded("=", assignment.Padding.Expression, JLeftPadded.Location.ASSIGNMENT, p);
        AfterSyntax(assignment, p);
        return assignment;
    }

    public override J VisitAssignmentOperation(J.AssignmentOperation assignOp, PrintOutputCapture<P> p)
    {
        var keyword = "";
        switch (assignOp.Operator)
        {
            case J.AssignmentOperation.Type.Addition:
                keyword = "+=";
                break;
            case J.AssignmentOperation.Type.Subtraction:
                keyword = "-=";
                break;
            case J.AssignmentOperation.Type.Multiplication:
                keyword = "*=";
                break;
            case J.AssignmentOperation.Type.Division:
                keyword = "/=";
                break;
            case J.AssignmentOperation.Type.Modulo:
                keyword = "%=";
                break;
            case J.AssignmentOperation.Type.BitAnd:
                keyword = "&=";
                break;
            case J.AssignmentOperation.Type.BitOr:
                keyword = "|=";
                break;
            case J.AssignmentOperation.Type.BitXor:
                keyword = "^=";
                break;
            case J.AssignmentOperation.Type.LeftShift:
                keyword = "<<=";
                break;
            case J.AssignmentOperation.Type.RightShift:
                keyword = ">>=";
                break;
            case J.AssignmentOperation.Type.UnsignedRightShift:
                keyword = ">>>=";
                break;
        }

        BeforeSyntax(assignOp, Space.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        Visit(assignOp.Variable, p);
        VisitSpace(assignOp.Padding.Operator.Before, Space.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        p.Append(keyword);
        Visit(assignOp.Assignment, p);
        AfterSyntax(assignOp, p);
        return assignOp;
    }

    public override J VisitBinary(J.Binary binary, PrintOutputCapture<P> p)
    {
        string keyword = "";
        switch (binary.Operator)
        {
            case J.Binary.Type.Addition:
                keyword = "+";
                break;
            case J.Binary.Type.Subtraction:
                keyword = "-";
                break;
            case J.Binary.Type.Multiplication:
                keyword = "*";
                break;
            case J.Binary.Type.Division:
                keyword = "/";
                break;
            case J.Binary.Type.Modulo:
                keyword = "%";
                break;
            case J.Binary.Type.LessThan:
                keyword = "<";
                break;
            case J.Binary.Type.GreaterThan:
                keyword = ">";
                break;
            case J.Binary.Type.LessThanOrEqual:
                keyword = "<=";
                break;
            case J.Binary.Type.GreaterThanOrEqual:
                keyword = ">=";
                break;
            case J.Binary.Type.Equal:
                keyword = "==";
                break;
            case J.Binary.Type.NotEqual:
                keyword = "!=";
                break;
            case J.Binary.Type.BitAnd:
                keyword = "&";
                break;
            case J.Binary.Type.BitOr:
                keyword = "|";
                break;
            case J.Binary.Type.BitXor:
                keyword = "^";
                break;
            case J.Binary.Type.LeftShift:
                keyword = "<<";
                break;
            case J.Binary.Type.RightShift:
                keyword = ">>";
                break;
            case J.Binary.Type.UnsignedRightShift:
                keyword = ">>>";
                break;
            case J.Binary.Type.Or:
                keyword = "||";
                break;
            case J.Binary.Type.And:
                keyword = "&&";
                break;
        }

        BeforeSyntax(binary, Space.Location.BINARY_PREFIX, p);
        Visit(binary.Left, p);
        VisitSpace(binary.Padding.Operator.Before, Space.Location.BINARY_OPERATOR, p);
        p.Append(keyword);
        Visit(binary.Right, p);
        AfterSyntax(binary, p);
        return binary;
    }

    public override J VisitBlock(J.Block block, PrintOutputCapture<P> p)
    {
        BeforeSyntax(block, Space.Location.BLOCK_PREFIX, p);

        if (block.Static)
        {
            p.Append("static");
            VisitRightPadded(block.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
        }

        p.Append('{');
        VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
        VisitSpace(block.End, Space.Location.BLOCK_END, p);
        p.Append('}');
        AfterSyntax(block, p);
        return block;
    }

    protected void VisitStatements(IList<JRightPadded<Statement>> statements, JRightPadded.Location location,
        PrintOutputCapture<P> p)
    {
        foreach (var paddedStat in statements)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement<T>(JRightPadded<T> paddedStat, JRightPadded.Location location,
        PrintOutputCapture<P> p) where T : J
    {
        if (paddedStat == null)
        {
            return;
        }

        Visit(paddedStat.Element, p);
        VisitSpace(paddedStat.After, location.AfterLocation, p);

        J s = paddedStat.Element;
        while (true)
        {
            if (s is J.Assert ||
                s is J.Assignment ||
                s is J.AssignmentOperation ||
                s is J.Break ||
                s is J.Continue ||
                s is J.DoWhileLoop ||
                s is J.Empty ||
                s is J.MethodInvocation ||
                s is J.NewClass ||
                s is J.Return ||
                s is J.Throw ||
                s is J.Unary ||
                s is J.VariableDeclarations ||
                s is J.Yield)
            {
                p.Append(';');
                return;
            }

            if (s is J.MethodDeclaration declaration && declaration.Body == null)
            {
                p.Append(';');
                return;
            }

            if (s is J.Label)
            {
                s = ((J.Label)s).Statement;
                continue;
            }

            // Assuming GetCursor() returns a Value type which has a GetValue() method
            // This section may need to change depending on the actual representation of these objects
            if (Cursor.Value is J.Case)
            {
                object aSwitch = Cursor
                    .DropParentUntil(c =>
                        c is J.Switch ||
                        c is J.SwitchExpression ||
                        c == Cursor.ROOT_VALUE)
                    .Value;

                if (aSwitch is J.SwitchExpression)
                {
                    J.Case aCase = (J.Case)Cursor.Value;
                    if (!(aCase.Body is J.Block))
                    {
                        p.Append(';');
                    }

                    return;
                }
            }

            return;
        }
    }

    public override J VisitBreak(J.Break breakStatement, PrintOutputCapture<P> p)
    {
        BeforeSyntax(breakStatement, Space.Location.BREAK_PREFIX, p);
        p.Append("break");
        Visit(breakStatement.Label, p);
        AfterSyntax(breakStatement, p);
        return breakStatement;
    }

    public override J VisitCase(J.Case case_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(case_, Space.Location.CASE_PREFIX, p);
        Expression elem = case_.Expressions[0];
        if (!(elem is J.Identifier) || !((J.Identifier)elem).SimpleName.Equals("default"))
        {
            p.Append("case");
        }

        VisitContainer("", case_.Padding.Expressions, JContainer.Location.CASE_EXPRESSION, ",", "", p);
        VisitSpace(case_.Padding.Statements.Before, Space.Location.CASE, p);
        p.Append(case_.CaseType == J.Case.Type.Statement ? ":" : "->");
        VisitStatements(case_.Padding.Statements.Padding
            .Elements, JRightPadded.Location.CASE, p);
        if (case_.Body is Statement)
        {
            VisitStatement(case_.Padding.Body!, JRightPadded.Location.CASE_BODY, p);
        }
        else
        {
            VisitRightPadded(case_.Padding.Body, JRightPadded.Location.CASE_BODY, ";", p);
        }

        AfterSyntax(case_, p);
        return case_;
    }

    public override J VisitCatch(J.Try.Catch catch_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(catch_, Space.Location.CATCH_PREFIX, p);
        p.Append("catch");
        Visit(catch_.Parameter, p);
        Visit(catch_.Body, p);
        AfterSyntax(catch_, p);
        return catch_;
    }
    
    public override J VisitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<P> p)
    {
        string kind = "";
        switch (classDecl.Padding.DeclarationKind.KindType)
        {
            case J.ClassDeclaration.Kind.Type.Class:
                kind = "class";
                break;
            case J.ClassDeclaration.Kind.Type.Enum:
                kind = "enum";
                break;
            case J.ClassDeclaration.Kind.Type.Interface:
                kind = "interface";
                break;
            case J.ClassDeclaration.Kind.Type.Annotation:
                kind = "@interface";
                break;
            case J.ClassDeclaration.Kind.Type.Record:
                kind = "record";
                break;
        }

        BeforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(classDecl.LeadingAnnotations, p);
        foreach (J.Modifier m in classDecl.Modifiers)
        {
            VisitModifier(m, p);
        }
        Visit(classDecl.Padding.DeclarationKind.Annotations, p);
        VisitSpace(classDecl.Padding.DeclarationKind.Prefix, Space.Location.CLASS_KIND, p);
        p.Append(kind);
        Visit(classDecl.Name, p);
        VisitContainer("<", classDecl.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        VisitContainer("(", classDecl.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
        VisitLeftPadded("extends", classDecl.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
        VisitContainer(classDecl.Padding.DeclarationKind.KindType.Equals(J.ClassDeclaration.Kind.Type.Interface) ? "extends" : "implements",
            classDecl.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
        VisitContainer("permits", classDecl.Padding.Permits, JContainer.Location.PERMITS, ",", null, p);
        Visit(classDecl.Body, p);
        AfterSyntax(classDecl, p);
        return classDecl;
    }

    public override J VisitCompilationUnit(J.CompilationUnit cu, PrintOutputCapture<P> p)
    {
        BeforeSyntax(cu, Space.Location.COMPILATION_UNIT_PREFIX, p);
        VisitRightPadded(cu.Padding.PackageDeclaration, JRightPadded.Location.PACKAGE, ";", p);
        VisitRightPadded(cu.Padding.Imports, JRightPadded.Location.IMPORT, ";", p);
        if (cu.Imports.Any())
        {
            p.Append(';');
        }

        Visit(cu.Classes, p);
        AfterSyntax(cu, p);
        VisitSpace(cu.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        return cu;
    }

    public override J VisitContinue(J.Continue continueStatement, PrintOutputCapture<P> p)
    {
        BeforeSyntax(continueStatement, Space.Location.CONTINUE_PREFIX, p);
        p.Append("continue");
        Visit(continueStatement.Label, p);
        AfterSyntax(continueStatement, p);
        return continueStatement;
    }

    public override J VisitControlParentheses<T>(J.ControlParentheses<T> controlParens, PrintOutputCapture<P> p)
    {
        BeforeSyntax(controlParens, Space.Location.CONTROL_PARENTHESES_PREFIX, p);
        p.Append('(');
        VisitRightPadded(controlParens.Padding.Tree, JRightPadded.Location.PARENTHESES, ")", p);
        AfterSyntax(controlParens, p);
        return controlParens;
    }

    public override J VisitDoWhileLoop(J.DoWhileLoop doWhileLoop, PrintOutputCapture<P> p)
    {
        BeforeSyntax(doWhileLoop, Space.Location.DO_WHILE_PREFIX, p);
        p.Append("do");
        VisitStatement(doWhileLoop.Padding.Body, JRightPadded.Location.WHILE_BODY, p);
        VisitLeftPadded("while", doWhileLoop.Padding.WhileCondition, JLeftPadded.Location.WHILE_CONDITION, p);
        AfterSyntax(doWhileLoop, p);
        return doWhileLoop;
    }

    public override J VisitElse(J.If.Else else_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(else_, Space.Location.ELSE_PREFIX, p);
        p.Append("else");
        VisitStatement(else_.Padding.Body, JRightPadded.Location.IF_ELSE, p);
        AfterSyntax(else_, p);
        return else_;
    }

    public override J VisitEmpty(J.Empty empty, PrintOutputCapture<P> p)
    {
        BeforeSyntax(empty, Space.Location.EMPTY_PREFIX, p);
        AfterSyntax(empty, p);
        return empty;
    }

    public override J VisitEnumValue(J.EnumValue enum_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(enum_, Space.Location.ENUM_VALUE_PREFIX, p);
        Visit(enum_.Annotations, p);
        Visit(enum_.Name, p);
        var initializer = enum_.Initializer;
        if (initializer != null)
        {
            VisitSpace(initializer.Prefix, Space.Location.NEW_CLASS_PREFIX, p);
            VisitSpace(initializer.New, Space.Location.NEW_PREFIX, p);
            if (initializer.Padding.Arguments.Markers.FindFirst<OmitParentheses>() == null)
            {
                VisitContainer("(", initializer.Padding.Arguments, JContainer.Location.NEW_CLASS_ARGUMENTS, ",", ")",
                    p);
            }

            Visit(initializer.Body, p);
        }

        AfterSyntax(enum_, p);
        return enum_;
    }

    public override J VisitEnumValueSet(J.EnumValueSet enums, PrintOutputCapture<P> p)
    {
        BeforeSyntax(enums, Space.Location.ENUM_VALUE_SET_PREFIX, p);
        VisitRightPadded(enums.Padding.Enums, JRightPadded.Location.ENUM_VALUE, ",", p);
        if (enums.TerminatedWithSemicolon)
        {
            p.Append(';');
        }

        AfterSyntax(enums, p);
        return enums;
    }

    public override J VisitFieldAccess(J.FieldAccess fieldAccess, PrintOutputCapture<P> p)
    {
        BeforeSyntax(fieldAccess, Space.Location.FIELD_ACCESS_PREFIX, p);
        Visit(fieldAccess.Target, p);
        VisitLeftPadded(".", fieldAccess.Padding.Name, JLeftPadded.Location.FIELD_ACCESS_NAME, p);
        AfterSyntax(fieldAccess, p);
        return fieldAccess;
    }

    public override J VisitForLoop(J.ForLoop forLoop, PrintOutputCapture<P> p)
    {
        BeforeSyntax(forLoop, Space.Location.FOR_PREFIX, p);
        p.Append("for");
        J.ForLoop.Control ctrl = forLoop.LoopControl;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Init, JRightPadded.Location.FOR_INIT, ",", p);
        p.Append(';');
        VisitRightPadded(ctrl.Padding.Condition, JRightPadded.Location.FOR_CONDITION, ";", p);
        VisitRightPadded(ctrl.Padding.Update, JRightPadded.Location.FOR_UPDATE, ",", p);
        p.Append(')');
        VisitStatement(forLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p);
        AfterSyntax(forLoop, p);
        return forLoop;
    }

    public override J VisitForEachLoop(J.ForEachLoop forEachLoop, PrintOutputCapture<P> p)
    {
        BeforeSyntax(forEachLoop, Space.Location.FOR_EACH_LOOP_PREFIX, p);
        p.Append("for");
        J.ForEachLoop.Control ctrl = forEachLoop.LoopControl;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, ":", p);
        VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, "", p);
        p.Append(')');
        VisitStatement(forEachLoop.Padding.Body, JRightPadded.Location.FOR_BODY, p);
        AfterSyntax(forEachLoop, p);
        return forEachLoop;
    }

    public override J VisitIdentifier(J.Identifier ident, PrintOutputCapture<P> p)
    {
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(ident.Annotations, p);
        BeforeSyntax(ident, Space.Location.IDENTIFIER_PREFIX, p);
        p.Append(ident.SimpleName);
        AfterSyntax(ident, p);
        return ident;
    }

    public override J VisitIf(J.If iff, PrintOutputCapture<P> p)
    {
        BeforeSyntax(iff, Space.Location.IF_PREFIX, p);
        p.Append("if");
        Visit(iff.IfCondition, p);
        VisitStatement(iff.Padding.ThenPart, JRightPadded.Location.IF_THEN, p);
        Visit(iff.ElsePart, p);
        AfterSyntax(iff, p);
        return iff;
    }

    public override J VisitInstanceOf(J.InstanceOf instanceOf, PrintOutputCapture<P> p)
    {
        BeforeSyntax(instanceOf, Space.Location.INSTANCEOF_PREFIX, p);
        VisitRightPadded(instanceOf.Padding.Expression, JRightPadded.Location.INSTANCEOF, "instanceof", p);
        Visit(instanceOf.Clazz, p);
        Visit(instanceOf.Pattern, p);
        AfterSyntax(instanceOf, p);
        return instanceOf;
    }

    public override J VisitIntersectionType(J.IntersectionType intersectionType, PrintOutputCapture<P> p)
    {
        BeforeSyntax(intersectionType, Space.Location.INTERSECTION_TYPE_PREFIX, p);
        VisitContainer("", intersectionType.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, "&", "", p);
        AfterSyntax(intersectionType, p);
        return intersectionType;
    }

    public override J VisitLabel(J.Label label, PrintOutputCapture<P> p)
    {
        BeforeSyntax(label, Space.Location.LABEL_PREFIX, p);
        VisitRightPadded(label.Padding.Name, JRightPadded.Location.LABEL, ":", p);
        Visit(label.Statement, p);
        AfterSyntax(label, p);
        return label;
    }

    public override J VisitLambda(J.Lambda lambda, PrintOutputCapture<P> p)
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
        p.Append("->");
        Visit(lambda.Body, p);
        AfterSyntax(lambda, p);
        return lambda;
    }

    public override J VisitLiteral(J.Literal literal, PrintOutputCapture<P> p)
    {
        BeforeSyntax(literal, Space.Location.LITERAL_PREFIX, p);
        IList<J.Literal.UnicodeEscape> unicodeEscapes = literal.UnicodeEscapes;
        if (unicodeEscapes == null)
        {
            p.Append(literal.ValueSource);
        }
        else if (literal.ValueSource != null)
        {
            var surrogateEnum = unicodeEscapes.GetEnumerator();
            J.Literal.UnicodeEscape surrogate = surrogateEnum.MoveNext() ?
                    surrogateEnum.Current : null;
            int i = 0;
            if (surrogate != null && surrogate.ValueSourceIndex == 0)
            {
                p.Append("\\u").Append(surrogate.CodePoint);
                if (surrogateEnum.MoveNext())
                {
                    surrogate = surrogateEnum.Current;
                }
            }

            string valueSource = literal.ValueSource;
            for (int j = 0; j < valueSource.Length; j++)
            {
                char c = valueSource[j];
                p.Append(c);
                if (surrogate != null && surrogate.ValueSourceIndex == ++i)
                {
                    while (surrogate != null && surrogate.ValueSourceIndex == i)
                    {
                        p.Append("\\u").Append(surrogate.CodePoint);
                        surrogate = surrogateEnum.MoveNext() ? surrogateEnum.Current : null;
                    }
                }
            }
        }
        AfterSyntax(literal, p);
        return literal;
    }

    public override J VisitMemberReference(J.MemberReference memberRef, PrintOutputCapture<P> p)
    {
        BeforeSyntax(memberRef, Space.Location.MEMBER_REFERENCE_PREFIX, p);
        VisitRightPadded(memberRef.Padding.Containing, JRightPadded.Location.MEMBER_REFERENCE_CONTAINING, p);
        p.Append("::");
        VisitContainer("<", memberRef.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        VisitLeftPadded("", memberRef.Padding.Reference, JLeftPadded.Location.MEMBER_REFERENCE_NAME, p);
        AfterSyntax(memberRef, p);
        return memberRef;
    }
    
    public override J VisitMethodDeclaration(J.MethodDeclaration method, PrintOutputCapture<P> p)
    {
        BeforeSyntax(method, Space.Location.METHOD_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(method.LeadingAnnotations, p);
        foreach (var m in method.Modifiers)
        {
            VisitModifier(m, p);
        }
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
        Visit(method.ReturnTypeExpression, p);
        Visit(method.Annotations.Name.Annotations, p);
        Visit(method.Padding.Name.Identifier, p);
        if (method.Markers.FindFirst<CompactConstructor>() == null)
        {
            VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }
        VisitContainer("throws", method.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
        Visit(method.Body, p);
        VisitLeftPadded("default", method.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE, p);
        AfterSyntax(method, p);
        return method;
    }
    
    public override J VisitMethodInvocation(J.MethodInvocation method, PrintOutputCapture<P> p)
    {
        BeforeSyntax(method, Space.Location.METHOD_INVOCATION_PREFIX, p);
        VisitRightPadded(method.Padding.Select, JRightPadded.Location.METHOD_SELECT, ".", p);
        VisitContainer("<", method.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        Visit(method.Name, p);
        VisitContainer("(", method.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
        AfterSyntax(method, p);
        return method;
    }

    public override J VisitMultiCatch(J.MultiCatch multiCatch, PrintOutputCapture<P> p)
    {
        BeforeSyntax(multiCatch, Space.Location.MULTI_CATCH_PREFIX, p);
        VisitRightPadded(multiCatch.Padding.Alternatives, JRightPadded.Location.CATCH_ALTERNATIVE, "|", p);
        AfterSyntax(multiCatch, p);
        return multiCatch;
    }

    public override J VisitVariableDeclarations(J.VariableDeclarations multiVariable, PrintOutputCapture<P> p)
    {
        BeforeSyntax(multiVariable, Space.Location.VARIABLE_DECLARATIONS_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(multiVariable.LeadingAnnotations, p);
        foreach (var m in multiVariable.Modifiers)
        {
            VisitModifier(m, p);
        }

        Visit(multiVariable.TypeExpression, p);

        //For backwards compatibility.
        foreach (var dim in multiVariable.DimensionsBeforeName)
        {
            VisitSpace(dim.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(dim.Element, Space.Location.DIMENSION, p);
            p.Append(']');
        }

        if (multiVariable.Varargs != null)
        {
            VisitSpace(multiVariable.Varargs, Space.Location.VARARGS, p);
            p.Append("...");
        }

        VisitRightPadded(multiVariable.Padding.Variables, JRightPadded.Location.NAMED_VARIABLE, ",", p);
        AfterSyntax(multiVariable, p);
        return multiVariable;
    }

    public override J VisitNewArray(J.NewArray newArray, PrintOutputCapture<P> p)
    {
        BeforeSyntax(newArray, Space.Location.NEW_ARRAY_PREFIX, p);
        if (newArray.TypeExpression != null)
        {
            p.Append("new");
        }

        Visit(newArray.TypeExpression, p);
        Visit(newArray.Dimensions, p);
        VisitContainer("{", newArray.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
        AfterSyntax(newArray, p);
        return newArray;
    }

    public override J VisitNewClass(J.NewClass newClass, PrintOutputCapture<P> p)
    {
        BeforeSyntax(newClass, Space.Location.NEW_CLASS_PREFIX, p);
        VisitRightPadded(newClass.Padding.Enclosing, JRightPadded.Location.NEW_CLASS_ENCLOSING, ".", p);
        VisitSpace(newClass.New, Space.Location.NEW_PREFIX, p);
        p.Append("new");
        Visit(newClass.Clazz, p);
        if (newClass.Padding.Arguments.Markers.FindFirst<OmitParentheses>() == null)
        {
            VisitContainer("(", newClass.Padding.Arguments, JContainer.Location.NEW_CLASS_ARGUMENTS, ",", ")", p);
        }

        Visit(newClass.Body, p);
        AfterSyntax(newClass, p);
        return newClass;
    }

    public override J VisitNullableType(J.NullableType nt, PrintOutputCapture<P> p)
    {
        BeforeSyntax(nt, Space.Location.NULLABLE_TYPE_PREFIX, p);
        Visit(nt.TypeTree, p);
        VisitSpace(nt.Padding.TypeTree.After, Space.Location.NULLABLE_TYPE_SUFFIX, p);
        p.Append("?");
        AfterSyntax(nt, p);
        return nt;
    }

    public override J VisitPackage(J.Package pkg, PrintOutputCapture<P> p)
    {
        foreach (var a in pkg.Annotations)
        {
            VisitAnnotation(a, p);
        }

        BeforeSyntax(pkg, Space.Location.PACKAGE_PREFIX, p);
        p.Append("package");
        Visit(pkg.Expression, p);
        AfterSyntax(pkg, p);
        return pkg;
    }

    public override J VisitParameterizedType(J.ParameterizedType type, PrintOutputCapture<P> p)
    {
        BeforeSyntax(type, Space.Location.PARAMETERIZED_TYPE_PREFIX, p);
        Visit(type.Clazz, p);
        VisitContainer("<", type.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        AfterSyntax(type, p);
        return type;
    }

    public override J VisitPrimitive(J.Primitive primitive, PrintOutputCapture<P> p)
    {
        if (true)
        {
            BeforeSyntax(primitive, Space.Location.PRIMITIVE_PREFIX, p);
            p.Append("foo");
            AfterSyntax(primitive, p);
            return primitive;
        }
        string keyword;
        switch (primitive.Type.Kind)
        {
            case JavaType.Primitive.PrimitiveType.Boolean:
                keyword = "bool";
                break;
            case JavaType.Primitive.PrimitiveType.Byte:
                keyword = "byte";
                break;
            case JavaType.Primitive.PrimitiveType.Char:
                keyword = "char";
                break;
            case JavaType.Primitive.PrimitiveType.Double:
                keyword = "double";
                break;
            case JavaType.Primitive.PrimitiveType.Float:
                keyword = "float";
                break;
            case JavaType.Primitive.PrimitiveType.Int:
                keyword = "int";
                break;
            case JavaType.Primitive.PrimitiveType.Long:
                keyword = "long";
                break;
            case JavaType.Primitive.PrimitiveType.Short:
                keyword = "short";
                break;
            case JavaType.Primitive.PrimitiveType.Void:
                keyword = "void";
                break;
            case JavaType.Primitive.PrimitiveType.String:
                keyword = "string";
                break;
            case JavaType.Primitive.PrimitiveType.None:
                throw new InvalidOperationException("Unable to print None primitive");
            case JavaType.Primitive.PrimitiveType.Null:
                throw new InvalidOperationException("Unable to print Null primitive");
            default:
                throw new InvalidOperationException("Unable to print non-primitive type");
        }

        BeforeSyntax(primitive, Space.Location.PRIMITIVE_PREFIX, p);
        p.Append(keyword);
        AfterSyntax(primitive, p);
        return primitive;
    }

    public override J VisitParentheses<T>(J.Parentheses<T> parens, PrintOutputCapture<P> p)
    {
        BeforeSyntax(parens, Space.Location.PARENTHESES_PREFIX, p);
        p.Append('(');
        VisitRightPadded(parens.Padding.Tree, JRightPadded.Location.PARENTHESES, ")", p);
        AfterSyntax(parens, p);
        return parens;
    }

    public override J VisitReturn(J.Return return_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(return_, Space.Location.RETURN_PREFIX, p);
        p.Append("return");
        Visit(return_.Expression, p);
        AfterSyntax(return_, p);
        return return_;
    }

    public override J VisitSwitch(J.Switch switch_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(switch_, Space.Location.SWITCH_PREFIX, p);
        p.Append("switch");
        Visit(switch_.Selector, p);
        Visit(switch_.Cases, p);
        AfterSyntax(switch_, p);
        return switch_;
    }

    public override J VisitSwitchExpression(J.SwitchExpression switch_, PrintOutputCapture<P> p)
    {
        BeforeSyntax(switch_, Space.Location.SWITCH_EXPRESSION_PREFIX, p);
        p.Append("switch");
        Visit(switch_.Selector, p);
        Visit(switch_.Cases, p);
        AfterSyntax(switch_, p);
        return switch_;
    }

    public override J VisitSynchronized(J.Synchronized synch, PrintOutputCapture<P> p)
    {
        BeforeSyntax(synch, Space.Location.SYNCHRONIZED_PREFIX, p);
        p.Append("synchronized");
        Visit(synch.Lock, p);
        Visit(synch.Body, p);
        AfterSyntax(synch, p);
        return synch;
    }

    public override J VisitTernary(J.Ternary ternary, PrintOutputCapture<P> p)
    {
        BeforeSyntax(ternary, Space.Location.TERNARY_PREFIX, p);
        Visit(ternary.Condition, p);
        VisitLeftPadded("?", ternary.Padding.TruePart, JLeftPadded.Location.TERNARY_TRUE, p);
        VisitLeftPadded(":", ternary.Padding.FalsePart, JLeftPadded.Location.TERNARY_FALSE, p);
        AfterSyntax(ternary, p);
        return ternary;
    }

    public override J VisitThrow(J.Throw thrown, PrintOutputCapture<P> p)
    {
        BeforeSyntax(thrown, Space.Location.THROW_PREFIX, p);
        p.Append("throw");
        Visit(thrown.Exception, p);
        AfterSyntax(thrown, p);
        return thrown;
    }

    public override J VisitTry(J.Try tryable, PrintOutputCapture<P> p)
    {
        BeforeSyntax(tryable, Space.Location.TRY_PREFIX, p);
        p.Append("try");
        if (tryable.Padding.Resources != null)
        {
            //Note: we do not call VisitContainer here because the last resource may or may not be semicolon terminated.
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
        }

        Visit(tryable.Body, p);
        Visit(tryable.Catches, p);
        VisitLeftPadded("finally", tryable.Padding.Finally, JLeftPadded.Location.TRY_FINALLY, p);
        AfterSyntax(tryable, p);
        return tryable;
    }

    public override J VisitTypeCast(J.TypeCast typeCast, PrintOutputCapture<P> p)
    {
        BeforeSyntax(typeCast, Space.Location.TYPE_CAST_PREFIX, p);
        Visit(typeCast.Clazz, p);
        Visit(typeCast.Expression, p);
        AfterSyntax(typeCast, p);
        return typeCast;
    }

    public override J VisitTypeParameter(J.TypeParameter typeParam, PrintOutputCapture<P> p)
    {
        BeforeSyntax(typeParam, Space.Location.TYPE_PARAMETERS_PREFIX, p);
        Visit(typeParam.Annotations, p);
        Visit(typeParam.Name, p);
        VisitContainer("extends", typeParam.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, "&", "", p);
        AfterSyntax(typeParam, p);
        return typeParam;
    }

    public override J VisitUnary(J.Unary unary, PrintOutputCapture<P> p)
    {
        BeforeSyntax(unary, Space.Location.UNARY_PREFIX, p);
        switch (unary.Operator)
        {
            case J.Unary.Type.PreIncrement:
                p.Append("++");
                Visit(unary.Expression, p);
                break;
            case J.Unary.Type.PreDecrement:
                p.Append("--");
                Visit(unary.Expression, p);
                break;
            case J.Unary.Type.PostIncrement:
                Visit(unary.Expression, p);
                VisitSpace(unary.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("++");
                break;
            case J.Unary.Type.PostDecrement:
                Visit(unary.Expression, p);
                VisitSpace(unary.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("--");
                break;
            case J.Unary.Type.Positive:
                p.Append('+');
                Visit(unary.Expression, p);
                break;
            case J.Unary.Type.Negative:
                p.Append('-');
                Visit(unary.Expression, p);
                break;
            case J.Unary.Type.Complement:
                p.Append('~');
                Visit(unary.Expression, p);
                break;
            case J.Unary.Type.Not:
            default:
                p.Append('!');
                Visit(unary.Expression, p);
                break;
        }

        AfterSyntax(unary, p);
        return unary;
    }

    public override J VisitUnknown(J.Unknown unknown, PrintOutputCapture<P> p)
    {
        BeforeSyntax(unknown, Space.Location.UNKNOWN_PREFIX, p);
        Visit(unknown.UnknownSource, p);
        AfterSyntax(unknown, p);
        return unknown;
    }

    public override J VisitUnknownSource(J.Unknown.Source source, PrintOutputCapture<P> p)
    {
        BeforeSyntax(source, Space.Location.UNKNOWN_SOURCE_PREFIX, p);
        p.Append(source.Text);
        AfterSyntax(source, p);
        return source;
    }

    public override J VisitVariable(J.VariableDeclarations.NamedVariable variable, PrintOutputCapture<P> p)
    {
        BeforeSyntax(variable, Space.Location.VARIABLE_PREFIX, p);
        Visit(variable.Name, p);
        foreach (var dimension in variable.DimensionsAfterName)
        {
            VisitSpace(dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(dimension.Element, Space.Location.DIMENSION, p);
            p.Append(']');
        }

        VisitLeftPadded("=", variable.Padding.Initializer, JLeftPadded.Location.VARIABLE_INITIALIZER, p);
        AfterSyntax(variable, p);
        return variable;
    }

    public override J VisitWhileLoop(J.WhileLoop whileLoop, PrintOutputCapture<P> p)
    {
        BeforeSyntax(whileLoop, Space.Location.WHILE_PREFIX, p);
        p.Append("while");
        Visit(whileLoop.Condition, p);
        VisitStatement(whileLoop.Padding.Body, JRightPadded.Location.WHILE_BODY, p);
        AfterSyntax(whileLoop, p);
        return whileLoop;
    }

    public override J VisitWildcard(J.Wildcard wildcard, PrintOutputCapture<P> p)
    {
        BeforeSyntax(wildcard, Space.Location.WILDCARD_PREFIX, p);
        p.Append('?');
        if (wildcard.Padding.WildcardBound != null)
        {
            switch (wildcard.WildcardBound)
            {
                case J.Wildcard.Bound.Extends:
                    VisitSpace(wildcard.Padding.WildcardBound.Before, Space.Location.WILDCARD_BOUND, p);
                    p.Append("extends");
                    break;
                case J.Wildcard.Bound.Super:
                    VisitSpace(wildcard.Padding.WildcardBound.Before, Space.Location.WILDCARD_BOUND, p);
                    p.Append("super");
                    break;
            }
        }

        Visit(wildcard.BoundedType, p);
        AfterSyntax(wildcard, p);
        return wildcard;
    }

    public override J VisitYield(J.Yield yield, PrintOutputCapture<P> p)
    {
        BeforeSyntax(yield, Space.Location.YIELD_PREFIX, p);
        if (!yield.Implicit)
        {
            p.Append("yield");
        }

        Visit(yield.Value, p);
        AfterSyntax(yield, p);
        return yield;
    }

    private static readonly Func<string, string> MARKER_WRAPPER =
        o => "/*~~" + o + (o.Length == 0 ? "" : "~~") + ">*/";

    protected void BeforeSyntax(J j, Space.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(j.Prefix, j.Markers, loc, p);
    }

    protected void BeforeSyntax(Space prefix, Markers markers, Space.Location? loc, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }

        VisitSpace(prefix, loc, p);
        VisitMarkers(markers, p);
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }
    }

    protected void AfterSyntax(J j, PrintOutputCapture<P> p)
    {
        AfterSyntax(j.Markers, p);
    }

    protected void AfterSyntax(Markers markers, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }
    }
}