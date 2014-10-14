using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer
{
    public class ModelFactory
    {
        public object CreateModel(Type type)
        {
            return type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}