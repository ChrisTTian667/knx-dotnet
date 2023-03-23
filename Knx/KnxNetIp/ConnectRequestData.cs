using Knx.Common;

namespace Knx.KnxNetIp;

/// <summary>
///     Data for ConnectRequest
/// </summary>
public class ConnectRequestData
{
    /// <summary>
    ///     Gets or sets the type of the connection.
    /// </summary>
    /// <value>The type of the connection.</value>
    public ConnectionType ConnectionType { get; set; } = ConnectionType.TunnelingConnection;

    /// <summary>
    ///     Gets or sets the KNX layer.
    /// </summary>
    /// <value>The KNX layer.</value>
    public KnxNetIpLayer NetIpLayer { get; set; } = KnxNetIpLayer.Link;

    /// <summary>
    ///     Toes the byte array.
    /// </summary>
    /// <returns>a <c>byte[]</c> representing this instance</returns>
    public byte[] ToByteArray()
    {
        var arrayBuilder =
            new ByteArrayBuilder().AddToken(1, out var lengthToken)
                .AddByte((byte)ConnectionType)
                .AddByte(
                    (byte)NetIpLayer)
                .AddByte(0x00);

        arrayBuilder.ReplaceToken(lengthToken, arrayBuilder.Length);

        return arrayBuilder.ToByteArray();
    }
}