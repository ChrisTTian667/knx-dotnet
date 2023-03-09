using System;
using System.Linq;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteUnsignedValue;

[DataLength(32)]
public abstract class Dpt4ByteUnsignedValue : DatapointType
{
    protected Dpt4ByteUnsignedValue()
    {
    }

    protected Dpt4ByteUnsignedValue(byte[] payload)
        : base(payload)
    {
        Payload = Payload.Take(4).ToArray();
    }

    protected Dpt4ByteUnsignedValue(uint value)
    {
        Value = value;
    }

    [DatapointProperty]
    [Range(0, 4294967295, ErrorMessage = "Value must be within 0...4294967295.")]
    public virtual uint Value
    {
        get
        {
            var payload = Payload.Take(2).ToArray();

            return BitConverter.ToUInt16(payload, 0);
        }

        set
        {
            if (value < 0 || value > uint.MaxValue)
                throw new ArgumentOutOfRangeException(
                    "value",
                    string.Format("Value must be within 0 ... {0}.", uint.MaxValue));

            var bytes = BitConverter.GetBytes(value);
            Payload = bytes.Take(4).ToArray();
        }
    }
}