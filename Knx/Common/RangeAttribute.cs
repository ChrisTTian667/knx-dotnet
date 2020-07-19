using System;

namespace Knx.Common
{
    /// <summary>
    /// Custom RangeAttribute to use within portable class libraries.
    /// </summary>
    public class RangeAttribute : Attribute
    {
        /// <summary>
        /// Gets the minimum.
        /// </summary>
        public object Minimum { get; private set; }

        /// <summary>
        /// Gets the maximum.
        /// </summary>
        public object Maximum { get; private set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        protected RangeAttribute(object minimum, object maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public RangeAttribute(
            double minimum,
            double maximum
        )
            : this((object)minimum, (object)maximum)
        {
        }

        public RangeAttribute(
            int minimum,
            int maximum
        )
            : this((object)minimum, (object)maximum)
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
    }
}