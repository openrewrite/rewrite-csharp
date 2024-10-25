using System.Diagnostics.CodeAnalysis;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public interface CsSpace
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public record Location
    {
        public static readonly Location TYPE_PARAMETERS_CONSTRAINT_PREFIX = new();
        public static readonly Location TYPE_PARAMETERS_CONSTRAINT_CLAUSE_PREFIX = new();
        public static readonly Location METHOD_DECLARATION_PREFIX = new();
        public static readonly Location METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES = new();
        public static readonly Location REF_STRUCT_CONSTRAINT_PREFIX = new();
        public static readonly Location ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS = new();
        public static readonly Location ALLOWS_CONSTRAINT_CLAUSE_PREFIX = new();
        public static readonly Location DEFAULT_CONSTRAINT_PREFIX = new();
        public static readonly Location CONSTRUCTOR_CONSTRAINT_PREFIX = new();
        public static readonly Location TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS = new();
        public static readonly Location CLASS_OR_STRUCT_CONSTRAINT_PREFIX = new();
        public static readonly Location ALLOWS_CONSTRAINT_EXPRESSIONS = new();
        public static readonly Location ALLOWS_CONSTRAINT_PREFIX = new();
        public static readonly Location TYPE_CONSTRAINT_PREFIX = new();
        public static readonly Location TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS = new();
        public static readonly Location TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER = new();
        public static readonly Location TYPE_PARAMETER_CONSTRAINT_CLAUSE_PREFIX = new();
        public static readonly Location CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES = new();
        public static readonly Location CLASS_DECLARATION_PREFIX = new();
        public static readonly Location NAMED_ARGUMENT_NAME_COLUMN_SUFFIX = new();
        public static readonly Location NAMED_ARGUMENT_PREFIX = new();
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
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_EXTERNS_SUFFIX = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX = new();
        public static readonly Location BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS = new();
        public static readonly Location COLLECTION_EXPRESSION_ELEMENTS = new();
        public static readonly Location COLLECTION_EXPRESSION_PREFIX = new();
        public static readonly Location COMPILATION_UNIT_EXTERNS_SUFFIX = new();
        public static readonly Location COMPILATION_UNIT_MEMBERS = new();
        public static readonly Location COMPILATION_UNIT_USINGS = new();
        public static readonly Location EXPRESSION_STATEMENT_PREFIX = new();
        public static readonly Location EXTERN_ALIAS_IDENTIFIER = new();
        public static readonly Location EXTERN_ALIAS_PREFIX = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_EXTERNS_SUFFIX = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_NAME = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX = new();
        public static readonly Location FILE_SCOPE_NAMESPACE_DECLARATION_USINGS = new();
        public static readonly Location INTERPOLATED_STRING_PARTS_SUFFIX = new();
        public static readonly Location INTERPOLATED_STRING_PREFIX = new();
        public static readonly Location INTERPOLATION_ALIGNMENT_SUFFIX = new();
        public static readonly Location INTERPOLATION_EXPRESSION_SUFFIX = new();
        public static readonly Location INTERPOLATION_FORMAT_SUFFIX = new();
        public static readonly Location INTERPOLATION_PREFIX = new();
        public static readonly Location NULL_SAFE_EXPRESSION_EXPRESSION_SUFFIX = new();
        public static readonly Location NULL_SAFE_EXPRESSION_PREFIX = new();
        public static readonly Location PROPERTY_DECLARATION_ACCESSORS = new();
        public static readonly Location PROPERTY_DECLARATION_ACCESSORS_PREFIX = new();
        public static readonly Location PROPERTY_DECLARATION_EXPRESSION_BODY = new();
        public static readonly Location PROPERTY_DECLARATION_INITIALIZER = new();
        public static readonly Location PROPERTY_DECLARATION_INTERFACE_SPECIFIER_PREFIX = new();
        public static readonly Location PROPERTY_DECLARATION_PREFIX = new();
        public static readonly Location STATEMENT_EXPRESSION_PREFIX = new();
        public static readonly Location USING_DIRECTIVE_ALIAS = new();
        public static readonly Location USING_DIRECTIVE_GLOBAL_SUFFIX = new();
        public static readonly Location USING_DIRECTIVE_PREFIX = new();
        public static readonly Location USING_DIRECTIVE_STATIC = new();
        public static readonly Location USING_DIRECTIVE_UNSAFE = new();
        public static readonly Location LAMBDA_PREFIX = new();
    }
}
