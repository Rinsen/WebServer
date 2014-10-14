using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Http;

namespace Rinsen.WebServer
{
    internal class ResponseContextBuilder
    {
        internal void BuildResponseContextBase(ResponseContext responseContext, RequestContext requestContext)
        {
            responseContext.Headers.AddValue("ContentType", ""); // Content-Type: text/html; charset=utf-8
            responseContext.Headers.AddValue("Content-Length", "0");
            responseContext.Headers.AddValue("Accept-Ranges", "bytes");
            responseContext.Headers.AddValue("Connection", "close");
            responseContext.Headers.AddValue("Server", "Fredde Web Server 1.0");

            responseContext.HttpVersion = "HTTP/1.1";
            return;
        }
    }
}
