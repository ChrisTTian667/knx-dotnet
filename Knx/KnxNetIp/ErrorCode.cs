namespace Knx.KnxNetIp;

public enum ErrorCode
{
    /// <summary>
    ///     Operation successful.
    /// </summary>
    NoError = 0,

    /// <summary>
    ///     The requested type of host protocol is not supported by the device.
    /// </summary>
    HostProtocolType = 0x01,

    /// <summary>
    ///     The requested protocol version is not supported by the device.
    /// </summary>
    VersionNotSupported = 0x02,

    /// <summary>
    ///     The received sequence number is out of order.
    /// </summary>
    SequenceNumber = 0x04,

    /// <summary>
    ///     The server device could not find an active data connection with the specified ID.
    /// </summary>
    ConnectionId = 0x21,

    /// <summary>
    ///     The server does not support the requested connection type.
    /// </summary>
    ConnectionType = 0x22,

    /// <summary>
    ///     The server does not support the requested connection options.
    /// </summary>
    ConnectionOption = 0x23,

    /// <summary>
    ///     The server could not accept a new connection, maximum reached.
    /// </summary>
    NoMoreConnections = 0x24,

    /// <summary>
    ///     The server detected an error concerning the data connection with the specified ID.
    /// </summary>
    DataConnection = 0x26,

    /// <summary>
    ///     The server detected an error concerning the KNX subsystem connection with the specified ID.
    /// </summary>
    KnxConnection = 0x27,

    /// <summary>
    ///     The requested tunneling layer is not supported by the server.
    /// </summary>
    TunnelingLayer = 0x29
}
