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
public class AvoidUsingRedundantElseAnalyzerMA0071 extends RoslynRecipe {

    final String recipeId = "MA0071";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Meziantou.Analyzer";
    final String nugetPackageVersion = "2.0.298";

    final String displayName = "Avoid using redundant else (search)";
    final String description = "This is a reporting only recipe. The 'if' block contains a jump statement (break, continue, goto, return, throw, yield break). Using 'else' is redundant and needlessly maintains a higher nesting level.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "MA0071", "meziantou", "csharp", "dotnet", "c#").collect(toSet());

}
