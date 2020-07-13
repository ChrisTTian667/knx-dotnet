using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointType(20, 11, Usage.FunctionBlock)]
    public class DptErrorClassSystem : Dpt8BitEnum<ErrorClassSystem>
    {
        public DptErrorClassSystem(byte[] payload)
            : base(payload)
        {
        }

        public DptErrorClassSystem(ErrorClassSystem value)
            : base(value)
        {
        }
    }
}