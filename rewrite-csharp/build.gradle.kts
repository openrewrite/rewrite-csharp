plugins {
    id("org.openrewrite.build.recipe-library") version "latest.release"
}

// Set as appropriate for your organization
group = "org.openrewrite"
description = "Rewrite C#."
version = "0.8.0-SNAPSHOT"

dependencies {
    // The bom version can also be set to a specific version
    // https://github.com/openrewrite/rewrite-recipe-bom/releases
    implementation(platform("org.openrewrite:rewrite-bom:latest.integration"))

    implementation("org.openrewrite:rewrite-core")
    implementation("org.openrewrite:rewrite-java")

    implementation("io.micrometer:micrometer-core:latest.release")
    implementation("com.fasterxml.jackson.core:jackson-core")
    implementation("com.fasterxml.jackson.dataformat:jackson-dataformat-cbor")

    // Need to have a slf4j binding to see any output enabled from the parser.
    runtimeOnly("ch.qos.logback:logback-classic:1.2.+")

    testRuntimeOnly("org.openrewrite:rewrite-java-17")
}

repositories {
    mavenCentral()
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
                    name.set("Creative Commons Attribution-NonCommercial 4.0")
                    url.set("https://creativecommons.org/licenses/by-nc-nd/4.0/deed.en")
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
                connection.set("scm:git:git://github.com/openrewrite/rewrite-csharp.git")
                developerConnection.set("scm:git:ssh://github.com:openrewrite/rewrite-csharp.git")
                url.set("https://github.com/openrewrite/rewrite-csharp/tree/main")
            }
        }
    }
}
