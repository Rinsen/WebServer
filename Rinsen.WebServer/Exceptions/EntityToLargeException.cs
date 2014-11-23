using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer.Exceptions
{
    public class EntityToLargeException : Exception
    {
        public EntityToLargeException(string message)
            : base (message)
        { }
    }
}
