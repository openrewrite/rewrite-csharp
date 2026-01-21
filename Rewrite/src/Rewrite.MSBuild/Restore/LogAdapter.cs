using Microsoft.Extensions.Logging;

namespace Rewrite.MSBuild.Restore;

internal sealed class LogAdapter(ILogger logger) : Logger
{

    protected override void LogCore(in Message message)
    {
            
        switch (message.Level)
        {
            case MessageLevel.Error:
                logger.LogError(message.Text);
                break;

            case MessageLevel.Warning:
                logger.LogWarning(message.Text);
                break;

            case MessageLevel.HighImportance:
            case MessageLevel.NormalImportance:
            case MessageLevel.LowImportance:
                logger.LogInformation(message.Text);
                break;

            default:
                throw new ArgumentException(
                    $"Message \"{message.Code}: {message.Text}\" logged with invalid Level=${message.Level}",
                    paramName: nameof(message));
        }
    }
}