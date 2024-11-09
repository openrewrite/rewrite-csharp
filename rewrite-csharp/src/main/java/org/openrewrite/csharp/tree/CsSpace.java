/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.openrewrite.csharp.tree;

public class CsSpace {
    public enum Location {
        ANNOTATED_STATEMENT_PREFIX,
        ARRAY_RANK_SPECIFIER_PREFIX,
        ARRAY_RANK_SPECIFIER_SIZES,
        ARRAY_RANK_SPECIFIER_SIZE_SUFFIX,
        ASSIGNMENT_OPERATION_OPERATOR,
        ASSIGNMENT_OPERATION_PREFIX,
        ATTRIBUTE_LIST_ATTRIBUTES_SUFFIX,
        ATTRIBUTE_LIST_PREFIX,
        ATTRIBUTE_LIST_TARGET_SUFFIX,
        AWAIT_EXPRESSION_PREFIX,
        BINARY_PREFIX, BINARY_OPERATOR,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_END,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_EXTERNS_SUFFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS_SUFFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX,
        COLLECTION_EXPRESSION_ELEMENTS_SUFFIX,
        COLLECTION_EXPRESSION_PREFIX,
        COMPILATION_UNIT_EXTERNS_SUFFIX,
        COMPILATION_UNIT_MEMBERS_PREFIX,
        COMPILATION_UNIT_USINGS_SUFFIX,
        EXPRESSION_STATEMENT_PREFIX,
        EXTERN_ALIAS,
        EXTERN_ALIAS_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_EXTERNS_SUFFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX,
        INTERPOLATED_STRING_PARTS_SUFFIX,
        INTERPOLATED_STRING_PREFIX,
        INTERPOLATION_ALIGNMENT_SUFFIX,
        INTERPOLATION_EXPRESSION_SUFFIX,
        INTERPOLATION_FORMAT_SUFFIX,
        INTERPOLATION_PREFIX,
        NULL_SAFE_EXPRESSION_EXPRESSION_SUFFIX,
        NULL_SAFE_EXPRESSION_PREFIX,
        PROPERTY_DECLARATION_EXPRESSION_BODY,
        PROPERTY_DECLARATION_INITIALIZER,
        PROPERTY_DECLARATION_INTERFACE_SPECIFIER_PREFIX,
        PROPERTY_DECLARATION_PREFIX,
        STATEMENT_EXPRESSION_PREFIX,
        NAMED_ARGUMENT_PREFIX,
        USING_DIRECTIVE_ALIAS_SUFFIX,
        USING_DIRECTIVE_GLOBAL_SUFFIX,
        USING_DIRECTIVE_PREFIX,
        USING_DIRECTIVE_STATIC,
        USING_DIRECTIVE_UNSAFE,
        NAMED_ARGUMENT_NAME_COLUMN_SUFFIX,
        CLASS_DECLARATION_PREFIX,
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_PREFIX,
        TYPE_CONSTRAINT_PREFIX,
        CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES,
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS,
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER,
        ALLOWS_CONSTRAINT_PREFIX,
        ALLOWS_CONSTRAINT_CLAUSE_PREFIX,
        REF_STRUCT_CONSTRAINT_PREFIX,
        CLASS_OR_STRUCT_CONSTRAINT_PREFIX,
        CONSTRUCTOR_CONSTRAINT_PREFIX,
        DEFAULT_CONSTRAINT_PREFIX,
        ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS,
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS,
        METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES,
        METHOD_DECLARATION_PREFIX,
        TYPE_PARAMETERS_CONSTRAINT_CLAUSE_PREFIX,
        TYPE_PARAMETERS_CONSTRAINT_PREFIX,
        USING_STATEMENT_PREFIX,
        USING_STATEMENT_RESOURCES,
        USING_STATEMENT_EXPRESSION,
        USING_STATEMENT_AWAIT_KEYWORD,
        ARGUMENT_NAME_COLUMN,
        KEYWORD_PREFIX,
        ARGUMENT_PREFIX,
        DECLARATION_EXPRESSION_SINGLE_VARIABLE_PREFIX,
        DECLARATION_EXPRESSION_PARENTHESIZED_VARIABLE_PREFIX,
        DECLARATION_EXPRESSION_PARENTHESIZED_VARIABLE_VARIABLES,
        DECLARATION_EXPRESSION_PREFIX,
        SINGLE_VARIABLE_PREFIX,
        PARENTHESIZED_VARIABLE_PREFIX,
        PARENTHESIZED_VARIABLE_DESIGNATION_VARIABLES,
        DECLARATION_EXPRESSION_VARIABLES,
        LAMBDA_PREFIX,
        DECLARATION_EXPRESSION_DESIGNATION_VARIABLES,
        SINGLE_VARIABLE_DESIGNATION_PREFIX,
        PARENTHESIZED_VARIABLE_DESIGNATION_PREFIX,
        DISCARD_VARIABLE_DESIGNATION_PREFIX,
        TUPLE_EXPRESSION_ARGUMENTS,
        TUPLE_EXPRESSION_PREFIX,
        METHOD_DECLARATION_PARAMETERS,
        CONSTRUCTOR_INITIALIZER_ARGUMENTS, CONSTRUCTOR_PREFIX, UNARY_PREFIX, UNARY_OPERATOR, CONSTRUCTOR_INITIALIZER_PREFIX, TUPLE_ELEMENT_PREFIX, TUPLE_TYPE_PREFIX, TUPLE_TYPE_ELEMENTS, NEW_CLASS_PREFIX, INITIALIZER_EXPRESSION_EXPRESSIONS, IMPLICIT_ELEMENT_ACCESS_PREFIX, IMPLICIT_ELEMENT_ACCESS_ARGUMENT_LIST, INITIALIZER_EXPRESSION_PREFIX, IS_PATTERN_PREFIX, SUBPATTERN_PATTERN, SUBPATTERN_PREFIX, IS_PATTERN_PATTERN, PROPERTY_PATTERN_CLAUSE_SUBPATTERNS, PROPERTY_PATTERN_CLAUSE_PREFIX, POSITIONAL_PATTERN_CLAUSE_SUBPATTERNS, POSITIONAL_PATTERN_CLAUSE_PREFIX, VAR_PATTERN_PREFIX, UNARY_PATTERN_PREFIX, TYPE_PATTERN_PREFIX, SLICE_PATTERN_PREFIX, RELATIONAL_PATTERN_OPERATOR, RELATIONAL_PATTERN_PREFIX, RECURSIVE_PATTERN_PREFIX, PARENTHESIZED_PATTERN_PREFIX, LIST_PATTERN_PATTERNS, LIST_PATTERN_PREFIX, DISCARD_PATTERN_PREFIX, CONSTANT_PATTERN_PREFIX, BINARY_PATTERN_OPERATOR, BINARY_PATTERN_PREFIX, YIELD_PREFIX, DEFAULT_EXPRESSION_TYPE_OPERATOR, SWITCH_EXPRESSION_ARM_EXPRESSION, SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION, SWITCH_EXPRESSION_ARM_PREFIX, CASE_PATTERN_SWITCH_LABEL_COLON_TOKEN, DEFAULT_SWITCH_LABEL_PREFIX, CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE, CASE_PATTERN_SWITCH_LABEL_PREFIX, LOCK_STATEMENT_STATEMENT, LOCK_STATEMENT_PREFIX, FIXED_STATEMENT_PREFIX, RANGE_EXPRESSION_START, RANGE_EXPRESSION_PREFIX, CHECKED_STATEMENT_PREFIX, UNSAFE_STATEMENT_PREFIX, SWITCH_SECTION_STATEMENTS, SWITCH_SECTION_PREFIX, SWITCH_STATEMENT_SECTIONS, SWITCH_STATEMENT_EXPRESSION, SWITCH_STATEMENT_PREFIX, SWITCH_EXPRESSION_ARMS, SWITCH_EXPRESSION_EXPRESSION, SWITCH_EXPRESSION_PREFIX, FOR_EACH_VARIABLE_LOOP_CONTROL_ITERABLE, FOR_EACH_VARIABLE_LOOP_CONTROL_VARIABLE, FOR_EACH_VARIABLE_LOOP_BODY, FOR_EACH_VARIABLE_LOOP_PREFIX, DESTRUCTOR_DECLARATION_PREFIX, DEFAULT_EXPRESSION_PREFIX, PARENTHESIZED_PATTERN_PATTERN, DEFAULT_SWITCH_LABEL_COLON_TOKEN, FOR_EACH_VARIABLE_LOOP_CONTROL_PREFIX;


    }
}
