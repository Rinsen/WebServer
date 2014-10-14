using System;
using Microsoft.SPOT;
using System.Reflection;
using System.Collections;

namespace Rinsen.WebServer.Routing
{
    internal class RouteGenerator
    {
        internal ArrayList GetRoutes(Assembly assembly)
        {
            var routes = new ArrayList();
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Controller)))
                {
                    routes.Add(new Route(type, GetControllerMethodName(type.Name), type.Name, type.FullName, GetMethods(type)));
                }
            }
            return routes;
        }

        private string GetControllerMethodName(string controllerName)
        {
            if (controllerName.IndexOf("Controller") > 0)
            {
                return controllerName.Substring(0, controllerName.IndexOf("Controller"));
            }
            throw new Exception("Controller name not supported " + controllerName + ", should end with Controller.");
        }

        private ArrayList GetMethods(Type type)
        {
            var methods = new ArrayList();
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                methods.Add(method.Name);
            }
            return methods;
        }

    }
}
