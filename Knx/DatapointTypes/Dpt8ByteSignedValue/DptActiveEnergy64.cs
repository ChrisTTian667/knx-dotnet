using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DatapointType(29, 10, Unit.Wh, Usage.General)]
    public class DptActiveEnergy64 : Dpt8ByteSignedValue
    {
        private DptActiveEnergy64()
        {
        }

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
