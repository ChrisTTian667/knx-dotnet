using System;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitBitset
{
    [DatapointType(21, 2, Usage.General)]
    [DataLength(8)]
    public class DptDeviceControl : DatapointType
    {
        private DptDeviceControl()
        {
        }

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
            get => Payload[0].GetBit(7);
            set => Payload[0] = Payload[0].SetBit(7, value);
        }

        [DatapointProperty]
        public bool DatagramWithOwnIndividualAddressReceived
        {
            get => Payload[0].GetBit(6);
            set => Payload[0] = Payload[0].SetBit(6, value);
        }

        [DatapointProperty]
        public bool VerifyMode
        {
            get => Payload[0].GetBit(5);
            set => Payload[0] = Payload[0].SetBit(5, value);
        }
    }
}