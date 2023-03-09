using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2ByteUnsignedValue;

[DatapointType(7, 10, Usage.General)]
public class DptPropDataType : Dpt2ByteUnsignedValue
{
    private DptPropDataType()
    {
    }

    public DptPropDataType(byte[] payload)
        : base(payload)
    {
    }

    public DptPropDataType(ushort value)
        : base(value)
    {
    }
}