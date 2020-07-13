using System.Collections;
using Knx.Common;

namespace Knx.ExtendedMessageInterface
{
    /// <summary>
    /// Represents the second control byte of the Common Extended Message Interface (cEMI)
    /// Its also called DLR-Byte which means (Destination-adress-flag, Routing-counter, Length) 
    /// </summary>
    internal class ControlByte2
    {
        /// <summary>
        /// Defines the maximum count of routes a message can be routed too.
        /// </summary>
        private const byte MaxRoutes = 7;

        #region properties

        public bool DestinationAddressIsKnxDeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the routing counter.
        /// 
        /// The routing counter starts a 7 and will be reduced by one, each 
        /// router it passes.
        /// </summary>
        /// <value>The routing counter.</value>
        public byte RoutingCounter { get; set; }

        /// <summary>
        /// Gets the hops, the message was delivered too.
        /// </summary>
        /// <value>The count of hops.</value>
        public byte Hops
        {
            get { return (byte)(MaxRoutes - RoutingCounter); }
        }

        #endregion

        #region construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlByte2"/> class.
        /// </summary>
        public ControlByte2()
        {
            DestinationAddressIsKnxDeviceAddress = false;
            RoutingCounter = MaxRoutes - 1;
        }

        #endregion

        #region (de-)serialization

        /// <summary>
        /// Toes the byte.
        /// </summary>
        /// <returns></returns>
        public byte ToByte()
        {
            BitArray bitArray = new BitArrayBuilder()
                .Add(!DestinationAddressIsKnxDeviceAddress)
                .Add(RoutingCounter, 3)
                .Add(0, 4) // add 4 empty bits ( don't know, what they mean, but we should find out some day... )
                .ToBitArray();
            return bitArray.ToByteArray()[0];
        }

        /// <summary>
        /// Deserializes the specified control byte.
        /// </summary>
        /// <param name="controlByte">The control byte.</param>
        public void Deserialize(byte controlByte)
        {
            BitArray controlBitArray = controlByte.ToBitArray();

            DestinationAddressIsKnxDeviceAddress = controlBitArray[0];

            RoutingCounter = new BitArray(new[]
                {
                    false,
                    false,
                    false,
                    false,
                    false,
                    controlBitArray[1],
                    controlBitArray[2],
                    controlBitArray[3]
                }).ToByteArray()[0];
        }

        #endregion
    }
}