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

        public RequestResponseHandler(HttpContextBuilder httpContextBuilder, HttpApplicationHandler httpApplicationHandler, ResponseHandler responseHandler, ServerContext serverContext)
        {
            _httpContextBuilder = httpContextBuilder;
            _httpApplicationHandler = httpApplicationHandler;
            _responseHandler = responseHandler;
            _serverContext = serverContext;

        }

        internal void ProcessRequest(Socket clientSocket)
        {
            try
            {
                // Get Request information
                var httpContext = CreateHttpContext(clientSocket);

                if (_serverContext.RequestFilter != null)
	            {
                    _serverContext.RequestFilter.FilterRequest(httpContext.Request, httpContext.Response);		 
	            }
                
                // Create reponse
                if (httpContext.Response.HttpStatusCode == null)
                {
                    CreateResponse(httpContext);
                }

                // IResponseFilter ?
                if (_serverContext.ResponseFilter != null)
                {
                    _serverContext.ResponseFilter.FilterResponse(httpContext.Response, httpContext.Request);
                }

                // Send response
                SendResponse(clientSocket, httpContext);

            }
            catch (Exception e)
            {
                try
                {
                    _responseHandler.SendInternalServerError(clientSocket, e);
                }
                catch (Exception innerException)
                {
                    throw new Exception("Error in error handler ", innerException);
                }
                throw; // Re throw error to error handler after trying to send to client
            }
        }

        private HttpContext CreateHttpContext(Socket clientSocket)
        {
            return _httpContextBuilder.CreateInitialContext(clientSocket);
        }

        private void CreateResponse(HttpContext httpContext)
        {
            RequestRoute resultRoute;
            // If request is of type file
            if(httpContext.Request.Uri.IsFile)
            {
                if (_serverContext.HasFileAndDirectoryService)
                    _serverContext.FileAndDirectoryService.SetFileNameAndPathIfFileExists(_serverContext, httpContext);
            }
            // If request is to type application
            else if (_serverContext.RouteTable.HasRouteToPath(httpContext.Request.Uri.AbsolutePath, out resultRoute))
            {
                httpContext.Request.RequestedRoute = resultRoute;
                _httpApplicationHandler.Execute(httpContext);
                httpContext.Response.HttpStatusCode = new Ok();
            }
            // If request is to browse file system
            else if (_serverContext.HasFileAndDirectoryService)
            {
                if (_serverContext.FileAndDirectoryService.TryGetDirectoryResultIfDirectoryExists(_serverContext, httpContext))
                    httpContext.Response.HttpStatusCode = new Ok();
            }

            // If nothing is found
            if (httpContext.Response.HttpStatusCode == null)
            {
                httpContext.Response.HttpStatusCode = new NotFound();
                httpContext.Response.Data = "Resource not found";
            }
        }

        private void SendResponse(Socket clientSocket, HttpContext httpContext)
        {
            _responseHandler.SendResponse(clientSocket, httpContext);
        }
    }
}
