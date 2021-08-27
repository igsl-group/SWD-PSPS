using CsvHelper;
using CsvHelper.Configuration;
using DocxGenerator.Library;
using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Organisation;
using Psps.Models.Dto.Reports;
using Psps.Services.ComplaintMasters;
using Psps.Services.Complaints;
using Psps.Services.FlagDays;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.PSPs;
using Psps.Services.Report;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.Mappings;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.Complaint;
using Psps.Web.ViewModels.Lookup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Complaint"), Route("{action=index}")]
    public class ComplaintController : BaseController
    {
        #region "Field & Construtor"

        private readonly static string ENQUIRY_COMPLAINT_SEARCH_SESSION = Constant.Session.ENQUIRY_COMPLAINT_SEARCH_SESSION;
        private readonly String dateFormat = @"dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILookupService _lookupService;
        private readonly IComplaintMasterService _complaintMasterService;
        private readonly IComplaintDocService _complaintDocService;
        private readonly IParameterService _parameterService;
        private readonly IPspApprovalHistoryService _pspApprovalHistoryService;
        private readonly IFdApprovalHistoryService _fdApprovalHistoryService;
        private readonly IOrganisationService _organisationService;
        private readonly IComplaintAttachmentService _complaintAttachmentService;
        private readonly IReportService _reportService;
        private readonly IFdEventService _fdEventService;
        private readonly IComplaintTelRecordService _complaintTelRecordService;
        private readonly IComplaintFollowUpActionService _complaintFollowUpActionService;
        private readonly IComplaintPoliceCaseService _complaintPoliceCaseService;
        private readonly IComplaintOtherDepartmentEnquiryService _complaintOtherDepartmentEnquiryService;
        private readonly IComplaintResultService _complaintResultService;
        private readonly IWithholdingHistoryService _withholdingHistoryService;
        private readonly IOrgEditLatestPspFdViewRepository _orgEditLatestPspFdViewRepository;
        private readonly IDictionary<string, string> compResults;
        private readonly IDictionary<string, string> nonCompliances;

        public ComplaintController(IUnitOfWork unitOfWork, IMessageService messageService,
            ILookupService lookupService, IComplaintMasterService complaintMasterService, IComplaintDocService complaintDocService,
            IParameterService parameterService, IOrganisationService organisationService, IPspApprovalHistoryService pspApprovalHistoryService,
            IFdApprovalHistoryService fdApprovalHistoryService, IComplaintAttachmentService complaintAttachmentService,
            IReportService reportService, IFdEventService fdEventService, IComplaintTelRecordService complaintTelRecordService,
            IComplaintFollowUpActionService complaintFollowUpActionService, IComplaintPoliceCaseService complaintPoliceCaseService,
            IComplaintOtherDepartmentEnquiryService complaintOtherDepartmentEnquiryService, IComplaintResultService complaintResultService,
            IWithholdingHistoryService withholdingHistoryService, IOrgEditLatestPspFdViewRepository orgEditLatestPspFdViewRepository)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._complaintMasterService = complaintMasterService;
            this._complaintDocService = complaintDocService;
            this._parameterService = parameterService;
            this._organisationService = organisationService;
            this._pspApprovalHistoryService = pspApprovalHistoryService;
            this._fdApprovalHistoryService = fdApprovalHistoryService;
            this._complaintAttachmentService = complaintAttachmentService;
            this._reportService = reportService;
            this._fdEventService = fdEventService;
            this._complaintTelRecordService = complaintTelRecordService;
            this._complaintFollowUpActionService = complaintFollowUpActionService;
            this._complaintPoliceCaseService = complaintPoliceCaseService;
            this._complaintOtherDepartmentEnquiryService = complaintOtherDepartmentEnquiryService;
            this._complaintResultService = complaintResultService;
            this._withholdingHistoryService = withholdingHistoryService;
            this._orgEditLatestPspFdViewRepository = orgEditLatestPspFdViewRepository;
            this.compResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintResult);
            if (compResults.Count == 0) compResults.Add("", "");
            this.nonCompliances = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            if (nonCompliances.Count == 0) nonCompliances.Add("", "");
        }

        #endregion "Field & Construtor"

        #region "Index"

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route]
        public ActionResult Index()
        {
            ComplaintMasterViewModel model = new ComplaintMasterViewModel();

            return View(model);
        }

        #endregion "Index"

        #region "Search"

        [PspsAuthorize(Allow.ComplaintMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            this.HttpContext.Session[ENQUIRY_COMPLAINT_SEARCH_SESSION] = null;
            EnquiryComplaintSearchViewModel model = new EnquiryComplaintSearchViewModel();
            model.isFirstSearch = true;
            model.OrgStatus = this._lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            model.SubventedIndicators = this._lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.RegistrationTypes = this._lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.RecordTypes = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            model.ComplaintSources = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            model.ActivityConcerns = this._lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            model.NonComplianceNatures = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.ProcessStatus = this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus); ;
            model.ComplaintResults = compResults;
            model.WithholdingRemarks = this._lookupService.getAllLkpInCodec(LookupType.ComplaintWithholdingRemarks);
            model.CollectionMethods = this._lookupService.getAllLkpInCodec(LookupType.ComplaintCollectionMethod);
            model.YesNos = this._lookupService.getAllLkpInCodec(LookupType.YesNo);
            return View(model);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("~/Complaint/ReturnSearch", Name = "EnquiryComplaintSearchPage")]
        public ActionResult ReturnSearch()
        {
            EnquiryComplaintSearchViewModel model = new EnquiryComplaintSearchViewModel();
            var session = this.HttpContext.Session[ENQUIRY_COMPLAINT_SEARCH_SESSION];
            if (session != null)
            {
                model = ((EnquiryComplaintSearchViewModel)(this.HttpContext.Session[ENQUIRY_COMPLAINT_SEARCH_SESSION]));
                model.isFirstSearch = false;
            }

            model.OrgStatus = this._lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            model.SubventedIndicators = this._lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.RegistrationTypes = this._lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.RecordTypes = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            model.ComplaintSources = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            model.ActivityConcerns = this._lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            model.NonComplianceNatures = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.ProcessStatus = this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);
            model.ComplaintResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintResult);
            model.WithholdingRemarks = this._lookupService.getAllLkpInCodec(LookupType.ComplaintWithholdingRemarks);
            model.CollectionMethods = this._lookupService.getAllLkpInCodec(LookupType.ComplaintCollectionMethod);
            model.YesNos = this._lookupService.getAllLkpInCodec(LookupType.YesNo);
            return View("Search", model);
        }

        [HttpGet, Route("~/api/ComplaintRef/search", Name = "SearchComplaintRef")]
        public JsonResult SearchComplaintRef(string searchTerm, int pageSize, int pageNum)
        {
            GridSettings grid = new GridSettings
            {
                PageIndex = pageNum,
                PageSize = pageSize
            };
            grid.AddDefaultRule(new List<Rule> {
                new Rule { data = searchTerm, field = "ComplaintRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "OrgMaster.OrgRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "OrgMaster.EngOrgName", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "OrgMaster.ChiOrgName", op = WhereOperation.Contains.ToEnumValue()},
            }, GroupOp.OR);

            var complaints = _complaintMasterService.GetPage(grid);
            var complaintRecordType = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            var complaintSource = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);

            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    Total = complaints.TotalCount,
                    Results = (from c in complaints
                               select new
                               {
                                   id = c.ComplaintMasterId,
                                   ComplaintMasterId = c.ComplaintMasterId,
                                   ComplaintRef = c.ComplaintRef,
                                   ComplaintRecordType = complaintRecordType[c.ComplaintRecordType],
                                   ComplaintSource = complaintSource[c.ComplaintSource],
                                   ComplaintDate = c.ComplaintDate == null ? "" : c.ComplaintDate.Value.ToString("dd/MM/yyyy"),
                                   OrgRef = c.OrgMaster == null ? "" : c.OrgMaster.OrgRef,
                                   EngOrgName = c.OrgMaster == null ? "" : c.OrgMaster.EngOrgName,
                                   ChiOrgName = c.OrgMaster == null ? "" : c.OrgMaster.ChiOrgName
                               }).ToArray()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/ComplaintRef/{complaintMasterId}", Name = "GetComplaintRef")]
        public JsonResult GetComplaintRef(string complaintMasterId)
        {
            Ensure.Argument.NotNullOrEmpty(complaintMasterId);
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(Convert.ToInt16(complaintMasterId));
            //var complaintMaster = _complaintMasterService.GetByComplaintRef(complaintRef);
            var complaintRecordType = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            var complaintSource = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);

            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    id = complaintMaster.ComplaintMasterId,
                    ComplaintMasterId = complaintMaster.ComplaintMasterId,
                    ComplaintRef = complaintMaster.ComplaintRef,
                    ComplaintRecordType = complaintRecordType[complaintMaster.ComplaintRecordType],
                    ComplaintSource = complaintSource[complaintMaster.ComplaintSource],
                    ComplaintDate = complaintMaster.ComplaintDate == null ? "" : complaintMaster.ComplaintDate.Value.ToString("dd/MM/yyyy"),
                    OrgRef = complaintMaster.OrgMaster == null ? "" : complaintMaster.OrgMaster.OrgRef,
                    EngOrgName = complaintMaster.OrgMaster == null ? "" : complaintMaster.OrgMaster.EngOrgName,
                    ChiOrgName = complaintMaster.OrgMaster == null ? "" : complaintMaster.OrgMaster.ChiOrgName
                }
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion "Search"

        #region "Edit Enquiry and Complaint"

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/Complaint/{complaintId:int}/Edit", Name = "EditEnquiryComplaint")]
        public ActionResult EditEnquiryComplaint(int complaintId, string ReturnUrl)
        {
            Ensure.Argument.NotNull(complaintId);
            EditEnquiryComplaintViewModel model = new EditEnquiryComplaintViewModel();
            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + ReturnUrl;
                model.PrePage = prePage;
            }

            model.ComplaintMasterId = complaintId;
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(complaintId);
            if (!String.IsNullOrEmpty(complaintMaster.NonComplianceNature))
            {
                model.NonComplianceNature = System.Text.RegularExpressions.Regex.Split(complaintMaster.NonComplianceNature, @",");
            }

            if (complaintMaster.OrgMaster != null)
            {
                model.OrgMasterId = complaintMaster.OrgMaster.OrgId.ToString();
                getDataFromOrgMaster(model, complaintMaster.OrgMaster);
            }

            return View(model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Update", "CreateComplaintAttachment", "UpdateComplaintAttachment")]
        public PartialViewResult RenderEditEnquiryComplaintDetailsModal()
        {
            EditEnquiryComplaintViewModel model = new EditEnquiryComplaintViewModel();
            model.ComplaintRecordTypes = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            model.ComplaintSources = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            model.ActivityConcerns = this._lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            model.NonComplianceNatures = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.ProcessStatus = this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);
            model.CollectionMethods = this._lookupService.getAllLkpInCodec(LookupType.ComplaintCollectionMethod);
            model.ComplaintResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintResult);
            model.WithholdingRemarks = this._lookupService.getAllLkpInCodec(LookupType.ComplaintWithholdingRemarks);

            return PartialView("_EditEnquiryComplaintDetailsModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintLettersModal()
        {
            return PartialView("_EditEnquiryComplaintLettersModal");
        }

        public PartialViewResult RenderEditEnquiryComplaintAttachmentModal()
        {
            return PartialView("_EditEnquiryComplaintAttachmentModal");
        }

        public PartialViewResult RenderEditEnquiryComplaintTelRecordModal()
        {
            return PartialView("_EditEnquiryComplaintTelRecordModal");
        }

        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public PartialViewResult RenderEditEnquiryComplaintTelRecordDetailsModal()
        {
            ComplaintTelRecordViewModel model = new ComplaintTelRecordViewModel();
            return PartialView("_EditEnquiryComplaintTelRecordDetailsModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintFollowUpActionModal()
        {
            return PartialView("_EditEnquiryComplaintFollowUpActionModal");
        }

        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public PartialViewResult RenderEditEnquiryComplaintFollowUpActionDetailsModal()
        {
            ComplaintFollowUpActionViewModel model = new ComplaintFollowUpActionViewModel();
            model.FollowUpFollowUpLetterTypes = this._lookupService.getAllLkpInCodec(LookupType.FollowUpLetterType);
            model.PoliceCaseInvestigationResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintInvestigationResult);
            return PartialView("_EditEnquiryComplaintFollowUpActionDetailsModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintPoliceCaseModal()
        {
            ComplaintPoliceCaseViewModel model = new ComplaintPoliceCaseViewModel();
            model.PoliceCaseInvestigationResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintInvestigationResult);
            return PartialView("_EditEnquiryComplaintPoliceCaseModal", model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public PartialViewResult RenderEditEnquiryComplaintPoliceCaseDetailsModal()
        {
            ComplaintPoliceCaseViewModel model = new ComplaintPoliceCaseViewModel();
            model.PoliceCaseInvestigationResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintInvestigationResult);
            return PartialView("_EditEnquiryComplaintPoliceCaseDetailsModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintOtherDepartmentEnquiryModal()
        {
            ComplaintOtherDepartmentEnquiryViewModel model = new ComplaintOtherDepartmentEnquiryViewModel();
            model.EnquiryDepartments = this._lookupService.getAllLkpInCodec(LookupType.Department);
            return PartialView("_EditEnquiryComplaintOtherDepartmentEnquiryModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintResultModal()
        {
            ComplaintResultViewModel model = new ComplaintResultViewModel();
            model.Results = compResults;
            model.NonComplianceNatures = nonCompliances;
            return PartialView("_EditEnquiryComplaintResultModal", model);
        }

        public PartialViewResult RenderEditEnquiryComplaintResultDetailsModal()
        {
            ComplaintResultViewModel model = new ComplaintResultViewModel();
            model.Results = compResults;
            model.NonComplianceNatures = nonCompliances;
            return PartialView("_EditEnquiryComplaintResultDetailsModal", model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public PartialViewResult RenderEditEnquiryComplaintOtherDepartmentEnquiryDetailsModal()
        {
            ComplaintOtherDepartmentEnquiryViewModel model = new ComplaintOtherDepartmentEnquiryViewModel();
            model.EnquiryDepartments = this._lookupService.getAllLkpInCodec(LookupType.Department);
            return PartialView("_EditEnquiryComplaintOtherDepartmentEnquiryDetailsModal", model);
        }

        private void getDataFromOrgMaster(EditEnquiryComplaintViewModel model, OrgMaster orgMaster)
        {
            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(orgMaster.OrgId);
            var WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate;
            var WithholdingEndDate = WithHoldingDate.WithholdingEndDate;
            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(orgMaster.OrgId);
            model.OrgRef = orgMaster.OrgRef;
            model.OrgChiName = orgMaster.ChiOrgName;
            model.OrgEngName = orgMaster.EngOrgName;
            model.LblWithholdingBeginDate = WithholdingBeginDate;
            model.LblWithholdingEndDate = WithholdingEndDate;
            model.LblPspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "";
            model.LblPspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "";
            model.LblPspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "";
            model.LblFdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "";
            model.LblFdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "";
            model.LblFdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "";
        }

        #endregion "Edit Enquiry and Complaint"

        #region "New"

        //[PspsAuthorize(Allow.ComplaintMaster)]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        //public ActionResult New()
        //{
        //    ComplaintMasterViewModel model = new ComplaintMasterViewModel();
        //    model.ComplaintSource = this._lookupService.getAllLkpInCodec("GrantType");
        //    model.ActivityConcern = this._lookupService.getAllLkpInCodec("GrantType");
        //    model.NonComplianceNature1 = this._lookupService.getAllLkpInCodec("GrantType");
        //    model.NonComplianceNature2 = this._lookupService.getAllLkpInCodec("GrantType");
        //    model.NonComplianceNature3 = this._lookupService.getAllLkpInCodec("GrantType");
        //    model.ProcessingStatus = this._lookupService.getAllLkpInCodec("GrantType");
        //    return View(model);
        //}

        #endregion "New"

        #region REST-like API

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createComplaintMaster", Name = "CreateComplaintMaster")]
        public JsonResult Create([CustomizeValidator(RuleSet = "default,Create")] ComplaintMasterViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            CultureInfo enUS = new CultureInfo("en-US");
            var complaintMaster = new ComplaintMaster();
            complaintMaster.ComplaintRef = "Ref";//not null,but have no Control,so Temporary set "Ref"
            complaintMaster.ActionFileEnclosureNum = model.ActionFileEnclosureNum;
            complaintMaster.ActivityConcern = model.ActivityConcernId;
            complaintMaster.ComplainantName = model.ComplainantName;
            if (model.ComplaintDate != null && !model.ComplaintDate.Equals(""))
            {
                DateTime complaintDate;
                DateTime.TryParseExact(model.ComplaintDate, "d/M/yyyy", enUS, DateTimeStyles.None, out complaintDate);
                complaintMaster.ComplaintDate = complaintDate;
            }

            //model.ComplaintDate1stReceivedBySWD
            //model.ComplaintDateReceivedByLFPS

            complaintMaster.ComplaintSource = model.ComplaintSourceId;
            complaintMaster.ComplaintSourceRemark = model.ComplaintSourceRemark;
            complaintMaster.DcLcContent = model.DcLcContent;
            complaintMaster.ComplaintEnclosureNum = model.EnclosureNum;
            //complaintMaster.NonComplianceNature1 = model.NonComplianceNature1Id;
            //complaintMaster.NonComplianceNature2 = model.NonComplianceNature2Id;
            //complaintMaster.NonComplianceNature3 = model.NonComplianceNature3Id;
            //model.OrganisationConcerned
            complaintMaster.OtherActivityConcern = model.OtherActivityConcern;
            //complaintMaster.OtherNonComplianceNature1 = model.OtherNonComplianceNature1;
            //complaintMaster.OtherNonComplianceNature2 = model.OtherNonComplianceNature2;
            //complaintMaster.OtherNonComplianceNature3 = model.OtherNonComplianceNature3;
            //model.PermitConcerned
            //model.ProcessingStatusId
            //complaintMaster.ReferFrom1823Indicator = model.ReferFrom1823Indicator;
            //model.RelatedCase
            if (model.ComplaintDate != null && !model.ComplaintDate.Equals(""))
            {
                DateTime replyDueDate;
                DateTime.TryParseExact(model.ReplyDueDate, "d/M/yyyy", enUS, DateTimeStyles.None, out replyDueDate);
                complaintMaster.ComplaintDate = replyDueDate;
            }
            //model.Unit

            using (_unitOfWork.BeginTransaction())
            {
                this._complaintMasterService.Create(complaintMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/listComplaint", Name = "ListComplaint")]
        public JsonResult List(ComplaintMasterViewModel model, GridSettings grid)
        {
            //converting in grid format
            Ensure.Argument.NotNull(grid);
            var complaints = _complaintMasterService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = complaints.TotalPages,
                CurrentPageIndex = complaints.CurrentPageIndex,
                TotalCount = complaints.TotalCount,
                Data = (from c in complaints
                        select new
                        {
                            ComplaintMasterId = c.ComplaintMasterId,
                            OrgRef = c.OrgMaster != null ? c.OrgMaster.OrgRef : "",
                            EngOrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgName : "",
                            Subvented = "",
                            RecordType = "",
                            ComplaintRef = c.ComplaintRef,
                            Source = c.ComplaintSource,
                            ComplaintDate = c.ComplaintDate,
                            PermitConcerned = "",
                            ActionFileEnclosureNum = c.ActionFileEnclosureNum,
                            ProcessingStatus = "",
                            FollowUpAction = "",
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion REST-like API

        #region "Template"

        #region "Template link"

        [PspsAuthorize(Allow.ComplaintTemplate)]
        public ActionResult Template()
        {
            ComplaintDocViewModel model = new ComplaintDocViewModel();
            return View(model);
        }

        #endregion "Template link"

        #region "ListComplaintTemplate"

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpGet, Route("~/complaint/listTemplate", Name = "ListComplaintTemplate")]
        public JsonResult ListComplaintTemplate(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var complaint = _complaintDocService.GetComplaintDocSummaryViewPage(grid);
            //converting in grid form
            var gridResult = new GridResult
            {
                TotalPages = complaint.TotalPages,
                CurrentPageIndex = complaint.CurrentPageIndex,
                TotalCount = complaint.TotalCount,
                Data = (from c in complaint
                        select new
                        {
                            DocNum = c.DocNum,
                            DocName = c.DocName,
                            VersionNum = c.VersionNum,
                            Enabled = c.Enabled,
                            ComplaintDocId = c.ComplaintDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion "ListComplaintTemplate"

        #region "New"

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/complaint/newdoc", Name = "NewComplaintDoc")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,Create,CreateComplaintDoc")] ComplaintDocViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template file from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_TEMPLATE_PATH);
            Ensure.NotNull(templatePath, "No letter found with the specified code");

            // Rename the file name by adding the current times
            var fileName = Path.GetFileName(model.File.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Form the Relative Path for storing in DB
            // and Absolute Path for actually saving the file
            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new ComplaintDoc row and fill the value
            var complaintDoc = new ComplaintDoc();
            complaintDoc.DocNum = model.DocNum;
            complaintDoc.DocStatus = true;
            complaintDoc.DocName = model.Description;
            complaintDoc.VersionNum = model.Version;
            complaintDoc.RowVersion = model.RowVersion;
            complaintDoc.FileLocation = relativePath;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.File.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                this._complaintDocService.CreateComplaintDoc(complaintDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        #endregion "New"

        #region "Edit"

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/complaint/{complaintDocId:int}/edit", Name = "EditComplaintVersion")]
        public JsonResult Edit(int complaintDocId, [CustomizeValidator(RuleSet = "default,UpdateComplaintDoc")]  ComplaintDocViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(complaintDocId.ToString());
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            ComplaintDoc complaint = new ComplaintDoc();
            complaint = this._complaintDocService.GetComplaintDocById(complaintDocId);
            complaint.DocNum = model.DocNum;
            complaint.DocName = model.Description;
            complaint.VersionNum = model.Version;
            complaint.DocStatus = model.IsActive;

            using (_unitOfWork.BeginTransaction())
            {
                this._complaintDocService.UpdateComplaintDoc(complaint);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion "Edit"

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("{id}/Template/{complaintDocId}/Generate", Name = "GenerateComplaintTemplate")]
        public ActionResult GenerateComplaintTemplate(int complaintDocId, int id)
        {
            Ensure.Argument.NotNull(complaintDocId);
            Ensure.Argument.NotNull(id);

            var template = _complaintDocService.GetComplaintDocById(complaintDocId);
            var comp = _complaintDocService.GetCompDocViewById(id);
            var sysParam = _parameterService.GetParameterByCode("ComplaintTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            SimpleDocumentGenerator<ComplaintDocSummaryView> docGenerator = new SimpleDocumentGenerator<ComplaintDocSummaryView>(new DocumentGenerationInfo
            {
                DataContext = comp,
                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
            });
            return File(docGenerator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", template.DocName + ".docx");
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/templatetab/list", Name = "ListComplaintTemplateTab")]
        public JsonResult ListComplaintTemplateTab(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var template = _complaintDocService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = template.TotalPages,
                CurrentPageIndex = template.CurrentPageIndex,
                TotalCount = template.TotalCount,
                Data = (from l in template
                        //where l.DocStatus == true
                        select new
                        {
                            DocNum = l.DocNum,
                            DocName = l.DocName,
                            RowVersion = l.RowVersion,
                            ComplaintDocId = l.ComplaintDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion "Template"

        #region "Complaint Version"

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpGet, Route("{complaintDocId:int}/Version", Name = "ComplaintVersion")]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "NewVersion")]
        public ActionResult Version(int complaintDocId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var complaint = this._complaintDocService.GetComplaintDocById(complaintDocId);

            Ensure.NotNull(complaint, "No letter found with the specified id");
            ComplaintDocViewModel model = new ComplaintDocViewModel();
            model.DocNum = complaint.DocNum;
            return View(model);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpGet, Route("~/api/complaint/{docNum}listVersion", Name = "ListComplaintVersion")]
        public JsonResult ListVersion(GridSettings grid, string docNum)
        {
            Ensure.Argument.NotNull(grid);
            var complaint = this._complaintDocService.GetPage(grid, docNum);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = complaint.TotalPages,
                CurrentPageIndex = complaint.CurrentPageIndex,
                TotalCount = complaint.TotalCount,
                Data = (from s in complaint
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            VersionNum = s.VersionNum,
                            DocStatus = s.DocStatus,
                            RowVersion = s.RowVersion,
                            complaintDocId = s.ComplaintDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/complaint/versionNew", Name = "NewComplaintVersion")]
        public JsonResult VersionNew([CustomizeValidator(RuleSet = "default,NewVersion")]ComplaintDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_TEMPLATE_PATH);

            // Rename the file name by adding the current time
            string fileName = Path.GetFileName(model.File.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path
            string rootPath = templatePath.Value;
            // Form the Relative Path for storing in DB
            // and Absolute Path for actually saving the file
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new ComplaintDoc row
            var complaintDoc = new ComplaintDoc();
            // Fill the values
            complaintDoc.DocNum = model.DocNum;
            complaintDoc.DocName = model.Description;
            complaintDoc.DocStatus = model.IsActive;
            complaintDoc.FileLocation = relativePath;
            complaintDoc.VersionNum = model.Version;
            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Save the fill to the absolute path
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.File.SaveAs(absolutePath);
                    }
                    // Insert record to DB and commit
                    _complaintDocService.CreateComplaintDoc(complaintDoc);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    // Clear the saved file while exception
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/complaint/{complaintDocId}/versionEdit", Name = "versionEditComplaint")]
        public JsonResult VersionEdit(int complaintDocId, [CustomizeValidator(RuleSet = "default,UpdateVersion")]ComplaintDocViewModel model)
        {
            Ensure.Argument.NotNull(complaintDocId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_TEMPLATE_PATH);

            // Set the root path
            string rootPath = templatePath.Value;
            // Paths for new file if needed
            string relativePath = string.Empty;
            string absolutePath = string.Empty;

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Get the ComplaintDoc record by given the ID
                    var complaint = _complaintDocService.GetComplaintDocById(complaintDocId);
                    Ensure.NotNull(complaint, "No letter found with the specified id");

                    // If new file need to be upload
                    if (model.File != null)
                    {
                        // Rename the file name by adding the current time
                        string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                        string fileName = Path.GetFileName(model.File.FileName);
                        string generatedFileName = string.Format("{0}-{1}", time, fileName);

                        // Form the Relative Path for storing in DB
                        // and Absolute Path for actually saving the file
                        relativePath = generatedFileName;
                        absolutePath = Path.Combine(rootPath, generatedFileName);

                        // Save the new file
                        if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        {
                            model.File.SaveAs(absolutePath);
                        }

                        // Form the path of the old file
                        string absolutePathOfOldFile = Path.Combine(rootPath, complaint.FileLocation);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                            System.IO.File.Delete(absolutePathOfOldFile);

                        // Replace with the new path
                        complaint.FileLocation = relativePath;
                    }

                    // Fill the update values
                    complaint.DocStatus = model.IsActive;
                    complaint.VersionNum = model.Version;
                    complaint.DocName = model.Description;

                    // Update DB record and commit
                    _complaintDocService.UpdateComplaintDoc(complaint);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    // Delete uploaded file while exception
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpGet, Route("~/api/complaint/{complaintDocId:int}/GetVersion", Name = "GetComplaintDoc")]
        public JsonResult GetComplaintDoc(int complaintDocId)
        {
            Ensure.Argument.NotNullOrEmpty(complaintDocId.ToString());
            var complaintDoc = this._complaintDocService.GetComplaintDocById(complaintDocId);
            if (complaintDoc == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            var model = new ComplaintDocViewModel()
            {
                DocNum = complaintDoc.DocNum,
                Description = complaintDoc.DocName,
                Version = complaintDoc.VersionNum,
                IsActive = complaintDoc.DocStatus,
                RowVersion = complaintDoc.RowVersion,
                ComplaintDocId = complaintDoc.Id
            };
            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpPost, Route("~/api/complaint/{complaintDocId:int}/delete", Name = "DeteteComplaintDoc")]
        public JsonResult Delete(int complaintDocId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the ComplaintDoc record by given the ID
            var complaintDoc = this._complaintDocService.GetComplaintDocById(complaintDocId);
            Ensure.NotNull(complaintDoc, "No Letter found with the specified id");

            // Get the root path of the Template file from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            string absolutePath = Path.Combine(rootPath, complaintDoc.FileLocation);

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB
                complaintDoc.RowVersion = rowVersion;
                _complaintDocService.DeleteComplaintDoc(complaintDoc);

                // Delete the Template file if exists
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }

                // Commit Delete
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = complaintDoc
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintTemplate)]
        [HttpGet, Route("{complaintDocId:int}/Download", Name = "DownloadComplaintFile")]
        public FileResult Download(int complaintDocId)
        {
            // Get the ComplaintDoc record by given the ID
            var complaint = _complaintDocService.GetComplaintDocById(complaintDocId);

            // Get the root path of the Template from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            var absolutePath = Path.Combine(rootPath, complaint.FileLocation);

            // Set the file name for saving
            string fileName = complaint.DocName + Path.GetExtension(Path.GetFileName(complaint.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        #endregion "Complaint Version"

        #region Enquiry and Complaint  Search

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/searchEnquiryComplaint", Name = "SearchEnquiryComplaint")]
        public JsonResult SearchEnquiryComplaint(EnquiryComplaintSearchViewModel model, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(model);
            grid = GetSearchGrid(grid, model);

            DateTime? complaintDt = model.ComplaintDate;
            DateTime? firstComplaintDt = model.FirstComplaintDate;
            var list = this._complaintMasterService.GetPageByComplaintMasterSearchView(grid, model.IsFollowUp, model.FollowUpIndicator, model.ReportPoliceIndicator, model.OthersFollowUpIndicator,
                                                                                       model.FromDate, model.ToDate);

            this.HttpContext.Session[ENQUIRY_COMPLAINT_SEARCH_SESSION] = model;

            IDictionary<string, string> lookupOrgStatus = this._lookupService.getAllLkpInCodec(LookupType.OrgStatus);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            ComplaintMasterId = c.ComplaintMasterId,
                            OrgRef = c.OrgRef,
                            EngChiOrgName = (c.EngOrgNameSorting != null || c.ChiOrgName != null) ? (c.EngOrgNameSorting + System.Environment.NewLine + c.ChiOrgName) : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                            OrgStatusId = c.DisableIndicator != null ? lookupOrgStatus[c.DisableIndicator.Value ? "1" : "0"] : "",
                            EngOrgName = c.EngOrgName,
                            ChiOrgName = c.ChiOrgName,
                            ConcernedOrgName = c.ConcernedOrgName,
                            SubventedIndicator = c.SubventedIndicator,
                            ComplaintRecordType = !String.IsNullOrEmpty(c.ComplaintRecordType) ? _lookupService.GetDescription(LookupType.ComplaintRecordType, c.ComplaintRecordType) : "",
                            ComplaintRef = c.ComplaintRef,
                            ComplaintSource = !String.IsNullOrEmpty(c.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, c.ComplaintSource) : "",
                            ActivityConcern = _lookupService.GetDescription(LookupType.ComplaintActivityConcern, c.ActivityConcern).Equals("Others") ? c.OtherActivityConcern : _lookupService.GetDescription(LookupType.ComplaintActivityConcern, c.ActivityConcern),
                            ComplaintDate = c.ComplaintDate,
                            ProcessStatus = !String.IsNullOrEmpty(c.ProcessStatus) ? _lookupService.GetDescription(LookupType.ComplaintProcessStatus, c.ProcessStatus) : "",
                            TelRecordNum = c.TelRecordNum,
                            FollowUpActionRecordNum = c.FollowUpActionRecordNum,
                            PoliceCaseNum = c.PoliceCaseNum,
                            OtherDepartmentEnquiryNum = c.OtherDepartmentEnquiryNum,
                            PoliceCaseIndicator = c.PoliceCaseIndicator,
                            PoliceCaseResult = c.PoliceCaseResult,
                            WithholdingBeginDate = c.WithholdingBeginDate,
                            WithholdingEndDate = c.WithholdingEndDate,
                            FollowUpAction = c.FollowUpAction,
                            PspRef = c.PspRef,
                            PspPermitNum = c.PspPermitNum,
                            ReplyDueDate = c.ReplyDueDate,
                            ComplaintResultRemark = c.ComplaintResultRemark,
                            DcLcContent = c.DcLcContent,
                            WithholdingRemark = c.WithholdingRemark,
                            OtherWithholdingRemark = c.OtherWithholdingRemark,
                            FundRaisingDate = c.FundRaisingDate,
                            ComplainantName = c.ComplainantName,
                            FollowUpIndicator = c.FollowUpIndicator,
                            ReportPoliceIndicator = c.ReportPoliceIndicator,
                            OtherFollowUpIndicator = c.OtherFollowUpIndicator
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createEnquiryComplaint", Name = "CreateEnquiryComplaint")]
        public JsonResult CreateEnquiryComplaint([CustomizeValidator(RuleSet = "default,Create")] EnquiryComplaintSearchViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var complaintMaster = new ComplaintMaster();
            complaintMaster.ComplaintRecordType = model.RecordTypeId;
            complaintMaster.ComplaintSource = model.ComplaintSourceId;
            complaintMaster.ComplaintSourceRemark = model.ComplaintSourceRemark;
            complaintMaster.ActivityConcern = model.ActivityConcernId;
            if (!String.IsNullOrEmpty(model.OtherActivityConcern))
            {
                complaintMaster.OtherActivityConcern = model.OtherActivityConcern;
            }
            complaintMaster.ComplaintDate = model.ComplaintDate;
            complaintMaster.ComplaintRef = _complaintMasterService.GenerateComplaintRef(complaintMaster.ComplaintDate.Value.Year);
            complaintMaster.FirstComplaintDate = model.FirstComplaintDate;
            complaintMaster.ReplyDueDate = model.ReplyDueDate;
            complaintMaster.LfpsReceiveDate = model.LfpsReceiveDate;

            complaintMaster.SwdUnit = model.SwdUnit;
            complaintMaster.FundRaisingDate = model.FundRaisingDate;
            complaintMaster.FundRaisingTime = model.FundRaisingTime;
            complaintMaster.FundRaiserInvolve = model.FundRaiserInvolve;
            complaintMaster.FundRaisingLocation = model.FundRaisingLocation;
            complaintMaster.CollectionMethod = model.CollectionMethod;
            complaintMaster.OtherCollectionMethod = model.OtherCollectionMethod;

            if (!String.IsNullOrEmpty(model.ConcernedOrgRef))
            {
                complaintMaster.OrgMaster = _organisationService.GetOrgByRef(model.ConcernedOrgRef);
            }

            complaintMaster.ConcernedOrgName = model.ConcernedOrgName;
            complaintMaster.ComplainantName = model.ComplainantName;

            //(PSP)Permit Concerned
            if (!String.IsNullOrEmpty(model.PspApprovalHistoryId))
            {
                complaintMaster.PspApprovalHistory = _pspApprovalHistoryService.getPspApprovalHistoryById(Convert.ToInt32(model.PspApprovalHistoryId));
            }

            //(FD)Permit Concerned
            if (!String.IsNullOrEmpty(model.FdEventId))
            {
                complaintMaster.FdEvent = _fdEventService.GetFdEventById(Convert.ToInt32(model.FdEventId));
            }
            complaintMaster.DcLcContent = model.DcLcContent;
            complaintMaster.DcLcContentHtml = model.DcLcContentHtml;

            complaintMaster.ComplaintPartNum = model.ComplaintPartNum;
            complaintMaster.ComplaintEnclosureNum = model.ComplaintEnclosureNum;
            complaintMaster.ProcessStatus = model.ProcessStatusId;
            complaintMaster.ComplaintResultRemark = model.ComplaintResultRemark;
            complaintMaster.ComplaintResultRemarkHtml = model.ComplaintResultRemarkHtml;
            complaintMaster.WithholdingListIndicator = model.WithholdingListIndicator;

            complaintMaster.WithholdingBeginDate = model.WithholdingBeginDate;
            complaintMaster.WithholdingEndDate = model.WithholdingEndDate;

            complaintMaster.ActionFilePartNum = model.ActionFilePartNum;
            complaintMaster.ActionFileEnclosureNum = model.ActionFileEnclosureNum;
            //Related Complaint / Enquiry Case
            if (!String.IsNullOrEmpty(model.RelatedComplaintMasterId))
            {
                complaintMaster.RelatedComplaintMaster = _complaintMasterService.GetComplaintMasterById(Convert.ToInt32(model.RelatedComplaintMasterId));
            }
            complaintMaster.WithholdingRemark = model.WithholdingRemarkId;
            complaintMaster.OtherWithholdingRemark = model.OtherWithholdingRemark;
            complaintMaster.OtherWithholdingRemarkHtml = model.OtherWithholdingRemarkHtml;

            using (_unitOfWork.BeginTransaction())
            {
                _complaintMasterService.Create(complaintMaster);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = complaintMaster.ComplaintMasterId,
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/org/enquiryComplaintListOrgMaster", Name = "EnquiryComplaintListOrgMaster")]
        public JsonResult EnquiryComplaintListOrgMaster(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var org = _organisationService.GetPage(grid);
            var gridResult = new GridResult
            {
                TotalPages = org.TotalPages,
                CurrentPageIndex = org.CurrentPageIndex,
                TotalCount = org.TotalCount,
                Data = (from p in org
                        select new
                        {
                            OrgId = p.OrgId,
                            OrgRef = p.OrgRef,
                            OrgName = p.EngOrgName + " " + p.ChiOrgName,
                            EngOrgName = p.EngOrgName,
                            ChiOrgName = p.ChiOrgName,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/searchComplaintListFdApprovalHistory", Name = "SearchComplaintListFdApprovalHistory")]
        public JsonResult SearchComplaintListFdApprovalHistory(string searchTerm, string orgRef, int pageSize, int pageNum)
        {
            GridSettings grid = new GridSettings
            {
                PageIndex = pageNum,
                PageSize = pageSize
            };

            if (orgRef.IsNotNullOrEmpty())
                grid.AddDefaultRule(new Rule { data = orgRef, field = "OrgRef", op = WhereOperation.Contains.ToEnumValue() });

            grid.AddDefaultRule(new List<Rule> {
                new Rule { data = searchTerm, field = "OrgRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "OrgName", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "FdRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "PermitNum", op = WhereOperation.Contains.ToEnumValue()},
            }, GroupOp.OR);

            var list = this._fdEventService.GetPageByComplaintFdPermitNumSearchView(grid);
            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    Total = list.TotalCount,
                    Results = (from p in list
                               select new
                               {
                                   id = p.FdEventId,
                                   OrgRef = p.OrgRef,
                                   OrgName = p.OrgName,
                                   FdRef = p.FdRef,
                                   PermitNum = p.PermitNum,
                               }).ToArray()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/getcomplaintlistpspapprovalhistory/{pspApprovalHistoryId:int}", Name = "GetComplaintListPspApprovalHistory")]
        public JsonResult GetComplaintListPspApprovalHistory(int pspApprovalHistoryId)
        {
            var pspAppHist = this._pspApprovalHistoryService.getPspApprovalHistoryById(pspApprovalHistoryId);

            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    Id = pspAppHist.PspApprovalHistoryId,
                    OrgRef = pspAppHist.PspMaster.OrgMaster.OrgRef,
                    OrgName = pspAppHist.PspMaster.OrgMaster.EngOrgName + " " + pspAppHist.PspMaster.OrgMaster.ChiOrgName,
                    PspRef = pspAppHist.PspMaster.PspRef,
                    PermitNum = pspAppHist.PermitNum,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/searchComplaintListPspApprovalHistory", Name = "SearchComplaintListPspApprovalHistory")]
        public JsonResult SearchComplaintListPspApprovalHistory(string searchTerm, string orgRef, int pageSize, int pageNum)
        {
            GridSettings grid = new GridSettings
            {
                PageIndex = pageNum,
                PageSize = pageSize
            };

            if (orgRef.IsNotNullOrEmpty())
                grid.AddDefaultRule(new Rule { data = orgRef, field = "PspMaster.OrgMaster.OrgRef", op = WhereOperation.Contains.ToEnumValue() });

            grid.AddDefaultRule(new Rule { data = "AP", field = "ApprovalStatus", op = WhereOperation.Equal.ToEnumValue() });

            grid.AddDefaultRule(new List<Rule> {
                new Rule { data = searchTerm, field = "PspMaster.OrgMaster.OrgRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "PspMaster.OrgMaster.EngOrgName", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "PspMaster.OrgMaster.ChiOrgName", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "PspMaster.PspRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "PermitNum", op = WhereOperation.Contains.ToEnumValue()},
            }, GroupOp.OR);

            var list = this._pspApprovalHistoryService.GetPage(grid);
            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    Total = list.TotalCount,
                    Results = (from p in list
                               select new
                               {
                                   id = p.PspApprovalHistoryId,
                                   OrgRef = p.PspMaster.OrgMaster.OrgRef,
                                   OrgName = p.PspMaster.OrgMaster.EngOrgName + " " + p.PspMaster.OrgMaster.ChiOrgName,
                                   PspRef = p.PspMaster.PspRef,
                                   PermitNum = p.PermitNum,
                               }).ToArray()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/enquiryComplaintListPspApprovalHistory", Name = "EnquiryComplaintListPspApprovalHistory")]
        public JsonResult EnquiryComplaintListPspApprovalHistory(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            grid.AddDefaultRule(new Rule { data = "AP", field = "ApprovalStatus", op = WhereOperation.Equal.ToEnumValue() });

            var list = this._pspApprovalHistoryService.GetPage(grid);
            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from p in list
                        select new
                        {
                            PspApprovalHistoryId = p.PspApprovalHistoryId,
                            OrgRef = p.PspMaster.OrgMaster.OrgRef,
                            OrgName = p.PspMaster.OrgMaster.EngOrgName + " " + p.PspMaster.OrgMaster.ChiOrgName,
                            Subvented = p.PspMaster.OrgMaster.SubventedIndicator,
                            PspRef = p.PspMaster.PspRef,
                            PermitNum = p.PermitNum,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/enquiryComplaintListFdApprovalHistory", Name = "EnquiryComplaintListFdApprovalHistory")]
        public JsonResult EnquiryComplaintListFdApprovalHistory(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var list = this._fdEventService.GetPageByComplaintFdPermitNumSearchView(grid);
            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from f in list
                        select new
                        {
                            FdEventId = f.FdEventId,
                            OrgRef = f.OrgRef,
                            OrgName = f.OrgName,
                            SubventedIndicator = f.SubventedIndicator,
                            FdRef = f.FdRef,
                            PermitNum = f.PermitNum,
                            FlagDay = f.FlagDay,
                            TWR = String.IsNullOrEmpty(f.TWR) ? "" : _lookupService.GetDescription(LookupType.TWR, f.TWR),
                            TwrDistrict = String.IsNullOrEmpty(f.TwrDistrict) ? "" : _lookupService.GetDescription(LookupType.TWRDistrict, f.TwrDistrict),
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/EnquiryComplaintListComplaintCase", Name = "EnquiryComplaintListComplaintCase")]
        public JsonResult EnquiryComplaintListComplaintCase(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var list = this._complaintMasterService.GetPage(grid);
            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            ComplaintMasterId = c.ComplaintMasterId,
                            OrgRef = c.OrgMaster != null ? c.OrgMaster.OrgRef : "",
                            OrgName = c.OrgMaster != null ? c.OrgMaster.EngOrgName + " " + c.OrgMaster.ChiOrgName : "",
                            SubventedIndicator = c.OrgMaster != null ? c.OrgMaster.SubventedIndicator : false,
                            ComplaintRecordType = String.IsNullOrEmpty(c.ComplaintRecordType) ? "" : _lookupService.GetDescription(LookupType.ComplaintRecordType, c.ComplaintRecordType),
                            ComplaintRef = c.ComplaintRef,
                            ComplaintSource = String.IsNullOrEmpty(c.ComplaintSource) ? "" : _lookupService.GetDescription(LookupType.ComplaintSource, c.ComplaintSource),
                            ActivityConcern = String.IsNullOrEmpty(c.ActivityConcern) ? "" : _lookupService.GetDescription(LookupType.ComplaintActivityConcern, c.ActivityConcern),
                            ComplaintDate = c.ComplaintDate,
                            ProcessStatus = String.IsNullOrEmpty(c.ProcessStatus) ? "" : _lookupService.GetDescription(LookupType.ComplaintProcessStatus, c.ProcessStatus),
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/exportSearchEnquiryComplaint", Name = "ExportSearchEnquiryComplaint")]
        public JsonResult ExportSearchEnquiryComplaint(ExportSettings exportSettings)
        {
            EnquiryComplaintSearchViewModel model = ((EnquiryComplaintSearchViewModel)(this.HttpContext.Session[ENQUIRY_COMPLAINT_SEARCH_SESSION]));
            if (model != null)
            {
                exportSettings.GridSettings = GetSearchGrid(exportSettings.GridSettings, model);
            }

            //var list = _complaintMasterService.GetPageByComplaintMasterSearchDto(exportSettings.GridSettings,model.IsFollowUp, model.FollowUpIndicator, model.ReportPoliceIndicator, model.OthersFollowUpIndicator);
            var list = this._complaintMasterService.GetPageByComplaintMasterSearchView(exportSettings.GridSettings, model.IsFollowUp, model.FollowUpIndicator, model.ReportPoliceIndicator, model.OthersFollowUpIndicator, model.FromDate, model.ToDate);

            IDictionary<string, string> lookupOrgStatus = this._lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            var dataList = (from c in list
                            select new
                            {
                                ComplaintMasterId = c.ComplaintMasterId,
                                OrgRef = c.OrgRef,
                                EngChiOrgName = (c.EngOrgNameSorting != null || c.ChiOrgName != null) ? (c.EngOrgNameSorting + System.Environment.NewLine + c.ChiOrgName) : (c.ConcernedOrgName != null ? c.ConcernedOrgName : ""),
                                OrgStatusId = c.DisableIndicator != null ? lookupOrgStatus[c.DisableIndicator.Value ? "1" : "0"]: "",
                                EngOrgName = c.EngOrgName,
                                ChiOrgName = c.ChiOrgName,
                                SubventedIndicator = c.SubventedIndicator != null ? (c.SubventedIndicator.Value ? "Yes" : "No") : "",
                                ComplaintRecordType = !String.IsNullOrEmpty(c.ComplaintRecordType) ? _lookupService.GetDescription(LookupType.ComplaintRecordType, c.ComplaintRecordType) : "",
                                ComplaintRef = c.ComplaintRef,
                                ComplaintSource = !String.IsNullOrEmpty(c.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, c.ComplaintSource) : "",
                                ActivityConcern = _lookupService.GetDescription(LookupType.ComplaintActivityConcern, c.ActivityConcern).Equals("Others") ? c.OtherActivityConcern : _lookupService.GetDescription(LookupType.ComplaintActivityConcern, c.ActivityConcern),
                                ComplaintDate = c.ComplaintDate != null ? CommonHelper.ConvertDateTimeToString(c.ComplaintDate.Value, "dd/MM/yyyy") : "",
                                ProcessStatus = !String.IsNullOrEmpty(c.ProcessStatus) ? _lookupService.GetDescription(LookupType.ComplaintProcessStatus, c.ProcessStatus) : "",
                                TelRecordNum = c.TelRecordNum,
                                FollowUpActionRecordNum = c.FollowUpActionRecordNum,
                                PoliceCaseNum = c.PoliceCaseNum,
                                OtherDepartmentEnquiryNum = c.OtherDepartmentEnquiryNum,
                                PoliceCaseIndicator = c.PoliceCaseIndicator ? "Yes" : "No",
                                WithholdingBeginDate = c.WithholdingBeginDate != null ? CommonHelper.ConvertDateTimeToString(c.WithholdingBeginDate.Value, "dd/MM/yyyy") : "",
                                WithholdingEndDate = c.WithholdingEndDate != null ? CommonHelper.ConvertDateTimeToString(c.WithholdingEndDate.Value, "dd/MM/yyyy") : "",
                                FollowUpAction = c.FollowUpAction,
                                PspRef = c.PspRef,
                                PspPermitNum = c.PspPermitNum,
                                ReplyDueDate = c.ReplyDueDate != null ? CommonHelper.ConvertDateTimeToString(c.ReplyDueDate.Value, "dd/MM/yyyy") : "",
                                ComplaintResultRemark = c.ComplaintResultRemark,
                                DcLcContent = c.DcLcContent,
                                WithholdingRemark = c.WithholdingRemark,
                                OtherWithholdingRemark = c.OtherWithholdingRemark,
                                FundRaisingDate = c.FundRaisingDate,
                                ComplainantName = c.ComplainantName,
                                FollowUpIndicator = c.FollowUpIndicator,
                                ReportPoliceIndicator = c.ReportPoliceIndicator,
                                OtherFollowUpIndicator = c.OtherFollowUpIndicator
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<EnquiryComplaintSearchViewModel>();

            if (model.OrgRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgRef"] + " : ORG" + model.OrgRef);

            if (model.OrgName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgName"] + " : " + model.OrgName);

            if (model.OrgStatusId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrgStatusId]);
            }

            if (model.SubventedIndicatorId != null)
                filterCriterias.Add(fieldNames["SubventedIndicatorId"] + " : " + (model.SubventedIndicatorId == "0" ? "False" : "True"));

            if (model.RegistrationTypeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["RegistrationTypeId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationTypeId]);
            }

            if (model.SearchRecordTypeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SearchRecordTypeId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType)[model.SearchRecordTypeId]);
            }

            if (model.PrefixComplaintRefNo.IsNotNullOrEmpty() || model.SuffixComplaintRefNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ComplaintRefNo"] + " : " + (model.PrefixComplaintRefNo == null ? "" : model.PrefixComplaintRefNo) + "C" + (model.SuffixComplaintRefNo == null ? "" : model.SuffixComplaintRefNo));

            if (model.ComplaintDate != null && model.FirstComplaintDate != null)
            {
                tmpVal = "From " + model.ComplaintDate + " to " + model.FirstComplaintDate;
            }
            else if (model.ComplaintDate != null)
            {
                tmpVal = "From " + model.ComplaintDate;
            }
            else if (model.FirstComplaintDate != null)
            {
                tmpVal = "To " + model.FirstComplaintDate;
            }

            if (tmpVal != "") filterCriterias.Add(fieldNames["ComplaintDate"] + " : " + tmpVal);

            if (model.SearchComplaintSourceId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SearchComplaintSourceId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource)[model.SearchComplaintSourceId]);
            }

            if (model.SearchActivityConcernId != null)
            {
                filterCriterias.Add(fieldNames["SearchActivityConcernId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern)[model.SearchActivityConcernId]);
            }

            if (model.FundRaisingLocation != null)
                filterCriterias.Add(fieldNames["FundRaisingLocation"] + " : " + model.FundRaisingLocation);

            if (model.NonComplianceNatureId != null)
            {
                string NonComplianceNatureDescs = "";
                var lookup = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
                foreach (string NonComplianceNatureId in model.NonComplianceNatureId)
                {
                    string NonComplianceNatureDesc = lookup[NonComplianceNatureId];
                    if (NonComplianceNatureId == "Others")
                    {
                        NonComplianceNatureDescs += NonComplianceNatureDesc + (model.OtherNonComplianceNature.IsNotNullOrEmpty() ? " ( " + model.OtherNonComplianceNature + " );" : "");
                    }
                    else
                    {
                        NonComplianceNatureDescs += NonComplianceNatureDesc + ";";
                    }
                }
                filterCriterias.Add(fieldNames["NonComplianceNatureId"] + " : " + NonComplianceNatureDescs);
            }

            if (model.SearchProcessStatusId != null)
            {
                filterCriterias.Add(fieldNames["SearchProcessStatusId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus)[model.SearchProcessStatusId]);
            }

            if (model.IsFollowUp)
            {
                List<string> followupList = new List<string>();
                string followupAdds = "";

                if (model.FollowUpIndicator) followupList.Add(fieldNames["FollowUpIndicator"]);
                if (model.ReportPoliceIndicator) followupList.Add(fieldNames["ReportPoliceIndicator"]);
                if (model.OthersFollowUpIndicator) followupList.Add(fieldNames["OthersFollowUpIndicator"]);
                foreach (string tmp in followupList)
                {
                    followupAdds += tmp + ",";
                }
                if (followupAdds != "")
                {
                    followupAdds = "( " + followupAdds.Remove(followupAdds.Length - 1) + " )";
                }

                filterCriterias.Add(fieldNames["IsFollowUp"] + " : True " + followupAdds);
            }

            if (model.ComplaintResultId != null)
            {
                filterCriterias.Add(fieldNames["ComplaintResultId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.ComplaintResult)[model.ComplaintResultId]);
            }

            if (model.PoliceCaseIndicator != null)
                filterCriterias.Add(fieldNames["PoliceCaseIndicator"] + " : " + (model.PoliceCaseIndicator == "0" ? "False" : "True"));

            if (model.WithholdingIndicator != null)
                filterCriterias.Add(fieldNames["WithholdingIndicator"] + " : " + (model.WithholdingIndicator == "0" ? "False" : "True"));

            if (model.OrgRefIndicator != null)
                filterCriterias.Add(fieldNames["OrgRefIndicator"] + " : " + (model.OrgRefIndicator == "0" ? "False" : "True"));

            if (model.PspPermitNum != null)
                filterCriterias.Add(fieldNames["PspPermitNum"] + " : " + model.PspPermitNum);

            if (model.FdPermitNum != null)
                filterCriterias.Add(fieldNames["FdPermitNum"] + " : " + model.FdPermitNum);
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        private GridSettings GetSearchGrid(GridSettings grid, EnquiryComplaintSearchViewModel model)
        {
            if (!String.IsNullOrEmpty(model.OrgRef))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OrgRef",
                    data = model.OrgRef,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.OrgName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "EngOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ConcernedOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.OrgStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "DisableIndicator",
                    data = model.OrgStatusId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SubventedIndicatorId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SubventedIndicator",
                    data = model.SubventedIndicatorId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.RegistrationTypeId))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationType1",
                        data = model.RegistrationTypeId,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationType2",
                        data = model.RegistrationTypeId,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.RegistrationOtherName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationOtherName1",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationOtherName2",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.SearchRecordTypeId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ComplaintRecordType",
                    data = model.SearchRecordTypeId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.PrefixComplaintRefNo) || !String.IsNullOrEmpty(model.SuffixComplaintRefNo))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ComplaintRef",
                    data = (!String.IsNullOrEmpty(model.PrefixComplaintRefNo) ? "(" + model.PrefixComplaintRefNo + ")" : "") + "C" + model.SuffixComplaintRefNo,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            DateTime? complaintDt = model.ComplaintDate;
            DateTime? firstComplaintDt = model.FirstComplaintDate;
            if (complaintDt != null && firstComplaintDt != null)
            {
                grid.AddDefaultRule(new List<Rule>{
                        new Rule()
                        {
                            field = "ComplaintDate",
                            data = complaintDt.ToString(),
                            op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                        },
                        new Rule()
                        {
                            field = "ComplaintDate",
                            data = firstComplaintDt.ToString(),
                            op = WhereOperation.LessThanOrEqual.ToEnumValue()
                        }
                    }, GroupOp.AND);
            }
            else if (complaintDt != null)
            {
                grid.AddDefaultRule(
                        new Rule()
                        {
                            field = "ComplaintDate",
                            data = complaintDt.ToString(),
                            op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                        }
                    );
            }
            else if (firstComplaintDt != null)
            {
                grid.AddDefaultRule(
                        new Rule()
                        {
                            field = "ComplaintDate",
                            data = firstComplaintDt.ToString(),
                            op = WhereOperation.LessThanOrEqual.ToEnumValue()
                        }
                    );
            }

            if (!String.IsNullOrEmpty(model.SearchComplaintSourceId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "complaintSource",
                    data = model.SearchComplaintSourceId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.SearchActivityConcernId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ActivityConcern",
                    data = model.SearchActivityConcernId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.OtherActivityConcern))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OtherActivityConcern",
                    data = model.OtherActivityConcern,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.FundRaisingLocation))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FundRaisingLocation",
                    data = model.FundRaisingLocation,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (model.NonComplianceNatureId != null && model.NonComplianceNatureId.Count() > 0)
            {
                List<Rule> rules = new List<Rule>();
                foreach (var nonComplianceNature in model.NonComplianceNatureId)
                {
                    if (!String.IsNullOrEmpty(nonComplianceNature.Trim()))
                    {
                        Rule rule = new Rule();
                        rule.field = "ComplaintResult>NonComplianceNature";
                        rule.data = nonComplianceNature;
                        rule.op = WhereOperation.Contains.ToEnumValue();
                        rules.Add(rule);
                    }
                    else
                    {
                        var list = model.NonComplianceNatureId.ToList();
                        list.RemoveAt(list.IndexOf(""));
                        model.NonComplianceNatureId = list.ToArray();
                    }
                }
                if (rules.Count() > 0)
                {
                    grid.AddDefaultRule(rules, GroupOp.OR);
                }
            }

            if (!String.IsNullOrEmpty(model.OtherNonComplianceNature))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ComplaintResult>OtherNonComplianceNature",
                    data = model.OtherNonComplianceNature,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SearchProcessStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ProcessStatus",
                    data = model.SearchProcessStatusId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.ComplaintResultId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ComplaintResult>Result",
                    data = model.ComplaintResultId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.PoliceCaseIndicator))
            {
                if (model.PoliceCaseIndicator.Equals("1"))
                {
                    grid.AddDefaultRule(new Rule()
                    {
                        field = "PoliceCaseNum",
                        data = "0",
                        op = WhereOperation.GreaterThan.ToEnumValue()
                    });
                }
                else
                {
                    grid.AddDefaultRule(new Rule()
                    {
                        field = "PoliceCaseNum",
                        data = "0",
                        op = WhereOperation.Equal.ToEnumValue()
                    });
                }
            }

            if (!String.IsNullOrEmpty(model.PoliceCaseIndicator))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PoliceCaseIndicator",
                    data = model.PoliceCaseIndicator.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.WithholdingIndicator))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingListIndicator",
                    data = model.WithholdingIndicator.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.PspPermitNum))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PspPermitNum",
                    data = model.PspPermitNum,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.FdPermitNum))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FdPermitNum",
                    data = model.FdPermitNum,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.OrgRefIndicator))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OrgRefIndicator",
                    data = model.OrgRefIndicator.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            //Advance Search
            if (!String.IsNullOrEmpty(model.WithholdingRemarkId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingRemark",
                    data = model.WithholdingRemarkId,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            return grid;
        }

        #endregion Enquiry and Complaint  Search

        #region Enquiry and Complaint  Edit

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/editEnquiryComplaintDetail", Name = "EditEnquiryComplaintDetail")]
        public JsonResult EditEnquiryComplaintDetail([CustomizeValidator(RuleSet = "default,Update")] EditEnquiryComplaintViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(model.ComplaintMasterId);
            var oldComplaintMaster = new ComplaintMaster();
            AutoMapper.Mapper.Map(complaintMaster, oldComplaintMaster);
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");
            complaintMaster.ComplaintRecordType = model.ComplaintRecordType;
            complaintMaster.ComplaintSource = model.ComplaintSource;
            complaintMaster.ComplaintSourceRemark = model.ComplaintSourceRemark;
            complaintMaster.ActivityConcern = model.ActivityConcern;

            complaintMaster.OtherActivityConcern = model.OtherActivityConcern;

            complaintMaster.ComplaintDate = model.ComplaintDate;
            complaintMaster.FirstComplaintDate = model.FirstComplaintDate;
            complaintMaster.ReplyDueDate = model.ReplyDueDate;
            complaintMaster.LfpsReceiveDate = model.LfpsReceiveDate;

            complaintMaster.SwdUnit = model.SwdUnit;

            //Organisation Concerned
            if (model.OrgRef.IsNotNullOrEmpty())
                complaintMaster.OrgMaster = _organisationService.GetOrgByRef(model.OrgRef);
            else
                complaintMaster.OrgMaster = null;

            complaintMaster.ConcernedOrgName = model.ConcernedOrgName;
            complaintMaster.ComplainantName = model.ComplainantName;

            //(PSP)Permit Concerned
            if (model.PspApprovalHistoryId.HasValue)
                complaintMaster.PspApprovalHistory = _pspApprovalHistoryService.getPspApprovalHistoryById(model.PspApprovalHistoryId.Value);
            else
                complaintMaster.PspApprovalHistory = null;

            //(FD)Permit Concerned
            if (model.FdEventId.HasValue)
                complaintMaster.FdEvent = _fdEventService.GetFdEventById(model.FdEventId.Value);
            else
                complaintMaster.FdEvent = null;

            complaintMaster.DcLcContent = model.DcLcContent;
            complaintMaster.DcLcContentHtml = model.DcLcContentHtml;
            complaintMaster.ComplaintPartNum = model.ComplaintPartNum;
            complaintMaster.ComplaintEnclosureNum = model.ComplaintEnclosureNum;
            complaintMaster.ProcessStatus = model.ProcessStatusId;
            complaintMaster.ComplaintResultRemark = model.ComplaintResultRemark;
            complaintMaster.ComplaintResultRemarkHtml = model.ComplaintResultRemarkHtml;

            complaintMaster.WithholdingListIndicator = model.WithholdingListIndicator;
            complaintMaster.WithholdingBeginDate = model.WithholdingBeginDate;
            complaintMaster.WithholdingEndDate = model.WithholdingEndDate;
            complaintMaster.ActionFilePartNum = model.ActionFilePartNum;
            complaintMaster.ActionFileEnclosureNum = model.ActionFileEnclosureNum;

            //Related Complaint / Enquiry Case
            if (model.RelatedComplaintMasterId.HasValue)
                complaintMaster.RelatedComplaintMaster = _complaintMasterService.GetComplaintMasterById(model.RelatedComplaintMasterId.Value);
            else
                complaintMaster.RelatedComplaintMaster = null;

            complaintMaster.FundRaisingDate = model.FundRaisingDate;
            complaintMaster.FundRaisingTime = model.FundRaisingTime;
            complaintMaster.FundRaisingLocation = model.FundRaisingLocation;
            complaintMaster.FundRaiserInvolve = model.FundRaiserInvolve;
            complaintMaster.CollectionMethod = model.CollectionMethod;
            complaintMaster.OtherCollectionMethod = model.OtherCollectionMethod;
            complaintMaster.WithholdingRemark = model.WithholdingRemark;
            complaintMaster.OtherWithholdingRemark = model.OtherWithholdingRemark;
            complaintMaster.OtherWithholdingRemarkHtml = model.OtherWithholdingRemarkHtml;
            complaintMaster.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _complaintMasterService.Update(oldComplaintMaster, complaintMaster);
                _unitOfWork.Commit();
            }

            EditEnquiryComplaintViewModel data = new EditEnquiryComplaintViewModel();
            if (complaintMaster.OrgMaster != null)
            {
                getDataFromOrgMaster(data, complaintMaster.OrgMaster);
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = new
                {
                    OrgRef = data.OrgRef,
                    OrgChiName = data.OrgChiName,
                    OrgEngName = data.OrgEngName,
                    LblWithholdingBeginDate = data.LblWithholdingBeginDate,
                    LblWithholdingEndDate = data.LblWithholdingEndDate,
                    LblPspRef = data.LblPspRef,
                    LblPspContactPersonName = data.LblPspContactPersonName,
                    LblPspContactPersonEmailAddress = data.LblPspContactPersonEmailAddress,
                    LblFdRef = data.LblFdRef,
                    LblFdContactPersonName = data.LblFdContactPersonName,
                    LblFdContactPersonEmailAddress = data.LblFdContactPersonEmailAddress,
                    RowVersion = complaintMaster.RowVersion
                },
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{orgRef}/GetOrgMasterIdByRef", Name = "GetOrgMasterIdByRef")]
        public JsonResult GetOrgMasterIdByRef(string orgRef)
        {
            int orgMasterId = _organisationService.GetOrgByRef(orgRef).OrgId;

            return Json(new JsonResponse(true)
            {
                Data = orgMasterId
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/GetEnquiryComplaintDetail", Name = "GetEnquiryComplaintDetail")]
        public JsonResult GetEnquiryComplaintDetail(EditEnquiryComplaintViewModel model)
        {
            Ensure.Argument.NotNull(model.ComplaintMasterId);
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(model.ComplaintMasterId);

            EditEnquiryComplaintViewModel data = new EditEnquiryComplaintViewModel();
            data.ComplaintMasterId = complaintMaster.ComplaintMasterId;
            data.ComplaintRef = complaintMaster.ComplaintRef;
            data.ComplaintRecordType = complaintMaster.ComplaintRecordType;
            data.ComplaintSource = complaintMaster.ComplaintSource;
            data.ComplaintSourceRemark = complaintMaster.ComplaintSourceRemark;
            data.ActivityConcern = complaintMaster.ActivityConcern;
            data.OtherActivityConcern = complaintMaster.OtherActivityConcern;
            data.ComplaintDate = complaintMaster.ComplaintDate;
            data.FirstComplaintDate = complaintMaster.FirstComplaintDate;
            data.ReplyDueDate = complaintMaster.ReplyDueDate;
            data.LfpsReceiveDate = complaintMaster.LfpsReceiveDate;
            data.SwdUnit = complaintMaster.SwdUnit;
            data.FundRaisingDate = complaintMaster.FundRaisingDate;
            data.FundRaisingTime = complaintMaster.FundRaisingTime;
            data.FundRaisingLocation = complaintMaster.FundRaisingLocation;
            data.FundRaiserInvolve = complaintMaster.FundRaiserInvolve;
            data.CollectionMethod = complaintMaster.CollectionMethod;
            data.OtherCollectionMethod = complaintMaster.OtherCollectionMethod;
            data.OrgId = complaintMaster.OrgMaster != null ? complaintMaster.OrgMaster.OrgId + "" : "";
            data.OrgRef = complaintMaster.OrgMaster != null ? complaintMaster.OrgMaster.OrgRef : "";
            data.EngOrgName = complaintMaster.OrgMaster != null ? complaintMaster.OrgMaster.EngOrgName : "";
            data.ChiOrgName = complaintMaster.OrgMaster != null ? complaintMaster.OrgMaster.ChiOrgName : "";
            data.ConcernedOrgName = complaintMaster.ConcernedOrgName != null ? complaintMaster.ConcernedOrgName : "";
            data.ComplainantName = complaintMaster.ComplainantName;
            data.PspApprovalHistoryId = complaintMaster.PspApprovalHistory != null ? (int?)complaintMaster.PspApprovalHistory.PspApprovalHistoryId : null;
            data.PspPermitNum = complaintMaster.PspApprovalHistory != null ? complaintMaster.PspApprovalHistory.PermitNum : "";
            data.FdEventId = complaintMaster.FdEvent != null ? (int?)complaintMaster.FdEvent.FdEventId : null;
            data.FdPermitNum = complaintMaster.FdEvent != null ? complaintMaster.FdEvent.PermitNum : "";
            data.DcLcContent = complaintMaster.DcLcContent;
            data.DcLcContentHtml = complaintMaster.DcLcContentHtml;
            data.ComplaintPartNum = complaintMaster.ComplaintPartNum;
            data.ComplaintEnclosureNum = complaintMaster.ComplaintEnclosureNum;
            data.ProcessStatusId = complaintMaster.ProcessStatus;
            data.ComplaintResult = complaintMaster.ComplaintResult;
            data.ComplaintResultRemark = complaintMaster.ComplaintResultRemark;
            data.ComplaintResultRemarkHtml = complaintMaster.ComplaintResultRemarkHtml;
            data.WithholdingRemark = complaintMaster.WithholdingRemark;
            data.OtherWithholdingRemark = complaintMaster.OtherWithholdingRemark;
            data.OtherWithholdingRemarkHtml = complaintMaster.OtherWithholdingRemarkHtml;
            data.WithholdingBeginDate = complaintMaster.WithholdingBeginDate;
            data.WithholdingEndDate = complaintMaster.WithholdingEndDate;
            data.WithholdingListIndicator = complaintMaster.WithholdingListIndicator;
            data.ActionFilePartNum = complaintMaster.ActionFilePartNum;
            data.ActionFileEnclosureNum = complaintMaster.ActionFileEnclosureNum;
            data.RelatedComplaintMasterId = complaintMaster.RelatedComplaintMaster != null ? (int?)complaintMaster.RelatedComplaintMaster.ComplaintMasterId : null;
            data.RelatedComplaintRef = complaintMaster.RelatedComplaintMaster != null ? complaintMaster.RelatedComplaintMaster.ComplaintRef : "";
            data.RowVersion = complaintMaster.RowVersion;

            return Json(new JsonResponse(true)
            {
                Data = data,
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listComplaintAttachment", Name = "ListComplaintAttachment")]
        public JsonResult ListComplaintAttachment(int complaintMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });
            var complaintAttachments = _complaintAttachmentService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = complaintAttachments.TotalPages,
                CurrentPageIndex = complaintAttachments.CurrentPageIndex,
                TotalCount = complaintAttachments.TotalCount,
                Data = (from c in complaintAttachments
                        select new
                        {
                            ComplaintAttachmentId = c.ComplaintAttachmentId,
                            FileDescription = c.FileDescription,
                            FileName = c.FileName,
                            CreatedById = c.CreatedById,
                            CreatedOn = c.CreatedOn
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId}/createcomplaintattachment", Name = "CreateComplaintAttachment")]
        public JsonResult CreateComplaintAttachment(string complaintMasterId, [CustomizeValidator(RuleSet = "default,CreateComplaintAttachment")] EditEnquiryComplaintViewModel model)
        {
            // Get the ComplaintMaster record by given the ID
            Ensure.Argument.NotNull(complaintMasterId);
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(model.ComplaintMasterId);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_ATTACHMENT_PATH);

            // Rename the file by adding current time
            var fileName = Path.GetFileName(model.AttachmentFile.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path by adding the ComplaintMasterId folder     ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] )
            string rootPath = Path.Combine(attachmentPath.Value, complaintMaster.ComplaintMasterId.ToString());
            // Form the Relative Path for storing in DB         ( [ComplaintMasterId Folder] \ [File Name] )
            // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] \ [File Name] )
            string relativePath = Path.Combine(complaintMaster.ComplaintMasterId.ToString(), generatedFileName);
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new ComplaintAttachment row and fill the value
            var complaintAttachment = new ComplaintAttachment();
            complaintAttachment.ComplaintMaster = complaintMaster;
            complaintAttachment.FileDescription = model.FileDescription;
            complaintAttachment.FileLocation = relativePath;
            complaintAttachment.FileName = model.FileName;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.AttachmentFile.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                _complaintAttachmentService.Create(complaintAttachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updatecomplaintattachment", Name = "UpdateComplaintAttachment")]
        public JsonResult UpdateComplaintAttachment([CustomizeValidator(RuleSet = "default,UpdateComplaintAttachment")] EditEnquiryComplaintViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the ComplaintAttachment record by given the ID
            var complaintAttachment = _complaintAttachmentService.GetById(Convert.ToInt32(model.complaintAttachmentId));
            Ensure.NotNull(complaintAttachment, "No ComplaintAttachment found with the specified id");

            // Fill the update values
            complaintAttachment.FileName = model.FileName;
            complaintAttachment.FileDescription = model.FileDescription;

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_ATTACHMENT_PATH);

            using (_unitOfWork.BeginTransaction())
            {
                // If new file need to be upload
                if (model.AttachmentFile != null)
                {
                    // Rename the file by adding current time
                    var fileName = Path.GetFileName(model.AttachmentFile.FileName);
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    string generatedFileName = string.Format("{0}-{1}", time, fileName);

                    // Set the root path by adding the ComplaintMasterId folder     ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] )
                    string rootPath = Path.Combine(attachmentPath.Value, complaintAttachment.ComplaintMaster.ComplaintMasterId.ToString());
                    // Form the Relative Path for storing in DB         ( [ComplaintMasterId Folder] \ [File Name] )
                    // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] \ [File Name] )
                    string relativePath = Path.Combine(complaintAttachment.ComplaintMaster.ComplaintMasterId.ToString(), generatedFileName);
                    string absolutePath = Path.Combine(rootPath, generatedFileName);

                    // Save the new file
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.AttachmentFile.SaveAs(absolutePath);
                    }

                    // Form the path of the old file
                    string absolutePathOfOldFile = Path.Combine(attachmentPath.Value, complaintAttachment.FileLocation);

                    // Delete the old file
                    if (System.IO.File.Exists(absolutePathOfOldFile))
                    {
                        System.IO.File.Delete(absolutePathOfOldFile);
                    }

                    // Replace with the new path
                    complaintAttachment.FileLocation = relativePath;
                }
                // Update DB record and commit
                _complaintAttachmentService.Update(complaintAttachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("{complaintAttachmentId:int}/DownloadComplaintAttachment", Name = "DownloadComplaintAttachment")]
        public FileResult DownloadComplaintAttachment(int complaintAttachmentId)
        {
            // Get the ComplaintAttachment record by given the ID
            var complaintAttachment = _complaintAttachmentService.GetById(complaintAttachmentId);
            Ensure.NotNull(complaintAttachment, "No ComplaintAttachment found with the specified id");

            // Get the root path of the Attachment from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_ATTACHMENT_PATH);
            string rootPath = attachmentPath.Value;
            var absolutePath = Path.Combine(rootPath, complaintAttachment.FileLocation);
            // ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] \ [File Name] )

            // Set the file name for saving
            string fileName = complaintAttachment.FileName + Path.GetExtension(Path.GetFileName(complaintAttachment.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintAttachmentId:int}/deletecomplaintattachment", Name = "DeleteComplaintAttachment")]
        public JsonResult DeleteComplaintAttachment(int complaintAttachmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the ComplaintAttachment record by given the ID
            var complaintAttachment = _complaintAttachmentService.GetById(complaintAttachmentId);
            Ensure.NotNull(complaintAttachment, "No ComplaintAttachment found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                // Get the root path of the Attachment file from DB
                // and combine with the FileLocation to get the Absolute Path that the file actually stored at
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.COMPLAINT_ATTACHMENT_PATH);
                var absolutePath = Path.Combine(attachmentPath.Value, complaintAttachment.FileLocation);
                // ( [Root Folder of Attachment] \ [ComplaintMasterId Folder] \ [File Name] )

                // Delete the record in DB (set IsDeleted flag)
                _complaintAttachmentService.Delete(complaintAttachment);

                // Delete the Attachment file if exists
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }

                // Commit Delete
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/deletecomplaintmaster", Name = "DeleteComplaintMaster")]
        public JsonResult DeleteComplaintMaster(int complaintMasterId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(complaintMasterId);
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintMasterService.Delete(complaintMaster);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listtelrecord", Name = "ListTelRecord")]
        public JsonResult ListTelRecord(int complaintMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var complaintTelRecords = _complaintTelRecordService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = complaintTelRecords.TotalPages,
                CurrentPageIndex = complaintTelRecords.CurrentPageIndex,
                TotalCount = complaintTelRecords.TotalCount,
                Data = (from c in complaintTelRecords
                        select new
                        {
                            ComplaintTelRecordId = c.ComplaintTelRecordId,
                            TelComplaintRef = c.TelComplaintRef,
                            ComplaintDate = c.ComplaintDate,
                            ComplainantName = c.ComplainantName,
                            ComplainantTelNum = c.ComplainantTelNum,
                            EngOrgName = c.OrgName,
                            PermitNum = GetPermitNum(c),
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintTelRecordId:int}/getcomplainttelrecord", Name = "GetComplaintTelRecord")]
        public JsonResult GetComplaintTelRecord(int complaintTelRecordId)
        {
            Ensure.Argument.NotNull(complaintTelRecordId);
            var complaintTelRecord = _complaintTelRecordService.GetById(complaintTelRecordId);
            Ensure.NotNull(complaintTelRecord, "No ComplaintTelRecord found with the specified id");

            ComplaintTelRecordViewModel model = new ComplaintTelRecordViewModel();
            model.OfficerPost = complaintTelRecord.OfficerPost;
            model.TelRecordComplainantName = complaintTelRecord.ComplainantName;
            model.ComplainantTelNum = complaintTelRecord.ComplainantTelNum;
            model.ComplaintContentRemark = complaintTelRecord.ComplaintContentRemark;
            model.ComplaintContentRemarkHtml = complaintTelRecord.ComplaintContentRemarkHtml;
            model.TelRecordComplaintDate = complaintTelRecord.ComplaintDate;
            model.ComplaintTelRecordId = complaintTelRecord.ComplaintTelRecordId;
            model.ComplaintTime = complaintTelRecord.ComplaintTime;
            model.FdEventId = complaintTelRecord.FdEvent != null ? (int?)complaintTelRecord.FdEvent.FdEventId : null;
            //model.FdPermitNum = complaintTelRecord.FdEvent != null ? complaintTelRecord.FdEvent.PermitNum : "";
            model.ComplaintTelRecordOrgName = complaintTelRecord.OrgName;
            model.OfficerName = complaintTelRecord.OfficerName;
            model.PspApprovalHistoryId = complaintTelRecord.PspApprovalHistory != null ? (int?)complaintTelRecord.PspApprovalHistory.PspApprovalHistoryId : null;
            //model.PspPermitNum = complaintTelRecord.PspApprovalHistory != null ? complaintTelRecord.PspApprovalHistory.PermitNum : "";
            model.TelComplaintRef = complaintTelRecord.TelComplaintRef;

            return Json(new JsonResponse(true)
            {
                Data = model,
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createcomplainttelrecord", Name = "CreateComplaintTelRecord")]
        public JsonResult CreateComplaintTelRecord([CustomizeValidator(RuleSet = "default,Create")] ComplaintTelRecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var complaintMaster = _complaintMasterService.GetComplaintMasterById(Convert.ToInt32(model.ComplaintMasterId));
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");

            ComplaintTelRecord record = new ComplaintTelRecord();

            record.ComplaintMaster = complaintMaster;
            record.TelComplaintRef = GenerateComplaintTelRecordRefNo(model);
            record.ComplaintDate = model.TelRecordComplaintDate;
            record.ComplaintTime = model.ComplaintTime;
            record.ComplainantName = model.TelRecordComplainantName;
            record.ComplainantTelNum = model.ComplainantTelNum;
            record.OrgName = model.ComplaintTelRecordOrgName;
            if (model.PspApprovalHistoryId != null)
            {
                record.PspApprovalHistory = _pspApprovalHistoryService.getPspApprovalHistoryById(Convert.ToInt32(model.PspApprovalHistoryId));
            }
            if (model.FdEventId != null)
            {
                record.FdEvent = _fdEventService.GetFdEventById(Convert.ToInt32(model.FdEventId));
            }
            record.ComplaintContentRemark = model.ComplaintContentRemark;
            record.ComplaintContentRemarkHtml = model.ComplaintContentRemarkHtml;
            record.OfficerName = model.OfficerName;
            record.OfficerPost = model.OfficerPost;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintTelRecordService.Create(record);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updatecomplainttelrecord", Name = "UpdateComplaintTelRecord")]
        public JsonResult UpdateComplaintTelRecord([CustomizeValidator(RuleSet = "default,Update")] ComplaintTelRecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            ComplaintTelRecord record = _complaintTelRecordService.GetById(model.ComplaintTelRecordId.Value);
            Ensure.NotNull(record, "No ComplaintTelRecord found with the specified id");
            record.ComplaintDate = model.TelRecordComplaintDate;
            record.ComplaintTime = model.ComplaintTime;
            record.ComplainantName = model.TelRecordComplainantName;
            record.ComplainantTelNum = model.ComplainantTelNum;
            record.OrgName = model.ComplaintTelRecordOrgName;

            if (model.PspApprovalHistoryId != null)
            {
                record.PspApprovalHistory = _pspApprovalHistoryService.getPspApprovalHistoryById(model.PspApprovalHistoryId.Value);
            }
            if (model.FdEventId != null)
            {
                record.FdEvent = _fdEventService.GetFdEventById(model.FdEventId.Value);
            }
            record.ComplaintContentRemark = model.ComplaintContentRemark;
            record.ComplaintContentRemarkHtml = model.ComplaintContentRemarkHtml;
            record.OfficerName = model.OfficerName;
            record.OfficerPost = model.OfficerPost;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintTelRecordService.Update(record);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/deletecomplainttelrecord", Name = "DeleteComplaintTelRecord")]
        public JsonResult DeleteComplaintTelRecord(ComplaintTelRecordViewModel model)
        {
            ComplaintTelRecord record = _complaintTelRecordService.GetById(model.ComplaintTelRecordId.Value);
            Ensure.NotNull(record, "No ComplaintTelRecord found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintTelRecordService.Delete(record);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/exportcomplainttelrecord", Name = "ExportComplaintTelRecord")]
        public JsonResult ExportComplaintTelRecord(int complaintMasterId, ExportSettings exportSettings)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintTelRecordService.GetPage(exportSettings.GridSettings);

            var dataList = (from c in list
                            select new
                            {
                                TelComplaintRef = c.TelComplaintRef,
                                ComplaintDate = c.ComplaintDate,
                                ComplainantName = c.ComplainantName,
                                ComplainantTelNum = c.ComplainantTelNum,
                                //FundRaisingDate = c.FundRaisingDate != null ? CommonHelper.ConvertDateTimeToString(c.FundRaisingDate) : "",
                                //FundRaisingTime = c.FundRaisingTime,
                                //FundRaisingLocation = c.FundRaisingLocation,
                                EngOrgName = c.ComplaintMaster.OrgMaster.EngOrgName,
                                PermitNum = GetPermitNum(c),
                                ComplaintTelRecordId = c.ComplaintTelRecordId,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/listtelrecordpspapprovalhistory", Name = "ListTelRecordPspApprovalHistory")]
        public JsonResult ListTelRecordPspApprovalHistory(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new List<Rule>
            {
                new Rule()
                {
                    field = "PermitNum",
                    data = "null",
                    op = WhereOperation.NotEqual.ToEnumValue()
                },
                new Rule()
                {
                    field = "PermitNum",
                    data = "",
                    op = WhereOperation.NotEqual.ToEnumValue()
                }
            }, GroupOp.AND);

            var list = this._pspApprovalHistoryService.GetPage(grid);
            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from p in list
                        select new
                        {
                            PspApprovalHistoryId = p.PspApprovalHistoryId,
                            OrgRef = p.PspMaster.OrgMaster.OrgRef,
                            OrgName = p.PspMaster.OrgMaster.EngOrgName + " " + p.PspMaster.OrgMaster.ChiOrgName,
                            Subvented = p.PspMaster.OrgMaster.SubventedIndicator,
                            PspRef = p.PspMaster.PspRef,
                            PermitNum = p.PermitNum,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/listtelrecordfdapprovalhistory", Name = "ListTelRecordFdApprovalHistory")]
        public JsonResult ListTelRecordFdApprovalHistory(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new List<Rule>
            {
                new Rule()
                {
                    field = "PermitNum",
                    data = "null",
                    op = WhereOperation.NotEqual.ToEnumValue()
                },
                new Rule()
                {
                    field = "PermitNum",
                    data = "",
                    op = WhereOperation.NotEqual.ToEnumValue()
                }
            }, GroupOp.AND);
            var list = this._fdEventService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from f in list
                        select new
                        {
                            FdEventId = f.FdEventId,
                            OrgRef = f.FdMaster.OrgMaster.OrgRef,
                            OrgName = f.FdMaster.OrgMaster.EngOrgName + " " + f.FdMaster.OrgMaster.ChiOrgName,
                            SubventedIndicator = f.FdMaster.OrgMaster.SubventedIndicator,
                            FdRef = f.FdMaster.FdRef,
                            PermitNum = f.PermitNum,
                            FlagDay = f.FlagDay,
                            TWR = String.IsNullOrEmpty(f.TWR) ? "" : _lookupService.GetDescription(LookupType.TWR, f.TWR),
                            TwrDistrict = String.IsNullOrEmpty(f.TwrDistrict) ? "" : _lookupService.GetDescription(LookupType.TWRDistrict, f.TwrDistrict),
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/caleditenquirycomplaintrelevantrecordsamount", Name = "CalEditEnquiryComplaintRelevantRecordsAmount")]
        public JsonResult CalEditEnquiryComplaintRelevantRecordsAmount(int complaintMasterId)
        {
            Ensure.Argument.NotNull(complaintMasterId);
            var map = _complaintMasterService.CalEditEnquiryComplaintRelevantRecordsAmount(complaintMasterId);
            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        private string GetPermitNum(ComplaintTelRecord record)
        {
            StringBuilder permitNum = new StringBuilder();
            permitNum.Append(record.PspApprovalHistory != null ? record.PspApprovalHistory.PermitNum + "  " : "");
            permitNum.Append(record.FdEvent != null ? record.FdEvent.PermitNum : "");
            return permitNum.ToString();
        }

        private string GenerateComplaintTelRecordRefNo(ComplaintTelRecordViewModel model)
        {
            string telComplaintRef = "";
            int year = model.TelRecordComplaintDate.Year;
            telComplaintRef = _complaintTelRecordService.GenerateTelComplaintRef(year);
            return telComplaintRef;
        }

        #endregion Enquiry and Complaint  Edit

        #region ComplaintFollowUpAction

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listfollowupaction", Name = "ListFollowUpAction")]
        public JsonResult ListFollowUpAction(int complaintMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintFollowUpActionService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            ComplaintFollowUpActionId = c.ComplaintFollowUpActionId,
                            EnclosureNum = c.EnclosureNum,
                            FollowUpActionType = c.ReportPoliceIndicator
                                                   ? ("Report to Police" + (c.VerbalReportDate != null ? System.Environment.NewLine + "Verbal Report: " + CommonHelper.ConvertDateTimeToString(c.VerbalReportDate.Value, "dd/MM/yyyy") : "")
                                                                        + (!String.IsNullOrEmpty(c.PoliceStation) ? System.Environment.NewLine + "Police Station: " + c.PoliceStation : "")
                                                                        + (!String.IsNullOrEmpty(c.PoliceOfficerName) ? System.Environment.NewLine + "Police Officer: " + c.PoliceOfficerName : "")
                                                                        + (!String.IsNullOrEmpty(c.RnNum) ? System.Environment.NewLine + "RN No.: " + c.RnNum : "")
                                                                        + (!String.IsNullOrEmpty(c.RnRemark) ? System.Environment.NewLine + "(Report to Police) Remarks: " + c.RnRemark : "")
                                                                        + (c.WrittenReferralDate != null ? (System.Environment.NewLine + "Written Referral: " + CommonHelper.ConvertDateTimeToString(c.WrittenReferralDate.Value, "dd/MM/yyyy")) : "")
                                                                        + (!String.IsNullOrEmpty(c.ReferralPoliceStation) ? System.Environment.NewLine + "(Written Referral) Police Station: " + c.ReferralPoliceStation : "")
                                                                        + ((c.ActionFileRefEnclosureNum.IsNotNullOrEmpty() || c.ActionFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "Action File Reference: (E): " + c.ActionFileRefEnclosureNum + " (Part): " + c.ActionFileRefPartNum : "")
                                                                        + (!String.IsNullOrEmpty(c.PoliceInvestigation) ? System.Environment.NewLine + "Result of Investigation: " + _lookupService.GetDescription(LookupType.ComplaintInvestigationResult, c.PoliceInvestigation) : "")
                                                                        + (!String.IsNullOrEmpty(c.PoliceInvestigationResult) ? System.Environment.NewLine + "Investigation Result of the Police: " + c.PoliceInvestigationResult : "")
                                                                        + (c.PoliceReplyDate != null ? System.Environment.NewLine + "Date of Investigation Result: " + CommonHelper.ConvertDateTimeToString(c.PoliceReplyDate.Value, "dd/MM/yyyy") : "")
                                                                        + (c.ConvictedPersonName.IsNotNullOrEmpty() ? System.Environment.NewLine + "Name of Convicted Person: " + c.ConvictedPersonName : "")
                                                                        + (c.ConvictedPersonPosition.IsNotNullOrEmpty() ? System.Environment.NewLine + "Position of Convicted Person: " + c.ConvictedPersonPosition : "")
                                                                        + (c.OffenceDetail.IsNotNullOrEmpty() ? System.Environment.NewLine + "Offence: " + c.OffenceDetail : "")
                                                                        + (c.SentenceDetail.IsNotNullOrEmpty() ? System.Environment.NewLine + "Sentence: " + c.SentenceDetail : "")
                                                                        + (c.CourtRefNum.IsNotNullOrEmpty() ? System.Environment.NewLine + "Court Reference: " + c.CourtRefNum : "")
                                                                        + (c.CourtHearingDate != null ? System.Environment.NewLine + "Date of Court Hearing: " + CommonHelper.ConvertDateTimeToString(c.CourtHearingDate.Value, "dd/MM/yyyy") : "")
                                                                        + (c.PoliceRemark.IsNotNullOrEmpty() ? System.Environment.NewLine + "Remarks: " + c.PoliceRemark : ""))
                                                    : (c.FollowUpIndicator ? ("Follow-up with Organisation" + (!String.IsNullOrEmpty(c.ContactOrgName) ? System.Environment.NewLine + "Telephone Contact with (Name of Organisation): " + c.ContactOrgName : "")
                                                                        + (!String.IsNullOrEmpty(c.ContactPersonName) ? System.Environment.NewLine + "Person Contacted: " + c.ContactPersonName : "")
                                                                        + (c.ContactDate != null ? (System.Environment.NewLine + "Contact Date: " + CommonHelper.ConvertDateTimeToString(c.ContactDate.Value, "dd/MM/yyyy")) : "")
                                                                        + (!String.IsNullOrEmpty(c.OrgRemark) ? System.Environment.NewLine + "Remarks: " + c.OrgRemark : "")
                                                                        + (!String.IsNullOrEmpty(c.FollowUpLetterType) ? System.Environment.NewLine + "Letter Issued: " + _lookupService.GetDescription(LookupType.FollowUpLetterType, c.FollowUpLetterType) : "")
                                                                        + (c.FollowUpLetterIssueDate != null ? System.Environment.NewLine + "Issue Date: " + CommonHelper.ConvertDateTimeToString(c.FollowUpLetterIssueDate.Value, "dd/MM/yyyy") : "")
                                                                        + (!String.IsNullOrEmpty(c.FollowUpLetterReceiver) ? System.Environment.NewLine + "Letter issued to (Name of Organisation): " + c.FollowUpLetterReceiver : "")
                                                                        + (!String.IsNullOrEmpty(c.FollowUpLetterRemark) ? System.Environment.NewLine + "Letter(Remarks): " + c.FollowUpLetterRemark : "")
                                                                        + ((c.FollowUpLetterActionFileRefEnclosureNum.IsNotNullOrEmpty() || c.FollowUpLetterActionFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "Action File Reference: Enclosure Num: " + c.FollowUpLetterActionFileRefEnclosureNum + " Part Num: " + c.FollowUpLetterActionFileRefPartNum : "")
                                                                        + (!String.IsNullOrEmpty(c.FollowUpLetterActionFileRefRemark) ? System.Environment.NewLine + "(Action File Reference)Remarks: " + c.FollowUpLetterActionFileRefRemark : "")
                                                                        + (!String.IsNullOrEmpty(c.FollowUpOrgReply) ? System.Environment.NewLine + "Reply from the Organisation: " + c.FollowUpOrgReply : "")
                                                                        + (c.FollowUpOrgReplyDate != null ? System.Environment.NewLine + "Date of Reply: " + CommonHelper.ConvertDateTimeToString(c.FollowUpOrgReplyDate.Value, "dd/MM/yyyy") : ""))
                                                    : ("Other Follow-up Actions" + (!String.IsNullOrEmpty(c.OtherFollowUpPartyName) ? System.Environment.NewLine + "Party Contacted: " + c.OtherFollowUpPartyName : "")
                                                                        + (c.OtherFollowUpContactDate != null ? System.Environment.NewLine + "Contact Date: " + CommonHelper.ConvertDateTimeToString(c.OtherFollowUpContactDate.Value, "dd/MM/yyyy") : "")
                                                                        + (!String.IsNullOrEmpty(c.OtherFollowUpRemark) ? System.Environment.NewLine + "Remarks: " + c.OtherFollowUpRemark : "")
                                                                        + ((c.OtherFollowUpFileRefEnclosureNum.IsNotNullOrEmpty() || c.OtherFollowUpFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "File Reference Number: Enclosure Num: " + c.OtherFollowUpFileRefEnclosureNum + " Part Num: " + c.OtherFollowUpFileRefPartNum : ""))
                                                    ),
                            VerbalReportDate = c.ReportPoliceIndicator ? c.VerbalReportDate : (c.FollowUpIndicator ? c.ContactDate : c.OtherFollowUpContactDate)
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/exportcomplaintfollowupaction", Name = "ExportComplaintFollowUpAction")]
        public JsonResult ExportComplaintFollowUpAction(ExportSettings exportSettings, int complaintMasterId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintFollowUpActionService.GetPage(exportSettings.GridSettings);

            var dataList = (from c in list
                            select new
                            {
                                ComplaintFollowUpActionId = c.ComplaintFollowUpActionId,
                                EnclosureNum = c.EnclosureNum,
                                FollowUpActionType = c.ReportPoliceIndicator ?
                                                        ("Report to Police" + (c.VerbalReportDate != null ? System.Environment.NewLine + "Verbal Report: " + CommonHelper.ConvertDateTimeToString(c.VerbalReportDate.Value, "dd/MM/yyyy") : "")
                                                                            + (!String.IsNullOrEmpty(c.PoliceStation) ? System.Environment.NewLine + "Police Station: " + c.PoliceStation : "")
                                                                            + (!String.IsNullOrEmpty(c.PoliceOfficerName) ? System.Environment.NewLine + "Police Officer: " + c.PoliceOfficerName : "")
                                                                            + (!String.IsNullOrEmpty(c.RnNum) ? System.Environment.NewLine + "RN No.: " + c.RnNum : "")
                                                                            + (!String.IsNullOrEmpty(c.RnRemark) ? System.Environment.NewLine + "(Report to Police) Remarks: " + c.RnRemark : "")
                                                                            + (c.WrittenReferralDate != null ? (System.Environment.NewLine + "Written Referral: " + CommonHelper.ConvertDateTimeToString(c.WrittenReferralDate.Value, "dd/MM/yyyy")) : "")
                                                                            + (!String.IsNullOrEmpty(c.ReferralPoliceStation) ? System.Environment.NewLine + "(Written Referral) Police Station: " + c.ReferralPoliceStation : "")
                                                                            + ((c.ActionFileRefEnclosureNum.IsNotNullOrEmpty() || c.ActionFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "Action File Reference: Enclosure Num: " + c.ActionFileRefEnclosureNum + " Part Num: " + c.ActionFileRefPartNum : "")
                                                                            + (!String.IsNullOrEmpty(c.PoliceInvestigation) ? System.Environment.NewLine + "Investigation Result: " + _lookupService.GetDescription(LookupType.ComplaintInvestigationResult, c.PoliceInvestigation) : "")
                                                                            + (!String.IsNullOrEmpty(c.PoliceInvestigationResult) ? System.Environment.NewLine + "Investigation Result: " + c.PoliceInvestigationResult : "")
                                                                            + (c.PoliceReplyDate != null ? System.Environment.NewLine + "Date of Investigation Result: " + CommonHelper.ConvertDateTimeToString(c.PoliceReplyDate.Value, "dd/MM/yyyy") : "")
                                                                            + (c.ConvictedPersonName.IsNotNullOrEmpty() ? System.Environment.NewLine + "Name of Convicted Person: " + c.ConvictedPersonName : "")
                                                                            + (c.ConvictedPersonPosition.IsNotNullOrEmpty() ? System.Environment.NewLine + "Position of Convicted Person: " + c.ConvictedPersonPosition : "")
                                                                            + (c.OffenceDetail.IsNotNullOrEmpty() ? System.Environment.NewLine + "Offence: " + c.OffenceDetail : "")
                                                                            + (c.SentenceDetail.IsNotNullOrEmpty() ? System.Environment.NewLine + "Sentence: " + c.SentenceDetail : "")
                                                                            + (c.CourtRefNum.IsNotNullOrEmpty() ? System.Environment.NewLine + "Court Reference: " + c.CourtRefNum : "")
                                                                            + (c.CourtHearingDate != null ? System.Environment.NewLine + "Date of Court Hearing: " + CommonHelper.ConvertDateTimeToString(c.CourtHearingDate.Value, "dd/MM/yyyy") : "")
                                                                            + (c.PoliceRemark.IsNotNullOrEmpty() ? System.Environment.NewLine + "Remarks: " + c.PoliceRemark : ""))
                                                        : (c.FollowUpIndicator ? ("Follow-up with Organisation" + (!String.IsNullOrEmpty(c.ContactOrgName) ? System.Environment.NewLine + "Telephone Contact with (Name of Organisation): " + c.ContactOrgName : "")
                                                                            + (!String.IsNullOrEmpty(c.ContactPersonName) ? System.Environment.NewLine + "Person Contacted: " + c.ContactPersonName : "")
                                                                            + (c.ContactDate != null ? (System.Environment.NewLine + "Contact Date: " + CommonHelper.ConvertDateTimeToString(c.ContactDate.Value, "dd/MM/yyyy")) : "")
                                                                            + (!String.IsNullOrEmpty(c.OrgRemark) ? System.Environment.NewLine + "Remarks: " + c.OrgRemark : "")
                                                                            + (!String.IsNullOrEmpty(c.FollowUpLetterType) ? System.Environment.NewLine + "Letter Issued: " + _lookupService.GetDescription(LookupType.FollowUpLetterType, c.FollowUpLetterType) : "")
                                                                            + (c.FollowUpLetterIssueDate != null ? System.Environment.NewLine + "Issue Date: " + CommonHelper.ConvertDateTimeToString(c.FollowUpLetterIssueDate.Value, "dd/MM/yyyy") : "")
                                                                            + (!String.IsNullOrEmpty(c.FollowUpLetterReceiver) ? System.Environment.NewLine + "Letter issued to (Name of Organisation): " + c.FollowUpLetterReceiver : "")
                                                                            + (!String.IsNullOrEmpty(c.FollowUpLetterRemark) ? System.Environment.NewLine + "Letter(Remarks): " + c.FollowUpLetterRemark : "")
                                                                            + ((c.FollowUpLetterActionFileRefEnclosureNum.IsNotNullOrEmpty() || c.FollowUpLetterActionFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "Action File Reference: Enclosure Num: " + c.FollowUpLetterActionFileRefEnclosureNum + " Part Num: " + c.FollowUpLetterActionFileRefPartNum : "")
                                                                            + (!String.IsNullOrEmpty(c.FollowUpLetterActionFileRefRemark) ? System.Environment.NewLine + "(Action File Reference)Remarks: " + c.FollowUpLetterActionFileRefRemark : "")
                                                                            + (!String.IsNullOrEmpty(c.FollowUpOrgReply) ? System.Environment.NewLine + "Reply from the Organisation: " + c.FollowUpOrgReply : "")
                                                                            + (c.FollowUpOrgReplyDate != null ? System.Environment.NewLine + "Date of Reply: " + CommonHelper.ConvertDateTimeToString(c.FollowUpOrgReplyDate.Value, "dd/MM/yyyy") : ""))
                                                        : ("Other Follow-up Actions" + (!String.IsNullOrEmpty(c.OtherFollowUpPartyName) ? System.Environment.NewLine + "Party Contacted: " + c.OtherFollowUpPartyName : "")
                                                                            + (c.OtherFollowUpContactDate != null ? System.Environment.NewLine + "Contact Date: " + CommonHelper.ConvertDateTimeToString(c.OtherFollowUpContactDate.Value, "dd/MM/yyyy") : "")
                                                                            + (!String.IsNullOrEmpty(c.OtherFollowUpRemark) ? System.Environment.NewLine + "Remarks: " + c.OtherFollowUpRemark : "")
                                                                            + ((c.OtherFollowUpFileRefEnclosureNum.IsNotNullOrEmpty() || c.OtherFollowUpFileRefPartNum.IsNotNullOrEmpty()) ? System.Environment.NewLine + "File Reference Number: Enclosure Num: " + c.OtherFollowUpFileRefEnclosureNum + " Part Num: " + c.OtherFollowUpFileRefPartNum : ""))
                                                        ),
                                VerbalReportDate = c.ReportPoliceIndicator ? c.VerbalReportDate : (c.FollowUpIndicator ? c.ContactDate : c.OtherFollowUpContactDate)
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createfollowup", Name = "CreateFollowUp")]
        public JsonResult CreateFollowUp([CustomizeValidator(RuleSet = "default,Create")] ComplaintFollowUpActionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            Ensure.NotNull(model.ComplaintMasterId, "No ComplaintMaster found with the specified id");

            var complaintMaster = _complaintMasterService.GetComplaintMasterById(model.ComplaintMasterId);
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");

            var followUpAction = new ComplaintFollowUpAction();
            followUpAction.EnclosureNum = _complaintFollowUpActionService.GenerateEnclosureNum(model.ComplaintMasterId);
            followUpAction.ComplaintMaster = complaintMaster;
            followUpAction.FollowUpIndicator = model.FollowUpIndicator;
            followUpAction.ReportPoliceIndicator = model.FollowUpReportPoliceIndicator;
            followUpAction.OtherFollowUpIndicator = model.FollowUpLetterActionOtherFollowUpIndicator;
            if (model.FollowUpIndicator == true)
            {
                followUpAction.ContactOrgName = model.FollowUpContactOrgName;
                followUpAction.ContactPersonName = model.FollowUpContactPersonName;

                followUpAction.ContactDate = model.FollowUpContactDate;
                followUpAction.OrgRemark = model.FollowUpOrgRemark;
                followUpAction.OrgRemarkHtml = model.FollowUpOrgRemarkHtml;
                followUpAction.FollowUpLetterType = model.FollowUpFollowUpLetterType;

                followUpAction.FollowUpLetterIssueDate = model.FollowUpLetterIssueDate;
                followUpAction.FollowUpLetterReceiver = model.FollowUpLetterReceiver;
                followUpAction.FollowUpLetterRemark = model.FollowUpLetterRemark;
                followUpAction.FollowUpLetterRemarkHtml = model.FollowUpLetterRemarkHtml;
                followUpAction.FollowUpLetterActionFileRefEnclosureNum = model.FollowUpLetterActionFileRefEnclosureNum;
                followUpAction.FollowUpLetterActionFileRefPartNum = model.FollowUpLetterActionFileRefPartNum;
                followUpAction.FollowUpLetterActionFileRefRemark = model.FollowUpLetterActionFileRefRemark;
                followUpAction.FollowUpLetterActionFileRefRemarkHtml = model.FollowUpLetterActionFileRefRemarkHtml;
                followUpAction.FollowUpOrgReply = model.FollowUpLetterActionFollowUpOrgReply;
                followUpAction.FollowUpOrgReplyHtml = model.FollowUpLetterActionFollowUpOrgReplyHtml;

                followUpAction.FollowUpOrgReplyDate = model.FollowUpLetterActionFollowUpOrgReplyDate;
                followUpAction.FollowUpOfficerName = model.FollowUpLetterActionFollowUpOfficerName;
                followUpAction.FollowUpOfficerPosition = model.FollowUpLetterActionFollowUpOfficerPosition;
            }

            if (model.FollowUpReportPoliceIndicator == true)
            {
                followUpAction.VerbalReportDate = model.FollowUpVerbalReportDate;
                followUpAction.PoliceStation = model.FollowUpPoliceStation;
                followUpAction.PoliceOfficerName = model.FollowUpPoliceOfficerName;
                followUpAction.RnNum = model.FollowUpRnNum;
                followUpAction.RnRemark = model.FollowUpRnRemark;
                followUpAction.RnRemarkHtml = model.FollowUpRnRemarkHtml;

                followUpAction.WrittenReferralDate = model.FollowUpWrittenReferralDate;
                followUpAction.ReferralPoliceStation = model.FollowUpReferralPoliceStation;
                followUpAction.ActionFileRefEnclosureNum = model.FollowUpActionFileRefEnclosureNum;
                followUpAction.ActionFileRefPartNum = model.FollowUpActionFileRefPartNum;
                followUpAction.PoliceInvestigation = model.FollowUpLetterActionPoliceInvestigation;
                followUpAction.PoliceInvestigationResult = model.FollowUpLetterActionPoliceInvestigationResult;
                followUpAction.PoliceInvestigationResultHtml = model.FollowUpLetterActionPoliceInvestigationResultHtml;
                followUpAction.PoliceReplyDate = model.FollowUpLetterActionPoliceReplyDate;

                followUpAction.ConvictedPersonName = model.FollowUpConvictedPersonName;
                followUpAction.ConvictedPersonPosition = model.FollowUpConvictedPersonPosition;
                followUpAction.OffenceDetail = model.FollowUpOffenceDetail;
                followUpAction.OffenceDetailHtml = model.FollowUpOffenceDetailHtml;
                followUpAction.SentenceDetail = model.FollowUpSentenceDetail;
                followUpAction.SentenceDetailHtml = model.FollowUpSentenceDetailHtml;
                followUpAction.CourtRefNum = model.FollowUpCourtRefNum;
                followUpAction.CourtHearingDate = model.FollowUpCourtHearingDate;
                followUpAction.PoliceRemark = model.FollowUpPoliceRemark;
                followUpAction.PoliceRemarkHtml = model.FollowUpPoliceRemarkHtml;

                followUpAction.ReferralPoliceOfficerName = model.FollowUpLetterActionReferralPoliceOfficerName;
                followUpAction.ReferralPoliceOfficerPosition = model.FollowUpLetterActionReferralPoliceOfficerPosition;
            }

            if (model.FollowUpLetterActionOtherFollowUpIndicator == true)
            {
                followUpAction.OtherFollowUpPartyName = model.FollowUpLetterActionOtherFollowUpPartyName;
                followUpAction.OtherFollowUpContactDate = model.FollowUpLetterActionOtherFollowUpContactDate;
                followUpAction.OtherFollowUpRemark = model.FollowUpLetterActionOtherFollowUpRemark;
                followUpAction.OtherFollowUpRemarkHtml = model.FollowUpLetterActionOtherFollowUpRemarkHtml;
                followUpAction.OtherFollowUpFileRefEnclosureNum = model.FollowUpLetterActionOtherFollowUpFileRefEnclosureNum;
                followUpAction.OtherFollowUpFileRefPartNum = model.FollowUpLetterActionOtherFollowUpFileRefPartNum;
                followUpAction.OtherFollowUpOfficerName = model.FollowUpLetterActionOtherFollowUpOfficerName;
                followUpAction.OtherFollowUpOfficerPosition = model.FollowUpLetterActionOtherFollowUpOfficerPosition;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _complaintFollowUpActionService.Create(followUpAction);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updatefollowup", Name = "UpdateFollowUp")]
        public JsonResult UpdateFollowUp([CustomizeValidator(RuleSet = "default,Update")] ComplaintFollowUpActionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var followUpAction = _complaintFollowUpActionService.GetById(model.ComplaintFollowUpActionId.Value);
            Ensure.NotNull(followUpAction, "No ComplaintFollowUpAction found with the specified id");

            followUpAction.FollowUpIndicator = model.FollowUpIndicator;
            followUpAction.ReportPoliceIndicator = model.FollowUpReportPoliceIndicator;
            followUpAction.OtherFollowUpIndicator = model.FollowUpLetterActionOtherFollowUpIndicator;
            if (model.FollowUpIndicator == true)
            {
                followUpAction.ContactOrgName = model.FollowUpContactOrgName;
                followUpAction.ContactPersonName = model.FollowUpContactPersonName;
                followUpAction.ContactDate = model.FollowUpContactDate;

                followUpAction.OrgRemark = model.FollowUpOrgRemark;
                followUpAction.OrgRemarkHtml = model.FollowUpOrgRemarkHtml;
                followUpAction.FollowUpLetterType = model.FollowUpFollowUpLetterType;
                followUpAction.FollowUpLetterIssueDate = model.FollowUpLetterIssueDate;

                followUpAction.FollowUpLetterReceiver = model.FollowUpLetterReceiver;
                followUpAction.FollowUpLetterRemark = model.FollowUpLetterRemark;
                followUpAction.FollowUpLetterRemarkHtml = model.FollowUpLetterRemarkHtml;
                followUpAction.FollowUpLetterActionFileRefEnclosureNum = model.FollowUpLetterActionFileRefEnclosureNum;
                followUpAction.FollowUpLetterActionFileRefPartNum = model.FollowUpLetterActionFileRefPartNum;
                followUpAction.FollowUpLetterActionFileRefRemark = model.FollowUpLetterActionFileRefRemark;
                followUpAction.FollowUpLetterActionFileRefRemarkHtml = model.FollowUpLetterActionFileRefRemarkHtml;
                followUpAction.FollowUpOrgReply = model.FollowUpLetterActionFollowUpOrgReply;
                followUpAction.FollowUpOrgReplyHtml = model.FollowUpLetterActionFollowUpOrgReplyHtml;

                followUpAction.FollowUpOrgReplyDate = model.FollowUpLetterActionFollowUpOrgReplyDate;
                followUpAction.FollowUpOfficerName = model.FollowUpLetterActionFollowUpOfficerName;
                followUpAction.FollowUpOfficerPosition = model.FollowUpLetterActionFollowUpOfficerPosition;
            }
            else
            {
                followUpAction.ContactOrgName = null;
                followUpAction.ContactPersonName = null;
                followUpAction.ContactDate = null;
                followUpAction.OrgRemark = null;
                followUpAction.OrgRemarkHtml = null;
                followUpAction.FollowUpLetterType = null;
                followUpAction.FollowUpLetterIssueDate = null;
                followUpAction.FollowUpLetterReceiver = null;
                followUpAction.FollowUpLetterRemark = null;
                followUpAction.FollowUpLetterRemarkHtml = null;
                followUpAction.FollowUpLetterActionFileRefEnclosureNum = null;
                followUpAction.FollowUpLetterActionFileRefPartNum = null;
                followUpAction.FollowUpLetterActionFileRefRemark = null;
                followUpAction.FollowUpLetterActionFileRefRemarkHtml = null;
                followUpAction.FollowUpOrgReply = null;
                followUpAction.FollowUpOrgReplyHtml = null;
                followUpAction.FollowUpOrgReplyDate = null;
                followUpAction.FollowUpOfficerName = null;
                followUpAction.FollowUpOfficerPosition = null;
            }

            if (model.FollowUpReportPoliceIndicator == true)
            {
                followUpAction.VerbalReportDate = model.FollowUpVerbalReportDate;
                followUpAction.PoliceStation = model.FollowUpPoliceStation;
                followUpAction.PoliceOfficerName = model.FollowUpPoliceOfficerName;
                followUpAction.RnNum = model.FollowUpRnNum;
                followUpAction.RnRemark = model.FollowUpRnRemark;
                followUpAction.RnRemarkHtml = model.FollowUpRnRemarkHtml;
                followUpAction.WrittenReferralDate = model.FollowUpWrittenReferralDate;

                followUpAction.ReferralPoliceStation = model.FollowUpReferralPoliceStation;
                followUpAction.ActionFileRefEnclosureNum = model.FollowUpActionFileRefEnclosureNum;
                followUpAction.ActionFileRefPartNum = model.FollowUpActionFileRefPartNum;
                followUpAction.PoliceInvestigation = model.FollowUpLetterActionPoliceInvestigation;
                followUpAction.PoliceInvestigationResult = model.FollowUpLetterActionPoliceInvestigationResult;
                followUpAction.PoliceInvestigationResultHtml = model.FollowUpLetterActionPoliceInvestigationResultHtml;
                followUpAction.PoliceReplyDate = model.FollowUpLetterActionPoliceReplyDate;

                followUpAction.ConvictedPersonName = model.FollowUpConvictedPersonName;
                followUpAction.ConvictedPersonPosition = model.FollowUpConvictedPersonPosition;
                followUpAction.OffenceDetail = model.FollowUpOffenceDetail;
                followUpAction.OffenceDetailHtml = model.FollowUpOffenceDetailHtml;
                followUpAction.SentenceDetail = model.FollowUpSentenceDetail;
                followUpAction.SentenceDetailHtml = model.FollowUpSentenceDetailHtml;
                followUpAction.CourtRefNum = model.FollowUpCourtRefNum;
                followUpAction.CourtHearingDate = model.FollowUpCourtHearingDate;
                followUpAction.PoliceRemark = model.FollowUpPoliceRemark;
                followUpAction.PoliceRemarkHtml = model.FollowUpPoliceRemarkHtml;

                followUpAction.ReferralPoliceOfficerName = model.FollowUpLetterActionReferralPoliceOfficerName;
                followUpAction.ReferralPoliceOfficerPosition = model.FollowUpLetterActionReferralPoliceOfficerPosition;
            }
            else
            {
                followUpAction.VerbalReportDate = null;
                followUpAction.PoliceStation = null;
                followUpAction.PoliceOfficerName = null;
                followUpAction.RnNum = null;
                followUpAction.RnRemark = null;
                followUpAction.RnRemarkHtml = null;
                followUpAction.WrittenReferralDate = null;
                followUpAction.ReferralPoliceStation = null;
                followUpAction.ActionFileRefEnclosureNum = null;
                followUpAction.ActionFileRefPartNum = null;
                followUpAction.PoliceInvestigation = null;
                followUpAction.PoliceInvestigationResult = null;
                followUpAction.PoliceInvestigationResultHtml = null;
                followUpAction.PoliceReplyDate = null;
                followUpAction.ConvictedPersonName = null;
                followUpAction.ConvictedPersonPosition = null;
                followUpAction.OffenceDetail = null;
                followUpAction.OffenceDetailHtml = null;
                followUpAction.SentenceDetail = null;
                followUpAction.SentenceDetailHtml = null;
                followUpAction.CourtRefNum = null;
                followUpAction.CourtHearingDate = null;
                followUpAction.PoliceRemark = null;
                followUpAction.PoliceRemarkHtml = null;
                followUpAction.ReferralPoliceOfficerName = null;
                followUpAction.ReferralPoliceOfficerPosition = null;
            }

            if (model.FollowUpLetterActionOtherFollowUpIndicator == true)
            {
                followUpAction.OtherFollowUpPartyName = model.FollowUpLetterActionOtherFollowUpPartyName;
                followUpAction.OtherFollowUpContactDate = model.FollowUpLetterActionOtherFollowUpContactDate;
                followUpAction.OtherFollowUpRemark = model.FollowUpLetterActionOtherFollowUpRemark;
                followUpAction.OtherFollowUpRemarkHtml = model.FollowUpLetterActionOtherFollowUpRemarkHtml;
                followUpAction.OtherFollowUpFileRefEnclosureNum = model.FollowUpLetterActionOtherFollowUpFileRefEnclosureNum;
                followUpAction.OtherFollowUpFileRefPartNum = model.FollowUpLetterActionOtherFollowUpFileRefPartNum;
                followUpAction.OtherFollowUpOfficerName = model.FollowUpLetterActionOtherFollowUpOfficerName;
                followUpAction.OtherFollowUpOfficerPosition = model.FollowUpLetterActionOtherFollowUpOfficerPosition;
            }
            else
            {
                followUpAction.OtherFollowUpIndicator = model.FollowUpLetterActionOtherFollowUpIndicator;
                followUpAction.OtherFollowUpPartyName = null;
                followUpAction.OtherFollowUpContactDate = null;
                followUpAction.OtherFollowUpRemark = null;
                followUpAction.OtherFollowUpRemarkHtml = null;
                followUpAction.OtherFollowUpFileRefEnclosureNum = null;
                followUpAction.OtherFollowUpFileRefPartNum = null;
                followUpAction.OtherFollowUpOfficerName = null;
                followUpAction.OtherFollowUpOfficerPosition = null;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _complaintFollowUpActionService.Create(followUpAction);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/deletefollowup", Name = "DeleteFollowUp")]
        public JsonResult DeleteFollowUp(ComplaintFollowUpActionViewModel model)
        {
            var followUpAction = _complaintFollowUpActionService.GetById(model.ComplaintFollowUpActionId.Value);
            Ensure.NotNull(followUpAction, "No ComplaintFollowUpAction found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintFollowUpActionService.Delete(followUpAction);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintFollowUpActionId:int}/getcomplaintfollowupaction", Name = "GetComplaintFollowUpAction")]
        public JsonResult GetComplaintFollowUpAction(int complaintFollowUpActionId)
        {
            Ensure.NotNull(complaintFollowUpActionId);
            ComplaintFollowUpAction followUpAction = _complaintFollowUpActionService.GetById(complaintFollowUpActionId);
            Ensure.NotNull(followUpAction, "No ComplaintFollowUpAction found with the specified id");

            var model = new ComplaintFollowUpActionViewModel();
            model.ComplaintFollowUpActionId = followUpAction.ComplaintFollowUpActionId;
            model.FollowUpContactDate = followUpAction.ContactDate;
            model.FollowUpContactOrgName = followUpAction.ContactOrgName;
            model.FollowUpContactPersonName = followUpAction.ContactPersonName;
            model.FollowUpEnclosureNum = followUpAction.EnclosureNum;
            model.FollowUpFollowUpLetterType = followUpAction.FollowUpLetterType;
            model.FollowUpIndicator = followUpAction.FollowUpIndicator;

            model.FollowUpLetterActionFileRefEnclosureNum = followUpAction.FollowUpLetterActionFileRefEnclosureNum;
            model.FollowUpLetterActionFileRefPartNum = followUpAction.FollowUpLetterActionFileRefPartNum;
            model.FollowUpLetterActionFileRefRemark = followUpAction.FollowUpLetterActionFileRefRemark;
            model.FollowUpLetterActionFileRefRemarkHtml = followUpAction.FollowUpLetterActionFileRefRemarkHtml;
            model.FollowUpLetterActionFollowUpOrgReply = followUpAction.FollowUpOrgReply;
            model.FollowUpLetterActionFollowUpOrgReplyHtml = followUpAction.FollowUpOrgReplyHtml;
            model.FollowUpLetterActionFollowUpOrgReplyDate = followUpAction.FollowUpOrgReplyDate;
            model.FollowUpLetterActionFollowUpOfficerName = followUpAction.FollowUpOfficerName;
            model.FollowUpLetterActionFollowUpOfficerPosition = followUpAction.FollowUpOfficerPosition;
            model.FollowUpLetterIssueDate = followUpAction.FollowUpLetterIssueDate;
            model.FollowUpLetterReceiver = followUpAction.FollowUpLetterReceiver;
            model.FollowUpLetterRemark = followUpAction.FollowUpLetterRemark;
            model.FollowUpLetterRemarkHtml = followUpAction.FollowUpLetterRemarkHtml;
            model.FollowUpOrgRemark = followUpAction.OrgRemark;
            model.FollowUpOrgRemarkHtml = followUpAction.OrgRemarkHtml;

            model.FollowUpReportPoliceIndicator = followUpAction.ReportPoliceIndicator;
            model.FollowUpActionFileRefEnclosureNum = followUpAction.ActionFileRefEnclosureNum;
            model.FollowUpActionFileRefPartNum = followUpAction.ActionFileRefPartNum;
            model.FollowUpPoliceOfficerName = followUpAction.PoliceOfficerName;
            model.FollowUpPoliceStation = followUpAction.PoliceStation;
            model.FollowUpReferralPoliceStation = followUpAction.ReferralPoliceStation;
            model.FollowUpLetterActionPoliceInvestigationResult = followUpAction.PoliceInvestigationResult;
            model.FollowUpLetterActionPoliceInvestigationResultHtml = followUpAction.PoliceInvestigationResultHtml;
            model.FollowUpLetterActionPoliceReplyDate = followUpAction.PoliceReplyDate;
            model.FollowUpLetterActionReferralPoliceOfficerName = followUpAction.ReferralPoliceOfficerName;
            model.FollowUpLetterActionReferralPoliceOfficerPosition = followUpAction.ReferralPoliceOfficerPosition;
            model.FollowUpRnNum = followUpAction.RnNum;
            model.FollowUpRnRemark = followUpAction.RnRemark;
            model.FollowUpRnRemarkHtml = followUpAction.RnRemarkHtml;
            model.FollowUpVerbalReportDate = followUpAction.VerbalReportDate;
            model.FollowUpWrittenReferralDate = followUpAction.WrittenReferralDate;

            model.FollowUpLetterActionPoliceInvestigation = followUpAction.PoliceInvestigation;
            model.FollowUpConvictedPersonName = followUpAction.ConvictedPersonName;
            model.FollowUpConvictedPersonPosition = followUpAction.ConvictedPersonPosition;
            model.FollowUpOffenceDetail = followUpAction.OffenceDetail;
            model.FollowUpOffenceDetailHtml = followUpAction.OffenceDetailHtml;
            model.FollowUpSentenceDetail = followUpAction.SentenceDetail;
            model.FollowUpSentenceDetailHtml = followUpAction.SentenceDetailHtml;
            model.FollowUpCourtRefNum = followUpAction.CourtRefNum;
            model.FollowUpCourtHearingDate = followUpAction.CourtHearingDate;
            model.FollowUpPoliceRemark = followUpAction.PoliceRemark;
            model.FollowUpPoliceRemarkHtml = followUpAction.PoliceRemarkHtml;

            model.FollowUpLetterActionOtherFollowUpIndicator = followUpAction.OtherFollowUpIndicator;
            model.FollowUpLetterActionOtherFollowUpPartyName = followUpAction.OtherFollowUpPartyName;
            model.FollowUpLetterActionOtherFollowUpContactDate = followUpAction.OtherFollowUpContactDate;
            model.FollowUpLetterActionOtherFollowUpRemark = followUpAction.OtherFollowUpRemark;
            model.FollowUpLetterActionOtherFollowUpRemarkHtml = followUpAction.OtherFollowUpRemarkHtml;
            model.FollowUpLetterActionOtherFollowUpFileRefEnclosureNum = followUpAction.OtherFollowUpFileRefEnclosureNum;
            model.FollowUpLetterActionOtherFollowUpFileRefPartNum = followUpAction.OtherFollowUpFileRefPartNum;
            model.FollowUpLetterActionOtherFollowUpOfficerName = followUpAction.OtherFollowUpOfficerName;
            model.FollowUpLetterActionOtherFollowUpOfficerPosition = followUpAction.OtherFollowUpOfficerPosition;

            return Json(new JsonResponse(true)
            {
                Data = model,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ComplaintFollowUpAction

        #region ComplaintPoliceCase

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listpolicecase", Name = "ListPoliceCase")]
        public JsonResult ListPoliceCase(int complaintMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintPoliceCaseService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            ComplaintPoliceCaseId = c.ComplaintPoliceCaseId,
                            CaseInvestigateRefNum = c.CaseInvestigateRefNum,
                            ReferralDate = c.ReferralDate,
                            MemoDate = c.MemoDate,
                            OrgId = c.OrgId,
                            ConcernOrgName = c.OrgMaster != null ? c.OrgMaster.OrgNameEngChi : string.Empty,
                            ConcernOrgRef = c.OrgMaster != null ? c.OrgMaster.OrgRef : string.Empty,
                            InvestigationResult = !String.IsNullOrEmpty(c.InvestigationResult) ? _lookupService.GetDescription(LookupType.ComplaintInvestigationResult, c.InvestigationResult) : "",
                            PoliceRefNum = c.PoliceRefNum,
                            EnclosureNum = c.EnclosureNum,
                            PartNum = c.PartNum,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/exportcomplaintpolicecase", Name = "ExportComplaintPoliceCase")]
        public JsonResult ExportComplaintPoliceCase(ExportSettings exportSettings, int complaintMasterId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintPoliceCaseService.GetPage(exportSettings.GridSettings);

            var dataList = (from c in list
                            select new
                            {
                                ComplaintPoliceCaseId = c.ComplaintPoliceCaseId,
                                CaseInvestigateRefNum = c.CaseInvestigateRefNum,
                                ReferralDate = c.ReferralDate,
                                MemoDate = c.MemoDate,
                                OrgId = c.OrgId,
                                InvestigationResult = !String.IsNullOrEmpty(c.InvestigationResult) ? _lookupService.GetDescription(LookupType.ComplaintInvestigationResult, c.InvestigationResult) : "",
                                PoliceRefNum = c.PoliceRefNum,
                                EnclosureNum = c.EnclosureNum,
                                PartNum = c.PartNum,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createPoliceCase", Name = "CreatePoliceCase")]
        public JsonResult CreatePoliceCase([CustomizeValidator(RuleSet = "default,Create")] ComplaintPoliceCaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            Ensure.NotNull(model.ComplaintMasterId, "No ComplaintMaster found with the specified id");
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(model.ComplaintMasterId);
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");
            var policeCase = new ComplaintPoliceCase();
            policeCase.ComplaintMaster = complaintMaster;
            policeCase.CaseInvestigateRefNum = _complaintPoliceCaseService.GenerateRefNum(complaintMaster.ComplaintMasterId);

            policeCase.ReferralDate = model.PoliceCaseReferralDate;
            policeCase.MemoDate = model.PoliceCaseMemoDate;

            //Organisation Concerned
            if (!String.IsNullOrEmpty(model.PoliceCaseConcernOrgRef))
            {
                policeCase.OrgId = _organisationService.GetOrgByRef(model.PoliceCaseConcernOrgRef).OrgId;
            }

            policeCase.CorrespondenceEnclosureNum = model.PoliceCaseCorrespondenceEnclosureNum;
            policeCase.CorrespondencePartNum = model.PoliceCaseCorrespondencePartNum;
            policeCase.InvestigationResult = model.PoliceCaseInvestigationResult;
            policeCase.PoliceRefNum = model.PoliceCasePoliceRefNum;
            policeCase.CaseNature = model.PoliceCaseCaseNature;
            policeCase.CaseNatureHtml = model.PoliceCaseCaseNatureHtml;
            policeCase.ResultDetail = model.PoliceCaseResultDetail;
            policeCase.ResultDetailHtml = model.PoliceCaseResultDetailHtml;
            policeCase.EnclosureNum = model.PoliceCaseEnclosureNum;
            policeCase.PartNum = model.PoliceCasePartNum;

            policeCase.FundRaisingDate = model.PoliceCaseFundRaisingDate;
            policeCase.FundRaisingTime = model.PoliceCaseFundRaisingTime;

            policeCase.FundRaisingLocation = model.PoliceCaseFundRaisingLocation;
            policeCase.ConvictedPersonName = model.PoliceCaseConvictedPersonName;
            policeCase.ConvictedPersonPosition = model.PoliceCaseConvictedPersonPosition;
            policeCase.OffenceDetail = model.PoliceCaseOffenceDetail;
            policeCase.OffenceDetailHtml = model.PoliceCaseOffenceDetailHtml;
            policeCase.SentenceDetail = model.PoliceCaseSentenceDetail;
            policeCase.SentenceDetailHtml = model.PoliceCaseSentenceDetailHtml;
            policeCase.CourtRefNum = model.PoliceCaseCourtRefNum;
            policeCase.CourtHearingDate = model.PoliceCaseCourtHearingDate;
            policeCase.Remark = model.Remark;
            policeCase.RemarkHtml = model.RemarkHtml;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintPoliceCaseService.Create(policeCase);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updatepolicecase", Name = "UpdatePoliceCase")]
        public JsonResult UpdatePoliceCase([CustomizeValidator(RuleSet = "default,Update")] ComplaintPoliceCaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            Ensure.NotNull(model.ComplaintPoliceCaseId, "No ComplaintPoliceCase found with the specified id");
            var policeCase = _complaintPoliceCaseService.GetById(model.ComplaintPoliceCaseId.Value);
            Ensure.NotNull(policeCase, "No ComplaintPoliceCase found with the specified id");

            policeCase.ReferralDate = model.PoliceCaseReferralDate;
            policeCase.MemoDate = model.PoliceCaseMemoDate;

            if (!String.IsNullOrEmpty(model.PoliceCaseConcernOrgRef))
            {
                policeCase.OrgId = _organisationService.GetOrgByRef(model.PoliceCaseConcernOrgRef).OrgId;
            }

            policeCase.CorrespondenceEnclosureNum = model.PoliceCaseCorrespondenceEnclosureNum;
            policeCase.CorrespondencePartNum = model.PoliceCaseCorrespondencePartNum;
            policeCase.InvestigationResult = model.PoliceCaseInvestigationResult;
            policeCase.PoliceRefNum = model.PoliceCasePoliceRefNum;
            policeCase.CaseNature = model.PoliceCaseCaseNature;
            policeCase.CaseNatureHtml = model.PoliceCaseCaseNatureHtml;
            policeCase.ResultDetail = model.PoliceCaseResultDetail;
            policeCase.ResultDetailHtml = model.PoliceCaseResultDetailHtml;
            policeCase.EnclosureNum = model.PoliceCaseEnclosureNum;
            policeCase.PartNum = model.PoliceCasePartNum;
            policeCase.FundRaisingDate = model.PoliceCaseFundRaisingDate;
            policeCase.FundRaisingTime = model.PoliceCaseFundRaisingTime;
            policeCase.FundRaisingLocation = model.PoliceCaseFundRaisingLocation;
            policeCase.ConvictedPersonName = model.PoliceCaseConvictedPersonName;
            policeCase.ConvictedPersonPosition = model.PoliceCaseConvictedPersonPosition;
            policeCase.OffenceDetail = model.PoliceCaseOffenceDetail;
            policeCase.OffenceDetailHtml = model.PoliceCaseOffenceDetailHtml;
            policeCase.SentenceDetail = model.PoliceCaseSentenceDetail;
            policeCase.SentenceDetailHtml = model.PoliceCaseSentenceDetailHtml;
            policeCase.CourtRefNum = model.PoliceCaseCourtRefNum;

            policeCase.CourtHearingDate = model.PoliceCaseCourtHearingDate;

            policeCase.Remark = model.Remark;
            policeCase.RemarkHtml = model.RemarkHtml;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintPoliceCaseService.Update(policeCase);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/deletepolicecase", Name = "DeletePoliceCase")]
        public JsonResult DeletePoliceCase(ComplaintPoliceCaseViewModel model)
        {
            Ensure.NotNull(model.ComplaintPoliceCaseId, "No ComplaintPoliceCase found with the specified id");
            var policeCase = _complaintPoliceCaseService.GetById(model.ComplaintPoliceCaseId.Value);
            Ensure.NotNull(policeCase, "No ComplaintPoliceCase found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintPoliceCaseService.Delete(policeCase);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{ComplaintPoliceCaseId:int}/getpolicecase", Name = "GetPoliceCase")]
        public JsonResult GetPoliceCase(int ComplaintPoliceCaseId)
        {
            Ensure.NotNull(ComplaintPoliceCaseId);
            var policeCase = _complaintPoliceCaseService.GetById(ComplaintPoliceCaseId);
            Ensure.NotNull(policeCase, "No ComplaintPoliceCase found with the specified id");
            var model = new ComplaintPoliceCaseViewModel();
            model.ComplaintPoliceCaseId = policeCase.ComplaintPoliceCaseId;
            model.PoliceCaseCaseInvestigateRefNum = policeCase.CaseInvestigateRefNum;
            model.PoliceCaseCaseNature = policeCase.CaseNature;
            model.PoliceCaseCaseNatureHtml = policeCase.CaseNatureHtml;
            model.PoliceCaseOrgId = policeCase.OrgId;
            model.PoliceCaseConcernOrgRef = policeCase.OrgMaster != null ? policeCase.OrgMaster.OrgRef : string.Empty;
            model.PoliceCaseConvictedPersonName = policeCase.ConvictedPersonName;
            model.PoliceCaseConvictedPersonPosition = policeCase.ConvictedPersonPosition;
            model.PoliceCaseCorrespondenceEnclosureNum = policeCase.CorrespondenceEnclosureNum;
            model.PoliceCaseCorrespondencePartNum = policeCase.CorrespondencePartNum;
            model.PoliceCaseCourtHearingDate = policeCase.CourtHearingDate;
            model.PoliceCaseCourtRefNum = policeCase.CourtRefNum;
            model.PoliceCaseEnclosureNum = policeCase.EnclosureNum;
            model.PoliceCasePartNum = policeCase.PartNum;
            model.PoliceCaseFundRaisingDate = policeCase.FundRaisingDate;
            model.PoliceCaseFundRaisingLocation = policeCase.FundRaisingLocation;
            model.PoliceCaseFundRaisingTime = policeCase.FundRaisingTime;
            model.PoliceCaseInvestigationResult = policeCase.InvestigationResult;
            model.PoliceCaseMemoDate = policeCase.MemoDate;
            model.PoliceCaseOffenceDetail = policeCase.OffenceDetail;
            model.PoliceCaseOffenceDetailHtml = policeCase.OffenceDetailHtml;
            model.PoliceCasePoliceRefNum = policeCase.PoliceRefNum;
            model.PoliceCaseReferralDate = policeCase.ReferralDate;
            model.PoliceCaseResultDetail = policeCase.ResultDetail;
            model.PoliceCaseResultDetailHtml = policeCase.ResultDetailHtml;
            model.PoliceCaseSentenceDetail = policeCase.SentenceDetail;
            model.PoliceCaseSentenceDetailHtml = policeCase.SentenceDetailHtml;
            model.Remark = policeCase.Remark;
            model.RemarkHtml = policeCase.RemarkHtml;
            return Json(new JsonResponse(true)
            {
                Data = model,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ComplaintPoliceCase

        #region ComplaintOtherDepartmentEnquiry

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listotherdepartmentenquiry", Name = "ListOtherDepartmentEnquiry")]
        public JsonResult ListOtherDepartmentEnquiry(GridSettings grid, int complaintMasterId)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintOtherDepartmentEnquiryService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            RefNum = c.RefNum,
                            ReferralDate = c.ReferralDate,
                            MemoDate = c.MemoDate,
                            EnquiryDepartment = !String.IsNullOrEmpty(c.EnquiryDepartment) ? _lookupService.GetDescription(LookupType.Department, c.EnquiryDepartment) + " " + c.OtherEnquiryDepartment : c.OtherEnquiryDepartment,
                            OrgInvolved = c.OrgInvolved,
                            EnquiryContent = c.EnquiryContent,
                            EnclosureNum = c.EnclosureNum,
                            ComplaintOtherDeptEnquiryId = c.ComplaintOtherDeptEnquiryId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/exportcomplaintotherdepartmentenquiry", Name = "ExportComplaintOtherDepartmentEnquiry")]
        public JsonResult ExportComplaintOtherDepartmentEnquiry(ExportSettings exportSettings, int complaintMasterId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "ComplaintMaster.ComplaintMasterId",
                data = complaintMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var list = _complaintOtherDepartmentEnquiryService.GetPage(exportSettings.GridSettings);
            var dataList = (from c in list
                            select new
                            {
                                RefNum = c.RefNum,
                                ReferralDate = c.ReferralDate,
                                MemoDate = c.MemoDate,
                                EnquiryDepartment = !String.IsNullOrEmpty(c.EnquiryDepartment) ? _lookupService.GetDescription(LookupType.Department, c.EnquiryDepartment) + " " + c.OtherEnquiryDepartment : c.OtherEnquiryDepartment,
                                OrgInvolved = c.OrgInvolved,
                                EnquiryContent = c.EnquiryContent,
                                EnclosureNum = c.EnclosureNum,
                                ComplaintOtherDeptEnquiryId = c.ComplaintOtherDeptEnquiryId,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createotherdepartmentenquiry", Name = "CreateOtherDepartmentEnquiry")]
        public JsonResult CreateOtherDepartmentEnquiry([CustomizeValidator(RuleSet = "default,Create")] ComplaintOtherDepartmentEnquiryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            Ensure.NotNull(model.ComplaintMasterId, "No ComplaintMaster found with the specified id");
            var complaintMaster = _complaintMasterService.GetComplaintMasterById(Convert.ToInt32(model.ComplaintMasterId));
            Ensure.NotNull(complaintMaster, "No ComplaintMaster found with the specified id");
            var otherDepartmentEnquiry = new ComplaintOtherDepartmentEnquiry();

            otherDepartmentEnquiry.ComplaintMaster = complaintMaster;
            otherDepartmentEnquiry.ReferralDate = model.OtherDepartmentEnquiryReferralDate;
            otherDepartmentEnquiry.MemoDate = model.OtherDepartmentEnquiryMemoDate;
            otherDepartmentEnquiry.MemoFromPoliceDate = model.OtherDepartmentEnquiryMemoFromPoliceDate;

            otherDepartmentEnquiry.EnquiryDepartment = model.EnquiryDepartment;
            otherDepartmentEnquiry.OtherEnquiryDepartment = model.OtherEnquiryDepartment;
            otherDepartmentEnquiry.OrgInvolved = model.OtherDepartmentEnquiryOrgInvolved;
            otherDepartmentEnquiry.EnquiryContent = model.OtherDepartmentEnquiryEnquiryContent;
            otherDepartmentEnquiry.EnquiryContentHtml = model.OtherDepartmentEnquiryEnquiryContentHtml;
            otherDepartmentEnquiry.EnclosureNum = model.OtherDepartmentEnquiryEnclosureNum;
            otherDepartmentEnquiry.Remark = model.OtherDepartmentEnquiryRemark;
            otherDepartmentEnquiry.RemarkHtml = model.OtherDepartmentEnquiryRemarkHtml;

            using (_unitOfWork.BeginTransaction())
            {
                _complaintOtherDepartmentEnquiryService.Create(otherDepartmentEnquiry);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updateotherdepartmentenquiry", Name = "UpdateOtherDepartmentEnquiry")]
        public JsonResult UpdateOtherDepartmentEnquiry([CustomizeValidator(RuleSet = "default,Update")] ComplaintOtherDepartmentEnquiryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            Ensure.NotNull(model.ComplaintOtherDeptEnquiryId);
            var otherDepartmentEnquiry = _complaintOtherDepartmentEnquiryService.GetById(Convert.ToInt32(model.ComplaintOtherDeptEnquiryId));
            Ensure.NotNull(otherDepartmentEnquiry, "No ComplaintOtherDepartmentEnquiry found with the specified id");
            otherDepartmentEnquiry.RefNum = model.OtherDepartmentEnquiryRefNum;
            otherDepartmentEnquiry.ReferralDate = model.OtherDepartmentEnquiryReferralDate;
            otherDepartmentEnquiry.MemoDate = model.OtherDepartmentEnquiryMemoDate;
            otherDepartmentEnquiry.MemoFromPoliceDate = model.OtherDepartmentEnquiryMemoFromPoliceDate;

            otherDepartmentEnquiry.EnquiryDepartment = model.EnquiryDepartment;
            otherDepartmentEnquiry.OtherEnquiryDepartment = model.OtherEnquiryDepartment;
            otherDepartmentEnquiry.OrgInvolved = model.OtherDepartmentEnquiryOrgInvolved;
            otherDepartmentEnquiry.EnquiryContent = model.OtherDepartmentEnquiryEnquiryContent;
            otherDepartmentEnquiry.EnquiryContentHtml = model.OtherDepartmentEnquiryEnquiryContentHtml;
            otherDepartmentEnquiry.EnclosureNum = model.OtherDepartmentEnquiryEnclosureNum;
            otherDepartmentEnquiry.Remark = model.OtherDepartmentEnquiryRemark;
            otherDepartmentEnquiry.RemarkHtml = model.OtherDepartmentEnquiryRemarkHtml;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintOtherDepartmentEnquiryService.Update(otherDepartmentEnquiry);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/deleteotherdepartmentenquiry", Name = "DeleteOtherDepartmentEnquiry")]
        public JsonResult DeleteOtherDepartmentEnquiry(ComplaintOtherDepartmentEnquiryViewModel model)
        {
            Ensure.NotNull(model.ComplaintOtherDeptEnquiryId);
            var otherDepartmentEnquiry = _complaintOtherDepartmentEnquiryService.GetById(Convert.ToInt32(model.ComplaintOtherDeptEnquiryId));
            Ensure.NotNull(otherDepartmentEnquiry, "No ComplaintOtherDepartmentEnquiry found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintOtherDepartmentEnquiryService.Delete(otherDepartmentEnquiry);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintOtherDeptEnquiryId:int}/getotherdepartmentenquiry", Name = "GetOtherDepartmentEnquiry")]
        public JsonResult GetOtherDepartmentEnquiry(int complaintOtherDeptEnquiryId)
        {
            Ensure.NotNull(complaintOtherDeptEnquiryId);
            var otherDepartmentEnquiry = _complaintOtherDepartmentEnquiryService.GetById(Convert.ToInt32(complaintOtherDeptEnquiryId));
            Ensure.NotNull(otherDepartmentEnquiry, "No ComplaintOtherDepartmentEnquiry found with the specified id");
            var model = new ComplaintOtherDepartmentEnquiryViewModel();
            model.ComplaintOtherDeptEnquiryId = otherDepartmentEnquiry.ComplaintOtherDeptEnquiryId + "";
            model.EnquiryDepartment = otherDepartmentEnquiry.EnquiryDepartment;
            model.OtherDepartmentEnquiryEnclosureNum = otherDepartmentEnquiry.EnclosureNum;
            model.OtherDepartmentEnquiryEnquiryContent = otherDepartmentEnquiry.EnquiryContent;
            model.OtherDepartmentEnquiryEnquiryContentHtml = otherDepartmentEnquiry.EnquiryContentHtml;
            model.OtherDepartmentEnquiryMemoDate = otherDepartmentEnquiry.MemoDate;
            model.OtherDepartmentEnquiryMemoFromPoliceDate = otherDepartmentEnquiry.MemoFromPoliceDate;
            model.OtherDepartmentEnquiryOrgInvolved = otherDepartmentEnquiry.OrgInvolved;
            model.OtherDepartmentEnquiryReferralDate = otherDepartmentEnquiry.ReferralDate;
            model.OtherDepartmentEnquiryRefNum = otherDepartmentEnquiry.RefNum;
            model.OtherDepartmentEnquiryRemark = otherDepartmentEnquiry.Remark;
            model.OtherDepartmentEnquiryRemarkHtml = otherDepartmentEnquiry.RemarkHtml;
            model.OtherEnquiryDepartment = otherDepartmentEnquiry.OtherEnquiryDepartment;
            return Json(new JsonResponse(true)
            {
                Data = model,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ComplaintOtherDepartmentEnquiry

        #region ComplaintResult

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintMasterId:int}/listcompaintresult", Name = "ListCompaintResult")]
        public JsonResult ListCompaintResult(GridSettings grid, int complaintMasterId)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(complaintMasterId);

            var list = _complaintResultService.GetPageByComplaintMasterId(grid, complaintMasterId);

            var gridResult = new GridResult
            {
                TotalPages = list.TotalPages,
                CurrentPageIndex = list.CurrentPageIndex,
                TotalCount = list.TotalCount,
                Data = (from c in list
                        select new
                        {
                            ComplaintResultId = c.ComplaintResultId,
                            ComplaintMasterId = c.ComplaintMaster.ComplaintMasterId,
                            NonComplianceNature = c.NonComplianceNature != null ? mapCompliance(c.NonComplianceNature) : "",
                            OtherNonComplianceNature = c.OtherNonComplianceNature,
                            Result = c.Result != null ? compResults[c.Result] : "",
                            ResultRemark = c.ResultRemark
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/createcomplaintresult", Name = "CreateComplaintResult")]
        public JsonResult CreateComplaintResult([CustomizeValidator(RuleSet = "default")] ComplaintResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            Ensure.NotNull(model.ComplaintMasterId, "No Complaint Master found with the specified id");

            var complaintResult = new ComplaintResult();
            var complaintMaster = _complaintMasterService.GetComplaintMasterById((int)model.ComplaintMasterId);
            complaintResult.ComplaintMaster = complaintMaster;
            complaintResult.Result = model.Result;
            complaintResult.ResultRemark = model.ResultRemark;
            complaintResult.ResultRemarkHtml = model.ResultRemarkHtml;
            complaintResult.NonComplianceNature = model.NonComplianceNature != null ? string.Join(",", model.NonComplianceNature) : "";
            complaintResult.OtherNonComplianceNature = model.OtherNonComplianceNature;

            using (_unitOfWork.BeginTransaction())
            {
                _complaintResultService.Create(complaintResult);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpGet, Route("~/api/complaint/{complaintResultId:int}/getcomplaintresult", Name = "GetComplaintResult")]
        public JsonResult GetComplaintResult(int complaintResultId)
        {
            Ensure.NotNull(complaintResultId, "No complaint result found");
            var complaintResult = _complaintResultService.GetComplaintResultById(Convert.ToInt32(complaintResultId));
            Ensure.NotNull(complaintResult, "No Complaint Result found with the specified id");

            ComplaintResultViewModel model = new ComplaintResultViewModel();
            model.ComplaintResultId = complaintResult.ComplaintResultId;
            model.ComplaintMasterId = complaintResult.ComplaintMaster.ComplaintMasterId;
            model.NonComplianceNature = complaintResult.NonComplianceNature.Split(',').ToList();

            model.OtherNonComplianceNature = complaintResult.OtherNonComplianceNature;
            model.Result = complaintResult.Result;
            model.ResultRemark = complaintResult.ResultRemark;
            model.ResultRemarkHtml = complaintResult.ResultRemarkHtml;
            model.Results = compResults;
            model.RowVersion = complaintResult.RowVersion;

            return Json(new JsonResponse(true)
            {
                Data = model,
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/updatecomplaintresult", Name = "UpdateComplaintResult")]
        public JsonResult UpdateComplaintResult([CustomizeValidator(RuleSet = "default")] ComplaintResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            Ensure.NotNull(model.ComplaintResultId, "No complaint result found with the specified id");
            var complaintResult = _complaintResultService.GetComplaintResultById(Convert.ToInt32(model.ComplaintResultId));

            complaintResult.NonComplianceNature = model.NonComplianceNature != null ? string.Join(",", model.NonComplianceNature) : "";
            complaintResult.OtherNonComplianceNature = model.OtherNonComplianceNature;
            complaintResult.Result = model.Result;
            complaintResult.ResultRemark = model.ResultRemark;
            complaintResult.ResultRemarkHtml = model.ResultRemarkHtml;
            complaintResult.RowVersion = model.RowVersion;
            using (_unitOfWork.BeginTransaction())
            {
                _complaintResultService.Update(complaintResult);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintResultId:int}/deletecomplaintresult", Name = "DeleteComplaintResult")]
        public JsonResult DeleteComplaintResult(int complaintResultId)
        {
            Ensure.NotNull(complaintResultId);
            var complaintResult = _complaintResultService.GetComplaintResultById(Convert.ToInt32(complaintResultId));

            Ensure.NotNull(complaintResult, "No complaint result found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _complaintResultService.Delete(complaintResult);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [HttpPost, Route("~/api/complaint/{complaintMasterId:int}/exportcomplaintresult", Name = "ExportComplaintResult")]
        public JsonResult ExportComplaintResult(ExportSettings exportSettings, int complaintMasterId)
        {
            //exportSettings.GridSettings.AddDefaultRule(new Rule()
            //{
            //    field = "ComplaintMaster.ComplaintMasterId",
            //    data = complaintMasterId + "",
            //    op = WhereOperation.Equal.ToEnumValue()
            //});

            var list = _complaintResultService.GetPageByComplaintMasterId(exportSettings.GridSettings, complaintMasterId);
            var dataList = (from c in list
                            select new
                            {
                                ComplaintResultId = c.ComplaintResultId,
                                ComplaintMasterId = c.ComplaintMaster.ComplaintMasterId,
                                NonComplianceNature = !string.IsNullOrEmpty(c.NonComplianceNature) ? mapCompliance(c.NonComplianceNature) : "",
                                OtherNonComplianceNature = c.OtherNonComplianceNature,
                                Result = !string.IsNullOrEmpty(c.Result) ? compResults[c.Result] : "",
                                ResultRemark = c.ResultRemark
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        private string mapCompliance(string input)
        {
            var arr = input.Split(',');
            for (int e = 0; e < arr.Length; e++)
            {
                arr[e] = nonCompliances[arr[e]];
            }
            return string.Join(",", arr);
        }

        #endregion ComplaintResult

        #region ComplaintReport

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/{complaintMasterId:int}/r17/generate", Name = "R17Generate")]
        public JsonResult R17Generate(int complaintMasterId)
        {
            Ensure.Argument.NotNull(complaintMasterId);
            var reportId = "R17";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".docx");

            var memoryStream = _reportService.GenerateR17Word(complaintMasterId, templatePath);
            return JsonFileResult(reportId, reportId + ".docx", memoryStream);
        }

        #endregion ComplaintReport
    }
}