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

public class CSharpPreventNumericIntPtrUIntPtrBehavioralChangesCA2020 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2020";
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
        return "Analysis: Prevent behavioral change";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Some built-in operators added in .NET 7 behave differently when overflowing than did the corresponding user-defined operators in .NET 6 and earlier versions. Some operators that previously threw in an unchecked context now don't throw unless wrapped within a checked context. Also, some operators that did not previously throw in a checked context now throw unless wrapped in an unchecked context.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA2020", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
