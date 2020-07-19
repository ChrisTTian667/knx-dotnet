using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 71, Unit.TermalCapacity, Usage.General, Description = "thermal capacity")]
    public class DptTermalCapacity : Dpt4ByteFloat
    {
        public DptTermalCapacity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptTermalCapacity(float value)
            : base(value)
        {
        }
    }
}