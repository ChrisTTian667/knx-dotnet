using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue;

[DatapointType(13, 1, Unit.Pulses, Usage.General)]
public class DptValue4Count : Dpt4ByteSignedValue
{
    private DptValue4Count()
    {
    }

    public DptValue4Count(byte[] payload) : base(payload)
    {
    }

    public DptValue4Count(int value) : base(value)
    {
    }
}