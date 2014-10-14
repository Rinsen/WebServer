using Rinsen.WebServer.Http;
using System.IO;
using System.Net.Sockets;
namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public class FileAndDirectoryService : IFileAndDirectoryService
    {
        public void SetFileNameAndPathIfFileExists(ServerContext serverContext, HttpContext httpContext)
        {
            var fileFullName = serverContext.FileServerBasePath + httpContext.Request.Uri.LocalPath;

            string contentType = string.Empty;

            if (fileFullName.IndexOf(".htm") != -1 || fileFullName.IndexOf(".HTM") != -1 || fileFullName.IndexOf(".html") != -1 || fileFullName.IndexOf(".HTML") != -1)
            {
                contentType = "text/html";
            }
            else if (fileFullName.IndexOf(".css") != -1 || fileFullName.IndexOf(".CSS") != -1)
            {
                contentType = "text/css";
            }
            else if (fileFullName.IndexOf(".txt") != -1 || fileFullName.IndexOf(".TXT") != -1)
            {
                contentType = "text/plain";
            }
            else if (fileFullName.IndexOf(".jpg") != -1 || fileFullName.IndexOf(".JPG") != -1 ||
                fileFullName.IndexOf(".bmp") != -1 || fileFullName.IndexOf(".BMP") != -1 ||
                fileFullName.IndexOf(".jpeg") != -1 || fileFullName.IndexOf(".JPEG") != -1)
            {
                contentType = "image/jpeg";
            }
            else if (fileFullName.IndexOf(".js") != -1 || fileFullName.IndexOf(".JS") != -1)
            {
                contentType = "text/javascript";
            }

            if (contentType != string.Empty)
            {
                httpContext.Response.ContentType = contentType;
            }
            else
            {
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

        public void SendFile(ServerContext serverContext, Socket clientSocket, HttpContext httpContext)
        {
            using (var fileStream = new FileStream(httpContext.Response.FileFullName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var fileLength = fileStream.Length;
                byte[] buf = new byte[2048];
                for (long bytesSent = 0; bytesSent < fileLength; )
                {
                    // Determines amount of data left.
                    long bytesToRead = fileLength - bytesSent;
                    bytesToRead = bytesToRead < 2048 ? bytesToRead : 2048;
                    // Reads the data.
                    fileStream.Read(buf, 0, (int)bytesToRead);
                    // Writes data to browser
                    clientSocket.Send(buf, 0, (int)bytesToRead, SocketFlags.None);

                    // Updates bytes read.
                    bytesSent += bytesToRead;
                }
            }
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
                httpContext.Response.ContentType = "text/html";
                return true;
            }

            return false;
        }


        public string GetFileServiceBasePath()
        {
            string basePath = "\\SD\\WWW";
            var directory = new DirectoryInfo(basePath);
            if (directory.Exists)
            {
                return basePath;
            }
            
            basePath = "\\WINFS\\WWW";
            directory = new DirectoryInfo(basePath);
            if (directory.Exists)
            {
                return basePath;
            }
            return string.Empty;
        }
    }
}
