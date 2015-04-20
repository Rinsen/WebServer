WebServer
=========

A .Net Micro Framework Web Server targeting Netduino Plus 2 and NETMF version 4.3

Introduction
-----------

When you would like to build a rich Web API on a Netduino device for control of any hardware and much more.

Web address to this example
http://192.168.1.100/MyFirst

    public class Program
    {
        public static void Main()
        {
            var webServer = new WebServer();
            webServer.Start(80);
        }
    }

    public class MyFirstController : Controller
    {
        public void Index()
        {
            SetHtmlResult("<!DOCTYPE html><html><body><h1>My page</h1></html>");        
        }
    }

###Json return type support

{"id": 1,"name": "My name","status": True,"myList": [1, 2, 3],"myOtherClass": {"innerName": "My inner name","innerStatus":      False}}

    public class JsonController : Controller
    {
        public void Index()
        {
            var myClass = new MyClass
            {
                Id = 1,
                Name = "My name",
                Status = true,
                MyList = new ArrayList { 1, 2, 3 },
                MyOtherClass = new MyOtherClass 
                {
                    InnerName = "My inner name",
                    InnerStatus = false,
                }
            };
            SetJsonResult(myClass);
        }
    }

    public class MyClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }
        
        public ArrayList MyList { get; set; }

        public MyOtherClass MyOtherClass { get; set; } 
    }

    public class MyOtherClass
    {
        public string InnerName { get; set; }

        public bool InnerStatus { get; set; }
    }

###Default Controller and Method

To add a default controller and to change the default controller method, default method name is Index and default controller name is empty string (not defined)

    public class Program
    {
        public static void Main()
        {
            var webServer = new WebServer();
            webServer.RouteTable.DefaultControllerName = "Default";
            webServer.RouteTable.DefaultMethodName = "MyMethodName";
            webServer.StartServer();
        }
    }

#Getting Started
---------------

## Run Demo web
1. Download Rinsen/WebServer.
2. Set desired .Net Micro Framework Deployment type in DemoWeb
3. Build and Start
4. Locate Output - Debug in Visual Studio
5. Copy http address 
6. Run address in browser and add example name at the end http://192.168.1.x:8500/ Put example name here

### Examples
1. Blink
2. DataModelBinder
3. FormCollection
4. Exception
5. Exception/InnerException
6. Json
7. DontLetMeIn
 
## Unit tests
Unit tests is based on MFUnit, MFUnit is included via NuGet

https://github.com/ducas/MFUnit/
### Run unit tests
1. Set Rinsen.WebServer.UnitTests as StartUp Project
2. Set Deployment option to Emulator
3. Build and Run

