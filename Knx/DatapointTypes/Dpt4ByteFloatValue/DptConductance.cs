using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 15, Unit.Conductance, Usage.General, Description = "conductance")]
    public class DptConductance : Dpt4ByteFloat
    {
        public DptConductance(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptConductance(float value)
            : base(value)
        {
        }
    }
}