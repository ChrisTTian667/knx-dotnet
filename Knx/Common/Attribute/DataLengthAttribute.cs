using System;

namespace Knx.Common.Attribute;

public enum DataLength
{
    Infinite = -1
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DataLengthAttribute : System.Attribute
{
    public DataLengthAttribute(int lengthInBit)
    {
        Length = lengthInBit;
    }

    public DataLengthAttribute(int minimum, int maximum) : this(minimum)
    {
        MaximumLength = maximum;
    }

    public DataLengthAttribute(int minimum, DataLength maximum)
        : this(minimum)
    {
        MaximumLength = (int)maximum;
    }

    public DataLengthAttribute(DataLength length)
    {
        Length = (int)length;
    }

    /// <summary>
    ///     Gets the length in bits.
    /// </summary>
    /// <value>
    ///     The length.
    /// </value>
    public int Length { get; }

    public int MaximumLength { get; }

    public int MinimumRequiredBytes => Length / 8 + (Length % 8 > 0 ? 1 : 0);
}