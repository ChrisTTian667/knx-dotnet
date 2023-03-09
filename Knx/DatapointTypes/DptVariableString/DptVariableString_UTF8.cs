﻿using System;
using System.Text;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes.DptVariableString
{
    [DatapointType(28, 1, Usage.General)]
    [DataLength(1, -1)]
    public class DptVariableString_UTF8 : DptVariableString
    {
        public DptVariableString_UTF8(byte[] payload)
            : base(payload)
        {
        }

        public DptVariableString_UTF8(string character)
            : base(character)
        {
        }

        private Encoding Encoding
        {
            get
            {
                var encoding = Encoding.UTF8;
                if (encoding == null)
                {
                    throw new Exception("Unable to retrieve encoding 'UTF-8'");
                }

                return encoding;
            }
        }

        protected override byte[] ToBytes(string value)
        {
            var byteArray = new byte[value.Length];
            var encodedBytes = Encoding.GetBytes(value);

            for (var i = 0; i < encodedBytes.Length; i++)
            {
                byteArray[i] = encodedBytes[i];
            }

            return byteArray;
        }

        protected override string ToValue(byte[] bytes)
        {
            return Encoding.GetString(bytes, 0, bytes.Length).TrimEnd('\0');
        }
    }
}
