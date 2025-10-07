using System.Collections.Immutable;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyModel;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Rewrite.MSBuild;

/// <summary>
/// Helper class for manipulating and analyzing .csproj files
/// </summary>
public class CsProjHelper
{
    private readonly XDocument _document;
    private readonly string _filePath;
    private readonly NuGet.Common.ILogger _nugetLogger;
    private readonly ISettings _nugetSettings;

    private const string MsBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

    public CsProjHelper(string csprojFilePath) : this(csprojFilePath, NullLogger.Instance)
    {}
    public CsProjHelper(string csprojFilePath, NuGet.Common.ILogger nugetLogger)
    {
        if (!File.Exists(csprojFilePath))
            throw new FileNotFoundException($"Project file not found: {csprojFilePath}");

        _filePath = csprojFilePath;
        _document = XDocument.Load(csprojFilePath);
        _nugetLogger = nugetLogger;
        _nugetSettings = Settings.LoadDefaultSettings(Path.GetDirectoryName(csprojFilePath));
    }

    #region Package Management

    /// <summary>
    /// Upgrades an existing package to a specific version
    /// </summary>
    public void UpgradePackage(string packageId, string version)
    {
        var packageReference = FindPackageReference(packageId);
        if (packageReference == null)
            throw new InvalidOperationException($"Package {packageId} not found in project");

        var versionAttr = packageReference.Attribute("Version");
        if (versionAttr != null)
            versionAttr.Value = version;
        else
            packageReference.Add(new XAttribute("Version", version));
    }

    /// <summary>
    /// Checks if a package is present and returns its version if found
    /// </summary>
    public string? GetPackageVersion(string packageId)
    {
        var packageReference = FindPackageReference(packageId);
        return packageReference?.Attribute("Version")?.Value;
    }

    /// <summary>
    /// Checks if a package is present in the project
    /// </summary>
    public bool HasPackage(string packageId)
    {
        return FindPackageReference(packageId) != null;
    }

    /// <summary>
    /// Removes a package from the project
    /// </summary>
    public void RemovePackage(string packageId)
    {
        var packageReference = FindPackageReference(packageId);
        if (packageReference == null)
            throw new InvalidOperationException($"Package {packageId} not found in project");

        packageReference.Remove();
    }

    /// <summary>
    /// Adds a new package reference to the project
    /// </summary>
    public void AddPackage(string packageId, string version, string? excludeAssets = null)
    {
        if (HasPackage(packageId))
            throw new InvalidOperationException($"Package {packageId} already exists in project");

        var itemGroup = _document.Root?.Elements("ItemGroup").FirstOrDefault()
            ?? _document.Root?.Elements().FirstOrDefault(e => e.Name.LocalName == "ItemGroup");

        if (itemGroup == null)
        {
            itemGroup = new XElement("ItemGroup");
            _document.Root?.Add(itemGroup);
        }

        var packageReference = new XElement("PackageReference",
            new XAttribute("Include", packageId),
            new XAttribute("Version", version));

        if (excludeAssets != null)
            packageReference.Add(new XAttribute("ExcludeAssets", excludeAssets));

        itemGroup.Add(packageReference);
    }

    /// <summary>
    /// Excludes specific asset types for a package
    /// </summary>
    public void ExcludePackageAssets(string packageId, string assetTypes)
    {
        var packageReference = FindPackageReference(packageId);
        if (packageReference == null)
            throw new InvalidOperationException($"Package {packageId} not found in project");

        var excludeAssetsAttr = packageReference.Attribute("ExcludeAssets");
        if (excludeAssetsAttr != null)
            excludeAssetsAttr.Value = assetTypes;
        else
            packageReference.Add(new XAttribute("ExcludeAssets", assetTypes));
    }

    private XElement? FindPackageReference(string packageId)
    {
        return _document.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .FirstOrDefault(e => e.Attribute("Include")?.Value == packageId);
    }

    #endregion

    #region Property Management

    /// <summary>
    /// Checks if a property exists and retrieves its value
    /// </summary>
    public string? GetProperty(string propertyName)
    {
        return FindPropertyElement(propertyName)?.Value;
    }

    /// <summary>
    /// Checks if a property exists in the project
    /// </summary>
    public bool HasProperty(string propertyName)
    {
        return FindPropertyElement(propertyName) != null;
    }

    /// <summary>
    /// Sets a property value (creates if it doesn't exist)
    /// </summary>
    public void SetProperty(string propertyName, string value)
    {
        var propertyElement = FindPropertyElement(propertyName);

        if (propertyElement != null)
        {
            propertyElement.Value = value;
        }
        else
        {
            var propertyGroup = _document.Root?.Elements("PropertyGroup").FirstOrDefault()
                ?? _document.Root?.Elements().FirstOrDefault(e => e.Name.LocalName == "PropertyGroup");

            if (propertyGroup == null)
            {
                propertyGroup = new XElement("PropertyGroup");
                _document.Root?.AddFirst(propertyGroup);
            }

            propertyGroup.Add(new XElement(propertyName, value));
        }
    }

    private XElement? FindPropertyElement(string propertyName)
    {
        return _document.Descendants()
            .Where(e => e.Name.LocalName == propertyName)
            .FirstOrDefault(e => e.Parent?.Name.LocalName == "PropertyGroup");
    }

    #endregion

    #region Common Properties

    /// <summary>
    /// Gets or sets the TargetFramework property
    /// </summary>
    public string? TargetFramework
    {
        get => GetProperty("TargetFramework");
        set => SetProperty("TargetFramework", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the TargetFrameworks property
    /// </summary>
    public string? TargetFrameworks
    {
        get => GetProperty("TargetFrameworks");
        set => SetProperty("TargetFrameworks", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the OutputType property
    /// </summary>
    public string? OutputType
    {
        get => GetProperty("OutputType");
        set => SetProperty("OutputType", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the RootNamespace property
    /// </summary>
    public string? RootNamespace
    {
        get => GetProperty("RootNamespace");
        set => SetProperty("RootNamespace", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the AssemblyName property
    /// </summary>
    public string? AssemblyName
    {
        get => GetProperty("AssemblyName");
        set => SetProperty("AssemblyName", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the LangVersion property
    /// </summary>
    public string? LangVersion
    {
        get => GetProperty("LangVersion");
        set => SetProperty("LangVersion", value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets or sets the Nullable property
    /// </summary>
    public string? Nullable
    {
        get => GetProperty("Nullable");
        set => SetProperty("Nullable", value ?? throw new ArgumentNullException(nameof(value)));
    }

    #endregion

    #region Package Analysis

    /// <summary>
    /// Gets all direct package references in the project
    /// </summary>
    public IReadOnlyCollection<PackageIdentity> GetDirectPackageReferences()
    {
        return _document.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .Select(e =>
            {
                var include = e.Attribute("Include")?.Value;
                var version = e.Attribute("Version")?.Value;
                if (include == null || version == null)
                    return null;

                if (!NuGetVersion.TryParse(version, out var nugetVersion))
                    return null;

                return new PackageIdentity(include, nugetVersion);
            })
            .Where(p => p != null)
            .Cast<PackageIdentity>()
            .ToList();
    }

    /// <summary>
    /// Retrieves list of top-level packages that have a transitive dependency of a particular package
    /// </summary>
    public async Task<IReadOnlyCollection<PackageIdentity>> GetPackagesWithTransitiveDependency(
        string dependencyPackageId,
        CancellationToken cancellationToken = default)
    {
        var lockFile = await RestoreProjectAsync(cancellationToken);
        var hierarchy = BuildDependencyHierarchy(lockFile);

        var packagesWithDependency = new List<PackageIdentity>();
        var directPackages = GetDirectPackageReferences();

        foreach (var directPackage in directPackages)
        {
            if (HasTransitiveDependency(hierarchy, directPackage, dependencyPackageId))
            {
                packagesWithDependency.Add(directPackage);
            }
        }

        return packagesWithDependency;
    }

    /// <summary>
    /// Retrieves the full transitive hierarchy of packages
    /// </summary>
    public async Task<Dictionary<PackageIdentity, IReadOnlyCollection<PackageIdentity>>> GetTransitiveDependencyHierarchy(
        CancellationToken cancellationToken = default)
    {
        var lockFile = await RestoreProjectAsync(cancellationToken);
        return BuildDependencyHierarchy(lockFile);
    }

    /// <summary>
    /// Retrieves which assemblies (and their versions) will be included based on selected packages
    /// </summary>
    public async Task<IReadOnlyCollection<AssemblyInfo>> GetRequiredAssemblies(
        CancellationToken cancellationToken = default)
    {
        var lockFile = await RestoreProjectAsync(cancellationToken);
        return GetAssembliesFromLockFile(lockFile);
    }

    private async Task<LockFile> RestoreProjectAsync(CancellationToken cancellationToken)
    {
        var directPackages = GetDirectPackageReferences();
        var framework = GetTargetFramework();

        var packageSpec = CreatePackageSpec(directPackages, framework);
        var cachingSourceProvider = GetCachingProvider();

        return await RestoreProject(packageSpec, cachingSourceProvider, cancellationToken);
    }

    private NuGetFramework GetTargetFramework()
    {
        var targetFramework = TargetFramework;
        if (targetFramework == null)
        {
            var targetFrameworks = TargetFrameworks?.Split(';').FirstOrDefault();
            targetFramework = targetFrameworks ?? "net9.0";
        }

        return NuGetFramework.ParseFolder(targetFramework);
    }

    private PackageSpec CreatePackageSpec(IReadOnlyCollection<PackageIdentity> packages, NuGetFramework framework)
    {
        var projectName = Path.GetFileNameWithoutExtension(_filePath);
        var projectDir = Path.GetDirectoryName(_filePath)!;

        var libDependencies = packages
            .Select(package => new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                    package.Id,
                    new VersionRange(package.Version),
                    LibraryDependencyTarget.Package)
            })
            .ToImmutableArray();

        var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>
        {
            new TargetFrameworkInformation
            {
                FrameworkName = framework,
                Dependencies = libDependencies
            }
        })
        {
            Name = projectName,
            FilePath = _filePath,
            RestoreMetadata = new ProjectRestoreMetadata
            {
                ProjectUniqueName = projectName,
                ProjectName = projectName,
                ProjectPath = _filePath,
                ProjectStyle = ProjectStyle.PackageReference,
                OutputPath = Path.Combine(projectDir, "obj"),
                TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                {
                    new(framework)
                }
            }
        };

        return packageSpec;
    }

    private CachingSourceProvider GetCachingProvider(IReadOnlyCollection<PackageSource>? packageSources = null)
    {
        packageSources ??= [new PackageSource(RecipeManager.NugetOrgRepository)];
        var sourceProvider = new PackageSourceProvider(_nugetSettings, packageSources);
        var cachingSourceProvider = new CachingSourceProvider(sourceProvider);
        return cachingSourceProvider;
    }

    private async Task<LockFile> RestoreProject(
        PackageSpec packageSpec,
        CachingSourceProvider cachingSourceProvider,
        CancellationToken cancellationToken)
    {
        var globalPackagesPath = SettingsUtility.GetGlobalPackagesFolder(_nugetSettings);
        var fallbackFolders = SettingsUtility.GetFallbackPackageFolders(_nugetSettings);
        var sources = cachingSourceProvider.GetRepositories().ToList();

        var cacheContext = new SourceCacheContext();
        var clientPolicy = ClientPolicyContext.GetClientPolicy(_nugetSettings, _nugetLogger);
        var mapping = PackageSourceMapping.GetPackageSourceMapping(_nugetSettings);

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

        var command = new RestoreCommand(request);
        var result = await command.ExecuteAsync(cancellationToken);

        if (!result.Success)
        {
            var errors = string.Join(Environment.NewLine, result.LockFile.LogMessages.Select(m => m.Message));
            throw new InvalidOperationException($"Package restore failed: {errors}");
        }

        return result.LockFile;
    }

    private Dictionary<PackageIdentity, IReadOnlyCollection<PackageIdentity>> BuildDependencyHierarchy(LockFile lockFile)
    {
        var hierarchy = new Dictionary<PackageIdentity, IReadOnlyCollection<PackageIdentity>>();

        var target = lockFile.Targets.FirstOrDefault();
        if (target == null)
            return hierarchy;

        foreach (var library in target.Libraries)
        {
            if (library.Name == null || library.Version == null)
                continue;

            var packageIdentity = new PackageIdentity(library.Name, library.Version);
            var dependencies = library.Dependencies
                .Select(dep => new PackageIdentity(dep.Id, dep.VersionRange.MinVersion ?? new NuGetVersion(0, 0, 0)))
                .ToList();

            hierarchy[packageIdentity] = dependencies;
        }

        return hierarchy;
    }

    private bool HasTransitiveDependency(
        Dictionary<PackageIdentity, IReadOnlyCollection<PackageIdentity>> hierarchy,
        PackageIdentity rootPackage,
        string targetPackageId)
    {
        var visited = new HashSet<string>();
        return HasTransitiveDependencyRecursive(hierarchy, rootPackage, targetPackageId, visited);
    }

    private bool HasTransitiveDependencyRecursive(
        Dictionary<PackageIdentity, IReadOnlyCollection<PackageIdentity>> hierarchy,
        PackageIdentity currentPackage,
        string targetPackageId,
        HashSet<string> visited)
    {
        if (visited.Contains(currentPackage.Id))
            return false;

        visited.Add(currentPackage.Id);

        if (currentPackage.Id.Equals(targetPackageId, StringComparison.OrdinalIgnoreCase))
            return true;

        if (!hierarchy.TryGetValue(currentPackage, out var dependencies))
            return false;

        foreach (var dependency in dependencies)
        {
            if (HasTransitiveDependencyRecursive(hierarchy, dependency, targetPackageId, visited))
                return true;
        }

        return false;
    }

    private IReadOnlyCollection<AssemblyInfo> GetAssembliesFromLockFile(LockFile lockFile)
    {
        var globalPackagesPath = SettingsUtility.GetGlobalPackagesFolder(_nugetSettings);

        var libsToPath = lockFile.Libraries.ToDictionary(x => (x.Name, x.Version), x => x.Path);
        var target = lockFile.Targets.FirstOrDefault();

        if (target == null)
            return Array.Empty<AssemblyInfo>();

        var libraryDlls = target.Libraries
            .SelectMany(targetLib => targetLib.RuntimeAssemblies
                .Select(assembly => new AssemblyInfo(
                    PackageId: targetLib.Name!,
                    PackageVersion: targetLib.Version!.ToString(),
                    AssemblyPath: Path.Combine(
                        globalPackagesPath,
                        libsToPath[(targetLib.Name!, targetLib.Version!)],
                        assembly.Path),
                    AssemblyName: Path.GetFileNameWithoutExtension(assembly.Path))))
            .Where(a => !a.AssemblyName.Equals("_._"))
            .ToList();

        return libraryDlls;
    }

    #endregion

    #region Save

    /// <summary>
    /// Saves changes back to the csproj file
    /// </summary>
    public void Save()
    {
        _document.Save(_filePath);
    }

    /// <summary>
    /// Saves changes to a different file
    /// </summary>
    public void SaveAs(string filePath)
    {
        _document.Save(filePath);
    }

    #endregion
}

/// <summary>
/// Information about an assembly included in the project
/// </summary>
public record AssemblyInfo(
    string PackageId,
    string PackageVersion,
    string AssemblyPath,
    string AssemblyName);
