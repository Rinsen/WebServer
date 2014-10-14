using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Http
{
    public class NotImplemented : HttpStatusCode
    {
        public override string ReasonPhrase { get { return "Not Implemented"; } }
        public override int StatusCode { get { return 501; } }
    }
}
