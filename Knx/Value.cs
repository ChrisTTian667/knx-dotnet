namespace Knx
{
    public class Value<T> : BindableObject, IValue<T>, IValue
    {
        #region Constructors and Destructors

        public Value(T value)
        {
            Current = value;
        }

        public Value(T value, string unit) : this(value)
        {
            Unit = unit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the current value.
        /// </summary>
        public T Current
        {
            get
            {
                return Get(() => Current);
            }
            set
            {
                Set(() => Current, value);
            }
        }

        public string Unit
        {
            get
            {
                return Get(() => Unit);
            }
            set
            {
                Set(() => Unit, value);
            }
        }

        /// <summary>
        /// Returns the current value as object.
        /// </summary>
        object IValue.Current
        {
            get
            {
                return (T)Current;
            }
            set
            {
                Current = (T)value;
            }
        }        

        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as Value<T>;
            return other != null && Current.Equals(other.Current);
        }

        protected bool Equals(Value<T> other)
        {
            return other != null && Current.Equals(other.Current);
        }

        public override int GetHashCode()
        {
            return Unit.GetHashCode() ^ Current.GetHashCode();
        }
    }
}