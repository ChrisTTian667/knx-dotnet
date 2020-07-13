using System;

namespace Knx.DatatypePropertyInfos
{
    public sealed class DateTimePropertyInfo : DatatypePropertyInfo<DateTime>
    {
        public DateTimePropertyInfo(string name, string unit)
            : base(name, unit)
        {
        }

        public override string GetDisplayValue(object value)
        {
            if (value is DateTime)
            {
                return ((DateTime)(value)).ToString();
            }
            return string.Empty;
        }
    }
}