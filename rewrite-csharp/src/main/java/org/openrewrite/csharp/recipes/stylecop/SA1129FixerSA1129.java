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

package org.openrewrite.csharp.recipes.stylecop;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class SA1129FixerSA1129 extends RoslynRecipe {
    @Getter
    final String recipeId = "SA1129";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "StyleCop.Analyzers";

    @Getter
    final String nugetPackageVersion = "1.1.118";

    @Getter
    final String displayName = "Do not use default value type constructor";

    @Getter
    final String description = "When creating a new instance of a value type T, the syntax 'default(T)' is functionally equivalent to the syntax 'new T()'. To avoid confusion regarding the behavior of the resulting instance, the first form is preferred.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "SA1129", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
