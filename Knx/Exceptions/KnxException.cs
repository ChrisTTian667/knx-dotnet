using System;

namespace Knx.Exceptions;

/// <summary>
///     Base exception for all knx protocol relative exceptions
/// </summary>
public class KnxException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxException" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public KnxException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxException" /> class.
    ///     ==> will copy the exception message of the innerException to this exception (by default)
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public KnxException(Exception innerException)
        : base(innerException.Message, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxException" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public KnxException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
