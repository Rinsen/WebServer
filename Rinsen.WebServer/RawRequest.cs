using System;
using Microsoft.SPOT;
using System.Collections;

namespace Rinsen.WebServer
{
    internal class RawRequest
    {
        public RawRequest()
        {
            Headers = new ArrayList();
        }

        public string RequestLine { get; set; }

        public ArrayList Headers { get; set; }
    }
}
