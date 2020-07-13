namespace Knx
{
    public class Endpoint
    {
        public Endpoint(string address, string datatype)
        {
            Address = address;
            Datatype = datatype;
        }

        public string Address { get; private set; }

        public string Datatype { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as Endpoint;
            if (other == null)
                return false;
            return Equals(other);
        }

        public bool Equals(Endpoint other)
        {
            return base.Equals(other)
                   && Address == other.Address
                   && Datatype == other.Datatype;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                   ^ Address.GetHashCode()
                   ^ Datatype.GetHashCode();
        }

    }
}