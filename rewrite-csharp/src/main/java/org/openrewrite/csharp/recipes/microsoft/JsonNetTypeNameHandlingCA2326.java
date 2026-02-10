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
public class JsonNetTypeNameHandlingCA2326 extends RoslynRecipe {

    final String recipeId = "CA2326";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.103";

    final String displayName = "Do not use TypeNameHandling values other than None (search)";
    final String description = "This is a reporting only recipe. Deserializing JSON when using a TypeNameHandling value other than None can be insecure.  If you need to instead detect Json.NET deserialization when a SerializationBinder isn't specified, then disable rule CA2326, and enable rules CA2327, CA2328, CA2329, and CA2330.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA2326", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
