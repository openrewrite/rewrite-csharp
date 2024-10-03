using Rewrite.Core;

namespace Rewrite.RewriteJava.Tree
{
    public partial interface J
    {
        partial class AnnotatedType
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Annotation
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ArrayAccess
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ArrayDimension
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ArrayType
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Assert
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Assignment
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class AssignmentOperation
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Binary
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Block
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Break
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Case
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ClassDeclaration
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class CompilationUnit
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Continue
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ControlParentheses<J2>
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class DoWhileLoop
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Empty
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class EnumValue
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class EnumValueSet
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class FieldAccess
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ForEachLoop
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ForLoop
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Identifier
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class If
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Import
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class InstanceOf
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class IntersectionType
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Label
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Lambda
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Literal
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class MemberReference
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class MethodDeclaration
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class MethodInvocation
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Modifier
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class MultiCatch
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class NewArray
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class NewClass
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class NullableType
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Package
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ParameterizedType
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Parentheses<J2>
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ParenthesizedTypeTree
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Primitive
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Return
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Switch
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class SwitchExpression
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Synchronized
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Ternary
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Throw
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Try
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class TypeCast
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class TypeParameter
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class TypeParameters
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Unary
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Unknown
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class VariableDeclarations
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class WhileLoop
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Wildcard
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Yield
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }
    }
}
