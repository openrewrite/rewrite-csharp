# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

The rewrite-csharp project is an OpenRewrite language module for C# that enables automated code refactoring and analysis. It uses a hybrid architecture combining Java and C# components, with a remoting mechanism for cross-runtime communication.

## High-Level Architecture

The project consists of two main parts:

1. **Java Components** (Gradle-based):
   - `rewrite-csharp` - Core C# LST (Lossless Semantic Tree) classes
   - `rewrite-csharp-remote` - Serializers/deserializers for cross-process communication
   - `rewrite-csharp-remote-server` - Embedded C# TCP server (zipped DLLs)

2. **C# Components** (.NET/MSBuild-based):
   - `Rewrite.Core` - Core framework port to C#
   - `Rewrite.CSharp` - C# language-specific implementations
   - `Rewrite.Remote` - Remoting infrastructure
   - `Rewrite.Server` - Language server implementation
   - `Rewrite.Analyzers` - Source generators and analyzers
   - `Rewrite.MSBuild` - MSBuild integration

The Java side communicates with the C# language server via TCP for recipe execution during CLI operations.

## Build Commands

The project uses NUKE build system for C# and Gradle for Java components.

### Basic Build Commands
```bash
# Clean build artifacts
./build.ps1 Clean

# Restore NuGet packages
./build.ps1 Restore

# Compile the project
./build.ps1 Compile

# Build and package
./build.ps1 Pack
```

### Test Commands
```bash
# Run all .NET tests
./build.ps1 Test

# Run specific test project
dotnet test Rewrite/tests/Rewrite.CSharp.Tests

# Run a single test
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"

# Download test fixtures (required for integration tests)
./build.ps1 DownloadTestFixtures
```

### Java/Gradle Commands
```bash
# Run Java tests
./gradlew :rewrite-csharp:test

# Build Java components
./gradlew :rewrite-csharp:assemble

# Run both build and test
./build.ps1 GradleAssembleAndTest
```

### Publishing Commands
```bash
# Publish the language server
./build.ps1 PublishServer

# Create NuGet packages
./build.ps1 Pack

# Push to NuGet (requires API key)
./build.ps1 NugetPush --nuget-api-key YOUR_KEY
```

## Development Workflow

1. **Before Running Tests**: Download test fixtures using `./build.ps1 DownloadTestFixtures`
2. **Clean Build**: Use `./build.ps1 Clean` to remove all artifacts and bin/obj directories
3. **Stop Server**: Use `./build.ps1 StopServer` to kill any running Rewrite.Server instances

## Test Infrastructure

- Test results are output to `artifacts/test-results/` in TRX format
- The project includes integration tests using real codebases (e.g., Bitwarden)
- Test fixtures are defined in `Rewrite/tests/fixtures/fixtures.txt`

## Important Notes

- The project supports both .NET 8.0.x and 9.0.x
- When making changes, ensure compatibility with both Java and C# components
- The remoting mechanism is critical for cross-runtime communication
- Source generators are used extensively to reduce boilerplate code