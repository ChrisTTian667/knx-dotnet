using System;

namespace Knx.Common.Attribute;

/// <summary>
///     DatapointTypeAttribute identifies a class as an DataPointType definition.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class DatapointTypeAttribute : System.Attribute
{
    public DatapointTypeAttribute(short mainNumber, short subNumber, Usage usage)
    {
        MainNumber = mainNumber;
        SubNumber = subNumber;
        Unit = Unit.None;
        Usage = usage;
    }

    public DatapointTypeAttribute(short mainNumber, short subNumber, Unit unit, Usage usage)
    {
        MainNumber = mainNumber;
        SubNumber = subNumber;
        Usage = usage;
        Unit = unit;
    }

    public short MainNumber { get; }
    public short SubNumber { get; }
    public Unit Unit { get; }
    public Usage Usage { get; }
    public string Description { get; set; }

    public override string ToString() =>
        $"{MainNumber}.{SubNumber:000}";
}
