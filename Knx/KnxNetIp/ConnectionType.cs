namespace Knx.KnxNetIp;

/// <summary>
///     Defines the different KnxNet/Ip connection types
/// </summary>
public enum ConnectionType
{
    /// <summary>
    ///     TunnelingConnection = 0x04
    /// </summary>
    TunnelingConnection = 0x04,

    /// <summary>
    ///     DeviceManagementConnection = 0x03
    /// </summary>
    DeviceManagementConnection = 0x03,

    /// <summary>
    ///     RemoteLoggingConnection = 0x06
    /// </summary>
    RemoteLoggingConnection = 0x06,

    /// <summary>
    ///     RemoteConfigurationConnection = 0x07
    /// </summary>
    RemoteConfigurationConnection = 0x07,

    /// <summary>
    ///     ObjectServerConnection = 0x08
    /// </summary>
    ObjectServerConnection = 0x08
}