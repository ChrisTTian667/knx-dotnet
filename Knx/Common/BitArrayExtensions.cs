using System.Collections;
using System.Linq;

namespace Knx.Common;

public static class BitArrayExtensions
{
    /// <summary>
    ///     Counts the positiv bit's of a BitArray.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>a <c>byte</c> representing the count of positive bits in the specified array.</returns>
    public static byte CountOfPositiveBits(this BitArray array)
    {
        return array.Cast<bool>().Aggregate<bool, byte>(0, (current, bit) => (byte)(current + (byte)(bit ? 1 : 0)));
    }

    /// <summary>
    ///     Converts the BitArray to a ByteArray
    /// </summary>
    /// <param name="bits">The bits.</param>
    /// <returns>a <c>byte[]</c></returns>
    public static byte[] ToByteArray(this BitArray bits)
    {
        var numBytes = bits.Length / 8;
        if (bits.Length % 8 != 0)
            numBytes++;

        var bytes = new byte[numBytes];
        int byteIndex = 0, bitIndex = 0;

        for (var i = 0; i < bits.Length; i++)
        {
            if (bits[i])
                bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

            bitIndex++;

            if (bitIndex != 8)
                continue;

            bitIndex = 0;
            byteIndex++;
        }

        return bytes;
    }
}