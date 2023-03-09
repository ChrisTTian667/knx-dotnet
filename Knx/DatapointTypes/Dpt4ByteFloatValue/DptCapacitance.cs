using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 11, Unit.Capacitance, Usage.General, Description = "capacitance")]
public class DptCapacitance : Dpt4ByteFloat
{
    private DptCapacitance()
    {
    }

    public DptCapacitance(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptCapacitance(float value)
        : base(value)
    {
    }
}