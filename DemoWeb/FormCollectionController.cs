using System;
using Microsoft.SPOT;
using Rinsen.WebServer;
using System.Text;

namespace DemoWeb
{
    public class FormCollectionController : Controller
    {
        // Example with form collection

        public void Index()
        {
            SetHtmlResult("<!DOCTYPE html><html><body><h1>My Form example page</h1><p>Type your name.</p><form action=\"/FormCollection/Data\" method=\"get\"> First name: <input type=\"text\" name=\"firstname\"><br> Last name: <input type=\"text\" name=\"lastname\"><input type=\"submit\" value=\"Submit\"></form> </body></html>");
        }

        public void Data()
        {
            var formCollection = GetFormCollection();
            if (formCollection.ContainsKey("firstname") && formCollection.ContainsKey("lastname"))
            {
                var firstName = formCollection["firstname"];
                var lastName = formCollection["lastname"];

                var sb = new StringBuilder();
                sb.Append("<!DOCTYPE html><html><body><h1>My Form example page</h1>");
                sb.Append("<p>Your first name is: ");
                sb.Append(firstName);
                sb.Append("<br />");
                sb.Append("Your last name is: ");
                sb.Append(lastName);
                sb.Append("</p></body></html>");
                SetHtmlResult(sb.ToString());
                return;
            }
            SetHtmlResult("<!DOCTYPE html><html><body><h1>My Form example page</h1><p>No name in input fields.</p></body></html>");
        }
    }
}
