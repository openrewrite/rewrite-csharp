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

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@Getter
public class DoNotSetSwitchCA5361 extends RoslynRecipe {

    final String recipeId = "CA5361";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Do Not Disable SChannel Use of Strong Crypto (search)";
    final String description = "This is a reporting only recipe. Starting with the .NET Framework 4.6, the System.Net.ServicePointManager and System.Net.Security.SslStream classes are recommended to use new protocols. The old ones have protocol weaknesses and are not supported. Setting Switch.System.Net.DontEnableSchUseStrongCrypto with true will use the old weak crypto check and opt out of the protocol migration.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA5361", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
