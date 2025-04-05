package org.openrewrite.csharp.remote;

import java.io.File;
import java.nio.file.Path;

public class GitRootFinder {

    public static File findGitRoot(File currentDir) {
        File dir = currentDir;
        while (dir != null) {
            File gitDir = new File(dir, ".git");
            if (gitDir.exists() && gitDir.isDirectory()) {
                return dir;
            }
            dir = dir.getParentFile();
        }
        throw new IllegalStateException("Git root directory not found from: " + currentDir.getAbsolutePath());
    }

    public static Path getGitRoot() {
        return findGitRoot(new File(System.getProperty("user.dir"))).toPath();
    }
}
