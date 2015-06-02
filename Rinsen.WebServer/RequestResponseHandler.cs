using Microsoft.SPOT;
using Rinsen.WebServer.Http;
using Rinsen.WebServer.Routing;
using System;
using System.Net.Sockets;

namespace Rinsen.WebServer
{
    class RequestResponseHandler
    {
        private readonly HttpContextBuilder _httpContextBuilder;
        private readonly HttpApplicationHandler _httpApplicationHandler;
        private readonly ResponseHandler _responseHandler;
        
        private ServerContext _serverContext;
        private HttpContext _httpContext;

        public RequestResponseHandler(HttpContextBuilder httpContextBuilder, HttpApplicationHandler httpApplicationHandler, ResponseHandler responseHandler, ServerContext serverContext)
        {
            _httpContextBuilder = httpContextBuilder;
            _httpApplicationHandler = httpApplicationHandler;
            _responseHandler = responseHandler;
            _serverContext = serverContext;

        }

        internal void ProcessRequest()
        {
            try
            {
                // Get Request information
                _httpContext = CreateHttpContext();

                if (_serverContext.RequestFilter != null)
	            {
                    _serverContext.RequestFilter.FilterRequest(_httpContext.Request, _httpContext.Response);
	            }

                // Interrupt execution of this request if response is sent
                if (_httpContext.Response.IsSent)
                    return;

                // Create reponse
                if (_httpContext.Response.HttpStatusCode == null)
                {
                    CreateResponse();
                }

                // Interrupt execution of this request if response is sent
                if (_httpContext.Response.IsSent)
                    return;

                // IResponseFilter ?
                if (_serverContext.ResponseFilter != null)
                {
                    _serverContext.ResponseFilter.FilterResponse(_httpContext.Response, _httpContext.Request);
                }

                // Interrupt execution of this request if response is sent
                if (_httpContext.Response.IsSent)
                    return;

                // Send response
                SendResponse();

            }
            catch (Exception e)
            {
                try
                {
                    _responseHandler.SendInternalServerError(_httpContext.Socket, e);
                }
                catch (Exception innerException)
                {
                    throw new Exception("Error in error handler ", innerException);
                }
                throw; // Re throw error to error handler after trying to send to client
            }
        }

        private HttpContext CreateHttpContext()
        {
            return _httpContextBuilder.CreateInitialContext();
        }

        private void CreateResponse()
        {
            RequestRoute resultRoute;
            // If request is of type file
            if(_httpContext.Request.Uri.IsFile)
            {
                if (_serverContext.HasFileAndDirectoryService)
                    _serverContext.FileAndDirectoryService.SetFileNameAndPathIfFileExists(_serverContext, _httpContext);
            }
            // If request is to type application
            else if (_serverContext.RouteTable.HasRouteToPath(_httpContext.Request.Uri.AbsolutePath, out resultRoute))
            {
                _httpContext.Request.RequestedRoute = resultRoute;
                _httpApplicationHandler.Execute(_httpContext);
                _httpContext.Response.HttpStatusCode = new Ok();
            }
            // If request is to browse file system
            else if (_serverContext.HasFileAndDirectoryService)
            {
                if (_serverContext.FileAndDirectoryService.TryGetDirectoryResultIfDirectoryExists(_serverContext, _httpContext))
                    _httpContext.Response.HttpStatusCode = new Ok();
            }

            // If nothing is found
            if (_httpContext.Response.HttpStatusCode == null)
            {
                _httpContext.Response.HttpStatusCode = new NotFound();
                _httpContext.Response.Data = "Resource not found";
            }
        }

        private void SendResponse()
        {
            _responseHandler.SendResponse(_httpContext);
        }
    }
}
