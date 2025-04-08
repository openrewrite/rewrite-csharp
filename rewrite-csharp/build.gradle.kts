plugins {
    id("org.openrewrite.build.language-library")
    id("org.openrewrite.build.moderne-source-available-license") version "latest.release"
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

    testRuntimeOnly("org.openrewrite:rewrite-java-17")
}
