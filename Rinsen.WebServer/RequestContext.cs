using System.Net;
using System.Collections;
using Rinsen.WebServer.Routing;
using Rinsen.WebServer.Collections;
using Rinsen.WebServer.Extensions;
namespace Rinsen.WebServer
{
    public enum HTTPMethod { Get, Post, Put, Delete, Undefined } //add as neccessary
     

    public class RequestContext
    {
        public RequestContext()
        {
            Headers = new HeaderCollection();
        }

        public string RequestLine { get { return Method.GetName() + " " + Uri.RawPath + " " + HttpVersion; } }

        public HeaderCollection Headers { get; private set; }

        public HTTPMethod Method { get; private set; }

        public ContentType ContentType { get; private set; }

        public string Boundary { get; set; }

        public string HttpVersion { get; private set; }

        public IPEndPoint IpEndPoint { get; set; }

        public Uri Uri { get; private set; }

        public RequestRoute RequestedRoute { get; set; }

        public string Data { get; set; }

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
            var headerKey = header.Substring(0, splitIndex);
            var headerValue = header.Substring(splitIndex + 1).TrimStart(' ');
            
            if (headerKey.Trim().ToLower().Equals("content-type"))
            {
                ContentType = new ContentType();
                var contentTypes = headerValue.Split('/'); //sample data "multipart/form-data; boundary=---------------------------2261521032598"
                 
                ContentType.MainContentType = contentTypes[0].GetContentTypeMain();
                if (ContentType.MainContentType == EnumMainContentType.MultiPart)
                {
                    var subcontentData = contentTypes[1].Split(';');
                    ContentType.SubContentType = subcontentData[0].GetContentTypeSub();

                    var boundaryData = subcontentData[1].Split('=');
                    Boundary = boundaryData[1].Trim('-');
                }
                else
                {
                    ContentType.SubContentType = contentTypes[1].GetContentTypeSub();
                }
            }
            Headers.AddValue(headerKey, headerValue);
        }

        public void SetRequestLineAndUri(string requestLine)
        {
            var parts = requestLine.Split(' ');

            switch (parts[0].Trim().ToUpper())
            {
                case "POST":
                    Method = HTTPMethod.Post;
                    break;
                case "GET":
                    Method = HTTPMethod.Get;
                    break;
                case "PUT":
                    Method = HTTPMethod.Put;
                    break;
                case "DELETE":
                    Method = HTTPMethod.Delete;
                    break;
                default:
                    Method = HTTPMethod.Undefined;
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
