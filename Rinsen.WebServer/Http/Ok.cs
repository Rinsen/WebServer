using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Http
{
    public class Ok : HttpStatusCode
    {
        public override string ReasonPhrase { get { return "OK"; } }
        public override int StatusCode { get { return 200; } }
    }
}
