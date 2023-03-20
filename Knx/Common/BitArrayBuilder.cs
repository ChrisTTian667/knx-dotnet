using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Knx.Common;

internal sealed class BitArrayBuilder
{
    private readonly List<BitArray> _bitArrayList = new();

    public BitArrayBuilder Add(bool bit)
    {
        _bitArrayList.Add(new BitArray(new[] { bit }));

        return this;
    }

    public BitArrayBuilder Add(int intValue, byte length)
    {
        _bitArrayList.Add(new BitArray(intValue.ConvertToBits(length).ToArray()));

        return this;
    }

    public BitArrayBuilder Add(byte byteValue, byte length)
    {
        _bitArrayList.Add(new BitArray(byteValue.ConvertToBits(length).ToArray()));

        return this;
    }

    public BitArrayBuilder Add(BitArray value)
    {
        _bitArrayList.Add(value);

        return this;
    }

    public BitArray ToBitArray()
    {
        var length = _bitArrayList.Sum(bitArray => bitArray.Length);

        var resultArray = new BitArray(length);

        var currentIdx = 0;

        foreach (var bit in _bitArrayList.SelectMany(bitArray => bitArray.Cast<bool>()))
        {
            resultArray.Set(currentIdx, bit);
            currentIdx++;
        }

        return resultArray;
    }
}
