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

public class CSharpDisposeMethodsShouldCallBaseClassDisposeCA2215 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2215";
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
    final String displayName = "Dispose methods should call base class dispose";

    @Getter
    final String description = "A type that implements System.IDisposable inherits from a type that also implements IDisposable. The Dispose method of the inheriting type does not call the Dispose method of the parent type. To fix a violation of this rule, call base.Dispose in your Dispose method.";

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA2215", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
