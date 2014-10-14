using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Routing;
using System.Collections;
using MFUnit;

namespace Rinsen.WebServer.UnitTests.Routing
{
    public class RouteTableTests
    {

        public void WhenNoRouteInRouteTable_GetFalseAndNullRoute()
        {
            // Arrange
            var routeTable = new RouteTable("");

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("Name", out route);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(route);
        }

        public void WhenRouteInRouteTable_GetTrueAndSetRouteInRequest()
        {
            // Arrange
            var routeTable = new RouteTable("");
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("/Mock/Index", out route);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(mockControllerType, route.Route.ControllerType);
        }
    }
}
