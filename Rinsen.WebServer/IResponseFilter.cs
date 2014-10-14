using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer
{
    public interface IResponseFilter
    {
        void FilterResponse(ResponseContext response, RequestContext request);
    }
}
