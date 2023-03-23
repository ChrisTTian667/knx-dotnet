using Knx.Common;

namespace Knx.KnxNetIp;

public class DescriptionInformationBlock
{
    protected DescriptionInformationBlock(byte[] bytes)
    {
        Payload = bytes;
        Length = bytes[0];
        Type = (DescriptionType)bytes[1];
        Information = bytes.ExtractBytes(2);
    }

    public int Length { get; }

    public DescriptionType Type { get; }

    public byte[] Information { get; }

    public byte[] Payload { get; }

    public static DescriptionInformationBlock Parse(byte[] bytes)
    {
        return new(bytes);
    }
}