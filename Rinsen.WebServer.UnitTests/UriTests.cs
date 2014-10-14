using MFUnit;

namespace Rinsen.WebServer.UnitTests
{
    public class UriTests
    {
        
        public void WhenNewUri_WithSimpleAbsoluteUri_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com");

            // Assert
            Assert.AreEqual("http", uri.UriScheme);
            Assert.AreEqual("www.testuri.com", uri.Host);
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual(80, uri.Port);
        }

        public void WhenNewUri_WithSimplePartsUri_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "", 80);

            // Assert
            Assert.AreEqual("http", uri.UriScheme);
            Assert.AreEqual("www.testuri.com", uri.Host);
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual(80, uri.Port);
        }

        public void WhenNewUri_WithSimpleAbsoluteUriWithSlash_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com/");

            // Assert
            Assert.AreEqual("http", uri.UriScheme);
            Assert.AreEqual("www.testuri.com", uri.Host);
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual(80, uri.Port);
        }

        public void WhenNewUri_WithSimplePartsUriWithSlash_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "/", 80);

            // Assert
            Assert.AreEqual("http", uri.UriScheme);
            Assert.AreEqual("www.testuri.com", uri.Host);
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual(80, uri.Port);
        }

        public void WhenNewUri_WithAbsoluteUriAndSpecificPath_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com/relative/path");

            // Assert
            Assert.AreEqual("/relative/path", uri.AbsolutePath);
        }

        public void WhenNewUri_WithPartsUriAndSpecificPath_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "/relative/path", 80);

            // Assert
            Assert.AreEqual("/relative/path", uri.AbsolutePath);
        }

        public void WhenNewUri_WithAbsoluteUriAndQuery_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com/?MyInput=15");

            // Assert
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithPartsUriAndQuery_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "/?MyInput=15", 80);

            // Assert
            Assert.AreEqual("/", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithAbsoluteUriAndSpecificPathWithQuery_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com/test/test/test?MyInput=15");

            // Assert
            Assert.AreEqual("/test/test/test", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithPartsUriAndSpecificPathWithQuery_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "/test/test/test?MyInput=15", 80);

            // Assert
            Assert.AreEqual("/test/test/test", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithAbsoluteUriAndSpecificPathWithQueryAndEndSlash_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com/test/test/test/?MyInput=15");

            // Assert
            Assert.AreEqual("/test/test/test/", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithPartsUriAndSpecificPathWithQueryAndEndSlash_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "/test/test/test/?MyInput=15", 80);

            // Assert
            Assert.AreEqual("/test/test/test/", uri.AbsolutePath);
            Assert.AreEqual("MyInput=15", uri.QueryString);
        }

        public void WhenNewUri_WithSpecificPort_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://www.testuri.com:5000/");

            // Assert
            Assert.AreEqual(5000, uri.Port);
        }

        public void WhenNewUri_WithSpecificPortInParts_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "www.testuri.com", "", 5000);

            // Assert
            Assert.AreEqual(5000, uri.Port);
        }

        public void WhenNewUri_WithIpAddressAndDefaultPort_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://192.168.1.56/");

            // Assert
            Assert.AreEqual("192.168.1.56", uri.Host);
            Assert.AreEqual(80, uri.Port);
        }

        public void WhenNewUri_WithIpAddressAndSpecificPort_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http://192.168.1.57:5000/");

            // Assert
            Assert.AreEqual("192.168.1.57", uri.Host);
            Assert.AreEqual(5000, uri.Port);
        }

        public void WhenNewUri_WithIpAddressAndSpecificPortInParts_GetCorrespondingResultInUri()
        {
            // Act
            var uri = new Uri("http", "192.168.1.57", "/", 5000);

            // Assert
            Assert.AreEqual("192.168.1.57", uri.Host);
            Assert.AreEqual(5000, uri.Port);
        }

        public void WhenNewUri_WithFileInPartsRelativeUri_GetTrue()
        {
            // Act
            var uri = new Uri("http", "192.168.1.57", "/First/Second.html", 5000);

            // Assert
            Assert.IsTrue(uri.IsFile);
        }

        public void WhenNewUri_WithNoFileInPath_GetFalse()
        {
            // Act
            var uri = new Uri("http://192.168.1.57:5000/First/Second/Third");

            // Assert
            Assert.IsFalse(uri.IsFile);
        }

        public void WhenNewUri_WithNoFileInPartsRelativeUri_GetFalse()
        {
            // Act
            var uri = new Uri("http", "192.168.1.57", "/First/Second/Third", 5000);

            // Assert
            Assert.IsFalse(uri.IsFile);
        }

        public void WhenToStringWithDefaultPort_GetStringRepresentationUfUriWithoutPort()
        {
            // Arrange
            var uri = new Uri("http", "testuri.se", "/testPath?Value=10", 80);

            // Act & Assert
            Assert.AreEqual("http://testuri.se/testPath?Value=10", uri.AbsoluteUri);
        }

        public void WhenToString_GetStringRepresentationUfUriWithPort()
        {
            // Arrange
            var uri = new Uri("http", "testuri.se", "/testPath?Value=10", 2500);

            // Act & Assert
            Assert.AreEqual("http://testuri.se:2500/testPath?Value=10", uri.AbsoluteUri);
        }

        public void WhenToStringWithNoQueryString_GetStringRepresentationUfUriWithNoQueryString()
        {
            // Arrange
            var uri = new Uri("http", "testuri.se", "/testPath", 2500);

            // Act
            var stringUri = uri.ToString();

            // Assert
            Assert.AreEqual("http://testuri.se:2500/testPath", uri.AbsoluteUri);
        }

        public void WhenGetLocalPath_GetCorrectLocalPath()
        {
            // Arrange
            var uri = new Uri("http", "testuri.se", "/folder1/folder2", 2500);

            // Act & Assert
            Assert.AreEqual("\\folder1\\folder2", uri.LocalPath);
        }
    }
}
