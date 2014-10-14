using System;
using Microsoft.SPOT;
using Rinsen.WebServer;

namespace DemoWeb
{
    public class RequestFilter : IRequestFilter
    {
        public void FilterRequest(RequestContext request, ResponseContext response)
        {
            if (request.Uri.IsFile)
            {
                response.HttpStatusCode = new Forbidden();
            }
        }
    }
}
