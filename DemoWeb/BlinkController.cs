using System;
using Microsoft.SPOT;
using Rinsen.WebServer;

namespace DemoWeb
{
    public class BlinkController : Controller
    {
        private const string ledOnHtml = "<!DOCTYPE html><html><body><h1>Blink example page</h1><p>Select state.</p><form action=\"/Blink/Index\" method=\"get\"><input type=\"radio\" name=\"led\" value=\"On\" checked>On</br><input type=\"radio\" name=\"led\" value=\"Off\">Off</br><input type=\"submit\" value=\"Submit\"></form> </body></html>";
        private const string ledOffHtml = "<!DOCTYPE html><html><body><h1>Blink example page</h1><p>Select state.</p><form action=\"/Blink/Index\" method=\"get\"><input type=\"radio\" name=\"led\" value=\"On\">On</br><input type=\"radio\" name=\"led\" value=\"Off\" checked>Off</br><input type=\"submit\" value=\"Submit\"></form> </body></html>";

        public void Index()
        {
            var formCollection = GetFormCollection();
            if (formCollection.ContainsKey("led"))
            {
                if (formCollection["led"] == "On")
                {
                    HardwareControl.OnBoardLed.TurnOn();
                    SetHtmlResult(ledOnHtml);
                    return;
                }
                else
                {
                    HardwareControl.OnBoardLed.TurnOff();
                    SetHtmlResult(ledOffHtml);
                    return;
                }
            }
            else if (HardwareControl.OnBoardLed.GetStatus())
            {
                SetHtmlResult(ledOnHtml);
                return;
            }

            SetHtmlResult(ledOffHtml);
        }
    }
}
