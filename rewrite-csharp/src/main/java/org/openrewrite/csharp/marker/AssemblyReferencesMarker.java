package org.openrewrite.csharp.marker;

import lombok.EqualsAndHashCode;
import lombok.Value;
import lombok.With;
import org.openrewrite.marker.Marker;

import javax.annotation.Nullable;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.UUID;

@Value
@With
public class AssemblyReferencesMarker implements Marker {
    @EqualsAndHashCode.Exclude
    UUID id;

    /**
     * Assembly reference
     */
    @Value
    @With
    public static class AssemblyReference
    {
        byte[] assembly;
        String fileName;
        @Nullable String packageName;
        @Nullable String packageVersion;
        public void writeToFolder(String directory) throws IOException {
            Path dirPath = Paths.get(directory);
            Files.createDirectories(dirPath); // Ensure the directory exists

            Path filePath = dirPath.resolve(fileName);
            Files.write(filePath, assembly, StandardOpenOption.CREATE, StandardOpenOption.TRUNCATE_EXISTING);
        }
    }
}
