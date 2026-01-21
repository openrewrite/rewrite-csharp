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
import lombok.Getter;

public class UseAutoValidateAntiforgeryTokenCA5395 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA5395";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Analysis: Miss HttpVerb attribute for action methods";

    @Getter
    final String description = "This is a reporting only recipe. All the methods that create, edit, delete, or otherwise modify data do so in the [HttpPost] overload of the method, which needs to be protected with the anti forgery attribute from request forgery. Performing a GET operation should be a safe operation that has no side effects and doesn't modify your persisted data.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA5395", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
