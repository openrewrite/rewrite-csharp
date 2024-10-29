using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree
{
    public partial interface Cs
    {
        partial class UsingStatement
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }
        partial class AnnotatedStatement
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ArrayRankSpecifier
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class AssignmentOperation
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class AttributeList
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class AwaitExpression
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Binary
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class BlockScopeNamespaceDeclaration
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class CollectionExpression
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class CompilationUnit
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ExpressionStatement
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class ExternAlias
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class FileScopeNamespaceDeclaration
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class InterpolatedString
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class Interpolation
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class NamedArgument
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class NullSafeExpression
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class PropertyDeclaration
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class StatementExpression
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }

        partial class UsingDirective
        {
            public override string? ToString() => Core.Tree.ToString(this) ?? base.ToString();
        }
    }
}
