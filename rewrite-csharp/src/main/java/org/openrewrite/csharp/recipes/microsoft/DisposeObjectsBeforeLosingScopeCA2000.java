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
public class DisposeObjectsBeforeLosingScopeCA2000 extends RoslynRecipe {

    final String recipeId = "CA2000";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "Microsoft.CodeAnalysis.NetAnalyzers";
    final String nugetPackageVersion = "10.0.102";

    final String displayName = "Dispose objects before losing scope (search)";
    final String description = "This is a reporting only recipe. If a disposable object is not explicitly disposed before all references to it are out of scope, the object will be disposed at some indeterminate time when the garbage collector runs the finalizer of the object. Because an exceptional event might occur that will prevent the finalizer of the object from running, the object should be explicitly disposed instead.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "CA2000", "microsoft", "csharp", "dotnet", "c#").collect(toSet());

}
