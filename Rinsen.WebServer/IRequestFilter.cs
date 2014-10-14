using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer
{
    public interface IRequestFilter
    {
        void FilterRequest(RequestContext request, ResponseContext response);
    }
}
