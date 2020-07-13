using System;

namespace Knx.Common
{
    [AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BooleanEncodingAttribute : Attribute
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