using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Posts;
using Psps.Services.Ranks;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Posts;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [RoutePrefix("DummyApi")]
    public class DummyApiController : BaseController
    {
        public DummyApiController()
        {
        }

        [HttpPost, Route("~/api/swd/organisation", Name = "CreateFRASOrganisation")]
        public ActionResult CreateFRASOrganisation()
        {
            return Content(new Random().Next(999999).ToString());
        }

        #region FRAS Organisation

        [HttpPost, Route("~/api-error/swd/organisation", Name = "CreateFRASOrganisationError")]
        public ActionResult CreateFRASOrganisationError()
        {
            this.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new
            {
                errorList = new[] {
                    "Organisation already exists. Re-enable instead of creating new by using HTTP PUT",
                    "Organisation Abbreviation exceeds 20 characters limit",
                    "Error detected during validation, organisation not saved"
                },
                sessionId = "OK5C0Ip3bHQFZXNqKcSCz42d.node1"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPut, Route("~/api/swd/organisation", Name = "UpdateFRASOrganisation")]
        public ActionResult UpdateFRASOrganisation()
        {
            return Content("updated");
        }

        [HttpPut, Route("~/api-error/swd/organisation", Name = "UpdateFRASOrganisationError")]
        public ActionResult UpdateFRASOrganisationError()
        {
            this.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new
            {
                errorList = new[] {
                    "Organisation already exists. Re-enable instead of creating new by using HTTP PUT",
                    "Organisation Abbreviation exceeds 20 characters limit",
                    "Error detected during validation, organisation not saved"
                },
                sessionId = "OK5C0Ip3bHQFZXNqKcSCz42d.node1"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpDelete, Route("~/api/swd/organisation/{orgId}", Name = "DeleteFRASOrganisation")]
        public ActionResult DeleteFRASOrganisation(int orgId)
        {
            return Content("deleted");
        }

        #endregion FRAS Organisation

        #region FRAS Activity

        [HttpPost, Route("~/api/swd/activity", Name = "CreateFRASActivity")]
        public ActionResult CreateFRASActivity()
        {
            return Content(new Random().Next(999999).ToString());
        }

        [HttpPost, Route("~/api-error/swd/activity", Name = "CreateFRASActivityError")]
        public ActionResult CreateFRASActivityError()
        {
            this.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new
            {
                errorList = new[] {
                    "Activity already exists. Re-enable instead of creating new by using HTTP PUT",
                    "Activity Abbreviation exceeds 20 characters limit",
                    "Error detected during validation, organisation not saved"
                },
                sessionId = "OK5C0Ip3bHQFZXNqKcSCz42d.node1"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPut, Route("~/api/swd/activity", Name = "UpdateFRASActivity")]
        public ActionResult UpdateFRASActivity()
        {
            return Content("updated");
        }

        [HttpPut, Route("~/api-error/swd/activity", Name = "UpdateFRASActivityError")]
        public ActionResult UpdateFRASActivityError()
        {
            this.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new
            {
                errorList = new[] {
                    "Organisation already exists. Re-enable instead of creating new by using HTTP PUT",
                    "Organisation Abbreviation exceeds 20 characters limit",
                    "Error detected during validation, organisation not saved"
                },
                sessionId = "OK5C0Ip3bHQFZXNqKcSCz42d.node1"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpDelete, Route("~/api/swd/activity/{eventId}/", Name = "DeleteFRASActivity")]
        public ActionResult DeleteFRASActivity(string eventId)
        {
            return Content("deleted");
        }

        [HttpDelete, Route("~/api-error/swd/activity/{eventId}", Name = "DeleteFRASActivityError")]
        public ActionResult DeleteFRASActivityError(string eventId)
        {
            this.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new
            {
                errorList = new[] {
                    "Hello World!!!"
                },
                sessionId = "OK5C0Ip3bHQFZXNqKcSCz42d.node1"
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion FRAS Activity
    }
}