using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(2, 9, "Reserved")]
    [DatapointEnumValueDescription(15, 19, "Reserved HVAC")]
    [DatapointEnumValueDescription(21, 29, "Reserved Lightning")]
    [DatapointEnumValueDescription(31, 39, "Reserved Security")]
    [DatapointEnumValueDescription(41, 49, "Reserved Security")]
    [DatapointEnumValueDescription(51, 255, "Reserved")]
    public enum ApplicationArea : byte
    {
        NoFault = 0,
        SystemAndFunctionsOfCommonInterest = 1,
        HVACGeneralFBs = 10,
        HVACHotWaterHeating = 11,
        HVACDirectElectricalHeating = 12,
        HVACTerminalUnits = 13,
        HVAC_VAC = 14,
        Lightning = 20,
        Security = 30,
        LoadManagement = 40,
        ShuttersAndBlinds = 50
    }
}