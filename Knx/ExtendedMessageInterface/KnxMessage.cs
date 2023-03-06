using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Knx.Common;

namespace Knx.ExtendedMessageInterface;

/// <summary>
///     The extended KnxMessage (Common extended message interface)
/// </summary>
public class KnxMessage : IKnxMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxMessage" /> class.
    ///     (sets the default values)
    /// </summary>
    public KnxMessage()
    {
        MessageCode = MessageCode.Indication;
        IsPositivConfirmation = false;
        AdditionalInfo = new byte[] { 0 };
    }

    /// <summary>
    ///     Calculates the security byte.
    /// </summary>
    /// <param name="messageArray">The message array.</param>
    /// <returns>a security <c>byte</c> representing the sum of all positiv bits</returns>
    internal byte CalculateSecurityByte(BitArray messageArray)
    {
        return messageArray.Cast<bool>()
            .Aggregate<bool, byte>(
                0,
                (current, bit) =>
                    (byte)(current + (byte)(bit ? 1 : 0)));
    }

    /// <summary>
    ///     Gets the MessageType and Payload byte[]
    /// </summary>
    /// <returns>ByteArray containing the Payload combined with the MessageType</returns>
    private byte[] GetMessageTypeAndPayload()
    {
        // use the first 2 Bits for the Messagetype
        // the next 6 Bits representing the first byte of the payload.

        var firstByte = (byte)((Payload.FirstOrDefault() & 0x3F) | (byte)MessageType);
        var resultBuilder = new ByteArrayBuilder().AddByte(firstByte);

        if (Payload.Length > 1)
            resultBuilder.Add(Payload.ExtractBytes(1));

        return resultBuilder.ToByteArray();
    }

    public override string ToString()
    {
        return string.Format(
            "{0,-12} {3, 8} => {4,-8} P:{1,-6} DPC:{6, -1} L:{2,-3} Payload: {5}",
            MessageCode,
            Priority,
            PayloadLength,
            SourceAddress,
            DestinationAddress,
            GetPayloadAsString(),
            DataPacketCount);
    }

    private string GetPayloadAsString()
    {
        return _payload == null
            ? string.Empty
            : _payload.Aggregate(string.Empty, (current, b) => current + b.ToString(CultureInfo.InvariantCulture));
    }

    private readonly ControlByte1 _controlByte1 = new();
    private readonly ControlByte2 _controlByte2 = new();

    private byte[] _payload;

    /// <summary>
    ///     Gets or sets the message code.
    /// </summary>
    /// <value>The message code.</value>
    public MessageCode MessageCode { get; set; }

    /// <summary>
    ///     Gets or sets the message type.
    /// </summary>
    public MessageType MessageType { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this <see cref="KnxMessage" /> is a repetition.
    /// </summary>
    /// <value><c>true</c> if repetition; otherwise, <c>false</c>.</value>
    public bool IsRepetition
    {
        get => MessageCode == MessageCode.Indication ? !_controlByte1.IsRepetition : _controlByte1.IsRepetition;
        set => _controlByte1.IsRepetition = MessageCode == MessageCode.Indication ? !value : value;
    }

    /// <summary>
    ///     Gets or sets the priority.
    /// </summary>
    /// <value>The priority.</value>
    public MessagePriority Priority
    {
        get => _controlByte1.Priority;
        set => _controlByte1.Priority = value;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether this message is a positiv confirmation.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this message is a positiv confirmation; otherwise, <c>false</c>.
    /// </value>
    public bool IsPositivConfirmation
    {
        get => _controlByte1.IsPositivConfirmation;
        set => _controlByte1.IsPositivConfirmation = value;
    }

    /// <summary>
    ///     Gets or sets the additional info.
    /// </summary>
    /// <value>The additional info.</value>
    public byte[] AdditionalInfo { get; set; }

    /// <summary>
    ///     Gets or sets the source (device-) address.
    /// </summary>
    /// <value>The source address.</value>
    public KnxDeviceAddress SourceAddress { get; set; }

    /// <summary>
    ///     Gets or sets the destination address (device or individual/logical address).
    /// </summary>
    /// <value>The destination address.</value>
    public KnxAddress DestinationAddress { get; set; }

    /// <summary>
    ///     Gets the hop count, a message did, till it was received.
    /// </summary>
    /// <value>The hop count.</value>
    public byte HopCount => _controlByte2.Hops;

    /// <summary>
    ///     Gets or sets the length of the data (after the 8th Byte).
    /// </summary>
    /// <value>The length of the data.</value>
    public byte PayloadLength => (byte)Math.Max(Payload.Length, 1);

    /// <summary>
    ///     Gets or sets the payload of the message (Data)
    /// </summary>
    public byte[] Payload
    {
        get { return _payload ??= new byte[] { }; }
        set => _payload = value;
    }

    /// <summary>
    ///     Gets or sets the transport layer control info.
    /// </summary>
    /// <value>The transport layer control info.</value>
    public TransportLayerControlInfo TransportLayerControlInfo { get; set; }

    /// <summary>
    ///     Gets or sets the data packet count, in case of an numbered transport layer type (e.g. NCD or NDT)
    /// </summary>
    /// <value>The data packet count.</value>
    public byte DataPacketCount { get; set; }

    /// <summary>
    ///     Toes the byte array.
    /// </summary>
    /// <returns></returns>
    public byte[] ToByteArray()
    {
        var result = new ByteArrayBuilder()
            .AddByte((byte)MessageCode)
            .Add(AdditionalInfo) // by default 1-byte
            .AddByte(_controlByte1.ToByte())
            .AddByte(_controlByte2.ToByte())

            // Source & Destination
            .AddBitArray(SourceAddress.ToBitArray())
            .AddBitArray(DestinationAddress.ToBitArray())

            // add a length byte ( again, don't know if this correct, but for now it seams to be ok.)
            .AddByte(PayloadLength > 1 ? (byte)(PayloadLength + 1) : PayloadLength)
            .AddByte((byte)TransportLayerControlInfo);

        // if the length of the payload is bigger than 1, the payload starts on the second byte
        if (PayloadLength > 1)
            result.AddByte((byte)MessageType).Add(Payload);
        else
            result.Add(GetMessageTypeAndPayload());

        //Schreibtelegramm (1-6 Bit)
        //bc 2101 0001 e1 00 80 02
        //Die 80 aufgedröselt in binär: 10 000000
        //die 1 signalisiert schreiben
        //ein responsetelegramm hätte hier 01 xxxxxx
        //ein read telegramm 00 000000
        //die unteren 6 bits sind die Daten.
        //Bei ein /aus ergibt sich also hexadezimal:
        //00 = Lesetelegramm
        //80 = schreiben wert 0
        //81 = schreiben wert 1
        //40 = antwort wert 0
        //41 = antwort wert 1


        // that means. TCPI = UDT ; Counter = 0; Data = 0x01 (lamp on!)

        // and last but not least... return the whole shit as a byte array
        return result.ToByteArray();
    }

    /// <summary>
    ///     Deserializes the specified bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns>a new <c>KnxMessageCemi</c></returns>
    public static KnxMessage Deserialize(byte[] bytes)
    {
        var messageCode = (MessageCode)Enum.Parse(typeof(MessageCode), ((int)bytes[0]).ToString(), true);

        var additionalInfo = new byte[bytes[1]];
        if (additionalInfo.Length > 0)
            additionalInfo = bytes.ExtractBytes(1, additionalInfo.Length);

        var idx = additionalInfo.Length + 2;
        var controlByte1 = new ControlByte1();
        controlByte1.Deserialize(bytes[idx]);
        var controlByte2 = new ControlByte2();
        controlByte2.Deserialize(bytes[idx + 1]);

        var sourceAddress = new KnxDeviceAddress(bytes.ExtractBytes(idx + 2, 2));
        var destinationAddress = controlByte2.DestinationAddressIsKnxDeviceAddress
            ? new KnxDeviceAddress(bytes.ExtractBytes(idx + 4, 2))
            : (KnxAddress)new KnxLogicalAddress(bytes.ExtractBytes(idx + 4, 2));

        var val = (bytes[idx + 8] >> 4) << 4;
        var msgType = (MessageType)Enum.Parse(typeof(MessageType), val.ToString(), true);

        var dataLength = bytes[idx + 6];
        var data = dataLength == 1 ? new[] { (byte)(bytes[idx + 8] & 0x0F) } : bytes.ExtractBytes(idx + 9);

        return new KnxMessage
        {
            MessageCode = messageCode,
            AdditionalInfo = additionalInfo,
            SourceAddress = sourceAddress,
            DestinationAddress = destinationAddress,
            Payload = data,
            MessageType = msgType
        };
    }
}