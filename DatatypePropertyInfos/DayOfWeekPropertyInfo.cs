using System;
using System.Collections.Generic;

namespace Knx.DatatypePropertyInfos
{
    public sealed class DayOfWeekPropertyInfo : DatatypePropertyInfo<DayOfWeek>
    {
        public IEnumerable<string> Values { get; private set; }
        
        public DayOfWeekPropertyInfo(string name, string unit, Type enumType, IEnumerable<string> values)
            : base(name, unit)
        {
            PropertyType = enumType;
            Values = values;
        }
    }
}