using Knx.Common;
using Knx.Exceptions;
using Knx.ExtendedMessageInterface;

namespace Knx.KnxNetIp.MessageBody
{
    /// <summary>
    /// Defines the message body of a TunnelingRequest message.
    /// </summary>
    [ResponseMessage(typeof(TunnelingAcknowledge))]
    public class TunnelingRequest : TunnelingMessageBody
    {
        #region Properties

        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.TunnelingRequest; }
        }

        /// <summary>
        /// Gets or sets the cemi.
        /// </summary>
        /// <value>The cemi.</value>
        public IKnxMessage Cemi { get; set; }

        /// <summary>
        /// Gets or sets the sequence counter.
        /// </summary>
        /// <value>The sequence counter.</value>
        [SequenceCounter(IncrementOnSendMessage = true)]
        public byte SequenceCounter { get; set; }

        /// <summary>
        /// Gets the length of the communication header.
        /// (SequenceCounter + CommunicationChannel)
        /// </summary>
        /// <value>The length.</value>
        private static byte Length
        {
            get
            {
                return 4;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public override void Deserialize(byte[] bytes)
        {
            if (bytes[0] != 4)
            {
                throw new KnxException("Could not parse ConnectionRequest: " + bytes);
            }

            this.CommunicationChannel = bytes[1];
            this.SequenceCounter = bytes[2];
            // bytes[3] is reserved (should be always 0x00)

            // starting at byte index 4
            this.Cemi = KnxMessage.Deserialize(bytes.ExtractBytes(4));
        }

        /// <summary>
        /// Serialize to ByteArray.
        /// </summary>
        /// <param name="byteArrayBuilder">The byte array builder.</param>
        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            byteArrayBuilder
                .AddByte(Length)
                .AddByte(this.CommunicationChannel)
                .AddByte(this.SequenceCounter)
                .AddByte(0)  // reserved
                .Add(this.Cemi.ToByteArray());
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} Seq: {1,-3} Msg: {2}", base.ToString(), SequenceCounter, Cemi != null ? Cemi.ToString() : "empty");
        }
    }
}