using System;
using System.Linq;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt8BitEnumeration;
using NUnit.Framework;

namespace Knx.Tests
{
    public class DatapointTypeEnumerationsTests
    {
        [Test]
        public void EnumerationsAreDerivedFromByteTest()
        {
            var types = typeof(DatapointType).Assembly.GetTypes();
            var enumTypes = types.Where(t => t.IsEnum);
            var knxEnumTypes = enumTypes.Where(t => t.FullName != null && t.FullName.Contains(nameof(DatapointType)));
            
            foreach (var enumType in knxEnumTypes)
            {
                foreach (var _ in Enum.GetValues(enumType).Cast<byte>())
                {
                    // Cast would throw an exception, if the value would not be able to cast to a byte.
                }
            }
        }

        [Test]
        public void Enumeration_WrapEnumeration_EnumerationOfEnumType()
        {
            var enumeration = new Enumeration(typeof(Priority));
            var names = enumeration.Names;
            var values = enumeration.Values;
            
            Assert.AreEqual(4, names.Distinct().Count());
            Assert.AreEqual(4, values.Distinct().Count());

            enumeration.Name = "high";
            Assert.AreEqual(Priority.High.ToString(), enumeration.Name);
            enumeration.Name = "void";
            Assert.AreEqual(Priority.Void, enumeration.Value);
            Assert.AreEqual((byte)Priority.Void, (byte)enumeration.Value);
        }
    }
}
