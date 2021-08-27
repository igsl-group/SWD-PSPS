using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.FdStatus;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Reports;
using Psps.Services.FlagDays;
using Psps.Services.Home;
using Psps.Services.Lookups;
using Psps.Services.PSPs;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.DisasterStatistics;
using Psps.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Home"), Route("{action=index}")]
    public class HomeController : BaseController
    {
        #region Var

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IPspService _pspService;
        private readonly IFdApprovalHistoryService _fdApprovalHistoryService;
        private readonly IHomeService _homeService;
        private readonly ILookupService _lookupService;
        private readonly IAclService _aclService;
        private readonly IAuthenticationService _authenticationService;

        #endregion Var

        #region Ctor

        public HomeController(IMessageService messageService, IUnitOfWork unitOfWork, IPspService pspService,
                            IFlagDayService flagDayService, IHomeService homeService, ILookupService lookupService,
                            IFdApprovalHistoryService fdApprovalHistoryService,
                            IAclService aclService, IAuthenticationService authenticationService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._pspService = pspService;
            this._homeService = homeService;
            this._lookupService = lookupService;
            this._fdApprovalHistoryService = fdApprovalHistoryService;
            this._aclService = aclService;
            this._authenticationService = authenticationService;
        }

        #endregion Ctor

        [HttpGet, Route("~/", Name = "Home")]
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            //dynamic complaint
            //var lookupCompEnqHeaders = _lookupService.GetAllLookupByType(LookupType.ComplaintRecordType);
            var listCompEnq = _homeService.GetLastFiveYrsCompEnqSummary();
            
            //lookupCompEnqHeaders.Remove("Others");
            //System.Collections.Generic.Dictionary<string, string> ()

            var lookupCompEnqHeaders = new Dictionary<string, string> ();
            //_lookupService.GetAllLookupByType(LookupType.ComplaintRecordType);

            lookupCompEnqHeaders.Add("01", "No. of Enquiry Received");
            lookupCompEnqHeaders.Add("02", "No. of Complaint Received");
            lookupCompEnqHeaders.Add("Others", "No. of Miscellaneous Correspondence Received");
            lookupCompEnqHeaders.Add("reminding", "No. of Reminding Letter Issued");
            lookupCompEnqHeaders.Add("alert", "No. of Alert Letter Issued");
            lookupCompEnqHeaders.Add("warning", "No. of Warning letter Issued");
            lookupCompEnqHeaders.Add("progress", "No. of Enquiry/Complaint In Progress");
            //lookupCompEnqHeaders.Add("null", "No. of Null Received");

            //dynamic suggestion
            var lookupSuggestionHeaders = _lookupService.GetAllLookupByType(LookupType.SuggestionNature);
            var listSuggestion = _homeService.GetLastFiveYrsSuggestionSummary();
            lookupSuggestionHeaders.Add("null", "No. of Null Received");

            model.DynCompEnqHeaders = lookupCompEnqHeaders;
            model.CompEnqRowData = listCompEnq;
            model.SuggestionHeaders = lookupSuggestionHeaders;
            model.SuggestionRowData = listSuggestion;

            var postIdToBeActed = _aclService.GetPostToBeActed(CurrentUser.UserId, DateTime.Today);
            if (CurrentUser.OriginalPostIdIfActed.IsNotNullOrEmpty())
            {
                if (!postIdToBeActed.Contains(CurrentUser.OriginalPostIdIfActed)) postIdToBeActed.Insert(0, CurrentUser.OriginalPostIdIfActed);
                postIdToBeActed.Remove(CurrentUser.PostId);
            }
            model.PostIdToBeActed = postIdToBeActed.ToDictionary(x => x, x => x);

            return View(model);
        }

        #region ReportDownload

        [HttpGet, Route("~/report/download/{reportId}", Name = "ReportDownload")]
        public ActionResult ReportDownload(string reportId)
        {
            var reportResultDto = Session[reportId] as ReportResultDto;
            if (reportResultDto == null || reportResultDto.ReportStream == null)
                return new EmptyResult();
            Session[reportId] = null;
            return File(reportResultDto.ReportStream, "application/pdf", reportResultDto.FileName);
        }

        #endregion ReportDownload

        #region REST-like API

        [HttpGet, Route("~/api/home/listDynamCompEnq", Name = "ListDynamCompEnq")]
        public ActionResult ListDynamCompEnq()
        {
            ////converting in grid format
            //var gridResult = new GridResult
            //{
            //    Data = (from u in fdPspStatus
            //            select new
            //            {
            //                Type = u.Type,
            //                Year = u.Year,
            //                DataInputStage = u.DataInputStage,
            //                ReadyForApproval = u.ReadyForApproval,
            //                Approved = u.Approved != 0 ? u.Approved : 0,
            //                MultiBatchApp = u.MultiBatchApp,
            //            }).ToArray()
            //};

            HomeViewModel model = new HomeViewModel();
            var lookupHeaders = _lookupService.GetAllLookupByType(LookupType.ComplaintRecordType);
            model.DynCompEnqHeaders = lookupHeaders;
            return View(model);
        }

        [HttpGet, Route("~/api/home/listPspFd", Name = "ListPspFd")]
        public JsonResult ListPspFd(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var fdPspStatus = _homeService.GetFdSatusSummary(grid, 2013);
            var result = _homeService.GetPspApplicationStatus(grid);
            ////converting in grid format
            var gridResult = new GridResult
            {
                Data = (from u in result
                        select new
                        {
                            Type = u.Type,
                            Year = u.Year,
                            ApplicationReceived = u.ApplicationReceived,
                            PSPIssued = u.PSPIssued,
                            RejectedApplication = u.RejectedApplication,
                            ApplicationWithdrawn = u.ApplicationWithdrawn,
                            TwoBatchApplication = u.TwoBatchApplication,
                            OverdueAC = u.OverdueAC,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/listSsaf", Name = "ListSsaf")]
        public JsonResult ListSsaf(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var fdPspStatus = _homeService.GetFdSatusSummary(grid, 2013);
            var result = _homeService.GetSsafApplicationStatus(grid);
            ////converting in grid format
            var gridResult = new GridResult
            {
                Data = (from u in result
                        select new
                        {
                            Type = u.Type,
                            Year = u.Year,
                            ApplicationReceived = u.ApplicationReceived,
                            PSPIssued = u.PSPIssued,
                            RejectedApplication = u.RejectedApplication,
                            ApplicationWithdrawn = u.ApplicationWithdrawn,
                            TwoBatchApplication = u.TwoBatchApplication,
                            OverdueAC = u.OverdueAC,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/ListCompEnq", Name = "ListCompEnq")]
        public JsonResult ListCompEnq(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var compEnq = _homeService.GetLastFiveYrsCompEnqSummary(grid);

            //////converting in grid format
            //var gridResult = new GridResult
            //{
            //    Data = (from u in compEnq
            //            select new
            //            {
            //                Year = u.Year,
            //                ComplaintRecieved = u.ComplaintRecieved,
            //                WarningLetterIssued = u.WarningLetterIssued,
            //                EnquiryRecieved = u.EnquiryRecieved,
            //                NARecieved = u.NARecieved,
            //                NullRecieved = u.NullRecieved,
            //            }).ToArray()
            //};

            var gridResult = new GridResult();

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/listSuggest", Name = "ListSuggestion")]
        public JsonResult ListSuggestion(GridSettings grid)
        {
            //Ensure.Argument.NotNull(grid);

            //var compEnqSuggest = _homeService.GetLastFiveYrsSuggestionSummary(grid);

            //////converting in grid format
            //var gridResult = new GridResult
            //{
            //    Data = (from u in compEnqSuggest
            //            select new
            //            {
            //                Year = u.Year,
            //                SuggestionRecieved = u.SuggestionRecieved,
            //                ComplimentRecieved = u.ComplimentRecieved,
            //                NARecieved = u.NARecieved,
            //                NullRecieved = u.NullRecieved,
            //            }).ToArray()
            //};

            var gridResult = new GridResult();

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/listPspBringUp", Name = "ListPspBringUp")]
        public JsonResult ListPspBringUp(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var pspBringUp = _homeService.GetPspBringUpSummary(grid);
            var pspBringUp = _homeService.GetPspBringUpSummaryView(grid);
            ////converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = pspBringUp.TotalPages,
                CurrentPageIndex = pspBringUp.CurrentPageIndex,
                TotalCount = pspBringUp.TotalCount,
                Data = (from u in pspBringUp
                        select new
                        {
                            Resubmit = u.Resubmit,
                            PspMasterId = u.PspMasterId,
                            PspRef = u.PspRef,
                            EngOrgName = u.EngOrgName,
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            PspEventDate = u.PspEventDate,
                            ProcessingOfficerPost = u.ProcessingOfficerPost,
                            SpecialRemark = u.SpecialRemark,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/listComplaintBringUp", Name = "ListComplaintBringUp")]
        public JsonResult ListComplaintBringUp(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            //var complaintBringUp = _homeService.GetComplaintBringUpSummary(grid);
            var complaintBringUp = _homeService.GetComplaintBringUpSummaryView(grid);

            ////converting in grid format
            var gridResult = new GridResult
            {
                Data = (from u in complaintBringUp
                        select new
                        {
                            complaintMasterId = u.ComplaintMasterId,
                            OrgRef = u.OrgRef,
                            EngChiOrgName = u.OtherEngOrgName,
                            ComplaintRef = u.ComplaintRef,
                            ComplaintDate = u.ComplaintDate,
                            Source = u.EngDescription,
                            PermitConcern = u.PermitConcern,
                            ActionFileEnclosureNum = u.ActionFileEnclosureNum
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/home/listFdStatus", Name = "ListFdStatus")]
        public JsonResult ListFdStatus(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var fdStatus = _fdApprovalHistoryService.GetFdStatus(grid);

            ////converting in grid format
            var gridResult = new GridResult
            {
                Data = fdStatus
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/home/post/{postId}/change", Name = "ChangePost")]
        public JsonResult ChangePost([CustomizeValidator(RuleSet = "default,ChangePost")]HomeViewModel model)
        {
            if (!ModelState.IsValid) return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);

            using (_unitOfWork.BeginTransaction())
            {
                _authenticationService.ChangePost(CurrentUser.UserId, model.ChangedPostId);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion REST-like API
    }
}