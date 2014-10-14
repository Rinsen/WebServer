using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer
{
    public class HttpContext
    {
        public HttpContext(RequestContext requestContext, ResponseContext responseContext)
        {
            Request = requestContext;
            Response = responseContext;
        }

        public RequestContext Request { get; private set; }

        public ResponseContext Response { get; private set; }

    }
}
