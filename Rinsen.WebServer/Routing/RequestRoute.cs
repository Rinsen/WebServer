using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Routing
{
    public class RequestRoute
    {
        public RequestRoute(Route route, string methodName)
        {
            Route = route;
            MethodName = methodName;
        }

        public Route Route { get; set; }

        public string MethodName { get; set; }
    }
}
