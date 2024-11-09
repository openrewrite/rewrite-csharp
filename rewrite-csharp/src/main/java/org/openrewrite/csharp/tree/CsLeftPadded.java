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

public class CsLeftPadded {
    public enum Location {
        ASSIGNMENT_OPERATION_OPERATOR(CsSpace.Location.ASSIGNMENT_OPERATION_OPERATOR),
        BINARY_OPERATOR(CsSpace.Location.BINARY_OPERATOR),
        EXTERN_ALIAS_IDENTIFIER(CsSpace.Location.EXTERN_ALIAS),
        PROPERTY_DECLARATION_EXPRESSION_BODY(CsSpace.Location.PROPERTY_DECLARATION_EXPRESSION_BODY),
        PROPERTY_DECLARATION_INITIALIZER(CsSpace.Location.PROPERTY_DECLARATION_INITIALIZER),
        USING_DIRECTIVE_STATIC(CsSpace.Location.USING_DIRECTIVE_STATIC),
        USING_DIRECTIVE_UNSAFE(CsSpace.Location.USING_DIRECTIVE_UNSAFE),
        UNARY_OPERATOR(CsSpace.Location.UNARY_OPERATOR),
        SWITCH_EXPRESSION_ARM_EXPRESSION(CsSpace.Location.SWITCH_EXPRESSION_ARM_EXPRESSION),
        SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION(CsSpace.Location.SWITCH_EXPRESSION_ARM_WHEN_EXPRESSION),
        CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE(CsSpace.Location.CASE_PATTERN_SWITCH_LABEL_WHEN_CLAUSE),
        IS_PATTERN_PATTERN(CsSpace.Location.IS_PATTERN_PATTERN),
        BINARY_PATTERN_OPERATOR(CsSpace.Location.BINARY_PATTERN_OPERATOR),
        RELATIONAL_PATTERN_OPERATOR(CsSpace.Location.RELATIONAL_PATTERN_OPERATOR),
        SUBPATTERN_PATTERN(CsSpace.Location.SUBPATTERN_PATTERN);

        private CsSpace.Location beforeLocation;

        private Location(CsSpace.Location beforeLocation) {
            this.beforeLocation = beforeLocation;
        }

        public CsSpace.Location getBeforeLocation() {
            return beforeLocation;
        }
    }
}
