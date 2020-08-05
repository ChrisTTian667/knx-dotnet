using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    [DatapointType(8, 10, Unit.Percentage, Usage.General)]
    public class DptPercentV16 : Dpt2ByteSignedValue
    {
        private DptPercentV16()
        {
        }

        public DptPercentV16(byte[] payload)
            : base(payload)
        {
        }

        public DptPercentV16(short value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(-327.68, 327.67, ErrorMessage = "Value must be within -327.68 ... 327.67.")]
        public new double Value
        {
            get
            {
                return (Int16)(base.Value * 100);
            }
            set
            {
                base.Value = (Int16)(value / 100);
            }
        }
    }
}