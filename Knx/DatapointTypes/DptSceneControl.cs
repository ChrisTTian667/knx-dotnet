using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DatapointType(18, 1, Unit.Scene, Usage.General)]
    [DataLength(8)]
    public class DptSceneControl : DptScene
    {
        public enum SceneControl : byte
        {
            /// <summary>
            /// Activate the scene corresponsing to the field scene Number.
            /// </summary>
            Activate = 0x00,

            /// <summary>
            /// Learn the scene corresponding to the field scene Number.
            /// </summary>
            Learn = 0x01
        }

        public DptSceneControl(byte[] payload)
            : base(payload)
        {
        }

        public DptSceneControl(SceneControl control, byte scene)
            : base(scene)
        {
            Control = control;
        }

        [DatapointProperty]
        public SceneControl Control
        {
            get
            {
                return Payload.Length > 0 ? (SceneControl)(Payload[0] >> 7) : 0;
            }

            set
            {
                if (!Payload.Any())
                {
                    Payload = new byte[1];
                }

                Payload[0] = Payload[0].SetBit(0, ((byte)value) == 1);
            }
        }
    }
}