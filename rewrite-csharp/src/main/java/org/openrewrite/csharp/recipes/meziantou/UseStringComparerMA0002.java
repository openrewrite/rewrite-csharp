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

package org.openrewrite.csharp.recipes.meziantou;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class UseStringComparerMA0002 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0002";
    }

    @Override
    public String getNugetPackageName() {
        return "Meziantou.Analyzer";
    }

    @Override
    public String getNugetPackageVersion() {
        return "2.0.221";
    }

    @Override
    public String getDisplayName() {
        return "IEqualityComparer<string> or IComparer<string> is missing";
    }

    @Override
    public String getDescription() {
        return "";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "MA0002", "meziantou", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
