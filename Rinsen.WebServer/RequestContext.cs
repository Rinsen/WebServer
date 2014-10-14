using System.Net;
using Fredde.Web.MicroFramework;
using Rinsen.WebServer.Routing;

namespace Rinsen.WebServer
{
    public class RequestContext
    {
        private readonly ServerContext _serverContext;

        public RequestContext(ServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        public string RequestLine { get { return Method + " " + Uri.RawPath + " " + HttpVersion; } }

        public HeaderCollection Headers { get; private set; }

        public string Method { get; private set;}

        public string HttpVersion { get; private set; }

        public IPEndPoint IpEndPoint { get; set; }

        public Uri Uri { get; private set; }

        public RequestRoute RequestedRoute { get; set; }

        public string Information { get; set; }

        public void SetHeaders(string[] headers)
        {
            var headerCollection = new HeaderCollection();
            foreach (var header in headers)
            {
                var headerParts = header.Split(':');
                headerCollection.AddValue(headerParts[0], headerParts[1].TrimStart(' '));
            }
            Headers = headerCollection;
        }

        public void SetRequestLineAndUri(string requestLine)
        {
            var parts = requestLine.Split(' ');
            Method = parts[0];
            var host = Headers["Host"].Split(':');
            if (host.Length > 1)
            {
                Uri = new Uri("http", host[0], parts[1], int.Parse(host[1]));
            }
            else
            {
                Uri = new Uri("http", host[0], parts[1], 80);
            }
            HttpVersion = parts[2];
        }
    }
}
