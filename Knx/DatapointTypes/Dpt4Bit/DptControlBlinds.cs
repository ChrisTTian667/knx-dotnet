using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4Bit;

[DataContract]
[DatapointType(3, 8, Usage.FunctionBlock)]
public class DptControlBlinds : Dpt3BitControlled
{
    private DptControlBlinds()
    {
    }

    public DptControlBlinds(bool increase, byte stepcode)
        : base(increase, stepcode)
    {
    }

    public DptControlBlinds(byte[] payload)
        : base(payload)
    {
    }

    [DatapointProperty]
    [BooleanEncoding(UnitEncoding.Down, UnitEncoding.Up)]
    public override bool Control
    {
        get => base.Control;
        set => base.Control = value;
    }
}