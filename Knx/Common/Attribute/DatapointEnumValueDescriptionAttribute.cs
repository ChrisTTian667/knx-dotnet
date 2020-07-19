using System;

namespace Knx.Common
{
    /// <summary>
    /// Describes values that are not part of the actual enum.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Enum, AllowMultiple = true)]
    public class DatapointEnumValueDescriptionAttribute : Attribute
    {
        public byte StartIndex { get; }
        public byte EndIndex { get; }
        public string Description { get; }

        public DatapointEnumValueDescriptionAttribute(byte startIdx, byte endIndex, string description)
        {
            StartIndex = startIdx;
            EndIndex = EndIndex;
            Description = description;
        }
    }
}
