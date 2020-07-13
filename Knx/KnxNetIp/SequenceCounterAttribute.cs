using System;

namespace Knx.KnxNetIp
{
    /// <summary>
    /// The sequence counter attribute is used to define the sequence counter property in a messageBody.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SequenceCounterAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceCounterAttribute"/> class.
        /// </summary>
        public SequenceCounterAttribute()
        {
            IncrementOnSendMessage = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the sequence counter must be [increment on sending the message].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [increment on send message]; otherwise, <c>false</c>.
        /// </value>
        public bool IncrementOnSendMessage { get; set; }

        #endregion
    }
}