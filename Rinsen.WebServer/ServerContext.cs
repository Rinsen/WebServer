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
            BufferSize = 2048;
            MaxClientHeaderSize = 8192;
        }

        public string FileServerBasePath { get; set; }

        public Assembly Assembly { get; private set; }

        public RouteTable RouteTable { get; private set; }

        public int ServerListeningPort { get; set; }

        public int MaxClientHeaderSize { get; private set; }
              
        public int BufferSize { get; private set; }

        public IFileAndDirectoryService FileAndDirectoryService { get; internal set; }

        public IRequestFilter RequestFilter { get; internal set; }

        public IResponseFilter ResponseFilter { get; internal set; }

        public IExceptionHandler ExceptionHandler { get; internal set; }

        public bool HasFileAndDirectoryService { get { return FileAndDirectoryService != null; } }

        public string DefaultMethodName { get; set; }

        public string HostName { get; set; }
		
		internal bool ThreadedResponses { get; set; }
    }
}
