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

import lombok.Builder;
import lombok.SneakyThrows;
import lombok.extern.java.Log;
import org.jspecify.annotations.Nullable;
import org.openrewrite.csharp.EmbeddedResourceHelper;
import org.openrewrite.remote.AbstractRemotingServerEngine;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.URL;
import java.nio.file.Path;
import java.time.Duration;
import java.util.ArrayList;
import java.util.List;

@Log
public class DotNetRemotingServerEngine extends AbstractRemotingServerEngine {

    public static final String REWRITE_SERVER_DLL_NAME = "Rewrite.Server.dll";

    public static final int DEFAULT_DEBUG_PORT = 54321;

    Config config;

    public static DotNetRemotingServerEngine create(Path extractedDotnetBinariesDir) {
        return create(Config.builder().extractedDotnetBinaryDir(extractedDotnetBinariesDir).build());
    }

    @SneakyThrows
    public static DotNetRemotingServerEngine create(Config config) {
        EmbeddedResourceHelper.installExecutable(config.dotnetServerZipLocation, config.extractedDotnetBinaryDir.toFile());
        // FIXME rather than using hardcoded port, read it after starting up server
        return new DotNetRemotingServerEngine(config);
    }

    protected DotNetRemotingServerEngine(Config config) {
        super(new InetSocketAddress(InetAddress.getLoopbackAddress(), config.port), Duration.ofMillis(config.timeoutInMilliseconds));
        this.config = config;
    }


    @Override
    protected ProcessBuilder configureProcess(ProcessBuilder processBuilder) {
        List<String> command = new ArrayList<>();
        command.add(config.dotnetExecutable);
        command.add(config.extractedDotnetBinaryDir.resolve(config.dotnetServerDllName).toString());
        command.add("-p" + config.port);
        command.add("-t" + config.timeoutInMilliseconds);
        if (config.logFilePath != null && !config.logFilePath.isEmpty()) {
            command.add("-l" + config.logFilePath);
        }
        return processBuilder.command(command);
    }



    @Override
    public String getLanguageName() {
        return "CSharp";
    }



    @Builder
    public static class Config {
        @Builder.Default()
        int port = DEFAULT_DEBUG_PORT;

        @Builder.Default()
        int timeoutInMilliseconds = (int) Duration.ofHours(1).toMillis();

        @Nullable
        @Builder.Default()
        String logFilePath = null;

        @Nullable
        @Builder.Default()
        String nugetPackagesFolder = null;

        @Builder.Default()
        String dotnetExecutable = "dotnet";

        Path extractedDotnetBinaryDir;

        @Builder.Default()
        URL dotnetServerZipLocation = EmbeddedResourceHelper.REWRITE_SERVER_DLL_RESOURCE;

        @Builder.Default()
        String dotnetServerDllName = REWRITE_SERVER_DLL_NAME;
    }
}
