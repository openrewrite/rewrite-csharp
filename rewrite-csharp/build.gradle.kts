import java.io.ByteArrayOutputStream

plugins {
    id("org.openrewrite.build.language-library")
    id("org.openrewrite.build.moderne-source-available-license") version "latest.release"
    id("com.netflix.nebula.release")
    id("com.netflix.nebula.maven-nebula-publish")
}

val latest = if (System.getenv("RELEASE_PUBLICATION") != null) "latest.release" else "latest.integration"

dependencies {
    // The bom version can also be set to a specific version
    // https://github.com/openrewrite/rewrite-recipe-bom/releases
    implementation(platform("org.openrewrite:rewrite-bom:${latest}"))

    implementation("org.openrewrite:rewrite-core")
    implementation("org.openrewrite:rewrite-java")

    implementation("io.micrometer:micrometer-core:latest.release")
    implementation("com.fasterxml.jackson.core:jackson-core")
    implementation("com.fasterxml.jackson.dataformat:jackson-dataformat-cbor")

    // Need to have a slf4j binding to see any output enabled from the parser.
    runtimeOnly("ch.qos.logback:logback-classic:1.2.+")
    testImplementation("org.openrewrite:rewrite-test")
    testImplementation("org.openrewrite:rewrite-xml")

    testRuntimeOnly("org.openrewrite:rewrite-java-17")
}

publishing {
    repositories {
        maven {
            name = "ModerneArtifactory"
            url = uri("https://artifactory.moderne.ninja/artifactory/moderne-recipes")
            credentials {
                username = (project.findProperty("moderne.artifactory.username") as String?) ?: System.getenv("MODERNE_ARTIFACTORY_USERNAME")
                password = (project.findProperty("moderne.artifactory.password") as String?) ?: System.getenv("MODERNE_ARTIFACTORY_PASSWORD")
            }
        }
    }
}
tasks.withType<JavaCompile>().configureEach {
//    options.compilerArgs.add("-Xlint:unchecked")
    options.compilerArgs.add("-nowarn")
}
tasks.withType<Javadoc>().configureEach {
    (options as StandardJavadocDocletOptions).apply {
        exclude("org/openrewrite/csharp/tree/Cs.java")
    }
}
// Copy DotnetServer.zip to both main and test resources
tasks.processResources {
    from(rootProject.file("artifacts/DotnetServer.zip"))
}

tasks.processTestResources {
    from(rootProject.file("artifacts/DotnetServer.zip"))
}

// Make sure IntelliJ includes the resources in the runtime classpath
//sourceSets {
//    main {
//        resources {
//            srcDir("src/main/resources")
//            // Also include the artifacts directory for IDE runtime
//            srcDir(rootProject.file("artifacts"))
//        }
//    }
//    test {
//        resources {
//            srcDir("src/test/resources")
//            // Also include the artifacts directory for IDE test runtime
//            srcDir(rootProject.file("artifacts"))
//
//        }
//    }
//}
// Run external CLI and capture stdout lazily

val captureReleaseVersion by tasks.registering {

    doFirst {
        val version = providers.exec {
            commandLine("nbgv", "get-version", "-v", "semver2")
        }.standardOutput.asText.get().trim()
        project.extensions.extraProperties.set("release.version", version)
        println("release.version = $version")
    }
}

tasks.register("printReleaseVersion") {
    dependsOn(captureReleaseVersion)

    doLast {
        println("release.version = ${project.findProperty("release.version")}")
    }
}
