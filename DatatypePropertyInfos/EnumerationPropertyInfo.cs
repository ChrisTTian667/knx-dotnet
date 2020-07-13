using System;
using System.Collections.Generic;
using System.Linq;

namespace Knx.DatatypePropertyInfos
{
    public sealed class EnumerationPropertyInfo : DatatypePropertyInfo<Enum>
    {
        private new List<string> _values;

        public IEnumerable<string> Values
        {
            get => _values;
            private set => _values = new List<string>(value);
        }

        public EnumerationPropertyInfo(string name, string unit, IEnumerable<string> enumerationNames)
            : base(name, unit)
        {
            Values = enumerationNames;
        }

        public override string GetDisplayValue(object value)
        {
            return value is Enum ? value.ToString() : string.Empty;
        }

        public override bool IsNullable
        {
            get => base.IsNullable;
            set
            {
                base.IsNullable = value;

                if (Values.Contains(Resources.Strings.Enum_NothingSelected) != value)
                {
                    if (value)
                        _values.Add(Resources.Strings.Enum_NothingSelected);
                    else
                        _values.Remove(Resources.Strings.Enum_NothingSelected);
                }
            }
        }
    }
}