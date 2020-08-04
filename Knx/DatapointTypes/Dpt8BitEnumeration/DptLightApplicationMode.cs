using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 5, Usage.FunctionBlock)]
    public class DptLightApplicationMode : Dpt8BitEnum<ApplicationMode>
    {
        private DptLightApplicationMode()
        {
        }

        public DptLightApplicationMode(byte[] payload)
            : base(payload)
        {
        }

        public DptLightApplicationMode(ApplicationMode value)
            : base(value)
        {
        }
    }
}