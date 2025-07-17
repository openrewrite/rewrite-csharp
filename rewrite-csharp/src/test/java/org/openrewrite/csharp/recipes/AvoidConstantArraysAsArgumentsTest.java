/*
 * Copyright 2025 the original author or authors.
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
package org.openrewrite.csharp.recipes;

import org.junit.jupiter.api.Test;
import org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers.AvoidConstArraysCA1861;
import org.openrewrite.test.RecipeSpec;
import org.openrewrite.test.RewriteTest;
import org.openrewrite.test.TypeValidation;

import static org.openrewrite.test.SourceSpecs.text;

import static org.openrewrite.xml.Assertions.xml;

public class AvoidConstantArraysAsArgumentsTest implements RewriteTest {

    @Override
    public void defaults(RecipeSpec spec) {
        spec
          .recipe(new AvoidConstArraysCA1861())
          .typeValidationOptions(TypeValidation.builder()
            .immutableExecutionContext(false).build());
    }

    @Test
    public void runRecipe() {
        rewriteRun(
          text(
            """
              Microsoft Visual Studio Solution File, Format Version 12.00
              Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "RoslynSample", "RoslynSample.csproj", "{81D4529D-02A5-4A2F-B8F3-261D2E365A73}"
              EndProject
              Global
                  GlobalSection(SolutionConfigurationPlatforms) = preSolution
                      Debug|Any CPU = Debug|Any CPU
                      Release|Any CPU = Release|Any CPU
                  EndGlobalSection
                  GlobalSection(ProjectConfigurationPlatforms) = postSolution
                      {81D4529D-02A5-4A2F-B8F3-261D2E365A73}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                      {81D4529D-02A5-4A2F-B8F3-261D2E365A73}.Debug|Any CPU.Build.0 = Debug|Any CPU
                      {81D4529D-02A5-4A2F-B8F3-261D2E365A73}.Release|Any CPU.ActiveCfg = Release|Any CPU
                      {81D4529D-02A5-4A2F-B8F3-261D2E365A73}.Release|Any CPU.Build.0 = Release|Any CPU
                  EndGlobalSection
              EndGlobal
              """,
            spec -> spec.path("Test.sln")
          ),
          text("""
            root = true

            [*]
            charset = utf-8
            end_of_line = lf
            """,
          spec -> spec.path(".editorconfig")),
          xml(
            //language=xml
            """
              <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                      <TargetFramework>net9.0</TargetFramework>
                      <ImplicitUsings>enable</ImplicitUsings>
                      <Nullable>enable</Nullable>
                  </PropertyGroup>
              </Project>
              """,
            spec -> spec.path("RoslynSample.csproj")
          ),
          text(
            """
              public class Test
              {
                  public void DoSomething()
                  {
                      string message = string.Join(" ", new[] { "Hello", "world!" });
                  }
              }
              """,
            """
              public class Test
              {
                  private static readonly string[] value = new[] { "Hello", "world!" };

                  public void DoSomething()
                  {
                      string message = string.Join(" ", value);
                  }
              }
              """,
            spec -> spec.path("Sample.cs")));

    }
}
