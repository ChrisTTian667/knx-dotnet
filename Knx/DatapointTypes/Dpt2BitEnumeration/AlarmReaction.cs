namespace Knx.DatapointTypes.Dpt2BitEnumeration;

public enum AlarmReaction : byte
{
    NoAlarmIsUsed = 0,
    AlarmPositionUp = 1,
    AlarmPositionDown = 3,
    Reserved = 4
}