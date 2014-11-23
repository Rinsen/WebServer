using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Rinsen.WebServer
{
    class ResponseHandler
    {
        private readonly ServerContext _serverContext;
        private readonly ResponseHeaderBuilder _responseHeaderBuilder;

        public ResponseHandler(ServerContext serverContext, ResponseHeaderBuilder responseHeaderBuilder)
        {
            _serverContext = serverContext;
            _responseHeaderBuilder = responseHeaderBuilder;
        }

        internal void SendResponse(HttpContext httpContext)
        {
            var response = httpContext.Response;
            
            var responseLineAndHeaders = _responseHeaderBuilder.BuildResponseLineAndHeaders(response);

            if(httpContext.Socket.Poll(5000000, SelectMode.SelectWrite))
            {
                httpContext.Socket.Send(Encoding.UTF8.GetBytes(responseLineAndHeaders), responseLineAndHeaders.Length, SocketFlags.None);

                if (response.IsFile)
                {
                    _serverContext.FileAndDirectoryService.SendFile(_serverContext, httpContext);
                    return;
                }

                if (response.Data != null && response.Data != string.Empty)
                {
                    httpContext.Socket.Send(Encoding.UTF8.GetBytes(response.Data), response.Data.Length, SocketFlags.None);
                }
            }
        }

        internal void SendInternalServerError(Socket socket, System.Exception e)
        {
            var response = "Internal server error\r\n\r\nException\r\nMessage:\r\n" + e.Message + "\r\nStack Trace:\r\n" + e.StackTrace;
            var length = response.Length;
            string innerException = string.Empty;
            if (e.InnerException != null)
	        {
		        innerException = "\r\nInner Exception: \r\nMessage: " + e.InnerException.Message + "\r\nStack Trace: " + e.InnerException.StackTrace;
                length += innerException.Length;
	        }
            var header = "HTTP/1.1 500 Internal Server Error\r\nContent-Length: " + length + "\r\nConnection: close\r\n\r\n";
            if (socket.Poll(5000000, SelectMode.SelectWrite))
            {
                socket.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                socket.Send(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);
                if (innerException != string.Empty)
                {
                    socket.Send(Encoding.UTF8.GetBytes(innerException), innerException.Length, SocketFlags.None);
                }
            }
        }
    }
}
