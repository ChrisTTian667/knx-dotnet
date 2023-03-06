using System;
using Knx.Exceptions;

namespace Knx.KnxNetIp;

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

    private static string GetErrorMessage(ErrorCode errorCode) =>
        errorCode switch
        {
            ErrorCode.NoError => "Operation successful",
            ErrorCode.HostProtocolType => "The requested type of host protocol is not supported by the device.",
            ErrorCode.VersionNotSupported => "The requested protocol version is not supported by the device.",
            ErrorCode.SequenceNumber => "The received sequence number is out of order.",
            ErrorCode.ConnectionId =>
                "The server device could not find an active data connection with the specified ID.",
            ErrorCode.ConnectionType => "The server does not support the requested connection type.",
            ErrorCode.ConnectionOption => "The server does not support the requested connection options.",
            ErrorCode.NoMoreConnections => "The server could not accept a new connection, maximum reached.",
            ErrorCode.DataConnection =>
                "The server detected an error concerning the data connection with the specified Id.",
            ErrorCode.KnxConnection =>
                "The server detected an error concerning the KNX subsystem connection with the specified ID.",
            ErrorCode.TunnelingLayer => "The requested tunneling layer is not supported by the server.",
            _ => "Unknown KnxNetIp Error."
        };
}
