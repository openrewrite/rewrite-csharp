/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Moderne Source Available License (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://docs.moderne.io/licensing/moderne-source-available-license
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/*
 * -------------------THIS FILE IS AUTO GENERATED--------------------------
 * Changes to this file may cause incorrect behavior and will be lost if
 * the code is regenerated.
*/

package org.openrewrite.csharp.recipes.stylecop.analyzers;

import org.openrewrite.csharp.RoslynRecipe;

public class PropertySummaryDocumentationSA1624 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "SA1624";
    }

    @Override
    public String getNugetPackageName() {
        return "StyleCop.Analyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "1.1.118";
    }

    @Override
    public String getDisplayName() {
        return "Property summary documentation should omit accessor with restricted access";
    }

    @Override
    public String getDescription() {
        return "The documentation text within a C# property’s <summary> tag takes into account all of the accessors within the property, but one of the accessors has limited access.";
    }
}
