using Rinsen.WebServer.Http;
using System.IO;
using System.Net.Sockets;

using System.Collections;
using Microsoft.SPOT;

namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public class FileAndDirectoryService : IFileAndDirectoryService
    {
        private static ISDCardManager SDCardManager { get; set; }

        public void SetFileNameAndPathIfFileExists(ServerContext serverContext, HttpContext httpContext)
        {
            var contentType = httpContext.Response.ContentType;
            var fileFullName = serverContext.FileServerBasePath + httpContext.Request.Uri.LocalPath;

            if (fileFullName.ToUpper().IndexOf(".HTM") != -1 || fileFullName.ToUpper().IndexOf(".HTML") != -1)
            {
                contentType = new ContentType { MainContentType = EnumMainContentType.Text, SubContentType = EnumSubContentType.Html };
            }
            else if (fileFullName.ToUpper().IndexOf(".CSS") != -1)
            {
                contentType = new ContentType { MainContentType = EnumMainContentType.Text, SubContentType = EnumSubContentType.Css };
            }
            else if (fileFullName.ToUpper().IndexOf(".TXT") != -1)
            {
                contentType = new ContentType { MainContentType = EnumMainContentType.Text, SubContentType = EnumSubContentType.Plain };
            }
            else if (fileFullName.ToUpper().IndexOf(".JPG") != -1 ||
                fileFullName.ToUpper().IndexOf(".BMP") != -1 ||
                fileFullName.ToUpper().IndexOf(".JPEG") != -1)
            {
                contentType = new ContentType { MainContentType = EnumMainContentType.Image, SubContentType = EnumSubContentType.Jpeg };
            }
            else if (fileFullName.ToUpper().IndexOf(".JS") != -1)
            {
                //note this was text/javascript; I updated because it was obsoleted in favor of application/javascript
                contentType = new ContentType { MainContentType = EnumMainContentType.Application, SubContentType = EnumSubContentType.JavaScript };
            }

            if (contentType != null)
            {
                Debug.Print("Set Content Type: " + contentType);
                httpContext.Response.ContentType = contentType;
            }
            else
            {
                Debug.Print("No Matching Content Type set... " + contentType);
                return;
            }

            if (File.Exists(fileFullName))
            {
                var files = new DirectoryInfo(fileFullName.Substring(0, fileFullName.LastIndexOf('\\'))).GetFiles();
                foreach (var file in files)
                {
                    if (file.FullName == fileFullName)
                    {
                        httpContext.Response.FileLength = file.Length.ToString();
                    }
                }
                httpContext.Response.FileFullName = fileFullName;
                httpContext.Response.HttpStatusCode = new Ok();
            }
        }

        public void SendFile(ServerContext serverContext, HttpContext httpContext)
        {
            SDCardManager.SendFile(httpContext.Response.FileFullName, httpContext.Socket);
        }

        public bool TryGetDirectoryResultIfDirectoryExists(ServerContext serverContext, HttpContext httpContext)
        {
            var directoryPath = serverContext.FileServerBasePath + httpContext.Request.Uri.LocalPath;

            DirectoryInfo rootDirectory = new DirectoryInfo(directoryPath);
            if (rootDirectory.Exists)
            {
                DirectoryInfo[] directories = rootDirectory.GetDirectories();
                FileInfo[] files = rootDirectory.GetFiles();
                httpContext.Response.Data = new DirectoryListBuilder().GenerateSimpleDirectoryList(httpContext.Request.Uri.RawPath, directories, files, serverContext.HostName);
                httpContext.Response.ContentType = new ContentType { MainContentType = EnumMainContentType.Text, SubContentType = EnumSubContentType.Html };
                return true;
            }

            return false;
        }

        public string GetFileServiceBasePath()
        {
            string basePath = string.Empty;

            Debug.Print("Setting File Services Base Path..." + "\r\nHas an SDCard Manager: " + HasSDCardManager);
            if (HasSDCardManager)
            {
                basePath = SDCardManager.GetWorkingDirectoryPath();
                Debug.Print("Base path is: " + basePath);
                return basePath;
            }

            basePath = "\\SD\\WWW";
            var directory = new DirectoryInfo(basePath);
            if (directory.Exists)
            {
                Microsoft.SPOT.Debug.Print("Base path is: " + basePath);
                return basePath;
            }

            basePath = "\\WINFS\\WWW";
            directory = new DirectoryInfo(basePath);
            if (directory.Exists)
            {
                Debug.Print("Base path is: " + basePath);
                return basePath;
            }
            Debug.Print("No Base Path Set...");
            return string.Empty;
        }

        public void SetSDCardManager(ISDCardManager sdCardManager)
        {
            SDCardManager = sdCardManager;
        }

        public bool HasSDCardManager { get { return SDCardManager != null; } }
    }
}
