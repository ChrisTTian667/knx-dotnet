﻿using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteFloat;

[DatapointType(9, 1, Unit.Temperature, Usage.General)]
public class DptTemperature : Dpt2ByteFloat
{
    private DptTemperature()
    {
    }

    public DptTemperature(byte[] twoBytes) : base(twoBytes)
    {
    }

    public DptTemperature(double value) : base(value)
    {
    }

    [DatapointProperty]
    [Range(-273, 670760, ErrorMessage = "Temperature Value out of Range")]
    public override double Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}