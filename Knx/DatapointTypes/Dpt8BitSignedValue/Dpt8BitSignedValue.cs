using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitSignedValue;

[DataLength(8)]
public abstract class Dpt8BitSignedValue : DatapointType
{
    protected Dpt8BitSignedValue()
    {
    }

    protected Dpt8BitSignedValue(byte[] payload)
        : base(payload)
    {
    }

    protected Dpt8BitSignedValue(sbyte value)
    {
        Value = value;
    }

    [DatapointProperty]
    public virtual sbyte Value
    {
        get
        {
            var sb = unchecked((sbyte)Payload[0]);

            return sb;
        }

        set
        {
            var payload = (byte)value;
            Payload = new[] { payload };
        }
    }
}