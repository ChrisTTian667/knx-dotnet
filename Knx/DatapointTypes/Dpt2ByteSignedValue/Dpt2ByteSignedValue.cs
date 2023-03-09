using System;
using System.Linq;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue;

[DataLength(16)]
public abstract class Dpt2ByteSignedValue : DatapointType
{
    protected Dpt2ByteSignedValue()
    {
    }

    protected Dpt2ByteSignedValue(byte[] payload)
        : base(payload)
    {
    }

    protected Dpt2ByteSignedValue(short value)
    {
        Value = value;
    }

    [DatapointProperty]
    [Range(-32768, 32767, ErrorMessage = "Value must be within -32768 ... 32767.")]
    public virtual short Value
    {
        get
        {
            var payload = Payload.Take(2).ToArray();

            return BitConverter.ToInt16(payload, 0);
        }

        set
        {
            if (value < -32768 || value > 32767)
                throw new ArgumentOutOfRangeException("value", "Value must be within -32 768 ... 32 767.");

            var bytes = BitConverter.GetBytes(value);

            Payload = bytes.Take(2).ToArray();
        }
    }
}