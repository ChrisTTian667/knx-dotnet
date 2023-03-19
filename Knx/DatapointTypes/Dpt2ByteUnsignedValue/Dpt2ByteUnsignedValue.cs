using System;
using System.Linq;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DataLength(16)]
public abstract class Dpt2ByteUnsignedValue : DatapointType
{
    protected Dpt2ByteUnsignedValue()
    {
    }

    protected Dpt2ByteUnsignedValue(byte[] payload)
        : base(payload)
    {
        Payload = Payload.Take(2).ToArray();
    }

    protected Dpt2ByteUnsignedValue(ushort value)
    {
        Value = value;
    }

    [DatapointProperty]
    [Range(0, 65535, ErrorMessage = "Value must be within 0...65535.")]
    public virtual ushort Value
    {
        get
        {
            var payload = Payload.Take(2).ToArray();
            return BitConverter.ToUInt16(payload, 0);
        }

        set
        {
            var bytes = BitConverter.GetBytes(value);
            Payload = bytes.Take(2).ToArray();
        }
    }
}
