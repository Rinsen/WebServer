using Fredde.Web.MicroFramework;
using Rinsen.WebServer.Exceptions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Rinsen.WebServer
{
    class RequestContextBuilder
    {
        private readonly ServerContext _serverContext;

        public RequestContextBuilder(ServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        internal void ProcessRequest(Socket clientSocket, RequestContext requestContext)
        {
            requestContext.IpEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;

            var bufferString = GetDataFromSocket(clientSocket);

            var messageHeaderLength = bufferString.IndexOf("\r\n\r\n");

            RemoveHeaderFromSocketBuffer(clientSocket, messageHeaderLength);

            if (messageHeaderLength == -1)
                throw new RequestClientSocketException("Unknown error in HTTP request");

            var requestLineLength = bufferString.IndexOf("\r\n");

            var requestLine = bufferString.Substring(0, requestLineLength);

            var headers = Regex.Split("\r\n", bufferString.Substring(requestLineLength + 2, messageHeaderLength - requestLineLength - 2), RegexOptions.None);

            requestContext.SetHeaders(headers);

            requestContext.SetRequestLineAndUri(requestLine);
        }

        private void RemoveHeaderFromSocketBuffer(Socket clientSocket, int messageHeaderLength)
        {
            messageHeaderLength = messageHeaderLength + 4;
            byte[] buffer = new byte[messageHeaderLength];
            clientSocket.Receive(buffer, messageHeaderLength, SocketFlags.None);
        }

        private string GetDataFromSocket(Socket clientSocket)
        {
            var available = clientSocket.Available;
            var bytesToRecive = available < _serverContext.MaxBufferSize ? available : _serverContext.MaxBufferSize;

            if (bytesToRecive == 0)
                throw new Exception("No data in socket");

            byte[] buffer = new byte[bytesToRecive];
            var bytesRead = clientSocket.Receive(buffer, bytesToRecive, SocketFlags.Peek);
            var bufferString = new String(Encoding.UTF8.GetChars(buffer));
            return bufferString;
        }
    }
}
