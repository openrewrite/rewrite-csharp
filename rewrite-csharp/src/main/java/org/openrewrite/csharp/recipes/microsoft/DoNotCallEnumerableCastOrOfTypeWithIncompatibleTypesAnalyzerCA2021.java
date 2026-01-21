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

public class DoNotCallEnumerableCastOrOfTypeWithIncompatibleTypesAnalyzerCA2021 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2021";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "10.0.102";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Do not call Enumerable.Cast<T> or Enumerable.OfType<T> with incompatible types";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Enumerable.Cast<T> and Enumerable.OfType<T> require compatible types to function expectedly.    The generic cast (IL 'unbox.any') used by the sequence returned by Enumerable.Cast<T> will throw InvalidCastException at runtime on elements of the types specified.    The generic type check (C# 'is' operator/IL 'isinst') used by Enumerable.OfType<T> will never succeed with elements of types specified, resulting in an empty sequence.    Widening and user defined conversions are not supported with generic types.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA2021", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
