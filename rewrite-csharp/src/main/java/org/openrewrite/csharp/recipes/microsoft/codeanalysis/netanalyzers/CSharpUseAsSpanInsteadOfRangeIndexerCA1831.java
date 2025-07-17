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

public class CSharpUseAsSpanInsteadOfRangeIndexerCA1831 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1831";
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
        return "Use AsSpan or AsMemory instead of Range-based indexers when appropriate";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "The Range-based indexer on string values produces a copy of requested portion of the string. This copy is usually unnecessary when it is implicitly used as a ReadOnlySpan or ReadOnlyMemory value. Use the AsSpan method to avoid the unnecessary copy.";
    }
}
