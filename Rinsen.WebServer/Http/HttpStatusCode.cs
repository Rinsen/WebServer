using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Http
{
    public abstract class HttpStatusCode
    {
        public abstract string ReasonPhrase { get; }

        public abstract int StatusCode { get; }
    }
}
