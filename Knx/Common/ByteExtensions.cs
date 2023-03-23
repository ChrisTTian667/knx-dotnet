using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Knx.Common;

/// <summary>
///     a collection of all extensions needed in the knx namespace.
/// </summary>
internal static class ByteExtensions
{
    public static IEnumerable<bool> ConvertToBits(this int value, byte length)
    {
        return new BitArray(new[] { value }).Cast<bool>().Take(length).Reverse();
    }

    public static IEnumerable<bool> ConvertToBits(this byte value, byte length)
    {
        return new BitArray(new[] { (int)value }).Cast<bool>().Take(length).Reverse();
    }

    public static IEnumerable<bool> ConvertToBits(this byte? value, byte length)
    {
        if (value != null)
            return new BitArray(new[] { (int)value }).Cast<bool>().Take(length).Reverse();

        return new bool[length];
    }

    /// <summary>
    ///     Gets the higher 4 bits of an byte.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///     <c>byte</c>
    /// </returns>
    public static byte HighBits(this byte value)
    {
        return (byte)(value >> 4);
    }

    /// <summary>
    ///     Gets the low 4 bits of an byte.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///     <c>byte</c>
    /// </returns>
    public static byte LowBits(this byte value)
    {
        return (byte)(value & 0xF);
    }

    /// <summary>
    ///     Reverses the specified byte.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>the reversed byte</returns>
    public static byte Reverse(this byte value)
    {
        byte result = 0x00;
        byte mask = 0x00;

        for (mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
        {
            result >>= 1;
            var tempbyte = (byte)(value & mask);
            if (tempbyte != 0x00) result |= 0x80;
        }

        return result;
    }

    /// <summary>
    ///     Toes the bit array.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static BitArray ToBitArray(this byte value)
    {
        return new BitArray(new[] { value });
    }

    public static byte SetBit(this byte value, byte index, bool bitValue)
    {
        if (index > 7) throw new ArgumentOutOfRangeException("index", "Index must be within 0..7.");

        if (bitValue) return (byte)(value | (byte)(0x80 >> index));

        var bitArrayBuilder = new BitArrayBuilder();

        for (var i = 0; i < 8; i++) bitArrayBuilder.Add(i != index);

        return (byte)(bitArrayBuilder.ToBitArray().ToByteArray().First() & value);
    }

    public static bool GetBit(this byte value, byte index)
    {
        if (index < 0 || index > 7) throw new ArgumentOutOfRangeException("index", "Index must be within 0..7.");

        return value.ConvertToBits(8).ToArray()[index];
    }
}