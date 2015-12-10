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
        private const int _PostRxBufferSize = 1500;

        public FormCollection RecieveFiles()
        {
            var request = HttpContext.Request;



            if (request.Method == HTTPMethod.Post &&
                request.ContentType.MainContentType == EnumMainContentType.MultiPart &&
                request.ContentType.SubContentType == EnumSubContentType.FormData)
            {
                var contentLengthFromHeader = int.Parse(request.Headers["Content-Length"].ToString());
                var contentLengthReceived = 0;
                var formCollection = new FormCollection();
                var socket = HttpContext.Socket;
                var allDataRecieved = false;
                var receivedByteCount = 0;
                var buffer = new byte[2048];
                var ReadCount = 0;
                var fileDirectoryPath = SDCardManager.GetWorkingDirectoryPath();
                var boundaryBytes = Encoding.UTF8.GetBytes(request.Boundary);
                var strBldr = new StringBuilder();
                var BoundaryDataSeparator = "\r\n\r\n";
                var BoundaryDataIndex = 0;


                Debug.Print("Boundary is: " + request.Boundary); //The Boundary property will be populated if it is multipart/form-data
                socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount, true); //Discard the first Boundary Read...
                SDCardManager.Write(fileDirectoryPath, "Test.txt", System.IO.FileMode.Create, buffer, receivedByteCount);
                while (!allDataRecieved)
                {
                    contentLengthReceived += receivedByteCount;
                    Debug.Print("Bytes Read: " + receivedByteCount +
                    "\r\nTotal Bytes Read: " + contentLengthReceived +
                    "\r\nTotal Bytes To Read: " + contentLengthFromHeader);

                    if (contentLengthReceived < contentLengthFromHeader)
                    {
                        //while ()
                        var data = socket.GetMoreBytes(2048, out receivedByteCount);
                        SDCardManager.Write(fileDirectoryPath, "Test.txt", System.IO.FileMode.Append, data, receivedByteCount);
                        //switch (ReadCount)
                        //{
                        //    case 1:
                        //        socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount); //The Boundary property will be populated if it is multipart/form-data
                        //        //Debug.Print("Discarding first read of Boundary...");
                        //        break;
                        //    case 2:
                        //        socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount);
                        //        strBldr.Clear();
                        //        strBldr.Append(Encoding.UTF8.GetChars(buffer));

                        //        BoundaryDataIndex = strBldr.ToString().IndexOf(BoundaryDataSeparator);

                        //        fileDirectoryPath = fileDirectoryPath + strBldr.ToString().Substring(BoundaryDataIndex, strBldr.Length - BoundaryDataIndex).Trim('-').Trim();
                        //        Debug.Print("FileDirectoryPath: " + fileDirectoryPath);
                        //        break;
                        //    case 3:
                        //        socket.ReceiveUntil(buffer, Encoding.UTF8.GetBytes(BoundaryDataSeparator), out receivedByteCount);
                        //        strBldr.Clear();
                        //        strBldr.Append(Encoding.UTF8.GetChars(buffer));
                        //        Debug.Print("Data from 3rd Read" + strBldr);
                        //        //SDCardManager.Write(fileDirectoryPath, "Text.txt", System.IO.FileMode.Create, buffer, receivedByteCount);

                        //        break;
                        //    default:
                        //        socket.ReceiveUntil(buffer, boundaryBytes, out receivedByteCount);
                        //        strBldr.Clear();
                        //        strBldr.Append(Encoding.UTF8.GetChars(buffer));
                        //        Debug.Print("Data from remaining Reads...\r\n" + strBldr);
                        //        //SDCardManager.Write(fileDirectoryPath, "Text.txt", System.IO.FileMode.Append, buffer, receivedByteCount);
                        //        break;
                        //}
                        //ReadCount += 1;
                        

                    }
                    else
                        allDataRecieved = true;
                }

                return formCollection;
            }

            throw new NotSupportedException("Only POST is supported");
        }
    }
}
