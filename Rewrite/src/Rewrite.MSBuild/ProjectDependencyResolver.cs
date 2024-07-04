using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Rewrite.MSBuild;

public class ProjectDependencyResolver
{
    static ProjectDependencyResolver()
    {
        var instance = MSBuildLocator.QueryVisualStudioInstances().OrderByDescending(instance => instance.Version)
            .First();
        MSBuildLocator.RegisterInstance(instance);
    }

    private readonly Regex? _packagePattern;
    private readonly Project _project;
    private readonly string _globalPackagesFolder;
    private readonly NuGetFramework _projectFramework;
    private readonly ILogger _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly SourceCacheContext _cache;
    private readonly SourceRepository _repository;
    private readonly HashSet<Dependency> _result = [];


    private ProjectDependencyResolver(string projectFileContent, string? packagePattern)
    {
        _packagePattern = packagePattern != null
            ? new Regex("^" + packagePattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", ".") + "$")
            : null;

        using var stringReader = new StringReader(projectFileContent);
        using var xmlReader = XmlReader.Create(stringReader);

        var projectRootElement = ProjectRootElement.Create(xmlReader);
        var projectOptions = new ProjectOptions();
        // projectOptions.GlobalProperties = new Dictionary<string, string>
        // {
        //     { "Configuration", "Debug" }
        // };
        _project = Project.FromProjectRootElement(
            projectRootElement, projectOptions);
        var settings = Settings.LoadDefaultSettings(root: null);
        _globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(settings) ??
                                Path.Combine(Path.GetTempPath(), "/nuget");
        var targetFramework = _project.GetPropertyValue("TargetFramework")!;
        _projectFramework = NuGetFramework.ParseFolder(targetFramework);

        var projectItems = _project.GetItems("Compile").ToList();
        foreach (var projectItem in projectItems)
        {
            Console.WriteLine(projectItem.EvaluatedInclude);
        }


        _logger = NullLogger.Instance;
        _cancellationToken = CancellationToken.None;

        _cache = new SourceCacheContext();
        _repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
    }

    public static IEnumerable<Dependency> TransitiveProjectDependencies(string projectFileContents, string? package)
    {
        var dependencyResolver = new ProjectDependencyResolver(projectFileContents, package);
        return dependencyResolver.ResolveProject().GetAwaiter().GetResult();
    }

    private async Task<IEnumerable<Dependency>> ResolveProject()
    {
        foreach (var packageReference in _project.GetItems("PackageReference"))
        {
            var packageName = packageReference.EvaluatedInclude;
            if (!packageReference.HasMetadata("Version") ||
                packageReference.GetMetadataValue("Version").Length == 0) continue;

            var packageVersion = packageReference.GetMetadataValue("Version");
            try
            {
                var versionRange = VersionRange.Parse(packageVersion);
                NuGetVersion? nuGetVersion;
                if (versionRange.IsFloating || !versionRange.IsMinInclusive || !versionRange.IsMaxInclusive ||
                    versionRange.MinVersion != versionRange.MaxVersion || packageVersion.Contains('['))
                {
                    nuGetVersion = await GetLatestVersionInRange(packageName, versionRange);
                }
                else
                {
                    nuGetVersion = NuGetVersion.Parse(packageVersion);
                }

                if (nuGetVersion != null)
                {
                    var identity = new PackageIdentity(packageName, nuGetVersion);
                    await ResolveDependency(identity);
                }
            }
            catch (ArgumentException ignore)
            {
            }
        }

        return _packagePattern != null ? _result.Where(d => _packagePattern.IsMatch(d.Package)) : _result;
    }

    async Task<NuGetVersion?> GetLatestVersionInRange(string packageId, VersionRange versionRange)
    {
        var resource = await _repository.GetResourceAsync<FindPackageByIdResource>(_cancellationToken);
        IEnumerable<NuGetVersion> versions =
            await resource.GetAllVersionsAsync(packageId, _cache, NullLogger.Instance, CancellationToken.None);
        return versions.Where(versionRange.Satisfies).MaxBy(v => v);
    }

    private async Task ResolveDependency(PackageIdentity identity)
    {
        if (!_result.Add(new Dependency(identity.Id, identity.Version.ToString())))
        {
            return;
        }

        var downloadResource = await _repository.GetResourceAsync<DownloadResource>(_cancellationToken);
        var result = await downloadResource.GetDownloadResourceResultAsync(identity,
            new PackageDownloadContext(_cache), _globalPackagesFolder, _logger, _cancellationToken);
        if (result.Status == DownloadResourceResultStatus.NotFound)
        {
            return;
        }

        var nuspecReader = new NuspecReader(await result.PackageReader.GetNuspecAsync(_cancellationToken));
        IEnumerable<PackageDependencyGroup?> groups = nuspecReader.GetDependencyGroups();
        var suitableGroup = groups
            .FirstOrDefault(g => _projectFramework.Equals(g.TargetFramework) || g.TargetFramework.IsAny);
        if (suitableGroup != null)
        {
            foreach (var dependency in suitableGroup.Packages)
            {
                if (dependency.VersionRange.MinVersion != null)
                {
                    var transitiveIdentity = new PackageIdentity(dependency.Id,
                        NuGetVersion.Parse(dependency.VersionRange.MinVersion!.ToString()));
                    await ResolveDependency(transitiveIdentity);
                }
            }
        }
    }
}