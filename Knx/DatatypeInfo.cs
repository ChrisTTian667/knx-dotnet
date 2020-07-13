using System.Collections.Generic;
using System.Runtime.Serialization;
using Knx.PropertyInfoFactories;

namespace Knx
{
    [DataContract(IsReference = true)]
    public sealed class DatatypeInfo
    {
        #region Constructors and Destructors

        public DatatypeInfo(string id, string description, int length, IEnumerable<IDatatypePropertyInfo> propertyInfos)
        {
            Id = id;
            Description = description;
            Length = length;
            PropertyInfos = propertyInfos;
        }

        #endregion

        #region Properties

        [DataMember]
        public string Id { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public int Length { get; private set; }

        [DataMember]
        public IEnumerable<IDatatypePropertyInfo> PropertyInfos { get; private set; }

        /// <summary>
        /// Creates a new Datatype instance.
        /// </summary>
        /// <returns></returns>
        public Datatype CreateDatatype()
        {
            return new Datatype(this);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return (obj is DatatypeInfo) && (obj as DatatypeInfo).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}