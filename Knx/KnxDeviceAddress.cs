﻿using System.Collections;
using Knx.Common;

namespace Knx
{
    public class KnxDeviceAddress : KnxAddress
    {
        #region private fields

        private byte _area;
        private byte _device;
        private byte _line;

        #endregion

        #region construction

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxDeviceAddress"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="line">The line.</param>
        /// <param name="device">The device.</param>
        public KnxDeviceAddress(byte area, byte line, byte device)
        {
            Area = area;
            Line = line;
            Device = device;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxDeviceAddress"/> class.
        /// </summary>
        /// <param name="bytes">ByteArray with Length 2</param>
        public KnxDeviceAddress(byte[] bytes)
        {
            _area = bytes[0].HighBits();
            _line = bytes[0].LowBits();
            _device = bytes[1];
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the area address.
        /// </summary>
        /// <value>The area.</value>
        public byte Area
        {
            get { return _area; }
            set
            {
                ValidateValue(value, 0, 15, "Area");
                _area = value;

                InvokeChangeEvent();
            }
        }

        /// <summary>
        /// Gets or sets the line address.
        /// </summary>
        /// <value>The line.</value>
        public byte Line
        {
            get { return _line; }
            set
            {
                ValidateValue(value, 0, 15, "Line");
                _line = value;

                InvokeChangeEvent();
            }
        }

        /// <summary>
        /// Gets or sets the device address.
        /// </summary>
        /// <value>The device.</value>
        public new byte Device
        {
            get { return _device; }

            set
            {
                ValidateValue(value, 0, 255, "Device");
                _device = value;

                InvokeChangeEvent();
            }
        }

        #endregion

        protected override void FillBitArray(BitArray bitArray)
        {
            int currentIdx = 0;
            foreach (bool bit in Area.ConvertToBits(4))
            {
                bitArray[currentIdx] = bit;
                currentIdx++;
            }

            foreach (bool bit in Line.ConvertToBits(4))
            {
                bitArray[currentIdx] = bit;
                currentIdx++;
            }

            foreach (bool bit in Device.ConvertToBits(8))
            {
                bitArray[currentIdx] = bit;
                currentIdx++;
            }
        }

        public override string ToString()
        {
            return Area + "." + Line + "." + Device;
        }
    }
}