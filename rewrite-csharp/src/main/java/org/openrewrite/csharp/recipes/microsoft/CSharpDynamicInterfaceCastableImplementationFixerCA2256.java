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

public class CSharpDynamicInterfaceCastableImplementationFixerCA2256 extends RoslynRecipe {
    @Getter
    final String recipeId = "CA2256";

    @Getter
    final boolean runCodeFixup = true;

    @Getter
    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";

    @Getter
    final String nugetPackageVersion = "10.0.102";

    @Getter
    final String displayName = "All members declared in parent interfaces must have an implementation in a DynamicInterfaceCastableImplementation-attributed interface";

    @Getter
    final String description = "Types attributed with 'DynamicInterfaceCastableImplementationAttribute' act as an interface implementation for a type that implements the 'IDynamicInterfaceCastable' type. As a result, it must provide an implementation of all of the members defined in the inherited interfaces, because the type that implements 'IDynamicInterfaceCastable' will not provide them otherwise.";

    @Getter
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA2256", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());

}
