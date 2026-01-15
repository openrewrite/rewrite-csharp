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

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

public class OverrideMethodsOnComparableTypesCA1036 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1036";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Getter
    final String displayName = "Override methods on comparable types";

    @Getter
    final String description = "A public or protected type implements the System.IComparable interface. It does not override Object.Equals nor does it overload the language-specific operator for equality, inequality, less than, less than or equal, greater than or greater than or equal.";

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA1036", "microsoft", "csharp", "dotnet", "c#").collect(toSet());
    }
    }
