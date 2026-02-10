/*
 * Copyright 2026 the original author or authors.
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

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class UriParametersShouldNotBeStringsFixerCA1054 extends RoslynRecipe {

    final String recipeId = "CA1054";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.103";

    final String displayName = "URI-like parameters should not be strings";
    final String description = "This rule assumes that the parameter represents a Uniform Resource Identifier (URI). A string representation or a URI is prone to parsing and encoding errors, and can lead to security vulnerabilities. 'System.Uri' class provides these services in a safe and secure manner.";
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA1054", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
