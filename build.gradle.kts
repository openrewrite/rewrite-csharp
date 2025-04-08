plugins {
    id("org.openrewrite.build.root") version("latest.release")
    id("org.openrewrite.build.java-base") version("latest.release")
    id("org.owasp.dependencycheck") version "latest.release"
    id("org.openrewrite.build.moderne-source-available-license") version "latest.release"
}

allprojects {
    group = "org.openrewrite"
    description = "OpenRewrite C# language module."
}
