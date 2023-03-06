using Knx.Common;

namespace Knx.KnxNetIp;

/// <summary>
///     Data for ConnectRequest
/// </summary>
public class ConnectRequestData
{
    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConnectRequestData" /> class.
    /// </summary>
    public ConnectRequestData()
    {
        SetDefaultValues();
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///     Toes the byte array.
    /// </summary>
    /// <returns>a <c>byte[]</c> representing this instance</returns>
    public byte[] ToByteArray()
    {
        ByteArrayToken lengthToken;

        var arrayBuilder =
            new ByteArrayBuilder().AddToken(1, out lengthToken)
                .AddByte((byte)ConnectionType)
                .AddByte(
                    (byte)NetIpLayer)
                .AddByte(0x00);

        arrayBuilder.ReplaceToken(lengthToken, arrayBuilder.Length);

        return arrayBuilder.ToByteArray();
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Sets the default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ConnectionType = ConnectionType.TunnelingConnection;
        NetIpLayer = KnxNetIpLayer.Link;
    }

    #endregion

    #region Properties

    /// <summary>
    ///     Gets or sets the type of the connection.
    /// </summary>
    /// <value>The type of the connection.</value>
    public ConnectionType ConnectionType { get; set; }

    /// <summary>
    ///     Gets or sets the KNX layer.
    /// </summary>
    /// <value>The KNX layer.</value>
    public KnxNetIpLayer NetIpLayer { get; set; }

    #endregion
}