using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Exceptions
{
    public class RequestClientSocketException : Exception
    {
        public RequestClientSocketException(string message)
            : base (message)
        { }
    }
}
