using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 12, Usage.FunctionBlock)]
    public class DptErrorClassHVAC : Dpt8BitEnum<ErrorClassHVAC>
    {
        private DptErrorClassHVAC()
        {
        }

        public DptErrorClassHVAC(byte[] payload)
            : base(payload)
        {
        }

        public DptErrorClassHVAC(ErrorClassHVAC value)
            : base(value)
        {
        }
    }
}