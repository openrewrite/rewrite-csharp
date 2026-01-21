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

package org.openrewrite.csharp.recipes.stylecop;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import lombok.Getter;

public class TokenSpacingFixerSA1113 extends RoslynRecipe {
    @Getter
    final String recipeId = "SA1113";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "StyleCop.Analyzers";

    @Getter
    final String nugetPackageVersion = "1.1.118";

    @Getter
    final String displayName = "Comma should be on the same line as previous parameter";

    @Getter
    final String description = "A comma between two parameters in a call to a C# method or indexer, or in the declaration of a method or indexer, is not placed on the same line as the previous parameter.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "SA1113", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
