using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DatapointType(29, 12, Unit.VARh, Usage.General)]
    public class DptReactiveEnergy64 : Dpt8ByteSignedValue
    {
        public DptReactiveEnergy64(byte[] payload)
            : base(payload)
        {
        }

        public DptReactiveEnergy64(Int64 value)
            : base(value)
        {
        }
    }
}
