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
            var routeTable = new RouteTable();

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("Name", out route);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(route);
        }

        public void WhenNoPathAndNoDefaultController_GetFalseAndNullRoute()
        {
            // Arrange
            var routeTable = new RouteTable();
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("/", out route);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(route);
        }

        public void WhenPathWithNoIdentifierAndNoDefaultController_GetFalseAndNullRoute()
        {
            // Arrange
            var routeTable = new RouteTable();
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("//", out route);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(route);
        }

        public void WhenRouteInRouteTable_GetTrueAndSetRouteInRequest()
        {
            // Arrange
            var routeTable = new RouteTable();
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

        public void WhenNoPathAndDefaultController_GetRouteToDefaultController()
        {
            // Arrange
            var defaultControllerName = "Mock";
            var routeTable = new RouteTable();
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));
            routeTable.DefaultControllerName = defaultControllerName;

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("/", out route);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(defaultControllerName, route.Route.ControllerName);
        }

        public void WhenRouteInRouteTableAndNoSpecificMethod_GetTrueAndSetRouteInRequestWithDefaultMethod()
        {
            // Arrange
            var routeTable = new RouteTable();
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("/Mock", out route);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(mockControllerType, route.Route.ControllerType);
            Assert.AreEqual("Index", route.MethodName);
        }

        public void WhenRouteInRouteTableAndNoExistingMethod_GetFalseAndNoRoute()
        {
            // Arrange
            var routeTable = new RouteTable();
            var mockControllerType = typeof(MockController);
            var methods = new ArrayList();
            methods.Add("Index");
            routeTable.Routes.Add(new Route(mockControllerType, "Mock", mockControllerType.Name, mockControllerType.FullName, methods));

            // Act
            RequestRoute route;
            var result = routeTable.HasRouteToPath("/Mock/MyNotExistingMethod", out route);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(route);
        }
    }
}
