using System;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Core.ActionFilters
{
    public class CompressResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            //TODO: Replace hard coded escape pages to parameter in web.config
            var regex = new Regex(@"^\/secure\/admin\/errors.*", RegexOptions.IgnoreCase);
            if (regex.Match(request.Path).Success)
                return;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (!String.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToLowerInvariant();

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (response.Filter != null && string.IsNullOrEmpty(response.Headers["Content-Encoding"]))
                {
                    if (acceptEncoding.Contains("gzip"))
                    {
                        response.AppendHeader("Content-Encoding", "gzip");
                        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate"))                    
                    {
                        response.AppendHeader("Content-Encoding", "deflate");
                        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    }
                }
            }
        }
    }
}