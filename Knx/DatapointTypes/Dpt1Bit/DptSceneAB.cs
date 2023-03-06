using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit;

[DatapointType(1, 22, Unit.SceneAB, Usage.FunctionBlock)]
public class DptSceneAB : Dpt1Bit
{
    private DptSceneAB()
    {
    }

    public DptSceneAB(byte[] payload)
        : base(payload)
    {
    }

    public DptSceneAB(bool value)
        : base(value)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.SceneA, UnitEncoding.SceneB)]
    public override bool Value
    {
        get => base.Value;
        set => base.Value = value;
    }
}