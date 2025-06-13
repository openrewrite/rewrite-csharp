using System.Runtime.CompilerServices;
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

    protected virtual void VisitContainer<T>(string before, JContainer<T>? container, JContainer.Location location, string suffixBetween, string? after, PrintOutputCapture<P> p) where T : J
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

    public override J? VisitModifier(J.Modifier node, PrintOutputCapture<P> p)
    {
        Visit(node.Annotations, p);
        var keyword = node.ModifierType switch
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
            _ => node.Keyword
        };

        BeforeSyntax(node, Space.Location.MODIFIER_PREFIX, p);
        p.Append(keyword);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAnnotation(J.Annotation node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ANNOTATION_PREFIX, p);
        p.Append('@');
        Visit(node.AnnotationType, p);
        VisitContainer("(", node.Padding.Arguments, JContainer.Location.ANNOTATION_ARGUMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAnnotatedType(J.AnnotatedType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ANNOTATED_TYPE_PREFIX, p);
        Visit(node.Annotations, p);
        Visit(node.TypeExpression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArrayDimension(J.ArrayDimension node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.DIMENSION_PREFIX, p);
        p.Append('[');
        VisitRightPadded(node.Padding.Index, JRightPadded.Location.ARRAY_INDEX, "]", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitArrayType(J.ArrayType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ARRAY_TYPE_PREFIX, p);

        TypeTree type = node;
        while (type is J.ArrayType arrayTypeElement)
        {
            type = arrayTypeElement.ElementType;
        }

        Visit(type, p);
        Visit(node.Annotations, p);
        if (node.Dimension != null)
        {
            VisitSpace(node.Dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(node.Dimension.Element, Space.Location.DIMENSION, p);
            p.Append(']');
            if (node.ElementType is J.ArrayType)
            {
                PrintDimensions((J.ArrayType)node.ElementType, p);
            }
        }

        AfterSyntax(node, p);
        return node;
    }

    private void PrintDimensions(J.ArrayType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ARRAY_TYPE_PREFIX, p);
        Visit(node.Annotations, p);
        VisitSpace(node.Dimension?.Before ?? Space.EMPTY, Space.Location.DIMENSION_PREFIX, p);
        p.Append('[');
        VisitSpace(node.Dimension?.Element ?? Space.EMPTY, Space.Location.DIMENSION, p);
        p.Append(']');
        if (node.ElementType is J.ArrayType)
        {
            PrintDimensions((J.ArrayType)node.ElementType, p);
        }

        AfterSyntax(node, p);
    }

    public override J? VisitAssert(J.Assert node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ASSERT_PREFIX, p);
        p.Append("assert");
        Visit(node.Condition, p);
        VisitLeftPadded(":", node.Detail, JLeftPadded.Location.ASSERT_DETAIL, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAssignment(J.Assignment node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ASSIGNMENT_PREFIX, p);
        Visit(node.Variable, p);
        VisitLeftPadded("=", node.Padding.Expression, JLeftPadded.Location.ASSIGNMENT, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitAssignmentOperation(J.AssignmentOperation node, PrintOutputCapture<P> p)
    {
        string keyword = node.Operator switch
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

        BeforeSyntax(node, Space.Location.ASSIGNMENT_OPERATION_PREFIX, p);
        Visit(node.Variable, p);
        VisitSpace(node.Padding.Operator.Before, Space.Location.ASSIGNMENT_OPERATION_OPERATOR, p);
        p.Append(keyword);
        Visit(node.Assignment, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitBinary(J.Binary node, PrintOutputCapture<P> p)
    {
        string keyword = node.Operator switch
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

        BeforeSyntax(node, Space.Location.BINARY_PREFIX, p);
        Visit(node.Left, p);
        VisitSpace(node.Padding.Operator.Before, Space.Location.BINARY_OPERATOR, p);
        p.Append(keyword);
        Visit(node.Right, p);
        AfterSyntax(node, p);
        return node;
    }


    public override J? VisitBlock(J.Block node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.BLOCK_PREFIX, p);
        if (node.Static)
        {
            p.Append("static");
            VisitRightPadded(node.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
        }

        p.Append('{');
        VisitStatements(node.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
        VisitSpace(node.End, Space.Location.BLOCK_END, p);
        p.Append('}');
        AfterSyntax(node, p);
        return node;
    }

    protected virtual void VisitStatements(IList<JRightPadded<Statement>> statements, JRightPadded.Location location, PrintOutputCapture<P> p)
    {
        foreach (var paddedStat in statements)
        {
            VisitStatement(paddedStat, location, p);
        }
    }

    protected void VisitStatement(JRightPadded<Statement>? node, JRightPadded.Location location, PrintOutputCapture<P> p)
    {
        if (node != null)
        {
            Visit(node.Element, p);
            VisitSpace(node.After, location.AfterLocation, p);
            VisitMarkers(node.Markers, p);
            PrintStatementTerminator(node.Element, p);
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

    public override J? VisitBreak(J.Break node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.BREAK_PREFIX, p);
        p.Append("break");
        Visit(node.Label, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCase(J.Case node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.CASE_PREFIX, p);
        Expression elem = (Expression)node.CaseLabels[0];
        if (elem is not J.Identifier identifier || !identifier.SimpleName.Equals("default"))
        {
            p.Append("case");
        }

        VisitContainer("", node.Padding.CaseLabels, JContainer.Location.CASE_CASE_LABELS, ",", "", p);
        VisitSpace(node.Padding.Statements.Before, Space.Location.CASE, p);
        p.Append(node.CaseType == J.Case.Types.Statement ? ":" : "->");
        VisitStatements(node.Padding.Statements.Padding.Elements, JRightPadded.Location.CASE, p);
        if (node.Body is Statement)
        {
            VisitStatement(node.Padding.Body?.Cast<Statement>(), JRightPadded.Location.CASE_BODY, p);
        }
        else
        {
            VisitRightPadded(node.Padding.Body!, JRightPadded.Location.CASE_BODY, ";", p);
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCatch(J.Try.Catch node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.CATCH_PREFIX, p);
        p.Append("catch");
        Visit(node.Parameter, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitClassDeclaration(J.ClassDeclaration node, PrintOutputCapture<P> p)
    {
        var kind = node.DeclarationKind switch
        {
            J.ClassDeclaration.Kind.Types.Class => "class",
            J.ClassDeclaration.Kind.Types.Enum => "enum",
            J.ClassDeclaration.Kind.Types.Interface => "interface",
            J.ClassDeclaration.Kind.Types.Annotation => "@interface",
            J.ClassDeclaration.Kind.Types.Record => "record",
            _ => ""
        };

        BeforeSyntax(node, Space.Location.CLASS_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(node.LeadingAnnotations, p);
        foreach (var m in node.Modifiers)
        {
            VisitModifier(m, p);
        }

        Visit(node.Padding.DeclarationKind.Annotations, p);
        VisitSpace(node.Padding.DeclarationKind.Prefix, Space.Location.CLASS_KIND, p);
        p.Append(kind);
        Visit(node.Name, p);
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        VisitContainer("(", node.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
        VisitLeftPadded("extends", node.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
        VisitContainer(node.DeclarationKind.Equals(J.ClassDeclaration.Kind.Types.Interface) ? "extends" : "implements",
            node.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
        VisitContainer("permits", node.Padding.Permits, JContainer.Location.PERMITS, ",", null, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitCompilationUnit(J.CompilationUnit node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.COMPILATION_UNIT_PREFIX, p);
        VisitRightPadded(node.Padding.PackageDeclaration, JRightPadded.Location.PACKAGE, ";", p);
        VisitRightPadded(node.Padding.Imports, JRightPadded.Location.IMPORT, ";", p);
        if (node.Imports.Count > 0)
        {
            p.Append(';');
        }

        Visit(node.Classes, p);
        AfterSyntax(node, p);
        VisitSpace(node.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        return node;
    }

    public override J? VisitContinue(J.Continue node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.CONTINUE_PREFIX, p);
        p.Append("continue");
        Visit(node.Label, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitControlParentheses<T>(J.ControlParentheses<T> node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.CONTROL_PARENTHESES_PREFIX, p);
        p.Append('(');
        VisitRightPadded(node.Padding.Tree, JRightPadded.Location.PARENTHESES, ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitDoWhileLoop(J.DoWhileLoop node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.DO_WHILE_PREFIX, p);
        p.Append("do");
        VisitStatement(node.Padding.Body, JRightPadded.Location.WHILE_BODY, p);
        VisitLeftPadded("while", node.Padding.WhileCondition, JLeftPadded.Location.WHILE_CONDITION, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitElse(J.If.Else node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ELSE_PREFIX, p);
        p.Append("else");
        VisitStatement(node.Padding.Body, JRightPadded.Location.IF_ELSE, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEmpty(J.Empty node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.EMPTY_PREFIX, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEnumValue(J.EnumValue node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ENUM_VALUE_PREFIX, p);
        Visit(node.Annotations, p);
        Visit(node.Name, p);
        var initializer = node.Initializer;
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

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitEnumValueSet(J.EnumValueSet node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.ENUM_VALUE_SET_PREFIX, p);
        VisitRightPadded(node.Padding.Enums, JRightPadded.Location.ENUM_VALUE, ",", p);
        if (node.TerminatedWithSemicolon)
        {
            p.Append(';');
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitFieldAccess(J.FieldAccess node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.FIELD_ACCESS_PREFIX, p);
        Visit(node.Target, p);
        VisitLeftPadded(".", node.Padding.Name, JLeftPadded.Location.FIELD_ACCESS_NAME, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitForLoop(J.ForLoop node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.FOR_PREFIX, p);
        p.Append("for");
        var ctrl = node.LoopControl;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Init, JRightPadded.Location.FOR_INIT, ",", p);
        p.Append(';');
        VisitRightPadded(ctrl.Padding.Condition, JRightPadded.Location.FOR_CONDITION, ";", p);
        VisitRightPadded(ctrl.Padding.Update, JRightPadded.Location.FOR_UPDATE, ",", p);
        p.Append(')');
        VisitStatement(node.Padding.Body, JRightPadded.Location.FOR_BODY, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitForEachLoop(J.ForEachLoop node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.FOR_EACH_LOOP_PREFIX, p);
        p.Append("for");
        var ctrl = node.LoopControl;
        VisitSpace(ctrl.Prefix, Space.Location.FOR_EACH_CONTROL_PREFIX, p);
        p.Append('(');
        VisitRightPadded(ctrl.Padding.Variable, JRightPadded.Location.FOREACH_VARIABLE, ":", p);
        VisitRightPadded(ctrl.Padding.Iterable, JRightPadded.Location.FOREACH_ITERABLE, "", p);
        p.Append(')');
        VisitStatement(node.Padding.Body, JRightPadded.Location.FOR_BODY, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIdentifier(J.Identifier node, PrintOutputCapture<P> p)
    {
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(node.Annotations, p);
        BeforeSyntax(node, Space.Location.IDENTIFIER_PREFIX, p);
        p.Append(node.SimpleName);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIf(J.If node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.IF_PREFIX, p);
        p.Append("if");
        Visit(node.IfCondition, p);
        VisitStatement(node.Padding.ThenPart, JRightPadded.Location.IF_THEN, p);
        Visit(node.ElsePart, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitImport(J.Import node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.IMPORT_PREFIX, p);
        p.Append("import");
        if (node.Static)
        {
            VisitSpace(node.Padding.Static.Before, Space.Location.STATIC_IMPORT, p);
            p.Append("static");
        }

        Visit(node.Qualid, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitInstanceOf(J.InstanceOf node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.INSTANCEOF_PREFIX, p);
        VisitRightPadded(node.Padding.Expression, JRightPadded.Location.INSTANCEOF, "instanceof", p);
        Visit(node.Clazz, p);
        Visit(node.Pattern, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitIntersectionType(J.IntersectionType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.INTERSECTION_TYPE_PREFIX, p);
        VisitContainer("", node.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, "&", "", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLabel(J.Label node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.LABEL_PREFIX, p);
        VisitRightPadded(node.Padding.Name, JRightPadded.Location.LABEL, ":", p);
        Visit(node.Statement, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLambda(J.Lambda node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.LAMBDA_PREFIX, p);
        VisitSpace(node.Params.Prefix, Space.Location.LAMBDA_PARAMETERS_PREFIX, p);
        VisitMarkers(node.Params.Markers, p);
        if (node.Params.Parenthesized)
        {
            p.Append('(');
            VisitRightPadded(node.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
            p.Append(')');
        }
        else
        {
            VisitRightPadded(node.Params.Padding.Elements, JRightPadded.Location.LAMBDA_PARAM, ",", p);
        }

        VisitSpace(node.Arrow, Space.Location.LAMBDA_ARROW_PREFIX, p);
        p.Append("->");
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitLiteral(J.Literal node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.LITERAL_PREFIX, p);
        var unicodeEscapes = node.UnicodeEscapes;
        if (unicodeEscapes == null)
        {
            p.Append(node.ValueSource);
        }
        else if (node.ValueSource != null)
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

            string valueSource = node.ValueSource;
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

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMemberReference(J.MemberReference node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.MEMBER_REFERENCE_PREFIX, p);
        VisitRightPadded(node.Padding.Containing, JRightPadded.Location.MEMBER_REFERENCE_CONTAINING, p);
        p.Append("::");
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        VisitLeftPadded("", node.Padding.Reference, JLeftPadded.Location.MEMBER_REFERENCE_NAME, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMethodDeclaration(J.MethodDeclaration node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.METHOD_DECLARATION_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(node.LeadingAnnotations, p);

        foreach (var modifier in node.Modifiers)
        {
            VisitModifier(modifier, p);
        }

        var typeParameters = node.Annotations.TypeParameters;
        if (typeParameters != null)
        {
            Visit(typeParameters.Annotations, p);
            VisitSpace(typeParameters.Prefix, Space.Location.TYPE_PARAMETERS, p);
            VisitMarkers(typeParameters.Markers, p);
            p.Append('<');
            VisitRightPadded(typeParameters.Padding.Parameters, JRightPadded.Location.TYPE_PARAMETER, ",", p);
            p.Append('>');
        }

        Visit(node.ReturnTypeExpression, p);
        Visit(node.Annotations.Name.Annotations, p);
        Visit(node.Name, p);

        if (!node.Markers.Any(marker => marker is CompactConstructor))
        {
            VisitContainer("(", node.Padding.Parameters, JContainer.Location.METHOD_DECLARATION_PARAMETERS, ",", ")", p);
        }

        VisitContainer("throws", node.Padding.Throws, JContainer.Location.THROWS, ",", null, p);
        Visit(node.Body, p);
        VisitLeftPadded("default", node.Padding.DefaultValue, JLeftPadded.Location.METHOD_DECLARATION_DEFAULT_VALUE,
            p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMethodInvocation(J.MethodInvocation node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.METHOD_INVOCATION_PREFIX, p);
        VisitRightPadded(node.Padding.Select, JRightPadded.Location.METHOD_SELECT, ".", p);
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        Visit(node.Name, p);
        VisitContainer("(", node.Padding.Arguments, JContainer.Location.METHOD_INVOCATION_ARGUMENTS, ",", ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitMultiCatch(J.MultiCatch node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.MULTI_CATCH_PREFIX, p);
        VisitRightPadded(node.Padding.Alternatives, JRightPadded.Location.CATCH_ALTERNATIVE, "|", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitVariableDeclarations(J.VariableDeclarations node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.VARIABLE_DECLARATIONS_PREFIX, p);
        VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
        Visit(node.LeadingAnnotations, p);

        foreach (var modifier in node.Modifiers)
        {
            VisitModifier(modifier, p);
        }

        Visit(node.TypeExpression, p);

        foreach (var dimension in node.DimensionsBeforeName)
        {
            VisitSpace(dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(dimension.Element, Space.Location.DIMENSION, p);
            p.Append(']');
        }

        if (node.Varargs != null)
        {
            VisitSpace(node.Varargs, Space.Location.VARARGS, p);
            p.Append("...");
        }

        VisitRightPadded(node.Padding.Variables, JRightPadded.Location.NAMED_VARIABLE, ",", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitNewArray(J.NewArray node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.NEW_ARRAY_PREFIX, p);
        if (node.TypeExpression != null)
        {
            p.Append("new");
        }

        Visit(node.TypeExpression, p);
        Visit(node.Dimensions, p);
        VisitContainer("{", node.Padding.Initializer, JContainer.Location.NEW_ARRAY_INITIALIZER, ",", "}", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitNewClass(J.NewClass node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.NEW_CLASS_PREFIX, p);
        VisitRightPadded(node.Padding.Enclosing, JRightPadded.Location.NEW_CLASS_ENCLOSING, ".", p);
        VisitSpace(node.New, Space.Location.NEW_PREFIX, p);
        p.Append("new");
        Visit(node.Clazz, p);

        if (!node.Padding.Arguments.Markers.Any(marker => marker is OmitParentheses))
        {
            VisitContainer("(", node.Padding.Arguments, JContainer.Location.NEW_CLASS_ARGUMENTS, ",", ")", p);
        }

        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitNullableType(J.NullableType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.NULLABLE_TYPE_PREFIX, p);
        Visit(node.TypeTree, p);
        VisitSpace(node.Padding.TypeTree.After, Space.Location.NULLABLE_TYPE_SUFFIX, p);
        p.Append("?");
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPackage(J.Package node, PrintOutputCapture<P> p)
    {
        foreach (var annotation in node.Annotations)
        {
            VisitAnnotation(annotation, p);
        }

        BeforeSyntax(node, Space.Location.PACKAGE_PREFIX, p);
        p.Append("package");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParameterizedType(J.ParameterizedType node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.PARAMETERIZED_TYPE_PREFIX, p);
        Visit(node.Clazz, p);
        VisitContainer("<", node.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitPrimitive(J.Primitive node, PrintOutputCapture<P> p)
    {
        var keyword = node.Type.Kind switch
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

        BeforeSyntax(node, Space.Location.PRIMITIVE_PREFIX, p);
        p.Append(keyword);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitParentheses<T>(J.Parentheses<T> node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.PARENTHESES_PREFIX, p);
        p.Append('(');
        VisitRightPadded(node.Padding.Tree, JRightPadded.Location.PARENTHESES, ")", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitReturn(J.Return node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.RETURN_PREFIX, p);
        p.Append("return");
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitch(J.Switch node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_PREFIX, p);
        p.Append("switch");
        Visit(node.Selector, p);
        Visit(node.Cases, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSwitchExpression(J.SwitchExpression node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.SWITCH_EXPRESSION_PREFIX, p);
        p.Append("switch");
        Visit(node.Selector, p);
        Visit(node.Cases, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitSynchronized(J.Synchronized node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.SYNCHRONIZED_PREFIX, p);
        p.Append("synchronized");
        Visit(node.Lock, p);
        Visit(node.Body, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTernary(J.Ternary node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.TERNARY_PREFIX, p);
        Visit(node.Condition, p);
        VisitLeftPadded("?", node.Padding.TruePart, JLeftPadded.Location.TERNARY_TRUE, p);
        VisitLeftPadded(":", node.Padding.FalsePart, JLeftPadded.Location.TERNARY_FALSE, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitThrow(J.Throw node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.THROW_PREFIX, p);
        p.Append("throw");
        Visit(node.Exception, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTry(J.Try node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.TRY_PREFIX, p);
        p.Append("try");

        if (node.Padding.Resources != null)
        {
            VisitSpace(node.Padding.Resources.Before, Space.Location.TRY_RESOURCES, p);
            p.Append('(');
            var resources = node.Padding.Resources.Padding.Elements;

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

        Visit(node.Body, p);
        Visit(node.Catches, p);
        VisitLeftPadded("finally", node.Padding.Finally, JLeftPadded.Location.TRY_FINALLY, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypeCast(J.TypeCast node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.TYPE_CAST_PREFIX, p);
        Visit(node.Clazz, p);
        Visit(node.Expression, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitTypeParameter(J.TypeParameter node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.TYPE_PARAMETERS_PREFIX, p);
        Visit(node.Annotations, p);
        Visit(node.Name, p);
        VisitContainer("extends", node.Padding.Bounds, JContainer.Location.TYPE_BOUNDS, "&", "", p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnary(J.Unary node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.UNARY_PREFIX, p);
        switch (node.Operator)
        {
            case J.Unary.Types.PreIncrement:
                p.Append("++");
                Visit(node.Expression, p);
                break;
            case J.Unary.Types.PreDecrement:
                p.Append("--");
                Visit(node.Expression, p);
                break;
            case J.Unary.Types.PostIncrement:
                Visit(node.Expression, p);
                VisitSpace(node.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("++");
                break;
            case J.Unary.Types.PostDecrement:
                Visit(node.Expression, p);
                VisitSpace(node.Padding.Operator.Before, Space.Location.UNARY_OPERATOR, p);
                p.Append("--");
                break;
            case J.Unary.Types.Positive:
                p.Append('+');
                Visit(node.Expression, p);
                break;
            case J.Unary.Types.Negative:
                p.Append('-');
                Visit(node.Expression, p);
                break;
            case J.Unary.Types.Complement:
                p.Append('~');
                Visit(node.Expression, p);
                break;
            default:
                p.Append('!');
                Visit(node.Expression, p);
                break;
        }

        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnknown(J.Unknown node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.UNKNOWN_PREFIX, p);
        Visit(node.UnknownSource, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitUnknownSource(J.Unknown.Source node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.UNKNOWN_SOURCE_PREFIX, p);
        p.Append(node.Text);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitVariable(J.VariableDeclarations.NamedVariable node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.VARIABLE_PREFIX, p);
        Visit(node.Name, p);

        foreach (var dimension in node.DimensionsAfterName)
        {
            VisitSpace(dimension.Before, Space.Location.DIMENSION_PREFIX, p);
            p.Append('[');
            VisitSpace(dimension.Element, Space.Location.DIMENSION, p);
            p.Append(']');
        }

        VisitLeftPadded("=", node.Padding.Initializer, JLeftPadded.Location.VARIABLE_INITIALIZER, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitWhileLoop(J.WhileLoop node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.WHILE_PREFIX, p);
        p.Append("while");
        Visit(node.Condition, p);
        VisitStatement(node.Padding.Body, JRightPadded.Location.WHILE_BODY, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitWildcard(J.Wildcard node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.WILDCARD_PREFIX, p);
        p.Append('?');

        if (node.Padding.WildcardBound != null)
        {
            switch (node.WildcardBound)
            {
                case J.Wildcard.Bound.Extends:
                    VisitSpace(node.Padding.WildcardBound.Before, Space.Location.WILDCARD_BOUND, p);
                    p.Append("extends");
                    break;
                case J.Wildcard.Bound.Super:
                    VisitSpace(node.Padding.WildcardBound.Before, Space.Location.WILDCARD_BOUND, p);
                    p.Append("super");
                    break;
            }
        }

        Visit(node.BoundedType, p);
        AfterSyntax(node, p);
        return node;
    }

    public override J? VisitYield(J.Yield node, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node, Space.Location.YIELD_PREFIX, p);
        if (!node.Implicit)
        {
            p.Append("yield");
        }

        Visit(node.Value, p);
        AfterSyntax(node, p);
        return node;
    }

    protected void BeforeSyntax(J node, Space.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(node.Prefix, node.Markers, loc, p);
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
