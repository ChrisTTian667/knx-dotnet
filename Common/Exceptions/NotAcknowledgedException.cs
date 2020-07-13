using System;

namespace Knx.Common.Exceptions
{
    /// <summary>
    /// Is thrown when the expected response to a message is not retrieved. (e.g. on SendAndReceiveReply)
    /// </summary>
    public class NotAcknowledgedException : KnxException
    {
        public NotAcknowledgedException(string message) : base(message)
        {
        }

        public NotAcknowledgedException(Exception innerException) : base(innerException)
        {
        }

        public NotAcknowledgedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}