using System;
using Microsoft.SPOT;

using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using Rinsen.WebServer.Collections;
using Rinsen.WebServer.Extensions;


namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public class FileController : Controller
    {
        public ISDCardManager SDCardManager { get; set; }
        //private const int _PostRxBufferSize = 1500;

        public FormCollection RecieveFiles()
        {
            var request = HttpContext.Request;



            if (request.Method == HTTPMethod.Post &&
                request.ContentType.MainContentType == EnumMainContentType.MultiPart &&
                request.ContentType.SubContentType == EnumSubContentType.FormData)
            {
                var contentLengthFromHeader = int.Parse(request.Headers["Content-Length"].ToString());
                var contentLengthReceived = 0;
                var PostedData = new FormCollection();
                var socket = HttpContext.Socket;
                var allDataRecieved = false;
                var receivedByteCount = 0;
                var buffer = new byte[2048]; //The size of this array essentially sets the read rate...
                var fileDirectoryPath = SDCardManager.GetWorkingDirectoryPath();
                var boundaryBytes = Encoding.UTF8.GetBytes(request.Boundary);
                var BeginningboundaryBytes = Encoding.UTF8.GetBytes("\r\n-----");
                var strBldrBoundaryHeader = new StringBuilder();
                var strBldrBoundaryData = new StringBuilder();
                StringBuilder strBldr = new StringBuilder();
                var BoundaryDataSeparator = Encoding.UTF8.GetBytes("\r\n\r\n"); //a double newline delimits Boundary Headers from Boundary Data...
                var BoundaryDataIndex = 0;
                Regex rx = new Regex("([^=\\s]+)=\"([^\"]*)\""); //matches key value pairs like the below...
                //var Value = "key1=\"value1\" key2=\"value 2\" key3=\"value3\" key4=\"value4\" key5=\"5555\" key6=\"xxx666\"";


                Debug.Print("Boundary is: " + request.Boundary); //The Boundary property will be populated if it is multipart/form-data
                Debug.Print("Reading Bytes...");
                socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount, true); //Discard the first Boundary Read...

                while (!allDataRecieved)
                {
                    contentLengthReceived += receivedByteCount;
                    Debug.Print("Bytes Read: " + receivedByteCount +
                    "\r\nTotal Bytes Read: " + contentLengthReceived +
                    "\r\nTotal Bytes To Read: " + contentLengthFromHeader);

                    if (contentLengthReceived < contentLengthFromHeader)
                    {
                        Debug.Print("Reading Bytes...");
                        socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount, true);
                        BoundaryDataIndex = buffer.IndexOf(BoundaryDataSeparator);
                        //Debug.Print("Boundary Data Index: " + BoundaryDataIndex);

                        /*The end of the  multipart form is denoted by the boundary data followed by -- and a Carriage Return Line Feed (ex -----------------------------42291685921978--)
                         * Since I read until the the Boundary Data, I loop until only those characters are found; I do a check for null bytes just to make sure.*/
                        if (buffer.IndexOf(Encoding.UTF8.GetBytes("--\r\n")) == 0 && buffer[5] == default(byte))
                        {
                            Debug.Print("You've reached the end of the multipart form! ");
                            continue;
                        }

                        strBldrBoundaryHeader.Clear();
                        strBldrBoundaryHeader.Append(Encoding.UTF8.GetChars(buffer.SubArray(0, BoundaryDataIndex + 1)));
                        //Debug.Print("Boundary Header: \r\n" + strBldrBoundaryHeader.ToString().ToString().Trim());

                        var objBoundaryHeaderCollection = new BoundaryHeaderCollection(strBldrBoundaryHeader.ToString());

                        strBldr.Clear();
                        strBldr.Append(objBoundaryHeaderCollection["Content-Disposition".ToLower()]); //I store all keys in lowercase...
                        //Debug.Print("Content-Disposition Value: " + strBldr);
                        var KeyValuePairs = new Hashtable();
                        foreach (Match m in rx.Matches(strBldr.ToString()))
                        {
                            Debug.Print("Found KeyValue Pair:" +
                                "\r\n\tKey: " + m.Groups[1] +
                                "\r\n\tValue: " + m.Groups[2]);
                            KeyValuePairs.Add(m.Groups[1].ToString().ToLower(), m.Groups[2]);

                        }
                            
                        
                        if (objBoundaryHeaderCollection.Contains("Content-Type".ToLower()))//write Boundary Data to File...
                        {
                            strBldr.Clear();//Get the value portion of the Content-Type Boundary Header
                            strBldr.Append(objBoundaryHeaderCollection["Content-Type".ToLower()]); //I store all keys in lowercase...
                            Debug.Print("I've got a file and the Content-Type is: " + strBldr.ToString());

                            //Get the file's name from the KeyValuePairs...
                            string fileName = string.Empty;
                            if (KeyValuePairs.Contains("filename")) //JQuery ajax post (from <input type="file">) sends filename parameter containing file's name; the name property is left as defined in the html.
                                fileName = KeyValuePairs["filename"].ToString();
                            else if (KeyValuePairs.Contains("name")) //Form Post sends name parameter containing file's name (no filename parameter is sent)
                                fileName = KeyValuePairs["name"].ToString();
                            else
                                fileName = "Unknown.txt";
                            Debug.Print("File name is set to: " + fileName);


                            
                            if (buffer.IndexOf(boundaryBytes) == -1) //Write to file, loop to get remaining file data
                            {
                                var BoundaryData = buffer.SubArray(BoundaryDataIndex + BoundaryDataSeparator.Length, receivedByteCount - BoundaryDataIndex - BoundaryDataSeparator.Length);
                                var intTemp = receivedByteCount;
                                SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Create, BoundaryData, BoundaryData.Length);
                                while (buffer.IndexOf(boundaryBytes) == -1)  //boundary hasn't been reached, get more data...
                                {
                                    Debug.Print("Getting more File data...");
                                    socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount, true);
                                    if (buffer.IndexOf(boundaryBytes) != -1)//remove boundary string here before you have written to a file...
                                    {
                                        var intBeginningBoundaryBytesIndex = buffer.IndexOf(BeginningboundaryBytes);
                                        var Data = buffer.SubArray(0, intBeginningBoundaryBytesIndex);
                                        SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Append, Data, Data.Length);
                                    }
                                    else
                                        SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Append, buffer, receivedByteCount);
                                    intTemp += receivedByteCount;
                                    Debug.Print("Iterated a loop and Recieved: " + receivedByteCount + " Bytes...");
                                }
                                
                                receivedByteCount = intTemp;
                            }
                            else //remove boundary string, write to file and exit...
                            {
                                var BoundaryData = buffer.SubArray(BoundaryDataIndex + BoundaryDataSeparator.Length, receivedByteCount - BoundaryDataIndex - BoundaryDataSeparator.Length + 1);
                                var intBeginningBoundaryBytesIndex = BoundaryData.IndexOf(BeginningboundaryBytes);
                                var Data = BoundaryData.SubArray(0, intBeginningBoundaryBytesIndex);
                                SDCardManager.Write(fileDirectoryPath, fileName, System.IO.FileMode.Append, Data, Data.Length);
                            }
                        }
                        else //add the 'name' value and BoundaryData to the PostedData FormCollection object...
                        {
                            strBldrBoundaryData.Clear();
                            strBldrBoundaryData.Append(Encoding.UTF8.GetChars(buffer.SubArray(BoundaryDataIndex + BoundaryDataSeparator.Length, buffer.Length - BoundaryDataIndex - BoundaryDataSeparator.Length)));
                            //Debug.Print("Boundary Data: \r\n" + strBldrBoundaryData.ToString());

                            var strBoundaryData = strBldrBoundaryData.ToString();
                            if (KeyValuePairs.Contains("name"))
                                PostedData.AddValue(KeyValuePairs["name"].ToString(), strBoundaryData.Substring(0, strBoundaryData.IndexOf('\n')));
                            Debug.Print("Added Key/Value Pair to PostedData FormCollection...");
                        }


                        

                        //var data = socket.GetMoreBytes(2048, out receivedByteCount);
                        //SDCardManager.Write(fileDirectoryPath, "Test.txt", System.IO.FileMode.Append, data, receivedByteCount);                  

                    }
                    else
                        allDataRecieved = true;
                }

                return PostedData;
            }

            throw new NotSupportedException("Only POST is supported");
        }
    }
}
