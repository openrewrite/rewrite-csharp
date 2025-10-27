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

package org.openrewrite.csharp.recipes.wpfanalyzers;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class MakeFieldStaticReadonlyFixWPF0123 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "WPF0123";
    }

    @Override
    public String getNugetPackageName() {
        return "WpfAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "4.1.1";
    }

    @Override
    public String getDisplayName() {
        return "Backing field for a RoutedCommand should be static and readonly";
    }

    @Override
    public String getDescription() {
        return "Backing field for a RoutedCommand should be static and readonly.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "WPF0123", "wpfanalyzers", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
