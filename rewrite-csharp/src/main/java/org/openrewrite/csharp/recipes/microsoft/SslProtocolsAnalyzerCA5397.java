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

public class SslProtocolsAnalyzerCA5397 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA5397";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Analysis: Do not use deprecated SslProtocols values";

    @Getter
    final String description = "This is a reporting only recipe. Older protocol versions of Transport Layer Security (TLS) are less secure than TLS 1.2 and TLS 1.3, and are more likely to have new vulnerabilities. Avoid older protocol versions to minimize risk.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA5397", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
