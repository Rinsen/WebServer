using System;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    internal class RequestHandlerFactory
    {
        private readonly ServerContext _serverContext;

        public RequestHandlerFactory(ServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        internal RequestResponseHandler Create(Socket socket)
        {
            var httpContext = new HttpContext(new RequestContext(_serverContext), new ResponseContext(), socket);
            
            var requestContextBuilder = new RequestContextBuilder(_serverContext);
            var responseContextBuilder = new ResponseContextBuilder();
            var httpApplicationHandler = new HttpApplicationHandler();
            var responseHandler = new ResponseHandler(_serverContext, new ResponseHeaderBuilder());

            var httpContextBuilder = new HttpContextBuilder(httpContext, requestContextBuilder, responseContextBuilder, _serverContext);

            return new RequestResponseHandler(httpContextBuilder, httpApplicationHandler, responseHandler, _serverContext);
        }
    }
}
