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
                var clientSocket = serverSocket.Accept();
                ProcessClientRequest(clientSocket, true);
            }
        }

        internal void ProcessClientRequest(Socket clientSocket, Boolean asynchronously)
        {
            if (asynchronously)
                // Spawn a new thread to handle the request.
                new Thread(() => ProcessRequest(clientSocket)).Start();
            else ProcessRequest(clientSocket);
            //ProcessRequest(clientSocket);
        }

        public void ProcessRequest(Socket clientSocket)
        {
            using (clientSocket)
            {
                var requestHandler = _requestHandlerFactory.Create();
                if (clientSocket.Poll(5000000, SelectMode.SelectRead))
                {
                    try
                    {
                        requestHandler.ProcessRequest(clientSocket);
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
