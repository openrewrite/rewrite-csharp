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
import org.openrewrite.remote.AbstractRemotingServerEngine;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.URL;
import java.nio.file.Path;
import java.time.Duration;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

@Log
public class DotNetRemotingServerEngine extends AbstractRemotingServerEngine {

    public static final String REWRITE_SERVER_DLL_NAME = "Rewrite.Server.dll";
    public static final URL REWRITE_SERVER_DLL_RESOURCE =
            Objects.requireNonNull(Thread.currentThread()
                    .getContextClassLoader()
                    .getResource("DotnetServer.zip"));
    public static final int DEFAULT_DEBUG_PORT = 54321;

    Config config;

    public static DotNetRemotingServerEngine create(Path extractedDotnetBinariesDir) {
        return create(Config.builder().extractedDotnetBinaryDir(extractedDotnetBinariesDir).build());
    }

    @SneakyThrows
    public static DotNetRemotingServerEngine create(Config config) {
        installExecutable(config.dotnetServerZipLocation, config.extractedDotnetBinaryDir.toFile());
        // FIXME rather than using hardcoded port, read it after starting up server
        return new DotNetRemotingServerEngine(config);
    }

    private DotNetRemotingServerEngine(Config config) {
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
        if (config.nugetPackagesFolder != null && !config.nugetPackagesFolder.isEmpty()) {
            command.add("-f" + config.nugetPackagesFolder);
        }
        return processBuilder.command(command);
    }

    @SneakyThrows
    private static void installExecutable(URL executable, File destDir) {
//        Files.copy(, destinationDir.resolve("extracted.zip"), StandardCopyOption.REPLACE_EXISTING);
        byte[] buffer = new byte[1024];
        try (ZipInputStream zis = new ZipInputStream(executable.openStream())) {
            ZipEntry zipEntry = zis.getNextEntry();
            while (zipEntry != null) {
                File newFile = newFile(destDir, zipEntry);
                if (zipEntry.isDirectory()) {
                    if (!newFile.isDirectory() && !newFile.mkdirs()) {
                        if (!newFile.isDirectory()) {
                            throw new IOException("Failed to create directory " + newFile);
                        }
                    }
                } else {
                    // fix for Windows-created archives
                    File parent = newFile.getParentFile();
                    if (!parent.isDirectory() && !parent.mkdirs()) {
                        throw new IOException("Failed to create directory " + parent);
                    }

                    // write file content
                    try (FileOutputStream fos = new FileOutputStream(newFile)) {
                        int len;
                        while ((len = zis.read(buffer)) > 0) {
                            fos.write(buffer, 0, len);
                        }
                    }
                }
                zipEntry = zis.getNextEntry();
            }
        }

    }

    static File newFile(File destinationDir, ZipEntry zipEntry) throws IOException {
        File destFile = new File(destinationDir, zipEntry.getName());

        String destDirPath = destinationDir.getCanonicalPath();
        String destFilePath = destFile.getCanonicalPath();

        if (!destFilePath.startsWith(destDirPath + File.separator)) {
            throw new IOException("Entry is outside of the target dir: " + zipEntry.getName());
        }

        return destFile;
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
        URL dotnetServerZipLocation = REWRITE_SERVER_DLL_RESOURCE;

        @Builder.Default()
        String dotnetServerDllName = REWRITE_SERVER_DLL_NAME;
    }
}
