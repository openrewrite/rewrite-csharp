plugins {
    id("org.openrewrite.build.language-library")
}

tasks.register<Zip>("zipDotnetServer") {
    archiveFileName.set("DotnetServer.zip")
    destinationDirectory.set(layout.buildDirectory.dir("tmp"))
    from(rootProject.file("Rewrite/src/Rewrite.Server/bin/Release/net8.0/publish"))
    include("*")
    include("*/*") //to include contents of a folder present inside Reports directory
}

tasks.processResources {
    dependsOn("zipDotnetServer")
    from(layout.buildDirectory.file("tmp/DotnetServer.zip"))
}

tasks.compileJava {
    options.release = 8
}

// We don't care about publishing javadocs anywhere, so don't waste time building them
tasks.withType<Javadoc>().configureEach {
    enabled = false
}

tasks.named<Jar>("sourcesJar") {
    enabled = false
}

tasks.named<Jar>("javadocJar") {
    enabled = false
}

val emptySourceJar = tasks.create<Jar>("emptySourceJar") {
    file("README.md")
    archiveClassifier.set("sources")
}

val emptyJavadocJar = tasks.create<Jar>("emptyJavadocJar") {
    file("README.md")
    archiveClassifier.set("javadoc")
}

publishing {
    publications.named<MavenPublication>("nebula") {
        artifactId = project.name
        description = project.description

        artifacts.clear() // remove the regular JAR
        // Empty JARs are OK: https://central.sonatype.org/publish/requirements/#supply-javadoc-and-sources
        artifact(tasks.named("jar"))
        artifact(emptySourceJar)
        artifact(emptyJavadocJar)

        pom {
            name.set(project.name)
            description.set(project.description)
            url.set("https://moderne.io")
            licenses {
                license {
                    name.set("Moderne, Inc. Commercial License")
                    url.set("https://docs.moderne.io/administrator-documentation/references/licensing")
                }
            }
            developers {
                developer {
                    name.set("Team Moderne")
                    email.set("support@moderne.io")
                    organization.set("Moderne, Inc.")
                    organizationUrl.set("https://moderne.io")
                }
            }
            scm {
                connection.set("scm:git:git://github.com/moderneinc/rewrite-remote.git")
                developerConnection.set("scm:git:ssh://github.com:moderneinc/rewrite-remote.git")
                url.set("https://github.com/moderneinc/rewrite-remote/tree/main")
            }
        }
    }
}
