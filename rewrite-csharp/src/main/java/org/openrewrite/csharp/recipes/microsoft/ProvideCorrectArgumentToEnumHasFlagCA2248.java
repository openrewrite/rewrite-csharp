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
public class ProvideCorrectArgumentToEnumHasFlagCA2248 extends RoslynRecipe {

    final String recipeId = "CA2248";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Provide correct 'enum' argument to 'Enum.HasFlag' (search)";
    final String description = "This is a reporting only recipe. 'Enum.HasFlag' method expects the 'enum' argument to be of the same 'enum' type as the instance on which the method is invoked and that this 'enum' is marked with 'System.FlagsAttribute'. If these are different 'enum' types, an unhandled exception will be thrown at runtime. If the 'enum' type is not marked with 'System.FlagsAttribute' the call will always return 'false' at runtime.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA2248", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
