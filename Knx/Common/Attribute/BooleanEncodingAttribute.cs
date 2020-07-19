using System;

namespace Knx.Common.Attribute
{
    [AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BooleanEncodingAttribute : System.Attribute
    {
        public BooleanEncodingAttribute(UnitEncoding falseEncoding, UnitEncoding trueEncoding)
        {
            FalseEncoding = falseEncoding;
            TrueEncoding = trueEncoding;
        }

        public UnitEncoding FalseEncoding { get; private set; }

        public UnitEncoding TrueEncoding { get; private set; }
    }
}