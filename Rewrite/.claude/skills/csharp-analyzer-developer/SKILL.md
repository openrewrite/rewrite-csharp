---
name: csharp-analyzer-developer
description: Comprehensive guidance for creating Roslyn analyzers and code fix providers in the Rewrite.RoslynRecipe project. Use when developing new analyzers, code fixes, or when asked about analyzer implementation patterns, testing requirements, or best practices for C# code analysis.
allowed-tools: [Read, Write, Edit, Grep, Glob, Task, TodoWrite]
---

# C# Analyzer and Code Fix Developer

This skill provides comprehensive guidance for developing Roslyn analyzers and code fix providers in the OpenRewrite C# project.

## Project Structure and Placement

### File Organization
- **Analyzers**: Place in `src/Rewrite.RoslynRecipe/` project
- **Code Fix Providers**: Place in `src/Rewrite.RoslynRecipe/` project
- **Tests**: Place in `tests/Rewrite.RoslynRecipe.Tests/`
- **Helper Utilities**: Place in `src/Rewrite.RoslynRecipe/Helpers/`

### Naming Conventions
- Analyzers: `[Category][Action]Analyzer.cs` (e.g., `NamingConventionAnalyzer.cs`, `ActionContextAccessorObsoleteAnalyzer.cs`)
- Code Fix Providers: `[Category][Action]CodeFixProvider.cs` (e.g., `NamingConventionCodeFixProvider.cs`)
- Test Files: `[AnalyzerName]Tests.cs` and `[CodeFixProviderName]Tests.cs`
- If analyzer targets an interface (such as `IActionContextAccessor`), the analyzer name should drop the `I` from the prefix as that clashes with naming convention identying interfaces - an analyzer is not an interface. Instead, it should name it like `ActionContextAccessorObsoleteAnalyzer`.
- Analyzers shall define `DiagnosticId` starting with characters `OR` (meaning OpenRewrite developed recipes). The next set of characters shall identify the target framework or package that the diagnostic is targeting. For example for recipes that help identify / fix issues when migration .NET framework versions, it would include `NETX`. The `X` is the roman numeral of the version 10 of .NET implying that the recipes targets migration to version 10. Roman numerals are used here as to not clash with the last digits of diagnostic id which are sequentially imported. The last number are 4 digits incrementing number for that specific issue for the target version. Full example for analyzer targeting issues in .NET 10: `ORNETX0001`.  

## Analyzer Implementation Pattern

### Semantic Analysis Best Practices

Use Two-Phase Analysis to first eliminate anything that doesn't match on pure syntax, and ALWAYS confirm any potential matches via semantic analysis to remove any potential ambiguity (confirm namespace / type, etc). Use extension method `IsSymbolOneOf` (provided by `Rewrite.RoslynRecipe.Helpers.SemanticAnalysisUtil` class) to confirm semantic identity. The semantic identity uses string returned by `GetDocumentationCommentId()` on `ISymbol` to do the underlying symbol comparison. The extension method takes this signature: `public static bool IsSymbolOneOf(this SyntaxNode node, SemanticModel semanticModel, params IEnumerable<string> symbolNames)`. Example which targets method invocation `WithOpenApi` and it's 3 potential overloads:

```csharp
private void AnalyzeNode(SyntaxNodeAnalysisContext context)
{
    // Phase 1: Low-cost syntax elimination
    var invocation = (InvocationExpressionSyntax)context.Node;
    
    if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        return;
    
    if (memberAccess.Name.Identifier.Text != "WithOpenApi")
        return;

    // Phase 2: Semantic confirmation
    if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
            "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1",
            "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1(System.Func{Microsoft.OpenApi.OpenApiOperation,Microsoft.OpenApi.OpenApiOperation})",
            "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1(System.Func{Microsoft.OpenApi.Models.OpenApiOperation,Microsoft.OpenApi.Models.OpenApiOperation})"
        ))
        return;

    // Report diagnostic
    var diagnostic = Diagnostic.Create(Rule, node.GetLocation());
    context.ReportDiagnostic(diagnostic);
}
```

## Code Fix Provider Implementation Pattern

1. All editing of the syntax tree is to be done using `DocumentEditor`, which provides a simplified and robust way to make multiple edits to the document and access to the original semantic model. Remember that semantic model is tied to the original syntax, so all decision need to be made on the original nodes, not any subsequent mutations.

1. If the codefix only targets the immediate code marked by the analyzer, semantic confirmation is not required since it's already guaranteed to have been done in the analyzer. However, if the refactoring requires nodes outside of what was selected by the diagnostic (other areas of the document), semantic model should always be used to ensure accurate targeting.

1. When introducing new types from namespaces that have potential to not be imported in the current scope (normally via a `using Some.Namespace` statement), you must call `Rewrite.RoslynRecipe.Helpers.UsingsUtil.MaybeAddUsingAsync` to ensure that the correct using statement is added to the document if necessary.

   ```
   /// <summary>
   /// Adds a using directive for the specified type if it's not already available at the usage site.
   /// </summary>
   /// <param name="document">The document to potentially add the using directive to.</param>
   /// <param name="root">The syntax root of the document.</param>
   /// <param name="semanticModel">The semantic model for the document.</param>
   /// <param name="usageSite">The syntax node where the type will be used.</param>
   /// <param name="fullTypeName">The fully qualified type name (e.g., "System.Threading.Tasks.Task").</param>
   /// <param name="cancellationToken">The cancellation token for the operation.</param>
   /// <returns>A tuple containing the potentially updated document and root.</returns>
   public static async Task<(Document document, SyntaxNode root)> MaybeAddUsingAsync(
       Document document,
       SyntaxNode root,
       SemanticModel semanticModel,
       SyntaxNode usageSite,
       string fullTypeName,
       CancellationToken cancellationToken)
   ```

   When a type is being removed from usage, call `MaybeRemoveUsingAsync` to clean up any potential unused `using` statements.

1. Except for very simple scenarios, when creating new syntax elements, prefer usage of SyntaxFactory.ParseXXXX methods vs constructing complex trees using object API. If a new fragment was created using Parse, ensure to call  `DiscardFormatting()` extension method (from `Rewrite.RoslynRecipe.Helpers.ElasticizeAllTokensRewriter` class) to ensure that it's all replaced with elastic trivia. 

1. We need to ensure new code is properly formatted but not try to format existing user code. To do this, whenever doing any operation on the `DocumentEditor` (ex. replace/insert), mark the new node with `.WithAdditionalAnnotations(Formatter.Annotation)`. After all edits are done, format only edited blocks via call like this: 
```
await Formatter.FormatAsync(newDocument, Formatter.Annotation,                  cancellationToken: cancellationToken);
```

1. If the formatter is invoked on the document and the test case fails due to formatting issues, adjust the "expected" code to match what the formatter produces. 

## Testing Requirements

### 1. Test Structure
Every analyzer and code fix MUST have comprehensive tests including:

#### For Analyzers:
- **Positive scenarios**: Where diagnostics should be reported
- **Negative scenarios**: Where diagnostics should NOT be reported

#### For Code Fixes:
- **Basic transformations**: Simple cases
- **Syntax permutations**: When multiple ways to write the same thing exists, test each permutation (ex. delegates may take form of a simple lambda expression, a block with multiple statements, or a method group)

### 2. Test Implementation Pattern

#### Analyzer Tests
```csharp
using Microsoft.CodeAnalysis.Testing;
using Rewrite.RoslynRecipe.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<
    Rewrite.RoslynRecipe.[Name]Analyzer>;

namespace Rewrite.RoslynRecipe.Tests
{
    public class [Name]AnalyzerTests
    {
        /// <summary>
        /// Tests that [specific scenario] triggers diagnostic.
        /// </summary>
        [Test]
        public async Task [Scenario]_TriggersDiagnostic()
        {
            const string source = """
                // Code with issue
                {|ORNETX[Number]:ProblemCode|}
                """;

            await Verifier.VerifyAnalyzerAsync(source,
                Assemblies.AspNet100 // Or appropriate reference assemblies
                    .AddPackage("PackageName", "Version"))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Tests that [valid scenario] does not trigger diagnostic.
        /// </summary>
        [Test]
        public async Task [ValidScenario]_NoDiagnostic()
        {
            const string source = """
                // Valid code
                ValidCode
                """;

            await Verifier.VerifyAnalyzerAsync(source,
                Assemblies.AspNet100)
                .ConfigureAwait(false);
        }
    }
}
```

#### Code Fix Tests

```csharp
using Microsoft.CodeAnalysis.Testing;
using Rewrite.RoslynRecipe.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipe.[Name]Analyzer,
    Rewrite.RoslynRecipe.[Name]CodeFixProvider>;

namespace Rewrite.RoslynRecipe.Tests
{
    public class [Name]CodeFixTests
    {
        /// <summary>
        /// [Tests that [specific transformation] is correctly applied.]
        /// </summary>
        [Test]
        public async Task [Scenario]_CorrectlyFixed()
        {
            const string source = """
                // Code before fix
                {|ORNETX[Number]:ProblemCode|}
                """;

            const string fixedSource = """
                // Code after fix
                FixedCode
                """;

            await Verifier.VerifyCodeFixAsync(source, fixedSource,
                Assemblies.AspNet100
                    .AddPackage("PackageName", "Version"))
                .ConfigureAwait(false);
        }
    }
}
```

- Note that if the package comes from a Microsoft package that is part of the same version scheme as the runtime, explicit version number can be ommited. For example `Assemblies.Net100.AddPackage("Microsoft.Extensions.Options")` the version will be set to `10.0.0` automatically and should be omitted. 
- Test code should target code and packages in the BEFORE state. For example if the recipe is intended to migrate code from .net 9 to 10, use `Net90` reference assemblies. 

### 3. Special Test Cases for Delegates

When refactoring delegates, test ALL variations:
- Expression lambdas: `x => x.Method()`
- Statement lambdas: `x => { return x.Method(); }`
- Method groups: `obj.Method`
- Async variations: `async x => await x.Method()`
- Non-async Task returns: `x => Task.FromResult(x.Method())`

### 4. Use Minimal Code in Tests
Keep test code concise:
```csharp
// GOOD: Minimal, focused test
const string source = """
    using Microsoft.AspNetCore.Builder;

    var app = WebApplication.Create();
    app.MapGet("/", () => "test")
       .{|ORNETX0010:ProblemMethod|}();
    """;

// BAD: Unnecessary complexity
const string source = """
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    namespace MyApp
    {
        public class Startup
        {
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", () => "test")
                        .{|ORNETX0010:ProblemMethod|}();
                });
            }
        }
    }
    """;
```

- Unless explicitly being tested for as part of the testcase, use top level statements when doing entrypoint code

  

## XML Documentation Requirements

ALL public APIs MUST include complete XML documentation:

```csharp
/// <summary>
/// Analyzes C# code for [specific issue] and reports diagnostics.
/// </summary>
/// <remarks>
/// This analyzer detects [detailed explanation of what it finds].
/// </remarks>
public class [Name]Analyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Registers analysis actions for [specific syntax kinds].
    /// </summary>
    /// <param name="context">The analysis context to register actions with. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public override void Initialize(AnalysisContext context)
    {
        // Implementation
    }
}
```



## Checklist for New Analyzer/Code Fix

- [ ] Analyzer placed in `src/Rewrite.RoslynRecipe/`
- [ ] Code fix provider in same project
- [ ] Follows naming conventions
- [ ] Uses semantic model when targeting types and their members
- [ ] Two-phase analysis (syntax then semantic)
- [ ] Uses IsSymbolOneOf extension method which uses `symbol.GetDocumentationCommentId` to determine symbol match
- [ ] Comprehensive test suite created
- [ ] Tests cover positive and negative cases
- [ ] Tests use minimal code samples
- [ ] XML documentation on all public members
- [ ] Using directives managed with UsingsUtil
- [ ] New syntax formatted with Formatter.Annotation
- [ ] DiscardFormatting() called on parsed syntax
- [ ] Document is formatted after transformation
- [ ] Test for all delegate variations (if applicable)
- [ ] Code compiles and all tests pass