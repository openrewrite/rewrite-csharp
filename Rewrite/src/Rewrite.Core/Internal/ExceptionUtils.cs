using System.Diagnostics;

namespace Rewrite.Core.Internal;

public static class ExceptionUtils
{
    public static string SanitizeStackTrace(this Exception exception, Type until)
    {
#if SANITIZE_STACK_TRACE
        var sanitized = "";
        sanitized = string.Join("\n", sanitized, exception.GetType().Name + ": " + exception.Message);

        var i = 0;
        foreach (var stackTraceElement in new StackTrace(exception).GetFrames()
                     .Select(x => x.GetMethod()?.DeclaringType)
                     .Where(x => x != null)
                     .Cast<Type>()
                     .TakeWhile(originatingClass => originatingClass != until))
        {
            if (i++ >= 16)
            {
                sanitized = string.Join("\n", sanitized, "  ...");
                break;
            }

            sanitized = string.Join("\n", sanitized, "  " + stackTraceElement);
        }

        return sanitized;
#else
        return exception.ToString();
#endif
    }
}
