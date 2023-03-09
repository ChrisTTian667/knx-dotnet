using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitBitset;

[DatapointType(21, 1, Usage.General)]
[DataLength(8)]
public class DptGeneralStatus : DatapointType
{
    private DptGeneralStatus()
    {
    }

    public DptGeneralStatus(byte[] payload)
        : base(payload)
    {
    }

    public DptGeneralStatus(bool outOfService, bool fault, bool overridden, bool inAlarm, bool alarmUnAck)
    {
        Payload = new byte[1];

        OutOfService = outOfService;
        Fault = fault;
        Overridden = overridden;
        InAlarm = inAlarm;
        AlarmUnacknowledged = alarmUnAck;
    }

    [DatapointProperty]
    public bool OutOfService
    {
        get => Payload[0].GetBit(7);
        set => Payload[0] = Payload[0].SetBit(7, value);
    }

    [DatapointProperty]
    public bool Fault
    {
        get => Payload[0].GetBit(6);
        set => Payload[0] = Payload[0].SetBit(6, value);
    }

    [DatapointProperty]
    public bool Overridden
    {
        get => Payload[0].GetBit(5);
        set => Payload[0] = Payload[0].SetBit(5, value);
    }

    [DatapointProperty]
    public bool InAlarm
    {
        get => Payload[0].GetBit(4);
        set => Payload[0] = Payload[0].SetBit(4, value);
    }

    [DatapointProperty]
    public bool AlarmUnacknowledged
    {
        get => Payload[0].GetBit(3);
        set => Payload[0] = Payload[0].SetBit(3, value);
    }
}