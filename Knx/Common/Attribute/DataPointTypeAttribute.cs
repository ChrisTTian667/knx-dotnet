using System;

namespace Knx.Common.Attribute
{
    /// <summary>
    /// DatapointTypeAttribute identifies a class as an DataPointType definition.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DatapointTypeAttribute : System.Attribute
    {
        public short MainNumber { get; private set; }
        public short SubNumber { get; private set; }
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
            return $"{MainNumber}.{SubNumber:000}";
        }
    }
}
