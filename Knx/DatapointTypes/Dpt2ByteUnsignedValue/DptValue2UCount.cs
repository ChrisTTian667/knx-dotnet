using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DatapointType(7, 1, Unit.Pulses, Usage.General)]
public class DptValue2UCount : Dpt2ByteUnsignedValue
{
    private DptValue2UCount()
    {
    }

    public DptValue2UCount(byte[] payload) : base(payload)
    {
    }

    public DptValue2UCount(ushort value) : base(value)
    {
    }
}