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
                        Array.Clear(buffer, count - readUntil.Length + 1, readUntil.Length);
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

        public static int Count(this IEnumerable collection)
        {
            var count = 0;
            foreach (var item in collection)
            {
                count++;
            }
            return count;
        }

        public static string ToHexString(this byte[] value, int index = 0)
        {
            return ToHexString(value, index, value.Length - index);
        }

        public static string ToHexString(this byte[] value, int index, int length)
        {
            char[] c = new char[length * 3];
            byte b;

            for (int y = 0, x = 0; y < length; ++y, ++x)
            {
                b = (byte)(value[index + y] >> 4);
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = (byte)(value[index + y] & 0xF);
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                c[++x] = '-';
            }
            return new string(c, 0, c.Length - 1);
        }
    }
}
