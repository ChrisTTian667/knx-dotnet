using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 22, Unit.ElectricalDisplacement, Usage.General, Description = "electric displacement")]
public class DptElectricDisplacement : Dpt4ByteFloat
{
    private DptElectricDisplacement()
    {
    }

    public DptElectricDisplacement(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptElectricDisplacement(float value)
        : base(value)
    {
    }
}