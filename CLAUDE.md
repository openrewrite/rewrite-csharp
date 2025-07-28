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
   - `rewrite-test-engine-remote` - Test engine for C# LST tests

2. **C# Components** (.NET 9.0/MSBuild-based):
   - `Rewrite.Core` - Core framework port to C# (AST/visitor patterns)
   - `Rewrite.CSharp` - C# language-specific implementations (parser, printer, visitor)
   - `Rewrite.Remote` - Remoting infrastructure for Java-C# communication
   - `Rewrite.Server` - Language server implementation
   - `Rewrite.Analyzers` - Source generators to reduce boilerplate code
   - `Rewrite.MSBuild` - MSBuild integration
   - `Rewrite.Rpc` - components for cross language communication and serialization of AST (between java and c#)
   - `Rewrite.Recipes` - Recipe implementations
   - Supporting modules for JSON, YAML, XML, Properties file parsing

The Java side communicates with the C# language server via JSON RPC over STDIO for recipe execution during CLI operations.



## Key Architectural Patterns

### AST/Visitor Pattern
The core framework uses Abstract Syntax Tree (AST) representation with visitor pattern for tree traversal and transformation. The LST (Lossless Semantic Tree) preserves all formatting and whitespace.

### Source Generators
Extensive use of C# source generators in `Rewrite.Analyzers` to generate boilerplate code for builders, visitors, and toString methods. When modifying analyzers, rebuild is required.

### Parser Architecture
Uses Roslyn (Microsoft.CodeAnalysis) for C# parsing, converting Roslyn syntax trees to OpenRewrite LST format while preserving semantic information via Roslyn's semantic model.

### Cross-Runtime Communication
JSONRPC remoting between Java CLI and C# language server with custom serialization protocol for efficient data transfer. The server runs as a separate process during CLI operations

## Important Notes

- When making changes, ensure compatibility with both Java and C# components
- The remoting mechanism is critical for cross-runtime communication
- Source generators require rebuilding analyzer projects when changed
- Integration with Java side requires both Gradle and .NET builds