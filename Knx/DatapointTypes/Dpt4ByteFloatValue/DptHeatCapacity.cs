using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 35, Unit.HeatCapacity, Usage.General, Description = "heat capacity")]
    public class DptHeatCapacity : Dpt4ByteFloat
    {
        public DptHeatCapacity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptHeatCapacity(float value)
            : base(value)
        {
        }
    }
}