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

public class ProvidePublicParameterlessSafeHandleConstructorCA1419 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1419";
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
        return "Provide a parameterless constructor that is as visible as the containing type for concrete types derived from 'System.Runtime.InteropServices.SafeHandle'";
    }

    @Override
    public String getDescription() {
        return "Providing a parameterless constructor that is as visible as the containing type for a type derived from 'System.Runtime.InteropServices.SafeHandle' enables better performance and usage with source-generated interop solutions.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA1419", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
