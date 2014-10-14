using System.IO;
using System.Text;
namespace Rinsen.WebServer.FileAndDirectoryServer
{
    internal class DirectoryListBuilder
    {
        public string GenerateSimpleDirectoryList(string rawUrl, DirectoryInfo[] directories, FileInfo[] files, string hostName)
        {
            var lastSlash = rawUrl.LastIndexOf('/');
            if (lastSlash == rawUrl.Length - 1)
            {
                rawUrl = rawUrl.Substring(0, lastSlash);
                lastSlash = rawUrl.LastIndexOf('/');
            }
            var htmlBuilder = new StringBuilder();
            var counter = 0;
            htmlBuilder.Append("<html><head><title>" + hostName + " - " + rawUrl + "</title></head><body><H1>" + hostName + " - " + rawUrl + "</H1><hr><pre><A HREF=\"" + GetParentUrl(rawUrl) + "\">[To Parent Directory]</A><br><br>");
            foreach (var dir in directories)
            {
                htmlBuilder.Append(dir.LastAccessTime.ToString() + "      &lt;dir&gt; <A HREF=\"" + rawUrl + "/" + dir.Name + "/" + "\" >" + dir.Name + "</A><br>");
                counter++;
                if (counter >= 5)
                    break;
            }
            foreach (var file in files)
            {
                htmlBuilder.Append(file.LastWriteTime.ToString() + FormatString(file.Length.ToString()) + " <A HREF=\"" + GetParentUrlForFileLink(rawUrl) + file.Name + "\" >" + file.Name + "</A><br>");
                counter++;
                if (counter >= 15)
                    break;
            }
            htmlBuilder.Append("</pre><hr></body></html>");
            return htmlBuilder.ToString();
        }

        private string GetParentUrlForFileLink(string rawUrl)
        {
            if (rawUrl.LastIndexOf('/') == rawUrl.Length)
                return rawUrl;
            return rawUrl + "/";
        }

        private string GetParentUrl(string rawUrl)
        {
            var lastSlash = rawUrl.LastIndexOf('/');
            if (lastSlash <= 0)
                return "/";
            return rawUrl.Substring(0, lastSlash);
        }

        private string FormatString(string fileSize, int totalLength = 11)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < totalLength - fileSize.Length; i++)
            {
                sb.Append(" ");
            }
            sb.Append(fileSize);
            return sb.ToString();
        }
    }
}
