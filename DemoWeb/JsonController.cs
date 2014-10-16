using System;
using Microsoft.SPOT;
using Rinsen.WebServer;
using System.Collections;

namespace DemoWeb
{
    public class JsonController : Controller
    {
        public void Index()
        {
            var myClass = new MyClass
            {
                Id = 1,
                Name = "My name",
                Status = true,
                MyList = new ArrayList { 1, 2, 3 },
                MyOtherClass = new MyOtherClass 
                {
                    InnerName = "My inner name",
                    InnerStatus = false,
                }
            };
            SetJsonResult(myClass);
        }
    }

    public class MyClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public ArrayList MyList { get; set; }

        public MyOtherClass MyOtherClass { get; set; } 
    }

    public class MyOtherClass
    {
        public string InnerName { get; set; }

        public bool InnerStatus { get; set; }
    }
}
