using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 66, Unit.Stress, Usage.General, Description = "stress")]
    public class DptStress : Dpt4ByteFloat
    {
        public DptStress(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptStress(float value)
            : base(value)
        {
        }
    }
}