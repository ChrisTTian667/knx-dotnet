using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Knx
{
    /// <summary>
    /// IParameter can create a Value or a Placeholder
    /// </summary>
    public interface IParameter
    {
        IValue CreateValue();

        string DatatypePropertyName { get; }
    }
    
    [DataContract(IsReference = true)]
    public sealed class Datatype : BindableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Datatype"/> class.
        /// </summary>
        /// <param name="datatypeInfo">The datatype info.</param>
        public Datatype(DatatypeInfo datatypeInfo)
        {
            Set(() => DatatypeInfo, datatypeInfo);
            CreateProperties();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the datatype info.
        /// </summary>
        public DatatypeInfo DatatypeInfo
        {
            get
            {
                return Get(() => DatatypeInfo);
            }

            private set
            {
                Set(() => DatatypeInfo, value);
            }
        }

        /// <summary>
        /// Gets the datatype id.
        /// </summary>
        public string Id
        {
            get
            {
                return Get(() => DatatypeInfo).Id;
            }
        }

        public IEnumerable<IDatatypeProperty> Properties { get; private set; }

        #endregion

        #region Methods

        private void CreateProperties()
        {
            var values = new Collection<IDatatypeProperty>();
            foreach (var propInfo in DatatypeInfo.PropertyInfos)
            {
                var parameterType = typeof(DatatypeProperty<>).MakeGenericType(propInfo.PropertyType);
                var parameter = (IDatatypeProperty)Activator.CreateInstance(parameterType, propInfo);

                values.Add(parameter);
            }

            this.Properties = values;
        }

        public void AssignParameterValues(IEnumerable<IParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentException("Parameter parameters may not be null");

            foreach (var parameter in parameters)
            {
                if (parameter == null)
                {
                   // Logger.Debug("PARAMETER NULL IN AssignParameterValues");
                    continue;
                }
                var property = this.Properties.FirstOrDefault(p => p.PropertyInfo.Name == parameter.DatatypePropertyName);
                if (property != null)
                {
                    var value = parameter.CreateValue();
                    property.Value = value;
                }
                else
                {
                    //Logger.Debug("property NULL  in AssignParameterValues");
                }
            }
        }

        #endregion
    }
}