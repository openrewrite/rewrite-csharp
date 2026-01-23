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
public class DoNotUseAccountSASCA5375 extends RoslynRecipe {

    final String recipeId = "CA5375";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Do Not Use Account Shared Access Signature (search)";
    final String description = "This is a reporting only recipe. Shared Access Signatures(SAS) are a vital part of the security model for any application using Azure Storage, they should provide limited and safe permissions to your storage account to clients that don't have the account key. All of the operations available via a service SAS are also available via an account SAS, that is, account SAS is too powerful. So it is recommended to use Service SAS to delegate access more carefully.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA5375", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
