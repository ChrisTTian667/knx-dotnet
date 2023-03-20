using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Knx.Common;

internal class ByteArrayBuilder
{
    private readonly List<byte> _list = new();

    public int Length => _list.Count;

    private static byte[] IntToByteArray(int integer)
    {
        var temp = BitConverter.GetBytes(integer);
        var intBytes = new byte[2];

        intBytes[0] = temp[1];
        intBytes[1] = temp[0];

        return intBytes;
    }

    public ByteArrayBuilder Add(IEnumerable<byte> array)
    {
        _list.AddRange(array);
        return this;
    }

    public ByteArrayBuilder Add(IPAddress ipAddress)
    {
        _list.AddRange(ipAddress.GetAddressBytes());
        return this;
    }

    /// <summary>
    ///     Adds the bit array.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>the ByteArrayBuilder</returns>
    public ByteArrayBuilder AddBitArray(BitArray array)
    {
        var byteSize = array.Length / 8 + (array.Length % 8 > 0 ? 1 : 0);
        var byteArray = new byte[byteSize];

        for (var i = 0; i < array.Length; i++)
        {
            var bytePos = i / 8;
            var bitPos = (byte)(i % 8);
            var value = array[i];

            byteArray[bytePos] = byteArray[bytePos].SetBit(bitPos, value);
        }

        foreach (var b in byteArray)
            AddByte(b);

        return this;
    }

    public ByteArrayBuilder AddByte(byte b)
    {
        _list.Add(b);
        return this;
    }

    public ByteArrayBuilder AddByte(byte? b)
    {
        _list.Add(b ??= 0x00);
        return this;
    }

    public ByteArrayBuilder AddInt(int integer) =>
        Add(IntToByteArray(integer));

    /// <summary>
    ///     Adds the length byteArrayToken.
    /// </summary>
    /// <returns>itself</returns>
    public ByteArrayBuilder AddToken(byte length, out ByteArrayToken byteArrayToken)
    {
        byteArrayToken = new ByteArrayToken(_list.Count, length);

        for (var i = 0; i < length; i++)
            _list.Add(0);

        return this;
    }

    private void ReplaceToken(ByteArrayToken byteArrayToken, byte value) =>
        _list[byteArrayToken.Index] = value;

    public void ReplaceToken(ByteArrayToken byteArrayToken, int value)
    {
        if (byteArrayToken.Index + 1 > _list.Count)
            throw new InvalidOperationException("no more space to add an integer.");

        switch (byteArrayToken.BytesToAdd)
        {
            case > 2:
                throw new NotSupportedException("ByteArrayBuilder supports only tokens with a length of one or two bytes.");
            case 1 when value > byte.MaxValue:
                throw new ArgumentException("Value to big to pass to an single byte");
            case 1:
                ReplaceToken(byteArrayToken, Convert.ToByte(value));
                return;
        }

        var byteArray = IntToByteArray(value);
        _list[byteArrayToken.Index] = byteArray[0];
        _list[byteArrayToken.Index + 1] = byteArray[1];
    }

    /// <summary>
    ///     Returns the completed byteArray
    /// </summary>
    /// <returns></returns>
    public byte[] ToByteArray() =>
        _list.ToArray();
}
