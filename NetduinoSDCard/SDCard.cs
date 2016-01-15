using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NetduinoSDCard
{
    public delegate void BytesDelegate(byte[] data);


    public class SDCard // todo web log and error log
    {
        const int READ_CHUNK_SIZE = 1500;// 
        const int WRITE_CHUNK_SIZE = 1500;// 

        public static Object SDCardLock = new Object();
        public const string MountDirectoryPath = @"\SD";

        public SDCard()
        {
        }
        public SDCard(string WorkingDirectory)
        {
            SetWorkingDirectoryInfo(WorkingDirectory);
        }

        public DirectoryInfo WorkingDirectoryInfo { get; set; }
        public void SetWorkingDirectoryInfo(string WorkingDirectory)
        {
            string strPath = string.Empty;


            if (WorkingDirectory.LastIndexOf("\\") == WorkingDirectory.Length - 1)
                strPath = MountDirectoryPath + WorkingDirectory;
            else
                strPath = MountDirectoryPath + WorkingDirectory + "\\";

            CreateDirectory(strPath);
            WorkingDirectoryInfo = new DirectoryInfo(strPath);
        }

        public string GetWorkingDirectoryPath()
        {
            if (WorkingDirectoryInfo == null)
                return MountDirectoryPath;
            else
                return WorkingDirectoryInfo.FullName + "\\";
        }

        public static string GetFullDirectoryPath(string folderName)
        {
            return MountDirectoryPath + folderName;
        }

        public static string GetFileFullPath(string fileName)// todo allow for trailing slash on f
        {
            return MountDirectoryPath + "\\" + fileName;
        }

        public static string GetFileFullPath(string directoryPath, string fileName)
        {
            if (directoryPath.LastIndexOf('\\') == directoryPath.Length - 1)
            {
                return MountDirectoryPath + fileName;
            }
            else
            {
                return directoryPath + "\\" + fileName;
            }
        }

        public static string GetFullPathFromUri(string uri)
        {
            return MountDirectoryPath + PathToBackSlash(uri);
        }

        public void CreateFile(string path, string fileName)// needs trailing slash
        {
            string fileFullPath = path + fileName;
            CreateDirectory(path);
            lock (SDCardLock)
            {
                if (!File.Exists(path + fileName))
                {
                    FileStream fs = File.Create(fileFullPath);
                    fs.Close();
                }
            }
        }

        public void CreateDirectory(string fullPath)
        {
            lock (SDCardLock)
            {
                if (!Directory.Exists(fullPath))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);
                }
            }
        }

        public bool WriteLine(string path, string fileName, FileMode fileMode, string text)
        {
            return Write(path, fileName, fileMode, text + "\n");// todo \r\n??
        }

        public bool Write(string path, string fileName, FileMode fileMode, string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            return Write(path, fileName, fileMode, buffer, buffer.Length);
        }

        public bool Write(string path, string fileName, FileMode fileMode, byte[] bytes, int length)
        {
            string fileFullPath = path + fileName;
            CreateFile(path, fileName);
            lock (SDCardLock)
            {
                FileStream fs = new FileStream(fileFullPath, fileMode, FileAccess.Write, FileShare.None, length);// todo make buffersize a constant somewhere
                fs.Write(bytes, 0, length);
                fs.Close();
            }
            return true;
        }

        public bool WriteLines(string path, string fileName, string[] lines)
        {
            string fileFullPath = path + fileName;
            CreateFile(path, fileName);
            lock (SDCardLock)
            {
                FileStream fs = new FileStream(fileFullPath, FileMode.Append, FileAccess.Write, FileShare.None, WRITE_CHUNK_SIZE);
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    var buf = Encoding.UTF8.GetBytes(lines[i] + "\r\n");
                    fs.Write(buf, 0, buf.Length);
                }
                fs.Close();
            }
            return true;
        }
        //http://forums.netduino.com/index.php?/topic/2394-memory-efficient-way-to-enumerate-an-array-of-fileinfo/page__p__16985__hl__%2Bsdcard+%2Benumerate__fromsearch__1#entry16985

        public string ReadLine(string fullPath)
        {
            var FileContents = string.Empty;

            ConsoleWrite.Print("Reading file: " + fullPath);
            lock (SDCardLock)
            {
                if (File.Exists(fullPath))
                    using (StreamReader objStreamReader = new StreamReader(fullPath))
                        FileContents = objStreamReader.ReadLine();
                else
                    throw new IOException("File Not Found!");
            }
            return FileContents;

        }

        public string ReadTextFile(string fullPath)
        {
            var FileContents = string.Empty;

            ConsoleWrite.Print("Reading file: " + fullPath);
            lock (SDCardLock)
            {
                if (File.Exists(fullPath))
                    using (StreamReader objStreamReader = new StreamReader(fullPath))
                        FileContents = objStreamReader.ReadToEnd();
                else
                    throw new IOException("File Not Found!");
            }
            return FileContents;
        }

        public void SendFile(string fullPath, Socket socket)
        {
            ConsoleWrite.Print("Reading file: " + fullPath);
            bool chunkHasBeenRead = false;
            int totalBytesRead = 0;
            lock (SDCardLock)
            {
                if (File.Exists(fullPath))
                {
                    using (FileStream inputStream = new FileStream(fullPath, FileMode.Open))
                    {
                        byte[] readBuffer = new byte[READ_CHUNK_SIZE];
                        while (true)
                        {
                            // Send the file a few bytes at a time
                            int bytesRead = inputStream.Read(readBuffer, 0, readBuffer.Length);
                            if (bytesRead == 0)
                                break;
                            socket.Send(readBuffer, 0, bytesRead, SocketFlags.None);
                            //sendDataCallback(readBuffer);
                            totalBytesRead += bytesRead;
                        }
                    }
                    chunkHasBeenRead = true;
                }
                else
                {
                    chunkHasBeenRead = false;
                }
            }
            if (chunkHasBeenRead == true)
                ConsoleWrite.Print("Sending " + totalBytesRead.ToString() + " bytes...");
            else
                ConsoleWrite.Print("Failed to read chunk, full path: " + fullPath);
        }

        public static string Replace(string input, char[] oldText, string newText)
        {
            string result = "";
            string[] split = input.Split(oldText);
            for (int i = 0; i < split.Length - 1; i++)
            {
                result += split[i] + newText;
            }
            result += split[split.Length - 1];
            return result;
        }

        public static string PathToBackSlash(string input)
        {
            string result = Replace(input, new char[] { '/' }, "\\");
            return result;
        }

        public long GetFileSize(string directoryPath, string fileName)
        {
            long result = 0;
            //            ConsoleWrite.Print("Get File Size: " + fileNameAndPath);
            lock (SDCardLock)
            {
                string fileNameAndPath = GetFileFullPath(directoryPath, fileName);
                if (Directory.Exists(directoryPath))
                {
                    if (File.Exists(fileNameAndPath))
                    {
                        using (FileStream inputStream = new FileStream(fileNameAndPath, FileMode.Open))
                        {
                            result = inputStream.Length;
                        }
                    }
                }
            }
            return result;
        }
        public long GetFileSize(string fileNameAndPath)
        {
            int lastIndexOf = fileNameAndPath.LastIndexOf('\\');
            string directoryName = fileNameAndPath.Substring(0, lastIndexOf);
            string fileName = fileNameAndPath.Substring(lastIndexOf + 1);
            return GetFileSize(directoryName, fileName);
        }

    }
}
