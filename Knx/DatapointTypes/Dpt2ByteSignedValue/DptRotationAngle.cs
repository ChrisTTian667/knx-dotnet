using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 11, Unit.Angle, Usage.FunctionBlock)]
    public class DptRotationAngle : Dpt2ByteSignedValue
    {
        public DptRotationAngle(byte[] payload)
            : base(payload)
        {
        }

        public DptRotationAngle(short value)
            : base(value)
        {
        }
    }
}