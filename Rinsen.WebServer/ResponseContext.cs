using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Exceptions;
using Rinsen.WebServer.Http;
using Rinsen.WebServer.Collections;

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
                else if (Data != null && Data != string.Empty)
                {
                    return Data.Length.ToString();
                }
                return "0";
            } 
        }

        public ContentType ContentType { get; set; }

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

        public bool IsSent { get; set; }
    }
}
