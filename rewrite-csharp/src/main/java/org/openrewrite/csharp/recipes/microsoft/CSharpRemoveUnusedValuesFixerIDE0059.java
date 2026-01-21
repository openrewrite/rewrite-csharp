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

public class CSharpRemoveUnusedValuesFixerIDE0059 extends RoslynRecipe {
    @Getter
    final String recipeId = "IDE0059";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.CSharp.CodeStyle";

    @Getter
    final String nugetPackageVersion = "5.0.0";

    @Getter
    final String displayName = "Unnecessary assignment of a value";

    @Getter
    final String description = "Avoid unnecessary value assignments in your code, as these likely indicate redundant value computations. If the value computation is not redundant and you intend to retain the assignment, then change the assignment target to a local variable whose name starts with an underscore and is optionally followed by an integer, such as '_', '_1', '_2', etc. These are treated as special discard symbol names.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "IDE0059", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
