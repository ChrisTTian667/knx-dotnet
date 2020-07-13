using Knx.Common;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DatapointType(23, 3, Usage.FunctionBlock)]
    public class DptUpDownAction : Dpt2BitEnum<UpDownAction>
    {
        public DptUpDownAction(byte[] payload)
            : base(payload)
        {
        }

        public DptUpDownAction(UpDownAction value)
            : base(value)
        {
        }
    }
}