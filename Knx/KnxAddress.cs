using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knx.Common;

namespace Knx;

/// <summary>
///     Base class for KNX addresses.
/// </summary>
public abstract class KnxAddress
{
    private readonly BitArray _bitArray;

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxAddress" /> class.
    /// </summary>
    protected KnxAddress()
    {
        _bitArray = new BitArray(16, false);
    }

    /// <summary>
    ///     Validates the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The min.</param>
    /// <param name="max">The max.</param>
    /// <param name="parameter">The parameter.</param>
    protected static void ValidateValue(int value, int min, int max, string parameter)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(parameter, $"Has to be between {min} and {max}");
    }

    protected abstract void FillBitArray(BitArray bitArray);

    public static KnxDeviceAddress Device(byte area, byte line, byte device)
    {
        return new(area, line, device);
    }

    public static KnxLogicalAddress Logical(byte group, byte subGroup)
    {
        return new(group, subGroup);
    }

    public static KnxLogicalAddress Logical(byte group, byte? middleGroup, byte subGroup)
    {
        return new(group, middleGroup, subGroup);
    }

    public static KnxDeviceAddress ParseDevice(string address)
    {
        var splitChars = new[] { '\\', '/', '-', ',', '.', ' ' };
        var exMessage =
            $"Input string was not in a correct format. For example: '15.15.254'. (Actual: '{address}')";

        if (string.IsNullOrWhiteSpace(address))
            throw new FormatException(exMessage);

        var addressParts = address.Split('\\', '/', '-', ',', '.', ' ');

        if (addressParts.Length != 3)
            throw new FormatException(exMessage);

        for (var i = 0; i < addressParts.Length - 1; i++)
            addressParts[i] = addressParts[i].Trim(splitChars);

        return new KnxDeviceAddress(
            Convert.ToByte(addressParts[0]),
            Convert.ToByte(addressParts[1]),
            Convert.ToByte(addressParts[2]));
    }

    public static bool TryParseLogical(string input, out KnxLogicalAddress address)
    {
        address = null;
        try
        {
            address = ParseLogical(input.Trim());

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static KnxLogicalAddress ParseLogical(string address)
    {
        var splitChars = new[] { '\\', '/', '-', ',', '.', ' ', '|' };
        var exMessageText = $"Input string was not in a correct format. For example: '15/7/254'. (Actual: '{address}')";

        if (string.IsNullOrWhiteSpace(address))
            throw new FormatException(exMessageText);

        var addressParts = address.Split('\\', '/', '-', ',', '.', ' ', '|');

        if (addressParts.Length is < 2 or > 3)
            throw new FormatException(exMessageText);

        for (var i = 0; i < addressParts.Length - 1; i++)
            addressParts[i] = addressParts[i].Trim(splitChars);

        try
        {
            return addressParts.Length == 2
                ? new KnxLogicalAddress(Convert.ToByte(addressParts[0]), Convert.ToByte(addressParts[1]))
                : new KnxLogicalAddress(
                    Convert.ToByte(addressParts[0]),
                    Convert.ToByte(addressParts[1]),
                    Convert.ToByte(addressParts[2]));
        }
        catch (Exception inner)
        {
            throw new FormatException(exMessageText, inner);
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is KnxAddress other &&
               _bitArray
                   .ToByteArray()
                   .SequenceEqual(other._bitArray.ToByteArray());
    }

    public override int GetHashCode()
    {
        return ToString()!.GetHashCode();
    }

    /// <summary>
    ///     Converts to bits.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="length">The length.</param>
    /// <returns>a list of bool values</returns>
    protected IEnumerable<bool> ConvertToBits(int value, byte length)
    {
        return new BitArray(new[] { value }).Cast<bool>().Take(length).Reverse();
    }

    /// <summary>
    ///     Toes the bit array.
    /// </summary>
    /// <returns>a BitArray containing the values</returns>
    public BitArray ToBitArray()
    {
        FillBitArray(_bitArray);

        return _bitArray;
    }
}