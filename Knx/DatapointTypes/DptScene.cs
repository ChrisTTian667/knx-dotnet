using System;
using System.Linq;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes;

[DatapointType(17, 1, Unit.Scene, Usage.General)]
[DataLength(8)]
public class DptScene : DatapointType
{
    protected DptScene()
    {
    }

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
        get => (byte)((Payload.Length > 0 ? (byte)(Payload[0] & 0x3F) : 0) + 1);

        init
        {
            if (value is < 1 or > 64)
                throw new ArgumentOutOfRangeException(nameof(Scene), "Scene must be within 1 ... 64");

            if (!Payload.Any())
                Payload = new byte[1];

            Payload[0] = (byte)((Payload[0] & 0xC0) | (value - 1));
        }
    }
}