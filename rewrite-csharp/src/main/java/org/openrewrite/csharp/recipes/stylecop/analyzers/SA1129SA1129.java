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

public class SA1129SA1129 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "SA1129";
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
        return "Do not use default value type constructor";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "When creating a new instance of a value type T, the syntax 'default(T)' is functionally equivalent to the syntax 'new T()'. To avoid confusion regarding the behavior of the resulting instance, the first form is preferred.";
    }
}
