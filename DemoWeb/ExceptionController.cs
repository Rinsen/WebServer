using System;
using Microsoft.SPOT;
using Rinsen.WebServer;

namespace DemoWeb
{
    public class ExceptionController : Controller
    {
        public void Index()
        {
            throw new Exception("This will always throw an exception!");
        }

        public void InnerException()
        {
            try
            {
                AddThisToStackTrace.GiveMeAnException();
            }
            catch (Exception e)
            {
                throw new Exception("I encapsulate the inner example", e);
            }
        }
    }

    public class AddThisToStackTrace
    {
        public static void GiveMeAnException()
        {
            throw new Exception("This is my inner exception");
        }
    }
}
