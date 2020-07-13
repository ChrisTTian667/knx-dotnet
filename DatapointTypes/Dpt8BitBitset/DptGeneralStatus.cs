using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitBitset
{
    [DatapointType(21, 1, Usage.General)]
    [DataLength(8)]
    public class DptGeneralStatus : DatapointType
    {
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
            get { return Payload[0].GetBit(7); }
            set
            {
                Payload[0] = Payload[0].SetBit(7, value);
                RaisePropertyChanged(() => OutOfService);
            }
        }

        [DatapointProperty]
        public bool Fault
        {
            get { return Payload[0].GetBit(6); }
            set
            {
                Payload[0] = Payload[0].SetBit(6, value);
                RaisePropertyChanged(() => Fault);
            }
        }

        [DatapointProperty]
        public bool Overridden
        {
            get { return Payload[0].GetBit(5); }
            set
            {
                Payload[0] = Payload[0].SetBit(5, value);
                RaisePropertyChanged(() => Overridden);
            }
        }

        [DatapointProperty]
        public bool InAlarm
        {
            get { return Payload[0].GetBit(4); }
            set
            {
                Payload[0] = Payload[0].SetBit(4, value);
                RaisePropertyChanged(() => InAlarm);
            }
        }

        [DatapointProperty]
        public bool AlarmUnacknowledged
        {
            get { return Payload[0].GetBit(3); }
            set
            {
                Payload[0] = Payload[0].SetBit(3, value);
                RaisePropertyChanged(() => AlarmUnacknowledged);
            }
        }
    }
}
