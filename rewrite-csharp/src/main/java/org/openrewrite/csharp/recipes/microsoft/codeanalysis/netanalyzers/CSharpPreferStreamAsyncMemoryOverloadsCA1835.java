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

import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferStreamAsyncMemoryOverloadsCA1835 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1835";
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
    public String getDisplayName() {
        return "Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'";
    }

    @Override
    public String getDescription() {
        return "'Stream' has a 'ReadAsync' overload that takes a 'Memory<Byte>' as the first argument, and a 'WriteAsync' overload that takes a 'ReadOnlyMemory<Byte>' as the first argument. Prefer calling the memory based overloads, which are more efficient.";
    }
}
