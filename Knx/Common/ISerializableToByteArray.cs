namespace Knx.Common
{
    public interface ISerializableToByteArray
    {
        #region Public Methods

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        void Deserialize(byte[] bytes);

        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <param name="byteArrayBuilder">The byte array builder.</param>
        void ToByteArray(ByteArrayBuilder byteArrayBuilder);

        #endregion
    }
}