using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 3, Unit.Activity, Usage.General, Description = "activity (radioactive)")]
    public class DptActivity : Dpt4ByteFloat
    {
        public DptActivity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptActivity(float value)
            : base(value)
        {
        }
    }
}