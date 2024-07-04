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
        BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS_SUFFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX,
        BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX,
        COLLECTION_EXPRESSION_ELEMENTS_SUFFIX,
        COLLECTION_EXPRESSION_PREFIX,
        COMPILATION_UNIT_MEMBERS_PREFIX,
        COMPILATION_UNIT_USINGS_SUFFIX,
        EXPRESSION_STATEMENT_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX,
        FILE_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX,
        NULL_SAFE_EXPRESSION_EXPRESSION_SUFFIX,
        NULL_SAFE_EXPRESSION_PREFIX,
        STATEMENT_EXPRESSION_PREFIX,
        USING_DIRECTIVE_ALIAS_SUFFIX,
        USING_DIRECTIVE_GLOBAL_SUFFIX,
        USING_DIRECTIVE_PREFIX,
        USING_DIRECTIVE_STATIC,
        USING_DIRECTIVE_UNSAFE,
    }
}
