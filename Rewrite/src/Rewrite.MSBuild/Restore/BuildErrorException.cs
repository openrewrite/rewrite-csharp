using System.Globalization;

namespace Rewrite.MSBuild.Restore;

/// <summary>
/// Represents an error that is neither avoidable in all cases nor indicative of a bug in this library.
/// It will be logged as a plain build error without the exception type or stack.
/// </summary>
internal class BuildErrorException : Exception
{
    public BuildErrorException()
    {
    }

    public BuildErrorException(string message) : base(message)
    {
    }

    public BuildErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public BuildErrorException(string format, params string[] args)
        : this(string.Format(CultureInfo.CurrentCulture, format, args))
    {
    }
}