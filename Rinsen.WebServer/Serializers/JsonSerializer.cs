using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Rinsen.WebServer.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        public virtual string Serialize(object obj)
        {
            var jsonStringBuilder = new StringBuilder("{");
            var properties = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var first = true;
            foreach (var property in properties)
            {
                // Skip if no return value                
                if (property.ReturnType == typeof(void))
                    continue;

                var propertyName = property.Name.Substring(4);
                propertyName = propertyName[0].ToLower() + propertyName.Substring(1);
                if (first)
                {
                    jsonStringBuilder.Append("\"" + propertyName + "\": ");
                    first = false;
                }
                else
                {
                    jsonStringBuilder.Append(",\"" + propertyName + "\": ");
                }

                var value = property.Invoke(obj, new object[] { });

                if (!AppendValueToJsonBuilder(value, jsonStringBuilder))
                {
                    // If unknown object in value try to serialize object content and add as value
                    jsonStringBuilder.Append(Serialize(value)); 
                }
            }
            jsonStringBuilder.Append("}");

            return jsonStringBuilder.ToString();
        }

        bool AppendValueToJsonBuilder(object value, StringBuilder jsonStringBuilder)
        {
            var type = value.GetType();

            if (type == typeof(ArrayList))
            {
                SerializeArray((ArrayList)value, jsonStringBuilder);
                return true;
            }
            if (type == typeof(bool))
            {
                jsonStringBuilder.Append(value.ToString().ToLower());
                return true;
            }
            if (type == typeof(byte))
            {
                jsonStringBuilder.Append((byte)value);
                return true;
            }
            if (type == typeof(sbyte))
            {
                jsonStringBuilder.Append((sbyte)value);
                return true;
            }
            if (type == typeof(char))
            {
                jsonStringBuilder.Append("\"" + (char)value + "\"");
                return true;
            }
            if (type == typeof(double))
            {
                jsonStringBuilder.Append((double)value);
                return true;
            }
            if (type == typeof(float))
            {
                jsonStringBuilder.Append((float)value);
                return true;
            }
            if (type == typeof(int))
            {
                jsonStringBuilder.Append((int)value);
                return true;
            }
            if (type == typeof(uint))
            {
                jsonStringBuilder.Append((uint)value);
                return true;
            }
            if (type == typeof(long))
            {
                jsonStringBuilder.Append((long)value);
                return true;
            }
            if (type == typeof(ulong))
            {
                jsonStringBuilder.Append((ulong)value);
                return true;
            }
            if (type == typeof(short))
            {
                jsonStringBuilder.Append((short)value);
                return true;
            }
            if (type == typeof(ushort))
            {
                jsonStringBuilder.Append((ushort)value);
                return true;
            }
            if (type == typeof(string))
            {
                jsonStringBuilder.Append("\"" + (string)value + "\"");
                return true;
            }

            return false;
        }

        void SerializeArray(ArrayList valueArray, StringBuilder jsonStringBuilder)
        {
            if (valueArray.Count == 0)
                return;

            jsonStringBuilder.Append("[");
            var first = true;
            foreach (var value in valueArray)
            {
                if (!first)
                    jsonStringBuilder.Append(", ");
                else
                    first = false;

                if (!AppendValueToJsonBuilder(value, jsonStringBuilder))
                {
                    // If unknown object in value try to serialize object content and add as value
                    jsonStringBuilder.Append(Serialize(value));
                }
            }
            jsonStringBuilder.Append("]");
        }


        public object DeSerialize(string jsonObject, Type type)
        {
            var propertyInfoList = GetPropertyInfoList(type);

            var instance = type.GetConstructor(new Type[] { }).Invoke(new object[] { });

            var objectStart = jsonObject.IndexOf('{');

            throw new NotImplementedException();
        }

        ArrayList GetPropertyInfoList(Type type)
        {
            var properties = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            var propertyInfoList = new ArrayList();
            foreach (var property in properties)
            {
                // Skip if is a get property
                if (property.DeclaringType != typeof(void))
                    continue;

                propertyInfoList.Add(new PropertyInfo
                {
                    Name = property.Name.Substring(4).ToLower(),
                    Property = property
                });
            }


            return propertyInfoList;
        }
    }

    public class PropertyInfo
    {
        public string Name { get; set; }

        public Type Type { get { return Property.DeclaringType; } }

        public MethodInfo Property { get; set; }
    }
}
