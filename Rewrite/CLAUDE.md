# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this directory.

## Project Overview

The rewrite-csharp project is an OpenRewrite language module for C# that enables automated code refactoring and analysis. It uses a hybrid architecture combining Java and C# components, with a remoting mechanism for cross-runtime communication.

## High-Level Architecture

The project consists of two main parts:

1. **C# Components** (.NET 9.0/MSBuild-based):
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

## Development Guidelines

### Roslyn Analyzer Development

#### Placement
- **All Roslyn analyzers and code fixes should be placed in the `Rewrite.RoslynRecipe` project**
- This project contains the Roslyn-based recipes that leverage Microsoft.CodeAnalysis APIs
- Analyzers should follow the naming convention: `[Category][Action]Analyzer.cs` (e.g., `NamingConventionAnalyzer.cs`)
- Associated code fix providers should be named: `[Category][Action]CodeFixProvider.cs`

#### Analyzer / Code fixup implementation
- Where possible, perform "low cost" syntax elimination first, and confirm correct targeting with semantic analysis
- Use `GetDocumentationCommentId` on symbols to determine symbol signature equality. See `WithOpenApiDeprecatedAnalyzer` for example on how to write analyzers targeting specific APIs for refactoring.

#### Test Requirements
- **Always generate comprehensive tests for analyzers unless explicitly instructed otherwise**

- Tests should be placed in the corresponding test project (`Rewrite.RoslynRecipe.Tests`)

- Include test cases for:
  - Positive scenarios (where the analyzer should report diagnostics)
  - Negative scenarios (where the analyzer should NOT report diagnostics)
  - When refactoring targets a delegate, tests cases should be generated to ensure correct refactoring when delegate is:
    - Expression Lambda
    - Statement Lambda
    - Method Group
    - If lambda returns any variation of `Task`, that it works in both scenarios if `async` keyword present or not 

- Use the Roslyn testing framework with verify methods for consistent test structure

- Special pattern for adding test references (such as asp.net core and nuget dependencies). See `WithOpenApiDeprecatedAnalyzerTests` and `WithOpenApiDeprecatedCodeFixTests` as an example

- Test files should follow the naming pattern: `[AnalyzerName]Tests.cs`

- Include test IntelliSense summary outlining the scenario being tested for this test method that makes it unique from other test methods in the test class

- If targeting a method that has multiple overloads in scenarios where multiple overloads are potential targets for refactoring, independent test method should be generated for each overload. 

- Code samples used in as input for test cases must contain minimal code necessary to test the scenario. Avoid adding unnecessary function calls, declarations, etc that are not explicitly being used to test a given scenario. Prefer using top level programs where possible. The most concise API is preferred. Example of concise code:
  ```
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.OpenApi;
  using System.Threading.Tasks;
  
  var builder = WebApplication.CreateBuilder();
  var app = builder.Build();
  
  app.MapGet("/", () => "test")
     .AddOpenApiOperationTransformer((operation, context, ct) =>
     {
         return Task.CompletedTask;
     });
  ```

- After generating test code, ensure the code compiles and tests pass. Iterate on fixing code/tests until it passes or you feel you've spent too much time on trying to resolve problem in same area and it's still not working. In that case stop, and ask for human help.

- Any syntax elements created by using `SyntaxFactory` parse helper methods must has `.DiscardFormatting()` called on them to ensure that they are subjected to full formatting.

- Any new elements should be annotated with `.WithAdditionalAnnotations(Formatter.Annotation))`. After all the refactoring is completed, a call should be made on the document to format sections of code that were affected like this: `finalDocument = await Formatter.FormatAsync(finalDocument, Formatter.Annotation, cancellationToken: cancellationToken);`



### API Documentation Standards

#### IntelliSense XML Documentation
**All public APIs must include complete XML documentation comments** with the following components:

1. **Summary Section** (`<summary>`):
   - Provide a clear, concise description of what the member does
   - Start with an action verb for methods (e.g., "Analyzes...", "Converts...", "Creates...")
   - Be specific about the purpose and behavior

2. **Parameter Documentation** (`<param>`):
   - Document every parameter with its purpose and expected values
   - Specify if null values are acceptable
   - Mention any constraints or validation rules
   - Example: `<param name="node">The syntax node to analyze. Must not be null.</param>`

3. **Return Value Documentation** (`<returns>`):
   - Describe what the method returns and under what conditions
   - Specify if null can be returned and when
   - Example: `<returns>A diagnostic descriptor if an issue is found; otherwise, null.</returns>`

4. **Exception Documentation** (`<exception>`):
   - Document all exceptions that can be thrown by the method
   - Include the conditions that cause each exception
   - Example: `<exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>`

5. **Additional Documentation Elements** (when applicable):
   - `<remarks>` - For additional important information or usage notes
   - `<example>` - For code examples showing proper usage
   - `<seealso>` - For related types or members
   - `<typeparam>` - For generic type parameters

#### Example Documentation Template

```csharp
/// <summary>
/// Analyzes C# code for naming convention violations and provides automated fixes.
/// </summary>
/// <remarks>
/// This analyzer checks for PascalCase, camelCase, and UPPER_SNAKE_CASE conventions
/// based on the member type and accessibility.
/// </remarks>
public class NamingConventionAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Registers analysis actions for the specified compilation context.
    /// </summary>
    /// <param name="context">The analysis context to register actions with. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public override void Initialize(AnalysisContext context)
    {
        // Implementation
    }

    /// <summary>
    /// Analyzes a symbol for naming convention violations.
    /// </summary>
    /// <param name="symbol">The symbol to analyze. Must not be null.</param>
    /// <param name="reportDiagnostic">The delegate to report diagnostics. Must not be null.</param>
    /// <returns>True if a violation was found and reported; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="symbol"/> or <paramref name="reportDiagnostic"/> is null.
    /// </exception>
    private bool AnalyzeSymbol(ISymbol symbol, Action<Diagnostic> reportDiagnostic)
    {
        // Implementation
    }
}
```

### Code Generation Best Practices

1. **Consistency**: Follow existing patterns in the codebase for similar components
2. **Error Handling**: Include proper null checks and validation with meaningful error messages
3. **Performance**: Consider performance implications, especially for analyzers that run frequently
4. **Immutability**: Prefer immutable data structures where appropriate
5. **Async/Await**: Use async patterns consistently for I/O operations
6. **Logging**: Include appropriate logging for debugging and troubleshooting