using System;

namespace Knx.Exceptions;

/// <summary>
///     Is thrown when a message could not be processed in the given timespan.
/// </summary>
public class TimeoutException : KnxException
{
    public TimeoutException(string message) : base(message)
    {
    }

    public TimeoutException(Exception innerException) : base(innerException)
    {
    }

    public TimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
