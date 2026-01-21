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

public class CSharpAvoidUsingCrefTagsWithAPrefixAnalyzerCA1200 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1200";
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
        return "Analysis: Avoid using cref tags with a prefix";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Use of cref tags with prefixes should be avoided, since it prevents the compiler from verifying references and the IDE from updating references during refactorings. It is permissible to suppress this error at a single documentation site if the cref must use a prefix because the type being mentioned is not findable by the compiler. For example, if a cref is mentioning a special attribute in the full framework but you're in a file that compiles against the portable framework, or if you want to reference a type at higher layer of Roslyn, you should suppress the error. You should not suppress the error just because you want to take a shortcut and avoid using the full syntax.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA1200", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
