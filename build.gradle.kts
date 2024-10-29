plugins {
    id("org.openrewrite.build.root") version("latest.release")
    id("org.openrewrite.build.java-base") version("latest.release")
}

allprojects {
    group = "org.openrewrite"
    description = "OpenRewrite C# language module."
    repositories {
        mavenCentral()
    }
}
