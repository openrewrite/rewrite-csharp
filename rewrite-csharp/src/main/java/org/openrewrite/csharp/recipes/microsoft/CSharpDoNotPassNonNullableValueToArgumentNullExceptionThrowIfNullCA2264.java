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
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class CSharpDoNotPassNonNullableValueToArgumentNullExceptionThrowIfNullCA2264 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2264";
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
    final String displayName = "Do not pass a non-nullable value to 'ArgumentNullException.ThrowIfNull'";

    @Getter
    final String description = "'ArgumentNullException.ThrowIfNull' throws when the passed argument is 'null'. Certain constructs like non-nullable structs, 'nameof()' and 'new' expressions are known to never be null, so 'ArgumentNullException.ThrowIfNull' will never throw.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "CA2264", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
