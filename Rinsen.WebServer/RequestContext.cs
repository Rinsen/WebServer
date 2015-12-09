using System.Net;
using System.Collections;
using Rinsen.WebServer.Routing;
using Rinsen.WebServer.Collections;

namespace Rinsen.WebServer
{
    public enum EnumRequestType { Get, Post, Put, Undefined } //add as neccessary
    //public enum EnumContentType { Text, Binary, MultipartFormData, Undefined } //add as neccessary

    public class RequestContext
    {
        public RequestContext()
        {
            Headers = new HeaderCollection();
        }

        public string RequestLine { get { return Method + " " + Uri.RawPath + " " + HttpVersion; } }

        public HeaderCollection Headers { get; private set; }

        public string Method { get; private set; }
        public EnumRequestType RequestType { get; private set; }


        public string HttpVersion { get; private set; }

        public IPEndPoint IpEndPoint { get; set; }

        public Uri Uri { get; private set; }

        public RequestRoute RequestedRoute { get; set; }

        public string Information { get; set; }

        public void SetHeaders(ArrayList headers)
        {
            Headers = new HeaderCollection();
            foreach (string header in headers)
            {
                SetHeader(header);
            }
        }

        public void SetHeader(string header)
        {
            var splitIndex = header.IndexOf(':');
            Headers.AddValue(header.Substring(0, splitIndex), header.Substring(splitIndex + 1).TrimStart(' '));

        }

        public void SetRequestLineAndUri(string requestLine)
        {
            var parts = requestLine.Split(' ');
            Method = parts[0];
            switch (Method.Trim().ToUpper())
            {
                case "POST":
                    RequestType = EnumRequestType.Post;
                    break;
                case "GET":
                    RequestType = EnumRequestType.Get;
                    break;
                case "PUT":
                    RequestType = EnumRequestType.Put;
                    break;
                default:
                    RequestType = EnumRequestType.Undefined;
                    break;
            }
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
