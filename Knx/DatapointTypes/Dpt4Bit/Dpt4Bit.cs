using System.Runtime.Serialization;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt4Bit;

[DataContract]
[DataLength(4)]
public abstract class Dpt4Bit : DatapointType
{
    protected Dpt4Bit()
    {
    }

    protected Dpt4Bit(byte[] payload)
        : base(payload)
    {
    }
}