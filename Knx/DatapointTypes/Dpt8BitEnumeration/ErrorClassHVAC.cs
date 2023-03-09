using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration;

[DatapointEnumValueDescription(5, 255, "Reserved")]
public enum ErrorClassHVAC : byte
{
    NoFault = 0,
    SensorFault = 1,
    ProcessOrControllerFault = 2,
    ActuatorFault = 3,
    OtherFault = 4
}