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

public class SA1132SA1132 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "SA1132";
    }

    @Override
    public String getNugetPackageName() {
        return "StyleCop.Analyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "1.1.118";
    }

    @Getter
    final String displayName = "Do not combine fields";

    @Getter
    final String description = "Each field should be declared on its own line, in order to clearly see each field of a type and allow for proper documentation of the behavior of each field.";

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "SA1132", "stylecop", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
