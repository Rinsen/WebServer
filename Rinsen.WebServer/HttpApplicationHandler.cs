using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Serializers;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    class HttpApplicationHandler
    {
        internal void Execute(HttpContext httpContext)
        {
            var requestedRoute = httpContext.Request.RequestedRoute;
            var method = requestedRoute.Route.ControllerType.GetMethod(requestedRoute.MethodName);

            var instance = (Controller)requestedRoute.Route.ControllerType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            instance.InitializeController(httpContext, new JsonSerializer(), new ModelFactory());

            method.Invoke(instance, null);
        }
    }
}
