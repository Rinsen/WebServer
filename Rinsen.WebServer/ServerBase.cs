using System;
using Microsoft.SPOT;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using Rinsen.WebServer.Routing;
using Rinsen.WebServer.Extensions;

namespace Rinsen.WebServer
{
    public abstract class ServerBase
    {
        private Thread _thread;
        private readonly RouteTable _routeTable;
        private readonly ServerContext _serverContext;
        private readonly RouteGenerator _routeGenerator;
        private IExceptionHandler _exceptionHandler;
		
		public bool ThreadedResponses {
			get
			{
				return _serverContext.ThreadedResponses;
			}
			set
			{
                _serverContext.ThreadedResponses = value;
			}
		}

        public ServerBase()
        {
            _routeGenerator = new RouteGenerator();
            _routeTable = new RouteTable("Index");
            _serverContext = new ServerContext(GetType().Assembly, _routeTable);
        }

        public void StartServer(int listeningPort = 8500)
        {
            var routes = _routeGenerator.GetRoutes(_serverContext.Assembly);
            _serverContext.ServerListeningPort = listeningPort;

            _serverContext.RouteTable.Routes.Append(routes);
            _thread = new Thread(() => MyThreadStart(_serverContext));
            _thread.Start();
        }

        private void MyThreadStart(ServerContext serverContext)
        {
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localEndPoint = new IPEndPoint(new NetworkInterfaceLocator().Locate(), serverContext.ServerListeningPort);
            Debug.Print("Local endpoint, IP: " + localEndPoint.Address + " Port: " + localEndPoint.Port);
            Debug.Print("http://" + localEndPoint.Address + ":" + localEndPoint.Port + "/");
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(2);
            
            var socketListener = new SocketListener(new RequestHandlerFactory(serverContext), _serverContext);
            socketListener.StartListening(serverSocket);
        }

        public void AddExceptionHandler(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public void AddRequestFilter(IRequestFilter requestFilter)
        {
            _serverContext.RequestFilter = requestFilter;
        }

        public void AddResponseFilter(IResponseFilter responseFilter)
        {
            _serverContext.ResponseFilter = responseFilter;
        }

        public void SetFileAndDirectoryService(IFileAndDirectoryService fileAndDirectoryService)
        {
            _serverContext.FileAndDirectoryService = fileAndDirectoryService;
            _serverContext.FileServerBasePath = fileAndDirectoryService.GetFileServiceBasePath();
        }
    }
}
