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

public class CSharpRemoveUnnecessaryImportsDiagnosticAnalyzerEnableGenerateDocumentationFile extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "EnableGenerateDocumentationFile";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.CSharp.CodeStyle";
    }

    @Override
    public String getNugetPackageVersion() {
        return "5.0.0";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Set MSBuild property 'GenerateDocumentationFile' to 'true'";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Add the following PropertyGroup to your MSBuild project file to enable IDE0005 (Remove unnecessary usings/imports) on build:    <PropertyGroup>      <!--        Make sure any documentation comments which are included in code get checked for syntax during the build, but do        not report warnings for missing comments.        CS1573: Parameter 'parameter' has no matching param tag in the XML comment for 'parameter' (but other parameters do)        CS1591: Missing XML comment for publicly visible type or member 'Type_or_Member'        CS1712: Type parameter 'type_parameter' has no matching typeparam tag in the XML comment on 'type_or_member' (but other type parameters do)      -->      <GenerateDocumentationFile>True</GenerateDocumentationFile>      <NoWarn>$(NoWarn),1573,1591,1712</NoWarn>    </PropertyGroup>      ";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "EnableGenerateDocumentationFile", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
