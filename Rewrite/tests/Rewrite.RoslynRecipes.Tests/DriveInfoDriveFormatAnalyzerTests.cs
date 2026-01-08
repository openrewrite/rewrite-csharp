
using Microsoft.CodeAnalysis.CSharp.Testing;
using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.DriveInfoDriveFormatAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

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
        
        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
    
}