using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using log4net;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using Rewrite.Core;

namespace Rewrite.MSBuild;

public class NugetManager
{
    private static readonly ILog log = LogManager.GetLogger(typeof(NugetManager));

    public static async Task<InstallableRemotingRecipe> InstallRecipeAsync(string packageId, string packageVersion,
        List<PackageSource> packageSources,
        string packagesPath,
        CancellationToken cancellationToken)
    {
        var logger = NugetManagerLogger.Instance;


        var frameworkName = Assembly.GetExecutingAssembly().GetCustomAttributes(true)
            .OfType<TargetFrameworkAttribute>()
            .Select(x => x.FrameworkName)
            .FirstOrDefault();
        var currentFramework = frameworkName == null
            ? NuGetFramework.AnyFramework
            : NuGetFramework.ParseFrameworkName(frameworkName, new DefaultFrameworkNameProvider());

        var nuGetVersion = packageVersion.ToUpper() is "LATEST" or "RELEASE"
            ? await FindLatestVersionAsync(packageId, packageVersion.Equals("LATEST", StringComparison.CurrentCultureIgnoreCase), packageSources, logger,
                cancellationToken)
            : new NuGetVersion(packageVersion);

        var packageIdentityToResolve = new PackageIdentity(packageId, nuGetVersion);

        var settings = Settings.LoadDefaultSettings(packagesPath, null, null);
        var sourceRepositoryProvider =
            new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());
        var packageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, packagesPath);
        var packageExists = packageManager.PackageExistsInPackagesFolder(packageIdentityToResolve);

        if (log.IsDebugEnabled)
        {
            log.Debug(packageExists
                ? $"Skip package {packageId}:{packageVersion} downloading since it already exists in the package store"
                : $"Downloading package {packageId}:{packageVersion}");
        }

        var (packageReader, source) =
            packageExists
                ? (new PackageArchiveReader(
                    packageManager.PackagesFolderNuGetProject.GetInstalledPackageFilePath(packageIdentityToResolve)), $"{Uri.UriSchemeFile}://{packagesPath}")
                : await DownloadNupkgAsync(packageId, packageVersion, packageSources,
                    settings,
                    logger, packageManager,
                    packageIdentityToResolve, cancellationToken);

        using (packageReader)
        {

            var nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

            if (log.IsDebugEnabled)
            {
                log.Debug($"Resolved nuspec {nuspecReader}");
            }

            var identity = nuspecReader.GetIdentity();

            if (log.IsDebugEnabled)
            {
                log.Debug($"Resolved package identity {identity}");
            }

            var dependency = FindDependencyGroup(currentFramework, nuspecReader) ??
                             throw new InvalidOperationException("Dependency group not found");
            await LoadRecipePackagesAsync(dependency, currentFramework,
                packageManager.PackagesFolderNuGetProject, cancellationToken);

            var installedPath = packageManager.PackagesFolderNuGetProject.GetInstalledPath(identity);
            var assemblies =
                LoadFrameworkSpecificGroupItemsToAssembly(currentFramework, packageReader, identity, installedPath);
            var recipes = assemblies.SelectMany(FindAllRecipes).ToList();

            if (log.IsDebugEnabled)
            {
                log.Debug($"Found {recipes.Count}");

                if (recipes.Count > 0)
                {
                    log.Debug($"Found the following recipes [{recipes}]");
                }
            }

            var mappedRecipes = recipes.Select(r =>
                    r.Descriptor.With(
                        source: new Uri(
                            $"recipe://{nuspecReader.GetIdentity().Id}/{nuspecReader.GetIdentity().Version}")))
                .ToList();

            return new InstallableRemotingRecipe(source, nuspecReader.GetIdentity().Version.ToString(), mappedRecipes);
        }
    }

    private static async Task<NuGetVersion> FindLatestVersionAsync(string packageId, bool includePrerelease, List<PackageSource> packageSources, ILogger logger, CancellationToken cancellationToken)
    {
        return (await Task.WhenAll(packageSources.Select(s => Repository.Factory.GetCoreV3(s))
                .Select(async repository =>
                {
                    var resource = await repository.GetResourceAsync<PackageSearchResource>(cancellationToken);
                    var packages = await resource.SearchAsync(packageId, new SearchFilter(includePrerelease), 0, 100, logger,
                        cancellationToken);
                    var iPackageSearchMetadata = packages.FirstOrDefault(psm => psm.Identity.Id == packageId);
                    if (iPackageSearchMetadata == null)
                    {
                        return [];
                    }
                    return await iPackageSearchMetadata.GetVersionsAsync();
                })))
            .SelectMany(psm => psm.Select(m => m.Version))
            .Distinct()
            .OrderDescending()
            .First();
    }

    private static async Task<(PackageArchiveReader, string)> DownloadNupkgAsync(string packageId, string packageVersion,
        List<PackageSource> packageSources,
        ISettings settings, ILogger logger, NuGetPackageManager packageManager,
        PackageIdentity packageIdentityToResolve,
        CancellationToken cancellationToken)
    {

        if (log.IsDebugEnabled)
        {
            log.Debug($"Downloading package {packageId}:{packageVersion}");
        }


        var allowPrereleaseVersions = true;
        var allowUnlisted = false;
        var resolutionContext = new ResolutionContext(
            DependencyBehavior.Lowest, allowPrereleaseVersions, allowUnlisted, VersionConstraints.None);
        var projectContext = new ProjectContext
        {
            PackageExtractionContext = new PackageExtractionContext(PackageSaveMode.Defaultv3,
                XmlDocFileSaveMode.Compress, ClientPolicyContext.GetClientPolicy(settings, logger), logger)
        };


        var sourceRepositories = packageSources.Select(s => Repository.Factory.GetCoreV3(s)).ToList();
        SourceRepository? packageSourceRepository = null;
        foreach (var sourceRepository in sourceRepositories)
        {
            if (await sourceRepository.GetResource<FindPackageByIdResource>().DoesPackageExistAsync(
                    packageIdentityToResolve.Id,
                    packageIdentityToResolve.Version, resolutionContext.SourceCacheContext, logger, cancellationToken))
            {
                packageSourceRepository = sourceRepository;
                break;
            }
        }

        if (packageSourceRepository == null)
        {
            throw new Exception($"Could not find package source repository for {packageId} {packageVersion}");
        }

        await packageManager.InstallPackageAsync(packageManager.PackagesFolderNuGetProject,
            packageIdentityToResolve,
            resolutionContext,
            projectContext,
            sourceRepositories,
            [],
            cancellationToken);

        if (log.IsDebugEnabled)
        {
            log.Debug(
                $"Package {packageId}:{packageVersion} has been successfully downloaded and stored at {packageManager.PackagesFolderNuGetProject}");
        }

        return (new PackageArchiveReader(
            packageManager.PackagesFolderNuGetProject.GetInstalledPackageFilePath(packageIdentityToResolve)), packageSourceRepository.PackageSource.Source);
    }

    public static async Task<Recipe> LoadRecipeAssemblyAsync(string recipeClassName, string packageId,
        string packageVersion,
        string nuGetProjectFolderPath, Dictionary<string, Func<Type, object>> options,
        CancellationToken cancellationToken)
    {
        var folderNuGetProject = new FolderNuGetProject(nuGetProjectFolderPath);
        var frameworkName = Assembly.GetExecutingAssembly().GetCustomAttributes(true)
            .OfType<TargetFrameworkAttribute>()
            .Select(x => x.FrameworkName)
            .FirstOrDefault();
        var currentFramework = frameworkName == null
            ? NuGetFramework.AnyFramework
            : NuGetFramework.ParseFrameworkName(frameworkName, new DefaultFrameworkNameProvider());


        var packageIdentity = new PackageIdentity(packageId, new NuGetVersion(packageVersion));
        var installedPath = folderNuGetProject.GetInstalledPath(packageIdentity);
        var nugetPackagePath = folderNuGetProject.GetInstalledPackageFilePath(packageIdentity);
        using var packageReader = new PackageArchiveReader(nugetPackagePath);
        var nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

        var dependencyGroup = FindDependencyGroup(currentFramework, nuspecReader) ??
                              throw new InvalidOperationException("Dependency group not found");

        await LoadRecipePackagesAsync(dependencyGroup, currentFramework, folderNuGetProject, cancellationToken);

        var assemblies =
            LoadFrameworkSpecificGroupItemsToAssembly(currentFramework, packageReader, packageIdentity, installedPath);


        var recipe = LoadRecipe(assemblies[0], recipeClassName, options);

        if (recipe == null)
        {
            throw new ArgumentException($"Recipe type {recipeClassName} not found");
        }

        return recipe;
    }

    static List<Recipe> FindAllRecipes(Assembly assembly)
    {
        var recipes = new List<Recipe>();
        foreach (var type in assembly.GetExportedTypes())
        {
            if (!typeof(Recipe).IsAssignableFrom(type)) continue;

            var constructorInfo = type.GetConstructors()[0];
            var parameters = new object?[constructorInfo.GetParameters().Length];
            for (var index = 0; index < constructorInfo.GetParameters().Length; index++)
            {
                var parameterInfo = constructorInfo.GetParameters()[index];
                var parameterInfoParameterType = parameterInfo.ParameterType;

                parameters[index] = parameterInfoParameterType.IsValueType
                    ? Activator.CreateInstance(parameterInfoParameterType)
                    : null;
            }

            recipes.Add((Recipe)constructorInfo.Invoke(parameters));
        }

        return recipes;
    }

    static Recipe LoadRecipe(Assembly assembly, string recipeClassName, Dictionary<string, Func<Type, object>> args)
    {
        var type = assembly.GetType(recipeClassName);
        if (!typeof(Recipe).IsAssignableFrom(type))
            throw new ArgumentException($"Given Recipe class name {recipeClassName} is not a Recipe type but {type}");

        var constructorInfo = type.GetConstructors()[0];
        var parameters = new object?[constructorInfo.GetParameters().Length];
        for (var index = 0; index < constructorInfo.GetParameters().Length; index++)
        {
            var parameterInfo = constructorInfo.GetParameters()[index];
            var parameterInfoParameterType = parameterInfo.ParameterType;
            if (!args.TryGetValue(parameterInfo.Name!, out var value))
            {
                if (!(parameterInfo.IsOptional || parameterInfo.HasDefaultValue ||
                      parameterInfoParameterType.CustomAttributes.FirstOrDefault(data =>
                          data.AttributeType == typeof(NullableAttribute)) != null))
                {
                    throw new ArgumentNullException(
                        $"Recipe requires {parameterInfo.Name} to be passed but given null");
                }

                parameters[index] = null;
            }
            else
            {
                parameters[index] = value.Invoke(parameterInfoParameterType);
            }
        }

        return (Recipe)constructorInfo.Invoke(parameters);
    }

    private static async Task LoadRecipePackagesAsync(PackageDependencyGroup dependencyGroup,
        NuGetFramework currentFramework,
        FolderNuGetProject folderNuGetProject, CancellationToken cancellationToken)
    {
        if (log.IsDebugEnabled)
        {
            log.Debug($"Loading assemblies for {dependencyGroup}");
        }

        foreach (var dependencyGroupPackage in dependencyGroup.Packages)
        {
            var packageIdentity =
                new PackageIdentity(dependencyGroupPackage.Id, dependencyGroupPackage.VersionRange.MinVersion);

            if (AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == packageIdentity.Id) !=
                null)
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug($"Skipping already loaded {packageIdentity.Id} assembly.");
                }

                continue;
            }

            var packageInstalledPath = folderNuGetProject.GetInstalledPath(packageIdentity);
            var nugetArchiveFilePath = folderNuGetProject.GetInstalledPackageFilePath(packageIdentity);

            using var packageReader = new PackageArchiveReader(nugetArchiveFilePath);
            var nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

            var packageDependencyGroup = FindDependencyGroup(currentFramework, nuspecReader);

            if (packageDependencyGroup == null && nuspecReader.GetDevelopmentDependency())
            {
                // some packages are dev packages and has nothing to load
                continue;
            }

            if (packageDependencyGroup == null)
            {
                throw new InvalidOperationException("PackageDependencyGroup snot found");
            }

            await LoadRecipePackagesAsync(packageDependencyGroup, currentFramework, folderNuGetProject,
                cancellationToken);
            LoadFrameworkSpecificGroupItemsToAssembly(currentFramework, packageReader, packageIdentity,
                packageInstalledPath);
        }
    }

    private static List<Assembly> LoadFrameworkSpecificGroupItemsToAssembly(NuGetFramework currentFramework,
        PackageArchiveReader packageReader, PackageIdentity packageIdentity, string packageInstalledPath)
    {
        var loadedAssemblies = new List<Assembly>();
        var findFrameworkSpecificGroup = FindFrameworkSpecificGroup(packageReader, currentFramework);
        foreach (var item in findFrameworkSpecificGroup.Items.Where(it => it.EndsWith(".dll")))
        {
            if (log.IsDebugEnabled)
            {
                log.Debug($"Loading assembly {item}");
            }

            // if (AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == packageIdentity.Id) !=
            //     null)
            // {
            //     continue;
            // }
            // else
            // {
            try
            {
                loadedAssemblies.Add(Assembly.LoadFrom(Path.Combine(packageInstalledPath, item)));
            }
            catch (FileLoadException e)
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug($"Failed to load assembly {item}\n{e}");
                }
            }
            // }
        }

        return loadedAssemblies;
    }

    private static PackageDependencyGroup? FindDependencyGroup(NuGetFramework currentFramework,
        NuspecReader nuspecReader)
    {
        // PackageDependencyGroup dependencyGroup;
        // dependencyGroup
        //     = nuspecReader.GetDependencyGroups()
        //         .Where(d => d.TargetFramework.Framework == currentFramework.Framework &&
        //                     d.TargetFramework.Version.CompareTo(currentFramework.Version) <= 0)
        //         .OrderBy(g => g.TargetFramework.Version).LastOrDefault();
        //
        // if (dependencyGroup == null)
        // {
        //     dependencyGroup = nuspecReader.GetDependencyGroups()
        //         .Where(d => d.TargetFramework.Framework == ".NETStandard").OrderBy(g => g.TargetFramework.Version)
        //         .Last();
        // }
        //
        // if (dependencyGroup == null)
        // {
        //     dependencyGroup = nuspecReader.GetDependencyGroups().Last();
        // }
        //
        // return dependencyGroup;

        return nuspecReader.GetDependencyGroups()
            .Where(pdg => DefaultCompatibilityProvider.Instance.IsCompatible(currentFramework, pdg.TargetFramework))
            .MinBy(pdg => pdg.Packages.Count());
    }

    private static FrameworkSpecificGroup FindFrameworkSpecificGroup(PackageArchiveReader packageArchiveReader,
        NuGetFramework currentFramework)
    {
        // FrameworkSpecificGroup dependencyGroup;
        // dependencyGroup
        //     = packageArchiveReader.GetFrameworkItems()
        //         .Where(d => d.TargetFramework.Framework == currentFramework.Framework &&
        //                     d.TargetFramework.Version.CompareTo(currentFramework.Version) <= 0)
        //         .OrderBy(g => g.TargetFramework.Version).LastOrDefault();
        //
        // if (dependencyGroup == null)
        // {
        //     dependencyGroup = packageArchiveReader.GetFrameworkItems()
        //         .Where(d => d.TargetFramework.Framework == ".NETStandard").OrderBy(g => g.TargetFramework.Version)
        //         .Last();
        //
        // }
        //
        // if (dependencyGroup == null)
        // {
        //     dependencyGroup = nuspecReader.GetDependencyGroups().Last();
        // }
        //
        // return dependencyGroup;
        // var entry = packageArchiveReader.GetEntry(packageArchiveReader.GetFiles()
        // .First(f => f.EndsWith(packageIdentity.Id + ".dll")));

        return packageArchiveReader.GetLibItems()
            .Where(pdg => DefaultCompatibilityProvider.Instance.IsCompatible(currentFramework, pdg.TargetFramework))
            .OrderBy(pdg => pdg.Items.Count())
            .First();
    }

    class NugetManagerLogger : ILogger
    {
        private static ILogger? _instance;

        public static ILogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NugetManagerLogger();
                }

                return _instance;
            }
        }

        public void LogDebug(string data)
        {
            log.Debug(data);
        }

        public void LogVerbose(string data)
        {
            log.Debug(data);
        }

        public void LogInformation(string data)
        {
            log.Info(data);
        }

        public void LogMinimal(string data)
        {
            log.Info(data);
        }

        public void LogWarning(string data)
        {
            log.Warn(data);
        }

        public void LogError(string data)
        {
            log.Error(data);
        }

        public void LogInformationSummary(string data)
        {
            log.Info(data);
        }

        public void Log(LogLevel level, string data)
        {
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Verbose:
                    log.Debug(data);
                    break;
                case LogLevel.Information:
                case LogLevel.Minimal:
                    log.Info(data);
                    break;
                case LogLevel.Warning:
                    log.Warn(data);
                    break;
                case LogLevel.Error:
                    log.Error(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message)
        {
            Log(message.Level, message.Message);
        }

        public Task LogAsync(ILogMessage message)
        {
            return LogAsync(message.Level, message.Message);
        }
    }
}
