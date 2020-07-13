using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 17, Usage.General)]
    public class DptSensorSelect : Dpt8BitEnum<SensorSelect>
    {
        public DptSensorSelect(byte[] payload)
            : base(payload)
        {
        }

        public DptSensorSelect(SensorSelect value)
            : base(value)
        {
        }
    }
}