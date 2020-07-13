using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 8, Usage.System)]
    public class DptPSUMode : Dpt8BitEnum<PSUMode>
    {
        public DptPSUMode(byte[] payload)
            : base(payload)
        {
        }

        public DptPSUMode(PSUMode value)
            : base(value)
        {
        }
    }
}