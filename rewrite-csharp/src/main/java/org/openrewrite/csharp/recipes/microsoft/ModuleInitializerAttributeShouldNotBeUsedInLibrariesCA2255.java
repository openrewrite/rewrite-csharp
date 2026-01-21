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

public class ModuleInitializerAttributeShouldNotBeUsedInLibrariesCA2255 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2255";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "10.0.102";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: The 'ModuleInitializer' attribute should not be used in libraries";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Module initializers are intended to be used by application code to ensure an application's components are initialized before the application code begins executing. If library code declares a method with the 'ModuleInitializerAttribute', it can interfere with application initialization and also lead to limitations in that application's trimming abilities. Instead of using methods marked with 'ModuleInitializerAttribute', the library should expose methods that can be used to initialize any components within the library and allow the application to invoke the method during application initialization.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA2255", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
