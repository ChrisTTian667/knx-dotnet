using System;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp;

/// <summary>
///     base class, for the base class.
///     Acts as a message factory.
/// </summary>
//[TypeConverter(typeof(KnxNetIpMessageConverter))]
public abstract class KnxNetIpMessage
{
    /// <summary>
    ///     The size of the header (in bytes)
    /// </summary>
    public static readonly byte HeaderLength = 6;

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes to be deserialized.</param>
    protected abstract void Deserialize(byte[] bytes);

    /// <summary>
    ///     Gets or sets the body.
    /// </summary>
    /// <value>The body.</value>
    public MessageBodyBase Body { get; internal set; }

    /// <summary>
    ///     Gets or sets the type of the message.
    /// </summary>
    /// <value>The type of the message.</value>
    public KnxNetIpServiceType ServiceType { get; set; }

    /// <summary>
    ///     Creates the specified message by type.
    /// </summary>
    /// <param name="serviceType">Type of the message.</param>
    /// <returns></returns>
    public static KnxNetIpMessage Create(KnxNetIpServiceType serviceType)
    {
        switch (serviceType)
        {
            case KnxNetIpServiceType.ConnectionRequest:
                return new KnxNetIpMessage<ConnectionRequest>();
            case KnxNetIpServiceType.ConnectionResponse:
                return new KnxNetIpMessage<ConnectionResponse>();
            case KnxNetIpServiceType.ConnectionStateRequest:
                return new KnxNetIpMessage<ConnectionStateRequest>();
            case KnxNetIpServiceType.ConnectionStateResponse:
                return new KnxNetIpMessage<ConnectionStateResponse>();
            case KnxNetIpServiceType.DescriptionRequest:
                return new KnxNetIpMessage<DescriptionRequest>();
            case KnxNetIpServiceType.DescriptionResponse:
                return new KnxNetIpMessage<DescriptionResponse>();
            case KnxNetIpServiceType.DisconnectRequest:
                return new KnxNetIpMessage<DisconnectRequest>();
            case KnxNetIpServiceType.DisconnectResponse:
                return new KnxNetIpMessage<DisconnectResponse>();
            case KnxNetIpServiceType.SearchRequest:
                return new KnxNetIpMessage<SearchRequest>();
            case KnxNetIpServiceType.SearchResponse:
                return new KnxNetIpMessage<SearchResponse>();
            case KnxNetIpServiceType.TunnelingRequest:
                return new KnxNetIpMessage<TunnelingRequest>();
            case KnxNetIpServiceType.TunnelingAcknowledge:
                return new KnxNetIpMessage<TunnelingAcknowledge>();
            case KnxNetIpServiceType.RoutingIndication:
                return new KnxNetIpMessage<RoutingIndication>();
            case KnxNetIpServiceType.RoutingLostMessage:
                return new KnxNetIpMessage<LostMessageIndication>();
            default:
                throw new ArgumentException("Knx message body unknown!");
        }
    }

    /// <summary>
    ///     Parses the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns>a new KnxMessageHeader</returns>
    public static KnxNetIpMessage Parse(byte[] bytes)
    {
        if (bytes.Length < HeaderLength) throw new ArgumentException("Could not parse message header");

        var messageType = (KnxNetIpServiceType)((bytes[2] << 8) + bytes[3]);
        var message = Create(messageType);

        message.Deserialize(bytes);
        message.ServiceType = messageType;

        return message;
    }

    /// <summary>
    ///     Tries the parse.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes">The bytes.</param>
    /// <param name="message">The message.</param>
    /// <returns><c>true</c>, if the parse was possible; otherwise <c>false</c></returns>
    public static bool TryParse<T>(byte[] bytes, out KnxNetIpMessage<T> message) where T : TunnelingMessageBody, new()
    {
        message = null;

        // prevent throwing a Parse exception
        if (bytes.Length < HeaderLength)
            return false;

        try
        {
            message = Parse(bytes) as KnxNetIpMessage<T>;

            return message != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     Determines whether the specified bytes are message of specified service type.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <param name="service">The service.</param>
    /// <returns>
    ///     <c>true</c> if the specified bytes are message of specified service type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsService(byte[] bytes, KnxNetIpServiceType service)
    {
        return bytes.Length >= HeaderLength && (KnxNetIpServiceType)((bytes[2] << 8) + bytes[3]) == service;
    }

    /// <summary>
    ///     Does the byte array.
    /// </summary>
    /// <returns>a new <c>byte[]</c> representing it's message data</returns>
    public abstract byte[] ToByteArray();
}