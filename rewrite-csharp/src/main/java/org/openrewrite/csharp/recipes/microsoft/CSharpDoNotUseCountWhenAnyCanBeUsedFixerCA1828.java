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
public class CSharpDoNotUseCountWhenAnyCanBeUsedFixerCA1828 extends RoslynRecipe {

    final String recipeId = "CA1828";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.103";

    final String displayName = "Do not use CountAsync() or LongCountAsync() when AnyAsync() can be used";
    final String description = "For non-empty collections, CountAsync() and LongCountAsync() enumerate the entire sequence, while AnyAsync() stops at the first item or the first item that satisfies a condition.";
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA1828", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
