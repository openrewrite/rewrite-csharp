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

public class DoNotUseInsecureXSLTScriptExecutionAnalyzerCA3076 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA3076";
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
        return "Analysis: Insecure XSLT script processing";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Providing an insecure XsltSettings instance and an insecure XmlResolver instance to XslCompiledTransform.Load method is potentially unsafe as it allows processing script within XSL, which on an untrusted XSL input may lead to malicious code execution. Either replace the insecure XsltSettings argument with XsltSettings.Default or an instance that has disabled document function and script execution, or replace the XmlResolver argument with null or an XmlSecureResolver instance. This message may be suppressed if the input is known to be from a trusted source and external resource resolution from locations that are not known in advance must be supported.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA3076", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
