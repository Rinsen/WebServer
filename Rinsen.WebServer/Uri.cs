using System.Text;
using System.Text.RegularExpressions;

namespace Rinsen.WebServer
{
    public class Uri
    {
        public Uri(string absoluteUri)
        {
            var regex = new Regex(@"(://)");
            var schemePartAndRest = regex.Split(absoluteUri);

            UriScheme = schemePartAndRest[0];

            var uriAndQueryString = schemePartAndRest[1].Split('?');

            if (uriAndQueryString.Length > 1)
            {
                QueryString = uriAndQueryString[1];
            }

            var hostPartLength = uriAndQueryString[0].IndexOf('/');
            if (hostPartLength != -1)
            {
                SetHostAndPort(uriAndQueryString[0].Substring(0, hostPartLength));
                AbsolutePath = uriAndQueryString[0].Substring(hostPartLength);
            }
            else
            {
                SetHostAndPort(uriAndQueryString[0]);
                AbsolutePath = "/";
            }
        }

        public Uri(string uriScheme,string host, string relativeUri, int port)
        {
            UriScheme = uriScheme;
            Host = host;
            var uriAndQueryString = relativeUri.Split('?');

            if (uriAndQueryString.Length > 1)
            {
                QueryString = uriAndQueryString[1];
            }

            if (uriAndQueryString[0] != string.Empty)
            {
                AbsolutePath = uriAndQueryString[0];
            }
            else
            {
                AbsolutePath = "/";
            }
                
            Port = port;
        }

        private void SetHostAndPort(string hostAndPortString)
        {
            var portSeparator = hostAndPortString.IndexOf(':');

            if (portSeparator != -1)
            {
                Host = hostAndPortString.Substring(0, portSeparator);
                Port = int.Parse(hostAndPortString.Substring(portSeparator + 1));
            }
            else
            {
                Host = hostAndPortString;
                Port = 80;
            }
        }

        public bool IsFile
        {
            get 
            {
                return AbsolutePath.Substring(AbsolutePath.LastIndexOf('/')).LastIndexOf(".") > -1;
            }
        }

        public string AbsoluteUri 
        {
            get 
            {
                var sb = new StringBuilder(UriScheme + "://");
                sb.Append(Host);
                if (Port != 80)
                {
                    sb.Append(":" + Port);
                }
                sb.Append(RawPath);

                return sb.ToString();
            } 
        }

        public string UriScheme { get; private set; }

        public string Host { get; private set; }

        public string QueryString { get { return _queryString != null ? _queryString : string.Empty; } private set { _queryString = value; } }

        private string _queryString;

        public string AbsolutePath { get; private set; }

        public string RawPath
        {
            get
            {
                var sb = new StringBuilder(AbsolutePath);
                if (QueryString != null && QueryString != string.Empty)
                {
                    sb.Append("?");
                    sb.Append(QueryString);
                }
                return sb.ToString();
            }
        }

        public int Port { get; set; }

        public string LocalPath 
        {
            get 
            {
                char[] chars = AbsolutePath.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i] == '/') 
                        chars[i] = '\\';
                }
                return new string(chars);
            } 
        }

        public override string ToString()
        {
            return AbsoluteUri;
        }
    }
}
