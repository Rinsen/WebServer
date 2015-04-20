using System;
using Microsoft.SPOT;
using Rinsen.WebServer;

namespace DemoWeb
{
    public class DefaultController : Controller
    {
        public void Index()
        {
            SetHtmlResult("<!DOCTYPE html><html><body><h1>Default controller example</h1></body></html>");
        }
    }
}
