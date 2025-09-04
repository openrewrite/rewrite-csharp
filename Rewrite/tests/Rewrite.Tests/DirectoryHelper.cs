// using NMica.Utils.Collections;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace Rewrite.Tests;

public class DirectoryHelper
{
    private const string NukeDirectoryName = ".nuke";
    private static Lazy<AbsolutePath> _rootDirectory = new(() => GetRootDirectoryFrom((AbsolutePath)Directory.GetCurrentDirectory()));
    public static AbsolutePath  RepositoryRoot => _rootDirectory.Value;
    internal static AbsolutePath GetRootDirectoryFrom(AbsolutePath startDirectory)
    {
        var rootDirectory = new DirectoryInfo(startDirectory)
            .DescendantsAndSelf(x => x.Parent!)
            .FirstOrDefault(x => x.GetDirectories(NukeDirectoryName).Any())
            ?.FullName;
        if(rootDirectory == null)
            throw new Exception("Could not find root directory");
        return (AbsolutePath)rootDirectory!;
    }
}