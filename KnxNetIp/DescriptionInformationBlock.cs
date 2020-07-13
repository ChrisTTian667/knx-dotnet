using Knx.Common;

namespace Knx.KnxNetIp
{
    public class DescriptionInformationBlock
    {
        protected DescriptionInformationBlock()
        {
        }

        public int Length { get; protected set; }

        public DescriptionType Type { get; protected set; }

        public byte[] Information { get; protected set; }

        public byte[] Payload { get; private set; }

        public static DescriptionInformationBlock Parse(byte[] bytes)
        {
            return new DescriptionInformationBlock(bytes);
        }

        protected DescriptionInformationBlock(byte[] bytes)
        {
            Payload = bytes;
            Length = bytes[0];
            Type = (DescriptionType) bytes[1];
            Information = bytes.ExtractBytes(2);
        }
    }
}