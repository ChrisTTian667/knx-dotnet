using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{

    public class ConnectionStateResponse : TunnelingMessageBody
    {
        #region Properties

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ErrorCode State { get; private set; }

        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.ConnectionStateResponse; }
        }

        #endregion

        #region Public Methods

        public override void Deserialize(byte[] bytes)
        {
            this.CommunicationChannel = bytes[0];
            //this.State = (ErrorCode)Enum.Parse(typeof(ErrorCode), (((int)bytes[1]).ToString()));
            this.State = (ErrorCode)bytes[1];
        }

        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            byteArrayBuilder.AddByte(this.CommunicationChannel).AddByte((byte)this.State);
        }

        #endregion
    }
}