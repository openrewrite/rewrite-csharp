namespace Rewrite.Test;

public static class EnvironmentInfo
{
    public static bool IsCI => Environment.GetEnvironmentVariable("CI")?.ToLowerInvariant() == "true";
}
