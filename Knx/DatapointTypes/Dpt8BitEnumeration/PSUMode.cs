using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointEnumValueDescription(3, 255, "Reserved")]
public enum PSUMode : byte
{
    /// <summary>
    ///     PSU / DPSU fixed off
    /// </summary>
    Disabled = 0,

    /// <summary>
    ///     PSU / DPSU fixed on
    /// </summary>
    Enabled = 1,

    /// <summary>
    ///     PSU / DPSU automatic on/off
    /// </summary>
    Auto = 2
}