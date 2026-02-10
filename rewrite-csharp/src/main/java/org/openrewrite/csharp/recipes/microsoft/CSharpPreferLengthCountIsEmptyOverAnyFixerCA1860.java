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
public class CSharpPreferLengthCountIsEmptyOverAnyFixerCA1860 extends RoslynRecipe {

    final String recipeId = "CA1860";
    final boolean runCodeFixup = true;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.103";

    final String displayName = "Avoid using 'Enumerable.Any()' extension method";
    final String description = "Prefer using 'IsEmpty', 'Count' or 'Length' properties whichever available, rather than calling 'Enumerable.Any()'. The intent is clearer and it is more performant than using 'Enumerable.Any()' extension method.";
    final Set<String> tags = Stream.of("roslyn", "codefix", "CA1860", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
