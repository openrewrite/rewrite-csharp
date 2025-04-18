plugins {
    id("org.openrewrite.build.language-library")
    id("org.openrewrite.build.moderne-source-available-license") version "latest.release"
}


var latest = if (System.getenv("RELEASE_PUBLICATION") != null) "latest.release" else "latest.integration"
latest = "latest.release"
dependencies {

    compileOnly("com.google.auto.service:auto-service-annotations:1.1.1")
    annotationProcessor("com.google.auto.service:auto-service:1.1.1")

    // The bom version can also be set to a specific version
    // https://github.com/openrewrite/rewrite-recipe-bom/releases
    implementation(platform("org.openrewrite:rewrite-bom:${latest}"))

    implementation("org.openrewrite:rewrite-core")
    implementation("org.openrewrite:rewrite-java")

    implementation(project(":rewrite-csharp"))
    implementation(project(":rewrite-csharp-remote"))

    implementation("org.openrewrite:rewrite-remote:${latest}") {
        exclude(group = "org.openrewrite", module = "rewrite-remote-java")
    }

    implementation("io.micrometer:micrometer-core:latest.release")
    implementation("com.fasterxml.jackson.core:jackson-core")
    implementation("com.fasterxml.jackson.dataformat:jackson-dataformat-cbor")

    // Need to have a slf4j binding to see any output enabled from the parser.
    runtimeOnly("ch.qos.logback:logback-classic:1.2.+")
    testImplementation("org.openrewrite:rewrite-test")

    testRuntimeOnly("org.openrewrite:rewrite-java-17")
}


//tasks.register<Zip>("zipDotnetServer") {
//    archiveFileName.set("DotnetServer.zip")
//    destinationDirectory.set(layout.buildDirectory.dir("tmp"))
//    from(rootProject.file("Rewrite/src/Rewrite.Server/bin/Release/net8.0/publish"))
//    include("*")
//    include("*/*") //to include contents of a folder present inside Reports directory
//}

tasks.processResources {
//    dependsOn("zipDotnetServer")
//    from(layout.buildDirectory.file("tmp/DotnetServer.zip"))
    from(layout.buildDirectory.file(rootProject.file("artifacts/DotnetServer.zip").absolutePath))
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
