using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Http
{
    public class InternalServerError : HttpStatusCode
    {
        public override string ReasonPhrase { get { return "Internal Server Error"; } }
        public override int StatusCode { get { return 500; } }
    }
}
