using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Serializers
{
    public interface IJsonSerializer
    {
        string Serialize(object objectToSerialize);

        object DeSerialize(string jsonObject, Type type);
    }
}
