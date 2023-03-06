using System;

namespace Knx.Common.Attribute;

/// <summary>
///     Specifies a more user friendly property name.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DatapointPropertyAttribute : System.Attribute
{
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

    public string PropertyName { get; }

    public Unit Unit { get; }
}