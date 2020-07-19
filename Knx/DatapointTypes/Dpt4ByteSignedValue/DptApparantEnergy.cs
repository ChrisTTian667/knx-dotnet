using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue
{
    [DatapointType(13, 11, Unit.VAh, Usage.General)]
    public class DptApparantEnergy : Dpt4ByteSignedValue
    {
        public DptApparantEnergy(byte[] payload)
            : base(payload)
        {
        }

        public DptApparantEnergy(int value)
            : base(value)
        {
        }
    }
}