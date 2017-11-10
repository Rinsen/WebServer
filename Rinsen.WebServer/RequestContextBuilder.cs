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
            var initialLine = string.Empty;

            // much faster to read from network in one shot
            var httpHeader = string.Empty;
            while(socket.Available > 0)
            {
                socket.Receive(buffer);
                httpHeader = httpHeader + new String(Encoding.UTF8.GetChars(buffer));
            }

            int count = 0;
            var lines = httpHeader.Split('\n');
            foreach(var item in lines)
            {
                var header = item.Trim(); // remove the \r
                if (header == string.Empty) continue;

                headerSize += header.Length;
                if (headerSize > _serverContext.MaxClientHeaderSize)
                {
                    throw new EntityToLargeException("Header entity is to large (HTTP413)");
                }

                if (count == 0)
                {
                    initialLine = header;
                }
                else
                {
                    requestContext.SetHeader(header);
                }
                count++;
            }

            // moved this to the end because "Host" may not be set before constructing Uri.
            requestContext.SetRequestLine(initialLine);
        }
    }
}
