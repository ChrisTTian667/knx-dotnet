using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue
{
    [DatapointType(13, 12, Unit.VARh, Usage.General)]
    public class DptReactiveEnergy : Dpt4ByteSignedValue
    {
        public DptReactiveEnergy(byte[] payload)
            : base(payload)
        {
        }

        public DptReactiveEnergy(int value)
            : base(value)
        {
        }
    }
}