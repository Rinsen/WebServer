using Rinsen.WebServer.FileAndDirectoryServer;

namespace DemoWeb
{
    public class Program
    {
        public const string WORKINGDIRECTORY = @"\WWW";

        public static void Main()
        {
            // write your code here
            var webServer = new WebServer();
            webServer.AddRequestFilter(new RequestFilter());
            var fileAndDirectoryService = new FileAndDirectoryService();
            fileAndDirectoryService.SetSDCard(new SDCardManager(WORKINGDIRECTORY));
            webServer.SetFileAndDirectoryService(new FileAndDirectoryService());
            webServer.RouteTable.DefaultControllerName = "Default";
            webServer.StartServer();
        }
    }
}