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

public class DoNotDirectlyAwaitATaskFixerCA2007 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA2007";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Consider calling ConfigureAwait on the awaited task";

    @Getter
    final String description = "When an asynchronous method awaits a Task directly, continuation occurs in the same thread that created the task. Consider calling Task.ConfigureAwait(Boolean) to signal your intention for continuation. Call ConfigureAwait(false) on the task to schedule continuations to the thread pool, thereby avoiding a deadlock on the UI thread. Passing false is a good option for app-independent libraries. Calling ConfigureAwait(true) on the task has the same behavior as not explicitly calling ConfigureAwait. By explicitly calling this method, you're letting readers know you intentionally want to perform the continuation on the original synchronization context.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA2007", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
