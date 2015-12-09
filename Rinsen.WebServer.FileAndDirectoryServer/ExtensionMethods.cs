using System;
using Microsoft.SPOT;

using System.Net.Sockets;


namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public static class ExtensionMethods
    {
        public static byte[] GetMoreBytes(this Socket socket, int rxBufferSize, out int count)
        {
            byte[] result = new byte[rxBufferSize];
            SocketFlags socketFlags = new SocketFlags();
            count = socket.Receive(result, result.Length, socketFlags);
            return result;
        }

        //const int _PostRxBufferSize = 1500;
        //public byte[] GetMoreBytes(Socket connectionSocket, out int count)
        //{
        //    byte[] result = new byte[_PostRxBufferSize];
        //    SocketFlags socketFlags = new SocketFlags();
        //    count = connectionSocket.Receive(result, result.Length, socketFlags);
        //    return result;
        //}
    }
}
