using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitBitset
{
    [DatapointType(21, 2, Usage.General)]
    [DataLength(8)]
    public class DptDeviceControl : DatapointType
    {
        public DptDeviceControl(byte[] payload)
            : base(payload)
        {
        }

        public DptDeviceControl(bool userStopped, bool datagramWithOwnIndividualAddressReceived, bool verifyMode)
        {
            Payload = new byte[1];

            UserStopped = userStopped;
            DatagramWithOwnIndividualAddressReceived = datagramWithOwnIndividualAddressReceived;
            VerifyMode = verifyMode;
        }

        [DatapointProperty]
        public bool UserStopped
        {
            get { return Payload[0].GetBit(7); }
            set
            {
                Payload[0] = Payload[0].SetBit(7, value);
                RaisePropertyChanged(() => UserStopped);
            }
        }

        [DatapointProperty]
        public bool DatagramWithOwnIndividualAddressReceived
        {
            get { return Payload[0].GetBit(6); }
            set
            {
                Payload[0] = Payload[0].SetBit(6, value);
                RaisePropertyChanged(() => DatagramWithOwnIndividualAddressReceived);
            }
        }

        [DatapointProperty]
        public bool VerifyMode
        {
            get { return Payload[0].GetBit(5); }
            set
            {
                Payload[0] = Payload[0].SetBit(5, value);
                RaisePropertyChanged(() => VerifyMode);
            }
        }
    }
}