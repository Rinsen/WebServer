using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Rinsen.WebServer.Extensions;
using Rinsen.WebServer.Exceptions;

namespace Rinsen.WebServer
{
    class RequestContextBuilder
    {
        private readonly ServerContext _serverContext;

        public RequestContextBuilder(ServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        internal void ProcessRequest(Socket socket, RequestContext requestContext)
        {
            requestContext.IpEndPoint = socket.RemoteEndPoint as IPEndPoint;

            GetHeaderPartsFromSocket(socket, requestContext);
        }

        private void GetHeaderPartsFromSocket(Socket socket, RequestContext requestContext)
        {
            var headerSize = 0;
            byte[] buffer = new byte[_serverContext.BufferSize];
            var requestLineSet = false;

            while (socket.Available > 0)
            {
                socket.ReceiveUntil(buffer, "\r\n");
                var headerString = new String(Encoding.UTF8.GetChars(buffer));

                // End if no more headers is discovered
                if (headerString == null)
                {
                    break;
                }

                headerSize += headerString.Length;
                // Check on sum of header size to try to avoid out of memory exceptions and other nasty things
                if (headerSize > _serverContext.MaxClientHeaderSize)
                {
                    throw new EntityToLargeException("Header entity is to large (HTTP413)");
                }

                if (!requestLineSet)
                {
                    if (headerString == null  || headerString == string.Empty)
                    {
                        throw new ArgumentNullException("Request does not contain any data");
                    }

                    requestLineSet = true;
                    requestContext.SetRequestLineAndUri(headerString);
                }
                else
                {
                    requestContext.SetHeader(headerString);
                }
            }
        }
    }
}
