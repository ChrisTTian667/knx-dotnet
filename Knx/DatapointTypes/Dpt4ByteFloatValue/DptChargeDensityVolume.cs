using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 13, Unit.ChargeDensityVolume, Usage.General, Description = "charge density (volume)")]
    public class DptChargeDensityVolume : Dpt4ByteFloat
    {
        public DptChargeDensityVolume(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptChargeDensityVolume(float value)
            : base(value)
        {
        }
    }
}