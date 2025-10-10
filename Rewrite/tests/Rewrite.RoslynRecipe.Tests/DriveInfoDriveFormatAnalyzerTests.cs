
using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.DriveInfoDriveFormatAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class DriveInfoDriveFormatAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is created when the DriveInfo.DriveFormat property is accessed.
    /// </summary>
    [Test]
    public async Task DriveFormatMemberAccess_WhenSemanticMatch_CreatesDiagnostic()
    {
        const string text = """
            using System.IO;
            class A
            {
                void M()
                {
                    var x = DriveInfo.GetDrives()[0].{|ORNETX0001:DriveFormat|};
                }
            }
            """;
        
        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }
    
}