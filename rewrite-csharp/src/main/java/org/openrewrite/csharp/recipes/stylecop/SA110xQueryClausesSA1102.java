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

public class SA110xQueryClausesSA1102 extends RoslynRecipe {
    @Getter
    final String recipeId = "SA1102";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "StyleCop.Analyzers";

    @Getter
    final String nugetPackageVersion = "1.1.118";

    @Getter
    final String displayName = "Analysis: Query clause should follow previous clause";

    @Getter
    final String description = "This is a reporting only recipe. A C# query clause does not begin on the same line as the previous clause, or on the next line.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "SA1102", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
