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
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

public class EquatableCA1066 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1066";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Getter
    final String displayName = "Implement IEquatable when overriding Object.Equals";

    @Getter
    final String description = "When a type T overrides Object.Equals(object), the implementation must cast the object argument to the correct type T before performing the comparison. If the type implements IEquatable<T>, and therefore offers the method T.Equals(T), and if the argument is known at compile time to be of type T, then the compiler can call IEquatable<T>.Equals(T) instead of Object.Equals(object), and no cast is necessary, improving performance.";

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "CA1066", "microsoft", "csharp", "dotnet", "c#").collect(toSet());
    }
    }
