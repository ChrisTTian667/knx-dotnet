﻿using System;
using Knx.Common;

namespace Knx.KnxNetIp.MessageBody
{
    [ResponseMessage(typeof(ConnectionResponse))]
    public class ConnectionRequest : TunnelingMessageBody
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionRequest"/> class.
        /// </summary>
        public ConnectionRequest()
        {
            this.ControlEndpoint = new KnxHpai();
            this.DataEndpoint = new KnxHpai();
            this.Data = new ConnectRequestData();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the control endpoint.
        /// </summary>
        /// <value>The control endpoint.</value>
        public KnxHpai ControlEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public ConnectRequestData Data { get; set; }

        /// <summary>
        /// Gets or sets the data endpoint.
        /// </summary>
        /// <value>The data endpoint.</value>
        public KnxHpai DataEndpoint { get; set; }

        #endregion

        #region Public Methods

        public override KnxNetIpServiceType ServiceType
        {
            get { return KnxNetIpServiceType.ConnectionRequest; }
        }

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public override void Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the message as byte array.
        /// </summary>
        /// <returns></returns>
        public override void ToByteArray(ByteArrayBuilder byteArrayBuilder)
        {
            byteArrayBuilder
                .Add(this.ControlEndpoint.ToByteArray())
                .Add(this.DataEndpoint.ToByteArray())
                .Add(this.Data.ToByteArray());
        }

        #endregion
    }
}