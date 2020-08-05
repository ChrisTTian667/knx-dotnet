using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4Bit
{
    [DataContract]
    [DatapointType(3, 7, Usage.FunctionBlock)]
    public class DptControlDimming : Dpt3BitControlled
    {
        private DptControlDimming()
        {
        }
        
        public DptControlDimming(bool increase, byte stepcode) : base(increase, stepcode)
        {
        }

        public DptControlDimming(byte[] payload) : base(payload)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Decrease, UnitEncoding.Increase)]
        public override bool Control
        {
            get => base.Control;
            set => base.Control = value;
        }
    }
}