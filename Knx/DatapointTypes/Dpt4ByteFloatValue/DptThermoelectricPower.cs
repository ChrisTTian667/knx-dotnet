using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 73, Unit.ThermoelectricPower, Usage.General, Description = "thermoelectric power")]
public class DptThermoelectricPower : Dpt4ByteFloat
{
    private DptThermoelectricPower()
    {
    }

    public DptThermoelectricPower(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptThermoelectricPower(float value)
        : base(value)
    {
    }
}