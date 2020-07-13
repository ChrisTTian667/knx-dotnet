using System;
using System.Collections.Generic;
using System.Linq;
using Knx.Common;
using Knx.DatapointTypes;
using NUnit.Framework;

namespace Knx.Tests
{
    /// <summary>
    /// General Datapoint types test
    /// </summary>
    
    public class DatapointTypesTests
    {
        #region Public Methods

        [Test]
        public void EachDatapointTypeHasDataLengthAttribute()
        {
            var datapointTypesCount = GetCountOfDatapointTypes();
            var allDatapointTypesWithDataLengthAttributeCount =
                typeof(DatapointType).Assembly.GetTypes().Where(
                    t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any()).Where(t => t.GetCustomAttributes(typeof(DataLengthAttribute), true).Count() == 1).Count(t => !t.IsAbstract);

            Assert.AreEqual(datapointTypesCount, allDatapointTypesWithDataLengthAttributeCount);
        }

        [Test]
        public void InstanciateEachDatapointType_NoException()
        {
            var count = 0;
            foreach (var type in GetDatapointTypes().OrderBy((t) => t.GetFirstCustomAttribute<DatapointTypeAttribute>(true).ToString()))
            {
                try
                {
                    var dpt = DatapointTypeFactory.Create(type);
                    Assert.IsNotNull(dpt);

                    count++;
                }
                catch (MissingMethodException ex)
                {
                    Assert.Fail($"Type {type}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Type {type}: {ex.Message}");
                }
            }

            Assert.AreEqual(GetCountOfDatapointTypes(), count);
        }

        #endregion Public Methods

        #region Methods

        private static int GetCountOfDatapointTypes()
        {
            return GetDatapointTypes().Count();
        }

        private static IEnumerable<Type> GetDatapointTypes()
        {
            return typeof(DatapointType).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(DatapointTypeAttribute), false).Any()).Where(t => !t.IsAbstract);
        }

        #endregion Methods
    }
}