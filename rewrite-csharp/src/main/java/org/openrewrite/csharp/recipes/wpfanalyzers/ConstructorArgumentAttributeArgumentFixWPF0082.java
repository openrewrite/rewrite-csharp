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

package org.openrewrite.csharp.recipes.wpfanalyzers;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class ConstructorArgumentAttributeArgumentFixWPF0082 extends RoslynRecipe {
    @Getter
    final String recipeId = "WPF0082";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "WpfAnalyzers";

    @Getter
    final String nugetPackageVersion = "4.1.1";

    @Getter
    final String displayName = "[ConstructorArgument] must match";

    @Getter
    final String description = "[ConstructorArgument] must match the name of the constructor parameter.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "WPF0082", "wpfanalyzers", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
