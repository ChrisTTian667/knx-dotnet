using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 14, Usage.General)]
    public class DptBeafortWindForceScale : Dpt8BitEnum<BeafortWindForceScale>
    {
        public DptBeafortWindForceScale(byte[] payload)
            : base(payload)
        {
        }

        public DptBeafortWindForceScale(BeafortWindForceScale value)
            : base(value)
        {
        }
    }
}