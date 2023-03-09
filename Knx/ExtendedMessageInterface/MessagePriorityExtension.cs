using System;
using System.Collections;
using Knx.Common;

namespace Knx.ExtendedMessageInterface;

/// <summary>
///     Extensions for the MessagePriority enum.
/// </summary>
public static class MessagePriorityExtension
{
    /// <summary>
    ///     Deserializes the specified message priority.
    /// </summary>
    /// <param name="messagePriority">The message priority.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static MessagePriority Deserialize(this MessagePriority messagePriority, byte value)
    {
        return (MessagePriority)Enum.Parse(typeof(MessagePriority), ((int)value).ToString(), true);
    }

    public static MessagePriority Deserialize(this MessagePriority messagePriority, bool bit1, bool bit2)
    {
        const MessagePriority returnValue = MessagePriority.Auto;

        var priorityBitArray = new BitArray(new[] { bit2, bit1 });
        var priorityByteArray = priorityBitArray.ToByteArray();
        returnValue.Deserialize(priorityByteArray[0].Reverse());

        return returnValue;
    }
}