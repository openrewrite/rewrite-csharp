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
public class BackgroundServiceExecuteAsyncAnalyzerORNETX0018 extends RoslynRecipe {

    final String recipeId = "ORNETX0018";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "BackgroundService.ExecuteAsync runs entirely on a background thread in .NET 10 (search)";
    final String description = "This is a reporting only recipe. In .NET 10, BackgroundService.ExecuteAsync runs entirely on a background thread. Previously, the synchronous portion before the first await ran on the main thread and blocked other services from starting. Code that relies on this startup-blocking behavior should be moved to the constructor, StartAsync override, or use IHostedLifecycleService for more granular control.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "ORNETX0018", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
