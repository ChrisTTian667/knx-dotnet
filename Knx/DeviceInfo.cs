namespace Knx;

public struct DeviceInfo
{
    internal DeviceInfo(string friendlyName, string connectionString)
    {
        FriendlyName = friendlyName ?? string.Empty;
        ConnectionString = connectionString ?? string.Empty;
    }

    public string FriendlyName { get; }
    public string ConnectionString { get; }

    public override string ToString()
    {
        return $"{FriendlyName}: {ConnectionString}";
    }
}