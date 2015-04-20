using System;
using Microsoft.SPOT;
using System.Collections;
using System.Net.Sockets;
using System.Text;

namespace Rinsen.WebServer.Extensions
{
    public static class ExtensionMethods
    {
        public static bool Any(this ArrayList arrayList)
        {
            return arrayList.Count > 0;
        }

        public static bool Any(this string[] stringArray)
        {
            return stringArray.Length > 0;
        }

        public static void Append(this ArrayList targetArrayList, ArrayList arrayList)
        {
            foreach (var item in arrayList)
            {
                targetArrayList.Add(item);
            }
        }

        public static void ReceiveUntil(this Socket socket, byte[] buffer, string readUntil)
        {
            ReceiveUntil(socket, buffer, Encoding.UTF8.GetBytes(readUntil));
        }

        public static void ReceiveUntil(this Socket socket, byte[] buffer, byte[] readUntil)
        {
            // Clear buffer always to remove old junk if buffer is reused
            Array.Clear(buffer, 0, buffer.Length);
            var count = 0;
            var matchCounter = 0;
            while (socket.Available > 0 && count < buffer.Length)
            {
                socket.Receive(buffer, count, 1, SocketFlags.None);
                if (buffer[count] == readUntil[matchCounter])
                {
                    matchCounter++;
                    if (matchCounter == readUntil.Length)
                    {
                        Array.Clear(buffer, count - 1, readUntil.Length);
                        break;
                    }
                }
                else
                {
                    matchCounter = 0;
                }
                count++;
            }
        }
    }
}
