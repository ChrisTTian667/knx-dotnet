using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 34, Unit.AngularFrequency, Usage.General, Description = "frequence, angular (pulssatance)")]
    public class DptAngularFrequency : Dpt4ByteFloat
    {
        private DptAngularFrequency()
        {
        }

        public DptAngularFrequency(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptAngularFrequency(float value)
            : base(value)
        {
        }
    }
}