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

package org.openrewrite.csharp.recipes.stylecop;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class SA1627DocumentationTextMustNotBeEmptySA1627 extends RoslynRecipe {

    final String recipeId = "SA1627";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "StyleCop.Analyzers";
    final String nugetPackageVersion = "1.1.118";

    final String displayName = "Documentation text should not be empty (search)";
    final String description = "This is a reporting only recipe. The XML header documentation for a C# code element contains an empty tag.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "SA1627", "stylecop", "csharp", "dotnet", "c#").collect(toSet());

}
