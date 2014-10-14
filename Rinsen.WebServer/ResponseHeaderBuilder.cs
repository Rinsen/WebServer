using System.Text;

namespace Rinsen.WebServer
{
    public class ResponseHeaderBuilder
    {
        public string BuildResponseLineAndHeaders(ResponseContext responseContext)
        {
            SetContentLength(responseContext);

            var responseLineAndHeaders = new StringBuilder(responseContext.HttpVersion + " " + responseContext.HttpStatusCode.StatusCode + " " + responseContext.HttpStatusCode.ReasonPhrase + "\r\n");
            foreach (var key in responseContext.Headers.Keys)
            {
                var value = responseContext.Headers[key.ToString()];
                responseLineAndHeaders.Append(key + ": " + value + "\r\n");
            }
            responseLineAndHeaders.Append("\r\n");

            return responseLineAndHeaders.ToString();
        }

        private void SetContentLength(ResponseContext response)
        {
            response.Headers["Content-Length"] = response.DataLength;
            response.Headers["ContentType"] = response.ContentType;
        }
    }
}
