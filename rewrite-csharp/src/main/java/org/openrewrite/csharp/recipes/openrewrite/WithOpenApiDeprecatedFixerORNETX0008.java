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

package org.openrewrite.csharp.recipes.openrewrite;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class WithOpenApiDeprecatedFixerORNETX0008 extends RoslynRecipe {

    final String recipeId = "ORNETX0008";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "WithOpenApi is deprecated in .NET 10";
    final String description = "The WithOpenApi extension method is deprecated in .NET 10 Preview 7. Its functionality has been replaced by the built-in OpenAPI document generation pipeline. For Microsoft.AspNetCore.OpenApi, use AddOpenApiOperationTransformer. For Swashbuckle, use IOperationFilter. For NSwag, use IOperationProcessor.";
    final Set<String> tags = Stream.of("roslyn", "codefix", "ORNETX0008", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
