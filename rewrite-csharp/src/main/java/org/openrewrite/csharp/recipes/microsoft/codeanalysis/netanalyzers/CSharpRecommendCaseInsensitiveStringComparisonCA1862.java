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

package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpRecommendCaseInsensitiveStringComparisonCA1862 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1862";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Use the 'StringComparison' method overloads to perform case-insensitive string comparisons";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Avoid calling 'ToLower', 'ToUpper', 'ToLowerInvariant' and 'ToUpperInvariant' to perform case-insensitive string comparisons because they lead to an allocation. Instead, prefer calling the method overloads of 'Contains', 'IndexOf' and 'StartsWith' that take a 'StringComparison' enum value to perform case-insensitive comparisons. Switching to using an overload that takes a 'StringComparison' might cause subtle changes in behavior, so it's important to conduct thorough testing after applying the suggestion. Additionally, if a culturally sensitive comparison is not required, consider using 'StringComparison.OrdinalIgnoreCase'.";
    }
}
