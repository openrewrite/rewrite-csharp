using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    [Solution(GenerateProjects = true)] Solution Solution;
    [NerdbankGitVersioning] NerdbankGitVersioning Version;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

    Target Pack => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetPack(x => x
                .SetProject(Solution.Path)
                .EnableIncludeSymbols()
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetVersion(Version.NuGetPackageVersion)
                .SetOutputDirectory(ArtifactsDirectory));
        });

    Target DownloadTestFixtures => _ => _
        .Executes(() =>
        {
            var fixturesDirectory = Solution.Directory / "tests" / "fixtures";
            var fixturesFile = fixturesDirectory / "fixtures.txt";
            var fixtures = fixturesFile.ReadAllLines()
                .Select(url => new 
                {
                    Name = url.Split('/').Last(x => !string.IsNullOrEmpty(x)).Replace(".git",""),
                    Url = url 
                })
                .ToList();
            foreach (var fixture in fixtures)
            {
                var fixtureDirectory = fixturesDirectory / fixture.Name;
                if (fixtureDirectory.Exists())
                {
                    GitTasks.Git("pull", fixtureDirectory);
                }
                else
                {
                    GitTasks.Git($"clone --depth 1 {fixture.Url}", fixturesDirectory);
                }
            }
        });
}
