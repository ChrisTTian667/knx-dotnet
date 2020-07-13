using System;

namespace Knx.DatatypePropertyInfos
{
    [Obsolete]
    public sealed class LongPropertyInfo : DatatypePropertyInfoWithRange<long>
    {
        public LongPropertyInfo(string name, string unit, long minValue, long maxValue) : base(name, unit, minValue, maxValue)
        {
        }
    }
}