using System;

namespace Knx.Common.Attribute;

/// <summary>
///     Custom RangeAttribute to use within portable class libraries.
/// </summary>
public class RangeAttribute : System.Attribute
{
    protected RangeAttribute(object minimum, object maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public RangeAttribute(
        double minimum,
        double maximum
    )
        : this(minimum, (object)maximum)
    {
    }

    public RangeAttribute(
        int minimum,
        int maximum
    )
        : this(minimum, (object)maximum)
    {
    }

    public RangeAttribute(
        Type type,
        string minimum,
        string maximum
    )
    {
        Minimum = Convert.ChangeType(minimum, type, null);
        Maximum = Convert.ChangeType(maximum, type, null);
    }

    /// <summary>
    ///     Gets the minimum.
    /// </summary>
    public object Minimum { get; }

    /// <summary>
    ///     Gets the maximum.
    /// </summary>
    public object Maximum { get; }

    /// <summary>
    ///     Gets or sets the error message.
    /// </summary>
    /// <value>The error message.</value>
    public string ErrorMessage { get; set; }
}