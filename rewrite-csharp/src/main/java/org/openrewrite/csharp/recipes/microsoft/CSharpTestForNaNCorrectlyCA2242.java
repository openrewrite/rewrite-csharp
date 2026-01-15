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

public class CSharpTestForNaNCorrectlyCA2242 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2242";
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
    final String displayName = "Test for NaN correctly";

    @Getter
    final String description = "This expression tests a value against Single.Nan or Double.Nan. Use Single.IsNan(Single) or Double.IsNan(Double) to test the value.";

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA2242", "microsoft", "csharp", "dotnet", "c#").collect(toSet());
    }
    }
