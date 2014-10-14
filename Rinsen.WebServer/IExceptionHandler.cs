using System;
using Microsoft.SPOT;

namespace Rinsen.WebServer
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);        
    }
}
