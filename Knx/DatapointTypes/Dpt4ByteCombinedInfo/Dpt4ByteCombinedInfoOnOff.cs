using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteCombinedInfo
{
    [DataLength(32)]
    [DatapointType(27, 1, Usage.General)]
    public class Dpt4ByteCombinedInfoOnOff : DatapointType
    {
        private Dpt4ByteCombinedInfoOnOff()
        {
            
        }
        
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
            get => Payload.GetBitAtPosition(31);
            set => Payload.SetBitAtPosition(31, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output1
        {
            get => Payload.GetBitAtPosition(30);
            set => Payload.SetBitAtPosition(30, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output2
        {
            get => Payload.GetBitAtPosition(29);
            set => Payload.SetBitAtPosition(29, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output3
        {
            get => Payload.GetBitAtPosition(28);
            set => Payload.SetBitAtPosition(28, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output4
        {
            get => Payload.GetBitAtPosition(27);
            set => Payload.SetBitAtPosition(27, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output5
        {
            get => Payload.GetBitAtPosition(26);
            set => Payload.SetBitAtPosition(26, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output6
        {
            get => Payload.GetBitAtPosition(25);
            set => Payload.SetBitAtPosition(25, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output7
        {
            get => Payload.GetBitAtPosition(24);
            set => Payload.SetBitAtPosition(24, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output8
        {
            get => Payload.GetBitAtPosition(23);
            set => Payload.SetBitAtPosition(23, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output9
        {
            get => Payload.GetBitAtPosition(22);
            set => Payload.SetBitAtPosition(22, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output10
        {
            get => Payload.GetBitAtPosition(21);
            set => Payload.SetBitAtPosition(21, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output11
        {
            get => Payload.GetBitAtPosition(20);
            set => Payload.SetBitAtPosition(20, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output12
        {
            get => Payload.GetBitAtPosition(19);
            set => Payload.SetBitAtPosition(19, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output13
        {
            get => Payload.GetBitAtPosition(18);
            set => Payload.SetBitAtPosition(18, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output14
        {
            get => Payload.GetBitAtPosition(17);
            set => Payload.SetBitAtPosition(17, value);
        }

        [DatapointProperty(Unit.OnOff)]
        public virtual bool Output15
        {
            get => Payload.GetBitAtPosition(16);
            set => Payload.SetBitAtPosition(16, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output0Valid
        {
            get => Payload.GetBitAtPosition(15);
            set => Payload.SetBitAtPosition(15, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output1Valid
        {
            get => Payload.GetBitAtPosition(14);
            set => Payload.SetBitAtPosition(14, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output2Valid
        {
            get => Payload.GetBitAtPosition(13);
            set => Payload.SetBitAtPosition(13, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output3Valid
        {
            get => Payload.GetBitAtPosition(12);
            set => Payload.SetBitAtPosition(12, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output4Valid
        {
            get => Payload.GetBitAtPosition(11);
            set => Payload.SetBitAtPosition(11, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output5Valid
        {
            get => Payload.GetBitAtPosition(10);
            set => Payload.SetBitAtPosition(10, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output6Valid
        {
            get => Payload.GetBitAtPosition(9);
            set => Payload.SetBitAtPosition(9, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output7Valid
        {
            get => Payload.GetBitAtPosition(8);
            set => Payload.SetBitAtPosition(8, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output8Valid
        {
            get => Payload.GetBitAtPosition(7);
            set => Payload.SetBitAtPosition(7, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output9Valid
        {
            get => Payload.GetBitAtPosition(6);
            set => Payload.SetBitAtPosition(6, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output10Valid
        {
            get => Payload.GetBitAtPosition(5);
            set => Payload.SetBitAtPosition(5, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output11Valid
        {
            get => Payload.GetBitAtPosition(4);
            set => Payload.SetBitAtPosition(4, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output12Valid
        {
            get => Payload.GetBitAtPosition(3);
            set => Payload.SetBitAtPosition(3, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output13Valid
        {
            get => Payload.GetBitAtPosition(2);
            set => Payload.SetBitAtPosition(2, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output14Valid
        {
            get => Payload.GetBitAtPosition(1);
            set => Payload.SetBitAtPosition(1, value);
        }

        [DatapointProperty(Unit.TrueFalse)]
        public virtual bool Output15Valid
        {
            get => Payload.GetBitAtPosition(0);
            set => Payload.SetBitAtPosition(0, value);
        }
    }
}
