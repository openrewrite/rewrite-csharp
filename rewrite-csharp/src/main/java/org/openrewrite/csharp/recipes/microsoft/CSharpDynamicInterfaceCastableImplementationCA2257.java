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

public class CSharpDynamicInterfaceCastableImplementationCA2257 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2257";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Override
    public String getDisplayName() {
        return "Members defined on an interface with the 'DynamicInterfaceCastableImplementationAttribute' should be 'static'";
    }

    @Override
    public String getDescription() {
        return "Since a type that implements 'IDynamicInterfaceCastable' may not implement a dynamic interface in metadata, calls to an instance interface member that is not an explicit implementation defined on this type are likely to fail at runtime. Mark new interface members 'static' to avoid runtime errors.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA2257", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
