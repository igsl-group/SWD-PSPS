using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;

//using Psps.Models.Dto.Security;
//using Psps.Services.Accounts;
//using Psps.Services.Actings;
//using Psps.Services.Posts;
using Psps.Services.Disaster;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.Disaster;
using System;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly IDisasterMasterService _disasterMasterService;
        private readonly IDisasterStatisticsService _disasterStatisticsService;

        #endregion Var

        #region Ctor

        public DisasterController(
            IMessageService messageService,
            IAuthenticationService authenticationService,
            IDisasterStatisticsService DisasterStatisticsService,
            IDisasterMasterService DisasterMasterService,
            IUnitOfWork unitOfWork
            )
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._authenticationService = authenticationService;
            this._disasterStatisticsService = DisasterStatisticsService;
            this._disasterMasterService = DisasterMasterService;
            //this._postService = postService;
            //this._roleService = roleService;
            //this._actingService = actingService;
        }

        #endregion Ctor

        #region Disaster Maintenance

        [PspsAuthorize(Allow.ListDisaster)]
        [HttpGet]
        public ActionResult Index()
        {
            var disastersMaster = _disasterMasterService.getAllDisasterMasterForDropdown();

            DisasterViewModel model = new DisasterViewModel();
            model.DisasterMaster = disastersMaster;

            return View(model);
        }

        [PspsAuthorize(Allow.ListDisaster)]
        [HttpGet, Route("~/api/disaster/list", Name = "ListDisaster")]
        public JsonResult List(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var disasterStatistics = _disasterStatisticsService.GetPage(grid);
            var disaster = _disasterMasterService.GetPageWithDto(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = disaster.TotalPages,
                CurrentPageIndex = disaster.CurrentPageIndex,
                TotalCount = disaster.TotalCount,
                Data = (from u in disaster
                        select new
                        {
                            DisasterStatisticsId = u.DisasterStatisticsId,
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
                            DisasterMasterId = u.DisasterMasterId,
                            DisasterName = u.DisasterName,
                            BeginDate = u.BeginDate,
                            EndDate = u.EndDate
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        //[PspsAuthorize(Allow.ReadDisaster)]
        //[HttpGet, Route("~/api/disaster/{disasterStatisticsId}", Name = "GetDisaster")]
        //public JsonResult Get(int disasterId)
        //{
        //    Ensure.Argument.NotNull(disasterId);

        //    var disasterStatistics = _disasterStatisticsService.GetDisasterStatisticsById(disasterId);

        //    if (disasterStatistics == null)
        //    {
        //        return Json(new JsonResponse(false)
        //        {
        //            Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //    var disastersMaster = _disasterMasterService.getAllDisasterMasterForDropdown();

        //    var model = new updateDisasterViewModel()
        //    {
        //        DisasterStatisticsId = disasterStatistics.DisasterStatisticsId,
        //        PspApplicationProcedurePublicCount = disasterStatistics.PspApplicationProcedurePublicCount,
        //        PspApplicationProcedureOtherCount = disasterStatistics.PspApplicationProcedureOtherCount,
        //        PspScopePublicCount = disasterStatistics.PspScopePublicCount,
        //        PspScopeOtherCount = disasterStatistics.PspScopeOtherCount,
        //        PspApplicationStatusPublicCount = disasterStatistics.PspApplicationStatusPublicCount,
        //        PspApplicationStatusOthersCount = disasterStatistics.PspApplicationStatusOthersCount,
        //        PspPermitConditionCompliancePublicCount = disasterStatistics.PspPermitConditionCompliancePublicCount,
        //        PspPermitConditionComplianceOtherCount = disasterStatistics.PspPermitConditionComplianceOtherCount,
        //        OtherEnquiryPublicCount = disasterStatistics.OtherEnquiryPublicCount,
        //        OtherEnquiryOtherCount = disasterStatistics.OtherEnquiryOtherCount,
        //        DisasterMasterId = disasterStatistics.DisasterMaster.DisasterMasterId,
        //        DisasterName = disasterStatistics.DisasterMaster.DisasterName,
        //        BeginDate = disasterStatistics.DisasterMaster.BeginDate,
        //        EndDate = disasterStatistics.DisasterMaster.EndDate,
        //        DisasterMaster = disastersMaster
        //    };

        //    return Json(new JsonResponse(true)
        //    {
        //        Data = model
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //[PspsAuthorize(Allow.CreateDisaster)]
        //[ValidateAntiForgeryToken]
        //[HttpPost, Route("~/api/disaster/new", Name = "NewDisaster")]
        //public JsonResult New(CreateDisasterViewModel model)
        //{
        //    Ensure.Argument.NotNull(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
        //    }

        //    var DisasterInfo = AutoMapper.Mapper.Map<CreateDisasterViewModel, DisasterInfoDto>(model);

        //    using (_unitOfWork.BeginTransaction())
        //    {
        //        _disasterStatisticsService.CreateDisasterStatistics(DisasterInfo);
        //        _unitOfWork.Commit();
        //    }

        //    return Json(new JsonResponse(true)
        //    {
        //        Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
        //    }, JsonRequestBehavior.DenyGet);
        //}

        //[PspsAuthorize(Allow.EditDisaster)]
        //[ValidateAntiForgeryToken]
        //[HttpPost, Route("~/api/disaster/{disasterStatisticsId}/edit", Name = "EditDisaster")]
        //public JsonResult Edit(int disasterStatisticsId, updateDisasterViewModel model)
        //{
        //    Ensure.Argument.NotNull(disasterStatisticsId);
        //    Ensure.Argument.NotNull(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
        //    }

        //    using (_unitOfWork.BeginTransaction())
        //    {
        //        var disasterInfo = AutoMapper.Mapper.Map<updateDisasterViewModel, DisasterInfoDto>(model);

        //        _disasterStatisticsService.UpdateDisasterStatistics(disasterInfo);
        //        _unitOfWork.Commit();
        //    }

        //    return Json(new JsonResponse(true)
        //    {
        //        Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
        //    }, JsonRequestBehavior.DenyGet);
        //}

        #endregion Disaster Maintenance
    }
}