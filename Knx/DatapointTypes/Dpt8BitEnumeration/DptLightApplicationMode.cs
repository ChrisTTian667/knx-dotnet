using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 5, Usage.FunctionBlock)]
    public class DptLightApplicationMode : Dpt8BitEnum<ApplicationMode>
    {
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