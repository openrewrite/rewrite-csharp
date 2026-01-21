---
name: testing-with-tunit
description: Describes how to use TUnit testing framework properly
allowed-tools: [Read, Write, Edit, Grep, Glob, Task, TodoWrite, dotnet]
---

# Testing with TUnit

The solution uses TUnit as testing framework. It is based on Microsoft.Testing.Platform architecture that has some differences from the traditional VSTest mode. When invoking tests via `dotnet test` the following key things should apply:

- Running `dotnet test` on Microsoft.Testing.Platform, the additional `--` are not required when passing test framework specific parameters. Ex: `dotnet test -- --report-trx` should become `dotnet test --report-trx`.

- If passing a specific solution (or directory containing solution), for example, `dotnet test MySolution.sln`, this should become `dotnet test --solution MySolution.sln`.

- If passing a specific project (or directory containing project), for example, `dotnet test MyProject.csproj`, this should become `dotnet test --project MyProject.csproj`.

  If passing a specific dll, for example, `dotnet test path/to/UnitTests.dll`, this should become `dotnet test --test-modules path/to/UnitTests.dll`. Note that `--test-modules` also supports globbing.