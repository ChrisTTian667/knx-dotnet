using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 3, Usage.General)]
    public class DptOccMode : Dpt8BitEnum<OccMode>
    {
        public DptOccMode(byte[] payload)
            : base(payload)
        {
        }

        public DptOccMode(OccMode value)
            : base(value)
        {
        }
    }
}