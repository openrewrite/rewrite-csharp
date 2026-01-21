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

public class SA1103FixerSA1103 extends RoslynRecipe {
    @Getter
    final String recipeId = "SA1103";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "StyleCop.Analyzers";

    @Getter
    final String nugetPackageVersion = "1.1.118";

    @Getter
    final String displayName = "Query clauses should be on separate lines or all on one line";

    @Getter
    final String description = "The clauses within a C# query expression are not all placed on the same line, and each clause is not placed on its own line.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "SA1103", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
