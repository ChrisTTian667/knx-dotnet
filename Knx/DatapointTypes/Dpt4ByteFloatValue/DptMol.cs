using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue;

[DatapointType(14, 4, Unit.Mol, Usage.General, Description = "amount of substance")]
public class DptMol : Dpt4ByteFloat
{
    private DptMol()
    {
    }

    public DptMol(byte[] twoBytes)
        : base(twoBytes)
    {
    }

    public DptMol(float value)
        : base(value)
    {
    }
}