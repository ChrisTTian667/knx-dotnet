using System;
using System.Collections;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx;

/// <summary>
///     A logical knx address (GroupAddress)
/// </summary>
public class KnxLogicalAddress : KnxAddress
{
    /// <summary>
    ///     Fills the bit array with the corresponding values.
    /// </summary>
    /// <param name="bitArray">The bit array.</param>
    protected override void FillBitArray(BitArray bitArray)
    {
        bitArray[0] = false; // reserved for special addressing methods

        var currentIdx = 1;

        // add MAIN group (always 4-bit)
        foreach (var bit in Group.ConvertToBits(4))
        {
            bitArray[currentIdx] = bit;
            currentIdx++;
        }

        // add MIDDLE group (3-bit, but it's not mandatory)
        if (MiddleGroup != null)
        {
            foreach (var bit in MiddleGroup.ConvertToBits(3))
            {
                bitArray[currentIdx] = bit;
                currentIdx++;
            }
        }

        var length = (byte)(MiddleGroup != null ? 8 : 11);

        foreach (var bit in ((int)SubGroup).ConvertToBits(length))
        {
            bitArray[currentIdx] = bit;
            currentIdx++;
        }
    }

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return MiddleGroup == null ? Group + "/" + SubGroup : Group + "/" + MiddleGroup + "/" + SubGroup;
    }

    #region private fields

    private byte _group;
    private byte? _middleGroup;
    private ushort _subGroup;

    #endregion

    #region construction

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxLogicalAddress" /> class.
    /// </summary>
    private KnxLogicalAddress()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxLogicalAddress" /> class.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="middleGroup">The middle group.</param>
    /// <param name="subGroup">The sub group.</param>
    public KnxLogicalAddress(byte group, byte? middleGroup, byte subGroup)
    {
        Group = group;
        MiddleGroup = middleGroup;
        SubGroup = subGroup;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxLogicalAddress" /> class.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="subGroup">The sub group.</param>
    public KnxLogicalAddress(byte group, ushort subGroup)
    {
        Group = group;
        MiddleGroup = null;
        SubGroup = subGroup;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxLogicalAddress" /> class.
    /// </summary>
    /// <param name="data">The data.</param>
    public KnxLogicalAddress(byte[] data)
    {
        if (data.Length != 2)
            throw new ArgumentException(
                @"LogicalAddress bytes array length did not match the length of 2 bytes.",
                "data");

        _group = (byte)((data[0] & 0x78) >> 3); // bit 1-4 of byte 0;
        _middleGroup = (byte)(data[0] & 0x07);
        _subGroup = data[1];
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxLogicalAddress" /> class.
    /// </summary>
    /// <param name="addressString">The address string. (e.g. '0|0|0' or '0|0').</param>
    public KnxLogicalAddress(string addressString)
    {
        var stringElements = addressString.Split('/', '|', '\\');

        if (stringElements.Length < 2 || stringElements.Length > 3)
            throw new ArgumentException("Incorrect logical address string (Must be e.g. '0/0/0' or '0/0').");

        _group = Convert.ToByte(stringElements[0]);
        _middleGroup = stringElements.Length == 2 ? null : Convert.ToByte(stringElements[1]);
        _subGroup = stringElements.Length == 2
            ? Convert.ToByte(stringElements[1])
            : Convert.ToByte(stringElements[2]);
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets or sets the group address.
    /// </summary>
    /// <value>The group.</value>
    [Range(0, 15, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public byte Group
    {
        get => _group;
        set
        {
            ValidateValue(value, 0, 15, "Group");
            _group = value;

            InvokeChangeEvent();
        }
    }

    /// <summary>
    ///     Gets or sets the middle group address.
    /// </summary>
    /// <value>The middle group.</value>
    [Range(0, 7, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public byte? MiddleGroup
    {
        get => _middleGroup;
        set
        {
            if (value != null)
                ValidateValue((byte)value, 0, 7, "MiddleGroup");
            _middleGroup = value;

            // revalidate the subgroup
            SubGroup = SubGroup;

            InvokeChangeEvent();
        }
    }

    /// <summary>
    ///     Gets or sets the sub group address.
    /// </summary>
    /// <value>The sub group.</value>
    [Range(0, 2047, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public ushort SubGroup
    {
        get => _subGroup;
        set
        {
            if (MiddleGroup == null)
                ValidateValue(value, 0, 2047, "SubGroup");
            else
                ValidateValue(value, 0, 255, "SubGroup");

            _subGroup = value;

            InvokeChangeEvent();
        }
    }

    #endregion
}