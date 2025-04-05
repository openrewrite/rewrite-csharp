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
        result = result.command(commands);
        return result;
    }

}
