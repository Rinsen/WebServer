using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Serializers;

namespace Rinsen.WebServer
{
    public interface IController
    {
        void InitializeController(HttpContext httpContext, IJsonSerializer jsonSerializer, ModelFactory modelfactory);
    }
}
