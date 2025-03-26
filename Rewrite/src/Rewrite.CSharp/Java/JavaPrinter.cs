using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava;

public class JavaPrinter<P> : JavaVisitor<PrintOutputCapture<P>>
{
    private static readonly Func<string, string> JAVA_MARKER_WRAPPER =
        o => $"/*~~{o}{(string.IsNullOrEmpty(o) ? "" : "~~")}>*/";

    public JavaPrinter()
    {
    }

    protected virtual void VisitRightPadded<T>(IList<JRightPadded<T>> nodes, JRightPadded.Location location, string suffixBetween, PrintOutputCapture<P> p) where T : J
    {
        for (int i = 0; i < nodes.Count; ++i)
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

    protected virtual void VisitContainer<T>(string before, JContainer<T>? container, JContainer.Location location,
        string suffixBetween, string? after, PrintOutputCapture<P> p) where T : J
    {
        if (container != null)
        {
            BeforeSyntax(container.Before, container.Markers, location.BeforeLocation, p);
            p.Append(before);
            VisitRightPadded(container.Padding.Elements, location.ElementLocation, suffixBetween, p);
            AfterSyntax(container.Markers, p);
            p.Append(after ?? "");
        }
    }


    public override Space VisitSpace(Space space, Space.Location? loc, PrintOutputCapture<P> p)
    {
        p.Append(space.Whitespace);
        var comments = space.Comments;

        for (int i = 0; i < comments.Count; ++i)
        {
            var comment = comments[i];
            VisitMarkers(comment.Markers, p);
            comment.PrintComment(Cursor, p);
            p.Append(comment.Suffix);
        }

        return space;
    }

    protected virtual void VisitLeftPadded<T>(string? prefix, JLeftPadded<T>? leftPadded, JLeftPadded.Location location,
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

    protected virtual void VisitRightPadded<T>(JRightPadded<T>? rightPadded, JRightPadded.Location location, string? suffix,
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

    public override J? VisitModifier(J.Modifier mod, PrintOutputCapture<P> p)
    {
        Visit(mod.Annotations, p);
        var keyword = mod.ModifierType switch
        {
            J.Modifier.Types.Default => "default",
            J.Modifier.Types.Public => "public",
            J.Modifier.Types.Protected => "protected",
            J.Modifier.Types.Private => "private",
            J.Modifier.Types.Abstract => "abstract",
            J.Modifier.Types.Static => "static",
            J.Modifier.Types.Final => "final",
            J.Modifier.Types.Native => "native",
            J.Modifier.Types.NonSealed => "non-sealed",
            J.Modifier.Types.Sealed => "sealed",
            J.Modifier.Types.Strictfp => "strictfp",
            J.Modifier.Types.Synchronized => "synchronized",
            J.Modifier.Types.Transient => "transient",
            J.Modifier.Types.Volatile => "volatile",
            J.Modifier.Types.Async => "async",
            J.Modifier.Types.Reified => "reified",
            J.Modifier.Types.Inline => "inline",
            _ => mod.Keyword
        };

        BeforeSyntax(mod, Space.Location.MODIFIER_PREFIX, p);
        p.Append(keyword);
        AfterSyntax(mod, p);
        return mod;
    }

    public override J? VisitAnnotation(J.Annotation annotation, PrintOutputCapture<P> p)
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
        while (type is J.ArrayType arrayTypeElement)
        {
            type = arrayTypeElement.ElementType;
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
        VisitSpace(arrayType.Dimension?.Before ?? Space.EMPTY, Space.Location.DIMENSION_PREFIX, p);
        p.Append('[');
        VisitSpace(arrayType.Dimension?.Element ?? Space.EMPTY, Space.Location.DIMENSION, p);
        p.Append(']');
        if (arrayType.ElementType is J.ArrayType)
        {
            PrintDimensions((J.ArrayType)arrayType.ElementType, p);
        }

        AfterSyntax(arrayType, p);
    }

    public override J VisitAssert(J.Assert assert, PrintOutputCapture<P> p)
    {
        BeforeSyntax(assert, Space.Location.ASSERT_PREFIX, p);
        p.Append("assert");
        Visit(assert.Condition, p);
        VisitLeftPadded(":", assert.Detail, JLeftPadded.Location.ASSERT_DETAIL, p);
        AfterSyntax(assert, p);
        return assert;
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
        string keyword = assignOp.Operator switch
        {
            J.AssignmentOperation.Types.Addition => "+=",
            J.AssignmentOperation.Types.Subtraction => "-=",
            J.AssignmentOperation.Types.Multiplication => "*=",
            J.AssignmentOperation.Types.Division => "/=",
            J.AssignmentOperation.Types.Modulo => "%=",
            J.AssignmentOperation.Types.BitAnd => "&=",
            J.AssignmentOperation.Types.BitOr => "|=",
            J.AssignmentOperation.Types.BitXor => "^=",
            J.AssignmentOperation.Types.LeftShift => "<<=",
            J.AssignmentOperation.Types.RightShift => ">>=",
            J.AssignmentOperation.Types.UnsignedRightShift => ">>>=",
            _ => ""
        };

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
        string keyword = binary.Operator switch
        {
            J.Binary.Types.Addition => "+",
            J.Binary.Types.Subtraction => "-",
            J.Binary.Types.Multiplication => "*",
            J.Binary.Types.Division => "/",
            J.Binary.Types.Modulo => "%",
            J.Binary.Types.LessThan => "<",
            J.Binary.Types.GreaterThan => ">",
            J.Binary.Types.LessThanOrEqual => "<=",
            J.Binary.Types.GreaterThanOrEqual => ">=",
            J.Binary.Types.Equal => "==",
            J.Binary.Types.NotEqual => "!=",
            J.Binary.Types.BitAnd => "&",
            J.Binary.Types.BitOr => "|",
            J.Binary.Types.BitXor => "^",
            J.Binary.Types.LeftShift => "<<",
            J.Binary.Types.RightShift => ">>",
            J.Binary.Types.UnsignedRightShift => ">>>",
            J.Binary.Types.Or => "||",
            J.Binary.Types.And => "&&",
            _ => ""
        };

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

    protected virtual void VisitStatements(IList<JRightPadded<Statement>> statements, JRightPadded.Location location,
        PrintOutputCapture<P> p)
    {
        foreach (var paddedStat in statements)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement(JRightPadded<Statement>? paddedStat, JRightPadded.Location location, PrintOutputCapture<P> p)
    {
        if (paddedStat != null)
        {
            Visit(paddedStat.Element, p);
            VisitSpace(paddedStat.After, location.AfterLocation, p);
            VisitMarkers(paddedStat.Markers, p);
            PrintStatementTerminator(paddedStat.Element, p);
        }
    }

    public virtual void PrintStatementTerminator(Statement s, PrintOutputCapture<P> p)
    {
        while (true)
        {
            if (s is not J.Assert and not J.Assignment and not J.AssignmentOperation and not J.Break and not J.Continue
                and not J.DoWhileLoop and not J.Empty and not J.MethodInvocation and not J.NewClass and not J.Return
                and not J.Throw and not J.Unary and not J.VariableDeclarations and not J.Yield)
            {
                if (s is J.MethodDeclaration methodDecl && methodDecl.Body == null)
                {
                    p.Append(';');
                    return;
                }

                if (s is J.Label label)
                {
                    s = label.Statement;
                    continue;
                }

                if (Cursor.Value is J.Case)
                {
                    var aSwitch = Cursor.DropParentUntil(c => c is J.Switch or J.SwitchExpression or "root").Value;
                    if (aSwitch is J.SwitchExpression)
                    {
                        var aCase = (J.Case)Cursor.Value;
                        if (aCase.Body is not J.Block)
                        {
                            p.Append(';');
                        }

                        return;
                    }
                }

                return;
            }

            p.Append(';');
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

    public override J VisitCase(J.Case @case, PrintOutputCapture<P> p)
    {
        BeforeSyntax(@case, Space.Location.CASE_PREFIX, p);
        Expression elem = (Expression)@case.CaseLabels[0];
        if (elem is not J.Identifier identifier || !identifier.SimpleName.Equals("default"))
        {
            p.Append("case");
        }

        VisitContainer("", @case.Padding.CaseLabels, JContainer.Location.CASE_CASE_LABELS, ",", "", p);
        VisitSpace(@case.Padding.Statements.Before, Space.Location.CASE, p);
        p.Append(@case.CaseType == J.Case.Types.Statement ? ":" : "->");
        VisitStatements(@case.Padding.Statements.Padding.Elements, JRightPadded.Location.CASE, p);
        if (@case.Body is Statement)
        {
            VisitStatement(@case.Padding.Body?.Cast<Statement>(), JRightPadded.Location.CASE_BODY, p);
        }
        else
        {
            VisitRightPadded(@case.Padding.Body!, JRightPadded.Location.CASE_BODY, ";", p);
        }

        AfterSyntax(@case, p);
        return @case;
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
        var kind = classDecl.DeclarationKind switch
        {
            J.ClassDeclaration.Kind.Types.Class => "class",
            J.ClassDeclaration.Kind.Types.Enum => "enum",
            J.ClassDeclaration.Kind.Types.Interface => "interface",
            J.ClassDeclaration.Kind.Types.Annotation => "@interface",
            J.ClassDeclaration.Kind.Types.Record => "record",
            _ => ""
        };

        BeforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(classDecl.LeadingAnnotations, p);
        foreach (var m in classDecl.Modifiers)
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
        VisitContainer(classDecl.DeclarationKind.Equals(J.ClassDeclaration.Kind.Types.Interface) ? "extends" : "implements",
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
        if (cu.Imports.Count > 0)
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
            if (!initializer.Padding.Arguments.Markers.Any(m => m is OmitParentheses))
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
        var ctrl = forLoop.LoopControl;
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
        var ctrl = forEachLoop.LoopControl;
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

    public override J VisitImport(J.Import import, PrintOutputCapture<P> p)
    {
        BeforeSyntax(import, Space.Location.IMPORT_PREFIX, p);
        p.Append("import");
        if (import.Static)
        {
            VisitSpace(import.Padding.Static.Before, Space.Location.STATIC_IMPORT, p);
            p.Append("static");
        }

        Visit(import.Qualid, p);
        AfterSyntax(import, p);
        return import;
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
        var unicodeEscapes = literal.UnicodeEscapes;
        if (unicodeEscapes == null)
        {
            p.Append(literal.ValueSource);
        }
        else if (literal.ValueSource != null)
        {
            var surrogateIter = unicodeEscapes.GetEnumerator();
            var surrogate = surrogateIter.MoveNext() ? surrogateIter.Current : null;
            int i = 0;
            if (surrogate != null && surrogate.ValueSourceIndex == 0)
            {
                p.Append("\\u").Append(surrogate.CodePoint);
                if (surrogateIter.MoveNext())
                {
                    surrogate = surrogateIter.Current;
                }
            }

            string valueSource = literal.ValueSource;
            for (int j = 0; j < valueSource.Length; ++j)
            {
                char c = valueSource[j];
                p.Append(c);
                if (surrogate != null)
                {
                    int var10000 = surrogate.ValueSourceIndex;
                    ++i;
                    if (var10000 == i)
                    {
                        while (surrogate != null && surrogate.ValueSourceIndex == i)
                        {
                            p.Append("\\u").Append(surrogate.CodePoint);
                            surrogate = surrogateIter.MoveNext() ? surrogateIter.Current : null;
                        }
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

        foreach (var modifier in method.Modifiers)
        {
            VisitModifier(modifier, p);
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
        Visit(method.Name, p);

        if (!method.Markers.Any(marker => marker is CompactConstructor))
        {
            VisitContainer("(", method.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        VisitContainer("throws", method.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
        Visit(method.Body, p);
        VisitLeftPadded("default", method.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE,
            p);
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

        foreach (var modifier in multiVariable.Modifiers)
        {
            VisitModifier(modifier, p);
        }

        Visit(multiVariable.TypeExpression, p);

        foreach (var dimension in multiVariable.DimensionsBeforeName)
        {
            VisitSpace(dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(dimension.Element, Space.Location.DIMENSION, p);
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

        if (!newClass.Padding.Arguments.Markers.Any(marker => marker is OmitParentheses))
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
        foreach (var annotation in pkg.Annotations)
        {
            VisitAnnotation(annotation, p);
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
        var keyword = primitive.Type.Kind switch
        {
            JavaType.Primitive.PrimitiveType.Boolean => "boolean",
            JavaType.Primitive.PrimitiveType.Byte => "byte",
            JavaType.Primitive.PrimitiveType.Char => "char",
            JavaType.Primitive.PrimitiveType.Double => "double",
            JavaType.Primitive.PrimitiveType.Float => "float",
            JavaType.Primitive.PrimitiveType.Int => "int",
            JavaType.Primitive.PrimitiveType.Long => "long",
            JavaType.Primitive.PrimitiveType.Short => "short",
            JavaType.Primitive.PrimitiveType.Void => "void",
            JavaType.Primitive.PrimitiveType.String => "String",
            _ => throw new InvalidOperationException("Unable to print unknown primitive type")
        };

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
            VisitSpace(tryable.Padding.Resources.Before, Space.Location.TRY_RESOURCES, p);
            p.Append('(');
            var resources = tryable.Padding.Resources.Padding.Elements;

            foreach (var resource in resources)
            {
                VisitSpace(resource.After, Space.Location.TRY_RESOURCE_SUFFIX, p);
                VisitSpace(resource.Element.Prefix, Space.Location.TRY_RESOURCE, p);
                VisitMarkers(resource.Element.Markers, p);
                Visit(resource.Element.VariableDeclarations, p);

                if (resource.Element.TerminatedWithSemicolon)
                {
                    p.Append(';');
                }
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
            case J.Unary.Types.PreIncrement:
                p.Append("++");
                Visit(unary.Expression, p);
                break;
            case J.Unary.Types.PreDecrement:
                p.Append("--");
                Visit(unary.Expression, p);
                break;
            case J.Unary.Types.PostIncrement:
                Visit(unary.Expression, p);
                VisitSpace(unary.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("++");
                break;
            case J.Unary.Types.PostDecrement:
                Visit(unary.Expression, p);
                VisitSpace(unary.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("--");
                break;
            case J.Unary.Types.Positive:
                p.Append('+');
                Visit(unary.Expression, p);
                break;
            case J.Unary.Types.Negative:
                p.Append('-');
                Visit(unary.Expression, p);
                break;
            case J.Unary.Types.Complement:
                p.Append('~');
                Visit(unary.Expression, p);
                break;
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

    protected void BeforeSyntax(J j, Space.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(j.Prefix, j.Markers, loc, p);
    }

    protected void BeforeSyntax(Space prefix, Markers markers, Space.Location? loc, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc, p);
        }

        VisitMarkers(markers, p);

        foreach (var marker in markers)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }

    protected void AfterSyntax(J j, PrintOutputCapture<P> p)
    {
        AfterSyntax(j.Markers, p);
    }

    protected void AfterSyntax(Markers markers, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), JAVA_MARKER_WRAPPER));
        }
    }
}
