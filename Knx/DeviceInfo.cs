namespace Knx
{
    public struct DeviceInfo
    {
        public DeviceInfo(string deviceDriverId, string connectionString)
        {
            DeviceDriverId = deviceDriverId;
            ConnectionString = connectionString;
        }

        public string DeviceDriverId; // { get; private set; }

        public string ConnectionString; // { get; private set; }

        #region operators and overloads

        public static bool operator ==(DeviceInfo a, DeviceInfo b)
        {
            return a.DeviceDriverId == b.DeviceDriverId && a.ConnectionString == b.ConnectionString;
        }

        public static bool operator !=(DeviceInfo a, DeviceInfo b)
        {
            return a.DeviceDriverId != b.DeviceDriverId || a.ConnectionString != b.ConnectionString;
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            var that = (DeviceInfo)obj;
            if (null == that)
                return false;

            return this == that;
        }

        public override int GetHashCode()
        {
            return (13 * DeviceDriverId.GetHashCode())
                   ^ (13 * ConnectionString.GetHashCode());
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", DeviceDriverId, ConnectionString);
        }
    }
}