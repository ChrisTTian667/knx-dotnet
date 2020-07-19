namespace Knx.Common.Attribute
{
    public class StringLengthAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        public StringLengthAttribute(int length)
        {
            Length = length;
        }
    }
}