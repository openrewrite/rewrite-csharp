#pragma warning disable CS8321
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;

using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.PowerShell;
using Nuke.Common.Utilities;
using Octokit;

using Serilog;
using Spectre.Console;
using ReflectionMagic;
using static GradleTasks;
using Commit = LibGit2Sharp.Commit;
using Credentials = LibGit2Sharp.Credentials;
using NotFoundException = LibGit2Sharp.NotFoundException;
using Repository = LibGit2Sharp.Repository;
using Signature = LibGit2Sharp.Signature;

#if LINK_MAIN_SOURCE

using NuGet.LibraryModel;
using Rewrite.Core;
using Rewrite.MSBuild;
using Rewrite.RewriteXml.Tree;
#endif

// ReSharper disable UnusedMember.Local
[HandleHelpRequests(Priority = 20)]
partial class Build : NukeBuild
{

    static Build()
    {
        Environment.SetEnvironmentVariable("NUKE_TELEMETRY_OPTOUT", "true");
        Environment.SetEnvironmentVariable("NoLogo", "true");
        var userDir = NukeBuild.RootDirectory / ".user";
        userDir.CreateDirectory();
        foreach (var file in userDir.GetFiles())
        {
            var envVarName = file.Name;
            Environment.SetEnvironmentVariable(envVarName, File.ReadAllText(file));
        }
    }

    public Build()
    {
        (ArtifactsDirectory / "test").CreateDirectory();

        AnsiConsole.Console.Profile.Width = 220;
        FigletFont LoadFont(string fontName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream($"FigletFonts.{fontName}.flf");
            var font = FigletFont.Load(stream);
            return font;
        }

        var openRewrite = new FigletText(LoadFont("ANSIShadow"), "OpenRewrite")
        {
            Pad = true
        };

        // var byModerne = new FigletText(LoadFont("BigChief"), "by Moderne");
        var grid = new Grid()
            .AddColumns(2);

        grid.Expand = false;

        grid.AddRow(openRewrite);
        AnsiConsole.Write(grid);

    }

    protected override void OnBuildCreated()
    {
        // ReSharper disable once LocalFunctionHidesMethod

        string O(string input)
        {
            string key = "test";
            if (input is null) throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key must be non-empty.", nameof(key));

            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] k = Encoding.UTF8.GetBytes(key);

            for (int i = 0; i < data.Length; i++)
                data[i] ^= k[i % k.Length];

            return Convert.ToBase64String(data);
        }

        base.OnBuildCreated();
    }

    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);


    GitHubActions GitHubActions => GitHubActions.Instance;

    [Parameter("Nuget.org API key required to push packages")][Secret] readonly string NugetApiKey;
    const string NugetOrgFeed = "https://api.nuget.org/v3/index.json";

    [Parameter("Nuget feed to which packages are pushed (default: https://api.nuget.org/v3/index.json)")] readonly string NugetFeed = NugetOrgFeed;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Parameter("GitHub personal access token with access to the repo")] readonly string GitHubToken;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory /  "test-results";
    AbsolutePath TestFixturesDirectory => RootDirectory / "Rewrite" / "tests" / "external-fixtures";
    AbsolutePath TestFixturesFile => TestFixturesDirectory / "fixtures.txt";
    [Solution(GenerateProjects = true)] Solution Solution;
    [NerdbankGitVersioning] NerdbankGitVersioning Version;
    [GitRepositoryExt] LibGit2Sharp.Repository GitRepository;
    AbsolutePath DotnetServerFilePath => ArtifactsDirectory / "DotnetServer.zip";

    readonly string[] TargetFrameworks = ["net10.0"];
    readonly string TargetFramework = "net10.0";



    bool IsOnMainBranch => string.Equals(GitRepository.Head.FriendlyName, "main", StringComparison.OrdinalIgnoreCase) && !GitRepository.Info.IsHeadDetached;
    bool IsPullRequest => GitHubActions?.EventName == "pull_request";
    // bool IsPreRelease => GitHubActions?.RefName?.Contains("-rc.") ?? true;
    bool IsPreRelease => !IsOnMainBranch || GetTagsForCurrentCheckout().Any(x => x.Contains("-rc."));
    bool IsCurrentCommitReleaseTagged => GetTagsForCurrentCheckout().Any(tag => tag.StartsWith("v") && !tag.Equals("latest", StringComparison.OrdinalIgnoreCase));

    // bool IsAllowedToPushToFeed => IsOnMainBranch && !IsPullRequest && IsGitCommitted;

    GradleSettings GradleSettings = new();

    bool IsGitCommitted => !GitRepository.RetrieveStatus().IsDirty;

    Target Clean => _ => _
        .Description("Clean out artifacts and all the bin/obj directories")
        .DependsOn(StopServer)
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
        .Description("Clean out test results directory")
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
                var packagePath = globalPackagesFolder / rewritePackageName / Version.NuGetPackageVersion;
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
        .After(CleanNugetCache)
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
        .Description("Publishes server into artifacts directory as a zip file")
        .DependsOn(Restore)
        .DependentFor(GradleTest)
        .Before(Test)
        .Executes(() =>
        {
            DotNetPublish(c => c
                .SetProject(Solution.src.Rewrite_Server)
                .SetVersion(Version.NuGetPackageVersion)
                .CombineWith(TargetFrameworks, (settings, targetFramework) => settings
                    .SetFramework(targetFramework)));

            var publishDir = Solution.src.Rewrite_Server.Directory / "bin" / "Release" / TargetFramework / "publish";
            var extension = IsWin ? ".exe" : "";
            Environment.SetEnvironmentVariable("ROSLYN_RECIPE_EXECUTABLE", publishDir);
            DotnetServerFilePath.DeleteFile();
            publishDir.ZipTo(DotnetServerFilePath);
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

    [Category("Test")]
    Target Test => _ => _
        .Description("Runs .NET tests")
        .DependsOn(Restore, Pack)
        .Executes(() =>
        {
            DotNetTest(x => x
                // .SetResultsDirectory(TestResultsDirectory)
                .CombineWith([
                        Solution.tests.Rewrite_Recipes_Tests,
                        Solution.tests.Rewrite_CSharp_Tests,
                        Solution.tests.Rewrite_MSBuild_Tests,
                    ], (c,project) => c
                    // .SetProjectFile(project)
                    // .SetVerbosity(DotNetVerbosity.normal)
                    // .SetLoggers("console;verbosity=quiet")
                    .AddProcessAdditionalArguments(
                        $"--project {project.Path}",
                        "--test-parameter RenderLST=false",
                        "--test-parameter NoAnsi=true",
                        "--no-progress",
                        "--no-ansi",
                        "--disable-logo",
                        "--report-trx",
                        // "--output Detailed",
                        // "--hide-test-output",
                        // "--help",
                        "--verbosity minimal",
                        $"--report-trx-filename {project.Name}.trx",
                        $"--results-directory {TestResultsDirectory}"
                    )
                )
            );
            // InjectLogsIntoTrx();
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
        .Requires(() => IsGitCommitted, () => !IsPullRequest, () => !IsOnMainBranch)
        .DependsOn(Pack)
        .Executes(() =>
        {
            // this.GitRepository.Head.Tip.
            var artifacts = ArtifactsDirectory.GlobFiles("*.nupkg");
            if (artifacts.IsEmpty())
            {
                Assert.Fail("No packages were found in artifacts directory.");
            }

            DotNetNuGetPush(s => s
                .SetSource(NugetFeed)
                .SetApiKey(NugetApiKey)
                .EnableSkipDuplicate()
                .SetTargetPath(ArtifactsDirectory / "*.nupkg"));
        });

    IReadOnlyCollection<ExecutableTarget> GetExecutableTargets() => this.AsDynamic().ExecutableTargets;
    Target SetDefaultModule => _ => _
        .Unlisted()
        .DependentFor(GradleTest)
        .Executes(() =>
            {
                if (Module == null && this.GetExecutableTargets().Contains(GradleTest))
                {
                    Module = "rewrite-csharp";
                }
            });


    Target SignalIfTestcaseOutputExists => _ => _
        .Unlisted()
        .Description("Sets github actions flags if any java or c# test output files were generated")
        .After(Test, GradleAssembleAndTestCSharpModule)
        .Executes(() =>
        {
            var hasTrx = TestResultsDirectory.GlobFiles("*.trx").Any();
            var hasJavaTest = (RootDirectory / "rewrite-csharp" / "build" / "test-results" / "test").GlobFiles("*.xml").Any();
            GitHubActions?.SetVariable("dotnet_tests_found", hasTrx);
            GitHubActions?.SetVariable("java_tests_found", hasJavaTest);
        });



    [Category("CI")]
    Target CIBuild => _ => _
        .Description("CI specific target for builds off main branch")
        .DependsOn(Pack, Test, GradleAssembleAndTestCSharpModule, DevRelease, SignalIfTestcaseOutputExists);


    [Category("CI")]
    Target CIRelease => _ => _
        .Description("CI specific target for explicit release builds")
        .DependsOn(Release);


    Target Release => _ => _
        .Description("Creates package releases and uploads them to feeds")
        .After(Pack, Test)
        .DependsOn(

            GradlePublishRelease,
            IncrementVersion,
            GitPush)
        .Triggers(ReleaseTagCommit);

    Target DevRelease => _ => _
        .Description("Creates package releases and uploads them to feeds")
        .After(Pack, Test)
        .OnlyWhenDynamic(() => !IsPullRequest && IsOnMainBranch)
        .DependsOn(GradlePublishSnapshot)
        .Executes(async () => await CreateGitHubRelease($"latest"));


    Target GitPush => _ => _
        .Executes(() =>
        {
            // GitTasks.Git("push");
        });

    Target IncrementVersion => _ => _
        .Description("Increments minor version in version.json")
        .Before(GitPush)
        .Executes(() =>
        {
            Commands.Checkout(this.GitRepository, "main");

            var versionFile = RootDirectory / "version.json";
            var versionJson = versionFile.ReadJson();
            var version = versionJson.GetPropertyValue<string>("version");

            // Parse version segments (e.g., "0.28" or "0.28-beta" -> ["0", "28"] or ["0", "28-beta"])
            var parts = version.Split('.');
            if (parts.Length < 2)
            {
                throw new InvalidOperationException($"Invalid version format: {version}");
            }

            // Increment the second segment (minor version)
            // Handle cases where second segment might have a suffix like "28-beta"
            var secondSegmentMatch = System.Text.RegularExpressions.Regex.Match(parts[1], @"^(\d+)(.*)$");
            if (!secondSegmentMatch.Success)
            {
                throw new InvalidOperationException($"Invalid minor version format: {parts[1]}");
            }

            var minor = int.Parse(secondSegmentMatch.Groups[1].Value);
            var suffix = secondSegmentMatch.Groups[2].Value; // Captures any suffix like "-beta"

            minor++;

            // Recombine all parts
            parts[1] = $"{minor}{suffix}";
            var newVersion = string.Join(".", parts);

            // Update the JSON
            versionJson["version"] = newVersion;

            // Write back to file
            versionFile.WriteJson(versionJson);

            Log.Information("Incremented version from {OldVersion} to {NewVersion}", version, newVersion);

            // Commit the change
            var signature = new Signature("Build System", "build@openrewrite.org", DateTimeOffset.Now);
            Commands.Stage(GitRepository, versionFile);

            var commit = GitRepository.Commit($"Increment version to {newVersion}", signature, signature);
            Log.Information("Committed version change: {CommitSha}", commit.Sha.Substring(0, 7));
        });

    Target ReleaseTagCommit => _ => _
        .Executes(() =>
        {
            var tagName = $"v{Version.SemVer2}";
            var currentCommit = GitRepository.Head.Tip;

            // Check if tag already exists
            var existingTag = GitRepository.Tags[tagName];

            if (existingTag != null)
            {
                // Get the commit the tag points to
                var taggedCommit = (existingTag.Target as Commit) ?? (existingTag.Target as TagAnnotation)?.Target as Commit;

                if (taggedCommit != null && taggedCommit.Sha != currentCommit.Sha)
                {
                    // Tag exists on different commit - remove it first
                    GitRepository.Tags.Remove(existingTag);
                    Log.Information("Removed existing tag {TagName} from commit {OldCommit}", tagName, taggedCommit.Sha.Substring(0, 7));
                }
                else if (taggedCommit != null && taggedCommit.Sha == currentCommit.Sha)
                {
                    // Tag already on current commit
                    Log.Information("Tag {TagName} already exists on current commit {Commit}", tagName, currentCommit.Sha.Substring(0, 7));
                    return;
                }
            }

            // Create tag on current commit
            var signature = new Signature("Build System", "build@openrewrite.org", DateTimeOffset.Now);
            var tag = GitRepository.Tags.Add(tagName, currentCommit, signature, $"Release {Version.SemVer2}", false);
            Log.Information("Created tag {TagName} on commit {Commit}", tagName, currentCommit.Sha.Substring(0, 7));


        });


    // Target PushFeeds => _ => _
    //     .Description("Creates package releases and uploads them to feeds")
    //     .After(Pack, Test)
    //     .DependsOn(NugetPush, GradlePush, GithubRelease);



    [Category("Test")]
    Target DownloadTestFixtures => _ => _
        .Description($"Clones git repos defined in {RootDirectory.GetRelativePathTo(TestFixturesFile)} to use in integration tests")
        .Executes(() =>
        {

            var fixtures = TestFixturesFile.ReadAllLines()
                .Select(url => new
                {
                    Name = url.Split('/').Last(x => !string.IsNullOrEmpty(x)).Replace(".git",""),
                    Url = url
                })
                .ToList();
            foreach (var fixture in fixtures)
            {
                var fixtureDirectory = TestFixturesDirectory / fixture.Name;
                if (fixtureDirectory.Exists())
                {
                    GitTasks.Git("pull", fixtureDirectory);
                }
                else
                {
                    GitTasks.Git($"clone --depth 1 {fixture.Url}", TestFixturesDirectory);
                }
            }
        });

    Target MergeMainIntoRelease => _ => _
        .Unlisted()
        .OnlyWhenStatic(() => IsGitCommitted)
        .Executes(() =>
        {
            var mainBranch = GitRepository.Branches["main"];
            var releaseBranch = GitRepository.Branches["release"];

            if (mainBranch == null)
            {
                throw new InvalidOperationException("Main branch not found");
            }

            if (releaseBranch == null)
            {
                // Create release branch from main if it doesn't exist
                releaseBranch = GitRepository.CreateBranch("release", mainBranch.Tip);
                GitRepository.Branches.Update(releaseBranch,
                    b => b.Remote = "origin",
                    b => b.UpstreamBranch = releaseBranch.CanonicalName);
                Log.Information("Created release branch from main at {Sha}", mainBranch.Tip.Sha.Substring(0, 7));
            }
            else
            {
                // Fast-forward release branch to main's tip without checkout
                var oldSha = releaseBranch.Tip.Sha.Substring(0, 7);
                var newSha = mainBranch.Tip.Sha.Substring(0, 7);

                // Check if fast-forward is possible (release is ancestor of main)
                var mergeBase = GitRepository.ObjectDatabase.FindMergeBase(releaseBranch.Tip, mainBranch.Tip);
                if (mergeBase == null || mergeBase.Sha != releaseBranch.Tip.Sha)
                {
                    Log.Warning("Cannot fast-forward release branch - it has diverged from main");
                    throw new InvalidOperationException("Release branch has diverged from main and cannot be fast-forwarded");
                }

                // Update release branch reference to point to main's tip
                GitRepository.Refs.UpdateTarget(releaseBranch.Reference, mainBranch.Tip.Id);
                Log.Information("Fast-forwarded release branch from {OldSha} to {NewSha} on 'main' branch", oldSha, newSha);
            }

        });

    Target GradleAssembleAndTestCSharpModule => _ => _
        .Unlisted()
        .DependsOn(PublishServer)
        .Before(GradleExecute)
        .Triggers(GradleExecute)
        .Executes(() =>
        {
            GradleSettings = GradleSettings.AddTasks(":rewrite-csharp:assemble", ":rewrite-csharp:test");
        });


    Target GradlePublishSnapshot => _ => _
        .Description("Invokes Gradle to create a SNAPSHOT release and push it to maven repositories")
        // .OnlyWhenStatic(() => IsAllowedToPushToFeed)
        .Requires(() => IsGitCommitted)
        .OnlyWhenDynamic(() => !IsPullRequest)
        .After(Pack, NugetPush)
        .Before(GradleExecute)
        .Triggers(GradleExecute)
        .Executes(() =>
        {
            GradleSettings = GradleSettings
                .AddProjectProperty(
                    "releasing",
                    "release.disableGitChecks=true"
                )
                .AddTasks(KnownGradleTasks.Publish)
                .EnableForceSigning()
                .AddExcludeTasks(KnownGradleTasks.Assemble, KnownGradleTasks.Test)
                .SetVersion(Version.MavenSnapshotVersion())
                .AddTasks(KnownGradleTasks.Snapshot);;
        });

    Target GradlePublishMavenLocal => _ => _
        .Description("Invokes Gradle to create a SNAPSHOT release and push it to maven repositories")
        // .OnlyWhenStatic(() => IsAllowedToPushToFeed)
        .DependsOn(Pack)
        .Before(GradleExecute)
        .Triggers(GradleExecute)
        .Executes(() =>
        {
            GradleSettings = GradleSettings
                .AddProjectProperty(
                    "releasing",
                    "release.disableGitChecks=true"
                )
                // .AddTasks(KnownGradleTasks.Publish)
                // .EnableForceSigning()
                // .AddExcludeTasks(KnownGradleTasks.Assemble, KnownGradleTasks.Test)
                .SetVersion(Version.MavenSnapshotVersion())
                .AddTasks(KnownGradleTasks.PublishToMavenLocal);;
        });


    Target GradlePublishRelease => _ => _
        .Description("Invokes Gradle to create a Java release and push it to maven repositories")
        // .OnlyWhenStatic(() => IsAllowedToPushToFeed)
        .Requires(() => IsGitCommitted)
        .OnlyWhenDynamic(() => !IsPullRequest && !IsOnMainBranch)
        .After(Pack, NugetPush)
        .Before(GradleExecute)
        .Triggers(GradleExecute)
        .Executes(() =>
        {
            GradleSettings = GradleSettings
                .AddProjectProperty(
                    "releasing",
                    "release.disableGitChecks=true"
                )
                .AddTasks(KnownGradleTasks.Publish)
                .EnableForceSigning()
                .AddExcludeTasks(KnownGradleTasks.Assemble, KnownGradleTasks.Test)
                .SetVersion(Version.SemVer2)
                .AddTasks(IsPreRelease ? KnownGradleTasks.Candidate : KnownGradleTasks.Final);
        });

    //
    //
    // Target GradlePush => _ => _
    //     .Description("Invokes Gradle to create a Java release and push it to maven repositories")
    //     // .OnlyWhenStatic(() => IsAllowedToPushToFeed)
    //     .Requires(() => IsAllowedToPushToFeed)
    //     .After(Pack, NugetPush)
    //     .Before(GradleExecute)
    //     .Triggers(GradleExecute)
    //     .Executes(() =>
    //     {
    //         GradleSettings = GradleSettings.AddProjectProperty(
    //             "releasing",
    //             "release.disableGitChecks=true"
    //         )
    //         .AddTasks(KnownGradleTasks.Publish)
    //         .SetVersion(Version.MavenSnapshotVersion())
    //         .EnableForceSigning()
    //         .AddExcludeTasks(KnownGradleTasks.Assemble, KnownGradleTasks.Test);
    //
    //         if (IsCurrentCommitReleaseTagged)
    //         {
    //             GradleSettings = GradleSettings
    //                 .AddTasks(IsPreRelease ? KnownGradleTasks.Candidate : KnownGradleTasks.Final)
    //                 // .AddProjectProperty(
    //                 //     "releasing",
    //                 //     "release.disableGitChecks=true"
    //                 //     // "release.useLastTag=true"
    //                 // )
    //                 // .AddTasks(KnownGradleTasks.CloseAndReleaseSonatypeStagingRepository)
    //                 // .EnableForceSigning()
    //                 ;
    //         }
    //         else
    //         {
    //             GradleSettings = GradleSettings
    //                 .AddTasks(KnownGradleTasks.Snapshot);
    //         }
    //
    //     });

    Target GradleExecute => _ => _
        .Description("Executes any queued up gradle stuff as a batch")
        .Unlisted()
        .Executes(() =>
        {
            GradleSettings = GradleSettings
                .SetJvmOptions("-Xmx2048m -XX:+HeapDumpOnOutOfMemoryError")
                .SetWarningMode(WarningMode.None)
                .SetProcessAdditionalArguments("--console=plain", "--info", "--stacktrace", "--no-daemon")
                .SetProcessEnvironmentVariable("ROSLYN_RECIPE_EXECUTABLE", Environment.GetEnvironmentVariable("ROSLYN_RECIPE_EXECUTABLE"));

            var publishRelease = GradleSettings.Tasks.Contains(KnownGradleTasks.Final) || GradleSettings.Tasks.Contains(KnownGradleTasks.Candidate);
            var publishesSnapshot = GradleSettings.Tasks.Contains(KnownGradleTasks.Snapshot);
            if (publishRelease && publishesSnapshot)
            {
                // we can't publish snapshots to maven central. if we try to do both publish & snapshot in one gradle run, it will fail

                var snapshotSettings = GradleSettings.SetTasks(GradleSettings.Tasks.Where(x => x != KnownGradleTasks.Publish && x != KnownGradleTasks.CloseAndReleaseSonatypeStagingRepository));
                var releaseSettings = GradleSettings.SetTasks(GradleSettings.Tasks.Where(x => x != KnownGradleTasks.Snapshot))
                    .AddExcludeTasks(KnownGradleTasks.Assemble, KnownGradleTasks.Test);
                Gradle(snapshotSettings);
                Gradle(releaseSettings);
            }
            Gradle(GradleSettings);
        });

    Target GithubRelease => _ => _
        .Description("Creates a GitHub release (or amends existing)")
        .Requires(() => GitHubToken)
        .Requires(() => !IsOnMainBranch, () => !IsPullRequest)
        .After(Pack, NugetPush, GradlePublishRelease, GradleSnapshot, Test, GradleAssembleAndTestCSharpModule, GradleTest)
        .Executes(async () =>
        {

            await CreateGitHubRelease($"v{Version.SemVer1}");
            await CreateGitHubRelease($"latest");
        });



    public async Task CreateGitHubRelease(string releaseName)
    {
        var client = new GitHubClient(new ProductHeaderValue("OpenRewrite"))
        {
            Credentials = new Octokit.Credentials(GitHubToken, AuthenticationType.Bearer)
        };
        var (owner, repoName) = GetGitHubOwnerAndRepo();

        Release release;
        try
        {
            release = await client.Repository.Release.Get(owner, repoName, releaseName);
        }
        catch (Octokit.NotFoundException)
        {
            var newRelease = new NewRelease(releaseName)
            {
                Name = releaseName,
                Draft = false,
                Prerelease = false
            };
            release = await client.Repository.Release.Create(owner, repoName, newRelease);
        }

        var existingAsset = release.Assets.FirstOrDefault(x => x.Name == DotnetServerFilePath.Name);
        if (existingAsset != null)
        {
            await client.Repository.Release.DeleteAsset(owner, repoName, existingAsset.Id);
        }

        var stream = File.OpenRead(DotnetServerFilePath);
        var releaseAssetUpload = new ReleaseAssetUpload(DotnetServerFilePath.Name, "application/zip", stream, TimeSpan.FromHours(1));
        var releaseAsset = await client.Repository.Release.UploadAsset(release, releaseAssetUpload);

    }
    public  (string Owner, string Repo) GetGitHubOwnerAndRepo()
    {
        var originRemote = GitRepository.Network.Remotes["origin"];
        if (originRemote == null)
            throw new Exception("No origin remote");

        var url = originRemote.Url;

        // Handle SSH and HTTPS GitHub URLs
        // Examples:
        //  - git@github.com:owner/repo.git
        //  - https://github.com/owner/repo.git
        string pattern = @"github\.com[:/](?<owner>[^/]+?)/(?<repo>[^/]+?)(\.git)?$";
        var match = System.Text.RegularExpressions.Regex.Match(url, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!match.Success)
            throw new Exception("Origin not set to github remote url");

        return (match.Groups["owner"].Value, match.Groups["repo"].Value);
    }

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

    public static string O(string input)
    {
        var key = "test";

        byte[] data = Encoding.UTF8.GetBytes(input);
        byte[] k = Encoding.UTF8.GetBytes(key);

        for (int i = 0; i < data.Length; i++)
            data[i] ^= k[i % k.Length];

        return Convert.ToBase64String(data);
    }


    protected override void OnBuildInitialized()
    {
        void log(string envVar) => Log.Information("{Value}",O(Environment.GetEnvironmentVariable(envVar) ?? ""));

        base.OnBuildInitialized();
    }
}
