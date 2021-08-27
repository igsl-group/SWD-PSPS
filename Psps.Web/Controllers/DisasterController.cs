using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;
using Psps.Services.Disaster;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Disaster;
using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Disaster"), Route("{action=index}")]
    public class DisasterController : BaseController
    {
        #region Var

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IDisasterMasterService _disasterMasterService;
        private readonly IDisasterStatisticsService _DisasterStatisticsService;
        private readonly IAuthenticationService _authenticationService;

        #endregion Var

        #region Ctor

        public DisasterController(
            IMessageService messageService,
            IAuthenticationService authenticationService,
            IDisasterMasterService DisasterMasterService,
            IDisasterStatisticsService DisasterStatisticsService,
            IUnitOfWork unitOfWork
            )
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._disasterMasterService = DisasterMasterService;
            this._DisasterStatisticsService = DisasterStatisticsService;
            this._authenticationService = authenticationService;
        }

        #endregion Ctor

        #region Disaster Maintenance

        [PspsAuthorize(Allow.DisasterMaster)]
        [HttpGet]
        [RuleSetForClientSideMessagesAttribute("default", "CreateDisaster", "UpdateDisaster")]
        public ActionResult Index()
        {
            DisasterViewModel model = new DisasterViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.DisasterMaster)]
        [HttpGet, Route("~/api/disaster/list", Name = "ListDisaster")]
        public JsonResult List(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var disaster = _disasterMasterService.GetPageWithDto(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = disaster.TotalPages,
                CurrentPageIndex = disaster.CurrentPageIndex,
                TotalCount = disaster.TotalCount,
                Data = (from u in disaster
                        orderby u.BeginDate descending
                        select new
                        {
                            DisasterMasterId = u.DisasterMasterId,
                            PspApplicationProcedurePublicCount = u.PspApplicationProcedurePublicCount,
                            PspApplicationProcedureOtherCount = u.PspApplicationProcedureOtherCount,
                            PspScopePublicCount = u.PspScopePublicCount,
                            PspScopeOtherCount = u.PspScopeOtherCount,
                            PspApplicationStatusPublicCount = u.PspApplicationStatusPublicCount,
                            PspApplicationStatusOthersCount = u.PspApplicationStatusOthersCount,
                            PspPermitConditionCompliancePublicCount = u.PspPermitConditionCompliancePublicCount,
                            PspPermitConditionComplianceOtherCount = u.PspPermitConditionComplianceOtherCount,
                            OtherEnquiryPublicCount = u.OtherEnquiryPublicCount,
                            OtherEnquiryOtherCount = u.OtherEnquiryOtherCount,
                            DisasterName = u.DisasterName,
                            ChiDisasterName = u.ChiDisasterName,
                            DisasterDate = u.DisasterDate,
                            BeginDate = u.BeginDate,
                            EndDate = u.EndDate,
                            SubTotal = u.SubTotal
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterMaster)]
        [HttpGet, Route("~/api/disaster/{disasterMasterId}", Name = "GetDisaster")]
        public JsonResult Get(int disasterMasterId)
        {
            Ensure.Argument.NotNull(disasterMasterId);

            var disastersMaster = _disasterMasterService.GetDisasterMasterById(disasterMasterId);

            if (disastersMaster == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            if (disastersMaster == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            DateTime? nullDate = (DateTime?)null;

            var model = new DisasterViewModel
            {
                //DisasterMasterId = disastersMaster != null ? disastersMaster.DisasterMasterId : 0,
                DisasterMasterId = disastersMaster.DisasterMasterId,
                DisasterName = disastersMaster.DisasterName,
                ChiDisasterName = disastersMaster.ChiDisasterName,
                DisasterDate = disastersMaster.DisasterDate != null ? disastersMaster.DisasterDate : nullDate,
                BeginDate = disastersMaster.BeginDate,
                EndDate = disastersMaster.EndDate != null ? disastersMaster.EndDate : nullDate,
                RowVersion = disastersMaster.RowVersion
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/disaster/new", Name = "NewDisaster")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,CreateDisaster")] DisasterViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //DateTime outBeginDate = new DateTime();
            //DateTime d = new DateTime();
            //Nullable<DateTime> outEndDate = null;
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.BeginDate, "d/M/yyyy", enUS, DateTimeStyles.None, out outBeginDate);
            //if (!string.IsNullOrEmpty(model.EndDate))
            //    DateTime.TryParseExact(model.EndDate, "d/M/yyyy", enUS, DateTimeStyles.None, out d);
            //if (d != default(DateTime))
            //    outEndDate = d;
            //else
            //    outEndDate = null;

            var disasterMaster = new DisasterMaster();

            disasterMaster.DisasterName = model.DisasterName;
            disasterMaster.ChiDisasterName = model.ChiDisasterName;
            disasterMaster.DisasterDate = model.DisasterDate;
            disasterMaster.BeginDate = model.BeginDate;
            disasterMaster.EndDate = model.EndDate;

            using (_unitOfWork.BeginTransaction())
            {
                _disasterMasterService.CreateDisasterMaster(disasterMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.DisasterMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/disaster/delete/{disasterMasterId}", Name = "DeleteDisaster")]
        public JsonResult Delete([CustomizeValidator(RuleSet = "default,DeleteDisaster")] DisasterViewModel model, int disasterMasterId)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var disasterMaster = _disasterMasterService.GetDisasterMasterById(disasterMasterId);

            using (_unitOfWork.BeginTransaction())
            {
                _DisasterStatisticsService.deleteDisasterStatisticsByMasterId(disasterMasterId);
                _disasterMasterService.delete(disasterMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.DisasterMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/disaster/{disasterMasterId}/edit", Name = "EditDisaster")]
        public JsonResult Edit(int disasterMasterId, [CustomizeValidator(RuleSet = "default,UpdateDisaster")] DisasterViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var disasterMaster = _disasterMasterService.GetDisasterMasterById(disasterMasterId);

            Ensure.NotNull(disasterMaster, "No disaster master found with the specified id");

            //DateTime outBeginDate;
            //DateTime d = new DateTime();
            //Nullable<DateTime> outEndDate = null;
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.BeginDate, "d/M/yyyy", enUS, DateTimeStyles.None, out outBeginDate);
            //if (!string.IsNullOrEmpty(model.EndDate))
            //    DateTime.TryParseExact(model.EndDate, "d/M/yyyy", enUS, DateTimeStyles.None, out d);
            //if (d != default(DateTime))
            //    outEndDate = d;
            //else
            //    outEndDate = null;
            if (model != null)
            {
                disasterMaster.DisasterName = model.DisasterName;
                disasterMaster.ChiDisasterName = model.ChiDisasterName;
                disasterMaster.DisasterDate = model.DisasterDate;
                disasterMaster.BeginDate = model.BeginDate;
                disasterMaster.EndDate = model.EndDate;
                disasterMaster.RowVersion = model.RowVersion;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _disasterMasterService.UpdateDisasterMaster(disasterMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion Disaster Maintenance
    }
}