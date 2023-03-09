using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 12, Unit.ChargeDensitySurface, Usage.General, Description = "charge density (surface)")]
    public class DptChargeDensitySurface : Dpt4ByteFloat
    {
        public DptChargeDensitySurface(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptChargeDensitySurface(float value)
            : base(value)
        {
        }
    }
}