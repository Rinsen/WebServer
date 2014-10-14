using System;
using Microsoft.SPOT;
using System.Collections;

namespace Rinsen.WebServer.Routing
{
    public class Route
    {
        public Route(Type controllerType, string controllerName, string controllerTypeName, string controllerAssemblyTypeName, ArrayList methods)
        {
            ControllerType = controllerType;
            ControllerName = controllerName;
            ControllerTypeName = controllerTypeName;
            ControllerAssemblyTypeName = controllerAssemblyTypeName;
            Methods = methods;
        }

        public Type ControllerType { get; private set; }

        public string ControllerName { get; private set; }

        public string ControllerTypeName { get; private set; }

        public string ControllerAssemblyTypeName { get; private set; }

        public ArrayList Methods { get; private set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return ControllerType == ((Route)obj).ControllerType;
        }
    }
}
