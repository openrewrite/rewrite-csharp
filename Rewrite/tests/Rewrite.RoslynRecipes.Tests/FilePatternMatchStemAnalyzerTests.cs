using Microsoft.CodeAnalysis.CSharp.Testing;
using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.FilePatternMatchStemAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class FilePatternMatchStemAnalyzerTests
{

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch constructor is called with null as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithNullStem_CreatesErrorDiagnostic()
    {
        string text = $$"""


            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch constructor is called with a null literal as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithNullLiteralStem_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("some/path.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch constructor is called with default literal as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithDefaultLiteralStem_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:default|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch constructor is called with default(string) as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithDefaultStringStem_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:default(string)|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when FilePatternMatch constructor is called with a valid string as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithValidStem_NoDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", "stem");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when FilePatternMatch constructor is called with an empty string as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithEmptyStringStem_NoDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", "");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when FilePatternMatch constructor is called with a variable as the stem parameter.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithVariableStem_NoDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    string stem = "my-stem";
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", stem);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is used in a field initializer.
    /// </summary>
    [Test]
    public async Task FilePatternMatchInFieldInitializer_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                private Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch _match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", {|ORNETX0005:null|});
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that multiple diagnostics are created when multiple FilePatternMatch instances are created with null stem.
    /// </summary>
    [Test]
    public async Task MultipleFilePatternMatchWithNull_CreatesMultipleDiagnostics()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match1 = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path1.txt", {|ORNETX0005:null|});
                    var match2 = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path2.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is used in a return statement.
    /// </summary>
    [Test]
    public async Task FilePatternMatchInReturnStatement_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch GetMatch()
                {
                    return new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is passed as a method argument.
    /// </summary>
    [Test]
    public async Task FilePatternMatchPassedToMethod_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    ProcessMatch(new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", {|ORNETX0005:null|}));
                }

                void ProcessMatch(Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch match)
                {
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when a custom FilePatternMatch type (not from Microsoft.Extensions.FileSystemGlobbing) is used.
    /// </summary>
    [Test]
    public async Task NonFilePatternMatchType_NoDiagnostic()
    {
        const string text = """
            namespace MyNamespace
            {
                class FilePatternMatch
                {
                    public FilePatternMatch(string path, string? stem) { }
                }
            }

            class TestClass
            {
                void Method()
                {
                    var match = new MyNamespace.FilePatternMatch("path.txt", null);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is used in a collection initializer.
    /// </summary>
    [Test]
    public async Task FilePatternMatchInCollectionInitializer_CreatesErrorDiagnostic()
    {
        string text = $$"""
            using System.Collections.Generic;

            class TestClass
            {
                void Method()
                {
                    var matches = new List<Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch>
                    {
                        new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path1.txt", {|ORNETX0005:null|}),
                        new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path2.txt", "stem")
                    };
                }
            }
            
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is created using named arguments.
    /// </summary>
    [Test]
    public async Task FilePatternMatchWithNamedArguments_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch(path: "file.txt", stem: {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when FilePatternMatch with null stem is created in a ternary expression.
    /// </summary>
    [Test]
    public async Task FilePatternMatchInTernaryExpression_CreatesErrorDiagnostic()
    {
        string text = $$"""

            class TestClass
            {
                void Method(bool condition)
                {
                    var match = condition
                        ? new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", "stem")
                        : new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("other.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90.AddPackage("Microsoft.Extensions.FileSystemGlobbing")).ConfigureAwait(false);
    }
}
