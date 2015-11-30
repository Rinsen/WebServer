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

            //var RootDirectory = "\\SD";
            //var RequiredDirectory = RootDirectory + "\\WWW";

            //DirectoryInfo objDirectoryInfo = new DirectoryInfo(RootDirectory);
            //Debug.Print("Current Directories...");
            //foreach (var objDir in objDirectoryInfo.GetDirectories())
            //    Debug.Print(objDir.FullName);

            //Debug.Print("Creating Directory  " + RequiredDirectory + "...");
            //Directory.CreateDirectory(RequiredDirectory);

            //Debug.Print("Now Current Directories...");
            //foreach (var objDir in objDirectoryInfo.GetDirectories())
            //    Debug.Print(objDir.FullName);
        }
    }
}