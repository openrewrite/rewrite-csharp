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

public class CodeMetricsAnalyzerCA1505 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1505";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "10.0.102";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Avoid unmaintainable code";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. The maintainability index is calculated by using the following metrics: lines of code, program volume, and cyclomatic complexity. Program volume is a measure of the difficulty of understanding of a symbol that is based on the number of operators and operands in the code. Cyclomatic complexity is a measure of the structural complexity of the type or method. A low maintainability index indicates that code is probably difficult to maintain and would be a good candidate to redesign.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA1505", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
