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

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import lombok.Getter;

public class SealInternalTypesFixerCA1852 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA1852";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Seal internal types";

    @Getter
    final String description = "When a type is not accessible outside its assembly and has no subtypes within its containing assembly, it can be safely sealed. Sealing types can improve performance.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA1852", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
