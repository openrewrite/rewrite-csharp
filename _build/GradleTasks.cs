
using System;
using Nuke.Common;
using Nuke.Common.Tooling;
using Serilog;

partial class GradleTasks
{

    public GradleTasks()
    {

        SetToolPath(NukeBuild.RootDirectory / "gradlew.bat");
    }
}
partial class GradleSettings
{
    public GradleSettings()
    {
        Set(() => ProcessWorkingDirectory, NukeBuild.RootDirectory);
    }
}

partial class KnownGradleTasks
{
    public override string ToString()
    {
        var str = base.ToString();
        var result = $"{str[0].ToString().ToLowerInvariant()}{str[1..]}";
        return result;
    }
}
