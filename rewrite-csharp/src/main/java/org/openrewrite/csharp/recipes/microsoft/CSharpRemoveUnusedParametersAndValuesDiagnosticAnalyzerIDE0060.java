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

public class CSharpRemoveUnusedParametersAndValuesDiagnosticAnalyzerIDE0060 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "IDE0060";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.CSharp.CodeStyle";
    }

    @Override
    public String getNugetPackageVersion() {
        return "5.0.0";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Remove unused parameter";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Avoid unused parameters in your code. If the parameter cannot be removed, then change its name so it starts with an underscore and is optionally followed by an integer, such as '_', '_1', '_2', etc. These are treated as special discard symbol names.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "IDE0060", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
