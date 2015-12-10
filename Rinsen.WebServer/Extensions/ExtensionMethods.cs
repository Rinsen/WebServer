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
            var bytesReceived = 0;

            ReceiveUntil(socket, buffer, readUntil, out bytesReceived);
        }

        public static void ReceiveUntil(this Socket socket, byte[] buffer, byte[] readUntil, out int bytesRecieved, bool leaveDelimeter = false)
        {
            // Clear buffer always to remove old junk if buffer is reused
            Array.Clear(buffer, 0, buffer.Length);
            var count = 0;
            var matchCounter = 0;

            bytesRecieved = 0;
            while (socket.Available > 0 && count < buffer.Length)
            {
                bytesRecieved += socket.Receive(buffer, count, 1, SocketFlags.None);
                if (buffer[count] == readUntil[matchCounter])
                {
                    matchCounter++;
                    if (matchCounter == readUntil.Length)
                    {
                        if (!leaveDelimeter)
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

        public static byte[] GetMoreBytes(this Socket socket, int rxBufferSize, out int bytesRecieved)
        {
            byte[] result = new byte[rxBufferSize];
            SocketFlags socketFlags = new SocketFlags();
            bytesRecieved = socket.Receive(result, result.Length, socketFlags);
            return result;
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

        public static string GetName(this HTTPMethod source)
        {
            switch (source)
            {
                case HTTPMethod.Get:
                    return "GET";
                case HTTPMethod.Post:
                    return "POST";
                case HTTPMethod.Put:
                    return "PUT";
                case HTTPMethod.Delete:
                    return "DELETE";
                default:
                    return "Undefined";
            }
        }

        public static string GetName(this EnumMainContentType source)
        {
            switch (source)
            {
                case EnumMainContentType.Application:
                    return "application";
                case EnumMainContentType.Audio:
                    return "audio";
                case EnumMainContentType.Example:
                    return "example";
                case EnumMainContentType.Image:
                    return "image";
                case EnumMainContentType.Message:
                    return "message";
                case EnumMainContentType.Model:
                    return "model";
                case EnumMainContentType.MultiPart:
                    return "multipart";
                case EnumMainContentType.Text:
                    return "text";
                case EnumMainContentType.Video:
                    return "video";
                default:
                    return "Undefined";
            }
        }

        public static EnumMainContentType GetContentTypeMain(this string source)
        {
            switch (source.Trim().ToLower())
            {
                case "application":
                    return EnumMainContentType.Application;
                case "audio":
                    return EnumMainContentType.Audio;
                case "example":
                    return EnumMainContentType.Example;
                case "image":
                    return EnumMainContentType.Image;
                case "message":
                    return EnumMainContentType.Message;
                case "model":
                    return EnumMainContentType.Model;
                case "multipart":
                    return EnumMainContentType.MultiPart;
                case "text":
                    return EnumMainContentType.Text;
                case "video":
                    return EnumMainContentType.Video;
                default:
                    return EnumMainContentType.Undefined;
            }
        }

        public static string GetName(this EnumSubContentType source)
        {
            switch (source)
            {
                case EnumSubContentType.FormData:
                    return "form-data";
                case EnumSubContentType.Json:
                    return "json";
                case EnumSubContentType.Html:
                    return "html";
                case EnumSubContentType.Css:
                    return "css";
                case EnumSubContentType.Plain:
                    return "plain";
                case EnumSubContentType.Jpeg:
                    return "jpeg";
                case EnumSubContentType.JavaScript:
                    return "javascript";
                default:
                    return "Undefined";
            }
        }

        public static EnumSubContentType GetContentTypeSub(this string source)
        {
            switch (source.Trim().ToLower())
            {
                case "form-data":
                    return EnumSubContentType.FormData;
                case "json":
                    return EnumSubContentType.Json;
                case "html":
                    return EnumSubContentType.Html;
                case "css":
                    return EnumSubContentType.Css;
                case "plain":
                    return EnumSubContentType.Plain;
                case "javascript":
                    return EnumSubContentType.JavaScript;
                default:
                    return EnumSubContentType.Undefined;
            }
        }

        public static int IndexOf(this byte[] source, byte[] patternToFind)
        {
            if (patternToFind.Length > source.Length)
                return -1;
            for (int i = 0; i < source.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (source[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
