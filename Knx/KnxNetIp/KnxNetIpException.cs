using System;
using Knx.Exceptions;

namespace Knx.KnxNetIp
{
    public class KnxNetIpException : KnxException
    {
        public KnxNetIpException(string message) : base(message)
        {
        }

        public KnxNetIpException(Exception innerException) : base(innerException)
        {
        }

        public KnxNetIpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public KnxNetIpException(ErrorCode errorCode)
            : this(GetErrorMessage(errorCode))
        {
        }

        private static string GetErrorMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.NoError:
                    return "Operation successfull";
                case ErrorCode.HostProtocolType:
                    return "The requested type of host protocol is not supported by the device.";
                case ErrorCode.VersionNotSupported:
                    return "The requested protocol version is not supported by the device.";
                case ErrorCode.SequenceNumber:
                    return "The received sequence number is out of order.";
                case ErrorCode.ConnectionId:
                    return "The server device could not find an active data connection with the specified ID.";
                case ErrorCode.ConnectionType:
                    return "The server does not support the requested connection type.";
                case ErrorCode.ConnectionOption:
                    return "The server does not support the requested connection options.";
                case ErrorCode.NoMoreConnections:
                    return "The server could not accept a new connection, maximum reached.";
                case ErrorCode.DataConnection:
                    return "The server detected an error concerning the data connection with the specified Id.";
                case ErrorCode.KnxConnection:
                    return "The server detected an error concerning the KNX subsystem connection with the specified ID.";
                case ErrorCode.TunnelingLayer:
                    return "The requested tunneling layer is not supported by the server.";
                default:
                    return "Unknown KnxNetIp Error.";
            }
        }
    }
}
