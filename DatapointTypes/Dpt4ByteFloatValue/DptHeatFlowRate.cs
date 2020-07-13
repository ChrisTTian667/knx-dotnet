using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 36, Unit.HeatFlowRate, Usage.General, Description = "heat flow rate")]
    public class DptHeatFlowRate : Dpt4ByteFloat
    {
        public DptHeatFlowRate(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptHeatFlowRate(float value)
            : base(value)
        {
        }
    }
}