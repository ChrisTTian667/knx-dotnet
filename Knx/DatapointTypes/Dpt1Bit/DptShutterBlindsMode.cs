using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt1Bit
{
    [DatapointType(1, 23, Unit.BlindsMode, Usage.FunctionBlock)]
    public class DptShutterBlindsMode : Dpt1Bit
    {
        private DptShutterBlindsMode()
        {
        }

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