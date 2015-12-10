using System;
using Microsoft.SPOT;

using Rinsen.WebServer.Extensions;


namespace Rinsen.WebServer
{
    public enum EnumMainContentType 
    {
        Application,
        Audio,
        Example,
        Image,
        Message,
        Model,
        MultiPart,
        Text,
        Video,
        Undefined
    }

    public enum EnumSubContentType
    {
        FormData,
        Json,
        Html,
        Css,
        Plain,
        Jpeg,
        JavaScript,
        Undefined
    }

    //per http://www.iana.org/assignments/media-types/media-types.xhtml
    public class ContentType
    {
        public EnumMainContentType MainContentType { get; set; }
        public EnumSubContentType SubContentType { get; set; }

        public override string ToString()
        {
            return MainContentType.GetName() + "/" + SubContentType.GetName();
        }
    }
}
