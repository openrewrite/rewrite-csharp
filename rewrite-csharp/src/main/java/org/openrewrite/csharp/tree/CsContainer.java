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

public class CsContainer {
    @SuppressWarnings("LombokGetterMayBeUsed")
    public enum Location {
        ARRAY_RANK_SPECIFIER_SIZES(CsSpace.Location.ARRAY_RANK_SPECIFIER_SIZES, CsRightPadded.Location.ARRAY_RANK_SPECIFIER_SIZE),
        CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES(CsSpace.Location.CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, CsRightPadded.Location.CLASS_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES),
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS(CsSpace.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS, CsRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_CONSTRAINTS),
        ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS(CsSpace.Location.ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS, CsRightPadded.Location.ALLOWS_CONSTRAINT_CLAUSE_EXPRESSIONS),
        TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS(CsSpace.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS, CsRightPadded.Location.TYPE_PARAMETER_CONSTRAINT_CLAUSE_TYPE_PARAMETER_CONSTRAINTS),
        METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES(CsSpace.Location.METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES, CsRightPadded.Location.METHOD_DECLARATION_TYPE_PARAMETER_CONSTRAINT_CLAUSES),
        USING_STATEMENT_EXPRESSION(CsSpace.Location.USING_STATEMENT_EXPRESSION, CsRightPadded.Location.USING_STATEMENT_EXPRESSION);

        private final CsSpace.Location beforeLocation;
        private final CsRightPadded.Location elementLocation;

        Location(CsSpace.Location beforeLocation, CsRightPadded.Location elementLocation) {
            this.beforeLocation = beforeLocation;
            this.elementLocation = elementLocation;
        }

        public CsSpace.Location getBeforeLocation() {
            return beforeLocation;
        }

        public CsRightPadded.Location getElementLocation() {
            return elementLocation;
        }
    }
}
