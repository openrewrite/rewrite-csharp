using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Task = Microsoft.Build.Utilities.Task;

namespace Rewrite.MSBuild.Restore;

public abstract class TaskBase
{

    internal TaskBase(Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        logger ??= LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger(this.GetType());
        this.Log = new LogAdapter(logger);
    }

    internal Logger Log { get; set; }

    public bool Execute()
    {
        try
        {
            ExecuteCore();
        }
        catch (BuildErrorException e)
        {
            Log.LogError(e.Message);
        }

        return !Log.HasLoggedErrors;
    }



    private static string ExceptionToStringWithoutMessage(Exception e)
    {
        const string AggregateException_ToString = "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}";
        if (e is AggregateException aggregate)
        {
            string text = NonAggregateExceptionToStringWithoutMessage(aggregate);

            for (int i = 0; i < aggregate.InnerExceptions.Count; i++)
            {
                text = string.Format(CultureInfo.InvariantCulture,
                    AggregateException_ToString,
                    text,
                    Environment.NewLine,
                    i,
                    ExceptionToStringWithoutMessage(aggregate.InnerExceptions[i]),
                    "<---",
                    Environment.NewLine);
            }

            return text;
        }
        else
        {
            return NonAggregateExceptionToStringWithoutMessage(e);
        }
    }

    private static string NonAggregateExceptionToStringWithoutMessage(Exception e)
    {
        string s;
        const string Exception_EndOfInnerExceptionStack = "--- End of inner exception stack trace ---";


        s = e.GetType().ToString();

        if (e.InnerException != null)
        {
            s = s + " ---> " + ExceptionToStringWithoutMessage(e.InnerException) + Environment.NewLine +
                "   " + Exception_EndOfInnerExceptionStack;

        }

        var stackTrace = e.StackTrace;

        if (stackTrace != null)
        {
            s += Environment.NewLine + stackTrace;
        }

        return s;
    }

    protected abstract void ExecuteCore();
}