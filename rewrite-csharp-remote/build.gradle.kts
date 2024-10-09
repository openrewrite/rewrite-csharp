plugins {
    id("org.openrewrite.build.language-library")
}

dependencies {

    compileOnly("com.google.auto.service:auto-service-annotations:1.1.1")
    annotationProcessor("com.google.auto.service:auto-service:1.1.1")

    // The bom version can also be set to a specific version
    // https://github.com/openrewrite/rewrite-recipe-bom/releases
    implementation(platform("org.openrewrite:rewrite-bom:latest.release"))

    implementation("org.openrewrite:rewrite-core")
    implementation("org.openrewrite:rewrite-java")

    implementation(project(":rewrite-csharp"))

    implementation("org.openrewrite:rewrite-remote:latest.release") {
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
