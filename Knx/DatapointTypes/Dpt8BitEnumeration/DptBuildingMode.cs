using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 2, Usage.General)]
    public class DptBuildingMode : Dpt8BitEnum<BuildingMode>
    {
        private DptBuildingMode()
        {
        }

        public DptBuildingMode(byte[] payload)
            : base(payload)
        {
        }

        public DptBuildingMode(BuildingMode value)
            : base(value)
        {
        }
    }
}