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

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SA1135SA1135 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "SA1135";
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
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Using directives should be qualified";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "All using directives should be qualified.";
    }
}
