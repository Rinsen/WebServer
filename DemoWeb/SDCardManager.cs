using System;
using Microsoft.SPOT;

using Rinsen.WebServer.FileAndDirectoryServer;

namespace DemoWeb
{
    public class SDCardManager : NetduinoSDCard.SDCard, ISDCardManager
    {
        public SDCardManager(string WorkingDirectory)
            : base(WorkingDirectory) {}
    }
}
