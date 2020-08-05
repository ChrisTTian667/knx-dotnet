using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DatapointType(23,1, Usage.FunctionBlock)]
    public class DptOnOffAction : Dpt2BitEnum<OnOffAction>
    {
        private DptOnOffAction()
        {
        }
        
        public DptOnOffAction(byte[] payload) : base(payload)
        {
        }

        public DptOnOffAction(OnOffAction value) : base(value)
        {
        }
    }
}