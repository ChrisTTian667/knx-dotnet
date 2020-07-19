using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DatapointEnumValueDescription(19, 255, "Reserved")]
    public enum ErrorClassSystem : byte
    {
        NoFault = 0,

        /// <summary>
        /// e.g. RAM, EEPROM, UI, Watchdog, ...
        /// </summary>
        GeneralDeviceFault = 1,

        CommunicationFault = 2,
        ConfigurationFault = 3,
        HardwareFault = 4,
        SoftwareFault = 5,
        InsufficientNonVolatileMemory = 6,
        InsufficientVolatileMemory = 7,
        MemoryAllocationCommandWithSizeOfZeroReceived = 8,
        CRCError = 9,
        WatchdogResetDetected = 10,
        InvalidOpCodeDetected = 11,
        GeneralProtectionFault = 12,
        MaximalTableLengthExceeded = 13,
        UndefinedLoadCommandReceived = 14,
        GroupAddressTableIsNotSorted = 15,
        InvalidConnectionNumber = 16,
        InvalidGroupObjectNumber = 17,
        GroupObjectTypeExceeded = 18,
    }
}