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

package org.openrewrite.csharp.recipes.openrewrite;

import lombok.Getter;
import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toSet;

@Getter
public class DriveInfoDriveFormatAnalyzerORNETX0001 extends RoslynRecipe {

    final String recipeId = "ORNETX0001";
    final boolean runCodeFixup = false;

    final String nugetPackageName = "OpenRewrite.RoslynRecipes";
    final String nugetPackageVersion = "0.31.35-SNAPSHOT";

    final String displayName = "DriveInfo.DriveFormat has behavioral changes in .NET 10 on Linux (search)";
    final String description = "This is a reporting only recipe. In .NET 10, DriveInfo.DriveFormat on Linux returns actual kernel filesystem type strings (e.g., 'ext3', 'ext4') instead of mapped constants. Filesystem names for cgroup ('cgroup'/'cgroup2' instead of 'cgroupfs'/'cgroup2fs') and SELinux ('selinuxfs' instead of 'selinux') have also changed. Check and update string comparisons to include these new filesystem type strings. Reference /proc/self/mountinfo for actual values.";
    final Set<String> tags = Stream.of("roslyn", "analyzer", "ORNETX0001", "openrewrite", "csharp", "dotnet", "c#").collect(toSet());

}
