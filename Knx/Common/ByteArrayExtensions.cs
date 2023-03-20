﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Knx.Common;

internal static class ByteArrayExtensions
{
    /// <summary>
    ///     Extracts the bytes from a given Idx.
    /// </summary>
    /// <param name="byteArray">The byte array.</param>
    /// <param name="fromIdx">From idx.</param>
    /// <returns>a new <c>byte[]</c> representing the data starting at given idx</returns>
    public static byte[] ExtractBytes(this byte[] byteArray, int fromIdx)
    {
        return byteArray.ExtractBytes(fromIdx, byteArray.Length - fromIdx);
    }

    /// <summary>
    ///     Extracts the bytes.
    /// </summary>
    /// <param name="byteArray">The byte array.</param>
    /// <param name="startingIdx">The starting idx.</param>
    /// <param name="count">The count.</param>
    /// <returns>a new <c>byte[]</c> representing the data starting at given idx</returns>
    [DebuggerStepThrough]
    public static byte[] ExtractBytes(this byte[] byteArray, int startingIdx, int count)
    {
        // limit the count of extracted bytes to the maximal available bytes
        if (count > byteArray.Length + startingIdx) count = byteArray.Length + startingIdx;

        // throw an exception, when starting index is greater than available bytes
        if (startingIdx >= byteArray.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(startingIdx),
                $"Cannot extract bytes from Idx: {startingIdx} cause the sourceArray has only a length of {byteArray.Length}");
        }

        var extractedBytes = new byte[count];

        Array.Copy(byteArray, startingIdx, extractedBytes, 0, count);

        return extractedBytes;
    }

    /// <summary>
    ///     Gets the bit at position.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="pos">The pos.</param>
    /// <returns></returns>
    public static bool GetBitAtPosition(this byte[] data, int pos)
    {
        var posByte = pos / 8;
        var posBit = pos % 8;
        var valByte = data[posByte];

        return ((valByte >> (8 - (posBit + 1))) & 0x0001) == 1;
    }

    public static void SetBitAtPosition(this byte[] data, int pos, bool bit)
    {
        if (bit)
            data[pos / 8] |= (byte)(1 << (7 - pos % 8));
        else
            data[pos / 8] ^= (byte)(1 << (7 - pos % 8));
    }

    public static byte[] Terminate(this byte[] data, byte terminationByte = 0)
    {
        if (data.Length == 0)
            return new byte[] { 0 };

        var collection = new List<byte>(data) { 0 };

        return collection.ToArray();
    }

    public static string ToReadableString(this byte[] byteArray) =>
        BitConverter.ToString(byteArray).Replace("-", " ");

    public static byte[] FromReadableString(string hexString)
    {
        hexString = hexString.Length switch
        {
            0 => "00",
            1 => $"0{hexString}",
            _ => hexString.Trim().Replace(" ", string.Empty)
        };

        var numberChars = hexString.Length;
        var bytes = new byte[numberChars / 2];
        for (var i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);

        return bytes;
    }
}
