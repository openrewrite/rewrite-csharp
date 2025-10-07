using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.FilePatternMatchStemAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class FilePatternMatchStemAnalyzerTests
{
    private const string FilePatternMatchStub = """
        namespace Microsoft.Extensions.FileSystemGlobbing
        {
            public struct FilePatternMatch
            {
                public FilePatternMatch(string path, string stem) { }
                public string Stem { get; }
            }
        }
        """;

    [Test]
    public async Task FilePatternMatchWithNullStem_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithNullLiteralStem_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("some/path.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithDefaultLiteralStem_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:default|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithDefaultStringStem_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", {|ORNETX0005:default(string)|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithValidStem_NoDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", "stem");
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithEmptyStringStem_NoDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", "");
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithVariableStem_NoDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    string stem = "my-stem";
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path/to/file.txt", stem);
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchInFieldInitializer_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                private Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch _match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", {|ORNETX0005:null|});
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task MultipleFilePatternMatchWithNull_CreatesMultipleDiagnostics()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match1 = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path1.txt", {|ORNETX0005:null|});
                    var match2 = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path2.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchInReturnStatement_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch GetMatch()
                {
                    return new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch("path.txt", {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchPassedToMethod_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

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

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task NonFilePatternMatchType_NoDiagnostic()
    {
        const string text = """
            namespace MyNamespace
            {
                class FilePatternMatch
                {
                    public FilePatternMatch(string path, string stem) { }
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

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchInCollectionInitializer_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

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

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchWithNamedArguments_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

            class TestClass
            {
                void Method()
                {
                    var match = new Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch(path: "file.txt", stem: {|ORNETX0005:null|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task FilePatternMatchInTernaryExpression_CreatesErrorDiagnostic()
    {
        string text = $$"""
            {{FilePatternMatchStub}}

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

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }
}
