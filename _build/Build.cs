using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
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
using Nuke.Common.Tools.PowerShell;
using Nuke.Common.Utilities;
using Rewrite.Core;
using Rewrite.MSBuild;
using Rewrite.RewriteXml.Tree;
using Serilog;
using Spectre.Console;
using static GradleTasks;

// ReSharper disable UnusedMember.Local
[HandleHelpRequestsAttribute(Priority = 20)]
partial class Build : NukeBuild
{
    static Build()
    {
        Environment.SetEnvironmentVariable("NUKE_TELEMETRY_OPTOUT", "true");
        Environment.SetEnvironmentVariable("NoLogo", "true");
    }

    public Build()
    {
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

    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);


    GitHubActions GitHubActions => GitHubActions.Instance;

    [Parameter("Nuget.org API key required to push packages")][Secret] readonly string NugetApiKey;

    [Parameter("Nuget feed to which packages are pushed (default: https://api.nuget.org/v3/index.json)")] readonly string NugetFeed = "https://api.nuget.org/v3/index.json";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory /  "test-results";
    AbsolutePath TestFixturesDirectory => RootDirectory / "Rewrite" / "tests" / "fixtures";
    AbsolutePath TestFixturesFile => TestFixturesDirectory / "fixtures.txt";
    [Solution(GenerateProjects = true)] Solution Solution;
    [NerdbankGitVersioning] NerdbankGitVersioning Version;
    [GitRepositoryExt] LibGit2Sharp.Repository GitRepository;

    const string TargetFramework = "net9.0";


    bool IsCurrentBranchCommitted() => !GitRepository.RetrieveStatus().IsDirty;

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
        .Description("Publishes server into artifacts directory as a zip file")
        .DependsOn(Restore)
        .Before(Test)
        .Executes(() =>
        {
            DotNetPublish(c => c
                .SetProject(Solution.src.Rewrite_Server)
                .SetVersion(Version.NuGetPackageVersion));

            var publishDir = Solution.src.Rewrite_Server.Directory / "bin" / "Release" / TargetFramework / "publish";
            var zipFilePath = ArtifactsDirectory / "DotnetServer.zip";
            zipFilePath.DeleteFile();
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

    [Category("Test")]
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

    [Category("Test")]
    Target GenerateRoslynRecipes => _ => _
        .Description("Generates Java recipe classes per .NET roslyn recipe found in common packages")
        // .DependsOn(Restore)
        .Executes(async () =>
        {
            var services = new ServiceCollection();
            services.AddLogging(c => c
                .AddSerilog());
            services.AddSingleton<RecipeManager>();
            services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
            var serviceProvider = services.BuildServiceProvider();
            T CreateObject<T>(params object[] args) => ActivatorUtilities.CreateInstance<T>(serviceProvider, args);

            var recipeManager = CreateObject<RecipeManager>();

            string[] feeds =
            [
                "https://api.nuget.org/v3/index.json"
            ];

            var packageSources = feeds.Select(x => new PackageSource(x)).ToList();
            // var recipeManager = new RecipeManager();
            // CA1802: Use Literals Where Appropriate
            // CA1861: Avoid constant arrays as arguments
            var packages =  new(string Package, string Version)[]
            {
                ("Microsoft.CodeAnalysis.NetAnalyzers", "9.0.0"), //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories
                ("Roslynator.Analyzers", "4.13.1"), //https://github.com/dotnet/roslynator
                ("Meziantou.Analyzer", "2.0.201"), //https://github.com/meziantou/Meziantou.Analyzer
            };

            var installablePackages = await Task.WhenAll(packages.Select(x => recipeManager.InstallRecipePackage(x.Package, x.Version, packageSources: packageSources)));
            var recipesDir = RootDirectory / "rewrite-csharp" / "src" / "main" / "java" / "org" / "openrewrite" / "csharp" / "recipes";

            var models = installablePackages.SelectMany(packageInfo => packageInfo.Recipes.Select(recipe =>
            {
                var className = recipe.Id.StartsWith(recipe.TypeName) ? recipe.Id : recipe.TypeName.FullName.Split('.').Last();
                className = className.ReplaceRegex("(Analyzer|Fixer|CodeFixProvider)$", _ =>  "");
                className = $"{className}{recipe.Id}";
                var @namespace = packageInfo.Package.Id.ToLower();
                return new
                {
                    recipe.Id,
                    Description = recipe.Description.Replace("\"","\\\"").Replace('\r',' ').Replace('\n',' '),
                    DisplayName = recipe.DisplayName.Replace("\"","\\\"").Replace('\r',' ').Replace('\n',' '),
                    PackageName = packageInfo.Package.Id,
                    PackageVersion = packageInfo.Package.Version,
                    ClassName = className,
                    Namespace = @namespace,
                    FileName = recipesDir / @namespace.Replace(".", "/") / $"{className}.java"
                };
            })).ToList();

            var result = models.Select(model => (model.FileName, Content: $$""""
                 package org.openrewrite.csharp.recipes.{{model.Namespace}};

                 import org.openrewrite.NlsRewrite;
                 import org.openrewrite.csharp.RoslynRecipe;

                 public class {{model.ClassName}} extends RoslynRecipe {

                     @Override
                     public String getRecipeId() {
                         return "{{model.Id}}";
                     }

                     @Override
                     public String getNugetPackageName() {
                         return "{{model.PackageName}}";
                     }

                     @Override
                     public String getNugetPackageVersion() {
                         return "{{model.PackageVersion}}";
                     }

                     @Override
                     public @NlsRewrite.DisplayName String getDisplayName() {
                         return "{{model.DisplayName}}";
                     }

                     @Override
                     public @NlsRewrite.Description String getDescription() {
                         return "{{model.Description}}";
                     }
                 }

                 """"))
                .ToList();
            foreach (var (filename,source) in result)
            {
                filename.WriteAllText(source);
            }
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

            DotNetNuGetPush(s => s
                .SetSource(NugetFeed)
                .SetApiKey(NugetApiKey)
                .EnableSkipDuplicate()
                .SetTargetPath(ArtifactsDirectory / "*.nupkg"));

            // NuGetTasks.NuGetPush(s => s
            //         .SetSource(NugetFeed)
            //         .SetApiKey(NugetApiKey)
            //         .CombineWith(
            //             artifacts, (cs, v) => cs
            //                 .SetTargetPath(v)),
            //     degreeOfParallelism: 3,
            //     completeOnFailure: true);
        });

    [Category("CI")]
    Target CIBuild => _ => _
        .Description("Builds, tests and produces test reports for regular builds on CI")
        .DependsOn(Pack, PublishServer, Test);

    [Category("CI")]
    Target CIRelease => _ => _
        .Description("Creates and publishes release artifacts to maven & nuget")
        .DependsOn(Pack, PublishServer, GradlePublish, NugetPush);


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

    // Target GradleAssemble => _ => _
    //     .Executes(() =>
    //     {
    //         GradleTasks.Gradle(c => c
    //             .SetJvmOptions("-Xmx2048m -XX:+HeapDumpOnOutOfMemoryError")
    //             .SetTasks(KnownGradleTasks.Assemble)
    //             .SetWarningMode(WarningMode.None)
    //             .SetProcessAdditionalArguments("--console=plain","--info","--stacktrace","--no-daemon")
    //         );
    //     });
    //
    // Target GradleClean => _ => _
    //     .Executes(() =>
    //     {
    //         GradleTasks.Gradle(c => c
    //             .SetTasks(KnownGradleTasks.Clean, KnownGradleTasks.Assemble)
    //             .SetWarningMode(WarningMode.None)
    //             .SetProcessAdditionalArguments("--console=plain","--info","--stacktrace","--no-daemon")
    //         );
    //     });

    Target GradlePublish => _ => _
        .Description("Invokes Gradle to create a Java release")
        .After(Pack, NugetPush)
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
                Gradle(c => ApplyCommonGradleSettings(c)
                    .SetTasks(KnownGradleTasks.Candidate, "publish", KnownGradleTasks.CloseAndReleaseSonatypeStagingRepository)

                );
            }
            else
            {
                Gradle(c => ApplyCommonGradleSettings(c)
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
