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

public class OverloadOperatorEqualsOnOverridingValueTypeEqualsAnalyzerCA2231 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA2231";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Analysis: Overload operator equals on overriding value type Equals";

    @Getter
    final String description = "This is a reporting only recipe. In most programming languages there is no default implementation of the equality operator (==) for value types. If your programming language supports operator overloads, you should consider implementing the equality operator. Its behavior should be identical to that of Equals.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA2231", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
