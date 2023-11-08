using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Disaster;
using Psps.Services.Posts;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.DisasterStatistics;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Psps.Services.UserLog;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("DisasterStatistics"), Route("{action=index}")]
    public class DisasterStatisticsController : BaseController
    {
        #region Var

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IDisasterStatisticsService _disasterStatisticsService;
        private readonly IPostService _postService;
        private readonly IDisasterMasterService _disasterMasterService;
        private readonly IUserLogService _userLogService;

        #endregion Var

        #region Ctor

        public DisasterStatisticsController(
            IMessageService messageService,
            IAuthenticationService authenticationService,
            IDisasterStatisticsService DisasterStatisticsService,
            IPostService PostService,
            IDisasterMasterService DisasterMasterService,
            IUnitOfWork unitOfWork,
            IUserLogService userLogService
            )
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._disasterStatisticsService = DisasterStatisticsService;
            this._postService = PostService;
            this._disasterMasterService = DisasterMasterService;
            this._userLogService = userLogService;
        }

        #endregion Ctor

        [PspsAuthorize(Allow.DisasterStat)]
        [HttpGet]
        [RuleSetForClientSideMessagesAttribute("default", "CreateDisasterStatistics", "UpdateDisasterStatistics")]
        public ActionResult Index()
        {
            var posts = _postService.GetAllPostsForDropdown();
            var disasterMasterNames = _disasterMasterService.GetAllDisasterMasterForDropdown();

            DisasterStatisticsViewModel model = new DisasterStatisticsViewModel();
            model.RecordPostIds = posts;
            model.DisasterNames = disasterMasterNames;
            return View(model);
        }

        #region REST-like API

        [PspsAuthorize(Allow.DisasterStat)]
        [HttpGet, Route("~/api/disasterStatistics/{disasterMasterId}/{recordPostId}/list", Name = "ListDisasterStatistics")]
        public JsonResult List(int disasterMasterId, string recordPostId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "disasterMaster.disasterMasterId",
                data = disasterMasterId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            grid.AddDefaultRule(new Rule()
            {
                field = "recordPostId",
                data = recordPostId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var disasterStatistics = _disasterStatisticsService.GetPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = disasterStatistics.TotalPages,
                CurrentPageIndex = disasterStatistics.CurrentPageIndex,
                TotalCount = disasterStatistics.TotalCount,
                Data = (from u in disasterStatistics
                        select new
                        {
                            DisasterStatisticsId = u.DisasterStatisticsId,
                            DisasterMasterId = u.DisasterMaster.DisasterMasterId,
                            RecordPostId = u.RecordPostId,
                            RecordDate = u.RecordDate,
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
                            SubTotal = u.PspApplicationProcedurePublicCount + u.PspApplicationProcedureOtherCount +
                                        u.PspScopePublicCount + u.PspScopeOtherCount +
                                        u.PspApplicationStatusPublicCount + u.PspApplicationStatusOthersCount +
                                        u.PspPermitConditionCompliancePublicCount + u.PspPermitConditionComplianceOtherCount +
                                        u.OtherEnquiryPublicCount + u.OtherEnquiryOtherCount
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterStat)]
        [HttpGet, Route("~/api/disasterStatistics/{disasterMasterId}/list", Name = "ListDisasterStatisticsFilterNameOnly")]
        public JsonResult ListDisasterStatisticsFilterNameOnly(int disasterMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "disasterMaster.disasterMasterId",
                data = disasterMasterId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var disasterStatistics = _disasterStatisticsService.GetPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = disasterStatistics.TotalPages,
                CurrentPageIndex = disasterStatistics.CurrentPageIndex,
                TotalCount = disasterStatistics.TotalCount,
                Data = (from u in disasterStatistics
                        select new
                        {
                            DisasterStatisticsId = u.DisasterStatisticsId,
                            DisasterMasterId = u.DisasterMaster.DisasterMasterId,
                            RecordPostId = u.RecordPostId,
                            RecordDate = u.RecordDate,
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
                            SubTotal = u.PspApplicationProcedurePublicCount + u.PspApplicationProcedureOtherCount +
                                        u.PspScopePublicCount + u.PspScopeOtherCount +
                                        u.PspApplicationStatusPublicCount + u.PspApplicationStatusOthersCount +
                                        u.PspPermitConditionCompliancePublicCount + u.PspPermitConditionComplianceOtherCount +
                                        u.OtherEnquiryPublicCount + u.OtherEnquiryOtherCount
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterStat)]
        [HttpGet, Route("~/api/disasterStatistics/{disasterStatisticsId}", Name = "GetDisasterStatistics")]
        public JsonResult Get(int disasterStatisticsId)
        {
            var disastersStatistics = _disasterStatisticsService.GetDisasterStatisticsById(disasterStatisticsId);

            if (disastersStatistics == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            DateTime? nullDate = (DateTime?)null;

            var model = new DisasterStatisticsViewModel
            {
                DisasterStatisticsId = disastersStatistics.DisasterStatisticsId,
                DisasterMasterId = disastersStatistics.DisasterMaster.DisasterMasterId,
                RecordPostId = disastersStatistics.RecordPostId,
                RecordDate = disastersStatistics.RecordDate,
                OtherEnquiryOtherCount = disastersStatistics.OtherEnquiryOtherCount != null ? disastersStatistics.OtherEnquiryOtherCount : 0,
                OtherEnquiryPublicCount = disastersStatistics.OtherEnquiryPublicCount != null ? disastersStatistics.OtherEnquiryPublicCount : 0,
                PspApplicationProcedureOtherCount = disastersStatistics.PspApplicationProcedureOtherCount != null ? disastersStatistics.PspApplicationProcedureOtherCount : 0,
                PspApplicationProcedurePublicCount = disastersStatistics.PspApplicationProcedurePublicCount != null ? disastersStatistics.PspApplicationProcedurePublicCount : 0,
                PspApplicationStatusOthersCount = disastersStatistics.PspApplicationStatusOthersCount != null ? disastersStatistics.PspApplicationStatusOthersCount : 0,
                PspApplicationStatusPublicCount = disastersStatistics.PspApplicationStatusPublicCount != null ? disastersStatistics.PspApplicationStatusPublicCount : 0,
                PspPermitConditionComplianceOtherCount = disastersStatistics.PspPermitConditionComplianceOtherCount != null ? disastersStatistics.PspPermitConditionComplianceOtherCount : 0,
                PspPermitConditionCompliancePublicCount = disastersStatistics.PspPermitConditionCompliancePublicCount != null ? disastersStatistics.PspPermitConditionCompliancePublicCount : 0,
                PspScopeOtherCount = disastersStatistics.PspScopeOtherCount != null ? disastersStatistics.PspScopeOtherCount : 0,
                PspScopePublicCount = disastersStatistics.PspScopePublicCount != null ? disastersStatistics.PspScopePublicCount : 0,
                BeginDate = disastersStatistics.DisasterMaster.BeginDate,
                EndDate = disastersStatistics.DisasterMaster.EndDate,

                RowVersion = disastersStatistics.RowVersion
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterStat)]
        [HttpGet, Route("~/api/disasterStatistics/{disasterMasterId}/getBeingEndDate", Name = "GetDisasterStatisticsBeginEndDate")]
        public JsonResult GetBeginEndDate(int disasterMasterId)
        {
            var disasterMaster = _disasterMasterService.GetDisasterMasterById(disasterMasterId);

            if (disasterMaster == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            DateTime? nullDate = (DateTime?)null;

            var model = new DisasterStatisticsViewModel
            {
                BeginDate = disasterMaster.BeginDate,
                EndDate = disasterMaster.EndDate
            };

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.DisasterStat)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/disasterStatistics/{recordPostId}/new", Name = "NewDisasterStatistics")]
        public JsonResult New(string recordPostId, [CustomizeValidator(RuleSet = "default,CreateDisasterStatistics")] DisasterStatisticsViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //DateTime outRecordDate = new DateTime();
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.RecordDate.ToString, "d/M/yyyy", enUS, DateTimeStyles.None, out outRecordDate);
            DateTime? nullDate = (DateTime?)null;

            var disasterStatistics = new DisasterStatistics();
            var disasterMaster = _disasterMasterService.GetDisasterMasterById(model.DisasterMasterId);

            if (model != null)
            {
                disasterStatistics.RecordPostId = model.RecordPostId;
                disasterStatistics.RecordDate = model.RecordDate;
                disasterStatistics.DisasterMaster = disasterMaster;
                disasterStatistics.OtherEnquiryOtherCount = model.OtherEnquiryOtherCount.HasValue ? model.OtherEnquiryOtherCount : 0;
                disasterStatistics.OtherEnquiryPublicCount = (decimal?)model.OtherEnquiryPublicCount != null ? model.OtherEnquiryPublicCount : 0;
                disasterStatistics.PspApplicationProcedureOtherCount = (decimal?)model.PspApplicationProcedureOtherCount != null ? model.PspApplicationProcedureOtherCount : 0;
                disasterStatistics.PspApplicationProcedurePublicCount = (decimal?)model.PspApplicationProcedurePublicCount != null ? model.PspApplicationProcedurePublicCount : 0;
                disasterStatistics.PspApplicationStatusOthersCount = (decimal?)model.PspApplicationStatusOthersCount != null ? model.PspApplicationStatusOthersCount : 0;
                disasterStatistics.PspApplicationStatusPublicCount = (decimal?)model.PspApplicationStatusPublicCount != null ? model.PspApplicationStatusPublicCount : 0;
                disasterStatistics.PspPermitConditionComplianceOtherCount = (decimal?)model.PspPermitConditionComplianceOtherCount != null ? model.PspPermitConditionComplianceOtherCount : 0;
                disasterStatistics.PspPermitConditionCompliancePublicCount = (decimal?)model.PspPermitConditionCompliancePublicCount != null ? model.PspPermitConditionCompliancePublicCount : 0;
                disasterStatistics.PspScopeOtherCount = (decimal?)model.PspScopeOtherCount != null ? model.PspScopeOtherCount : 0;
                disasterStatistics.PspScopePublicCount = (decimal?)model.PspScopePublicCount != null ? model.PspScopePublicCount : 0;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _disasterStatisticsService.CreateDisasterStatistics(disasterStatistics);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.DisasterStat)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/disasterStatistics/{disasterStatisticsId}/edit", Name = "EditDisasterStatistics")]
        public JsonResult Edit(int disasterStatisticsId, [CustomizeValidator(RuleSet = "default,UpdateDisasterStatistics")] DisasterStatisticsViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var disasterMaster = _disasterMasterService.GetDisasterMasterById(model.DisasterMasterId);
            var disasterStatistics = _disasterStatisticsService.GetDisasterStatisticsById(disasterStatisticsId);

            disasterStatistics.DisasterMaster = disasterMaster;

            Ensure.NotNull(disasterMaster, "No disaster master found with the specified id");

            //DateTime outRecordDate = new DateTime();
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime.TryParseExact(model.RecordDate, "d/M/yyyy", enUS, DateTimeStyles.None, out outRecordDate);

            if (model != null)
            {
                disasterStatistics.RecordPostId = model.RecordPostId;
                disasterStatistics.RecordDate = model.RecordDate;
                disasterStatistics.DisasterMaster = disasterMaster;
                disasterStatistics.OtherEnquiryOtherCount = (decimal?)model.OtherEnquiryOtherCount != null ? model.OtherEnquiryOtherCount : 0;
                disasterStatistics.OtherEnquiryPublicCount = (decimal?)model.OtherEnquiryPublicCount != null ? model.OtherEnquiryPublicCount : 0;
                disasterStatistics.PspApplicationProcedureOtherCount = (decimal?)model.PspApplicationProcedureOtherCount != null ? model.PspApplicationProcedureOtherCount : 0;
                disasterStatistics.PspApplicationProcedurePublicCount = (decimal?)model.PspApplicationProcedurePublicCount != null ? model.PspApplicationProcedurePublicCount : 0;
                disasterStatistics.PspApplicationStatusOthersCount = (decimal?)model.PspApplicationStatusOthersCount != null ? model.PspApplicationStatusOthersCount : 0;
                disasterStatistics.PspApplicationStatusPublicCount = (decimal?)model.PspApplicationStatusPublicCount != null ? model.PspApplicationStatusPublicCount : 0;
                disasterStatistics.PspPermitConditionComplianceOtherCount = (decimal?)model.PspPermitConditionComplianceOtherCount != null ? model.PspPermitConditionComplianceOtherCount : 0;
                disasterStatistics.PspPermitConditionCompliancePublicCount = (decimal?)model.PspPermitConditionCompliancePublicCount != null ? model.PspPermitConditionCompliancePublicCount : 0;
                disasterStatistics.PspScopeOtherCount = (decimal?)model.PspScopeOtherCount != null ? model.PspScopeOtherCount : 0;
                disasterStatistics.PspScopePublicCount = (decimal?)model.PspScopePublicCount != null ? model.PspScopePublicCount : 0;
                disasterStatistics.RowVersion = model.RowVersion;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _disasterStatisticsService.UpdateDisasterStatistics(disasterStatistics);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}