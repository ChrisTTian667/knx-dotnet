using System;
using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4Bit
{
    [DataContract]
    public abstract class Dpt3BitControlled : Dpt4Bit
    {
        protected Dpt3BitControlled()
        {
        }
            
        protected Dpt3BitControlled(bool control, byte stepcode)
            : base()
        {
            Payload = ToBytes(control, stepcode);
        }

        protected Dpt3BitControlled(byte[] payload)
            : base(payload)
        {
        }

        [DatapointProperty]
        public virtual bool Control
        {
            get => GetControlFlag(Payload);
            set => Payload = ToBytes(value, Stepcode);
        }
        
        [DatapointProperty]
        [Range(0, 7, ErrorMessage = "Stepcount must be within 0 and 7")]
        public virtual byte Stepcode
        {
            get => GetStepcode(Payload);
            set => Payload = ToBytes(Control, value);
        }

        private static bool GetControlFlag(byte[] bytes)
        {
            return Convert.ToBoolean((byte)(bytes[0] >> 3));
        }

        private static byte GetStepcode(byte[] bytes)
        {
            return (byte)(bytes[0] & 0x07);
        }

        private static byte[] ToBytes(bool control, byte stepcode)
        {
            var controlFlag = (byte)(Convert.ToByte(control) << 3);
            stepcode = (byte)(stepcode & 0x07);

            return new[] { (byte)(controlFlag | stepcode) };
        }
    }
}