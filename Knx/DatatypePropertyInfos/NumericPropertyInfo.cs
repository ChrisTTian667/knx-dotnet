using System;

namespace Knx.DatatypePropertyInfos
{
    public sealed class NumericPropertyInfo : DatatypePropertyInfoWithRange<object>
    {
        public NumericPropertyInfo(string name, string unit, double minValue, double maxValue) : base(name, unit, minValue, maxValue)
        {
        }

        public NumericPropertyInfo(string name, string unit, Type propertyType, string minValue, string maxValue)
            : base(name, unit, propertyType, minValue, maxValue)
        {
        }
    }
}