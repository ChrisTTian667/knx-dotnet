using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8ByteSignedValue
{
    [DatapointType(29, 11, Unit.VAh, Usage.General)]
    public class DptApparantEnergy64 : Dpt8ByteSignedValue
    {
        public DptApparantEnergy64(byte[] payload)
            : base(payload)
        {
        }

        public DptApparantEnergy64(Int64 value)
            : base(value)
        {
        }
    }
}
