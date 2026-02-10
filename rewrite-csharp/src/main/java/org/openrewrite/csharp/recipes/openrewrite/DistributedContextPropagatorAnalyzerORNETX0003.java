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
public class DistributedContextPropagatorAnalyzerORNETX0003 extends RoslynRecipe {

    final String recipeId = "ORNETX0003";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "Default trace context propagator changed to W3C in .NET 10 (search)";
    final String description = "This is a reporting only recipe. In .NET 10, DistributedContextPropagator.CreateDefaultPropagator() and DistributedContextPropagator.Current return the W3C propagator instead of the legacy propagator. The W3C propagator uses the 'baggage' header instead of 'Correlation-Context' and enforces W3C-compliant encoding with stricter formatting for trace parent, trace state, and baggage keys/values. To retain legacy behavior, use: DistributedContextPropagator.Current = DistributedContextPropagator.CreatePreW3CPropagator();";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "ORNETX0003", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
