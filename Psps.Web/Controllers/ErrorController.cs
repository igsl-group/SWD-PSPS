using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ActionResult Index(Exception exception)
        {
            Response.TrySkipIisCustomErrors = true;
            int statusCode = GetStatusCode(exception);

            switch (statusCode)
            {
                case 401:
                    return Unauthorised();

                case 403:
                    return AccessDenied();

                case 404:
                    return NotFound();

                default:
                    return InternalServerError(exception);
            }
        }

        public ActionResult InternalServerError(Exception exception)
        {
            string message = null;
            StringBuilder messageBuilder = new StringBuilder();
            if (exception is AggregateException)
            {
                (exception as AggregateException).Flatten().Handle((x) =>
                {
                    messageBuilder.AppendLine(x.GetBaseException().Message);
                    return true;
                });
                message = messageBuilder.ToString();
            }
            else
                message = exception.GetBaseException().Message;

            var model = new ErrorViewModel
            {
                Message = message
            };

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (IsAjaxRequest)
                return Json(new JsonResponse(false)
                {
                    Message = message
                }, JsonRequestBehavior.AllowGet);

            //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, model.Message);

            return View(model);
        }

        public ActionResult Unauthorised()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var model = new ErrorViewModel
            {
                Message = "Your session has expired. Please login again to continue."
            };

            if (IsAjaxRequest)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, model.Message);

            return View(model);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            var model = new ErrorViewModel
            {
                Message = "The resource you are looking for has been removed, had its name changed, or is temporarily unavailable."
            };

            if (IsAjaxRequest)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, model.Message);

            return View("NotFound", model);
        }

        public ActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var model = new ErrorViewModel
            {
                Message = "You do not have permission to perform this action."
            };

            if (IsAjaxRequest)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, model.Message);

            return View("AccessDenied", model);
        }

        private int GetStatusCode(Exception exception)
        {
            var httpException = exception as HttpException;
            return httpException != null ? httpException.GetHttpCode() : (int)HttpStatusCode.InternalServerError;
        }
    }
}