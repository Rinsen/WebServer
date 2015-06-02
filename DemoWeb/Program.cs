using Rinsen.WebServer.FileAndDirectoryServer;

namespace DemoWeb
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            var webServer = new WebServer();
            webServer.AddRequestFilter(new RequestFilter());
            webServer.SetFileAndDirectoryService(new FileAndDirectoryService());
            webServer.RouteTable.DefaultControllerName = "Default";
            webServer.StartServer();
        }
    }
}