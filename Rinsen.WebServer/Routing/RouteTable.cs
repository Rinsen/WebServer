using System;
using Microsoft.SPOT;
using System.Collections;
using Rinsen.WebServer.Extensions;

namespace Rinsen.WebServer.Routing
{
    public class RouteTable
    {
        public string DefaultMethodName { get; set; }
        public string DefaultControllerName { get; set; }
        public bool HasDefaultControllerName { get { return DefaultControllerName != string.Empty; } }

        public ArrayList Routes { get; private set; }

        public RouteTable()
        {
            DefaultMethodName = "Index";
            DefaultControllerName = string.Empty;
            Routes = new ArrayList();
        }

        public bool HasRouteToPath(string path, out RequestRoute resultRoute)
        {
            string candidateControllerName;
            var pathParts = path.TrimStart('/').Split('/');

            if (pathParts[0] != string.Empty)
            {
                candidateControllerName = pathParts[0];
            }
            else if (HasDefaultControllerName)
            {
                candidateControllerName = DefaultControllerName;
            }
            else
            {
                resultRoute = null;
                return false;
            }
            
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
            if (pathParts.Length > 1 && pathParts[1] != string.Empty)
            {
                foreach (string method in route.Methods)
                {
                    if (pathParts[1] == method)
                    {
                        methodName = method;
                        return true;
                    }
                }
            }
            else
            {
                foreach (string method in route.Methods)
                {
                    if (method == DefaultMethodName)
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
