using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4ByteSignedValue
{
    [DatapointType(13, 13, Unit.kWh, Usage.General)]
    public class DptActiveEnergykWh : Dpt4ByteSignedValue
    {
        public DptActiveEnergykWh(byte[] payload)
            : base(payload)
        {
        }

        public DptActiveEnergykWh(int value)
            : base(value)
        {
        }
    }
}