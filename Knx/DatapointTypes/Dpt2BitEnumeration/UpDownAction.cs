namespace Knx.DatapointTypes.Dpt2BitEnumeration
{
    public enum UpDownAction : byte
    {
        Down = 0x00,
        Up = 0x01,
        DownUp = 0x03,
        UpDown = 0x04,
    }
}