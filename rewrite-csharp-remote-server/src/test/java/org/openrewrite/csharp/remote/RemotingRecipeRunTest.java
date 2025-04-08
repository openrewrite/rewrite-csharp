/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.openrewrite.csharp.remote;

import lombok.extern.java.Log;
import org.junit.jupiter.api.Test;
import org.openrewrite.*;
import org.openrewrite.config.RecipeDescriptor;
import org.openrewrite.csharp.tree.Cs;
import org.openrewrite.internal.InMemoryLargeSourceSet;
import org.openrewrite.java.JavaParser;
import org.openrewrite.java.tree.J;
import org.openrewrite.java.tree.JRightPadded;
import org.openrewrite.java.tree.Space;
import org.openrewrite.marker.Markers;
import org.openrewrite.remote.InstallableRemotingRecipe;
import org.openrewrite.remote.PackageSource;
import org.openrewrite.remote.RemotingContext;
import org.openrewrite.remote.RemotingExecutionContextView;
import org.openrewrite.remote.RemotingRecipe;
import org.openrewrite.remote.RemotingRecipeManager;
import org.openrewrite.remote.TcpUtils;

import java.io.File;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.logging.Level;

import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.fail;

@Log
public class RemotingRecipeRunTest {

    @Test
    public void testRemotingRecipeInstallAndRun() throws IOException {
        File nuPkgLocation = new File("./build/nupkgs");
        nuPkgLocation.mkdir();
        int port = TcpUtils.findAvailableTcpPortInternal();
        port = 54321;
        Path extractedDotnetBinaryDir = Paths.get("./build/dotnet-servet-archive");
        extractedDotnetBinaryDir.toFile().mkdirs();
        var serverConfig = DotNetRemotingServerEngine.Config.builder()
                .extractedDotnetBinaryDir(extractedDotnetBinaryDir)
                .logFilePath(Paths.get("./build/test.log").toAbsolutePath().toString())
                .nugetPackagesFolder(nuPkgLocation.toPath().toAbsolutePath().normalize().toString())
                .port(port)
                .build();
        DotNetRemotingServerEngine server = DotNetRemotingServerEngine.create(serverConfig);
//        DotNetRemotingServerEngine server = ExternalDotNetRemotingServerEngine.create(serverConfig);
        try {
            server.start();

            InMemoryExecutionContext ctx = new InMemoryExecutionContext((e) -> log.log(Level.WARNING, e.toString()));

            RemotingExecutionContextView view = RemotingExecutionContextView.view(ctx);
            view.setRemotingContext(new RemotingContext(this.getClass().getClassLoader(), false));

            RemotingRecipeManager manager = new RemotingRecipeManager(server, () -> server);

            Path artifactsFolder = GitRootFinder.getGitRoot().resolve("artifacts");

            InstallableRemotingRecipe recipes = manager.install(
              "Rewrite.Recipes",
              "0.0.1",
              Arrays.asList(
                new PackageSource(artifactsFolder.toAbsolutePath().toFile().toURI().toURL(), null, null, true),
                new PackageSource(artifactsFolder.resolve("test").toAbsolutePath().toFile().toURI().toURL(), null, null, true)
              ),
              true,
              ctx
            );

            Recipe selectedRecipe = recipes.getRecipes().stream().filter(x -> x.getName().equals("Rewrite.Recipes.FindClass"))
                    .findFirst()
                    .orElseThrow();


            RemotingRecipe remotingRecipe = new RemotingRecipe(selectedRecipe.getDescriptor(), () -> server, DotNetRemotingServerEngine.class);
            Cs.ClassDeclaration classDeclaration = new Cs.ClassDeclaration(
                    Tree.randomId(),
                    Space.EMPTY,
                    Markers.EMPTY,
                    new ArrayList<>(),
                    new ArrayList<>(),
                    new J.ClassDeclaration.Kind(
                                    Tree.randomId(),
                                    Space.EMPTY,
                                    Markers.EMPTY,
                                    new ArrayList<>(),
                            J.ClassDeclaration.Kind.Type.Class),
                    new J.Identifier(
                            Tree.randomId(),
                            Space.EMPTY,
                            Markers.EMPTY,
                            new ArrayList<>(),
                            "myclass",
                            null,
                            null),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );
            SourceFile tree = new Cs.CompilationUnit(
                    Tree.randomId(),
                    Space.EMPTY,
                    Markers.EMPTY,
                    Path.of("foo.cs"),
                    null,
                    null,
                    false,
                    null,
                    new ArrayList<>(),
                    new ArrayList<>(),
                    new ArrayList<>(),
                    List.of(JRightPadded.build(classDeclaration)),
                    Space.EMPTY);
//            SourceFile tree = JavaParser.fromJavaVersion().build().parse("class Foo {}")
//              .findFirst()
//              .orElseThrow();

            RecipeRun run = remotingRecipe.run(new InMemoryLargeSourceSet(Collections.singletonList(tree)), ctx);
            assertThat(run.getChangeset().getAllResults()).hasSize(1);
            run.getChangeset().getAllResults().forEach(r -> System.out.println(r.diff()));
        } catch (Exception e) {
            server.close();
            fail(e);
        }

        server.close();
    }
}
