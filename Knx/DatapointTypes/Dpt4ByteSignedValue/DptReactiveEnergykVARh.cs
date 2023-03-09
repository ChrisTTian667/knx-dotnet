using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue
{
    [DatapointType(13, 15, Unit.kVARh, Usage.General)]
    public class DptReactiveEnergykVARh : Dpt4ByteSignedValue
    {
        public DptReactiveEnergykVARh(byte[] payload)
            : base(payload)
        {
        }

        public DptReactiveEnergykVARh(int value)
            : base(value)
        {
        }
    }
}