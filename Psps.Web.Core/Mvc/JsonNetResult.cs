using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Psps.Core.Helper;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.None,
                ContractResolver = new NHibernateContractResolver()
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            Ensure.Argument.NotNull(context, "context");

            Ensure.Not<InvalidOperationException>(
                this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && "GET".Equals(context.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase)
                , "JSON GET is not allowed");

            HttpRequestBase request = context.HttpContext.Request;
            HttpResponseBase response = context.HttpContext.Response;

            //correct the content-type for file upload from IE8
            if (request.ContentType != null && request.ContentType.StartsWith("multipart/form-data;"))
                response.ContentType = "text/html";
            else
                response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(this.Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }

        public class NHibernateContractResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonContract CreateContract(Type objectType)
            {
                if (typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType))
                    return base.CreateContract(objectType.BaseType);
                else
                    return base.CreateContract(objectType);
            }
        }
    }
}