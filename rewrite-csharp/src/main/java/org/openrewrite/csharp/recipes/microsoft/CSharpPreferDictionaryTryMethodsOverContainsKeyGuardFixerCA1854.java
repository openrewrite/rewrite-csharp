/*
 * Copyright 2026 the original author or authors.
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
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class CSharpPreferDictionaryTryMethodsOverContainsKeyGuardFixerCA1854 extends RoslynRecipe {

    final String recipeId = "CA1854";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method";
    final String description = "Prefer a 'TryGetValue' call over a Dictionary indexer access guarded by a 'ContainsKey' check. 'ContainsKey' and the indexer both would lookup the key under the hood, so using 'TryGetValue' removes the extra lookup.";
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA1854", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
