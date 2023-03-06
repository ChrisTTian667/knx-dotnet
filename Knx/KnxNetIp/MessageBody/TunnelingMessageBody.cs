namespace Knx.KnxNetIp.MessageBody;

/// <summary>
///     base class for all message body's
/// </summary>
public abstract class TunnelingMessageBody : MessageBodyBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TunnelingMessageBody" /> class.
    /// </summary>
    protected TunnelingMessageBody()
    {
        CommunicationChannel = null;
    }

    /// <summary>
    ///     Gets or sets the communiction channel.
    /// </summary>
    /// <value>The communiction channel.</value>
    public byte? CommunicationChannel { get; set; }

    public override string ToString()
    {
        return string.Format(
            "Ch: {0,2} {1, -24}",
            CommunicationChannel == null ? "na" : CommunicationChannel.ToString(),
            GetType().Name);
    }
}