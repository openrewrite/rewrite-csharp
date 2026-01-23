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

package org.openrewrite.csharp.recipes.microsoft;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@Getter
public class RecommendCaseInsensitiveStringComparisonAnalyzerCA1862 extends RoslynRecipe {

    final String recipeId = "CA1862";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Use the 'StringComparison' method overloads to perform case-insensitive string comparisons (search)";
    final String description = "This is a reporting only recipe. Avoid calling 'ToLower', 'ToUpper', 'ToLowerInvariant' and 'ToUpperInvariant' to perform case-insensitive string comparisons because they lead to an allocation. Instead, prefer calling the method overloads of 'Contains', 'IndexOf' and 'StartsWith' that take a 'StringComparison' enum value to perform case-insensitive comparisons. Switching to using an overload that takes a 'StringComparison' might cause subtle changes in behavior, so it's important to conduct thorough testing after applying the suggestion. Additionally, if a culturally sensitive comparison is not required, consider using 'StringComparison.OrdinalIgnoreCase'.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA1862", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
