using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DatapointType(29, 10, Unit.Wh, Usage.General)]
    public class DptActiveEnergy64 : Dpt8ByteSignedValue
    {
        public DptActiveEnergy64(byte[] payload)
            : base(payload)
        {
        }

        public DptActiveEnergy64(Int64 value)
            : base(value)
        {
        }
    }

}
