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

import org.openrewrite.test.RewriteTest;
import org.openrewrite.test.SourceSpec;
import org.openrewrite.test.SourceSpecs;

import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static org.openrewrite.test.SourceSpecs.text;

public abstract class RoslynRecipeTest implements RewriteTest {

    protected static final String DEFAULT_EDITORCONFIG = """
        root = true

        [*]
        charset = utf-8
        end_of_line = lf
        """;

    protected Solution createSolution() {
        return new Solution();
    }

    protected Solution createSolution(String name) {
        return new Solution(name);
    }

    public static class Solution {
        private final String name;
        private final List<Project> projects = new ArrayList<>();
        private String editorConfig = DEFAULT_EDITORCONFIG;

        public Solution() {
            this("Test.sln");
        }

        public Solution(String name) {
            this.name = name;
        }

        public Solution withEditorConfig(String editorConfig) {
            this.editorConfig = editorConfig;
            return this;
        }

        public Solution addProject(Project project) {
            projects.add(project);
            return this;
        }

        public Solution addProject(String path) {
            projects.add(new Project(path));
            return this;
        }

        public SourceSpecs[] toSourceSpecs() {
            List<SourceSpecs> specs = new ArrayList<>();

            // Add solution file
            specs.add(text(generateSolutionContent(), spec -> spec.path(name)));

            // Add .editorconfig
            specs.add(text(editorConfig, spec -> spec.path(".editorconfig")));

            // Add all projects and their files
            for (Project project : projects) {
                specs.addAll(project.toSourceSpecs());
            }

            return specs.toArray(new SourceSpecs[0]);
        }

        private String generateSolutionContent() {
            StringBuilder sb = new StringBuilder();
            sb.append("Microsoft Visual Studio Solution File, Format Version 12.00\n");

            for (Project project : projects) {
                String projectGuid = project.getGuid();
                String projectName = project.getName();
                String projectPath = project.getPath();

                sb.append("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"%s\", \"%s\", \"{%s}\"\n".formatted(
                        projectName, projectPath, projectGuid
                ));
                sb.append("EndProject\n");
            }

            sb.append("Global\n");
            sb.append("    GlobalSection(SolutionConfigurationPlatforms) = preSolution\n");
            sb.append("        Debug|Any CPU = Debug|Any CPU\n");
            sb.append("        Release|Any CPU = Release|Any CPU\n");
            sb.append("    EndGlobalSection\n");

            if (!projects.isEmpty()) {
                sb.append("    GlobalSection(ProjectConfigurationPlatforms) = postSolution\n");
                for (Project project : projects) {
                    String projectGuid = project.getGuid();
                    sb.append("        {%s}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n".formatted(projectGuid));
                    sb.append("        {%s}.Debug|Any CPU.Build.0 = Debug|Any CPU\n".formatted(projectGuid));
                    sb.append("        {%s}.Release|Any CPU.ActiveCfg = Release|Any CPU\n".formatted(projectGuid));
                    sb.append("        {%s}.Release|Any CPU.Build.0 = Release|Any CPU\n".formatted(projectGuid));
                }
                sb.append("    EndGlobalSection\n");
            }

            sb.append("EndGlobal\n");

            return sb.toString();
        }
    }

    public static class Project {
        private final String path;
        private final String guid;
        private final List<SourceFile> sourceFiles = new ArrayList<>();
        private String targetFramework = "net9.0";
        private boolean implicitUsings = true;
        private boolean nullable = true;

        public Project(String path) {
            this.path = path;
            this.guid = UUID.randomUUID().toString().toUpperCase();
        }

        public String getPath() {
            return path;
        }

        public String getName() {
            return Path.of(path).getFileName().toString().replace(".csproj", "");
        }

        public String getGuid() {
            return guid;
        }

        public Project withTargetFramework(String targetFramework) {
            this.targetFramework = targetFramework;
            return this;
        }

        public Project withImplicitUsings(boolean implicitUsings) {
            this.implicitUsings = implicitUsings;
            return this;
        }

        public Project withNullable(boolean nullable) {
            this.nullable = nullable;
            return this;
        }

        public Project addSourceFile(String path, String content) {
            sourceFiles.add(new SourceFile(path, content, null));
            return this;
        }

        public Project addSourceFile(String path, String before, String after) {
            sourceFiles.add(new SourceFile(path, before, after));
            return this;
        }

        public List<SourceSpecs> toSourceSpecs() {
            List<SourceSpecs> specs = new ArrayList<>();

            // Add project file
            specs.add(text(generateProjectContent(), spec -> spec.path(path)));

            // Add source files
            String projectDir = getProjectDirectory();
            for (SourceFile file : sourceFiles) {
                String fullPath = projectDir.isEmpty() ? file.path : projectDir + "/" + file.path;

                if (file.after != null) {
                    // File with before/after transformation
                    specs.add(text(file.content, file.after, spec -> spec.path(fullPath)));
                } else {
                    // File without transformation
                    specs.add(text(file.content, spec -> spec.path(fullPath)));
                }
            }

            return specs;
        }

        private String getProjectDirectory() {
            int lastSlash = path.lastIndexOf('/');
            return lastSlash > 0 ? path.substring(0, lastSlash) : "";
        }

        private String generateProjectContent() {
            return """
                <Project Sdk="Microsoft.NET.Sdk">
                    <PropertyGroup>
                        <TargetFramework>%s</TargetFramework>
                        <ImplicitUsings>%s</ImplicitUsings>
                        <Nullable>%s</Nullable>
                    </PropertyGroup>
                </Project>
                """.formatted(
                    targetFramework,
                    implicitUsings ? "enable" : "disable",
                    nullable ? "enable" : "disable"
            );
        }

        private static class SourceFile {
            final String path;
            final String content;
            final String after;

            SourceFile(String path, String content, String after) {
                this.path = path;
                this.content = content;
                this.after = after;
            }
        }
    }

    /**
     * Helper method to expand a solution into source specs for use in rewriteRun
     */
    protected SourceSpecs[] solutionToSpecs(Solution solution) {
        return solution.toSourceSpecs();
    }

    /**
     * Helper method to combine multiple solutions into a single SourceSpecs array
     */
    protected SourceSpecs[] combineSolutions(Solution... solutions) {
        List<SourceSpecs> combined = new ArrayList<>();
        for (Solution solution : solutions) {
            for (SourceSpecs spec : solution.toSourceSpecs()) {
                combined.add(spec);
            }
        }
        return combined.toArray(new SourceSpecs[0]);
    }

    /**
     * Convenience method to run a recipe with a solution
     */
    protected void rewriteRunWithSolution(Solution solution) {
        rewriteRun(solution.toSourceSpecs());
    }
}
