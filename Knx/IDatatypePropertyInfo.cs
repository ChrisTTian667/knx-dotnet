using System;

namespace Knx
{
    public interface IDatatypePropertyInfo
    {
        #region Properties

        string Name { get; }

        Type PropertyType { get; }

        string Unit { get; }

        string GetDisplayValue(object value);

        #endregion
    }
}