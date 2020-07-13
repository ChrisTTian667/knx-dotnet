using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 38, Unit.Impendance, Usage.General, Description = "impendance")]
    public class DptImpendance : Dpt4ByteFloat
    {
        public DptImpendance(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptImpendance(float value)
            : base(value)
        {
        }
    }
}