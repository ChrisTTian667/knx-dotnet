using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{
    public class ConnectionResponse : TunnelingMessageBody
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the connection.
        /// </summary>
        /// <value>The type of the connection.</value>
        public ConnectionType ConnectionType { get; private set; }

        /// <summary>
        /// Gets or sets the host protocol address info.
        /// </summary>
        /// <value>The host protocol address info.</value>
        public KnxHpai HostProtocolAddressInfo { get; private set; }

        /// <summary>
        /// Gets or sets the state of the connection.
        /// </summary>
        /// <value>The state.</value>
        public ErrorCode State { get; private set; }

        #endregion

        #region Public Methods

        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.ConnectionResponse; }
        }

        public override void Deserialize(byte[] bytes)
        {
            this.CommunicationChannel = bytes[0];
            //this.State = (ErrorCode)Enum.Parse(typeof(ErrorCode), (((int)bytes[1]).ToString()));
            this.State = (ErrorCode)bytes[1];

            if (this.State != ErrorCode.NoError)
            {
                return;
            }

            var hpaiLength = (int)bytes[2]; // get the length for the "HostProtocolAddressInformation //
            var hpaiBytes = new byte[hpaiLength]; // extract the hpai bytes
            Array.Copy(bytes, 2, hpaiBytes, 0, hpaiLength); // parse the host protocol address information

            this.HostProtocolAddressInfo = KnxHpai.Parse(hpaiBytes);

            // the connection type is written behind the hpai
            //this.ConnectionType =
            //    (ConnectionType)Enum.Parse(typeof(ConnectionType), (((int)bytes[hpaiLength + 2]).ToString()));

            this.ConnectionType = (ConnectionType)bytes[hpaiLength + 2];
        }

        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <param name="byteArrayBuilder">The byte array builder.</param>
        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            byteArrayBuilder.AddByte(this.CommunicationChannel).AddByte((byte)this.State);

            if (this.State != ErrorCode.NoError)
            {
                return;
            }

            byteArrayBuilder.Add(this.HostProtocolAddressInfo.ToByteArray()).AddByte(2) // Length
                .AddByte((byte)this.ConnectionType);
        }

        #endregion
    }
}