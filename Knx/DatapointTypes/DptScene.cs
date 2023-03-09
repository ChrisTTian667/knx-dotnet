using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DatapointType(17,1, Unit.Scene, Usage.General)]
    [DataLength(8)]
    public class DptScene : DatapointType
    {
        public DptScene(byte[] payload) : base(payload)
        {
        }

        public DptScene(byte scene)
        {
            Scene = scene;
        }

        [Range(1, 64, ErrorMessage = "Scene must be within 1 ... 64.")]
        [DatapointProperty]
        public byte Scene
        {
            get
            {
                return (byte)((Payload.Length > 0 ? (byte)(Payload[0] & 0x3F) : (byte)0) + 1);
            }
 
            set
            {
                if (value < 1 || value > 64)
                {
                    throw new Exception("Scene must be within 1 ... 64.");
                }

                if (!Payload.Any())
                {
                    Payload = new byte[1];
                }

                Payload[0] = (byte)((Payload[0] & 0xC0) | (value-1));
            }
        }
    }
}
