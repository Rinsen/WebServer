using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Threading;

namespace Rinsen.WebServer
{
    internal class SocketListener
    {
        private RequestHandlerFactory _requestHandlerFactory;
        private ServerContext _serverContext;

        public SocketListener(RequestHandlerFactory requestHandlerFactory, ServerContext serverContext)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _serverContext = serverContext;
        }

        public void StartListening(Socket serverSocket)
        {
            while (true)
            {
                var socket = serverSocket.Accept();
                ProcessClientRequest(socket, true);
            }
        }

        internal void ProcessClientRequest(Socket socket, Boolean asynchronously)
        {
            if (asynchronously)
                // Spawn a new thread to handle the request.
                new Thread(() => ProcessRequest(socket)).Start();
            else ProcessRequest(socket);
        }

        public void ProcessRequest(Socket socket)
        {
            using (socket)
            {
                var requestHandler = _requestHandlerFactory.Create(socket);
                if (socket.Poll(5000000, SelectMode.SelectRead))
                {
                    try
                    {
                        requestHandler.ProcessRequest();
                    }
                    catch (Exception e) // Catch all unhandled internal server exceptions
                    {
                        if (_serverContext.ExceptionHandler != null)
                        {
                            _serverContext.ExceptionHandler.HandleException(e);
                        }
                        else
                        {
                            Debug.Print("Unhandled exception in web server, Message: " + e.Message + ", StackTrace:  " + e.StackTrace);
                        }                        
                    }
                }
            }

        }
    }
}
