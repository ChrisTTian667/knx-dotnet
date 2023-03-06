using Knx.Common;

namespace Knx.ExtendedMessageInterface;

/// <summary>
///     Represents the first control byte of the Common Extended Message Interface (cEMI)
///     (only for internal usage)
/// </summary>
internal class ControlByte1
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlByte1" /> class.
    /// </summary>
    public ControlByte1()
    {
        IsRepetition = false;
        IsStandardFrame = true;
        IsPositivConfirmation = true;
        Priority = MessagePriority.Auto;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is repetition.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is repetition; otherwise, <c>false</c>.
    /// </value>
    public bool IsRepetition { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is positiv confirmation.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is positiv confirmation; otherwise, <c>false</c>.
    /// </value>
    public bool IsPositivConfirmation { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is standard frame.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is standard frame; otherwise, <c>false</c>.
    /// </value>
    public bool IsStandardFrame { get; set; }

    /// <summary>
    ///     Gets or sets the priority.
    /// </summary>
    /// <value>The priority.</value>
    public MessagePriority Priority { get; set; }

    /// <summary>
    ///     Serializes this instance into a single byte.
    /// </summary>
    /// <returns>
    ///     <c>byte</c>
    /// </returns>
    public byte ToByte()
    {
        var bitArray = new BitArrayBuilder()
            .Add(IsStandardFrame) // set standard frame (see calimero... don't know what it mean!)
            .Add(false)
            .Add(!IsRepetition) // If the message is send the first time, this flag needs to be false; otherwise true.
            .Add(true)
            .Add((byte)Priority, 2)
            .Add(false)
            .Add(IsPositivConfirmation)
            .ToBitArray();

        return bitArray.ToByteArray()[0];
    }

    /// <summary>
    ///     Deserializes the specified control byte.
    /// </summary>
    /// <param name="controlByte">The control byte.</param>
    public void Deserialize(byte controlByte)
    {
        var controlBitArray = controlByte.ToBitArray();

        // set internal variable instead of the property, 
        // so the value will not be inverted in case of an IND message code.
        IsStandardFrame = controlBitArray[0];
        IsRepetition = controlBitArray[2];
        Priority.Deserialize(controlBitArray[4], controlBitArray[5]);
        IsPositivConfirmation = controlBitArray[7];
    }
}