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

public class DoNotCreateTasksWithoutPassingATaskSchedulerAnalyzerCA2008 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2008";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "10.0.102";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Do not create tasks without passing a TaskScheduler";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Do not create tasks unless you are using one of the overloads that takes a TaskScheduler. The default is to schedule on TaskScheduler.Current, which would lead to deadlocks. Either use TaskScheduler.Default to schedule on the thread pool, or explicitly pass TaskScheduler.Current to make your intentions clear.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA2008", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
