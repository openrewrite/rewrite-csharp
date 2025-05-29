using System.Diagnostics;

namespace Rewrite.Core.Internal;

public static class ExceptionUtils
{
    public static string SanitizeStackTrace(this Exception exception, Type until)
    {
        return exception.ToString();
    }
}
