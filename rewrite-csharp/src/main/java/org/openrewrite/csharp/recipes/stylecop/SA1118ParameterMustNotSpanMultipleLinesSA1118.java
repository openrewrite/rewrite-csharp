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

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class SA1118ParameterMustNotSpanMultipleLinesSA1118 extends RoslynRecipe {
    @Getter
    final String recipeId = "SA1118";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "StyleCop.Analyzers";

    @Getter
    final String nugetPackageVersion = "1.1.118";

    @Getter
    final String displayName = "Analysis: Parameter should not span multiple lines";

    @Getter
    final String description = "This is a reporting only recipe. A parameter to a C# method/indexer/attribute/array, other than the first parameter, spans across multiple lines. If the parameter is short, place the entire parameter on a single line. Otherwise, save the contents of the parameter in a temporary variable and pass the temporary variable as a parameter.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "SA1118", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
