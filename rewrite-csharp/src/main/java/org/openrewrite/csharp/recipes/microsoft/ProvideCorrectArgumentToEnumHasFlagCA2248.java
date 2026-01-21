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

public class ProvideCorrectArgumentToEnumHasFlagCA2248 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2248";
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
        return "Analysis: Provide correct 'enum' argument to 'Enum.HasFlag'";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. 'Enum.HasFlag' method expects the 'enum' argument to be of the same 'enum' type as the instance on which the method is invoked and that this 'enum' is marked with 'System.FlagsAttribute'. If these are different 'enum' types, an unhandled exception will be thrown at runtime. If the 'enum' type is not marked with 'System.FlagsAttribute' the call will always return 'false' at runtime.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA2248", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
