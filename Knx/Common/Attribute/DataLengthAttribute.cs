﻿using System;

namespace Knx.Common.Attribute
{
    public enum DataLength
    {
        Infinite = -1,
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataLengthAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the length in bits.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; }

        public int MaximumLength { get; }

        public int MinimumRequiredBytes
        {
            get
            {
                return ((int) (Length/8)) + ((Length%8) > 0 ? 1 : 0);
            }
        }

        public DataLengthAttribute(int lengthInBit)
        {
            Length = lengthInBit;
        }

        public DataLengthAttribute(int minimum, int maximum) : this(minimum)
        {
            MaximumLength = maximum;
        }

        public DataLengthAttribute(int minimum, DataLength maximum)
            : this(minimum)
        {
            MaximumLength = (int)maximum;
        }

        public DataLengthAttribute(DataLength length)
        {
            Length = (int)length;
        }
    }
}
