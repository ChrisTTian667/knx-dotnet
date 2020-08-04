using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.Dpt8BitEnumeration
{
    [DataLength(8)]
    public abstract class Dpt8BitEnum<T> : DptEnumeration<T>
    {
        protected Dpt8BitEnum()
        {
        }

        protected Dpt8BitEnum(byte[] payload)
            : base(payload)
        {
        }

        protected Dpt8BitEnum(T value)
            : base(value)
        {
        }
    }
}
