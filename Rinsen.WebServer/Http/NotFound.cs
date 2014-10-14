using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Http
{
    public class NotFound : HttpStatusCode
    {
        public override string ReasonPhrase { get { return "Not Found"; } }
        public override int StatusCode { get { return 404; } }
    }
}
