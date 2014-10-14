using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Exceptions;
using Fredde.Web.MicroFramework;
using Rinsen.WebServer.Http;

namespace Rinsen.WebServer
{
    public class ResponseContext
    {
        public ResponseContext()
        {
            Headers = new HeaderCollection();
        }

        public bool Completed { get; set; }

        public string FileFullName { get; set; }

        public string FileLength { get; set; }

        public string Data { get; set; }

        public string DataLength
        {
            get
            {
                if (FileFullName != null && FileFullName != string.Empty)
                {
                    return FileLength;
                }
                return Data.Length.ToString(); 
            } 
        }

        public string ContentType { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public HeaderCollection Headers { get; set; }

        public string HttpVersion { get; set; }

        public bool IsFile { get { return FileFullName != null && FileFullName != string.Empty; } }

        public bool IsValid
        {
            get
            {
                return Data != null || FileFullName != null;
            }
        }
    }
}
