﻿using System;
using System.Collections;
using Knx.Common;
using static System.Byte;

namespace Knx;

public class KnxDeviceAddress : KnxAddress
{
    protected override void FillBitArray(BitArray bitArray)
    {
        var currentIdx = 0;
        foreach (var bit in Area.ConvertToBits(4))
        {
            bitArray[currentIdx] = bit;
            currentIdx++;
        }

        foreach (var bit in Line.ConvertToBits(4))
        {
            bitArray[currentIdx] = bit;
            currentIdx++;
        }

        foreach (var bit in Device.ConvertToBits(8))
        {
            bitArray[currentIdx] = bit;
            currentIdx++;
        }
    }

    public override string ToString() =>
        $"{Area}.{Line}.{Device}";

    private byte _area;
    private byte _line;
    private byte _device;

    /// <summary>
    ///     Don't use a parameterless constructor; this is only for serialization
    /// </summary>
    private KnxDeviceAddress()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxDeviceAddress" /> class.
    /// </summary>
    /// <param name="area">The area.</param>
    /// <param name="line">The line.</param>
    /// <param name="device">The device.</param>
    public KnxDeviceAddress(byte area, byte line, byte device)
    {
        Area = area;
        Line = line;
        Device = device;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KnxDeviceAddress" /> class.
    /// </summary>
    /// <param name="bytes">ByteArray with Length 2</param>
    public KnxDeviceAddress(byte[] bytes)
    {
        _area = bytes[0].HighBits();
        _line = bytes[0].LowBits();
        _device = bytes[1];
    }

    /// <summary>
    ///     Gets or sets the area address.
    /// </summary>
    /// <value>The area.</value>
    public byte Area
    {
        get => _area;
        set
        {
            ValidateValue(value, 0, 15, "Area");
            _area = value;

            InvokeChangeEvent();
        }
    }

    /// <summary>
    ///     Gets or sets the line address.
    /// </summary>
    /// <value>The line.</value>
    public byte Line
    {
        get => _line;
        set
        {
            ValidateValue(value, 0, 15, "Line");
            _line = value;

            InvokeChangeEvent();
        }
    }

    /// <summary>
    ///     Gets or sets the device address.
    /// </summary>
    /// <value>The device.</value>
    public new byte Device
    {
        get => _device;
        set
        {
            ValidateValue(value, 0, 255, "Device");
            _device = value;

            InvokeChangeEvent();
        }
    }

    public static implicit operator KnxDeviceAddress(string input)
    {
        var parts = input.Split(new[] { '.', '/', '-' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
            throw new ArgumentException("KnxDeviceAddress should be in Format Area/Line/Device", nameof(input));
        if (!TryParse(parts[0], out var area)|| area > 15)
            throw new ArgumentException("Area is not in a valid range. Should be between 0-15", nameof(input));
        if (!TryParse(parts[1], out var line) || line > 15)
            throw new ArgumentException("Line is not in a valid range. Should be between 0-15", nameof(input));
        if (!TryParse(parts[1], out var device) || line > 255)
            throw new ArgumentException("Device is not in a valid range. Should be between 0-255", nameof(input));

        return new KnxDeviceAddress(area, line, device);
    }
}
