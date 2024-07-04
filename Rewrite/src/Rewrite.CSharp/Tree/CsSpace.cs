using System.Diagnostics.CodeAnalysis;

namespace Rewrite.RewriteCSharp.Tree;

public interface CsSpace
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public record Location
    {
        public static readonly Location ANNOTATED_STATEMENT_PREFIX = new();
        public static readonly Location ARRAY_RANK_SPECIFIER_PREFIX = new();
        public static readonly Location ARRAY_RANK_SPECIFIER_SIZES = new();
        public static readonly Location ARRAY_RANK_SPECIFIER_SIZE_SUFFIX = new();
        public static readonly Location ASSIGNMENT_OPERATION_OPERATOR = new();
        public static readonly Location ASSIGNMENT_OPERATION_PREFIX = new();
        public static readonly Location ATTRIBUTE_LIST_ATTRIBUTE_SUFFIX = new();
        public static readonly Location ATTRIBUTE_LIST_PREFIX = new();
        public static readonly Location ATTRIBUTE_LIST_TARGET_SUFFIX = new();
        public static readonly Location AWAIT_EXPRESSION_PREFIX = new();
        public static readonly Location BINARY_OPERATOR = new();
        public static readonly Location BINARY_PREFIX = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_END = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS = new();
        public static readonly Location COLLECTION_EXPRESSION_ELEMENTS = new();
        public static readonly Location COLLECTION_EXPRESSION_PREFIX = new();
        public static readonly Location COMPILATION_UNIT_MEMBERS = new();
        public static readonly Location COMPILATION_UNIT_USINGS = new();
        public static readonly Location EXPRESSION_STATEMENT_PREFIX = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_NAME = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_USINGS = new();
        public static readonly Location NULL_SAFE_EXPRESSION_EXPRESSION_SUFFIX = new();
        public static readonly Location NULL_SAFE_EXPRESSION_PREFIX = new();
        public static readonly Location STATEMENT_EXPRESSION_PREFIX = new();
        public static readonly Location USING_DIRECTIVE_ALIAS = new();
        public static readonly Location USING_DIRECTIVE_GLOBAL_SUFFIX = new();
        public static readonly Location USING_DIRECTIVE_PREFIX = new();
        public static readonly Location USING_DIRECTIVE_STATIC = new();
        public static readonly Location USING_DIRECTIVE_UNSAFE = new();
    }
}