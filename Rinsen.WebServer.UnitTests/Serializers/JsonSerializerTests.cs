using System;
using Microsoft.SPOT;
using System.Collections;
using MFUnit;
using Rinsen.WebServer;
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
            Assert.AreEqual("{\"intValue\": 5,\"stringValue\": \"Fredde\",\"boolValue\": False}", result);

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
            Assert.AreEqual("{\"myName\": \"Fredde\",\"myAge\": 30,\"myArray\": [5, 10, 100],\"mySimpleObject\": {\"intValue\": 5,\"stringValue\": \"Fredde\",\"boolValue\": False}}", result);

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
