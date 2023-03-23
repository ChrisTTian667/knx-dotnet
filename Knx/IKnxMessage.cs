namespace Knx;

public interface IKnxMessage
{
    /// <summary>
    ///     Gets or sets the message code.
    /// </summary>
    /// <value>The message code.</value>
    MessageCode MessageCode { get; set; }

    /// <summary>
    ///     Gets or sets the message type.
    /// </summary>
    MessageType MessageType { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this is a repetition.
    /// </summary>
    /// <value><c>true</c> if repetition; otherwise, <c>false</c>.</value>
    bool IsRepetition { get; set; }

    /// <summary>
    ///     Gets or sets the priority.
    /// </summary>
    /// <value>The priority.</value>
    MessagePriority Priority { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this message is a positiv confirmation.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this message is a positive confirmation; otherwise, <c>false</c>.
    /// </value>
    bool IsPositiveConfirmation { get; set; }

    /// <summary>
    ///     Gets or sets the additional info.
    /// </summary>
    /// <value>The additional info.</value>
    byte[] AdditionalInfo { get; set; }

    /// <summary>
    ///     Gets or sets the source (device-) address.
    /// </summary>
    /// <value>The source address.</value>
    KnxDeviceAddress? SourceAddress { get; set; }

    /// <summary>
    ///     Gets or sets the destination address (device or individual/logical address).
    /// </summary>
    /// <value>The destination address.</value>
    KnxAddress DestinationAddress { get; set; }

    /// <summary>
    ///     Gets the hop count, a message did, till it was received.
    /// </summary>
    /// <value>The hop count.</value>
    byte HopCount { get; }

    /// <summary>
    ///     Gets or sets the length of the data (after the 8th Byte).
    /// </summary>
    /// <value>The length of the data.</value>
    byte PayloadLength { get; }

    /// <summary>
    ///     Gets or sets the payload of the message (Data)
    /// </summary>
    byte[] Payload { get; set; }

    /// <summary>
    ///     Gets or sets the transport layer control info.
    /// </summary>
    /// <value>The transport layer control info.</value>
    TransportLayerControlInfo TransportLayerControlInfo { get; set; }

    /// <summary>
    ///     Gets or sets the data packet count, in case of an numbered transport layer type (e.g. NCD or NDT)
    /// </summary>
    /// <value>The data packet count.</value>
    byte DataPacketCount { get; set; }

    /// <summary>
    ///     Toes the byte array.
    /// </summary>
    /// <returns></returns>
    byte[] ToByteArray();
}