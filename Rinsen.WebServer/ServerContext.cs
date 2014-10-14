using System;
using Microsoft.SPOT;
using System.Reflection;
using Rinsen.WebServer.Routing;

namespace Rinsen.WebServer
{
    public class ServerContext
    {
        public ServerContext(Assembly assembly, RouteTable routeTable)
        {
            Assembly = assembly;
            RouteTable = routeTable;
            MaxBufferSize = 2048;
        }

        public string FileServerBasePath { get; set; }

        public Assembly Assembly { get; private set; }

        public RouteTable RouteTable { get; private set; }

        public int ServerListeningPort { get; set; }
              
        public int MaxBufferSize { get; private set; }

        public IFileAndDirectoryService FileAndDirectoryService { get; internal set; }

        public IRequestFilter RequestFilter { get; internal set; }

        public IResponseFilter ResponseFilter { get; internal set; }

        public IExceptionHandler ExceptionHandler { get; internal set; }

        public bool HasFileAndDirectoryService { get { return FileAndDirectoryService != null; } }

        public string DefaultMethodName { get; set; }

        public string HostName { get; set; }
    }
}