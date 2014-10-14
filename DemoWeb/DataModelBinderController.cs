using System;
using Microsoft.SPOT;
using Rinsen.WebServer;
using System.Text;

namespace DemoWeb
{
    public class DataModelBinderController : Controller
    {
        // Example with data model

        public void Index()
        {
            SetHtmlResult("<!DOCTYPE html><html><body><h1>My Form example page</h1><p>Type your name.</p><form action=\"/DataModelBinder/Data\" method=\"get\"> First name: <input type=\"text\" name=\"FirstName\"><br> Last name: <input type=\"text\" name=\"LastName\"><input type=\"submit\" value=\"Submit\"></form> </body></html>");        
        }

        public void Data()
        {
            var model = (NameModel)GetDataModel(typeof(NameModel));

            if (model.FirstName == string.Empty || model.LastName == string.Empty)
            {
                SetHtmlResult("<!DOCTYPE html><html><body><h1>My Form example page</h1><p>No name in input fields.</p></body></html>");
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><body><h1>My Form example page</h1>");
            sb.Append("<p>Your first name is: ");
            sb.Append(model.FirstName);
            sb.Append("<br />");
            sb.Append("Your last name is: ");
            sb.Append(model.LastName);
            sb.Append("</p></body></html>");
            SetHtmlResult(sb.ToString());
        }
    }
}
