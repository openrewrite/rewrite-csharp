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

package org.openrewrite.csharp.recipes.meziantou;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import lombok.Getter;

public class UseAttributeIsDefinedAnalyzerMA0179 extends RoslynRecipe {
    @Getter
    final String recipeId = "MA0179";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Meziantou.Analyzer";

    @Getter
    final String nugetPackageVersion = "2.0.285";

    @Getter
    final String displayName = "Analysis: Use Attribute.IsDefined instead of GetCustomAttribute(s)";

    @Getter
    final String description = "This is a reporting only recipe. Detects inefficient attribute existence checks that can be replaced with Attribute.IsDefined for better performance.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "MA0179", "meziantou", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
