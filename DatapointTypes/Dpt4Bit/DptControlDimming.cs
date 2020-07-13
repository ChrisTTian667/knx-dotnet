using System.Runtime.Serialization;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt4Bit
{
    [DataContract]
    [DatapointType(3, 7, Usage.FunctionBlock)]
    public class DptControlDimming : Dpt3BitControlled
    {
        public DptControlDimming(bool increase, byte stepcode) : base(increase, stepcode)
        {
        }

        public DptControlDimming(byte[] payload) : base(payload)
        {
        }

        [DataMember]
        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.Decrease, UnitEncoding.Increase)]
        public override bool Control
        {
            get
            {
                return base.Control;
            }
            set
            {
                base.Control = value;
            }
        }
    }
}