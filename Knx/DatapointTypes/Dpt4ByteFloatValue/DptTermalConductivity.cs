using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 72, Unit.TermalConductivity, Usage.General, Description = "thermal conductivity")]
    public class DptTermalConductivity : Dpt4ByteFloat
    {
        private DptTermalConductivity()
        {
        }

        public DptTermalConductivity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTermalConductivity(float value)
            : base(value)
        {
        }
    }
}