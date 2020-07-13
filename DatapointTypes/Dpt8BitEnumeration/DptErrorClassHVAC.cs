using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 12, Usage.FunctionBlock)]
    public class DptErrorClassHVAC : Dpt8BitEnum<ErrorClassHVAC>
    {
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