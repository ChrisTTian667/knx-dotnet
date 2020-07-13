using System;

namespace Knx.Common
{
    /// <summary>
    /// Describes values that are not part of the actual enum.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Enum, AllowMultiple = true)]
    public class DatapointEnumValueDescriptionAttribute : Attribute
    {
        public byte StartIndex { get; private set; }
        public byte EndIndex { get; private set; }
        public string Description { get; private set; }

        public DatapointEnumValueDescriptionAttribute(byte startIdx, byte endIndex, string description)
        {
            StartIndex = startIdx;
            EndIndex = EndIndex;
            Description = description;
        }
    }
}
