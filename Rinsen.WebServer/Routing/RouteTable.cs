using System;
using Microsoft.SPOT;
using System.Collections;

namespace Rinsen.WebServer.Routing
{
    public class RouteTable
    {
        private readonly string _defaultMethodName;
        
        public RouteTable(string defaultMethodName)
        {
            _defaultMethodName = defaultMethodName;
            Routes = new ArrayList();
        }

        public ArrayList Routes { get; private set; }

        public bool HasRouteToPath(string path, out RequestRoute resultRoute)
        {
            var pathParts = path.TrimStart('/').Split('/');

            if (!(pathParts.Length > 0))
            {
                resultRoute = null;
                return false;
            }

            var candidateControllerName = pathParts[0];
	        
            foreach (Route route in Routes)
            {
                if (route.ControllerName == candidateControllerName)
                {
                    string methodName;
                    if (!TryFindMethod(route, pathParts, out methodName))
                        break;

                    resultRoute = new RequestRoute(route, methodName);
                    return true;
                }
            }

            resultRoute = null;
            return false;
        }

        private bool TryFindMethod(Route route, string[] pathParts, out string methodName)
        {
            string defaultMethod = string.Empty;
            if (pathParts.Length > 1)
            {
                foreach (string method in route.Methods)
                {
                    if (pathParts[1] == method)
                    {
                        methodName = method;
                        return true;
                    }
                    else if (method == _defaultMethodName)
                    {
                        defaultMethod = method;
                    }
                }
                if (defaultMethod != string.Empty)
                {
                    methodName = defaultMethod;
                    return true;
                }
            }
            else if(pathParts.Length == 1)
            {
                foreach (string method in route.Methods)
                {
                    if (method == _defaultMethodName)
                    {
                        methodName = method;
                        return true;
                    }
                }
            }
            methodName = string.Empty;
            return false;
        }
    }
}
