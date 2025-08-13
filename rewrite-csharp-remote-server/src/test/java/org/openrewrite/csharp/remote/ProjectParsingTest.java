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

import lombok.SneakyThrows;
import org.junit.jupiter.api.Test;
import org.openrewrite.Cursor;
import org.openrewrite.InMemoryExecutionContext;
import org.openrewrite.remote.RemotingProjectParser;
import org.openrewrite.scheduling.WatchableExecutionContext;

import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.concurrent.ThreadLocalRandom;

import static org.assertj.core.api.Assertions.assertThatThrownBy;

public class ProjectParsingTest {

    @SneakyThrows
    @Test
    public void testParse() {
        WatchableExecutionContext ctx =
          new WatchableExecutionContext(new InMemoryExecutionContext());

        int port = ThreadLocalRandom.current().nextInt(50000, 65535);
        try (DotNetRemotingServerEngine server = DotNetRemotingServerEngine.create(DotNetRemotingServerEngine.Config.builder()
          .extractedDotnetBinaryDir(Paths.get(System.getProperty("java.io.tmpdir")))
          .port(port)
          .logFilePath(Paths.get("./build/test.log").toAbsolutePath().toString())
          .build())) {

            server.start();
            var client = new RemotingProjectParser(server);

            Path pathToSolution = Path.of(Thread.currentThread()
              .getContextClassLoader()
              .getResource(
                "ModerneHelloWorld/ModerneHelloWorld.sln")
                    .toURI());
            client.findAllProjects(pathToSolution, ctx)
              .forEach(proj -> client.parseProjectSources(proj,
                  pathToSolution,
                  pathToSolution.getParent(),
                  ctx)
                .forEach(sf ->
                    System.out.println(sf.print(new Cursor(new Cursor(
                      null,
                      Cursor.ROOT_VALUE), sf)))));
        }
    }

    @Test
    public void testThrowExceptionOnIncorrectDotnetPath() {
        int port = ThreadLocalRandom.current().nextInt(50000, 65535);
        try (DotNetRemotingServerEngine server = DotNetRemotingServerEngine.create(DotNetRemotingServerEngine.Config.builder().dotnetExecutable("dotnet1").extractedDotnetBinaryDir(
          Paths.get(System.getProperty("java.io.tmpdir"))).port(port).build())) {
            assertThatThrownBy(server::start)
              .isInstanceOf(IOException.class);
        }
    }

    @Test
    public void testThrowExceptionOnIncorrectRunnable() {
        int port = ThreadLocalRandom.current().nextInt(50000, 65535);
        try (DotNetRemotingServerEngine server = DotNetRemotingServerEngine.create(DotNetRemotingServerEngine.Config.builder()
          .extractedDotnetBinaryDir(Paths.get(System.getProperty("java.io.tmpdir")))
          .dotnetServerDllName("Rewrite.Server1.dll")
          .port(port)
          .build())) {
            assertThatThrownBy(server::start)
              .isInstanceOf(IllegalStateException.class);
        }
    }
}
