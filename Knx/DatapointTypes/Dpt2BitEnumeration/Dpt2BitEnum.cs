using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    [DataLength(2)]
    public class Dpt2BitEnum<T> : DptEnumeration<T>
    {
        protected Dpt2BitEnum()
        {
        }
        
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