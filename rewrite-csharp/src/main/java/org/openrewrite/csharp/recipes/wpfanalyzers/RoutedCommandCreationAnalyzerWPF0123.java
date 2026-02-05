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
public class RoutedCommandCreationAnalyzerWPF0123 extends RoslynRecipe {

    final String recipeId = "WPF0123";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "WpfAnalyzers";
    final String nugetPackageVersion = "4.1.1";

    final String displayName = "Backing field for a RoutedCommand should be static and readonly (search)";
    final String description = "This is a reporting only recipe. Backing field for a RoutedCommand should be static and readonly.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "WPF0123", "wpfanalyzers", "csharp", "dotnet", "c#").collect(toSet());

}
