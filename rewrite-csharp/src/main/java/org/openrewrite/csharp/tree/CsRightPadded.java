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

public interface CsRightPadded {
    enum Location {
        ARRAY_RANK_SPECIFIER_SIZE(CsSpace.Location.ARRAY_RANK_SPECIFIER_SIZE_SUFFIX),
        ATTRIBUTE_LIST_ATTRIBUTES(CsSpace.Location.ATTRIBUTE_LIST_ATTRIBUTES_SUFFIX),
        ATTRIBUTE_LIST_TARGET(CsSpace.Location.ATTRIBUTE_LIST_TARGET_SUFFIX),
        BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS(CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_MEMBERS_SUFFIX),
        BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME(CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX),
        BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS(CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX),
        COLLECTION_EXPRESSION_ELEMENTS(CsSpace.Location.COLLECTION_EXPRESSION_ELEMENTS_SUFFIX),
        COMPILATION_UNIT_MEMBERS(CsSpace.Location.COMPILATION_UNIT_MEMBERS_PREFIX),
        COMPILATION_UNIT_USINGS(CsSpace.Location.COMPILATION_UNIT_USINGS_SUFFIX),
        FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS(CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_MEMBERS_PREFIX),
        FILE_SCOPE_NAMESPACE_DECLARATION_NAME(CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_NAME_PREFIX),
        FILE_SCOPE_NAMESPACE_DECLARATION_USINGS(CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_USINGS_SUFFIX),
        NULL_SAFE_EXPRESSION_EXPRESSION(CsSpace.Location.NULL_SAFE_EXPRESSION_EXPRESSION_SUFFIX),
        PROPERTY_DECLARATION_INTERFACE_SPECIFIER(CsSpace.Location.PROPERTY_DECLARATION_INTERFACE_SPECIFIER_PREFIX),
        USING_DIRECTIVE_ALIAS(CsSpace.Location.USING_DIRECTIVE_ALIAS_SUFFIX),
        USING_DIRECTIVE_GLOBAL(CsSpace.Location.USING_DIRECTIVE_GLOBAL_SUFFIX),
	    ;

        private final CsSpace.Location afterLocation;

        Location(CsSpace.Location afterLocation) {
            this.afterLocation = afterLocation;
        }

        public CsSpace.Location getAfterLocation() {
            return afterLocation;
        }
    }
}
