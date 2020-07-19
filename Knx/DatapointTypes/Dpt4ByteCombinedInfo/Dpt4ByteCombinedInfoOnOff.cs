using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteCombinedInfo
{
    [DataLength(32)]
    [DatapointType(27, 1, Usage.General)]
    public class Dpt4ByteCombinedInfoOnOff : DatapointType
    {
        public Dpt4ByteCombinedInfoOnOff(byte[] fourBytes)
        {
            Payload = fourBytes;
        }

        public Dpt4ByteCombinedInfoOnOff(bool output0, bool output1, bool output2, bool output3, bool output4, bool output5, bool output6, bool output7, bool output8, bool output9, bool output10, bool output11, bool output12, bool output13, bool output14, bool output15, bool output0Valid, bool output1Valid, bool output2Valid, bool output3Valid, bool output4Valid, bool output5Valid, bool output6Valid, bool output7Valid, bool output8Valid, bool output9Valid, bool output10Valid, bool output11Valid, bool output12Valid, bool output13Valid, bool output14Valid, bool output15Valid)
        {
            Payload = new byte[4];

            Output0 = output0;
            Output1 = output1;
            Output2 = output2;
            Output3 = output3;
            Output4 = output4;
            Output5 = output5;
            Output6 = output6;
            Output7 = output7;
            Output8 = output8;
            Output9 = output9;
            Output10 = output10;
            Output11 = output11;
            Output12 = output12;
            Output13 = output13;
            Output14 = output14;
            Output15 = output15;

            Output0Valid = output0Valid;
            Output1Valid = output1Valid;
            Output2Valid = output2Valid;
            Output3Valid = output3Valid;
            Output4Valid = output4Valid;
            Output5Valid = output5Valid;
            Output6Valid = output6Valid;
            Output7Valid = output7Valid;
            Output8Valid = output8Valid;
            Output9Valid = output9Valid;
            Output10Valid = output10Valid;
            Output11Valid = output11Valid;
            Output12Valid = output12Valid;
            Output13Valid = output13Valid;
            Output14Valid = output14Valid;
            Output15Valid = output15Valid;
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output0
        {
            get
            {
                return Payload.GetBitAtPosition(31);
            }
            set
            {
                Payload.SetBitAtPosition(31, value);
                RaisePropertyChanged(() => Output0);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output1
        {
            get
            {
                return Payload.GetBitAtPosition(30);
            }
            set
            {
                Payload.SetBitAtPosition(30, value);
                RaisePropertyChanged(() => Output1);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output2
        {
            get
            {
                return Payload.GetBitAtPosition(29);
            }
            set
            {
                Payload.SetBitAtPosition(29, value);
                RaisePropertyChanged(() => Output2);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output3
        {
            get
            {
                return Payload.GetBitAtPosition(28);
            }
            set
            {
                Payload.SetBitAtPosition(28, value);
                RaisePropertyChanged(() => Output3);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output4
        {
            get
            {
                return Payload.GetBitAtPosition(27);
            }
            set
            {
                Payload.SetBitAtPosition(27, value);
                RaisePropertyChanged(() => Output4);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output5
        {
            get
            {
                return Payload.GetBitAtPosition(26);
            }
            set
            {
                Payload.SetBitAtPosition(26, value);
                RaisePropertyChanged(() => Output5);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output6
        {
            get
            {
                return Payload.GetBitAtPosition(25);
            }
            set
            {
                Payload.SetBitAtPosition(25, value);
                RaisePropertyChanged(() => Output6);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output7
        {
            get
            {
                return Payload.GetBitAtPosition(24);
            }
            set
            {
                Payload.SetBitAtPosition(24, value);
                RaisePropertyChanged(() => Output7);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output8
        {
            get
            {
                return Payload.GetBitAtPosition(23);
            }
            set
            {
                Payload.SetBitAtPosition(23, value);
                RaisePropertyChanged(() => Output8);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output9
        {
            get
            {
                return Payload.GetBitAtPosition(22);
            }
            set
            {
                Payload.SetBitAtPosition(22, value);
                RaisePropertyChanged(() => Output9);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output10
        {
            get
            {
                return Payload.GetBitAtPosition(21);
            }
            set
            {
                Payload.SetBitAtPosition(21, value);
                RaisePropertyChanged(() => Output10);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output11
        {
            get
            {
                return Payload.GetBitAtPosition(20);
            }
            set
            {
                Payload.SetBitAtPosition(20, value);
                RaisePropertyChanged(() => Output11);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output12
        {
            get
            {
                return Payload.GetBitAtPosition(19);
            }
            set
            {
                Payload.SetBitAtPosition(19, value);
                RaisePropertyChanged(() => Output12);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output13
        {
            get
            {
                return Payload.GetBitAtPosition(18);
            }
            set
            {
                Payload.SetBitAtPosition(18, value);
                RaisePropertyChanged(() => Output13);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output14
        {
            get
            {
                return Payload.GetBitAtPosition(17);
            }
            set
            {
                Payload.SetBitAtPosition(17, value);
                RaisePropertyChanged(() => Output14);
            }
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output15
        {
            get
            {
                return Payload.GetBitAtPosition(16);
            }
            set
            {
                Payload.SetBitAtPosition(16, value);
                RaisePropertyChanged(() => Output15);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output0Valid
        {
            get
            {
                return Payload.GetBitAtPosition(15);
            }
            set
            {
                Payload.SetBitAtPosition(15, value);
                RaisePropertyChanged(() => Output0Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output1Valid
        {
            get
            {
                return Payload.GetBitAtPosition(14);
            }
            set
            {
                Payload.SetBitAtPosition(14, value);
                RaisePropertyChanged(() => Output1Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output2Valid
        {
            get
            {
                return Payload.GetBitAtPosition(13);
            }
            set
            {
                Payload.SetBitAtPosition(13, value);
                RaisePropertyChanged(() => Output2Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output3Valid
        {
            get
            {
                return Payload.GetBitAtPosition(12);
            }
            set
            {
                Payload.SetBitAtPosition(12, value);
                RaisePropertyChanged(() => Output3Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output4Valid
        {
            get
            {
                return Payload.GetBitAtPosition(11);
            }
            set
            {
                Payload.SetBitAtPosition(11, value);
                RaisePropertyChanged(() => Output4Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output5Valid
        {
            get
            {
                return Payload.GetBitAtPosition(10);
            }
            set
            {
                Payload.SetBitAtPosition(10, value);
                RaisePropertyChanged(() => Output5Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output6Valid
        {
            get
            {
                return Payload.GetBitAtPosition(9);
            }
            set
            {
                Payload.SetBitAtPosition(9, value);
                RaisePropertyChanged(() => Output6Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output7Valid
        {
            get
            {
                return Payload.GetBitAtPosition(8);
            }
            set
            {
                Payload.SetBitAtPosition(8, value);
                RaisePropertyChanged(() => Output7Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output8Valid
        {
            get
            {
                return Payload.GetBitAtPosition(7);
            }
            set
            {
                Payload.SetBitAtPosition(7, value);
                RaisePropertyChanged(() => Output8Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output9Valid
        {
            get
            {
                return Payload.GetBitAtPosition(6);
            }
            set
            {
                Payload.SetBitAtPosition(6, value);
                RaisePropertyChanged(() => Output9Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output10Valid
        {
            get
            {
                return Payload.GetBitAtPosition(5);
            }
            set
            {
                Payload.SetBitAtPosition(5, value);
                RaisePropertyChanged(() => Output10Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output11Valid
        {
            get
            {
                return Payload.GetBitAtPosition(4);
            }
            set
            {
                Payload.SetBitAtPosition(4, value);
                RaisePropertyChanged(() => Output11Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output12Valid
        {
            get
            {
                return Payload.GetBitAtPosition(3);
            }
            set
            {
                Payload.SetBitAtPosition(3, value);
                RaisePropertyChanged(() => Output12Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output13Valid
        {
            get
            {
                return Payload.GetBitAtPosition(2);
            }
            set
            {
                Payload.SetBitAtPosition(2, value);
                RaisePropertyChanged(() => Output13Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output14Valid
        {
            get
            {
                return Payload.GetBitAtPosition(1);
            }
            set
            {
                Payload.SetBitAtPosition(1, value);
                RaisePropertyChanged(() => Output14Valid);
            }
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output15Valid
        {
            get
            {
                return Payload.GetBitAtPosition(0);
            }
            set
            {
                Payload.SetBitAtPosition(0, value);
                RaisePropertyChanged(() => Output15Valid);
            }
        }
    }
}
