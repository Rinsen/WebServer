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
            fileAndDirectoryService.SetSDCardManager(new SDCardManager(WORKINGDIRECTORY));
            webServer.SetFileAndDirectoryService(new FileAndDirectoryService());
            /*By not setting a default Controller, the root web directory (set with WORKINGDIRECTORY) will have it's contents listed 
             * with the FileAndDirectoryServer library */
            //webServer.RouteTable.DefaultControllerName = "Default"; 
            webServer.StartServer(80);
        }
    }
}