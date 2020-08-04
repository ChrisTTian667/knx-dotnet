using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 5, Usage.General, Description = "aplitude (unit as appropriate)")]
    public class DptAmplitude : Dpt4ByteFloat
    {
        private DptAmplitude()
        {
        }

        public DptAmplitude(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAmplitude(float value)
            : base(value)
        {
        }
    }
}