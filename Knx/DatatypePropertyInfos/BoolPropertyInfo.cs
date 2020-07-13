namespace Knx.DatatypePropertyInfos
{
    public sealed class BoolPropertyInfo : DatatypePropertyInfo<bool>
    {
        public BoolPropertyInfo(string name, string unit, string falseValue, string trueValue)
            : base(name, unit)
        {
            FalseValue = falseValue;
            TrueValue = trueValue;
        }

        public override string GetDisplayValue(object value)
        {
            if (value is bool)
            {
                return ((bool)value) ? TrueValue : FalseValue;
            }
            return string.Empty;
        }

        public string TrueValue { get; private set; }
        public string FalseValue { get; private set; }
    }
}