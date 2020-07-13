using System;
using System.Reflection;
using Knx.PropertyInfoFactories;

namespace Knx
{
    public interface IPropertyInfoFactory
    {
        IDatatypePropertyInfo Create(PropertyInfo propertyInfo, Type datapointTypeType);
    }
}