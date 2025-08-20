using NMica.Utils.IO;
using Nuke.Common;
using Nuke.Common.Tooling;
using Rewrite.Tests;
using TUnit.Assertions;
using TUnit.Core;

namespace Rewrite.MSBuild.Tests;

public class IntegrationTests : BaseTests
{
    [Test]
    [Explicit]
    public void RunRoslynRecipeFromExecutable()
    {
        var content = """
            class A
            {
                string Test() => string.Join(" ", new[] { "Hello", "world!" });
            }
            """;
        using var testProject = TestProject.CreateTemporaryLibrary(content);
        
        var serverExecutable = DirectoryHelper.RepositoryRoot / "Rewrite" / "src" / "Rewrite.Server" / "bin" / "Debug" / "net9.0" / "Rewrite.Server.dll";
        Fail.When(!File.Exists(serverExecutable), $"{serverExecutable} not found. Did you build the solution?");
        var recipeId = "CA1861"; // Avoid constant arrays as arguments
        var package = "Microsoft.CodeAnalysis.NetAnalyzers";
        var version = "9.0.0";
        ProcessTasks.StartProcess("dotnet", $"{serverExecutable} run-recipe --solution {testProject.SolutionFile} --id {recipeId} --package {package} --version {version}").AssertZeroExitCode();
        
    }
}