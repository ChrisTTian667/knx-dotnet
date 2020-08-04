using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DatapointType(6, 20, Usage.FunctionBlock)]
    [DataLength(8)]
    public class DptStatusWithMode3 : DatapointType
    {
        private DptStatusWithMode3()
        {
        }

        public DptStatusWithMode3(byte[] payload)
            : base(payload)
        {
            base.Payload = payload;
        }

        public DptStatusWithMode3(SetClear a, SetClear b, SetClear c, SetClear d, SetClear e, Mode3 mode)
        {
            Payload = new[] { (byte)0 };

            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            Mode = mode;
        }

        [DatapointProperty]
        public SetClear A
        {
            get { return (SetClear)(Payload[0] >> 7); }

            set
            {
                Payload[0] = Payload[0].SetBit(0, Convert.ToBoolean((byte)value));
                RaisePropertyChanged(() => A);
            }
        }

        [DatapointProperty]
        public SetClear B
        {
            get { return (SetClear)((Payload[0] & 0x40) >> 6); }

            set
            {
                Payload[0] = Payload[0].SetBit(1, Convert.ToBoolean((byte)value));
                RaisePropertyChanged(() => B);
            }
        }

        [DatapointProperty]
        public SetClear C
        {
            get { return (SetClear)((Payload[0] & 0x20) >> 5); }

            set
            {
                Payload[0] = Payload[0].SetBit(2, Convert.ToBoolean((byte)value));
                RaisePropertyChanged(() => C);
            }
        }

        [DatapointProperty]
        public SetClear D
        {
            get { return (SetClear)((Payload[0] & 0x10) >> 4); }

            set
            {
                Payload[0] = Payload[0].SetBit(3, Convert.ToBoolean((byte)value));
                RaisePropertyChanged(() => D);
            }
        }

        [DatapointProperty]
        public SetClear E
        {
            get { return (SetClear)((Payload[0] & 0x08) >> 3); }

            set
            {
                Payload[0] = Payload[0].SetBit(4, Convert.ToBoolean((byte)value));

                RaisePropertyChanged(() => E);
            }
        }

        [DatapointProperty]
        public Mode3 Mode
        {
            get
            {
                return ((Mode3)(Payload[0] & 0x07));
            }

            set
            {
                Payload[0] = (byte)(Payload[0] & 0xF8);         // reset mode to 0
                Payload[0] = (byte)(Payload[0] + (byte)value);  // set the mode

                RaisePropertyChanged(() => Mode);
            }
        }
    }
}