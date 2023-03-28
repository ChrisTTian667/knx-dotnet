using System;

namespace Knx.Common.Attribute;

/// <summary>
///     Describes values that are not part of the actual enum.
/// </summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
public class DatapointEnumValueDescriptionAttribute : System.Attribute
{
    public DatapointEnumValueDescriptionAttribute(byte startIdx, byte endIndex, string description)
    {
        StartIndex = startIdx;
        EndIndex = endIndex;
        Description = description;
    }

    public byte StartIndex { get; }
    public byte EndIndex { get; }
    public string Description { get; }
}
