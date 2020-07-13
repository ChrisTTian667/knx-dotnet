using System;

namespace Knx
{
    public class DatatypePropertyInfoWithRange<T> : DatatypePropertyInfo<T>
    {
        public T MinValue { get; private set; }
        public T MaxValue { get; private set; }

        public DatatypePropertyInfoWithRange(string name, string unit, T minValue, T maxValue) : base(name, unit)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public DatatypePropertyInfoWithRange(string name, string unit, Type type, string minValue, string maxValue) : base(name, unit)
        {
            MinValue = (T)Convert.ChangeType(minValue.FixMinMaxDoubleBug(), type, null);
            MinValue = (T)Convert.ChangeType(maxValue.FixMinMaxDoubleBug(), type, null);

            PropertyType = type;
        }
    }

    internal static class StringPatches
    {
        internal static string FixMinMaxDoubleBug(this string doubleValue)
        {
            return string.IsNullOrWhiteSpace(doubleValue) ? "0" : doubleValue.Replace("79769313486232E+308", "79769313486231E+308");
        }
    }
}