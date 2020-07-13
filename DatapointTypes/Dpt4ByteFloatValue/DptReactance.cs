using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 59, Unit.Reactance, Usage.General, Description = "reactance")]
    public class DptReactance : Dpt4ByteFloat
    {
        public DptReactance(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptReactance(float value)
            : base(value)
        {
        }
    }
}