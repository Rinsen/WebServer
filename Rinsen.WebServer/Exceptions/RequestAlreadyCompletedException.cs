using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Exceptions
{
    public class RequestAlreadyCompletedException : Exception
    {
        public RequestAlreadyCompletedException(string message)
            : base (message)
        { }
    }
}
