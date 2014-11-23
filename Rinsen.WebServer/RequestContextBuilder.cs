using Rinsen.WebServer.Exceptions;
using Rinsen.WebServer.Extensions;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            var headerParts = GetHeaderPartsFromSocket(socket);

            requestContext.SetHeaders(headerParts);

            requestContext.SetRequestLineAndUri((string)headerParts[0]);
        }

        private ArrayList GetHeaderPartsFromSocket(Socket socket)
        {
            var headerStringRows = new ArrayList();
            var headerSize = 0;
            byte[] buffer = new byte[_serverContext.BufferSize];
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
                headerStringRows.Add(headerString);
            }
            return headerStringRows;
        }
    }
}
