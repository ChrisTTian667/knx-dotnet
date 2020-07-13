using Knx.Common;

namespace Knx.DatapointTypes.Dpt4ByteFloatValue
{
    [DatapointType(14, 64, Unit.SoundIntensity, Usage.General, Description = "sound intensity")]
    public class DptSoundIntensity : Dpt4ByteFloat
    {
        public DptSoundIntensity(byte[] twoBytes)
            : base(twoBytes)
        {
        }

        public DptSoundIntensity(float value)
            : base(value)
        {
        }
    }
}