using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Serializers;
using System.Reflection;
using System.Net.Sockets;
using System.Text;
using Rinsen.WebServer.Collections;
using Rinsen.WebServer.Extensions;

namespace Rinsen.WebServer
{
    public class Controller : IController
    {
        public HttpContext HttpContext { get; private set; }
        public IJsonSerializer JsonSerializer { get; private set; }
        private ModelFactory _modelFactory;

        public void InitializeController(HttpContext httpContext, IJsonSerializer jsonSerializer, ModelFactory modelFactory)
        {
            HttpContext = httpContext;
            JsonSerializer = jsonSerializer;
            _modelFactory = modelFactory;
        }
        
        public void SetHtmlResult(string data)
        {
            HttpContext.Response.ContentType = "text/html";
            HttpContext.Response.Data = data;
        }

        public void SetJsonResult(object objectToSerialize)
        {
            HttpContext.Response.ContentType = "application/json";
            //HttpContext.Response.Data = JsonSerializer.Serialize(objectToSerialize);
            HttpContext.Response.Data = Json.NETMF.JsonSerializer.SerializeObject(objectToSerialize);
        }

        /// <summary>
        /// Get form collection with values from query
        /// </summary>
        /// <returns></returns>
        public FormCollection GetFormCollection()
        {
            if (HttpContext.Request.Method == "GET")
            {
                return new FormCollection(HttpContext.Request.Uri.QueryString);    
            }
            else if (HttpContext.Request.Method == "POST")
            {
                var socket = HttpContext.Socket;
                var buffer = new byte[2048];

                var formCollection = new FormCollection(HttpContext.Request.Uri.QueryString);

                while (socket.Available > 0)
                {
                    socket.ReceiveUntil(buffer, "&");
                    var keyValuePair = new String(Encoding.UTF8.GetChars(buffer)).Split('=');
                    formCollection.AddValue(keyValuePair[0], keyValuePair[1]);
                }

                return formCollection;
            }

            throw new NotSupportedException("Only GET and POST is supported");
        }

        /// <summary>
        /// Property names is case sensitive from query string
        /// </summary>
        /// <param name="type">Type of model to create and populate</param>
        /// <returns>Model with values from query</returns>
        public object GetDataModel(Type type)
        {
            var formCollection = GetFormCollection();
            object model = _modelFactory.CreateModel(type);
            var properties = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var property in properties)
            {
                // Skip if is a get property
                if (property.ReturnType != typeof(void))
                    continue;

                var propertyName = property.Name.Substring(4);

                if (formCollection.ContainsKey(propertyName))
                    property.Invoke(model, new object[] { formCollection[propertyName] });
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainFilter"></param>
        public void AllowCors(string domainFilter)
        {
            if (domainFilter == "*")
	        {
                HttpContext.Response.Headers.AddValue("Access-Control-Allow-Origin", "*");
		        return;
	        }
            else if (domainFilter == HttpContext.Request.Uri.Host)
	        {
		        HttpContext.Response.Headers.AddValue("Access-Control-Allow-Origin", domainFilter);
                return;
	        } 
            else if (domainFilter.LastIndexOf(',') > -1)
	        {
		        var domains = domainFilter.Split(',');
                foreach (var domain in domains)
	            {
                    if (domain == HttpContext.Request.Uri.Host)
	                {
		                HttpContext.Response.Headers.AddValue("Access-Control-Allow-Origin", domain);
                        return;
	                }
	            }
	        }
        }
    }
}
