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
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using LibGit2Sharp;
using Nuke.Common.CI.GitHubActions;

// ReSharper disable UnusedMember.Local
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);


    GitHubActions GitHubActions => GitHubActions.Instance;

    [Parameter("ApiKey for the specified source")][Secret] readonly string NugetApiKey;
    [Parameter] readonly string NugetFeed = "https://api.nuget.org/v3/index.json";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    [Solution(GenerateProjects = true)] Solution Solution;
    [NerdbankGitVersioning] NerdbankGitVersioning Version;
    [GitRepositoryExt] LibGit2Sharp.Repository GitRepository;



    bool IsCurrentBranchCommitted() => !GitRepository.RetrieveStatus().IsDirty;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(c => c
                .SetProjectFile(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion));
        });

    Target Compile => _ => _
        .Description("Compiles .net projects")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(c => c
                .SetProjectFile(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion));
        });

    Target Pack => _ => _
        .Description("Creates nuget packages inside artifacts directory")
        .DependsOn(Restore, Compile)
        .Executes(() =>
        {
            DotNetPack(x => x
                .EnableNoBuild()
                .EnableNoRestore()
                .SetProject(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion)
                .SetOutputDirectory(ArtifactsDirectory));

        });

    Target PublishServer => _ => _
        .Description("Publishes server")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetPublish(c => c
                .SetProject(Solution.src.Rewrite_Server)
                .SetVersion(Version.NuGetPackageVersion));
        });

    Target Test => _ => _
        .Description("Runs .NET tests")
        .DependsOn(Restore)
        .Executes(() =>
        {
            var categoriesToExclude = new []
            {
                "KnownBug",
                "Exploratory",
                "Roslyn"
            };
            var filter = string.Join('&', categoriesToExclude.Select(x => $"Category!={x}"));
            DotNetTest(x => x
                .SetProjectFile(Solution.tests.Rewrite_CSharp_Tests)
                .SetFilter(filter)
                .SetResultsDirectory(ArtifactsDirectory / "test-results")
                .SetLoggers("trx")
            );
        });


    Target NugetPush => _ => _
        .Description("Publishes NuGet packages to Nuget.org")
        .Requires(() => NugetApiKey, () => NugetFeed)
        .OnlyWhenStatic(IsCurrentBranchCommitted)
        .DependsOn(Pack)
        .Executes(() =>
        {
            // this.GitRepository.Head.Tip.
            var artifacts = ArtifactsDirectory.GlobFiles("*.nupkg");
            if (artifacts.IsEmpty())
            {
                Assert.Fail("No packages were found in artifacts directory.");
            }
            NuGetTasks.NuGetPush(s => s
                    .SetSource(NugetFeed)
                    .SetApiKey(NugetApiKey)
                    .CombineWith(
                        artifacts, (cs, v) => cs
                            .SetTargetPath(v)),
                degreeOfParallelism: 3,
                completeOnFailure: true);
        });

    Target CIBuild => _ => _
        .DependsOn(PublishServer, Test);

    Target CIRelease => _ => _
        .DependsOn(Pack, NugetPush);

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

    Target GradleAssemble => _ => _
        .Executes(() =>
        {
            ProcessTasks.StartProcess(RootDirectory / "gradlew.bat", "assemble", RootDirectory).AssertWaitForExit();
        });


    string[] GetTagsForCurrentCheckout()
    {
        var headCommit = GitRepository.Head.Tip;
        var tagsPointingAtHead = GitRepository.Tags
            .Where(tag =>
            {
                var targetCommit = (tag.Target as Commit) ?? (tag.Target as TagAnnotation)?.Target as Commit;
                return targetCommit != null && targetCommit.Sha == headCommit.Sha;
            })
            .Select(x => x.FriendlyName)
            .ToArray();
        return tagsPointingAtHead;
    }

}
