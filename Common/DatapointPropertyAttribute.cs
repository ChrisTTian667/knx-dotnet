using System;

namespace Knx.Common
{
    /// <summary>
    /// Specifies a more user friendly property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatapointPropertyAttribute : Attribute
    {
        public string PropertyName { get; private set; }        
        
        public Unit Unit { get; private set; }

        public DatapointPropertyAttribute()
        {
            Unit = Unit.None;
        }

        public DatapointPropertyAttribute(Unit unit)
        {
            Unit = unit;
        }

        public DatapointPropertyAttribute(string propertyName, Unit unit)
        {
            PropertyName = propertyName;
            Unit = unit;
        }
    }
}
