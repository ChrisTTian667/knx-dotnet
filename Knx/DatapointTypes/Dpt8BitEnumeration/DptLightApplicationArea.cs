using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 6, Usage.FunctionBlock)]
    public class DptLightApplicationArea : Dpt8BitEnum<ApplicationArea>
    {
        public DptLightApplicationArea(byte[] payload)
            : base(payload)
        {
        }

        public DptLightApplicationArea(ApplicationArea value)
            : base(value)
        {
        }
    }
}