using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Knx.Common;
using Knx.Common.Attribute;

namespace Knx.DatapointTypes
{
    [DataContract]
    public abstract class DatapointType : INotifyPropertyChanged
    {
        #region Constants and Fields

        #region Static Fields and Constants

        private static IQueryable<Type> _datapointTypes;

        #endregion

        #region Fields and Constants

        private byte[] _payload = new byte[0];


        #endregion

        #endregion

        #region Constructors and Destructors

        protected DatapointType()
        {
        }

        protected DatapointType(byte[] payload) : this(payload, false)
        {
        }

        protected DatapointType(byte[] payload, bool verifyExactPayloadLength) : this()
        {
            Payload = payload;

            int requiredBytes;
            var myType = GetType();
            if (!VerifyPayload(myType, payload, verifyExactPayloadLength, out requiredBytes))
            {
                if (requiredBytes < 0)
                {
                    throw new ArgumentException("Payload verification failed.", "payload");
                }

                throw new ArgumentOutOfRangeException("payload", string.Format(verifyExactPayloadLength ? "Payload needs to have a length of {0} bytes." : "Payload needs at least a length {0} bytes.", requiredBytes));
            }
        }

        #endregion

        public static bool VerifyPayload(Type datapointType, byte[] payload, bool exactMatch = false)
        {
            var dataLengthAttribute = datapointType.GetTypeInfo().GetCustomAttributes(typeof(DataLengthAttribute), true).Cast<DataLengthAttribute>().FirstOrDefault();
            if  ((dataLengthAttribute == null) || (dataLengthAttribute.MinimumRequiredBytes < 0))
            {
                return true;
            }

            return payload != null && (!exactMatch ? payload.Length >= dataLengthAttribute.MinimumRequiredBytes : payload.Length == dataLengthAttribute.MinimumRequiredBytes);
        }

        public static bool VerifyPayload(Type datapointType, byte[] payload, bool exactMatch, out int requiredBytes)
        {
            var dataLengthAttribute = datapointType.GetTypeInfo().GetCustomAttributes(typeof(DataLengthAttribute), true).Cast<DataLengthAttribute>().FirstOrDefault();
            if (dataLengthAttribute == null)
            {
                requiredBytes = -1;
                return true;
            }

            var isOk = (payload != null && (!exactMatch ? payload.Length >= dataLengthAttribute.MinimumRequiredBytes : payload.Length == dataLengthAttribute.MinimumRequiredBytes));
            requiredBytes = dataLengthAttribute.MinimumRequiredBytes;
            
            return isOk;
        }
        
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public string DatapointTypeId
        {
            get { return GetId(GetType()); }
        }

        [DataMember]
        public virtual byte[] Payload
        {
            get => _payload;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentOutOfRangeException("Payload", "Datapoint Type needs at least one byte of data.");

                SetProperty(ref _payload, value, () => Payload);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the datapointtype type for the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Type</returns>
        public static Type GetType(string id)
        {
            Type datapointTypeType;
            if (!TryGetType(id, out datapointTypeType))
            {
                throw new Exception(string.Format("There is no datapoint type with the specified id '{0}'", id));
            }

            return datapointTypeType;
        }

        /// <summary>
        /// Returns a list of types of all supported Datapoint types.
        /// </summary>
        public static IQueryable<Type> GetTypes()
        {
            //return _datapointTypes ??
            //       (_datapointTypes =
            //        typeof(DatapointType).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any() && !t.IsAbstract).AsQueryable());
            return _datapointTypes ??
                   (_datapointTypes =
                    typeof(DatapointType).GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any() && !t.IsAbstract).Select(ti => ti.AsType()).AsQueryable());
        }

        /// <summary>
        /// Tries to get the type of the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="datapointTypeType">Type of the datapoint type.</param>
        /// <returns><c>true</c> if the type could be found; otherwise <c>false</c></returns>
        public static bool TryGetType(string id, out Type datapointTypeType)
        {
            datapointTypeType = GetTypes().FirstOrDefault(t => t.GetFirstCustomAttribute<DatapointTypeAttribute>(false).ToString() == id);

            return (datapointTypeType != null);
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.GetPropertyName()));
            }
        }

        /// <summary>
        /// Sets the properties backing end field and raises the property change.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingField">The backing field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyExpression">The property expression.</param>
        protected void SetProperty<T>(ref T backingField, T value, Expression<Func<T>> propertyExpression)
        {
            backingField = value;
            RaisePropertyChanged(propertyExpression);
        }

        public static string GetId(Type datapointTypeType)
        {
            var datapointTypeAttribute = datapointTypeType.GetTypeInfo().GetCustomAttributes(typeof(DatapointTypeAttribute), true).FirstOrDefault() as DatapointTypeAttribute;
            if (datapointTypeAttribute == null)
            {
                throw new InvalidOperationException("Type is missing DatapointType attribute.");
            }

            return datapointTypeAttribute.ToString();
        }

        #endregion
    }
}