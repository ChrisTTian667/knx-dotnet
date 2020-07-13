using Knx.Common;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DataLength(2)]
    public class Dpt2BitEnum<T> : DptEnumeration<T>
    {
        public Dpt2BitEnum(byte[] payload)
            : base(payload)
        {
        }

        public Dpt2BitEnum(T value)
            : base(value)
        {
        }
    }
}