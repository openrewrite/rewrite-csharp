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
import lombok.Getter;

public class DoNotUseInsecureXSLTScriptExecutionAnalyzerCA3076 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA3076";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Analysis: Insecure XSLT script processing";

    @Getter
    final String description = "This is a reporting only recipe. Providing an insecure XsltSettings instance and an insecure XmlResolver instance to XslCompiledTransform.Load method is potentially unsafe as it allows processing script within XSL, which on an untrusted XSL input may lead to malicious code execution. Either replace the insecure XsltSettings argument with XsltSettings.Default or an instance that has disabled document function and script execution, or replace the XmlResolver argument with null or an XmlSecureResolver instance. This message may be suppressed if the input is known to be from a trusted source and external resource resolution from locations that are not known in advance must be supported.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA3076", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
