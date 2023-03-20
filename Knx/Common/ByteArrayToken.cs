namespace Knx.Common;

internal sealed class ByteArrayToken
{
    public ByteArrayToken(int index, int bytesToAdd)
    {
        Index = index;
        BytesToAdd = bytesToAdd;
    }

    public int BytesToAdd { get; }

    public int Index { get; }
}