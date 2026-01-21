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

public class DoNotInstallRootCertCA5380 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA5380";
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
        return "Analysis: Do Not Add Certificates To Root Store";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. By default, the Trusted Root Certification Authorities certificate store is configured with a set of public CAs that has met the requirements of the Microsoft Root Certificate Program. Since all trusted root CAs can issue certificates for any domain, an attacker can pick a weak or coercible CA that you install by yourself to target for an attack - and a single vulnerable, malicious or coercible CA undermines the security of the entire system. To make matters worse, these attacks can go unnoticed quite easily.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA5380", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
