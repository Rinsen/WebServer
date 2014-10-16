using System;
using Microsoft.SPOT;
using Rinsen.WebServer;

namespace DemoWeb
{
    public class RequestFilter : IRequestFilter
    {
        public void FilterRequest(RequestContext request, ResponseContext response)
        {
            if (request.Uri.LocalPath == "\\DontLetMeIn")
            {
                response.HttpStatusCode = new Forbidden();
                response.Data = "Forbidden - 403";
            }
        }
    }
}
