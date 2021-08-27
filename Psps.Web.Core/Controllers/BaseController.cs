using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Core.Models;
using Psps.Models.Dto.Reports;
using Psps.Web.Core.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Core.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected bool IsAjaxRequest
        {
            get
            {
                //assume all the ajax methods are routed to /api/*
                return HttpContext.Request.Path.StartsWith("/api/") || HttpContext.Request.IsAjaxRequest();
            }
        }

        protected IUserInfo CurrentUser
        {
            get { return EngineContext.Current.Resolve<IWorkContext>().CurrentUser; }
        }

        /// <summary>
        /// Store the file stream to session and wait for download
        /// </summary>
        /// <param name="uniqueId">unique id for storing the file stream</param>
        /// <param name="fileName">file name to be used for download</param>
        /// <param name="stream">file stream</param>
        /// <param name="contentType">content type (for file upload it must be text/html)</param>
        /// <returns>json result with the download url</returns>
        protected JsonResult JsonFileResult(string uniqueId, string fileName, Stream stream, string contentType = "application/json", string message = "")
        {
            stream.Seek(0, SeekOrigin.Begin);
            ReportResultDto fileResultDto = new ReportResultDto
            {
                FileName = fileName,
                ReportStream = stream
            };

            Session[uniqueId] = fileResultDto;

            return Json(new JsonResponse(true)
            {
                Message = message,
                Data = Url.RouteUrl("FileDownload", new { uniqueId = uniqueId })
            }, contentType, JsonRequestBehavior.AllowGet);
        }

        protected new JsonResult Json(object data)
        {
            return Json(data, "application/json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        protected new JsonResult Json(object data, string contentType)
        {
            return Json(data, contentType, System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        protected new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return Json(data, "application/json", System.Text.Encoding.UTF8, behavior);
        }

        protected new JsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
        {
            return Json(data, contentType, System.Text.Encoding.UTF8, behavior);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected FileResult FileDownload(string filePath, string fileName, string contentType = System.Net.Mime.MediaTypeNames.Application.Octet)
        {
            return File(filePath, contentType, Url.Encode(fileName));
        }

        //Get Model Description
        protected Dictionary<string, string> GetViewModelDescList<T>()
        {
            Dictionary<string, string> fieldNames = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DisplayAttribute attr = (DisplayAttribute)System.Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
                if (attr != null)
                {
                    fieldNames.Add(property.Name, attr.GetName());
                }
            }

            return fieldNames;
        }

        #region JsonReportResult

        public JsonResult JsonReportResult(string reportId, string fileName, Stream stream)
        {
            stream.Flush();
            stream.Position = 0;
            ReportResultDto reportResultDto = new ReportResultDto
            {
                FileName = fileName,
                ReportStream = stream
            };
            Session[reportId] = reportResultDto;

            return Json(new JsonResponse(true)
            {
                Data = Url.RouteUrl("ReportDownload", new { reportId = reportId })
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion JsonReportResult
    }
}