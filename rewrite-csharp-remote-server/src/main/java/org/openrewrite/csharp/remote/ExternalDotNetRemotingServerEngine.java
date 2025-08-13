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
package org.openrewrite.csharp.remote;

import lombok.SneakyThrows;

import java.util.List;

public class ExternalDotNetRemotingServerEngine extends DotNetRemotingServerEngine {

    public ExternalDotNetRemotingServerEngine(Config config) {
        super(config);
    }

    @SneakyThrows
    public static DotNetRemotingServerEngine create(Config config) {
        try {
            installExecutable(config.dotnetServerZipLocation, config.extractedDotnetBinaryDir.toFile());
        }catch(Exception e){

        }
        // FIXME rather than using hardcoded port, read it after starting up server
        return new ExternalDotNetRemotingServerEngine(config);
    }

    @Override
    protected ProcessBuilder configureProcess(ProcessBuilder processBuilder) {
        ProcessBuilder result = super.configureProcess(processBuilder);
        List<String> commands = result.command();
        commands.add("--dummy");
        return result.command(commands);
    }

}
