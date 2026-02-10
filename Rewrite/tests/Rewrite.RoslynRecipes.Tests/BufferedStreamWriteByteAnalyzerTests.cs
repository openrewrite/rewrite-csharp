using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.BufferedStreamWriteByteAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class BufferedStreamWriteByteAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream instance.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStream_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            var stream = new BufferedStream(new MemoryStream());
            stream.{|ORNETX0012:WriteByte|}(1);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream
    /// constructed with an explicit buffer size.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStreamWithBufferSize_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            var stream = new BufferedStream(new MemoryStream(), bufferSize: 4);
            stream.{|ORNETX0012:WriteByte|}(1);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream
    /// passed as a method parameter.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStreamParameter_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                void M(BufferedStream stream)
                {
                    stream.{|ORNETX0012:WriteByte|}(0xFF);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream
    /// stored as a field.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStreamField_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                private BufferedStream _stream = new BufferedStream(new MemoryStream());

                void M()
                {
                    _stream.{|ORNETX0012:WriteByte|}(42);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for multiple WriteByte calls on the same BufferedStream.
    /// </summary>
    [Test]
    public async Task WriteByte_MultipleCalls_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System.IO;

            var stream = new BufferedStream(new MemoryStream(), bufferSize: 4);
            stream.{|ORNETX0012:WriteByte|}(1);
            stream.{|ORNETX0012:WriteByte|}(2);
            stream.{|ORNETX0012:WriteByte|}(3);
            stream.{|ORNETX0012:WriteByte|}(4);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream
    /// returned from a method.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStreamReturnedFromMethod_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                BufferedStream GetStream() => new BufferedStream(new MemoryStream());

                void M()
                {
                    GetStream().{|ORNETX0012:WriteByte|}(1);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when WriteByte is called on a plain MemoryStream,
    /// which is not a BufferedStream.
    /// </summary>
    [Test]
    public async Task WriteByte_OnMemoryStream_NoDiagnostic()
    {
        const string text = """
            using System.IO;

            var stream = new MemoryStream();
            stream.WriteByte(1);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when WriteByte is called on a FileStream,
    /// which is not a BufferedStream.
    /// </summary>
    [Test]
    public async Task WriteByte_OnFileStream_NoDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                void M(FileStream stream)
                {
                    stream.WriteByte(1);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when WriteByte is called on a variable typed as
    /// the base Stream class, even if it could be a BufferedStream at runtime.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBaseStreamType_NoDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                void M(Stream stream)
                {
                    stream.WriteByte(1);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when a non-Stream class has its own WriteByte method.
    /// </summary>
    [Test]
    public async Task WriteByte_OnCustomClass_NoDiagnostic()
    {
        const string text = """
            class MyWriter
            {
                public void WriteByte(byte b) { }
            }

            class A
            {
                void M()
                {
                    var writer = new MyWriter();
                    writer.WriteByte(1);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when Write (not WriteByte) is called on a BufferedStream.
    /// </summary>
    [Test]
    public async Task Write_OnBufferedStream_NoDiagnostic()
    {
        const string text = """
            using System.IO;

            var stream = new BufferedStream(new MemoryStream());
            stream.Write(new byte[] { 1, 2, 3 }, 0, 3);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when WriteByte is called on a BufferedStream
    /// typed as a local variable assigned from a property.
    /// </summary>
    [Test]
    public async Task WriteByte_OnBufferedStreamProperty_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;

            class A
            {
                BufferedStream Stream { get; } = new BufferedStream(new MemoryStream());

                void M()
                {
                    Stream.{|ORNETX0012:WriteByte|}(1);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
