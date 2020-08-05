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
    public class DatapointType
    {
        private static IQueryable<Type> _datapointTypes;
        private byte[] _payload = new byte[0];

        #region Constructors and Destructors

        protected DatapointType()
        {
            DatapointTypeId = GetId(GetType());
        }

        protected DatapointType(byte[] payload) : this(payload, false)
        {
            DatapointTypeId = GetId(GetType());
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

        [DataMember]
        public string DatapointTypeId
        {
            get;
            private set;
        }
        
        [DataMember]
        public virtual byte[] Payload
        {
            get => _payload;
            set
            {
                if (value.Length == 0)
                    throw new ArgumentOutOfRangeException("Payload", "Datapoint Type needs at least one byte of data.");

                _payload = value;
            }
        }

        /// <summary>
        /// Gets the datapointtype type for the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Type</returns>
        public static Type GetType(string id)
        {
            if (!TryGetType(id, out var datapointTypeType))
                throw new Exception($"There is no datapoint type with the specified id '{id}'");

            return datapointTypeType;
        }

        /// <summary>
        /// Returns a list of types of all supported Datapoint types.
        /// </summary>
        public static IQueryable<Type> GetTypes()
        {
            return _datapointTypes ??= typeof(DatapointType).GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any() && !t.IsAbstract).Select(ti => ti.AsType()).AsQueryable();
        }

        /// <summary>
        /// Tries to get the type of the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="datapointTypeType">Type of the datapoint type.</param>
        /// <returns><c>true</c> if the type could be found; otherwise <c>false</c></returns>
        public static bool TryGetType(string id, out Type datapointTypeType)
        {
            datapointTypeType = GetTypes().FirstOrDefault(t => t.GetCustomAttribute(typeof(DatapointTypeAttribute),false).ToString() == id);
            return (datapointTypeType != null);
        }

        public static string GetId(Type datapointTypeType)
        {
            if (datapointTypeType == typeof(DatapointType))
                return string.Empty;
            
            var datapointTypeAttribute = datapointTypeType.GetTypeInfo().GetCustomAttributes(typeof(DatapointTypeAttribute), true).FirstOrDefault() as DatapointTypeAttribute;
            if (datapointTypeAttribute == null)
                throw new InvalidOperationException("Type is missing DatapointType attribute.");

            return datapointTypeAttribute.ToString();
        }
    }
}