using System;

namespace Knx.Common.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public abstract class DatapointPropertyEncodingAttribute : System.Attribute
{
}

public class BooleanEncodingAttribute : DatapointPropertyEncodingAttribute
{
    public BooleanEncodingAttribute(UnitEncoding falseEncoding, UnitEncoding trueEncoding)
    {
        FalseEncoding = falseEncoding;
        TrueEncoding = trueEncoding;
    }

    public UnitEncoding FalseEncoding { get; }

    public UnitEncoding TrueEncoding { get; }
}
