using Knx.Common;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 23, Unit.BlindsMode, Usage.FunctionBlock)]
    public class DptShutterBlindsMode : Dpt1Bit
    {
        public DptShutterBlindsMode(byte[] payload)
            : base(payload)
        {
        }

        public DptShutterBlindsMode(bool value)
            : base(value)
        {
        }

        [DatapointProperty]
        [BooleanEncoding(UnitEncoding.OnlyMoveUpDown, UnitEncoding.MoveUpDownAndStepMode)]
        public override bool Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}