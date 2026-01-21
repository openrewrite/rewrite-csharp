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

package org.openrewrite.csharp.recipes.roslynator;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import lombok.Getter;

public class UsePatternMatchingInsteadOfAsAndNullCheckFixerRCS1221 extends RoslynRecipe {
    @Getter
    final String recipeId = "RCS1221";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "Roslynator.Analyzers";

    @Getter
    final String nugetPackageVersion = "4.15.0";

    @Getter
    final String displayName = "Use pattern matching instead of combination of 'as' operator and null check";

    @Getter
    final String description = "";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "RCS1221", "roslynator", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
