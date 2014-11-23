using System;
using Microsoft.SPOT;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    public class HttpContext
    {
        public HttpContext(RequestContext requestContext, ResponseContext responseContext, Socket socket)
        {
            Request = requestContext;
            Response = responseContext;
            Socket = socket;
        }

        public RequestContext Request { get; private set; }

        public ResponseContext Response { get; private set; }

        public Socket Socket { get; private set; }

    }
}
