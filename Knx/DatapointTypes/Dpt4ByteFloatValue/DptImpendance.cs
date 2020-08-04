using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 38, Unit.Impendance, Usage.General, Description = "impendance")]
    public class DptImpendance : Dpt4ByteFloat
    {
        private DptImpendance()
        {
        }

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