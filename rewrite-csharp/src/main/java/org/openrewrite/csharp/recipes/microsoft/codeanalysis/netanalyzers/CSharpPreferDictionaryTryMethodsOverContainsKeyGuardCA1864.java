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
package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferDictionaryTryMethodsOverContainsKeyGuardCA1864 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1864";
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
        return "Prefer the 'IDictionary.TryAdd(TKey, TValue)' method";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Prefer a 'TryAdd' call over an 'Add' call guarded by a 'ContainsKey' check. 'TryAdd' behaves the same as 'Add', except that when the specified key already exists, it returns 'false' instead of throwing an exception.";
    }
}
