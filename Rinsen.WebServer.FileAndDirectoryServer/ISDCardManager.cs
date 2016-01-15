using System;
using Microsoft.SPOT;

using System.Net.Sockets;
using System.IO;

namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public interface ISDCardManager
    {
        string GetWorkingDirectoryPath();

        void SendFile(string fullPath, Socket socket);

        bool Write(string path, string fileName, FileMode fileMode, string text);

        bool Write(string path, string fileName, FileMode fileMode, byte[] bytes, int length);

        string ReadTextFile(string fullPath);
    }
}
