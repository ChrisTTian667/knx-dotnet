using System;

namespace Knx.Exceptions
{
    /// <summary>
    /// Base exception for all knx protocol relative exceptions
    /// </summary>
    public class KnxException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public KnxException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxException"/> class.
        /// ==> will copy the exception message of the innerException to this exception (by default)
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public KnxException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public KnxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }

    /// <summary>
    /// Is thrown when a message could not be processed in the given timespan.
    /// </summary>
    public class KnxTimeoutException : KnxException
    {
        public KnxTimeoutException(string message) : base(message)
        {
        }

        public KnxTimeoutException(Exception innerException) : base(innerException)
        {
        }

        public KnxTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}