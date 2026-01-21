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

public class CSharpDoNotCompareSpanToNullFixerCA2265 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2265";
    }

    @Override
    public boolean getRunCodeFixup() {
        return true;
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
        return "Do not compare Span<T> to 'null' or 'default'";
    }

    @Override
    public String getDescription() {
        return "Comparing a span to 'null' or 'default' might not do what you intended. 'default' and the 'null' literal are implicitly converted to 'Span<T>.Empty'. Remove the redundant comparison or make the code more explicit by using 'IsEmpty'.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "codefix", "CA2265", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
