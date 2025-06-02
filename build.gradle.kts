import groovy.json.JsonOutput
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

repositories {
    mavenCentral()
    maven {
        name = "ModerneArtifactoryCache3"
        url = uri("https://artifactory.moderne.ninja/artifactory/moderne-cache-3")
        credentials {
            username = (project.findProperty("moderne.artifactory.username") as String?) ?: System.getenv("MODERNE_ARTIFACTORY_USERNAME")
            password = (project.findProperty("moderne.artifactory.password") as String?) ?: System.getenv("MODERNE_ARTIFACTORY_PASSWORD")
        }
    }
}

tasks.register("getTasks") {
    group = "custom"
    description = "Outputs all tasks as formatted JSON into a file"

    doLast {
        val taskList = project.tasks.map {
            mapOf(
                "name" to it.name,
                "description" to (it.description ?: ""),
                "group" to (it.group ?: "")
            )
        }

        val json = JsonOutput.prettyPrint(JsonOutput.toJson(taskList))
        File("${project.projectDir}/_build/.gradle-tasks.json").writeText(json)
        println(json)
    }
}
tasks.withType<JavaCompile>().configureEach {
    options.compilerArgs.add("-Xlint:unchecked")
}
