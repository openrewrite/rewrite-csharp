<p align="center">
  <a href="https://docs.openrewrite.org">
    <picture>
      <source media="(prefers-color-scheme: dark)" srcset="https://github.com/openrewrite/rewrite/raw/main/doc/logo-oss-dark.svg">
      <source media="(prefers-color-scheme: light)" srcset="https://github.com/openrewrite/rewrite/raw/main/doc/logo-oss-light.svg">
      <img alt="OpenRewrite Logo" src="https://github.com/openrewrite/rewrite/raw/main/doc/logo-oss-light.svg" width='600px'>
    </picture>
  </a>
</p>

<div align="center">
  <h1>rewrite-csharp</h1>
</div>

<div align="center">

<!-- Keep the gap above this line, otherwise they won't render correctly! -->
[![ci](https://github.com/openrewrite/rewrite-csharp/actions/workflows/ci.yml/badge.svg)](https://github.com/openrewrite/rewrite-csharp/actions/workflows/ci.yml)
[![Maven Central](https://img.shields.io/maven-central/v/org.openrewrite/rewrite-csharp.svg)](https://mvnrepository.com/artifact/org.openrewrite/rewrite-csharp)
[![Revved up by Gradle Enterprise](https://img.shields.io/badge/Revved%20up%20by-Gradle%20Enterprise-06A0CE?logo=Gradle&labelColor=02303A)](https://ge.openrewrite.org/scans)
[![Contributing Guide](https://img.shields.io/badge/Contributing-Guide-informational)](https://github.com/openrewrite/.github/blob/main/CONTRIBUTING.md)
</div>

## Introduction

This project contains a variety of OpenRewrite recipes, visitors, and supporting code for the C# language. Most of OpenRewrite, including the core framework, is Java-based, so a remoting mechanism is used to communicate between the Java and C# runtimes.

**Note**: For now, this language and the associated recipes are only supported via the [Moderne CLI](https://docs.moderne.io/user-documentation/moderne-cli/getting-started/cli-intro) or the [Moderne Platform](https://docs.moderne.io/user-documentation/moderne-platform/getting-started/running-your-first-recipe) (at least until native build tool support catches up). That being said, the Moderne CLI is free to use for open-source repositories. If your repository is closed-source, though, you will need to obtain a license to use the CLI or the Moderne Platform. [Please contact Moderne to learn more](https://www.moderne.ai/contact-us).

## Getting started

For help getting started with the Moderne CLI, check out our [getting started guide](https://docs.moderne.io/user-documentation/moderne-cli/getting-started/cli-intro). Or, if you'd like to try running these recipes in the Moderne Platform, check out the [Moderne Platform quickstart guide](https://docs.moderne.io/user-documentation/moderne-platform/getting-started/running-your-first-recipe).

## Contributing

We appreciate all types of contributions. See the [contributing guide](https://github.com/openrewrite/.github/blob/main/CONTRIBUTING.md) for detailed instructions on how to get started.

## Development information

### Project structure

* `Rewrite` - OpenRewrite port to C#
* `rewrite-csharp` - Module with pure C# related LST classes written in Java
* `rewrite-csharp-remoting` - Module with the generated C# serializers/deserializers classes required for cross-process communication
* `rewrite-test-engine-remote` - Test engine used to power the OpenRewrite LST test written in C#
* `rewrite-csharp-remote-server` - The embedded C# based tcp server zipped dlls required to run C# lang server during CLI executions (build/recipe run/ etc )

### Licensing

For more information about licensing, please visit our [licensing page](https://docs.openrewrite.org/licensing/openrewrite-licensing).
