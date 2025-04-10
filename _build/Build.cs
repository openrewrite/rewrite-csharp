using System;
using System.Collections.Generic;
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
using NuGet.Configuration;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.PowerShell;

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
    AbsolutePath TestResultsDirectory => ArtifactsDirectory /  "test-results";
    [Solution(GenerateProjects = true)] Solution Solution;
    [NerdbankGitVersioning] NerdbankGitVersioning Version;
    [GitRepositoryExt] LibGit2Sharp.Repository GitRepository;



    bool IsCurrentBranchCommitted() => !GitRepository.RetrieveStatus().IsDirty;

    Target Clean => _ => _
        .Description("Clean out artifacts and all the bin/obj directories")
        .DependsOn(StopServer, CleanNugetCache)
        .Before(Restore)
        .Executes(() =>
        {
            IEnumerable<AbsolutePath> GetSubDirectories(params string[] patterns)
            {
                var options = new EnumerationOptions() { RecurseSubdirectories = true };
                return patterns
                    .SelectMany(pattern => Directory.EnumerateDirectories(Solution.Directory!, pattern, options))
                    .Select(AbsolutePath.Create);
            }
            ArtifactsDirectory.CreateOrCleanDirectory();
            var objBin = GetSubDirectories("obj", "bin");
            foreach (var subDirectory in objBin)
            {
                subDirectory.DeleteDirectory();
            }


        });

    Target CleanTestResults => _ => _
        .Description("Clean out artifacts and all the bin/obj directories")
        .Executes(() =>
        {
            TestResultsDirectory.CreateOrCleanDirectory();
        });

    Target CleanNugetCache => _ => _
        .Description("Clean out artifacts and all the bin/obj directories")
        .DependsOn(StopServer)
        .Before(Restore)
        .Executes(() =>
        {
            // clean out rewrite nuget packages from global cache to ensure recent builds are properly used from artifacts folder
            var globalPackagesFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
            var rewritePackageNames = Solution.AllProjects.Where(x => x.Name.StartsWith("Rewrite")).Select(x => x.Name.ToLower());
            foreach (var rewritePackageName in rewritePackageNames)
            {
                var packagePath = globalPackagesFolder / rewritePackageName;
                packagePath.DeleteDirectory();
            }
        });

    Target Restore => _ => _
        .Description("Restores nuget packages")
        .Executes(() =>
        {

            DotNetRestore(c => c
                .SetProjectFile(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion));
        });

    Target Compile => _ => _
        .Description("Compiles .Net projects")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(c => c
                .SetProjectFile(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion));
        });

    Target Pack => _ => _
        .Description("Creates nuget packages inside artifacts directory")
        .DependsOn(Restore, PublishServer)
        .Executes(() =>
        {
            DotNetPack(x => x
                .EnableNoRestore()
                .SetProject(Solution.Path)
                .SetVersion(Version.NuGetPackageVersion)
                .SetAssemblyVersion(Version.AssemblyVersion)
                .SetOutputDirectory(ArtifactsDirectory));

            // publish recipes with a static version number so it can be referenced in tests
            DotNetPack(x => x
                .EnableNoRestore()
                .SetProject(Solution.src.Rewrite_Recipes)
                .SetVersion("0.0.1")
                .SetAssemblyVersion(Version.AssemblyVersion)
                .SetOutputDirectory(ArtifactsDirectory / "test"));


        });

    Target PublishServer => _ => _
        .Description("Publishes server")
        .DependsOn(Restore)
        .Before(Test)
        .Executes(() =>
        {
            DotNetPublish(c => c
                .SetProject(Solution.src.Rewrite_Server)
                .SetVersion(Version.NuGetPackageVersion));

            var publishDir = Solution.src.Rewrite_Server.Directory / "bin" / "Release" / "net8.0" / "publish";
            publishDir.ZipTo(ArtifactsDirectory / "DotnetServer.zip");
        });

    Target StopServer => _ => _
        .Description("Stops any instances of Rewrite.Server that may have not shutdown (such as when they were launched by gradle)")
        .Executes(() =>
        {
            var projectName = Solution.src.Rewrite_Server.Name;
            if (IsWin)
            {
                PowerShellTasks.PowerShell(c => c.SetCommand(
                    $$"""
                      Get-CimInstance Win32_Process |
                      Where-Object {
                        $_.CommandLine -like "*{{projectName}}.dll*" -and
                        $_.CommandLine -notlike "*Get-CimInstance*"
                      } |
                      ForEach-Object { Stop-Process -Id $_.ProcessId -Force }
                      """).DisableProcessInvocationLogging());
            }
            else
            {
                ProcessTasks.StartProcess("/bin/bash", "-c",
                    $$"""
                    ps -eo pid,cmd |
                    grep dotnet |
                    grep {{projectName}}.dll |
                    awk '{print $1}' |
                    xargs -r kill -9
                    """);
            }
        });

    Target Test => _ => _
        .Description("Runs .NET tests")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTest(x => x
                .SetResultsDirectory(TestResultsDirectory)
                .CombineWith([Solution.tests.Rewrite_Recipes_Tests, Solution.tests.Rewrite_CSharp_Tests, Solution.tests.Rewrite_MSBuild_Tests], (c,v) => c
                    .SetProjectFile(v)
                    .AddProcessAdditionalArguments("--",
                        "--test-parameter RenderLST=false",
                        "--test-parameter NoAnsi=true",
                        "--no-progress",
                        "--no-ansi",
                        "--disable-logo",
                        "--report-trx",
                        "--output Detailed",
                        "--hide-test-output",
                        $"--report-trx-filename {v.Name}.trx",
                        "--results-directory", TestResultsDirectory
                    )
                )
            );
            InjectLogsIntoTrx();
        });

    Target Test2 => _ => _
        .Description("Runs .NET tests")
        .DependsOn(CleanTestResults)
        .Executes(() =>
        {

            DotNetTest(x => x
                .SetResultsDirectory(TestResultsDirectory)
                .CombineWith([Solution.tests.Rewrite_CSharp_Tests], (c,v) => c
                    .SetProjectFile(v)
                    .AddProcessAdditionalArguments("--",
                        "--test-parameter RenderLST=false",
                        "--test-parameter NoAnsi=true",
                        "--no-progress",
                        "--no-ansi",
                        "--disable-logo",
                        "--report-trx",
                        "--output Detailed",
                        "--hide-test-output",
                        $"--report-trx-filename {v.Name}.trx",
                        "--results-directory", TestResultsDirectory
                    )
                )
            );

            InjectLogsIntoTrx();
        });

    void InjectLogsIntoTrx()
    {
        var trxFiles = TestResultsDirectory.GlobFiles("*.trx");
        foreach (var trxFile in trxFiles)
        {
            var logFile = TestResultsDirectory.GlobFiles($"{trxFile.NameWithoutExtension}*.log").FirstOrDefault();
            if (logFile == null)
                continue;
            TrxLogsInjector.InjectLogs(trxFile, logFile);
        }
    }

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
        .DependsOn(Pack, PublishServer, Test);

    Target CIRelease => _ => _
        .DependsOn(Pack, PublishServer, GradlePublish, NugetPush);

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
            GradleTasks.Gradle(c => c
                .SetJvmOptions("-Xmx2048m -XX:+HeapDumpOnOutOfMemoryError")
                .SetTasks(KnownGradleTasks.Clean, KnownGradleTasks.Assemble)
                .SetWarningMode(WarningMode.None)
                .SetProcessAdditionalArguments("--console=plain","--info","--stacktrace","--no-daemon")
            );
        });

    Target GradlePublish => _ => _
        .Executes(() =>
        {
            var isPreRelease = GitHubActions?.RefName?.Contains("-rc.") ?? true;

            GradleSettings ApplyCommonGradleSettings(GradleSettings options) => options
                .SetJvmOptions("-Xmx2048m -XX:+HeapDumpOnOutOfMemoryError")
                .SetWarningMode(WarningMode.None)
                .SetProjectProperty(
                    "releasing",
                    "release.disableGitChecks=true",
                    "release.useLastTag=true"
                )
                .SetProcessAdditionalArguments("--console=plain", "--info", "--stacktrace", "--no-daemon");
            if (isPreRelease)
            {
                GradleTasks.Gradle(c => ApplyCommonGradleSettings(c)
                    .SetTasks(KnownGradleTasks.Candidate, "publish", KnownGradleTasks.CloseAndReleaseSonatypeStagingRepository)

                );
            }
            else
            {
                GradleTasks.Gradle(c => ApplyCommonGradleSettings(c)
                    .SetTasks(KnownGradleTasks.Final, "publish", KnownGradleTasks.CloseAndReleaseSonatypeStagingRepository)
                    .AddProcessAdditionalArguments("--info")
                );
            }

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
