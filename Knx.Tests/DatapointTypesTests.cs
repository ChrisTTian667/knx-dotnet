using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Knx.Common;
using Knx.Common.Attribute;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Knx.DatapointTypes.Dpt2Bit;
using Newtonsoft.Json;
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
        public void CreateSimpleDatapointType()
        {
            var boolDpt = DatapointTypeFactory.Create("1.001", new byte[]{ 1 });
            Assert.IsNotNull(boolDpt);
        }
        
        [Test]
        public void SerializeSimpleBooleanDatapointType()
        {
            var dpt = DatapointTypeFactory.Create("1.001", new byte[]{ 1 });
            var serializedObject = JsonConvert.SerializeObject(dpt);
            var deserializeObject = JsonConvert.DeserializeObject<DptBoolean>(serializedObject);
            
            Assert.IsNotNull(dpt);
            Assert.AreEqual(dpt.Payload, deserializeObject.Payload);
        }
        
        [Test]
        public void SerializeDptAlarmControl()
        {
            var dpt = DatapointTypeFactory.Create(typeof(DptAlarmControl), new byte[]{1});
            var serializedObject = JsonConvert.SerializeObject(dpt);
            var deserializeObject = JsonConvert.DeserializeObject<DptAlarmControl>(serializedObject);
            
            Assert.IsNotNull(dpt);
            Assert.AreEqual(dpt.Payload, deserializeObject.Payload);
        }
        
        [Test]
        public void EachDatapointType_Serialize_NoException()
        {
            var count = 0;
            foreach (var type in GetDatapointTypes().OrderBy((t) => ((DatapointTypeAttribute)t.GetCustomAttributes(typeof(DatapointTypeAttribute),true).First())?.ToString()))
            {
                try
                {
                    var dpt = DatapointTypeFactory.Create(type);
                    var serializeDpt = JsonConvert.SerializeObject(dpt);
                    var deserializeDpt =  JsonConvert.DeserializeObject(serializeDpt, type);

                    Assert.IsNotNull(deserializeDpt);
                    
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
            foreach (var type in GetDatapointTypes().OrderBy((t) => t.GetCustomAttributes(typeof(DatapointTypeAttribute),true).First()?.ToString()))
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