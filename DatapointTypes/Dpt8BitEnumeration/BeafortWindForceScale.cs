using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(13, 255, "Reserved")]
    public enum BeafortWindForceScale : byte
    {
        /// <summary>
        /// No Wind
        /// </summary>
        Calm = 0,
        LightAir = 1,
        LightBreeze = 2,
        GentleBreeze = 3,
        ModerateBreeze = 4,
        FreshBreeze = 5,
        StrongBreeze = 6,
        NearGale = 7,
        FreshGale = 8,
        StrongGale = 9,
        WholeGale = 10,
        ViolentStorm = 11,
        Hurricane = 12,
    }
}