using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 29, Unit.ElectromagnetMoment, Usage.General, Description = "electromagnetic moment")]
public class DptElectromagnetMoment : Dpt4ByteFloat
{
    private DptElectromagnetMoment()
    {
    }

    public DptElectromagnetMoment(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptElectromagnetMoment(float value)
        : base(value)
    {
    }
}