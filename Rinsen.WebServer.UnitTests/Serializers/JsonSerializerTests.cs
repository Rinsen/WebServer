using System.Collections;
using MFUnit;
using Rinsen.WebServer.Serializers;

namespace Rinsen.WebServer.UnitTests
{
    public class JsonSerializerUnitTests
    {
        #region ModelToJsonSerialize

        public class JsonSimpleObject
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public bool BoolValue { get; set; }
        }

        public class JsonComplexObject
        {
            public string MyName { get; set; }

            public int MyAge { get; set; }

            public ArrayList MyArray { get; set; }

            public JsonSimpleObject MySimpleObject { get; set; }
        }

        #endregion

        public void WhenSerializeWithSimpleObject_GetSerialiedJsonString()
        {
            // Arrange            
            var jsonSerializer = new JsonSerializer();
            var simpleObject = new JsonSimpleObject { IntValue = 5, StringValue = "Fredde" };

            //Act

            var result = jsonSerializer.Serialize(simpleObject);

            //Assert
            Assert.AreEqual("{\"intValue\": 5,\"stringValue\": \"Fredde\",\"boolValue\": false}", result);

        }

        public void WhenSerializeWithComplexObject_GetSerialiedJsonString()
        {
            // Arrange            
            var jsonSerializer = new JsonSerializer();

            var simpleObject = new JsonSimpleObject { IntValue = 5, StringValue = "Fredde" };
            var complexObject = new JsonComplexObject { MyName = "Fredde", MyAge = 30, MyArray = new ArrayList(), MySimpleObject = simpleObject };

            complexObject.MyArray.Add(5);
            complexObject.MyArray.Add(10);
            complexObject.MyArray.Add(100);

            //Act
            var result = jsonSerializer.Serialize(complexObject);

            //Assert
            Assert.AreEqual("{\"myName\": \"Fredde\",\"myAge\": 30,\"myArray\": [5, 10, 100],\"mySimpleObject\": {\"intValue\": 5,\"stringValue\": \"Fredde\",\"boolValue\": false}}", result);

        }

        public class JsonMoreDataTypes
        {
            public bool BoolProperty { get; set; }

            public byte ByteProperty { get; set; }

            public sbyte SByteProperty { get; set; }

            public char CharProperty { get; set; }

            public double DoubleProperty { get; set; }

            public float FloatProperty { get; set; }

            public int IntProperty { get; set; }

            public uint UIntProperty { get; set; }

            public long LongProperty { get; set; }

            public ulong ULongProperty { get; set; }

            public short ShortProperty { get; set; }

            public ushort UShortProperty { get; set; }

            public string StringProperty { get; set; }
            
        }

        public void WhenSerializeWithMoreDatatypes_GetSerialiedJsonString()
        {
            // Arrange
            var jsonSerializer = new JsonSerializer();
            var jsonMoreDataTypes = new JsonMoreDataTypes
            {
                BoolProperty = true,
                ByteProperty = 1,
                SByteProperty = 2,
                CharProperty = 'K',
                DoubleProperty = 12.52,
                FloatProperty = 12.52F,
                IntProperty = 15789,
                UIntProperty = 1234,
                LongProperty = 152,
                ULongProperty = 12345,
                ShortProperty = 123,
                UShortProperty = 555,
                StringProperty = "This is a string"
            };

            // Act
            var result = jsonSerializer.Serialize(jsonMoreDataTypes);

            // Assert
            Assert.AreEqual("{\"boolProperty\": true,\"byteProperty\": 1,\"sByteProperty\": 2,\"charProperty\": \"K\",\"doubleProperty\": 12.52,\"floatProperty\": 12.5200005,\"intProperty\": 15789,\"uIntProperty\": 1234,\"longProperty\": 152,\"uLongProperty\": 12345,\"shortProperty\": 123,\"uShortProperty\": 555,\"stringProperty\": \"This is a string\"}", result);

        }

        public void WhenDeSerializeWithSimpleJsonObject_GetDeSerializedObject()
        {
            // Arrange            
            var jsonSerializer = new JsonSerializer();
            var jsonString = "{\"intValue\": 5,\"stringValue\": \"Fredde\",\"boolValue\": False}";

            var simpleObject = new JsonSimpleObject { IntValue = 5, StringValue = "Fredde" };

            //Act

            var result = jsonSerializer.DeSerialize(jsonString, typeof(JsonSimpleObject));

            //Assert
            //Assert.AreEqual(, result);

        }
    }
}
