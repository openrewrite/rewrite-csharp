package org.openrewrite.csharp.recipes;

import org.junit.jupiter.api.Test;
import org.openrewrite.test.RecipeSpec;
import org.openrewrite.test.RewriteTest;
import org.openrewrite.test.TypeValidation;

import static org.openrewrite.test.SourceSpecs.text;

import static org.openrewrite.xml.Assertions.xml;

public class AvoidConstantArraysAsArgumentsTest implements RewriteTest {

    @Override
    public void defaults(RecipeSpec spec) {
        spec
          .recipe(new AvoidConstantArraysAsArguments())
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
