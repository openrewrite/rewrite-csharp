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
