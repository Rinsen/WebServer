using System;
using Microsoft.SPOT;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    public interface IFileAndDirectoryService
    {
        void SetFileNameAndPathIfFileExists(ServerContext serverContext, HttpContext httpContext);

        void SendFile(ServerContext serverContext, HttpContext httpContext);

        bool TryGetDirectoryResultIfDirectoryExists(ServerContext serverContext, HttpContext httpContext);

        string GetFileServiceBasePath();
    }
}
