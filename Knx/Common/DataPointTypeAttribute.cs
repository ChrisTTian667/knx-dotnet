using System;

namespace Knx.Common
{
    /// <summary>
    /// DatapointTypeAttribute identifies a class as an DataPointType definition.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DatapointTypeAttribute : Attribute
    {
        public Int16 MainNumber { get; private set; }
        public Int16 SubNumber { get; private set; }
        public Unit Unit { get; private set; }
        public Usage Usage { get; private set; }
        public string Description { get; set; }

        public DatapointTypeAttribute(Int16 mainNumber, Int16 subNumber, Usage usage)
        {
            MainNumber = mainNumber;
            SubNumber = subNumber;
            Unit = Unit.None;
            Usage = usage;
        }

        public DatapointTypeAttribute(Int16 mainNumber, Int16 subNumber, Unit unit, Usage usage)
        {
            MainNumber = mainNumber;
            SubNumber = subNumber;
            Usage = usage;
            Unit = unit;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1:000}", MainNumber, SubNumber);
        }
    }
}
