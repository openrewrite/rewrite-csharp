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
public class OpenSslVersionOverrideEnvVarAnalyzerORNETX0017 extends RoslynRecipe {

    final String recipeId = "ORNETX0017";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "CLR_OPENSSL_VERSION_OVERRIDE is renamed to DOTNET_OPENSSL_VERSION_OVERRIDE in .NET 10 (search)";
    final String description = "This is a reporting only recipe. In .NET 10, the environment variable 'CLR_OPENSSL_VERSION_OVERRIDE' has been renamed to 'DOTNET_OPENSSL_VERSION_OVERRIDE' to align with .NET naming conventions. Applications that read or set this variable must use the new name for the OpenSSL version preference to take effect.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "ORNETX0017", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
