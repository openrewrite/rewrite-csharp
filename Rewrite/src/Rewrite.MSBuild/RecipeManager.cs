using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using NMica.Utils.IO;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Rewrite.Core;
using Rewrite.Core.Config;


namespace Rewrite.MSBuild;

public class RecipeManager
{
    private ILogger _log;
    private readonly ILoggerFactory _loggerFactory;

    private NuGet.Common.ILogger _nugetLogger;
    // private SortedDictionary<PackageIdentity, RecipeExecutionContext> _loadedRecipes = new();
    public static string NugetOrgRepository = "https://api.nuget.org/v3/index.json";

    public RecipeManager(ILoggerFactory loggerFactory, NuGet.Common.ILogger nugetLogger)
    {
        _log = loggerFactory.CreateLogger<RecipeManager>();
        _loggerFactory = loggerFactory;
        _nugetLogger = nugetLogger;
    }
    // [Obsolete]
    // public RecipeDescriptor FindRecipeDescriptor(InstallableRecipe installableRecipeId)
    // {
    //     var context = FindContext(installableRecipeId.GetPackageIdentity());
    //     return context.Recipes.FirstOrDefault(x => x.Id == installableRecipeId.Id) 
    //            ?? throw new InvalidOperationException($"Recipe {installableRecipeId.Id} not found in loaded package {installableRecipeId.NugetPackageName}:{installableRecipeId.NugetPackageVersion}");
    // }

    // public Recipe CreateRecipe(RecipeStartInfo startInfo)
    // {
    //     _log.LogInformation("Starting recipe {@RecipeId} with Options: {@Options}", startInfo.Id, startInfo.Arguments.Values.Where(x => x.Value != null).ToDictionary(x => x.Name, x => x.Value));
    //     var context = FindContext(startInfo.NugetPackageId);
    //     var recipeDescriptor = context.CreateRecipe(startInfo);
    //     return recipeDescriptor;
    //     
    // }
    //
    //
    // public async Task<RecipePackageInfo> InstallRecipePackage(
    //     RecipePackage installableRecipeId,
    //     List<PackageSource>? packageSources = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await InstallRecipePackage(
    //         installableRecipeId.NugetPackageName, 
    //         installableRecipeId.NugetPackageVersion, 
    //         packageSources: packageSources, 
    //         cancellationToken: cancellationToken);
    // }
    // public async Task<RecipePackageInfo> InstallRecipePackage(
    //     string packageId,
    //     bool includePrerelease = false,
    //     List<PackageSource>? packageSources = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await InstallRecipePackage(
    //         new LibraryRange(packageId), 
    //         includePrerelease,
    //         packageSources, 
    //         cancellationToken);
    // }
    // public async Task<RecipePackageInfo> InstallRecipePackage(
    //     string packageId,
    //     string packageVersion,
    //     bool includePrerelease = false,
    //     List<PackageSource>? packageSources = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await InstallRecipePackage(
    //         new PackageIdentity(packageId, NuGetVersion.Parse(packageVersion)), 
    //         includePrerelease,
    //         packageSources, 
    //         cancellationToken);
    // }
    //
    // public async Task<RecipePackageInfo> InstallRecipePackage(
    //     PackageIdentity packageId,
    //     bool includePrerelease = false,
    //     List<PackageSource>? packageSources = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await InstallRecipePackage(
    //         new LibraryRange(packageId.Id, 
    //             VersionRange.Combine([packageId.Version]), 
    //             LibraryDependencyTarget.Package), 
    //         includePrerelease, 
    //         packageSources, 
    //         cancellationToken);
    // }


    /// <summary>
    /// Installs the requested recipe packages that satisfy requested library range and creates an isolated execution context.
    /// This isolation context allows clean separation of all recipe packages that need to run together as a single batch
    /// </summary>
    /// <param name="requestedPackages"></param>
    /// <param name="includePrerelease"></param>
    /// <param name="packageSources"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<RecipeExecutionContext> CreateExecutionContext(
        IReadOnlyCollection<LibraryRange> requestedPackages,
        CancellationToken cancellationToken,
        bool includePrerelease = false,
        IReadOnlyCollection<PackageSource>? packageSources = null)
    {
        
        requestedPackages = requestedPackages
            .Select(x => x.VersionRange != null ? x : new LibraryRange(x.Name, VersionRange.AllStable, LibraryDependencyTarget.Package))
            .ToList();

        packageSources ??= [new PackageSource(NugetOrgRepository)];
        
        var settings = NullSettings.Instance;
        var sourceProvider = new PackageSourceProvider(settings, packageSources);
        var cachingSourceProvider = new CachingSourceProvider(sourceProvider);

        var selectedPackages = await Task.WhenAll(requestedPackages
            .Select(async requestedPackage =>
            {
                var bestAvailableVersion = await FindHighestCompatibleVersion(requestedPackage, includePrerelease, cachingSourceProvider, cancellationToken);
                if (bestAvailableVersion == null || !bestAvailableVersion.HasVersion)
                {
                    throw new InvalidOperationException($"Could not find a compatible version for {requestedPackage.Name}");
                }
                
                return new PackageIdentity(requestedPackage.Name, bestAvailableVersion.Version);
            }));
        
        var requestedVsSelected = requestedPackages.Join(selectedPackages, x => x.Name, x => x.Id, (requested, selected) => new
        {
            PackageId = requested.Name,
            RequestedRange = requested.VersionRange?.ToString(),
            SelectedVersion = selected.Version
        });
        
        _log.LogDebug("Resolved packages: {@ResolvedPackages}", requestedVsSelected);

        
        var projectDefinition = CreatePackageRestoreSpec(selectedPackages);
        var lockFile = await RestoreProject(projectDefinition, cachingSourceProvider, cancellationToken);
        var requiredAssemblies = GetRequiredAssemblies(lockFile, settings);
        var recipeExecutionContext = new RecipeExecutionContext(requiredAssemblies.Select(x => x.AssemblyPath).ToList(), _loggerFactory);
        _log.LogInformation("{@Recipes}", recipeExecutionContext.Recipes.Select(x => new {x.Id, x.TypeName, x.DisplayName}));
        return recipeExecutionContext;

    }

    // private RecipeExecutionContext FindContext(PackageIdentity packageIdentity)
    // {
    //     if(!_loadedRecipes.TryGetValue(packageIdentity, out var context))
    //     {
    //         throw new InvalidOperationException($"Recipe package containing recipe {packageIdentity} isn't loaded");
    //     }
    //
    //     return context;
    // }


    private async Task<SourcePackageDependencyInfo> FindHighestCompatibleVersion(LibraryRange libraryRange, bool includePreRelease, CachingSourceProvider cachingSourceProvider, CancellationToken cancellationToken = default)
    {
        var repositories = cachingSourceProvider.PackageSourceProvider.LoadPackageSources()
            .Where(s => s.IsEnabled)
            .Select(cachingSourceProvider.CreateRepository)
            .ToList();
        var cacheContext = new SourceCacheContext();
        NuGetVersion? highest = null;
        SourceRepository? repository = null;

        foreach (var repo in repositories)
        {
            repository = repo;
            var metadataResource = await repo.GetResourceAsync<MetadataResource>(cancellationToken);
            var availableVersions = await metadataResource.GetVersions(
                libraryRange.Name,
                includePrerelease: true,
                includeUnlisted: false,
                cacheContext,
                _nugetLogger,
                cancellationToken);

            // Filter by version range and pick the highest
            var matched = availableVersions
                .Where(v => (libraryRange.VersionRange?.Satisfies(v) ?? true ) && (includePreRelease || !v.IsPrerelease))
                .OrderByDescending(v => v)
                .FirstOrDefault();

            if (matched != null && (highest == null || matched > highest))
            {
                highest = matched;
            }
        }

        var result = new SourcePackageDependencyInfo(libraryRange.Name, highest, [], true, repository);
        return result;
    }

    
    /// <summary>
    /// Creates a PackageSpec that represents a psuedo-project that is a merge of dependencies between currently running app (as read from deps.json file in executing assembly folder)
    /// and the target library.
    /// </summary>
    /// <returns></returns>
    private PackageSpec CreatePackageRestoreSpec(IReadOnlyCollection<PackageIdentity> packagesToInstall)
    {
        var deps = DependencyContext.Default!;
        var framework = NuGetFramework.ParseFrameworkName(deps.Target.Framework, DefaultFrameworkNameProvider.Instance);

        // remove Rewrite projects in this solution from restore, because they will be already present in the host app.
        // We only want to restore package itself and any third party dependencies it relies on 
        var packagesToPrune = AssemblyResolution.CoreRewriteAssemblies.Select(x => x.GetName().Name!).ToList();
        
        // we're gonna create a dummy project that has the main recipe package as a "package reference"
        // this will allow us to use NuGet resolution mechanism to build dependency graph and do the heavy lifting to populate global nuget cache
        // and give us comprehensive summary of all dll references this recipe needs to be able to run
        var projectName = "DummyProject";
        var currentAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        var libDependencies = packagesToInstall
            .Select(packageToInstall => new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                    packageToInstall.Id,
                    VersionRange.Parse(packageToInstall.Version.ToString()),
                    LibraryDependencyTarget.Package)
            })
            .ToImmutableArray();
            
        var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>
        {
            new TargetFrameworkInformation
            {
                FrameworkName = framework,
                Dependencies = libDependencies,
                PackagesToPrune = packagesToPrune.ToDictionary(x => x, x => new PrunePackageReference(x, VersionRange.Combine([NuGetVersion.Parse("0.0.1"),NuGetVersion.Parse("9999.1.1")])))
            }
        })
        {
            Name = projectName,
            FilePath = $"{projectName}.csproj",
            RestoreMetadata = new ProjectRestoreMetadata
            {
                ProjectUniqueName = projectName,
                ProjectName = projectName,
                ProjectPath = Path.Combine(currentAssemblyDir, "_dummy.csproj"),
                ProjectStyle = ProjectStyle.PackageReference,
                OutputPath = "obj",
                TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                {
                    new(framework)
                }
            }
        };

        return packageSpec;
    }
    
    private async Task<LockFile> RestoreProject(PackageSpec packageSpec, CachingSourceProvider cachingSourceProvider, CancellationToken cancellationToken = default)
    {
        var settings = Settings.LoadDefaultSettings(null);
        var globalPackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings);
        var fallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings);
        var sources = cachingSourceProvider.GetRepositories().ToList();

        var cacheContext = new SourceCacheContext();
        var clientPolicy = ClientPolicyContext.GetClientPolicy(settings, _nugetLogger);
        var mapping = PackageSourceMapping.GetPackageSourceMapping(settings);

        var providerCache = new RestoreCommandProvidersCache();
        var providers = providerCache.GetOrCreate(
            globalPackagesPath: globalPackagesPath,
            fallbackPackagesPaths: fallbackFolders,
            sources: sources,
            cacheContext: cacheContext,
            log: _nugetLogger);
            
        var request = new RestoreRequest(
            project: packageSpec,
            dependencyProviders: providers,
            cacheContext: cacheContext,
            clientPolicyContext: clientPolicy,
            packageSourceMapping: mapping,
            log: _nugetLogger,
            lockFileBuilderCache: new LockFileBuilderCache())
        {
            RestoreForceEvaluate = true
        };

        // Run the restore
        var command = new RestoreCommand(request);
        var result = await command.ExecuteAsync(cancellationToken);
        return result.LockFile;
    }
    
    /// <summary>
    /// For a given lock file, returns all the assemblies required at runtime and resolves their absolute path to Global nuget cache
    /// </summary>
    /// <returns></returns>
    private static List<(PackageIdentity PackageIdentity, AbsolutePath AssemblyPath)> GetRequiredAssemblies(LockFile lockFile, ISettings settings)
    {
        var globalPackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings);
        
        var libsToPath = lockFile.Libraries.ToDictionary(x => (x.Name, x.Version), x => x.Path);
        var libraryDlls = lockFile.Targets.First()
            .Libraries
            .SelectMany(targetLib => targetLib.RuntimeAssemblies
                .Select(assembly => (Folder: libsToPath[(targetLib.Name!, targetLib.Version!)], File: assembly.Path, PackageIdentity: new PackageIdentity(targetLib.Name, targetLib.Version))))
            
            .ToList();
        var targetFolderRegex = new Regex(@"^analyzers/dotnet/(roslyn[0-9]\.[0-9]/)?cs/");
        
        var analyzerDlls = lockFile.Libraries.SelectMany(lib =>
            {
                // we may have folder structure like this, so we need to just pick files from one folder
                // "analyzers/dotnet/roslyn4.7/cs/test.dll",
                // "analyzers/dotnet/roslyn4.7/cs/test2.dll",
                // "analyzers/dotnet/roslyn4.3/cs/test.dll",
                // "analyzers/dotnet/cs/test.dll"
                var targetFolder = lib.Files
                    .Select(x => targetFolderRegex.Match(x).Value)
                    .Distinct()
                    .Where(x => x != "")
                    .OrderBy(x => x)
                    .LastOrDefault();
                if(targetFolder is null)
                    return [];
                var targetFilesRegex = new Regex($"^{targetFolder}.+");
                var files =  lib.Files.Where(x => targetFilesRegex.IsMatch(x) & x.EndsWith(".dll"));
                
                return files.Select(file => (Folder: lib.Path, File: file, PackageIdentity: new PackageIdentity(lib.Name, lib.Version)));
            })
            // .Where(x => dllsInAnalyzerSubfolder.IsMatch(x.File))
            .ToList();
        
        //
        var combined = libraryDlls
            .Union(analyzerDlls)
            .Select(x => (x.PackageIdentity, AssemblyPath: (AbsolutePath)globalPackagesPath / x.Folder / x.File))
            .Where(x => x.AssemblyPath.Name != "_._")
            .ToList();
        return combined;
    }
    
}