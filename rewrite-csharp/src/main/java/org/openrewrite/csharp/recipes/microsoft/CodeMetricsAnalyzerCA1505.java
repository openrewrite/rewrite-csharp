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

package org.openrewrite.csharp.recipes.microsoft;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class CodeMetricsAnalyzerCA1505 extends RoslynRecipe {

    final String recipeId = "CA1505";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Avoid unmaintainable code (search)";
    final String description = "This is a reporting only recipe. The maintainability index is calculated by using the following metrics: lines of code, program volume, and cyclomatic complexity. Program volume is a measure of the difficulty of understanding of a symbol that is based on the number of operators and operands in the code. Cyclomatic complexity is a measure of the structural complexity of the type or method. A low maintainability index indicates that code is probably difficult to maintain and would be a good candidate to redesign.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA1505", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
