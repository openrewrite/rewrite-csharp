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

package org.openrewrite.csharp.recipes.microsoft;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class DoNotDirectlyAwaitATaskAnalyzerCA2007 extends RoslynRecipe {

    final String recipeId = "CA2007";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Consider calling ConfigureAwait on the awaited task (search)";
    final String description = "This is a reporting only recipe. When an asynchronous method awaits a Task directly, continuation occurs in the same thread that created the task. Consider calling Task.ConfigureAwait(Boolean) to signal your intention for continuation. Call ConfigureAwait(false) on the task to schedule continuations to the thread pool, thereby avoiding a deadlock on the UI thread. Passing false is a good option for app-independent libraries. Calling ConfigureAwait(true) on the task has the same behavior as not explicitly calling ConfigureAwait. By explicitly calling this method, you're letting readers know you intentionally want to perform the continuation on the original synchronization context.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA2007", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
