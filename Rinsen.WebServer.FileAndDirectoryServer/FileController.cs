using System;
using Microsoft.SPOT;

using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using Rinsen.WebServer.Collections;


namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public class FileController : Controller
    {
        public ISDCardManager SDCardManager { get; set; }

        public string RecieveFile()
        {
            var request = HttpContext.Request;
            Hashtable formVariables = new Hashtable();
            if (request.RequestType == EnumRequestType.Post)
            {

                // This form should be short so get it all data now, in one go.
                // assume no content sent with first packet todo fix
                int contentLengthFromHeader = int.Parse(request.Headers["Content-Length"].ToString());
                int contentLengthReceived = 0;
                string requestContent = "";
                string fileName = "";
                string boundaryPattern = "";
                string fileDirectoryPath = SDCardManager.GetWorkingDirectoryPath();
                {
                    if (contentLengthReceived < contentLengthFromHeader)// get next packet, this should have the start of any file in it. // todo put timeout
                    {
                        int count = 0;
                        byte[] data = SDCardManager.GetMoreBytes(HttpContext.Socket, out count);
                        requestContent += new string(Encoding.UTF8.GetChars(data, contentLengthReceived, count));
                        contentLengthReceived += count;
                    }

                    string strTemp = request.Headers["Content-Type"].Split(new char[] { ';' })[1].Split(new char[] { '=' })[1].ToString();
                    var ContentType = request.Headers["Content-Type"]; //will have a value like "multipart/form-data; boundary=---------------------------2261521032598"
                    var boundarystring = ContentType.Split(new char[] { ';' })[1]; //gives me " boundary=---------------------------2261521032598"
                    string boundary = boundarystring.Split(new char[] { '=' })[1].ToString(); //gives me "---------------------------2261521032598"
                    //int nextBoundaryIndex = requestContent.IndexOf(boundary);// todo boundaries can change
                    boundaryPattern = "--" + boundary;//"#\n\n(.*)\n--$boundary#"
                    Regex MyRegex = new Regex(boundaryPattern, RegexOptions.Multiline);
                    string[] split = MyRegex.Split(requestContent);
                    for (int i = 0; i < split.Length; i++)
                    {
                        const string _ContentDispositionSearch = "Content-Disposition: form-data; name=\"";
                        int pos = split[i].IndexOf(_ContentDispositionSearch);
                        if (pos >= 0)
                        {
                            string remainder = split[i].Substring(pos + _ContentDispositionSearch.Length);
                            //ConsoleWrite.Print(remainder);
                            string[] nameSplit = remainder.Split(new char[] { '\"' }, 2);
                            string name = nameSplit[0];
                            if (nameSplit[1][0] == ';')
                            {// file
                                int fileDataSeparatorIndex = nameSplit[1].IndexOf("\r\n\r\n"); // "\r\n\r\n" data starts after double new line
                                if (fileDataSeparatorIndex >= 0)
                                {
                                    string fileNameSection = nameSplit[1].Substring(0, fileDataSeparatorIndex);
                                    string[] fileNameSplit = fileNameSection.Split(new char[] { '\"' });
                                    formVariables.Add("fileName", fileNameSplit[1]);
                                    fileName = fileNameSplit[1];
                                    string fileDataPart1 = nameSplit[1].Substring(fileDataSeparatorIndex + 4);
                                    SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Create, fileDataPart1);
                                }
                            }
                            else
                            {// normal form variable
                                StringBuilder value = new StringBuilder(nameSplit[1]);
                                value = value.Replace("\r", "").Replace("\n", "").Replace("/", "\\");
                                if (nameSplit[0] == "path")
                                {
                                    fileDirectoryPath = SDCardManager.GetWorkingDirectoryPath() + value + "\\";
                                }
                                formVariables.Add(nameSplit[0], value);


                            }
                        }

                    }
                }

                // get the rest of the file and send to sd card
                if (fileName.Length > 0)// todo what other checks
                {
                    while (contentLengthReceived < contentLengthFromHeader)// get next packet, this should have the start of any file in it. // todo put timeout
                    {
                        byte[] data = null;
                        int count = 0;
                        {
                            data = SDCardManager.GetMoreBytes(HttpContext.Socket, out count);
                            contentLengthReceived += count;
                            //requestContent = new string(Encoding.UTF8.GetChars(data, 0, count));
                        }
                        //ConsoleWrite.CollectMemoryAndPrint(true, System.Threading.Thread.CurrentThread.ManagedThreadId);
                        int boundaryPosition = requestContent.IndexOf(boundaryPattern);
                        if (boundaryPosition < 0)
                        {// no boundary so write all the bytes
                            SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Append, data, count);
                        }
                        else
                        {//  boundary so write some of the bytes via a string
                            string fileContent = requestContent.Substring(0, boundaryPosition);
                            SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Append, fileContent);
                        }
                        // todo other params following

                    }
                }

            }
            string message = string.Empty;
            foreach (string key in formVariables.Keys)
                message += "<p>" + key + ": " + formVariables[key].ToString() + "</p>";

            return message;
        }

    }
}
