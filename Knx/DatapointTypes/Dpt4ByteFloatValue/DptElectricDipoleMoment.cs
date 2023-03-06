using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 21, Unit.ElectricalDipoleMoment, Usage.General, Description = "electric dipole moment")]
public class DptElectricDipoleMoment : Dpt4ByteFloat
{
    private DptElectricDipoleMoment()
    {
    }

    public DptElectricDipoleMoment(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptElectricDipoleMoment(float value)
        : base(value)
    {
    }
}