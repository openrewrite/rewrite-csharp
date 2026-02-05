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

package org.openrewrite.csharp.recipes.wpfanalyzers;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class RoutedEventEventDeclarationAnalyzerWPF0105 extends RoslynRecipe {

    final String recipeId = "WPF0105";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "WpfAnalyzers";
    final String nugetPackageVersion = "4.1.1";

    final String displayName = "Call RemoveHandler in remove (search)";
    final String description = "This is a reporting only recipe. Call RemoveHandler in remove.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "WPF0105", "wpfanalyzers", "csharp", "dotnet", "c#").collect(toSet());

}
