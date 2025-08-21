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
package org.openrewrite.csharp;

import lombok.*;
import org.jspecify.annotations.Nullable;
import org.openrewrite.*;
import org.openrewrite.quark.Quark;
import org.openrewrite.scheduling.WorkingDirectoryExecutionContextView;
import org.openrewrite.text.PlainText;
import org.openrewrite.tree.ParseError;

import java.io.File;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.*;
import java.nio.file.attribute.BasicFileAttributes;
import java.util.*;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicInteger;

import static java.util.Collections.emptyList;
import static org.openrewrite.scheduling.WorkingDirectoryExecutionContextView.WORKING_DIRECTORY_ROOT;

public abstract class RoslynRecipe extends ScanningRecipe<RoslynRecipe.Accumulator> {
    private static final String FIRST_RECIPE = RoslynRecipe.class.getName() + ".FIRST_RECIPE";
    private static final String RECIPE_COUNT = RoslynRecipe.class.getName() + ".RECIPE_COUNT";
    private static final String RECIPE_LIST = RoslynRecipe.class.getName() + ".RECIPE_LIST";
    private static final String PREVIOUS_RECIPE = RoslynRecipe.class.getName() + ".PREVIOUS_RECIPE";
    private static final String INIT_REPO_DIR = RoslynRecipe.class.getName() + ".INIT_REPO_DIR";

    @Override
    public int maxCycles() {
        return 1;
    }

    public final String getExecutable() {
        String executable = System.getenv("ROSLYN_RECIPE_EXECUTABLE");
        if (executable == null) {
            throw new IllegalStateException("ROSLYN_RECIPE_EXECUTABLE environment variable not set");
        }
        if(!executable.endsWith(".dll"))
        {
            executable = ensureTrailingSeparator(executable) + "Rewrite.Server.dll";
        }

        return executable;
    }

    public static String ensureTrailingSeparator(String path) {
        if (path == null || path.isEmpty()) return File.separator;
        String separator = File.separator;
        if (path.endsWith("/") || path.endsWith("\\")) {
            // Normalize to current OS separator
            if (!path.endsWith(separator)) {
                path = path.substring(0, path.length() - 1) + separator;
            }
            return path;
        }
        return path + separator;
    }

    public abstract String getRecipeId();

    public abstract String getNugetPackageName();

    public abstract String getNugetPackageVersion();

    @Override
    public Accumulator getInitialValue(ExecutionContext ctx) {
        WorkingDirectoryExecutionContextView.view(ctx).getWorkingDirectory(); // ensure working directory is created
        List<RoslynRecipe> recipes = ctx.getMessage(RECIPE_LIST, new ArrayList<>());
        recipes.add(this);
        ctx.putMessage(RECIPE_LIST, recipes);
//        int recipeCount = ctx.getMessage(RECIPE_COUNT, 0);
//        recipeCount++;
//        ctx.putMessage(RECIPE_COUNT, recipeCount);
        Path directory = createDirectory(ctx, "repo");
        if (ctx.getMessage(INIT_REPO_DIR) == null) {
            ctx.putMessage(INIT_REPO_DIR, directory);
            ctx.putMessage(FIRST_RECIPE, ctx.getCycleDetails().getRecipePosition());
        }
        return new Accumulator(directory);
    }

    @Override
    public TreeVisitor<?, ExecutionContext> getScanner(Accumulator acc) {
        return new TreeVisitor<Tree, ExecutionContext>() {
            @Override
            public @Nullable Tree visit(@Nullable Tree tree, ExecutionContext ctx) {
            if (tree instanceof SourceFile
                    && !(tree instanceof Quark)
                    && !(tree instanceof ParseError)
                    && !tree.getClass().getName().equals("org.openrewrite.java.tree.J$CompilationUnit")) {
                SourceFile sourceFile = (SourceFile) tree;
                String fileName = sourceFile.getSourcePath().getFileName().toString();
                int lastDot = fileName.lastIndexOf('.');
                if (lastDot > 0) {
                    String extension = fileName.substring(lastDot + 1);
                    if ("sln".equals(extension)) {
                        acc.solutionFiles.add(sourceFile.getSourcePath());
                    }
                    acc.extensionCounts.computeIfAbsent(extension, e -> new AtomicInteger(0)).incrementAndGet();
                }


                // only extract initial source files for first roslyn recipe
                if (Objects.equals(ctx.getMessage(FIRST_RECIPE), ctx.getCycleDetails().getRecipePosition())) {
                    // FIXME filter out more source types; possibly only write plain text, json, and yaml?
                    acc.writeSource(sourceFile);
                }
            }
            return tree;
            }
        };
    }

    @Override
    public Collection<? extends SourceFile> generate(Accumulator acc, ExecutionContext ctx) {
        Path previous = ctx.getMessage(PREVIOUS_RECIPE);
        if (previous != null && !Objects.equals(ctx.getMessage(FIRST_RECIPE), ctx.getCycleDetails().getRecipePosition())) {
            acc.copyFromPrevious(previous);
        }
        if(isLastRecipe(ctx)) {
            runRoslynRecipe(acc, ctx);
        }
        ctx.putMessage(PREVIOUS_RECIPE, acc.getDirectory());

        // FIXME check for generated files
        return emptyList();
    }
    boolean isLastRecipe(ExecutionContext ctx) {
        List<RoslynRecipe> recipes = ctx.getMessage(RECIPE_LIST, new ArrayList<>());

        return ctx.getCycleDetails().getRecipePosition() == recipes.size();
    }

    private Path getCodeDir(ExecutionContext ctx) {
        Path workingDirectoryRoot = ctx.getMessage(WORKING_DIRECTORY_ROOT);
        if (workingDirectoryRoot == null) {
            WorkingDirectoryExecutionContextView.view(ctx).getWorkingDirectory(); // ensure we have a global root for the whole run
            workingDirectoryRoot = ctx.getMessage(WORKING_DIRECTORY_ROOT);
        }

        assert workingDirectoryRoot != null;

        return Optional.of(workingDirectoryRoot)
                .map(d -> d.resolve("repo"))
                .map(d -> {
                    try {
                        return Files.createDirectory(d).toRealPath();
                    } catch (IOException e) {
                        throw new UncheckedIOException(e);
                    }
                })
                .orElseThrow(() -> new IllegalStateException("Failed to create code working directory"));

        return workingDirectoryRoot.resolve("roslyn");
    }

    final String template = "dotnet ${exec} run-recipe --solution ${solution} --id ${recipeId} --package ${package} --version ${version}";
    @SneakyThrows
    protected void runRoslynRecipe(Accumulator acc, ExecutionContext ctx) {
        Path dir = acc.getDirectory();
        List<RoslynRecipe> recipes = ctx.getMessage(RECIPE_LIST, new ArrayList<>());

        Map<String, String> env = getCommandEnvironment(acc, ctx);
        var commandBuilder = new StringBuilder();
        for(RoslynRecipe recipe : recipes) {
            commandBuilder.append(recipe.getNugetPackageName());
            commandBuilder.append(":");
            commandBuilder.append(recipe.getNugetPackageVersion());
            commandBuilder.append("/");
            commandBuilder.append(recipe.getRecipeId());
            commandBuilder.append(" -> ");
            for (var solutionFile : acc.getSolutionFiles()) {

                commandBuilder.append(solutionFile.toString());
                commandBuilder.append("\n");
            }
        }
        var codeDir = getCodeDir(ctx);

        var recipeCommand = template;
        recipeCommand = recipeCommand.replace("${exec}", Objects.requireNonNull(this.getExecutable()));
        recipeCommand = recipeCommand.replace("${recipeId}", Objects.requireNonNull(this.getRecipeId()));
        recipeCommand = recipeCommand.replace("${package}", Objects.requireNonNull(this.getNugetPackageName()));
        recipeCommand = recipeCommand.replace("${version}", Objects.requireNonNull(this.getNugetPackageVersion()));

        for (var solutionFile : acc.getSolutionFiles()) {

            var finalCommand = recipeCommand;
            finalCommand = finalCommand.replace("${solution}", solutionFile.toString());
            List<String> command = new ArrayList<>();
            for (String part : finalCommand.split(" ")) {
                part = part.trim();
                command.add(part);
            }

            Path out = null, err = null;
            try {
                ProcessBuilder builder = new ProcessBuilder();
                builder.command(command);
                builder.directory(dir.toFile());
                builder.environment().put("TERM", "dumb");
                env.forEach(builder.environment()::put);

                out = Files.createTempFile(WorkingDirectoryExecutionContextView.view(ctx).getWorkingDirectory(), "roslyn", null);
                err = Files.createTempFile(WorkingDirectoryExecutionContextView.view(ctx).getWorkingDirectory(), "roslyn", null);
                builder.redirectOutput(ProcessBuilder.Redirect.to(out.toFile()));
                builder.redirectError(ProcessBuilder.Redirect.to(err.toFile()));

                Process process = builder.start();
                if (!process.waitFor(5, TimeUnit.MINUTES)) {
                    throw new RuntimeException(String.format("Command '%s' timed out after 5 minutes", String.join(" ", command)));
                } else if (process.exitValue() != 0) {
                    String error = "Command failed: " + String.join(" ", command);
                    if (Files.exists(err)) {
                        error += "\n" + new String(Files.readAllBytes(err));
                    }
                    error += "\nCommand:" + finalCommand;
                    throw new RuntimeException(error);
                } else {
                    for (Map.Entry<Path, Long> entry : acc.beforeModificationTimestamps.entrySet()) {
                        Path path = entry.getKey();
                        if (!Files.exists(path) || Files.getLastModifiedTime(path).toMillis() > entry.getValue()) {
                            acc.modified(path);
                        }
                    }
                    processOutput(out, acc, ctx);
                }
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            } finally {
                if (out != null) {
                    String content = new String(Files.readAllBytes(out));
                    System.out.println(content);
                    //noinspection ResultOfMethodCallIgnored
                    out.toFile().delete();
                }
                if (err != null) {
                    String content = new String(Files.readAllBytes(err));
                    System.out.println(content);
                    //noinspection ResultOfMethodCallIgnored
                    err.toFile().delete();
                }
            }
        }
    }


    protected Map<String, String> getCommandEnvironment(Accumulator acc, ExecutionContext ctx) {
        return new HashMap<>();
    }

    protected void processOutput(Path out, Accumulator acc, ExecutionContext ctx) {
    }


    @Override
    public TreeVisitor<?, ExecutionContext> getVisitor(Accumulator acc) {
        return new TreeVisitor<Tree, ExecutionContext>() {
            @Override
            public @Nullable Tree visit(@Nullable Tree tree, ExecutionContext ctx) {
                if (tree instanceof SourceFile) {
                    SourceFile sourceFile = (SourceFile) tree;
                    // TODO parse sources like JSON where parser doesn't require an environment
                    return createAfter(sourceFile, acc, ctx);
                }
                return tree;
            }
        };
    }

    protected SourceFile createAfter(SourceFile before, Accumulator acc, ExecutionContext ctx) {
        if (!acc.wasModified(before)) {
            return before;
        }
        return new PlainText(
                before.getId(),
                before.getSourcePath(),
                before.getMarkers(),
                before.getCharset() != null ? before.getCharset().name() : null,
                before.isCharsetBomMarked(),
                before.getFileAttributes(),
                null,
                acc.content(before),
                emptyList()
        );
    }

    @ToString
    @EqualsAndHashCode
    @RequiredArgsConstructor
    public static class Accumulator {
        @Getter
        @Setter
        int recipeCount;
        @Getter
        final Path directory;
        @Getter
        List<Path> solutionFiles = new ArrayList<>();
        final Map<Path, Long> beforeModificationTimestamps = new HashMap<>();
        final Set<Path> modified = new LinkedHashSet<>();
        final Map<String, AtomicInteger> extensionCounts = new HashMap<>();
        final Map<String, Object> data = new HashMap<>();

        public void copyFromPrevious(Path previous) {
            try {
                Files.walkFileTree(previous, new SimpleFileVisitor<Path>() {
                    @Override
                    public FileVisitResult preVisitDirectory(Path dir, BasicFileAttributes attrs) throws IOException {
                        Path target = directory.resolve(previous.relativize(dir));
                        if (!target.equals(directory)) {
                            Files.createDirectory(target);
                        }
                        return FileVisitResult.CONTINUE;
                    }

                    @Override
                    public FileVisitResult visitFile(Path file, BasicFileAttributes attrs) throws IOException {
                        try {
                            Path target = directory.resolve(previous.relativize(file));
                            Files.copy(file, target);
                            beforeModificationTimestamps.put(target, Files.getLastModifiedTime(target).toMillis());
                        } catch (NoSuchFileException ignore) {
                        }
                        return FileVisitResult.CONTINUE;
                    }
                });
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            }
        }

        public String parser() {
            if (extensionCounts.containsKey("tsx")) {
                return "tsx";
            } else if (extensionCounts.containsKey("ts")) {
                return "ts";
            } else {
                return "babel";
            }
        }

        public void writeSource(SourceFile tree) {
            try {
                Path path = resolvedPath(tree);
                Files.createDirectories(path.getParent());
                PrintOutputCapture.MarkerPrinter markerPrinter = new PrintOutputCapture.MarkerPrinter() {
                };
                Path written = Files.write(path, tree.printAll(new PrintOutputCapture<>(0, markerPrinter)).getBytes(tree.getCharset() != null ? tree.getCharset() : StandardCharsets.UTF_8));
                beforeModificationTimestamps.put(written, Files.getLastModifiedTime(written).toMillis());
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            }
        }

        public void modified(Path path) {
            modified.add(path);
        }

        public boolean wasModified(SourceFile tree) {
            return modified.contains(resolvedPath(tree));
        }

        public String content(SourceFile tree) {
            try {
                Path path = resolvedPath(tree);
                return tree.getCharset() != null ? new String(Files.readAllBytes(path), tree.getCharset()) :
                        new String(Files.readAllBytes(path));
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            }
        }

        public Path resolvedPath(SourceFile tree) {
            return directory.resolve(tree.getSourcePath());
        }

        public <T> void putData(String key, T value) {
            data.put(key, value);
        }

        public <T> @Nullable T getData(String key) {
            //noinspection unchecked
            return (T) data.get(key);
        }
    }

    protected static Path createDirectory(ExecutionContext ctx, String prefix) {
        WorkingDirectoryExecutionContextView view = WorkingDirectoryExecutionContextView.view(ctx);
        return Optional.of(view.getWorkingDirectory())
                .map(d -> d.resolve(prefix))
                .map(d -> {
                    try {
                        return Files.createDirectory(d).toRealPath();
                    } catch (IOException e) {
                        throw new UncheckedIOException(e);
                    }
                })
                .orElseThrow(() -> new IllegalStateException("Failed to create working directory for " + prefix));
    }

}
