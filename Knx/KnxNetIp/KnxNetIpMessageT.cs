using System;
using System.Linq;
using Knx.Common;
using Knx.KnxNetIp.MessageBody;

namespace Knx.KnxNetIp
{
    /// <summary>
    /// base class for all Knx messages
    /// </summary>
    public class KnxNetIpMessage<T> : KnxNetIpMessage where T : MessageBodyBase, new()
    {
        #region Constants and Fields

        /// <summary>
        /// The protocol version
        /// </summary>
        private const int ProtocolVersion = 0x10;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxNetIpMessage{T}"/> class.
        /// </summary>
        public KnxNetIpMessage()
        {
            Body = Activator.CreateInstance<T>();
            ServiceType = Body.ServiceType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public new T Body
        {
            get => base.Body as T;
            internal set => base.Body = value;
        }

        /// <summary>
        /// Gets the bytearray length.
        /// </summary>
        /// <value>The length.</value>
        public int Length => ToByteArray().Length;

        #endregion

        #region Public Methods

        /// <summary>
        /// Does the byte array.
        /// </summary>
        /// <returns>
        /// a new <c>byte[]</c> representing it's message data
        /// </returns>
        public override byte[] ToByteArray()
        {
            ByteArrayToken totalLengthToken;

            ByteArrayBuilder arrayBuilder =
                new ByteArrayBuilder()
                    .AddByte(HeaderLength)
                    .AddByte(ProtocolVersion)
                    .AddInt((int) ServiceType)
                    .AddToken(2, out totalLengthToken);

            Body.ToByteArray(arrayBuilder);

            arrayBuilder.ReplaceToken(totalLengthToken, arrayBuilder.Length);

            return arrayBuilder.ToByteArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to be deserialized.</param>
        protected override void Deserialize(byte[] bytes)
        {
            Body.Deserialize(bytes.ExtractBytes(HeaderLength, bytes.Count() - HeaderLength));
        }

        #endregion

        public override string ToString()
        {
            return string.Format("KnxNetIp {0}", Body != null ? Body.ToString() : "empty");
        }
    }
}