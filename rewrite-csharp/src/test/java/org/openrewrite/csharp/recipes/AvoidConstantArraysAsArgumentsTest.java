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
import org.openrewrite.Recipe;
import org.openrewrite.config.CompositeRecipe;
import org.openrewrite.csharp.recipes.microsoft.AbstractTypesShouldNotHaveConstructorsFixerCA1012;
import org.openrewrite.csharp.recipes.microsoft.AvoidConstArraysFixerCA1861;
import org.openrewrite.test.RecipeSpec;
import org.openrewrite.test.TypeValidation;

import java.util.List;

public class AvoidConstantArraysAsArgumentsTest extends RoslynRecipeTest {

    @Override
    public void defaults(RecipeSpec spec) {
        spec
          .recipe(new AvoidConstArraysFixerCA1861())
          .typeValidationOptions(TypeValidation.builder()
            .immutableExecutionContext(false).build());
    }

    @Test
    public void runRecipe() {
        Solution solution = createSolution()
            .addProject(new Project("RoslynSample.csproj")
                .addSourceFile("Sample.cs",
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
                      """));

        rewriteRun(solution.toSourceSpecs());
    }

    @Test
    public void multipleSolutions() {
        Solution solution1 = createSolution("Solution1.sln")
            .addProject(new Project("Project1/Project1.csproj")
                .addSourceFile("Class1.cs",
                    """
                      public class Class1
                      {
                          public void Method1()
                          {
                              string result = string.Join(",", new[] { "a", "b", "c" });
                          }
                      }
                      """,
                    """
                      public class Class1
                      {
                          private static readonly string[] value = new[] { "a", "b", "c" };

                          public void Method1()
                          {
                              string result = string.Join(",", value);
                          }
                      }
                      """));
        Solution solution2 = createSolution("Solution2.sln")
            .addProject(new Project("Project2/Project2.csproj")
                .addSourceFile("Class2.cs",
                    """
                      public class Class2
                      {
                          public void Method2()
                          {
                              var items = string.Join("|", new[] { "x", "y", "z" });
                          }
                      }
                      """,
                    """
                      public class Class2
                      {
                          private static readonly string[] value = new[] { "x", "y", "z" };

                          public void Method2()
                          {
                              var items = string.Join("|", value);
                          }
                      }
                      """));

        rewriteRun(combineSolutions(solution1, solution2));
    }

    @Test
    public void compositeRecipe(){
        Solution solution1 = createSolution("Solution1.sln")
          .addProject(new Project("Project1/Project1.csproj")
            .addSourceFile("Class1.cs",
              """
                public class Class1
                {
                    public void Method1()
                    {
                        string result = string.Join(",", new[] { "a", "b", "c" });
                    }
                }
                """,
              """
                public class Class1
                {
                    private static readonly string[] value = new[] { "a", "b", "c" };

                    public void Method1()
                    {
                        string result = string.Join(",", value);
                    }
                }
                """));
        Solution solution2 = createSolution("Solution2.sln")
          .addProject(new Project("Project2/Project2.csproj")
            .addSourceFile("Class2.cs",
              """
                public class Class2
                {
                    public void Method2()
                    {
                        var items = string.Join("|", new[] { "x", "y", "z" });
                    }
                }
                """,
              """
                public class Class2
                {
                    private static readonly string[] value = new[] { "x", "y", "z" };

                    public void Method2()
                    {
                        var items = string.Join("|", value);
                    }
                }
                """));

        Recipe recipe = new CompositeRecipe(List.of(
          new AvoidConstArraysFixerCA1861(),
          new AbstractTypesShouldNotHaveConstructorsFixerCA1012())
        );

        var sources = this.combineSolutions(solution1, solution2);
        rewriteRun(
          spec -> spec
            .recipe(recipe),
          sources
        );
    }
}
