using System;
using System.Net;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    class HttpContextBuilder
    {
        private readonly HttpContext _httpContext;
        private readonly RequestContextBuilder _requestContextBuilder;
        private readonly ResponseContextBuilder _responseContextBuilder;
        private readonly ServerContext _serverContext;

        public HttpContextBuilder(HttpContext httpContext, RequestContextBuilder requestContextBuilder, ResponseContextBuilder responseContextBuilder, ServerContext serverContext)
        {
            _httpContext = httpContext;
            _requestContextBuilder = requestContextBuilder;
            _responseContextBuilder = responseContextBuilder;
            _serverContext = serverContext;
        }

        internal HttpContext CreateInitialContext()
        {
            _requestContextBuilder.ProcessRequest(_httpContext.Socket, _httpContext.Request);

            _responseContextBuilder.BuildResponseContextBase(_httpContext.Response, _httpContext.Request);

            return _httpContext;
        }
    }
}
