using System;
using Microsoft.SPOT;
using System.Collections;
using MFUnit;
using Rinsen.WebServer.Extensions;

namespace Rinsen.WebServer.UnitTests
{
    public class RequestContextTests
    {
        public void WhenSomeHeadersWithValue_GetCorrectHeadersAndValuesInHeaderCollection()
        {
            // Arrange
            var requestContext = new RequestContext();
            var headerList = new ArrayList();
            headerList.Add("Accept: text/plain");
            headerList.Add("Referer: http://en.wikipedia.org/wiki/Main_Page");
            headerList.Add("User-Agent: Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0");

            // Act
            requestContext.SetHeaders(headerList);

            // Assert

            Assert.AreEqual(3, requestContext.Headers.Keys.Count());
            Assert.AreEqual("text/plain", requestContext.Headers["Accept"]);
            Assert.AreEqual("http://en.wikipedia.org/wiki/Main_Page", requestContext.Headers["Referer"]);
            Assert.AreEqual("Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0", requestContext.Headers["User-Agent"]);

        }

        public void WhenHeaderWithValue_GetHeaderAndValueInHeaderCollection()
        {
            // Arrange
            var requestContext = new RequestContext();
            var headerList = new ArrayList();
            headerList.Add("My-header:");
            headerList.Add("My-header2: ");
            
            // Act
            requestContext.SetHeaders(headerList);

            // Assert

            Assert.AreEqual(2, requestContext.Headers.Keys.Count());
            Assert.AreEqual("", requestContext.Headers["My-header"]);
            Assert.AreEqual("", requestContext.Headers["My-header2"]);
        }
    }
}
