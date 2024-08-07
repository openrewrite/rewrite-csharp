namespace Rewrite.Core.Internal;

public class ExceptionUtils
{
    public static string SanitizeStackTrace(Exception t, Type until)
    {
        var cause = t;
        var sanitized = "";
        sanitized = string.Join("\n", sanitized, cause.GetType().Name + ": " + cause.Message);

        var i = 0;
        foreach (var stackTraceElement in cause.StackTrace.TakeWhile(stackTraceElement =>
                     !stackTraceElement.Equals(until.Name)))
        {
            if (i++ >= 16)
            {
                sanitized = string.Join("\n", sanitized, "  ...");
                break;
            }

            sanitized = string.Join("\n", sanitized, "  " + stackTraceElement);
        }

        return sanitized;
    }
}