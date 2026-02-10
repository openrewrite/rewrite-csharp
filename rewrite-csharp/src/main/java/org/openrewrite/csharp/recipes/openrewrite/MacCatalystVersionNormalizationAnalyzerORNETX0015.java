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
public class MacCatalystVersionNormalizationAnalyzerORNETX0015 extends RoslynRecipe {

    final String recipeId = "ORNETX0015";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "IsOSPlatformVersionAtLeast has behavioral changes in .NET 10 due to MacCatalyst version normalization (search)";
    final String description = "This is a reporting only recipe. In .NET 10, OperatingSystem.IsOSPlatformVersionAtLeast normalizes the MacCatalyst build component to 0 if undefined (-1), and the revision component is always set to -1. This may change the result of version checks on MacCatalyst that previously relied on inconsistent normalization.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "ORNETX0015", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
