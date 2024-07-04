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
        ARRAY_RANK_SPECIFIER_SIZES(CsSpace.Location.ARRAY_RANK_SPECIFIER_SIZES, CsRightPadded.Location.ARRAY_RANK_SPECIFIER_SIZE);

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
