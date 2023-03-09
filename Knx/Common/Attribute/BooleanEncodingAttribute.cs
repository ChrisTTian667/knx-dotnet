using System;

namespace Knx.Common.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class BooleanEncodingAttribute : System.Attribute
{
    public BooleanEncodingAttribute(UnitEncoding falseEncoding, UnitEncoding trueEncoding)
    {
        FalseEncoding = falseEncoding;
        TrueEncoding = trueEncoding;
    }

    public UnitEncoding FalseEncoding { get; }

    public UnitEncoding TrueEncoding { get; }
}