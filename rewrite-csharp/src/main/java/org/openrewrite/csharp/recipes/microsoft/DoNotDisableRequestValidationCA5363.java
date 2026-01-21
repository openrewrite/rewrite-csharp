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

public class DoNotDisableRequestValidationCA5363 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA5363";

    @Getter
    final boolean runCodeFixup = false;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "Analysis: Do Not Disable Request Validation";

    @Getter
    final String description = "This is a reporting only recipe. Request validation is a feature in ASP.NET that examines HTTP requests and determines whether they contain potentially dangerous content. This check adds protection from markup or code in the URL query string, cookies, or posted form values that might have been added for malicious purposes. So, it is generally desirable and should be left enabled for defense in depth.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA5363", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
