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

package org.openrewrite.csharp.recipes.meziantou;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class SimplifyStringCreateWhenAllParametersAreCultureInvariantFixerMA0185 extends RoslynRecipe {

    final String recipeId = "MA0185";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "Meziantou.Analyzer";
    final String nugetPackageVersion = "2.0.298";

    final String displayName = "Simplify string.Create when all parameters are culture invariant";
    final String description = "";
    final Set<String> tags = Stream.of("roslyn", "codefix", "MA0185", "meziantou", "csharp", "dotnet", "c#").collect(toSet());

}
