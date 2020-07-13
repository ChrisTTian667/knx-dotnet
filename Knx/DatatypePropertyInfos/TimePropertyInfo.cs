using System;

namespace Knx.DatatypePropertyInfos
{
    public sealed class TimePropertyInfo : DatatypePropertyInfo<TimeSpan>
    {
        public TimePropertyInfo(string name, string unit) : base(name, unit)
        {
        }
    }
}