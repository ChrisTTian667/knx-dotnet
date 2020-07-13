using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{
    [ResponseMessage(typeof(ConnectionStateResponse))]
    public class ConnectionStateRequest : TunnelingMessageBody
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStateRequest"/> class.
        /// </summary>
        public ConnectionStateRequest()
        {
            this.HostProtocolAddressInfo = new KnxHpai();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the host protocol address info.
        /// </summary>
        /// <value>The host protocol address info.</value>
        public KnxHpai HostProtocolAddressInfo { get; private set; }

        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.ConnectionStateRequest; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public override void Deserialize(byte[] bytes)
        {
            var hpaiLength = (int)bytes[2]; // get the length for the "HostProtocolAddressInformation //
            var hpaiBytes = new byte[hpaiLength]; // extract the hpai bytes
            Array.Copy(bytes, 2, hpaiBytes, 0, hpaiLength); // parse the host protocol address information

            this.CommunicationChannel = bytes[0];
            this.HostProtocolAddressInfo = KnxHpai.Parse(hpaiBytes);
        }

        /// <summary>
        /// Serialize to ByteArray.
        /// </summary>
        /// <param name="byteArrayBuilder">The byte array builder.</param>
        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            byteArrayBuilder.AddByte(this.CommunicationChannel).AddByte((byte)ErrorCode.NoError).Add(
                this.HostProtocolAddressInfo.ToByteArray());
        }

        #endregion
    }
}