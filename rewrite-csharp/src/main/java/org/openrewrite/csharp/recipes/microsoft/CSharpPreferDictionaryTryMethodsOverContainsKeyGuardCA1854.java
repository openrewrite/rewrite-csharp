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

public class CSharpPreferDictionaryTryMethodsOverContainsKeyGuardCA1854 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1854";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Getter
    final String displayName = "Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method";

    @Getter
    final String description = "Prefer a 'TryGetValue' call over a Dictionary indexer access guarded by a 'ContainsKey' check. 'ContainsKey' and the indexer both would lookup the key under the hood, so using 'TryGetValue' removes the extra lookup.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "CA1854", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
