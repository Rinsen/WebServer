using System;
using Microsoft.SPOT;
using Rinsen.WebServer.Serializers;
using Fredde.Web.MicroFramework;
using System.Reflection;

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
            HttpContext.Response.Data = JsonSerializer.Serialize(objectToSerialize);
        }

        /// <summary>
        /// Get form collection with values from query
        /// </summary>
        /// <returns></returns>
        public FormCollection GetFormCollection()
        {
            return new FormCollection(HttpContext.Request.Uri.QueryString);
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
