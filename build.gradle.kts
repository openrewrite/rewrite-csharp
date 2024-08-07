plugins {
    id("org.openrewrite.build.root") version("latest.release")
    id("org.openrewrite.build.java-base") version("latest.release")
}


repositories {
    mavenLocal()
    mavenCentral()
}



allprojects {
// Set as appropriate for your organization
    group = "org.openrewrite"
    description = "Rewrite C#."
}

subprojects {

    repositories {
        mavenCentral()
    }
}
