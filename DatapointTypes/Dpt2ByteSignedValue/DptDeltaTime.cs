using System;
using Knx.Common;

namespace Knx.DatapointTypes.Dpt2ByteSignedValue
{
    public abstract class DptDeltaTime : Dpt2ByteSignedValue
    {
        protected DptDeltaTime(byte[] payload) : base(payload)
        {
        }

        protected DptDeltaTime(TimeSpan timeSpan) : base(0)
        {
            Value = timeSpan;
        }

        [DatapointProperty]
        public new TimeSpan Value
        {
            get
            {
                return TimeSpanFromShort(base.Value);
            }

            set { base.Value = ShortFromTimeSpan(value); }
        }

        protected abstract TimeSpan TimeSpanFromShort(short value);

        protected abstract short ShortFromTimeSpan(TimeSpan timeSpan);
    }
}