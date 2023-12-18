using DocxGenerator.Library;
using FluentValidation.Mvc;
using log4net;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Organisation;
using Psps.Models.Dto.Psp;
using Psps.Models.Dto.Reports;
using Psps.Services.ComplaintMasters;
using Psps.Services.Disaster;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.Posts;
using Psps.Services.PSPs;
using Psps.Services.Report;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.PSP;
using Psps.Web.ViewModels.PspApprove;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("PSP"), Route("{action=index}")]
    public class PSPController : BaseController
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Field & Construtor

        private readonly string PSP_SEARCH_SESSION = Constant.PSP_SEARCH_SESSION;
        private readonly string PSP_DEFAULT_SEARCH_SESSION = Constant.PSP_DEFAULT_SEARCH_SESSION;
        private readonly string PSP_DEFAULT_EXPORT_SESSION = Constant.PSP_DEFAULT_EXPORT_SESSION;
        private readonly string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        private readonly IDisasterMasterService _disasterMasterService;
        private readonly ILookupService _lookupService;
        private readonly IPspService _pspService;
        private readonly IPspEventService _pspEventService;
        private readonly IOrganisationService _organisationService;
        private readonly IPspApprovalHistoryService _pspApprovalHistoryService;
        private readonly IPspDocService _pspDocService;
        private readonly IParameterService _parameterService;
        private readonly IPspCountEventsService _pspCountEventsService;
        private readonly IPostService _postService;
        private readonly IReportService _reportService;
        private readonly IPspAttachmentService _pspAttachmentService;
        private readonly IWithholdingHistoryService _withholdingHistoryService;
        private readonly IOrgEditLatestPspFdViewRepository _orgEditLatestPspFdViewRepository;
        private readonly IPspRecommendEventsViewRepository _pspRecommendEventsViewRepository;
        private readonly IPspEventsToProformaRepository _pspEventsToProformaRepository;
        private readonly IPspApprovedEventsRepository _pspApprovedEventsRepository;
        private readonly IComplaintMasterService _complaintMasterService;

        private readonly IDictionary<bool, string> boolStringDict;
        private readonly IDictionary<string, string> checkIndicator;

        private readonly IDictionary<string, string> activityConcerned;
        private readonly IDictionary<string, string> applyForTwrs;
        private readonly IDictionary<string, string> approvalStatusDict;
        private readonly IDictionary<string, string> caseCloseReasons;
        private readonly IDictionary<string, string> chiSalutes;
        private readonly IDictionary<string, string> complaintResults;
        private readonly IDictionary<string, string> complaintSource;
        private readonly IDictionary<string, string> collectionMethods;
        private readonly IDictionary<int, string> disasterNames;
        private readonly IDictionary<int, string> disasterNamesBySysDate;
        private readonly IDictionary<string, string> docSubmissions;
        private readonly IDictionary<string, string> engSalutes;
        private readonly IDictionary<string, string> fundUseds;
        private readonly IDictionary<string, string> issueLetterType;
        private readonly IDictionary<string, string> languages;
        private readonly IDictionary<string, string> orgStatus;
        private readonly IDictionary<string, string> positions;
        private readonly IDictionary<string, string> processStatus;
        private readonly IDictionary<string, string> pspApplicationResults;
        private readonly IDictionary<string, string> pspEventRemarks;
        private readonly IDictionary<string, string> pspNotRequireReasons;

        private readonly IDictionary<string, string> regTypes;
        private readonly IDictionary<string, string> rejectReasons;
        private readonly IDictionary<string, string> specialRemarks;

        private readonly IDictionary<string, string> twrDistricts;
        private readonly IDictionary<string, string> yesNo;
        private IDictionary<string, string> yearofPspList;

        private readonly IDictionary<string, string> publicPlaceIndicatorDict;
        private readonly IDictionary<string, string> publicPlaceIndicatorSearchDict;
        private readonly IUserLogService _userLogService;

        public PSPController(IUnitOfWork unitOfWork, IMessageService messageService,
            ILookupService lookupService, IPspService pspService, IPspDocService pspDocService, IParameterService parameterService, IPspEventService pspEventService,
            IPspApprovalHistoryService pspApprovalHistoryService, IOrganisationService organisationService, IPostService PostService,
            IDisasterMasterService disasterMasterService, IPspCountEventsService pspCountEventsService,
            IReportService reportService, IPspAttachmentService pspAttachmentService, IWithholdingHistoryService withholdingHistoryService,
            IOrgEditLatestPspFdViewRepository orgEditLatestPspFdViewRepository, IPspRecommendEventsViewRepository pspRecommendEventsViewRepository, IPspEventsToProformaRepository pspEventsToProformaRepository,
            IPspApprovedEventsRepository pspApprovedEventsRepository, IComplaintMasterService complaintMasterService, IUserLogService userLogService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._pspService = pspService;
            this._pspDocService = pspDocService;
            this._parameterService = parameterService;
            this._pspEventService = pspEventService;
            this._pspApprovalHistoryService = pspApprovalHistoryService;
            this._organisationService = organisationService;
            this._postService = PostService;
            this._disasterMasterService = disasterMasterService;
            this._pspCountEventsService = pspCountEventsService;
            this._reportService = reportService;
            this._pspAttachmentService = pspAttachmentService;
            this._withholdingHistoryService = withholdingHistoryService;
            this._orgEditLatestPspFdViewRepository = orgEditLatestPspFdViewRepository;
            this._pspRecommendEventsViewRepository = pspRecommendEventsViewRepository;
            this._pspEventsToProformaRepository = pspEventsToProformaRepository;
            this._pspApprovedEventsRepository = pspApprovedEventsRepository;
            this._complaintMasterService = complaintMasterService;
            this._userLogService = userLogService;

            boolStringDict = new Dictionary<bool, string>();
            boolStringDict.Add(false, "No");
            boolStringDict.Add(true, "Yes");

            checkIndicator = _lookupService.getAllLkpInCodec(LookupType.CheckIndicator);

            approvalStatusDict = new Dictionary<string, string>();
            approvalStatusDict.Add("AP", "Approved");
            approvalStatusDict.Add("RA", "Ready for Approve");
            approvalStatusDict.Add("RJ", "Rejected");
            approvalStatusDict.Add("CA", "Cancelled");
            approvalStatusDict.Add("RC", "Ready for Cancel");


            publicPlaceIndicatorSearchDict = new Dictionary<string, string>();
            publicPlaceIndicatorSearchDict.Add("1", "Public");
            publicPlaceIndicatorSearchDict.Add("0", "Non-Public");
            publicPlaceIndicatorSearchDict.Add("", "(empty)");

            publicPlaceIndicatorDict = new Dictionary<string, string>();
            publicPlaceIndicatorDict.Add("true", "Public");
            publicPlaceIndicatorDict.Add("false", "Non-Public");



            this.activityConcerned = _lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            if (activityConcerned.Count == 0) activityConcerned.Add("", "");

            this.applyForTwrs = _lookupService.getAllLkpInCodec(LookupType.TWR);
            if (applyForTwrs.Count == 0) applyForTwrs.Add("", "");

            this.caseCloseReasons = _lookupService.getAllLkpInCodec(LookupType.CaseCloseReason);
            if (caseCloseReasons.Count == 0) caseCloseReasons.Add("", "");

            this.chiSalutes = _lookupService.getAllLkpInChiCodec(LookupType.Salute);
            if (chiSalutes.Count == 0) chiSalutes.Add("", "");

            this.collectionMethods = _lookupService.getAllLkpInCodec(LookupType.PspCollectionMethod);
            if (collectionMethods.Count == 0) collectionMethods.Add("", "");

            this.complaintResults = _lookupService.getAllLkpInCodec(LookupType.ComplaintResult);
            this.complaintSource = _lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            if (complaintSource.Count == 0) complaintSource.Add("", "");

            this.engSalutes = _lookupService.getAllLkpInCodec(LookupType.Salute);
            if (engSalutes.Count == 0) engSalutes.Add("", "");

            this.disasterNames = this._disasterMasterService.GetAllDisasterMasterForDropdown();

            this.disasterNamesBySysDate = _disasterMasterService.GetAllDisasterMasterBySysDate();
            if (disasterNamesBySysDate.Count == 0) disasterNamesBySysDate.Add(0, "");

            this.docSubmissions = _lookupService.getAllLkpInCodec(LookupType.PspDocSubmitted);
            if (docSubmissions.Count == 0) docSubmissions.Add("", "");

            this.fundUseds = _lookupService.getAllLkpInCodec(LookupType.FundUsed);
            if (fundUseds.Count == 0) fundUseds.Add("", "");

            this.issueLetterType = _lookupService.getAllLkpInCodec(LookupType.FollowUpLetterType);
            if (issueLetterType.Count == 0) issueLetterType.Add("", "");

            this.languages = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
            if (languages.Count == 0) languages.Add("", "");

            this.orgStatus = lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            if (orgStatus.Count == 0) orgStatus.Add("", "");

            this.pspApplicationResults = _lookupService.getAllLkpInCodec(LookupType.PSPApplicationResult);
            if (pspApplicationResults.Count == 0) pspApplicationResults.Add("", "");

            this.positions = _postService.GetProcessingOfficerPostsForDropdown();
            if (positions.Count == 0) positions.Add("", "");

            this.processStatus = _lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);

            this.pspEventRemarks = _lookupService.getAllLkpInCodec(LookupType.PspEventRemark);
            if (pspEventRemarks.Count == 0) pspEventRemarks.Add("", "");

            this.pspNotRequireReasons = _lookupService.getAllLkpInCodec(LookupType.PSPNotRequireReason);
            if (pspNotRequireReasons.Count == 0) pspNotRequireReasons.Add("", "");

            this.regTypes = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            if (regTypes.Count == 0) regTypes.Add("", "");

            this.rejectReasons = _lookupService.getAllLkpInCodec(LookupType.PSPRejectReason);
            if (rejectReasons.Count == 0) rejectReasons.Add("", "");

            this.specialRemarks = _lookupService.getAllLkpInCodec(LookupType.PspSpecialRemark);
            if (specialRemarks.Count == 0) specialRemarks.Add("", "");

            this.twrDistricts = _lookupService.getAllLkpInCodec(LookupType.PspRegion);
            if (twrDistricts.Count == 0) twrDistricts.Add("", "");

            this.yesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);
        }

        #endregion Field & Construtor

        #region New

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("New/{createMode}/{pspMasterId:int}", Name = "NewPspMaster")] // copy to new
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult New(string createMode, int pspMasterId, int orgId, string ReturnUrl)
        {
            PSPViewModel model = new PSPViewModel();
            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + ReturnUrl;
                model.PrePage = prePage;
            }

            var pspMaster = new PspMaster();
            var orgMaster = new OrgMaster();

            if (pspMasterId > 0) //when copy to new
            {
                pspMaster = _pspService.GetPSPById(pspMasterId);
                model = getDataFromPspMaster(model, pspMaster);
                model.PrevPspMasterId = pspMasterId;
                model.PreviousReferenceNumber = "";
                model.CreateModelReferenceNumber = "";
                model.PermitNo = "";
                model.DateofEventPeriodFrom = null;
                model.DateofEventPeriodTo = null;
                model.DateofReceivingApplication = DateTime.Today;
                model.DateofApplicationDisposal = DateTime.Today;
                //model.IsShowNewApplicant = pspMaster.OrgMaster == null ? true : _pspService.IsShowNewApplicant(pspMaster.OrgMaster.OrgId);
            }
            else
            {
                //model.IsShowNewApplicant = true;
            }

            initPspViewModel(model, false);
            //model = initPspViewModel(model);
            if (createMode == "copyToNew")
            {
                model.NewApplicant = false;
                model.BypassValidation = false;

                //Clear AFS and Track Record info
                model.TrackRecordStartDate = null;
                model.TrackRecordEndDate = null;
                model.TrackRecordDetails = "";
                model.AfsRecordStartDate = null;
                model.AfsRecordEndDate = null;
                model.AfsRecordDetails = "";

                //Clear Application Result Data
                model.DateofCompletingApplication = null;
                model.DateofApplicationDisposal = null;
                model.ApplicationResultId = null;
                model.PermitNo = "";
                model.RejectReasonId = null;
                model.PspNotRequireReasonId = null;
                model.CaseCloseReasonId = null;
                model.SpecialRemark = null;
                model.OtherSpecialRemark = "";
                model.RejectionLetterDate = null;
                model.RepresentationReceiveDate = null;
                model.IsSsaf = false;
            }

            return View(model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("New/{createMode}/{orgId:int}/createPspMaster", Name = "CreatePspMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult New(string createMode, int orgId, string returnUrl, bool isSsaf = false)
        {
            PSPViewModel model = new PSPViewModel();
            if (!String.IsNullOrEmpty(returnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + returnUrl;
                model.PrePage = prePage;
            }

            var orgMaster = new OrgMaster();
            if (orgId > 0)
            {
                orgMaster = _organisationService.GetOrgById(orgId);
                model.CreateModelOrgRef = orgMaster.OrgRef;
                model.OrgEngName = orgMaster.EngOrgName;
                model.OrgChiName = orgMaster.ChiOrgName;
                model.CreateModelSection88 = orgMaster.Section88Indicator;
                model.IsSsaf = isSsaf;
            }

            initPspViewModel(model, false);
            //model = initPspViewModel(model);

            return View(model);
        }

        private PSPViewModel getDataFromPspMaster(PSPViewModel model, PspMaster pspMaster)
        {
            //var p = pspMaster.PspApprovalHistory != null ? (from u in pspMaster.PspApprovalHistory
            //                                                where !string.IsNullOrEmpty(u.PermitNum)
            //                                                select u.PermitNum).ToArray() : null;
            //p = p.Distinct().ToArray();
            //var pNum = string.Join(",", p);

            model.CreateModelReferenceNumber = pspMaster.PspRef;
            model.PermitNo = pspMaster.PermitNum;
            model.PrevPspMasterId = pspMaster.PreviousPspMasterId;
            model.LanguageUsedId = pspMaster.UsedLanguage;
            model.CreateModelOrgRef = pspMaster.OrgMaster.OrgRef;
            model.EngSalute = pspMaster.ContactPersonSalute;
            model.ContactPersonFirstName = pspMaster.ContactPersonFirstName;
            model.ContactPersonLastName = pspMaster.ContactPersonLastName;
            model.ContactPersonChiFirstName = pspMaster.ContactPersonChiFirstName;
            model.ContactPersonChiLastName = pspMaster.ContactPersonChiLastName;
            model.ContactPersonPost = pspMaster.ContactPersonPosition;
            model.TelNo = pspMaster.ContactPersonTelNum;
            model.FaxNo = pspMaster.ContactPersonFaxNum;
            model.ContactPersonEmailAddress = pspMaster.ContactPersonEmailAddress;
            model.NewApplicant = pspMaster.NewApplicantIndicator != null ? (bool)pspMaster.NewApplicantIndicator : false;
            model.PositionId = pspMaster.ProcessingOfficerPost;
            model.YearofPsp = pspMaster.PspYear;
            model.DateofEventPeriodFrom = pspMaster.EventPeriodFrom;
            model.DateofEventPeriodTo = pspMaster.EventPeriodTo;
            model.PermitRevokeIndicator = pspMaster.PermitRevokeIndicator != null ? (bool)pspMaster.PermitRevokeIndicator : false;
            model.BypassValidation = pspMaster.BypassValidationIndicator != null ? (bool)pspMaster.BypassValidationIndicator : false;
            model.CreateModelSection88 = pspMaster.Section88Indicator != null ? (bool)pspMaster.Section88Indicator : false;
            model.DateofApplication = pspMaster.ApplicationDate;
            model.DateofReceivingApplication = pspMaster.ApplicationReceiveDate;
            model.DateofCompletingApplication = pspMaster.ApplicationCompletionDate;
            model.DateofApplicationDisposal = pspMaster.ApplicationDisposalDate;
            model.BUDateforAction = pspMaster.ActionBuDate;
            model.DisasterId = pspMaster.DisasterMaster != null ? pspMaster.DisasterMaster.DisasterMasterId : (int?)null;
            model.BeneficiaryOrg = pspMaster.BeneficiaryOrg;
            model.PurposeofFundRaising = pspMaster.EngFundRaisingPurpose;
            model.PurposeofChiFundRaising = pspMaster.ChiFundRaisingPurpose;
            model.EventTitle = pspMaster.EventTitle;
            model.CharitySalesItems = pspMaster.EngCharitySalesItem;
            model.CharitySalesItemsChi = pspMaster.ChiCharitySalesItem;
            model.LanguageUsedId = pspMaster.UsedLanguage;
            //model.DateofApplicationDisposal = pspMaster.ApplicationDisposalDate;
            model.ApplicationResultId = pspMaster.ApplicationResult;
            model.RejectReasonId = pspMaster.RejectReason;
            model.RejectRemark = pspMaster.RejectRemark;
            model.OtherRejectReason = pspMaster.OtherRejectReason;
            model.PspNotRequireReasonId = pspMaster.PspNotRequireReason;
            model.OtherPspNotRequireReason = pspMaster.OtherPspNotRequireReason;
            model.CaseCloseReasonId = pspMaster.CaseCloseReason;
            model.OtherCaseCloseReason = pspMaster.OtherCaseCloseReason;
            model.SpecialRemark = pspMaster.SpecialRemark.IsNotNullOrEmpty() ? pspMaster.SpecialRemark.Split(',').ToList() : null;
            model.OtherSpecialRemark = pspMaster.OtherSpecialRemark;
            model.RejectionLetterDate = pspMaster.RejectionLetterDate;
            model.RepresentationReceiveDate = pspMaster.RepresentationReceiveDate;
            model.RepresentationReplyDate = pspMaster.RepresentationReplyDate;
            model.FundUsedId = pspMaster.FundUsed;
            model.DocSubmission = pspMaster.DocSubmission.IsNotNullOrEmpty() ? pspMaster.DocSubmission.Split(',').ToList() : null;
            model.SubmissionDueDate = pspMaster.SubmissionDueDate;
            model.TrackRecordStartDate = pspMaster.TrackRecordStartDate;
            model.TrackRecordEndDate = pspMaster.TrackRecordEndDate;
            model.TrackRecordDetails = pspMaster.TrackRecordDetails;
            model.AfsRecordStartDate = pspMaster.AfsRecordStartDate;
            model.AfsRecordEndDate = pspMaster.AfsRecordEndDate;
            model.AfsRecordDetails = pspMaster.AfsRecordDetails;
            model.FirstReminderIssueDate = pspMaster.FirstReminderIssueDate;
            model.FirstReminderDeadline = pspMaster.FirstReminderDeadline;
            model.SecondReminderIssueDate = pspMaster.SecondReminderIssueDate;
            model.SecondReminderDeadline = pspMaster.SecondReminderDeadline;
            model.AuditedReportReceivedDate = pspMaster.AuditedReportReceivedDate;
            model.PublicationReceivedDate = pspMaster.PublicationReceivedDate;
            model.NewspaperCuttingReceivedDate = pspMaster.NewspaperCuttingReceivedDate;
            model.OfficialReceiptReceivedDate = pspMaster.OfficialReceiptReceivedDate;
            model.GrossProceed = pspMaster.GrossProceed;
            model.Expenditure = pspMaster.Expenditure;
            model.NetProceed = pspMaster.NetProceed;
            model.ExpPerGpPercentage = pspMaster.GrossProceed == 0 ? 0.00M : pspMaster.Expenditure / pspMaster.GrossProceed * 100;
            model.OrgAnnualIncome = pspMaster.OrgAnnualIncome;
            model.DocReceivedRemark = pspMaster.DocReceivedRemark;
            if (pspMaster.QualifyOpinionIndicator != null)
            {
                model.QualifyOpinionIndicator = pspMaster.QualifyOpinionIndicator.Value;
            }
            model.QualityOpinionDetail = pspMaster.QualityOpinionDetail;
            if (pspMaster.WithholdingListIndicator != null)
            {
                model.WithholdingListIndicator = pspMaster.WithholdingListIndicator.Value;
            }
            model.WithholdingBeginDate = pspMaster.WithholdingBeginDate;
            model.WithholdingEndDate = pspMaster.WithholdingEndDate;
            model.WithholdingRemark = pspMaster.WithholdingRemark;
            if (pspMaster.ArCheckIndicator != null)
            {
                model.ArCheckIndicator = pspMaster.ArCheckIndicator;
            }
            if (pspMaster.PublicationCheckIndicator != null)
            {
                model.PublicationCheckIndicator = pspMaster.PublicationCheckIndicator;
            }
            if (pspMaster.OfficialReceiptCheckIndicator != null)
            {
                model.OfficialReceiptCheckIndicator = pspMaster.OfficialReceiptCheckIndicator;
            }
            if (pspMaster.NewspaperCheckIndicator != null)
            {
                model.NewspaperCuttingCheckIndicator = pspMaster.NewspaperCheckIndicator;
            }
            model.DocRemark = pspMaster.DocRemark;
            model.twEventsRecCount = _pspApprovalHistoryService.ifContainTwoBatchRec(pspMaster.PspMasterId);
            model.prevApHistExistInd = false;
            if (pspMaster.PreviousPspMasterId != null)
            { model.prevApHistExistInd = _pspApprovalHistoryService.HasRecomAprovRecs((int)pspMaster.PreviousPspMasterId) ? true : false; }
            model.prevTwRecCount = pspMaster.PreviousPspMasterId == null ? 0 : _pspApprovalHistoryService.ifContainTwoBatchRec((int)pspMaster.PreviousPspMasterId);
            model.HasPspChild = _pspService.GetChildPspmaster(pspMaster.PspMasterId) == null ? false : true;
            model.PspEventViewModel.ProcessEvents = 0;

            model.OrgEngName = pspMaster.OrgMaster.EngOrgName;
            model.OrgChiName = pspMaster.OrgMaster.ChiOrgName;
            model.OrgRef = pspMaster.OrgMaster.OrgRef;
            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(pspMaster.OrgMaster.OrgId);
            var WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate;
            var WithholdingEndDate = WithHoldingDate.WithholdingEndDate;
            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(pspMaster.OrgMaster.OrgId);
            model.LblWithholdingBeginDate = WithholdingBeginDate;
            model.LblWithholdingEndDate = WithholdingEndDate;
            model.LblPspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "";
            model.LblPspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "";
            model.LblPspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "";
            model.LblFdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "";
            model.LblFdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "";
            model.LblFdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "";
            return model;
        }

        #endregion New

        #region Search

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("Search", Name = "SearchPspMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            this.HttpContext.Session[PSP_SEARCH_SESSION] = null;
            PSPViewModel model = new PSPViewModel();
            model.isFirstSearch = true;            
            initPspViewModel(model, true);
            return View(model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("ReturnSearch", Name = "PSPSearchPage")]
        public ActionResult ReturnSearch()
        {
            PSPViewModel model = new PSPViewModel();
            var session = this.HttpContext.Session[PSP_SEARCH_SESSION];
            if (session != null)
            {
                model = ((PSPViewModel)(this.HttpContext.Session[PSP_SEARCH_SESSION]));
                model.isFirstSearch = false;
            }

            initPspViewModel(model, true);

            return View("Search", model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/list", Name = "ListPspSearch")]
        public JsonResult ListPspSearch(GridSettings grid, PSPViewModel model)
        {
            Ensure.Argument.NotNull(grid);
            var pspMasters = _pspService.GetPagePspSearchDto(grid);

            var gridResult = new GridResult
            {
                TotalPages = pspMasters.TotalPages,
                CurrentPageIndex = pspMasters.CurrentPageIndex,
                TotalCount = pspMasters.TotalCount,
                Data = (
                        from u in pspMasters
                        select new
                        {
                            PspMasterId = u.PspMasterId,
                            OrgRef = u.OrgRef,
                            OrgName = u.OrgName,
                            OrgValidTo_Month = u.OrgValidTo_Month,
                            OrgValidTo_Year = u.OrgValidTo_Year,
                            IVP = u.IVP,
                            SubventedIndicator = u.SubventedIndicator,
                            PspRef = u.PspRef,
                            PreviousPspRef = u.PreviousPspRef,
                            Section88Indicator = u.Section88Indicator,
                            RegDescription = string.IsNullOrEmpty(u.RegType1) ? string.IsNullOrEmpty(u.RegType2) ? "" : getRegDesc(u.RegType2) : getRegDesc(u.RegType1),
                            RegType1 = string.IsNullOrEmpty(u.RegType1) ? "" : regTypes[u.RegType1],
                            RegOtherName1 = u.RegOtherName1,
                            RegType2 = string.IsNullOrEmpty(u.RegType2) ? "" : regTypes[u.RegType2],
                            RegOtherName2 = u.RegOtherName2,
                            PermitNum = u.PermitNum,
                            ApprovalStatus = !String.IsNullOrEmpty(u.ApprovalStatus) ? _lookupService.GetDescription(LookupType.PSPApplicationResult, u.ApprovalStatus) : "",
                            //CR-005 02
                            ProcessingOfficerPost = u.ProcessingOfficerPost,
                            TotalLocation = u.TotalLocation,
                            TotEvent = u.TotEvent,
                            EventApprovedNum = u.EventApprovedNum,
                            //TIR #: PSUAT00037-9 - All events are cancelled but the statistics do not show up.
                            EventHeldNum = u.TotEvent == u.EventCancelledNum ? 0 : u.EventHeldNum,
                            EventCancelledNum = u.EventCancelledNum,
                            EventHeldPercent = u.TotEvent == u.EventCancelledNum ? 0 : u.EventHeldPercent,
                            OverdueIndicator = u.OverdueIndicator,
                            LateIndicator = u.LateIndicator,
                            ReSubmit = u.ReSubmit,
                            ReEvents = "{0} of {1} event(s) not submitted".FormatWith(u.ReEvents, u.TotEvent),
                            isSsaf = u.IsSsaf
                        }
                       ).ToArray()
            };

            this.HttpContext.Session[PSP_SEARCH_SESSION] = model;

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/organisation/list", Name = "ListPSPOrganisation")]
        public JsonResult ListOrganisation(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var organisations = _organisationService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = organisations.TotalPages,
                CurrentPageIndex = organisations.CurrentPageIndex,
                TotalCount = organisations.TotalCount,
                Data = (
                        from u in organisations
                        select new
                        {
                            OrgRef = u.OrgRef,
                            OrgName = u.EngOrgName + System.Environment.NewLine + u.ChiOrgName,
                            OrgEngName = u.EngOrgName,
                            OrgChiName = u.ChiOrgName,
                            SubventedIndicator = u.SubventedIndicator
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/create", Name = "CreatePsp")]
        public JsonResult CreatePsp([CustomizeValidator(RuleSet = "default,Create")] PSPViewModel model, int? previousPspMasterId)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var pspMaster = newPspMasterByModel(model);
            pspMaster.PreviousPspMasterId = null;

            if (previousPspMasterId != null)
            {
                pspMaster.PreviousPspMasterId = previousPspMasterId;
            }

            using (_unitOfWork.BeginTransaction())
            {
                _pspService.CreatePspMaster(pspMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = pspMaster.PspMasterId,
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/exportPspGridData", Name = "ExportPspGridData")]
        public JsonResult ExportPspGridData(ExportSettings exportSettings)
        {
            PSPViewModel model = ((PSPViewModel)(this.HttpContext.Session[PSP_SEARCH_SESSION]));
            var pspMasters = _pspService.GetPagePspSearchDto(exportSettings.GridSettings);
            var dataList = (from u in pspMasters
                            select new
                            {
                                PspMasterId = u.PspMasterId,
                                OrgRef = u.OrgRef,
                                OrgName = u.OrgName,
                                EngOrgName = u.EngOrgName,
                                ChiOrgName = u.ChiOrgName,
                                OrgValidTo_Month = u.OrgValidTo_Month,
                                OrgValidTo_Year = u.OrgValidTo_Year,
                                IVP = u.IVP,
                                SubventedIndicator = u.SubventedIndicator != null ? (u.SubventedIndicator.Value ? "Yes" : "No") : "",
                                PspRef = u.PspRef,
                                PreviousPspRef = u.PreviousPspRef,
                                ApplicationReceiveDate = u.ApplicationReceiveDate,
                                DisableIndicator = u.DisableIndicator != null ? (u.DisableIndicator.Value ? "Yes" : "No") : "",
                                ContactPersonName = u.ContactPersonName,
                                ContactPersonChiName = u.ContactPersonChiName,
                                Section88Indicator = u.Section88Indicator != null ? (u.Section88Indicator.Value ? "Yes" : "No") : "",
                                RegDescription = string.IsNullOrEmpty(u.RegType1) ? string.IsNullOrEmpty(u.RegType2) ? "" : getRegDesc(u.RegType2) : getRegDesc(u.RegType1),
                                RegType1 = string.IsNullOrEmpty(u.RegType1) ? "" : regTypes[u.RegType1],
                                RegOtherName1 = u.RegOtherName1,
                                RegType2 = string.IsNullOrEmpty(u.RegType2) ? "" : regTypes[u.RegType2],
                                RegOtherName2 = u.RegOtherName2,
                                PermitNum = u.PermitNum,
                                EventStartDate = u.EventStartDate,
                                ApprovalStatus = !String.IsNullOrEmpty(u.ApprovalStatus) ? _lookupService.GetDescription(LookupType.PSPApplicationResult, u.ApprovalStatus) : "",
                                RejectReason = u.RejectReason,
                                RejectRemark = u.RejectRemark,
                                //CR-005 02
                                ProcessingOfficerPost = u.ProcessingOfficerPost,
                                TotalLocation = u.TotalLocation,
                                TotEvent = u.TotEvent,
                                EventApprovedNum = u.EventApprovedNum,
                                EventHeldNum = u.EventHeldNum,
                                EventCancelledNum = u.EventCancelledNum,
                                EventHeldPercent = u.EventHeldPercent,
                                OverdueIndicator = u.OverdueIndicator,
                                LateIndicator = u.LateIndicator,
                                ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                                ContactPerson = u.ContactPerson,
                                EngRegisteredAddress = u.EngRegisteredAddress1 + " " + u.EngRegisteredAddress2,
                                ChiRegisteredAddress = u.ChiRegisteredAddress1 + " " + u.ChiRegisteredAddress2,
                                OrgEmailAddress = u.OrgEmailAddress,
                                IsSsaf = u.IsSsaf
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<PSPViewModel>();

            if (model.OrgRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgRef"] + " : ORG" + model.OrgRef);

            if (model.OrgName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgName"] + " : " + model.OrgName);

            if (model.OrgStatusId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrgStatusId]);
            }

            if (model.Subvented != null)
            {
                filterCriterias.Add(fieldNames["Subvented"] + " : " + (model.Subvented.Value ? "Yes" : "No"));
            }

            if (model.SectionId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SectionId"] + " : " + (model.SectionId == "0" ? "False" : "True"));

            if (model.RegistrationType.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationType];
                if (model.RegistrationType == "Others")
                {
                    if (model.RegistrationOtherName.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationType"] + " : " + tempDesc + " ( " + model.RegistrationOtherName + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationType"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationType"] + " : " + tempDesc);
                }
            }

            if (model.RegistrationType2.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationType2];
                if (model.RegistrationType == "Others")
                {
                    if (model.RegistrationOtherName2.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationType2"] + " : " + tempDesc + " ( " + model.RegistrationOtherName2 + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationType2"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationType2"] + " : " + tempDesc);
                }
            }

            if (model.PSPRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PSPRef"] + " : " + model.PSPRef);

            if (model.PrincipalActivities.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PrincipalActivities"] + " : " + model.PrincipalActivities);

            if (model.RecevAppFromDate != null && model.RecevAppToDate != null)
            {
                tmpVal = "From " + model.RecevAppFromDate.Value.ToString(DATE_FORMAT) + " to " + model.RecevAppToDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.RecevAppFromDate != null)
            {
                tmpVal = "From " + model.RecevAppFromDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.RecevAppToDate != null)
            {
                tmpVal = "To " + model.RecevAppToDate.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["DateofReceivingApplication"] + " : " + tmpVal);

            tmpVal = "";
            if (model.PermitNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PermitNo"] + " : " + model.PermitNo);
            if (model.EvenStartFromDate != null && model.EvenStartToDate != null)
            {
                tmpVal = "From " + model.EvenStartFromDate.Value.ToString(DATE_FORMAT) + " to " + model.EvenStartToDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.EvenStartFromDate != null)
            {
                tmpVal = "From " + model.EvenStartFromDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.EvenStartToDate != null)
            {
                tmpVal = "To " + model.EvenStartToDate.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["EvenStartDate"] + " : " + tmpVal);

            tmpVal = "";
            if (model.PspApplicationResult.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["PspApplicationResult"] + " : " + _lookupService.getAllLkpInCodec(LookupType.PSPApplicationResult)[model.PspApplicationResult]);
            }
            if (model.DisposAppFromDate != null && model.DisposAppToDate != null)
            {
                tmpVal = "From " + model.DisposAppFromDate.Value.ToString(DATE_FORMAT) + " to " + model.DisposAppToDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.DisposAppFromDate != null)
            {
                tmpVal = "From " + model.DisposAppFromDate.Value.ToString(DATE_FORMAT);
            }
            else if (model.DisposAppToDate != null)
            {
                tmpVal = "To " + model.DisposAppToDate.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["DateofApplicationDisposal"] + " : " + tmpVal);

            if (model.ProcessingOfficerPost.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ProcessingOfficerPost"] + " : " + (model.ProcessingOfficerPost == "0" ? "False" : "True"));

            if (model.QualityOpinionDetail.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["QualityOpinionDetail"] + " : " + model.QualityOpinionDetail);

            if (model.OverdueIndicator != null)
                filterCriterias.Add(fieldNames["OverdueIndicator"] + " : " + (model.OverdueIndicator.Value ? "False" : "True"));

            if (model.LateIndicator != null)
                filterCriterias.Add(fieldNames["LateIndicator"] + " : " + (model.LateIndicator.Value ? "False" : "True"));
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //CR-005 Rename the column the column name to reflect only single column value
            try
            {
                exportSettings.ColumnModel.names[exportSettings.ColumnModel.models.FindIndex(x => "overdueIndicator".Equals(x.name))] = fieldNames["OverdueIndicator"];
            }
            catch
            {
                
            }

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/ExportPsp", Name = "ExportPsp")]
        public JsonResult ExportPsp(ExportSettings export)
        {
            var ms = _pspService.GetPageToXls(export.GridSettings);

            return JsonFileResult("PspMaster", "PspMaster.xlsx", ms);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/ImportPsp", Name = "ImportPsp")]
        public JsonResult ImportPsp([CustomizeValidator(RuleSet = "ImportPsp")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            MemoryStream logStream;
            using (_unitOfWork.BeginTransaction())
            {
                logStream = _pspService.InserPspByImportXls(model.ImportFile.InputStream);
                if (logStream == null)
                {
                    _unitOfWork.Commit();
                    return Json(new JsonResponse(true)
                    {
                        Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                    }, "text/html", JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    String sessionId = "ImportPspLog";
                    return JsonFileResult(sessionId, "ImportPspLog" + time + ".txt", logStream, "text/html");
                }
            }
        }

        private string getRegDesc(string code)
        {
            var strRegDesc = regTypes[code];

            return regTypes[code];
        }

        #endregion Search

        #region Edit

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("{pspMasterId:int}/Edit", Name = "EditPsp")]
        [RuleSetForClientSideMessagesAttribute("default", "Update", "UpdatePspEvent", "CreatePspEvent", "UpdatePspAttachment")]
        public ActionResult Edit(int pspMasterId, string ReturnUrl)
        {
            Session[PSP_DEFAULT_SEARCH_SESSION] = true;
            Session[PSP_DEFAULT_EXPORT_SESSION] = true;

            PSPViewModel model = new PSPViewModel();
            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + ReturnUrl;
                model.PrePage = prePage;
            }
            var pspMaster = _pspService.GetPSPById(pspMasterId);
            var pspCountEvents = _pspCountEventsService.GetByPspMasterId(pspMasterId);

            //drop down properties
            initPspViewModel(model, false);
            //model = initPspViewModel(model);

            //general properties
            model = getDataFromPspMaster(model, pspMaster);
            model.RowVersion = pspMaster.RowVersion;
            model.OrgMasterId = pspMaster.OrgMaster == null ? "" : pspMaster.OrgMaster.OrgId.ToString();
            model.IsSsaf = pspMaster.IsSsaf.GetValueOrDefault(false);
            
            model.OrgValidTo_Month = pspMaster.OrgMaster.OrgValidTo_Month;
            model.OrgValidTo_Year = pspMaster.OrgMaster.OrgValidTo_Year;
            model.IVP = pspMaster.OrgMaster.IVP;


            if (pspMaster.PreviousPspMasterId != null)
            {
                var prePspMaster = _pspService.GetPSPById((int)pspMaster.PreviousPspMasterId);
                model.PreviousReferenceNumber = prePspMaster.PspRef;
                model.PreviousPermitNum = prePspMaster.PermitNum;
            }
            else
            {
                model.PreviousReferenceNumber = "";
                model.PreviousPermitNum = "";
            }
            
            //Add back the disaster info
            if (model.DisasterId != null && !disasterNamesBySysDate.ContainsKey(model.DisasterId.Value))
            {
                model.DisasterNames.Add(model.DisasterId.Value.ToString(), disasterNames[model.DisasterId.Value]);
            }

            //model.IsShowNewApplicant = pspMaster.OrgMaster == null ? true : _pspService.IsShowNewApplicant(pspMaster.OrgMaster.OrgId);

            ViewData["IS_PSP_APPROVER"] = HttpContext.User.GetPspsUser().IsSysAdmin || HttpContext.User.IsInRole(Allow.PspApprove.GetName());
            ViewData["PSP_REMINDER_DEADLINE"] = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE).Value);
            ViewData["PSP_REMINDER_DEADLINE2"] = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE2).Value);

            return View(model);
        }

        //CR-005 01
        [PspsAuthorize(Allow.PspMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/ReleasePermitNum", Name = "ReleasePspPermitNum")]
        public JsonResult ReleasePermitNum([CustomizeValidator(RuleSet = "Update")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            Ensure.Not(_pspApprovalHistoryService.HasRecomAprovRecs(model.PspMasterId), "Permit Number can't be released as application with Approval History.");
            
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var pspMaster = _pspService.GetPSPById(model.PspMasterId);
            var oldPspMaster = new PspMaster();
            AutoMapper.Mapper.Map(pspMaster, oldPspMaster);

            string permitNum = pspMaster.PermitNum;

            //CR-005 TIR-01
            pspMaster.ApplicationDisposalDate = null;

            pspMaster.PermitNum = null;
            pspMaster.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _pspService.UpdatePspMaster(oldPspMaster, pspMaster);
                _unitOfWork.Commit();
            }
            //pspMaster = _pspService.GetPSPById(model.PspMasterId);
            model.RowVersion = pspMaster.RowVersion;
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = model.PspMasterId
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/Update", Name = "UpdatePsp")]
        public JsonResult Update([CustomizeValidator(RuleSet = "Update")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var pspDeadlineParam = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE); //21
            var pspDeadlineParam2 = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE2); //21
            var pspMaster = _pspService.GetPSPById(model.PspMasterId);
            var oldPspMaster = new PspMaster();
            AutoMapper.Mapper.Map(pspMaster, oldPspMaster);
            var orgMaster = _organisationService.GetOrgByRef(model.CreateModelOrgRef);
            var disasterMaster = model.DisasterId.HasValue ? _disasterMasterService.GetDisasterMasterById((int)model.DisasterId) : null;

            pspMaster.ApplicationDate = model.DateofApplication;
            pspMaster.ApplicationReceiveDate = model.DateofReceivingApplication;
            pspMaster.UsedLanguage = model.LanguageUsedId;
            pspMaster.OrgMaster = orgMaster;
            pspMaster.PspYear = model.YearofPsp;
            pspMaster.ContactPersonSalute = model.EngSalute;
            pspMaster.ContactPersonFirstName = model.ContactPersonFirstName;
            pspMaster.ContactPersonLastName = model.ContactPersonLastName;
            pspMaster.ContactPersonChiFirstName = model.ContactPersonChiFirstName;
            pspMaster.ContactPersonChiLastName = model.ContactPersonChiLastName;
            pspMaster.ContactPersonPosition = model.ContactPersonPost;
            pspMaster.ContactPersonTelNum = model.TelNo;
            pspMaster.ContactPersonFaxNum = model.FaxNo;
            pspMaster.ContactPersonEmailAddress = model.ContactPersonEmailAddress;
            pspMaster.NewApplicantIndicator = model.NewApplicant;
            pspMaster.ProcessingOfficerPost = model.PositionId;
            pspMaster.PspYear = model.YearofPsp;
            pspMaster.ApplicationCompletionDate = model.DateofCompletingApplication;
            pspMaster.ActionBuDate = model.BUDateforAction;
            pspMaster.DisasterMaster = disasterMaster;
            pspMaster.BeneficiaryOrg = model.BeneficiaryOrg;
            pspMaster.EngFundRaisingPurpose = model.PurposeofFundRaising;
            pspMaster.ChiFundRaisingPurpose = model.PurposeofChiFundRaising;
            pspMaster.EventTitle = model.EventTitle;
            pspMaster.EngCharitySalesItem = model.CharitySalesItems;
            pspMaster.ChiCharitySalesItem = model.CharitySalesItemsChi;
            pspMaster.UsedLanguage = model.LanguageUsedId;
            pspMaster.ApplicationDisposalDate = model.DateofApplicationDisposal;
            pspMaster.ApplicationResult = model.ApplicationResultId;
            pspMaster.RejectReason = model.RejectReasonId;
            pspMaster.RejectRemark = model.RejectRemark;
            pspMaster.OtherRejectReason = model.RejectReasonId.IsNullOrEmpty() ? "" : model.RejectReasonId.Equals("Others") ? (model.OtherRejectReason.IsNullOrEmpty() ? "" : (model.OtherRejectReason)) : "";
            pspMaster.PspNotRequireReason = model.PspNotRequireReasonId;
            pspMaster.OtherPspNotRequireReason = model.PspNotRequireReasonId.IsNullOrEmpty() ? "" : model.PspNotRequireReasonId.Equals("Others") ? (model.OtherPspNotRequireReason.IsNullOrEmpty() ? "" : (model.OtherPspNotRequireReason)) : "";
            pspMaster.CaseCloseReason = model.CaseCloseReasonId;
            pspMaster.OtherCaseCloseReason = model.CaseCloseReasonId.IsNullOrEmpty() ? "" : model.CaseCloseReasonId.Equals("Others") ? (model.OtherCaseCloseReason.IsNullOrEmpty() ? "" : (model.OtherCaseCloseReason)) : "";
            pspMaster.SpecialRemark = model.SpecialRemark != null ? string.Join(",", model.SpecialRemark) : string.Empty;
            pspMaster.OtherSpecialRemark = model.OtherSpecialRemark;
            pspMaster.RejectionLetterDate = model.RejectionLetterDate;
            pspMaster.RepresentationReceiveDate = model.RepresentationReceiveDate;
            pspMaster.RepresentationReplyDate = model.RepresentationReplyDate;
            pspMaster.AfsRecordDetails = model.AfsRecordDetails;
            pspMaster.AfsRecordStartDate = model.AfsRecordStartDate;
            pspMaster.AfsRecordEndDate = model.AfsRecordEndDate;
            pspMaster.TrackRecordDetails = model.TrackRecordDetails;
            pspMaster.TrackRecordStartDate = model.TrackRecordStartDate;
            pspMaster.TrackRecordEndDate = model.TrackRecordEndDate;
            pspMaster.PermitRevokeIndicator = model.PermitRevokeIndicator;
            pspMaster.BypassValidationIndicator = model.BypassValidation;
            pspMaster.Section88Indicator = model.CreateModelSection88;

            pspMaster.FundUsed = model.FundUsedId;
            pspMaster.DocSubmission = model.DocSubmission != null ? string.Join(",", model.DocSubmission) : string.Empty;
            pspMaster.SubmissionDueDate = model.SubmissionDueDate;
            pspMaster.FirstReminderIssueDate = model.FirstReminderIssueDate;
            pspMaster.FirstReminderDeadline = model.FirstReminderIssueDate != null ? (DateTime?)model.FirstReminderIssueDate.Value.AddDays(int.Parse(pspDeadlineParam.Value)) : null;
            // pspMaster.FirstReminderDeadline = model.FirstReminderDeadline;
            pspMaster.SecondReminderIssueDate = model.SecondReminderIssueDate;
            pspMaster.SecondReminderDeadline = model.SecondReminderIssueDate != null ? (DateTime?)model.SecondReminderIssueDate.Value.AddDays(int.Parse(pspDeadlineParam2.Value)) : null;
            // pspMaster.SecondReminderDeadline = model.SecondReminderDeadline;
            pspMaster.AuditedReportReceivedDate = model.AuditedReportReceivedDate;
            pspMaster.PublicationReceivedDate = model.PublicationReceivedDate;
            pspMaster.NewspaperCuttingReceivedDate = model.NewspaperCuttingReceivedDate;
            pspMaster.OfficialReceiptReceivedDate = model.OfficialReceiptReceivedDate;
            pspMaster.DocReceivedRemark = model.DocReceivedRemark;
            pspMaster.GrossProceed = model.GrossProceed;
            pspMaster.Expenditure = model.Expenditure;
            pspMaster.NetProceed = model.NetProceed;
            pspMaster.OrgAnnualIncome = model.OrgAnnualIncome;
            pspMaster.QualifyOpinionIndicator = model.QualifyOpinionIndicator;
            pspMaster.QualityOpinionDetail = model.QualityOpinionDetail;
            pspMaster.WithholdingListIndicator = model.WithholdingListIndicator;
            pspMaster.WithholdingBeginDate = model.WithholdingBeginDate;
            pspMaster.WithholdingEndDate = model.WithholdingEndDate;
            pspMaster.WithholdingRemark = model.WithholdingRemark;
            pspMaster.ArCheckIndicator = model.ArCheckIndicator;
            pspMaster.PublicationCheckIndicator = model.PublicationCheckIndicator;
            pspMaster.OfficialReceiptCheckIndicator = model.OfficialReceiptCheckIndicator;
            pspMaster.NewspaperCheckIndicator = model.NewspaperCuttingCheckIndicator;
            pspMaster.DocRemark = model.DocRemark;
            pspMaster.IsSsaf = model.IsSsaf.GetValueOrDefault(false);

            if (pspMaster.PspApprovalHistory.Count == 0)
            {
                pspMaster.EventPeriodFrom = model.DateofEventPeriodFrom;
                pspMaster.EventPeriodTo = model.DateofEventPeriodTo;
            }

            pspMaster.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _pspService.UpdatePspMaster(oldPspMaster, pspMaster);
                _unitOfWork.Commit();

                //Update Withholding Date Info
                WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(pspMaster.OrgMaster.OrgId);
                OrgEditLatestPspFdView orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(pspMaster.OrgMaster.OrgId);

                model.OrgEngName = pspMaster.OrgMaster.EngOrgName;
                model.OrgChiName = pspMaster.OrgMaster.ChiOrgName;
                model.OrgRef = pspMaster.OrgMaster.OrgRef;
                
                model.LblPspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "";
                model.LblPspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "";
                model.LblPspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "";
                model.LblFdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "";
                model.LblFdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "";
                model.LblFdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "";

                model.LblWithholdingBeginDate = WithHoldingDate.WithholdingBeginDate;
                model.LblWithholdingEndDate = WithHoldingDate.WithholdingEndDate;
                model.WithholdingListIndicator = _withholdingHistoryService.GetWithHoldBySysDt(pspMaster.OrgMaster.OrgId);
            }
            //pspMaster = _pspService.GetPSPById(model.PspMasterId);
            model.RowVersion = pspMaster.RowVersion;
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = model
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/CloneTwoBatchPsp", Name = "CloneTwoBatchPsp")]
        public JsonResult CloneTwoBatchPsp([CustomizeValidator(RuleSet = "default,Create")] PSPViewModel model) // two batches psp master
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var pspMaster = newPspMasterByModel(model);
            pspMaster.NewApplicantIndicator = false;
            pspMaster.BypassValidationIndicator = false;

            var pspEvents = _pspEventService.GetPspEventsByPspMasterId(model.PspMasterId).Where(x => x.Value.PspApprovalHistory == null);

            {
                using (_unitOfWork.BeginTransaction())
                {
                    pspMaster.PreviousPspMasterId = model.PspMasterId;
                    _pspService.CreatePspMaster(pspMaster);
                    foreach (var eve in pspEvents)
                    {
                        eve.Value.PspMaster = pspMaster;
                        _pspEventService.UpdatePspEvent(eve.Value);

                        //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                        //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                        //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                        UpdatePspEventDate(ref pspMaster);
                        _pspService.UpdatePspMaster(pspMaster);
                    }
                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = pspMaster.PspMasterId
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/pspAmendment", Name = "PspAmendment")] // psp amendment
        public JsonResult PspAmendment(PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //TIR #: PSUAT00037-1
            //For Amendment, 1) “Event Start Date”, “Event End Date”, “Date of Receiving Application”, “Date of Application” should not be copied
            //               2) “Service Record” and “AFS” information should not be copied

            model.DateofEventPeriodFrom = null;
            model.DateofEventPeriodTo = null;
            // User input New DateofReceivingApplication
            //model.DateofReceivingApplication = null;
            model.DateofApplication = null;

            model.NewApplicant = false;
            model.BypassValidation = false;
                        
            //Clear Application Result Data
            model.DateofCompletingApplication = null;
            model.DateofApplicationDisposal = null;
            model.ApplicationResultId = null;
            model.PermitNo = "";
            model.RejectReasonId = null;
            model.PspNotRequireReasonId = null;
            model.CaseCloseReasonId = null;
            model.SpecialRemark = null;
            model.OtherSpecialRemark = "";
            model.RejectionLetterDate = null;
            model.RepresentationReceiveDate = null;

            //Clear AFS and Track Record info
            model.TrackRecordStartDate = null;
            model.TrackRecordEndDate = null;
            model.TrackRecordDetails = string.Empty;
            model.AfsRecordStartDate = null;
            model.AfsRecordEndDate = null;
            model.AfsRecordDetails = string.Empty;

            return CreatePsp(model, model.PspMasterId);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/templatetab/list", Name = "ListPSPTemplateTab")]
        public JsonResult ListPSPTemplateTab(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var template = _pspDocService.GetPage(grid);

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
                            PspDocId = l.PspDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("{id}/Template/{pspDocId}/Generate", Name = "GeneratePSPTemplate")]
        public ActionResult GeneratePSPTemplate(int pspDocId, int id)
        {
           
            Ensure.Argument.NotNull(pspDocId);
            Ensure.Argument.NotNull(id);

            var template = _pspDocService.GetPspDocById(pspDocId);
            var psp = _pspDocService.GetPspDocViewById(id);
            //CR-005 01
            //Permit Number will be assigned b4 approval
            if (_pspApprovedEventsRepository.getListByPspMasterId(id).Count() > 0)
            {
                psp.Proformas1 = _pspApprovedEventsRepository.getListByPspMasterId(id);
                psp.Proformas1 = psp.Proformas1.OrderBy(x => x.PspEventId).ToList();

                var filtered = _pspEventService.GetPspEventsByPspMasterId(id).Select(x => x.Value).Where(x => new string[] { "RA", "AP", "RC" }.Contains(x.EventStatus) && x.EventStartDate != x.EventEndDate && x.EventStartTime.Value.ToString("HH:mm") == "00:00" && x.EventEndTime.Value.ToString("HH:mm") == "23:59");

                //Filter out the RA cases after approval issued
                bool approved = !psp.PermitIssueDate.IsNullOrDefault();

                psp.Proformas2 = (from x in filtered
                                    where (approved && !x.EventStatus.Equals("RA")) || !approved
                                   
                                    orderby x.PspEventId
                                    select x).ToList();
                //orderby x.District, x.EventStartDate, x.EventStartTime, x.EventEndDate, x.EventEndTime, x.Location

                filtered = _pspEventService.GetPspEventsByPspMasterId(id).Select(x => x.Value).Where(x => new string[] { "RA", "AP", "RC" }.Contains(x.EventStatus) && x.EventStartDate != x.EventEndDate && !(x.EventStartTime.Value.ToString("HH:mm") == "00:00" && x.EventEndTime.Value.ToString("HH:mm") == "23:59"));

                psp.Proformas3 = (from x in filtered
                                    where (approved && !x.EventStatus.Equals("RA")) || !approved
                                    
                                    orderby x.PspEventId
                                    select x).ToList();
                //orderby x.District, x.EventStartDate, x.EventStartTime, x.EventEndDate, x.EventEndTime, x.Location
            }
            else
            {
                psp.Proformas1 = (from x in _pspEventsToProformaRepository.getListByPspMasterId(id)
                                  orderby x.PspEventId
                                  select new PspApprovedEvents
                                    {
                                        PspEventId = x.PspEventId,
                                        EventStartYear = x.EventStartYear,
                                        EventStartMonth = x.EventStartMonth,
                                        EventDays = x.EventDays,
                                        EventStartTime = x.EventStartTime,
                                        EventEndTime = x.EventEndTime,
                                        Location = x.Location,
                                        ChiLocation = x.ChiLocation,
                                        District = x.District,
                                        CollectionMethod = x.CollectionMethod
                                    }).ToList();
                //orderby x.District, x.EventStartYear, x.EventStartMonth, x.EventDays, x.EventStartTime, x.EventEndTime, x.Location


                var filtered = _pspEventService.GetPspEventsByPspMasterId(id).Select(x => x.Value).Where(x => x.EventStartDate != x.EventEndDate && x.EventStartTime.Value.ToString("HH:mm") == "00:00" && x.EventEndTime.Value.ToString("HH:mm") == "23:59");

                psp.Proformas2 = (from x in filtered
                                  orderby x.PspEventId
                                  select x).ToList();
                //orderby x.District, x.EventStartDate, x.EventStartTime, x.EventEndDate, x.EventEndTime, x.Location


                filtered = _pspEventService.GetPspEventsByPspMasterId(id).Select(x => x.Value).Where(x => x.EventStartDate != x.EventEndDate && !(x.EventStartTime.Value.ToString("HH:mm") == "00:00" && x.EventEndTime.Value.ToString("HH:mm") == "23:59"));

                psp.Proformas3 = (from x in filtered
                                  orderby x.PspEventId
                                  select x).ToList();
                //orderby x.District, x.EventStartDate, x.EventStartTime, x.EventEndDate, x.EventEndTime, x.Location

            }

            psp.Proformas1.ForEach(x => x.EventDays = x.EventDays.Replace(",", ", "));


            //foreach (var proforma in psp.Proformas)
            //    proforma.Id = Guid.NewGuid();

            var sysParam = _parameterService.GetParameterByCode("PspTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);
            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            SimpleDocumentGenerator<PspDocView> docGenerator = new SimpleDocumentGenerator<PspDocView>(new DocumentGenerationInfo
            {
                DataContext = psp,
                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
            });

            String message = "";
            if (template.DocNum.ToUpper() == "PSP01" || template.DocNum.ToUpper() == "PSP02")
                if (psp.Proformas2.Count > 0 || psp.Proformas3.Count > 0)
                    message = _messageService.GetMessage(SystemMessage.Info.PrintProforma);

            MemoryStream ms = new MemoryStream(docGenerator.GenerateDocument());

            return JsonFileResult(String.Format("{0}_{1:HHmmssFFFF}", template.DocNum, DateTime.Now), template.DocName + ".docx", ms, message: message);            
            //return File(docGenerator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", template.DocName + ".docx");
        }

        [RuleSetForClientSideMessagesAttribute("default", "Update")]
        public PartialViewResult RenderPSPTemplateModal()
        {
            PspDocViewModel model = new PspDocViewModel();
            return PartialView("_PSPTemplateModal", model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspMasterId:int}/CalPspEditRecCnt", Name = "CalPspEditRecCnt")]
        public JsonResult CalPspEditRecCnt(int pspMasterId)
        {
            Ensure.Argument.NotNull(pspMasterId);
            var map = _pspService.CalPspEditRecCnt(pspMasterId);
            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspMasterId:int}/listPspApprovHist", Name = "ListPspApprovHist")]
        public JsonResult ListPspApprovHist(GridSettings grid, int pspMasterId)
        {
            Ensure.Argument.NotNull(grid);

            var recApprvHists = _pspApprovalHistoryService.GetPspApprovHistSummary(grid, pspMasterId);

            var gridResult = new GridResult
            {
                TotalPages = recApprvHists.TotalPages,
                CurrentPageIndex = recApprvHists.CurrentPageIndex,
                TotalCount = recApprvHists.TotalCount,
                Data = (
                        from u in recApprvHists
                        select new
                        {
                            PspPermitNo = u.PspPermitNo,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            EventStatus = u.EventStatus,
                            ApprType = u.ApprType,
                            TotalEventsToBeApprove = u.TotalEventsToBeApprove,
                            RejectionDate = u.RejectionDate,
                            PermitIssueDate = u.PermitIssueDate,
                            CancelReason = u.CancelReason,
                            Remark = u.Remark,
                            UpdatedOn = u.UpdatedOn,
                            PspApprovalHistoryId = u.PspApprovalHistoryId
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/psp/{pspMasterId:int}/getEveNumByStEndDt", Name = "GetEveNumByStEndDt")]
        public JsonResult GetEveNumByStEndDt(int pspMasterId, PSPViewModel model)
        {
            DateTime? co = model.PspEventViewModel.CutOffDateFrom;
            DateTime? ct = model.PspEventViewModel.CutOffDateTo;
            var eveList = _pspEventService.GetPspEventsByPspMasterId(pspMasterId);
            var count = eveList.Where(x => x.Value.EventStartDate >= co && x.Value.EventEndDate <= ct).ToList().Count();

            return Json(new JsonResponse(true)
            {
                Data = new { count = count },
            }, JsonRequestBehavior.DenyGet);
        }

        private PspMaster newPspMasterByModel(PSPViewModel model)
        {
            var orgMaster = _organisationService.GetOrgByRef(model.CreateModelOrgRef);
            var disasterMaster = model.DisasterId.HasValue ? _disasterMasterService.GetDisasterMasterById((int)model.DisasterId) : null;
            //var newRef = _pspService.GetMaxSeq(model.YearofPsp);
            // Modified by Kavin, PSP Ref. based on the year of Application Receive Date
            var newRef = _pspService.GetMaxSeq(model.DateofReceivingApplication.Value.Year.ToString());

            var PspMaster = new PspMaster();

            PspMaster.PreviousPspMasterId = model.PspMasterId;
            PspMaster.PspRef = newRef;
            PspMaster.UsedLanguage = model.LanguageUsedId;
            PspMaster.OrgMaster = orgMaster;
            PspMaster.PspYear = model.YearofPsp;
            PspMaster.ContactPersonSalute = model.EngSalute;
            PspMaster.ContactPersonFirstName = model.ContactPersonFirstName;
            PspMaster.ContactPersonLastName = model.ContactPersonLastName;
            PspMaster.ContactPersonChiFirstName = model.ContactPersonChiFirstName;
            PspMaster.ContactPersonChiLastName = model.ContactPersonChiLastName;
            PspMaster.ContactPersonPosition = model.ContactPersonPost;
            PspMaster.ContactPersonTelNum = model.TelNo;
            PspMaster.ContactPersonFaxNum = model.FaxNo;
            PspMaster.ContactPersonEmailAddress = model.ContactPersonEmailAddress;
            PspMaster.NewApplicantIndicator = model.NewApplicant;
            PspMaster.ProcessingOfficerPost = model.PositionId;
            PspMaster.EventPeriodFrom = model.DateofEventPeriodFrom;
            PspMaster.EventPeriodTo = model.DateofEventPeriodTo;
            PspMaster.PermitRevokeIndicator = model.PermitRevokeIndicator;
            PspMaster.BypassValidationIndicator = model.BypassValidation;
            PspMaster.Section88Indicator = model.CreateModelSection88;
            PspMaster.ApplicationDate = model.DateofApplication;
            PspMaster.ApplicationReceiveDate = model.DateofReceivingApplication;
            PspMaster.ApplicationCompletionDate = model.DateofCompletingApplication;
            PspMaster.ActionBuDate = model.BUDateforAction;
            PspMaster.DisasterMaster = disasterMaster;
            PspMaster.BeneficiaryOrg = model.BeneficiaryOrg;
            PspMaster.EngFundRaisingPurpose = model.PurposeofFundRaising;
            PspMaster.ChiFundRaisingPurpose = model.PurposeofChiFundRaising;
            PspMaster.EventTitle = model.EventTitle;
            PspMaster.EngCharitySalesItem = model.CharitySalesItems;
            PspMaster.ChiCharitySalesItem = model.CharitySalesItemsChi;
            PspMaster.ApplicationDisposalDate = model.DateofApplicationDisposal;
            PspMaster.ApplicationResult = model.ApplicationResultId;
            PspMaster.RejectReason = model.RejectReasonId;
            PspMaster.RejectRemark = model.RejectRemark;
            PspMaster.OtherRejectReason = model.OtherRejectReason;
            PspMaster.PspNotRequireReason = model.PspNotRequireReasonId;
            PspMaster.OtherPspNotRequireReason = model.OtherPspNotRequireReason;
            PspMaster.CaseCloseReason = model.CaseCloseReasonId;
            PspMaster.OtherCaseCloseReason = model.OtherCaseCloseReason;
            PspMaster.SpecialRemark = model.SpecialRemark != null ? string.Join(",", model.SpecialRemark) : string.Empty;
            PspMaster.OtherSpecialRemark = model.OtherSpecialRemark;
            PspMaster.RejectionLetterDate = model.RejectionLetterDate;
            PspMaster.RepresentationReceiveDate = model.RepresentationReceiveDate;
            PspMaster.RepresentationReplyDate = model.RepresentationReplyDate;
            PspMaster.FundUsed = model.FundUsedId;
            PspMaster.DocSubmission = model.DocSubmission != null ? string.Join(",", model.DocSubmission) : string.Empty;
            PspMaster.TrackRecordStartDate = model.TrackRecordStartDate;
            PspMaster.TrackRecordEndDate = model.TrackRecordEndDate;
            PspMaster.TrackRecordDetails = model.TrackRecordDetails;
            PspMaster.AfsRecordStartDate = model.AfsRecordStartDate;
            PspMaster.AfsRecordEndDate = model.AfsRecordEndDate;
            PspMaster.AfsRecordDetails = model.AfsRecordDetails;
            PspMaster.IsSsaf = model.IsSsaf.GetValueOrDefault(false);

            return PspMaster;
        }

        private void initPspViewModel(PSPViewModel model, bool isSearch)
        {
            this.yearofPspList = _pspService.GetAllPspYearForDropdown();
            if (yearofPspList.Count == 0) yearofPspList.Add("", "");

            model.OrgStatus = orgStatus;
            model.LanguageUseds = languages;
            model.EngSalutes = engSalutes;
            model.ChiSalutes = chiSalutes;
            model.ApplicationResults = pspApplicationResults;
            model.Subventeds = boolStringDict;
            model.Section88s = boolStringDict;
            model.Registrations = regTypes;
            model.ApplyForTwrs = applyForTwrs;
            model.Positions = positions;
            model.PspNotRequireReasons = pspNotRequireReasons;
            model.RejectReasons = rejectReasons;
            model.CaseCloseReasons = caseCloseReasons;
            model.SpecialRemarks = specialRemarks;
            model.DocSubmissions = docSubmissions;
            model.YearofPspList = yearofPspList;



            model.PspEventViewModel.PublicPlaceIndicatorSearchDict = publicPlaceIndicatorSearchDict;
            model.PspEventViewModel.PublicPlaceIndicatorDict = publicPlaceIndicatorDict;

            if (isSearch)
            {
                // Reform the Dictionary of DisasterNames by adding the Sorting Index in the Key
                // (Because the JQGrid of Advance Search would reorder the list by the Key (selectedValue))
                int intSortingIndex = 0;
                model.DisasterNames =
                disasterNames.ToDictionary(k => (++intSortingIndex).ToString() + "[SortingDelimiter]" + k.Key.ToString(), v => v.Value);

                model.PspApplicationResults = pspApplicationResults;
                model.OverdueIndicators = boolStringDict;
                model.LateIndicators = boolStringDict;
                model.YesNos = boolStringDict;
            }
            else
            {
                model.DisasterNames = disasterNamesBySysDate.ToDictionary(k => k.Key.ToString(), v => v.Value);
                model.FundUseds = fundUseds;
                model.SanctionListIndicators = boolStringDict;
                model.QualifyOpinionIndicators = boolStringDict;
                model.WithholdingListIndicators = boolStringDict;
                model.ArCheckIndicators = checkIndicator;
                model.PublicationCheckIndicators = checkIndicator;
                model.OfficialReceiptCheckIndicators = checkIndicator;
                model.NewspaperCuttingCheckIndicators = checkIndicator;
                model.PspEventViewModel.Districts = twrDistricts;
                model.PspEventViewModel.MethodOfCollections = collectionMethods;
                model.PspEventViewModel.EditEventRemarks = pspEventRemarks;
                model.YearofPsp = DateTime.Today.Year.ToString();

                model.ActivityConcerns = activityConcerned;
                model.ComplaintSources = complaintSource;
                model.ComplaintResults = complaintResults;
                model.ProcessStatus = processStatus;
                model.IsSsaf = model.IsSsaf.HasValue && model.IsSsaf.Value ? model.IsSsaf.Value : false;
            }

            ViewData["OrgDisabledOrWithheld"] = _messageService.GetMessage(SystemMessage.Warning.Psp.OrgDisabledOrWithheld);
            ViewData["DiffSection88"] = _messageService.GetMessage(SystemMessage.Warning.Psp.DiffSection88);
        }

        #endregion Edit

        #region Report

        [PspsAuthorize(Allow.PspReport)]
        [RuleSetForClientSideMessagesAttribute("default", "R1", "R2", "R4", "R5", "R6", "R21", "R27")]
        public ActionResult Report()
        {
            PspReportViewModel model = new PspReportViewModel();
            model.R6_Disaster = _disasterMasterService.GetAllDisasterMasterForDropdown();
            return View(model);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r1/generate", Name = "R1Generate")]
        public JsonResult R1Generate([CustomizeValidator(RuleSet = "default,R1")]PspReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R01";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR1Excel(templatePath, model.R1_YearFrom, model.R1_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r27/generate", Name = "R27Generate")]
        public JsonResult R27Generate([CustomizeValidator(RuleSet = "default,R27")]PspReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R27";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR27Excel(templatePath, model.R27_YearFrom, model.R27_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r2/generate", Name = "R2Generate")]
        public JsonResult R2Generate([CustomizeValidator(RuleSet = "default,R2")]PspReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R02";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR2Excel(templatePath, model.R2_YearFrom, model.R2_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r4/generate", Name = "R4Generate")]
        public JsonResult R4Generate([CustomizeValidator(RuleSet = "default,R4")]PspReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R04";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR4Excel(templatePath, model.R4_YearFrom, model.R4_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r5/generate", Name = "R5Generate")]
        public JsonResult R5Generate(PspReportViewModel model)
        {
            var reportId = "R05";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR5Excel(templatePath, model.R5_Year.Value);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r6/generate", Name = "R6Generate")]
        public JsonResult R6Generate([CustomizeValidator(RuleSet = "default,R6")] PspReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var reportId = "R06";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR6Excel(templatePath, model.R6_DisasterId.Value);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r7/generate", Name = "R7Generate")]
        public JsonResult R7Generate(PspReportViewModel model)
        {
            var reportId = "R07";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR7Excel(templatePath, model.R7_YearFrom, model.R7_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r21/generate", Name = "R21Generate")]
        public JsonResult R21Generate([CustomizeValidator(RuleSet = "default,R21")] PspReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R21";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR21Excel(templatePath, model.R21_Years);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r22/generate", Name = "R22Generate")]
        public JsonResult R22Generate(PspReportViewModel model)
        {
            var reportId = "R22";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR22Excel(templatePath, model.R22_FromDate, model.R22_ToDate);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspReport)]
        [HttpPost, Route("~/api/report/r23/generate", Name = "R23Generate")]
        public JsonResult R23Generate(PspReportViewModel model)
        {
            var reportId = "R23";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR23Excel(templatePath, model.R23_YearFrom, model.R23_YearTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion Report

        #region Export Psp

        [PspsAuthorize(Allow.OrgReport)]
        [HttpPost, Route("~/api/report/rawdata2/generate", Name = "RawData2Generate")]
        public JsonResult RawData2Generate([CustomizeValidator(RuleSet = "default,Raw2")] PspReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "RawDataOfPSP";
            string strTable = "PspRawView";
            string strWhere = null;

            if (model.Raw2_YearFrom != null)
                strWhere = "DateofReceivingApplication >= '{0}'".FormatWith(model.Raw2_YearFrom.Value.ToString("yyyy-MM-dd"));

            if (model.Raw2_YearTo != null)
                strWhere += (strWhere != null ? " AND " : "") + "DateofReceivingApplication <= '{0}'".FormatWith(model.Raw2_YearTo.Value.ToString("yyyy-MM-dd"));

            if (strWhere != null)
                strWhere = "WHERE " + strWhere;

            Dictionary<string, string> fieldNames = new Dictionary<string, string>();
            var properties = typeof(PSPViewModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DisplayAttribute attr = (DisplayAttribute)System.Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
                if (attr != null)
                    fieldNames.Add(property.Name, attr.GetName());
            }

            var ms = _reportService.ExportTableToExcel(reportId, strTable, fieldNames, strWhere);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion Export Psp

        #region Complaint & Enquiry

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{code}/listcomplaintenquiry/{type}", Name = "ListPspComplaintEnquiry")]
        public JsonResult ListPspComplaintEnquiry(GridSettings grid, string code, string type)
        {
            grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "pspRef",
                        data = code,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "complaintRecordType",
                        data = type,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                });

            var list = this._complaintMasterService.GetPageByComplaintMasterSearchView(grid, false, false, false, false, null, null);

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
                            WithholdingBeginDate = c.WithholdingBeginDate,
                            WithholdingEndDate = c.WithholdingEndDate,
                            WithholdingRemark = c.WithholdingRemark,
                            FollowUpAction = c.FollowUpAction,
                            PspRef = c.PspRef,
                            PspPermitNum = c.PspPermitNum,
                            ReplyDueDate = c.ReplyDueDate,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Complaint & Enquiry

        //#region Complaint

        //[PspsAuthorize(Allow.PspMaster)]
        //[HttpGet, Route("~/api/psp/complaint/{pspMasterId}/listpspcomplaint", Name = "ListPspComplaint")]
        //public JsonResult ListPspComplaint(GridSettings grid, int pspMasterId)
        //{
        //    Ensure.Argument.NotNull(grid);

        // var complaintMaster = _organisationService.getPageComplaintByPspMasterId(grid, pspMasterId);

        // var gridResult = new GridResult { TotalPages = complaintMaster.TotalPages,
        // CurrentPageIndex = complaintMaster.CurrentPageIndex, TotalCount =
        // complaintMaster.TotalCount, Data = ( from u in complaintMaster select new { ComplaintRef
        // = string.IsNullOrEmpty(u.ComplaintRef) ? "" : u.ComplaintRef, ComplaintSource =
        // string.IsNullOrEmpty(u.ComplaintSource) ? "" : complaintSource[u.ComplaintSource],
        // ActivityConcern = string.IsNullOrEmpty(u.ActivityConcern) ? "" :
        // activityConcerned[u.ActivityConcern], ComplaintDate = u.ComplaintDate, PermitNum =
        // string.IsNullOrEmpty(u.PermitNum) ? "" : u.PermitNum, FollowUpLetterType =
        // string.IsNullOrEmpty(u.FollowUpLetterType) ? "" : issueLetterType[u.FollowUpLetterType],
        // FollowUpLetterIssueDate = u.FollowUpLetterIssueDate, LetterIssuedNum = u.LetterIssuedNum,
        // ComplaintRemarks = u.ComplaintRemarks, ComplaintMasterId = u.ComplaintMasterId, }
        // ).ToArray() };

        //    return Json(new JsonResponse(true)
        //    {
        //        Data = gridResult
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //#endregion Complaint

        //#region Enquiry

        //[PspsAuthorize(Allow.PspMaster)]
        //[HttpGet, Route("~/api/psp/enquiry/{pspMasterId}/listPspEnquiry", Name = "ListPspEnquiry")]
        //public JsonResult ListPspEnquiry(GridSettings grid, int pspMasterId)
        //{
        //    Ensure.Argument.NotNull(grid);

        // var complaintMaster = _organisationService.getPageEnquiryByPspMasterId(grid, pspMasterId);

        // var gridResult = new GridResult { TotalPages = complaintMaster.TotalPages,
        // CurrentPageIndex = complaintMaster.CurrentPageIndex, TotalCount =
        // complaintMaster.TotalCount, Data = ( from u in complaintMaster select new { ComplaintRef
        // = string.IsNullOrEmpty(u.ComplaintRef) ? "" : u.ComplaintRef, ComplaintSource =
        // string.IsNullOrEmpty(u.ComplaintSource) ? "" : complaintSource[u.ComplaintSource],
        // ActivityConcern = string.IsNullOrEmpty(u.ActivityConcern) ? "" :
        // activityConcerned[u.ActivityConcern], ComplaintDate = u.ComplaintDate, PermitNum =
        // string.IsNullOrEmpty(u.PermitNum) ? "" : u.PermitNum, FollowUpLetterType =
        // string.IsNullOrEmpty(u.FollowUpLetterType) ? "" : issueLetterType[u.FollowUpLetterType],
        // FollowUpLetterIssueDate = u.FollowUpLetterIssueDate, LetterIssuedNum = u.LetterIssuedNum,
        // ComplaintRemarks = u.ComplaintRemarks, ComplaintMasterId = u.ComplaintMasterId, }
        // ).ToArray() };

        //    return Json(new JsonResponse(true)
        //    {
        //        Data = gridResult
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //#endregion Enquiry

        #region PSP Master Template

        [PspsAuthorize(Allow.PspTemplate)]
        public ActionResult Template()
        {
            PspDocViewModel model = new PspDocViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpGet, Route("~/api/psp/template/list", Name = "ListPspTemplate")]
        public JsonResult ListPspTemplate(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var psp = _pspDocService.GetPspDocSummaryViewPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = psp.TotalPages,
                CurrentPageIndex = psp.CurrentPageIndex,
                TotalCount = psp.TotalCount,
                Data = (from p in psp
                        select new
                        {
                            DocNum = p.DocNum,
                            DocName = p.DocName,
                            VersionNum = p.VersionNum,
                            Enabled = p.Enabled,
                            PspDocId = p.PspDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/template/new", Name = "NewPspDoc")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,Create,CreatePspDoc")] PspDocViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_TEMPLATE_PATH);
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            var pspDoc = new PspDoc();
            pspDoc.DocNum = model.DocNum;
            pspDoc.DocStatus = true;
            pspDoc.DocName = model.Description;
            pspDoc.VersionNum = model.Version;
            pspDoc.FileLocation = relativePath;
            pspDoc.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                if (CommonHelper.CreateFolderIfNeeded(templatePath.Value))
                {
                    model.File.SaveAs(absolutePath);
                }
                this._pspDocService.CreatePspDoc(pspDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/template/{pspDocId:int}/edit", Name = "EditPspVersion")]
        public JsonResult Edit(int pspDocId, [CustomizeValidator(RuleSet = "default,UpdatePspDoc")]  PspDocViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(pspDocId.ToString());
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            PspDoc psp = new PspDoc();
            psp = this._pspDocService.GetPspDocById(pspDocId);
            psp.DocNum = model.DocNum;
            psp.DocName = model.Description;
            psp.VersionNum = model.Version;
            psp.DocStatus = model.IsActive;

            using (_unitOfWork.BeginTransaction())
            {
                this._pspDocService.UpdatePspDoc(psp);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion PSP Master Template

        #region Event

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("Events", Name = "ReadPspEvents")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update", "ImportProforma")]
        public JsonResult ReadEvents(Psps.Core.JqGrid.Models.Filter filter, int pspMasterId)
        {
            //Psps.Core.JqGrid.Models.Filter

            PSPViewModel model = new PSPViewModel();
            var pspMaster = _pspService.GetPSPById(pspMasterId);
            //var orgmaster = _organisationService.GetOrgById(pspMaster.OrgMaster.OrgId);
            //model.CreateModelReferenceNumber = pspMaster.PspRef;

            model.hasRecomApproFlag = _pspApprovalHistoryService.HasRecomAprovRecs(pspMasterId);

            List<PspEvent> pspEvents = _pspEventService.GetPspEventIds(pspMasterId);

            //model.approvedEventsCount = _pspEventService.GetNumApprovedEvents(pspMasterId);
            //model.PspEventViewModel.TotalEvents = _pspEventService.GetNumOfRemainingRecs(pspMasterId);

            if (pspEvents.Any(x => x.EventStatus == "CA" || x.EventStatus == "RC" || x.EventStatus == "AP"))
                model.PspEventViewModel.TotalEvents = pspEvents.Count(x => x.PspApprovalHistory != null && x.PspCancelHistory == null);
            else
                model.PspEventViewModel.TotalEvents = pspEvents.Count(x => x.PspApprovalHistory == null && x.PspCancelHistory == null);

            /*
            var eventFilter = pspEvents;
            if (pspEvents.Any(x => x.EventStatus == "CA" || x.EventStatus == "RA" || x.EventStatus == "RC" || x.EventStatus == "AP"))
                if (pspEvents.Any(x => x.EventStatus == "CA" || x.EventStatus == "RC" || x.EventStatus == "AP"))
                    eventFilter = pspEvents.Where(x => new string[] { "AP", "RC" }.Contains(x.EventStatus)).ToList();
                else
                    eventFilter = pspEvents.Where(x => x.EventStatus.Equals("RA")).ToList();

            model.TotalEvent = Convert.ToInt32(eventFilter.Sum(x => (x.EventEndDate - x.EventStartDate).Value.TotalDays + 1));
            model.TotalLocation = eventFilter.GroupBy(x => new {x.District, x.Location, x.ChiLocation, x.SimpChiLocation}).Select(x => x.First()).Count();
            */

            model.approvedEventsCount = pspEvents.Count(x => x.EventStatus == "AP");
            if (filter != null)
                model.PspEveLst = (from u in pspEvents.AsQueryable().Where(filter)
                                   select new { u.PspEventId, u.EventStatus }).ToDictionary(k => k.PspEventId, v => v.EventStatus);
            else
                model.PspEveLst = (from u in pspEvents
                                   select new { u.PspEventId, u.EventStatus }).ToDictionary(k => k.PspEventId, v => v.EventStatus);

            //model.hasCancelledFlag = _pspApprovalHistoryService.HasCancelledRecs(pspMasterId);
            model.PspEventViewModel.BypassValidation = pspMaster.BypassValidationIndicator != null ? (bool)pspMaster.BypassValidationIndicator : false;
            model.DateofEventPeriodFrom = pspMaster.EventPeriodFrom;
            model.DateofEventPeriodTo = pspMaster.EventPeriodTo;

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspMasterId}/event/list", Name = "ListPspEventsByPspMasterId")]
        public JsonResult ListPspEventsByPspMasterId(GridSettings grid, int pspMasterId)
        {
            Ensure.Argument.NotNull(grid);
            var pspEvent = _pspEventService.GetPageByPspMasterId(grid, pspMasterId);

            bool defaultSearch = Session[PSP_DEFAULT_SEARCH_SESSION] == null ? true : (bool)Session[PSP_DEFAULT_SEARCH_SESSION];

            string sord = "1";
            //PropertyInfo pi = null;
            if (defaultSearch)
            {
                Session[PSP_DEFAULT_SEARCH_SESSION] = false;
            }
            else
            {
                //non-first time sorting
                Session[PSP_DEFAULT_EXPORT_SESSION] = false;
            }


            var result = from u in pspEvent
                         orderby (defaultSearch ? "u.EventStartDate, u.EventEndDate, u.EventStartTime, u.EventEndTime, u.Location" : sord)
                         select new PspReadEventDto
                         {
                             PspEventId = u.PspEventId,
                             BatchNum = u.BatchNum,
                             EventCount = u.EventCount,
                             EventStartDate = u.EventStartDate,
                             EventEndDate = u.EventEndDate,
                             Time = u.Time,
                             District = u.District,
                             Location = u.Location,
                             ChiLocation = u.ChiLocation,
                             CollectionMethod = string.IsNullOrEmpty(u.CollectionMethod) ? "" : u.CollectionMethod,
                             PublicPlaceIndicator = u.PublicPlaceIndicator,
                             EventStatus = u.EventStatus,
                             Remarks = string.IsNullOrEmpty(u.Remarks) ? "" : u.Remarks.Replace("S", pspEventRemarks["S"]),
                             ValidationMessage = u.ValidationMessage,
                             UpdatedOn = u.UpdatedOn,
                         };

            var gridResult = new GridResult
            {
                TotalPages = pspEvent.TotalPages,
                CurrentPageIndex = pspEvent.CurrentPageIndex,
                TotalCount = pspEvent.TotalCount,
                Data = (
                        result
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        // import proforma
        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{pspMasterId}/event/import", Name = "InsertPspEventsByImportXls")]
        public JsonResult InsertPspEventsByImportXls(int pspMasterId, [CustomizeValidator(RuleSet = "ImportProforma")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            using (_unitOfWork.BeginTransaction())
            {
                var pspMaster = _pspService.GetPSPById(pspMasterId);

                var logStream = _pspEventService.InsertPspEventByImportXls(model.PspEventViewModel.ImportFile.InputStream, pspMasterId);
                if (logStream != null && logStream.Length > 3)
                {
                    _unitOfWork.Rollback();
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    String sessionId = "ImportProformaLog";
                    return JsonFileResult(sessionId, "Proforma_Upload_Log_" + time + ".txt", logStream, "text/html");
                }
                else
                {
                    //CR-005 01
                    if (pspMaster.PermitNum.IsNullOrEmpty())
                        pspMaster.PermitNum = getPermitNum(pspMaster);

                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);

                    pspMaster.RowVersion = model.RowVersion;
                    _pspService.UpdatePspMaster(pspMaster);
                    _unitOfWork.Commit();
                    model.RowVersion = pspMaster.RowVersion;
                    return Json(new JsonResponse(true)
                    {
                        Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                        Data = new
                        {
                            RowVersion = model.RowVersion,
                            EventPeriodFrom = pspMaster.EventPeriodFrom,
                            EventPeriodTo = pspMaster.EventPeriodTo,
                            PermitNum = pspMaster.PermitNum
                        }
                    }, "text/html", JsonRequestBehavior.DenyGet);
                }
            }
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspEventId}/geteventfromgrid", Name = "GetEventFromGrid")]
        public JsonResult GetEventFromGrid(int pspEventId)
        {
            Ensure.Argument.NotNull(pspEventId);

            var pspEvent = _pspEventService.GetPspEventById(pspEventId);

            if (pspEvent == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }

            var pspMaster = _pspService.GetPSPById(pspEvent.PspMaster.PspMasterId);
            var model = new PSPViewModel();

            // Fill the Row Version of PSP Master (need to used in PSP >> Approve >> Edit)
            model.RowVersion = pspMaster.RowVersion;

            //DisasterMasterId = disastersMaster != null ? disastersMaster.DisasterMasterId : 0,
            model.PspEventViewModel.BypassValidation = pspMaster.BypassValidationIndicator != null ? pspMaster.BypassValidationIndicator.Value : false;
            model.PspEventViewModel.EventStartDate = pspEvent.EventStartDate;
            model.PspEventViewModel.EventEndDate = pspEvent.EventEndDate;
            model.PspEventViewModel.EventStartTime = pspEvent.EventStartTime.Value.ToString("HH") + pspEvent.EventStartTime.Value.ToString("mm");
            model.PspEventViewModel.EventEndTime = pspEvent.EventEndTime.Value.ToString("HH") + pspEvent.EventEndTime.Value.ToString("mm");
            model.PspEventViewModel.District = pspEvent.District;
            model.PspEventViewModel.Location = pspEvent.Location;
            model.PspEventViewModel.ChiLocation = pspEvent.ChiLocation;
            model.PspEventViewModel.SimpChiLocation = pspEvent.SimpChiLocation;
            //model.PspEventViewModel.PublicPlace = pspEvent.PublicPlaceIndicator.HasValue ? (bool)pspEvent.PublicPlaceIndicator : false;
            //model.PspEventViewModel.MethodOfCollection = pspEvent.CollectionMethod.Split(',').ToList();
            model.PspEventViewModel.MethodOfCollection = string.IsNullOrEmpty(pspEvent.CollectionMethod) ? new List<string>() : pspEvent.CollectionMethod.Split(',').ToList();
            model.PspEventViewModel.OtherMethodOfCollection = pspEvent.OtherCollectionMethod;
            model.PspEventViewModel.CharitySalesItem = pspEvent.CharitySalesItem;
            model.PspEventViewModel.EditEventRemark = string.IsNullOrEmpty(pspEvent.Remarks) ? new List<string>() : pspEvent.Remarks.Split(',').ToList();
            model.PspEventViewModel.EventStatus = pspEvent.EventStatus;
            model.PspEventViewModel.ValidationMessage = pspEvent.ValidationMessage;
            model.PspEventViewModel.RowVersion = pspEvent.RowVersion;
            model.PspEventViewModel.PublicPlaceIndicator = pspEvent.PublicPlaceIndicator;

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/event/update", Name = "UpdatePspEvent")]
        public JsonResult UpdatePspEvent([CustomizeValidator(RuleSet = "default,UpdatePspEvent")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);
            List<string> byPassedErrors = new List<string>();

            // If Model State is invalid
            if (!ModelState.IsValid)
            {
                // Loop all the Errors Message
                foreach (ModelState _modelState in ModelState.Values)
                {
                    for (int i = _modelState.Errors.Count - 1; i >= 0; i--)
                    {
                        if (_modelState.Errors[i].ErrorMessage.Contains("(Bypassed)"))
                        {
                            byPassedErrors.Add(_modelState.Errors[i].ErrorMessage);
                            _modelState.Errors.RemoveAt(i);
                        }
                    }
                }

                // Return error message if there are any error besides By-passed validation
                if (!ModelState.IsValid)
                {
                    return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
                }
            }

            if (model != null)
            {
                PspEvent pspEvent = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
                PspMaster pspMaster = pspEvent.PspMaster; //_pspService.GetPSPById(pspEvent.PspMaster.PspMasterId);
                pspMaster.RowVersion = model.RowVersion;

                pspEvent.EventStartDate = model.PspEventViewModel.EventStartDate;
                pspEvent.EventEndDate = model.PspEventViewModel.EventEndDate;
                pspEvent.EventStartTime = new DateTime(
                                            model.PspEventViewModel.EventStartDate.Value.Year,
                                            model.PspEventViewModel.EventStartDate.Value.Month,
                                            model.PspEventViewModel.EventStartDate.Value.Day,
                                            Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(0, 2)),
                                            Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(2, 2)), 0);
                pspEvent.EventEndTime = new DateTime(
                                            model.PspEventViewModel.EventEndDate.Value.Year,
                                            model.PspEventViewModel.EventEndDate.Value.Month,
                                            model.PspEventViewModel.EventEndDate.Value.Day,
                                            Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(0, 2)),
                                            Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(2, 2)), 0);
                pspEvent.District = model.PspEventViewModel.District;
                pspEvent.Location = model.PspEventViewModel.Location;
                pspEvent.ChiLocation = model.PspEventViewModel.ChiLocation;
                pspEvent.SimpChiLocation = model.PspEventViewModel.SimpChiLocation;
                pspEvent.CollectionMethod = model.PspEventViewModel.MethodOfCollection != null ? string.Join(",", model.PspEventViewModel.MethodOfCollection) : string.Empty;
                pspEvent.OtherCollectionMethod = model.PspEventViewModel.OtherMethodOfCollection;
                pspEvent.CharitySalesItem = model.PspEventViewModel.CharitySalesItem;
                pspEvent.Remarks = model.PspEventViewModel.EditEventRemark != null ? string.Join(",", model.PspEventViewModel.EditEventRemark) : string.Empty;
                pspEvent.RowVersion = model.PspEventViewModel.RowVersion;
                pspEvent.PublicPlaceIndicator = model.PspEventViewModel.PublicPlaceIndicator;

                if (byPassedErrors.Count > 0)
                {
                    string validationMessage = string.Empty;

                    if (byPassedErrors.Any(x => x.Contains("flag date") || x.Contains("solicitation")))
                    {
                        validationMessage = " (Date of Flag Day / Solicit Date validation bypassed)";
                    }

                    if (byPassedErrors.Any(x => x.Contains("Duplicate")))
                    {
                        validationMessage += (validationMessage.Length > 0 ? "," : "") + " (Duplicated record and overlapped time bypassed)";
                    }

                    if (byPassedErrors.Any(x => x.Contains("Event End Date must be earlier than")))
                    {
                        validationMessage += (validationMessage.Length > 0 ? "," : "") + " (Event Period validation bypassed)";
                    }

                    pspEvent.ValidationMessage = "Validated" + validationMessage;
                }
                else
                {
                    pspEvent.ValidationMessage = "Validated";
                }

                using (_unitOfWork.BeginTransaction())
                {
                    _pspEventService.UpdatePspEvent(pspEvent);

                    // Added by Kavin for update EventPeriodStart and EventPeriodEnd
                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);
                    _pspService.UpdatePspMaster(pspMaster);
                    // End Added

                    _unitOfWork.Commit();
                }

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                    Data = new
                    {
                        RowVersion = pspMaster.RowVersion,
                        EventPeriodFrom = pspMaster.EventPeriodFrom,
                        EventPeriodTo = pspMaster.EventPeriodTo
                    }
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage("Update unsuccessful"),
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{pspMasterId}/event/create", Name = "CreatePspEvent")]
        public JsonResult CreatePspEvent([CustomizeValidator(RuleSet = "default,CreatePspEvent")] PSPViewModel model, int pspMasterId)
        {
            Ensure.Argument.NotNull(model);
            List<string> byPassedErrors = new List<string>();

            // If Model State is invalid
            if (!ModelState.IsValid)
            {
                // Loop all the Errors Message
                foreach (ModelState _modelState in ModelState.Values)
                {
                    for (int i = _modelState.Errors.Count - 1; i >= 0; i--)
                    {
                        if (_modelState.Errors[i].ErrorMessage.Contains("(Bypassed)"))
                        {
                            byPassedErrors.Add(_modelState.Errors[i].ErrorMessage);
                            _modelState.Errors.RemoveAt(i);
                        }
                    }
                }

                // Return error message if there are any error besides By-passed validation
                if (!ModelState.IsValid)
                {
                    return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
                }
            }

            var messageService = Psps.Core.Infrastructure.EngineContext.Current.Resolve<IMessageService>();
            var pspEventService = Psps.Core.Infrastructure.EngineContext.Current.Resolve<IPspEventService>();

            var pspMaster = _pspService.GetPSPById(pspMasterId);
            //CR-005 01
            if (pspMaster.PermitNum.IsNullOrEmpty())
                pspMaster.PermitNum = getPermitNum(pspMaster);

            pspMaster.RowVersion = model.RowVersion;

            var pspEvent = new PspEvent();
            pspEvent.PspMaster = pspMaster;
            pspEvent.EventStartDate = model.PspEventViewModel.EventStartDate;
            pspEvent.EventEndDate = model.PspEventViewModel.EventEndDate;
            pspEvent.EventStartTime = model.PspEventViewModel.EventStartDate == null ? new DateTime() : new DateTime(
                                        model.PspEventViewModel.EventStartDate.Value.Year,
                                        model.PspEventViewModel.EventStartDate.Value.Month,
                                        model.PspEventViewModel.EventStartDate.Value.Day,
                                        Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(0, 2)),
                                        Convert.ToInt32(model.PspEventViewModel.EventStartTime.Substring(2, 2)), 0);
            pspEvent.EventEndTime = model.PspEventViewModel.EventStartDate == null ? new DateTime() : new DateTime(
                                        model.PspEventViewModel.EventEndDate.Value.Year,
                                        model.PspEventViewModel.EventEndDate.Value.Month,
                                        model.PspEventViewModel.EventEndDate.Value.Day,
                                        Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(0, 2)),
                                        Convert.ToInt32(model.PspEventViewModel.EventEndTime.Substring(2, 2)), 0);
            pspEvent.District = model.PspEventViewModel.District;
            pspEvent.Location = model.PspEventViewModel.Location;
            pspEvent.ChiLocation = model.PspEventViewModel.ChiLocation;
            pspEvent.SimpChiLocation = model.PspEventViewModel.SimpChiLocation;
            pspEvent.CollectionMethod = model.PspEventViewModel.MethodOfCollection != null ? string.Join(",", model.PspEventViewModel.MethodOfCollection) : string.Empty;
            pspEvent.OtherCollectionMethod = model.PspEventViewModel.OtherMethodOfCollection;
            pspEvent.CharitySalesItem = model.PspEventViewModel.CharitySalesItem;
            pspEvent.Remarks = model.PspEventViewModel.EditEventRemark != null ? string.Join(",", model.PspEventViewModel.EditEventRemark) : string.Empty;
            pspEvent.PublicPlaceIndicator = model.PspEventViewModel.PublicPlaceIndicator;

            if (byPassedErrors.Count > 0)
            {
                string validationMessage = string.Empty;

                if (byPassedErrors.Any(x => x.Contains("flag date") || x.Contains("solicitation")))
            {
                    validationMessage = " (Date of Flag Day / Solicit Date validation bypassed)";
                }

                if (byPassedErrors.Any(x => x.Contains("Duplicate")))
                {
                    validationMessage += (validationMessage.Length > 0 ? "," : "") + " (Duplicated record and overlapped time bypassed)";
                }

                if (byPassedErrors.Any(x => x.Contains("Event End Date must be earlier than")))
                {
                    validationMessage += (validationMessage.Length > 0 ? "," : "") + " (Event Period validation bypassed)";
                }

                pspEvent.ValidationMessage = "Validated" + validationMessage;
            }
            else
            {
                pspEvent.ValidationMessage = "Validated";
            }

            using (_unitOfWork.BeginTransaction())
            {
                _pspEventService.CreatePspEvent(pspEvent);

                // Added by Kavin for update EventPeriodStart and EventPeriodEnd
                //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                UpdatePspEventDate(ref pspMaster);
                _pspService.UpdatePspMaster(pspMaster);
                // End Added

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = new
                {
                    RowVersion = pspMaster.RowVersion,
                    EventPeriodFrom = pspMaster.EventPeriodFrom,
                    EventPeriodTo = pspMaster.EventPeriodTo,
                    PermitNum = pspMaster.PermitNum
                }
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/event/delete", Name = "DeletePspEvent")]
        public JsonResult DeletePspEvent(PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);
            Ensure.Argument.NotNull(model.PspEventViewModel);
            Ensure.Argument.NotNull(model.PspEventViewModel.PspEventId);

            PspEvent pspEvent = _pspEventService.GetPspEventById(model.PspEventViewModel.PspEventId.Value);
            PspMaster pspMaster = _pspService.GetPSPById(model.PspMasterId);
            if (pspEvent != null && pspMaster != null && pspEvent.PspMaster.PspMasterId == model.PspMasterId)
            {
                using (_unitOfWork.BeginTransaction())
                {
                    // Added by Kavin for update EventPeriodStart and EventPeriodEnd
                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);

                    pspMaster.RowVersion = model.RowVersion;
                    _pspService.UpdatePspMaster(pspMaster);
                    // End Added

                    _pspEventService.DeletePspEvent(pspEvent);
                    _unitOfWork.Commit();
                }
                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                    Data = new
                    {
                        RowVersion = pspMaster.RowVersion,
                        EventPeriodFrom = pspMaster.EventPeriodFrom,
                        EventPeriodTo = pspMaster.EventPeriodTo
                    }
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/event/batchdelete", Name = "BatchDeletePspEvents")]
        public JsonResult BatchDeletePspEvents(PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            var pspEventList = getPspEventList(model, "");
            var pspMaster = _pspService.GetPSPById(model.PspMasterId);

            if (pspEventList.Count() != 0 && pspEventList != null)
            {
                using (_unitOfWork.BeginTransaction())
                {
                    foreach (var eve in pspEventList)
                    {
                        var pspEvent = eve.Value;
                        _pspEventService.DeletePspEvent(pspEvent);
                    }
                    // Added by Kavin for update EventPeriodStart and EventPeriodEnd
                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);
                    pspMaster.RowVersion = model.RowVersion;
                    _pspService.UpdatePspMaster(pspMaster);
                    // End Added

                    _unitOfWork.Commit();
                }
                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                    Data = new
                    {
                        RowVersion = pspMaster.RowVersion,
                        EventPeriodFrom = pspMaster.EventPeriodFrom,
                        EventPeriodTo = pspMaster.EventPeriodTo
                    }
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspMasterId}/event/exporttoproforma", Name = "ExportToProforma")]
        public JsonResult ExportToProforma(int pspMasterId)
        {
            var ms = _pspEventService.GetMemoryStreamByPspMasterId(pspMasterId);

            return JsonReportResult("Proforma", "Proforma_" + string.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HHmmss")) + ".xlsx", ms);
            //return JsonReportResult("Proforma", "Proforma_" + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{pspMasterId}/event/exportpspeventgriddata", Name = "ExportPspEventGridData")]
        public JsonResult ExportPspEventGridData(ExportSettings exportSettings, int pspMasterId)
        {
            bool defaultSearch = Session[PSP_DEFAULT_EXPORT_SESSION] == null ? true : (bool)Session[PSP_DEFAULT_EXPORT_SESSION];

            var pspEvent = _pspEventService.GetPageByPspMasterId(exportSettings.GridSettings, pspMasterId);

            string sord = "1";

            var dataList = (
                    from u in pspEvent
                    orderby (defaultSearch ? "u.EventStartDate, u.EventEndDate, u.EventStartTime, u.EventEndTime, u.Location" : sord)
                    select new PspReadEventDto
                    {
                        PspEventId = u.PspEventId,
                        BatchNum = u.BatchNum,
                        EventStartDate = u.EventStartDate,
                        EventEndDate = u.EventEndDate,
                        EventStartTime = u.EventStartTime,
                        EventEndTime = u.EventEndTime,
                        Time = u.Time,
                        District = u.District,
                        Location = u.Location,
                        EventCount = u.EventCount,
                        ChiLocation = u.ChiLocation,
                        CollectionMethod = u.CollectionMethod, //collectionMethods
                        EventStatus = GetEventStatusDesc(u.EventStatus),
                        EventType = u.EventType,
                        UpdatedOn = u.UpdatedOn,
                        ValidationMessage = u.ValidationMessage,
                        Remarks = string.IsNullOrEmpty(u.Remarks) ? "" : u.Remarks.Replace("S", pspEventRemarks["S"]),
                    }
                   ).ToArray();

            var columnMappings = new Dictionary<string, Func<object, object>>();
            columnMappings.Add("EventStartDate", x => x != null ? (x as DateTime?).Value.ToString("dd/MM/yyyy") : null);
            MemoryStream ms = _reportService.ExportToExcel(dataList, columnMappings, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            var reportId = "EventsGridData";
            var fileName = time + ".xlsx";

            ms.Seek(0, SeekOrigin.Begin);
            //stream.Position = 0;
            ReportResultDto reportResultDto = new ReportResultDto
            {
                FileName = fileName,
                ReportStream = ms
            };

            Session[reportId] = reportResultDto;

            return Json(new JsonResponse(true)
            {
                Data = Url.RouteUrl("ReportDownload", new { reportId = reportId })
            }, JsonRequestBehavior.DenyGet);

            //return JsonReportResult("EventsGridData", time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/event/approve", Name = "ApprovePspEvents")]
        public JsonResult ApprovePspEvents([CustomizeValidator(RuleSet = "RecommendToApprove")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            //apprtype for approval history
            //apptstatus for current psp event status
            if (model != null)
            {
                var approvalHist = new PspApprovalHistory();
                var pspMaster = _pspService.GetPSPById(model.PspMasterId);
                var apprType = "";
                var appStatus = model.PspEventViewModel.ApprovalStatus;
                if (appStatus == "RC")
                {
                    var p = (from u in pspMaster.PspApprovalHistory
                             select u.PermitNum).ToList();
                    var pNum = p.Distinct().FirstOrDefault();
                    apprType = "CE";
                    approvalHist.CancelReason = model.PspEventViewModel.CancelReason;
                    approvalHist.PermitNum = pNum;
                }
                else if (appStatus == "RA")
                {
                    apprType = model.TwoBatchEx != true && model.PrevPspMasterId == null ? "NM" : (model.TwoBatchEx != true ? "AM" : "TW");

                    //CR-005 01
                    //Obtain the Permit Num from PspMaster
                    if (pspMaster.PermitNum.IsNullOrEmpty())
                        pspMaster.PermitNum = getPermitNum(pspMaster);
                    approvalHist.PermitNum = pspMaster.PermitNum;

                    //if (model.PrevPspMasterId == null)
                    //{
                    //    //approvalHist.PermitNum = _pspApprovalHistoryService.GetNewCaseMaxSeq(pspMaster.PspYear);
                    //    approvalHist.PermitNum = _pspApprovalHistoryService.GetNewCaseMaxSeq(DateTime.Today.Year.ToString());
                    //}
                    //else
                    //{
                    //    //approvalHist.PermitNum = _pspApprovalHistoryService.GetOldCaseMaxSeq((int)model.PrevPspMasterId, pspMaster.PspYear);
                    //    approvalHist.PermitNum = _pspApprovalHistoryService.GetOldCaseMaxSeq((int)model.PrevPspMasterId, DateTime.Today.Year.ToString());
                    //}

                    approvalHist.PermitIssueDate = model.PspEventViewModel.PermitIssueDate;
                    //approvalHist.RepresentationReceiveDate = model.PspEventViewModel.RepresentationReceiveDate;
                    //approvalHist.RejectionLetterDate = model.PspEventViewModel.RejectionLetterDate;
                    pspMaster.ApplicationDisposalDate = DateTime.Today;
                }

                approvalHist.Remark = model.PspEventViewModel.Remarks;
                approvalHist.ApprovalStatus = appStatus;
                approvalHist.ApprovalType = apprType;
                approvalHist.PspMaster = pspMaster;
                approvalHist.TwoBatchApproachIndicator = model.TwoBatchEx;

                var pspEventList = getPspEventList(model, appStatus);

                if (pspEventList != null && pspEventList.Count() != 0)
                {
                    using (_unitOfWork.BeginTransaction())
                    {
                        _pspApprovalHistoryService.CreatePspApprovalHistory(approvalHist);
                        foreach (var eveId in pspEventList)
                        {
                            var pspEvent = eveId.Value;
                            pspEvent.EventStatus = string.IsNullOrEmpty(pspEvent.EventStatus) ? "" : pspEvent.EventStatus;
                            if (!pspEvent.EventStatus.Equals("CF")) // prevent flagday conflicted records associate with any approv hist
                            {
                                pspEvent.EventStatus = appStatus;
                                if (appStatus == "RA")
                                    pspEvent.PspApprovalHistory = approvalHist;
                                else
                                    pspEvent.PspCancelHistory = approvalHist;
                                _pspEventService.UpdatePspEvent(pspEvent);
                            }
                            else
                            {
                                _unitOfWork.Rollback();
                                return Json(new JsonResponse(true)
                                {
                                    Message = _messageService.GetMessage(SystemMessage.Error.FlagDay.FlagDayConflictedRecords),
                                }, JsonRequestBehavior.DenyGet);
                            }
                        }

                        //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                        //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                        //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                        UpdatePspEventDate(ref pspMaster);
                        _pspService.UpdatePspMaster(pspMaster);

                        _unitOfWork.Commit();
                    }
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
            }, JsonRequestBehavior.DenyGet);
        }

        //[HttpGet, Route("~/api/psp/{pspMasterId}/event/getpspeventids", Name = "GetPspEventIds")]
        //public JsonResult GetPspEventIds(int pspMasterId)
        //{
        //    var recApproveEvents = _pspEventService.GetPspEventIds(pspMasterId);

        // //Ensure.Argument.NotNull(recApproveEvents);

        //    return Json(new JsonResponse(true)
        //    {
        //        Data = recApproveEvents
        //    }, JsonRequestBehavior.AllowGet);
        //}

        protected Dictionary<int, PspEvent> getPspEventList(PSPViewModel model, string appStatus)
        {
            var pspEventList = new Dictionary<int, PspEvent>();
            if (model.PspEventViewModel.CutOffDateFrom != null && model.PspEventViewModel.CutOffDateTo != null) // user give cut off date from and to
            {
                pspEventList = _pspEventService.GetPspEventsByCutoffDt((DateTime)model.PspEventViewModel.CutOffDateFrom, (DateTime)model.PspEventViewModel.CutOffDateTo, model.PspMasterId, appStatus);
            }
            else if (model.PspEventViewModel.EventIds == null && model.PspEventViewModel.ProcessEvents != null) // user give event approve numbers
            {
                pspEventList = _pspEventService.GetPspEventsByRange((int)model.PspEventViewModel.ProcessEvents, model.PspMasterId, appStatus);
            }
            else
            {
                pspEventList = _pspEventService.GetPspEventsByPspEventList(model.PspMasterId, model.PspEventViewModel.EventIds, appStatus);
            }

            return pspEventList;
        }

        private string GetEventStatusDesc(string eventStatusCode)
        {
            if (eventStatusCode.IsNullOrEmpty()) return "";

            switch (eventStatusCode)
            {
                case "AP":
                    eventStatusCode = "Approved";
                    break;

                case "CA":
                    eventStatusCode = "Cancelled";
                    break;

                case "CF":
                    eventStatusCode = "Flag Day Conflicted";
                    break;

                case "RA":
                    eventStatusCode = "Ready to approve";
                    break;

                case "RC":
                    eventStatusCode = "Ready to cancel";
                    break;

                default:
                    eventStatusCode = "";
                    break;
            }

            return eventStatusCode;
        }

        [RuleSetForClientSideMessagesAttribute("SplitEvent")]
        public PartialViewResult RenderPspEventSplitModal()
        {
            PSPViewModel model = new PSPViewModel();

            return PartialView("_SplitPspEventModal", model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/event/split", Name = "SplitPspEvent")]
        public JsonResult SplitPspEvent([CustomizeValidator(RuleSet = "SplitEvent")] PSPViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            if (model != null)
            {
                PspEvent pspEvent = _pspEventService.GetPspEventById((int)model.PspEventViewModel.PspEventId);
                PspEvent newPspEvent = new PspEvent()
                {
                    PspMaster = pspEvent.PspMaster,
                    EventStartDate = model.PspEventViewModel.EventEndDate.Value.AddDays(1),
                    EventEndDate = pspEvent.EventEndDate,
                    EventStartTime = pspEvent.EventStartTime,
                    EventEndTime = pspEvent.EventEndTime,
                    District = pspEvent.District,
                    Location = pspEvent.Location,
                    ChiLocation = pspEvent.ChiLocation,
                    SimpChiLocation = pspEvent.SimpChiLocation,
                    CharitySalesItem = pspEvent.CharitySalesItem,
                    CollectionMethod = pspEvent.CollectionMethod,
                    OtherCollectionMethod = pspEvent.OtherCollectionMethod,
                    Remarks = pspEvent.Remarks,
                    EventStatus = pspEvent.EventStatus,
                    PspApprovalHistory = pspEvent.PspApprovalHistory,
                    PspCancelHistory = pspEvent.PspCancelHistory,
                    PspAttachment = pspEvent.PspAttachment,
                    PublicPlaceIndicator = pspEvent.PublicPlaceIndicator,
                    FrasCharityEventId = pspEvent.FrasCharityEventId,
                    FrasStatus = pspEvent.FrasStatus
                };

                pspEvent.EventEndDate = model.PspEventViewModel.EventEndDate;
                pspEvent.RowVersion = model.PspEventViewModel.RowVersion;

                using (_unitOfWork.BeginTransaction())
                {
                    _pspEventService.UpdatePspEvent(pspEvent);
                    _pspEventService.CreatePspEvent(newPspEvent);

                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
            }, JsonRequestBehavior.DenyGet);
        }


        #endregion Event

        #region Psp Template Version

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpGet, Route("Template/{pspDocId:int}/Version", Name = "PspVersion")]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "NewVersion")]
        public ActionResult Version(int pspDocId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var psp = this._pspDocService.GetPspDocById(pspDocId);

            Ensure.NotNull(psp, "No letter found with the specified id");
            PspDocViewModel model = new PspDocViewModel();
            model.DocNum = psp.DocNum;
            return View(model);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpGet, Route("~/api/psp/template/{docNum}/list", Name = "ListPspVersion")]
        public JsonResult ListVersion(GridSettings grid, string docNum)
        {
            Ensure.Argument.NotNull(grid);
            var psp = this._pspDocService.GetPage(grid, docNum);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = psp.TotalPages,
                CurrentPageIndex = psp.CurrentPageIndex,
                TotalCount = psp.TotalCount,
                Data = (from s in psp
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            VersionNum = s.VersionNum,
                            DocStatus = s.DocStatus,
                            RowVersion = s.RowVersion,
                            pspDocId = s.PspDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/template/newVersion", Name = "NewPspVersion")]
        public JsonResult VersionNew([CustomizeValidator(RuleSet = "default,NewVersion")]PspDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_TEMPLATE_PATH);
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            var pspDoc = new PspDoc();
            pspDoc.DocNum = model.DocNum;
            pspDoc.DocName = model.Description;
            pspDoc.DocStatus = model.IsActive;
            pspDoc.FileLocation = relativePath;
            pspDoc.VersionNum = model.Version;
            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.File.SaveAs(absolutePath);
                    }

                    _pspDocService.CreatePspDoc(pspDoc);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
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

        [PspsAuthorize(Allow.PspTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/psp/template/{pspDocId}/versionEditPsp", Name = "versionEditPsp")]
        public JsonResult VersionEdit(int pspDocId, [CustomizeValidator(RuleSet = "default,UpdateVersion")]PspDocViewModel model)
        {
            Ensure.Argument.NotNull(pspDocId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_TEMPLATE_PATH);

            // Set the root path
            string rootPath = templatePath.Value;
            // Paths for new file if needed
            string relativePath = string.Empty;
            string absolutePath = string.Empty;

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Get the PspDoc record by given the ID
                    var psp = _pspDocService.GetPspDocById(pspDocId);

                    // If new file need to be upload
                    if (model.File != null)
                    {
                        // Rename the file by adding current time
                        string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                        string fileName = Path.GetFileName(model.File.FileName);
                        string generatedFileName = string.Format("{0}-{1}", time, fileName);

                        // Form the Relative Path for storing in DB and Absolute Path for actually
                        // saving the file
                        relativePath = generatedFileName;
                        absolutePath = Path.Combine(rootPath, generatedFileName);

                        // Save the new file
                        if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        {
                            model.File.SaveAs(absolutePath);
                        }

                        // Form the path of the old file
                        string absolutePathOfOldFile = Path.Combine(rootPath, psp.FileLocation);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                            System.IO.File.Delete(absolutePathOfOldFile);

                        // Replace with the Relative Path of the new file
                        psp.FileLocation = relativePath;
                    }

                    // Fill the update values
                    psp.DocStatus = model.IsActive;
                    psp.VersionNum = model.Version;
                    psp.DocName = model.Description;

                    // Update DB record and commit
                    _pspDocService.UpdatePspDoc(psp);
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

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpGet, Route("~/api/psp/template/{pspDocId:int}", Name = "GetPspDoc")]
        public JsonResult GetPspDoc(int pspDocId)
        {
            Ensure.Argument.NotNullOrEmpty(pspDocId.ToString());
            var pspDoc = this._pspDocService.GetPspDocById(pspDocId);
            if (pspDoc == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            var model = new PspDocViewModel()
            {
                DocNum = pspDoc.DocNum,
                Description = pspDoc.DocName,
                Version = pspDoc.VersionNum,
                IsActive = pspDoc.DocStatus,
                RowVersion = pspDoc.RowVersion,
                PspDocId = pspDoc.Id
            };
            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpPost, Route("~/api/psp/template/{pspDocId:int}/delete", Name = "DetetePspDoc")]
        public JsonResult Delete(int pspDocId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the Fddoc record by given the ID
            var pspDoc = this._pspDocService.GetPspDocById(pspDocId);
            Ensure.NotNull(pspDoc, "No Letter found with the specified id");

            // Get the root path of the Template file from DB and combine with the FileLocation to
            // get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            string absolutePath = Path.Combine(rootPath, pspDoc.FileLocation);

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB
                pspDoc.RowVersion = rowVersion;
                _pspDocService.DeletePspDoc(pspDoc);

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
                Data = pspDoc
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspTemplate)]
        [HttpGet, Route("Template/{pspDocId:int}/Download", Name = "DownloadPspFile")]
        public FileResult Download(int pspDocId)
        {
            // Get the PspDoc record by given the ID
            var psp = _pspDocService.GetPspDocById(pspDocId);

            // Get the root path of the Template from DB and combine with the FileLocation to get
            // the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            var absolutePath = Path.Combine(rootPath, psp.FileLocation);

            // Set the file name for saving
            string fileName = psp.DocName + Path.GetExtension(Path.GetFileName(psp.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        #endregion Psp Template Version

        #region Approve

        [PspsAuthorize(Allow.PspApprove)]
        [RuleSetForClientSideMessagesAttribute("ApproveRecommendEvent")]
        public ActionResult Approve()
        {
            // Master View Model of Approve
            PspApproveViewModel model = new PspApproveViewModel();

            // Create PSP View Model for edit record
            PSPViewModel l_PSPViewModel = new PSPViewModel();

            Uri prePage = Request.UrlReferrer;
            if (prePage != null)
            {
                l_PSPViewModel.PrePage = prePage.ToString();
            }
            else
            {
                l_PSPViewModel.PrePage = "";
            }

            // PSP View Model drop down properties
            initPspViewModel(l_PSPViewModel, false);

            // fill the PSP View Model Nodel to Approve View Model
            model.PSPViewModel = l_PSPViewModel;

            ViewData["NotAllEventsApproved"] = _messageService.GetMessage(SystemMessage.Warning.Psp.NotAllEventsApproved);

            return View(model);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("~/api/PspApprove/listPspRecommendEvents", Name = "ListPspRecommendEvents")]
        public JsonResult ListPspRecommendEvents(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var recApproveEvents = _pspRecommendEventsViewRepository.GetPagePspRecommendDto(grid);

            var gridResult = new GridResult
            {
                TotalPages = recApproveEvents.TotalPages,
                CurrentPageIndex = recApproveEvents.CurrentPageIndex,
                TotalCount = recApproveEvents.TotalCount,
                Data = (
                        from u in recApproveEvents
                        select new PspRecommendEventsDto
                        {
                            PspMasterId = u.PspMasterId,
                            EngOrgNameSorting = u.EngOrgNameSorting + "<br>" + u.ChiOrgName,
                            PspRef = u.PspRef,
                            PermitNum = u.PermitNum,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            ApprovalType = u.ApprovalType,
                            TotEventsToBeApproved = u.TotEventsToBeApproved,
                            RejectionLetterDate = u.RejectionLetterDate,
                            PermitIssueDate = u.PermitIssueDate,
                            CancelReason = u.CancelReason,
                            PspApprovalHistoryId = u.PspApprovalHistoryId,
                            ProcessingOfficerPost = u.ProcessingOfficerPost
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("~/api/pspapprove/getaprovars", Name = "GetAproVars")]
        public JsonResult GetAproVars(int pspApprovalHistoryId)
        {
            PspApproveViewModel model = new PspApproveViewModel();
            var pspAppHist = _pspApprovalHistoryService.getPspApprovalHistoryById(pspApprovalHistoryId);
            var pspMaster = _pspService.GetPSPById(pspAppHist.PspMaster.PspMasterId);
            var orgMaster = _organisationService.GetOrgById(pspMaster.OrgMaster.OrgId);
            if (orgMaster != null)
            {
                model.PspRecommendEventsViewModel.WithHoldInd = _withholdingHistoryService.GetWithHoldBySysDt(orgMaster.OrgId);
                model.PspRecommendEventsViewModel.DisableInd = orgMaster.DisableIndicator;
            }

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("~/api/PspApprove/{pspApprovalHistoryId}/{pspEventStatus}/listPspRecommendApproveEvents/{*pspPermitNum}", Name = "ListPspRecommendApproveEvents")]
        public JsonResult ListPspRecommendApproveEvents(GridSettings grid, int pspApprovalHistoryId, string pspEventStatus, string pspPermitNum, PSPViewModel model)
        {
            Ensure.Argument.NotNull(grid);

            var recApproveEvents = _pspEventService.GetRecommendApproveCancelPspEvents(grid, pspApprovalHistoryId, pspEventStatus, pspPermitNum);

            var gridResult = new GridResult
            {
                TotalPages = recApproveEvents.TotalPages,
                CurrentPageIndex = recApproveEvents.CurrentPageIndex,
                TotalCount = recApproveEvents.TotalCount,
                Data = (
                        from u in recApproveEvents
                        select new PspEventApprovalOrCancelDto
                        {
                            PspEventId = u.PspEventId,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            Time = u.EventStartTime != null ? u.EventStartTime.Value.ToString("HH:mm") + " - " + u.EventEndTime.Value.ToString("HH:mm") : "",
                            District = u.District,
                            Location = u.Location,
                            ChiLocation = u.ChiLocation,
                            CollectionMethod = string.IsNullOrEmpty(u.CollectionMethod) ? "" : u.CollectionMethod,
                            EventStatus = u.PspApprovalHistory.ApprovalStatus,
                            Remark = u.PspApprovalHistory.Remark,
                            //Remark = string.IsNullOrEmpty(u.Remarks) ? "" : u.Remarks.Replace("S", pspEventRemarks["S"]),
                            UpdatedOn = u.UpdatedOn,
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("~/api/PspApprove/{pspApprovalHistoryId}/{pspEventStatus}/listPspRecommendCancelEvents/{*pspPermitNum}", Name = "ListPspRecommendCancelEvents")]
        public JsonResult ListPspRecommendCancelEvents(GridSettings grid, int pspApprovalHistoryId, string pspEventStatus, string pspPermitNum)
        {
            Ensure.Argument.NotNull(grid);

            var recApproveEvents = _pspEventService.GetRecommendApproveCancelPspEvents(grid, pspApprovalHistoryId, pspEventStatus, pspPermitNum);

            var gridResult = new GridResult
            {
                TotalPages = recApproveEvents.TotalPages,
                CurrentPageIndex = recApproveEvents.CurrentPageIndex,
                TotalCount = recApproveEvents.TotalCount,
                Data = (
                        from u in recApproveEvents
                        select new PspEventApprovalOrCancelDto
                        {
                            PspEventId = u.PspEventId,
                            EventStartDate = u.EventStartDate,
                            EventEndDate = u.EventEndDate,
                            Time = u.EventStartTime != null ? u.EventStartTime.Value.ToString("HH:mm") + " - " + u.EventEndTime.Value.ToString("HH:mm") : "",
                            District = u.District,
                            Location = u.Location,
                            ChiLocation = u.ChiLocation,
                            CollectionMethod = string.IsNullOrEmpty(u.CollectionMethod) ? "" : u.CollectionMethod,
                            EventStatus = u.PspApprovalHistory.ApprovalStatus,
                            Remark = string.IsNullOrEmpty(u.Remarks) ? "" : u.Remarks.Replace("S", pspEventRemarks["S"]),
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/PspApprove/{pspApprovalHistoryId}/ApproveRecommendEvent", Name = "ApproveRecommendEvent")]
        public JsonResult ApproveRecommendEvent([CustomizeValidator(RuleSet = "ApproveRecommendEvent")] PspApproveViewModel model, int pspApprovalHistoryId, string[] pspEventIds)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
                //return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            int success = 0;
            int groupCnt = 0;

            if (model != null)
            {
                var pspMaster = _pspApprovalHistoryService.getPspMasterByApprHistId(pspApprovalHistoryId);

                if (ChkRepRecs(model.PspRecommendApproveEventsViewModel.EventIds))
                {
                    return Json(new JsonResponse(false)
                    {
                        Message = _messageService.GetMessage("Repeated records were found")
                    }, JsonRequestBehavior.DenyGet);
                }

                var approvalHist = _pspApprovalHistoryService.getPspApprovalHistoryById(pspApprovalHistoryId);
                approvalHist.PermitIssueDate = model.PspRecommendApproveEventsViewModel.PermitIssueDate;
                //approvalHist.RepresentationReceiveDate = model.pspRecommendApproveEventsViewModel.RepresentationReceiveDate;
                approvalHist.Remark = model.PspRecommendApproveEventsViewModel.Remark;
                approvalHist.ApprovalStatus = "AP";
                //approvalHist.RowVersion = approvalHist.RowVersion;

                var pspEventList = _pspEventService.GetPspEventsByApprovalHistoryId(pspApprovalHistoryId);
                var eventIds = model.PspRecommendApproveEventsViewModel.EventIds;

                //var periodFromDt = _pspEventService.GetMinEventDateFromRs(model.PspRecommendApproveEventsViewModel.EventIds);
                //var periodToDt = _pspEventService.GetMaxEventDateFromRs(model.PspRecommendApproveEventsViewModel.EventIds);
                //pspMaster.EventPeriodFrom = periodFromDt;
                //if (periodToDt != null)
                //{
                //    pspMaster.EventPeriodTo = periodToDt;
                //    var pspDueDateParam = _parameterService.GetParameterByCode("PspDueDate");
                //    pspMaster.SubmissionDueDate = periodToDt.Value.AddDays(Convert.ToInt32(pspDueDateParam.Value));
                //}

                using (_unitOfWork.BeginTransaction())
                {
                    foreach (var eveId in model.PspRecommendApproveEventsViewModel.EventIds)
                    {
                        PspEvent pspEvent;
                        if (pspEventList.ContainsKey(Convert.ToInt32(eveId)))
                        {
                            pspEvent = pspEventList[Convert.ToInt32(eveId)];
                            pspEvent.EventStatus = "AP";
                            pspEvent.FrasStatus = "RC";
                            pspEventList.Remove(Convert.ToInt32(eveId));
                            _pspEventService.UpdatePspEvent(pspEvent);
                        }
                    }

                    foreach (PspEvent pspEvent in pspEventList.Values)
                    {
                        pspEvent.EventStatus = "";
                        pspEvent.PspApprovalHistory = null;
                        _pspEventService.UpdatePspEvent(pspEvent);
                    }

                    pspMaster.ApplicationResult = "01"; // 01 meaning approved
                    _pspApprovalHistoryService.UpdatePspApprovalHistory(approvalHist);

                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);

                    var pspDueDateParam = _parameterService.GetParameterByCode("PspDueDate");
                    pspMaster.SubmissionDueDate = pspMaster.EventPeriodTo.HasValue ? pspMaster.EventPeriodTo.Value.AddDays(Convert.ToInt32(pspDueDateParam.Value)) : (DateTime?)null;

                    _pspService.UpdatePspMaster(pspMaster);
                    if (pspMaster.PreviousPspMasterId.HasValue)
                    {
                        PspMaster parent = _pspService.GetPSPById(pspMaster.PreviousPspMasterId.Value);
                        if (parent.SubmissionDueDate < pspMaster.SubmissionDueDate)
                        {
                            parent.SubmissionDueDate = pspMaster.SubmissionDueDate;
                        }

                        _pspService.UpdatePspMaster(parent);
                    }

                    List<PspEventScheduleDto> proformas = _pspEventService.GetPspEventsForFras(pspMaster.PspMasterId);

                    foreach (var proforma in proformas)
                    {
                        string message;
                        bool result = _pspEventService.SendToFRAS(proforma, out message);

                        proforma.PspEvents.ForEach(x =>
                        {
                            if (result)
                                x.FrasStatus = "C";

                            x.FrasResponse = message;

                            _pspEventService.UpdatePspEvent(x);
                        });

                        if (result)
                            success++;
                    }

                    //groupCnt = proformas.Select(x => x.PspEvents.Count).Aggregate((a, b) => a + b);
                    groupCnt = proformas.Count;

                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated) + " with {0} of {1} event group(s) submitted to FRAS".FormatWith(success, groupCnt),
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/PspApprove/{pspApprovalHistoryId:int}/RejectEvent/{type:int}", Name = "RejectEvent")]
        public JsonResult RejectEvent(PspApproveViewModel model, int pspApprovalHistoryId, int type)
        {
            Ensure.Argument.NotNull(model);
            Ensure.Argument.Is(type == 1 || type == 2);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            if (model != null)
            {
                var pspMaster = _pspApprovalHistoryService.getPspMasterByApprHistId(pspApprovalHistoryId);
                var approvalHist = _pspApprovalHistoryService.getPspApprovalHistoryById(pspApprovalHistoryId);
                Dictionary<int, PspEvent> pspEventList;
                if (type == 1)
                    pspEventList = _pspEventService.GetPspEventsByApprovalHistoryId(pspApprovalHistoryId);
                else
                    pspEventList = _pspEventService.GetPspEventsByCancelHistoryId(pspApprovalHistoryId);

                var eventIds = model.PspRecommendApproveEventsViewModel.EventIds;

                using (_unitOfWork.BeginTransaction())
                {
                    foreach (var eve in pspEventList)
                    {
                        if (type == 1)
                        {
                            eve.Value.PspApprovalHistory = null;
                            eve.Value.EventStatus = "";
                        }
                        else
                        {
                            eve.Value.PspCancelHistory = null;
                            eve.Value.EventStatus = "AP";
                        }

                        _pspEventService.UpdatePspEvent(eve.Value);
                    }

                    //CR-005 TIR-01
                    pspMaster.ApplicationDisposalDate = null;
                    pspMaster.ApplicationResult = ""; // 01 meaning approved
                    _pspApprovalHistoryService.Delete(approvalHist);

                    // 20201208: Added by Allen for update EventPeriodStart and EventPeriodEnd
                    UpdatePspEventDate(ref pspMaster);
                    _pspService.UpdatePspMaster(pspMaster);

                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/PspApprove/{pspApprovalHistoryId}/CancelRecommendEvent", Name = "CancelRecommendEvent")]
        public JsonResult CancelRecommendEvent(PspApproveViewModel model, int pspApprovalHistoryId, string[] pspEventIds)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            int success = 0;
            int groupCnt = 0;

            if (model != null)
            {
                var approvalHist = _pspApprovalHistoryService.getPspApprovalHistoryById(pspApprovalHistoryId);
                approvalHist.Remark = model.PspRecommendCancelEventsViewModel.Remark;
                approvalHist.ApprovalStatus = "CA";

                var pspEventList = _pspEventService.GetPspEventsByCancelHistoryId(pspApprovalHistoryId);
                var eventIds = model.PspRecommendCancelEventsViewModel.EventIds;

                var pspMaster = approvalHist.PspMaster;

                using (_unitOfWork.BeginTransaction())
                {
                    foreach (PspEvent pspEvent in pspEventList.Values)
                    {
                        if (eventIds.Any(u => u == pspEvent.PspEventId.ToString()))
                        {
                            pspEvent.EventStatus = "CA";

                            if (pspEvent.FrasStatus != "RC")
                                pspEvent.FrasStatus = "RR";
                        }
                        else
                        {
                            pspEvent.PspCancelHistory = null;
                            pspEvent.EventStatus = "AP";
                        }

                        _pspEventService.UpdatePspEvent(pspEvent);
                    }

                    _pspApprovalHistoryService.UpdatePspApprovalHistory(approvalHist);

                    //var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
                    //pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
                    //pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
                    UpdatePspEventDate(ref pspMaster);

                    var pspDueDateParam = _parameterService.GetParameterByCode("PspDueDate");
                    pspMaster.SubmissionDueDate = pspMaster.EventPeriodTo.HasValue ? pspMaster.EventPeriodTo.Value.AddDays(Convert.ToInt32(pspDueDateParam.Value)) : (DateTime?)null;

                    _pspService.UpdatePspMaster(pspMaster);
                    if (pspMaster.PreviousPspMasterId.HasValue)
                    {
                        PspMaster parent = _pspService.GetPSPById(pspMaster.PreviousPspMasterId.Value);
                        if (parent.SubmissionDueDate < pspMaster.SubmissionDueDate)
                        {
                            parent.SubmissionDueDate = pspMaster.SubmissionDueDate;
                        }

                        _pspService.UpdatePspMaster(parent);
                    }

                    List<PspEventScheduleDto> proformas = _pspEventService.GetPspEventsForFrasByCharityID(pspMaster.PspMasterId);
                    foreach (var proforma in proformas)
                    {
                        string message;

                        if (!proforma.Status.Equals("C"))
                        {
                            bool result = _pspEventService.SendToFRAS(proforma, out message);

                            proforma.PspEvents.ForEach(x =>
                            {
                                if (result)
                                {
                                    if (x.FrasStatus == "RR")
                                        x.FrasCharityEventId = string.Empty;
                                    x.FrasStatus = "C";
                                }

                                x.FrasResponse = message;

                                _pspEventService.UpdatePspEvent(x);
                            });

                            if (result)
                                success++;
                        }
                    }

                    groupCnt = proformas.Count;
                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated) + " with {0} of {1} event group(s) submitted to FRAS".FormatWith(success, groupCnt),
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet, Route("~/api/PspApprove/{pspApprovalHistoryId}/{pspEventStatus}/GetToBeApprovedEvents/{*pspPermitNum}", Name = "GetToBeApprovedEvents")]
        public JsonResult GetToBeApprovedEvents(int pspApprovalHistoryId, string pspEventStatus, string pspPermitNum)
        {
            var recApproveEvents = _pspEventService.GetRecommendApproveCancelEventsList(pspApprovalHistoryId, pspEventStatus, pspPermitNum);

            Ensure.Argument.NotNull(recApproveEvents);

            return Json(new JsonResponse(true)
            {
                Data = recApproveEvents
            }, JsonRequestBehavior.AllowGet);
        }

        public bool ChkRepRecs(string[] eventIds)
        {
            return _pspEventService.ifLstRepeated(eventIds);
        }

        #endregion Approve

        #region Attachment

        [RuleSetForClientSideMessagesAttribute("default")]
        public PartialViewResult RenderPspAttachmentModal()
        {
            PspAttachmentViewModel model = new PspAttachmentViewModel();
            return PartialView("_EditPspAttachmentModal", model);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspMasterId:int}/listPspAttachment", Name = "ListPspAttachment")]
        public JsonResult ListPspAttachment(int pspMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(pspMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "PspMaster.PspMasterId",
                data = pspMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });
            var pspAttachments = _pspAttachmentService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = pspAttachments.TotalPages,
                CurrentPageIndex = pspAttachments.CurrentPageIndex,
                TotalCount = pspAttachments.TotalCount,
                Data = (from c in pspAttachments
                        select new
                        {
                            PspAttachmentId = c.PspAttachmentId,
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

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{pspMasterId:int}/CreatePspAttachment", Name = "CreatePspAttachment")]
        public JsonResult CreatePspAttachment(int pspMasterId, [CustomizeValidator(RuleSet = "default,CreatePspAttachment")] PSPViewModel model)
        {
            // Get the PspMaster record by given the ID
            Ensure.Argument.NotNull(pspMasterId);
            var pspMaster = _pspService.GetPSPById(model.PspAttachmentViewModel.PspMasterId);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            if (model.PspAttachmentViewModel.AttachmentFile != null)
            {
                // Get the root path of the Attachment from DB
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_ATTACHMENT_PATH);

                // Rename the file by adding current time
                string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                string fileName = Path.GetFileName(model.PspAttachmentViewModel.AttachmentFile.FileName);
                string generatedFileName = string.Format("{0}-{1}", time, fileName);

                // Set the root path by adding the PspMasterId folder ( [Root Folder of Attachment]
                // \ [PspMasterId Folder] )
                string rootPath = Path.Combine(attachmentPath.Value, pspMaster.PspMasterId.ToString());
                // Form the Relative Path for storing in DB ( [PspMasterId Folder] \ [File Name] )
                // and Absolute Path for actually saving the file ( [Root Folder of Attachment] \
                // [PspMasterId Folder] \ [File Name] )
                string relativePath = Path.Combine(pspMaster.PspMasterId.ToString(), generatedFileName);
                string absolutePath = Path.Combine(rootPath, generatedFileName);

                // Create new PspAttachment row and fill the value
                var pspAttachment = new PspAttachment();
                pspAttachment.PspMaster = pspMaster;
                pspAttachment.FileDescription = model.PspAttachmentViewModel.FileDescription;
                pspAttachment.FileLocation = relativePath;
                pspAttachment.FileName = model.PspAttachmentViewModel.FileName;

                using (_unitOfWork.BeginTransaction())
                {
                    // Save the file to the Absolute Path
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.PspAttachmentViewModel.AttachmentFile.SaveAs(absolutePath);
                    }

                    // Insert record to DB and commit
                    _pspAttachmentService.Create(pspAttachment);
                    _unitOfWork.Commit();
                }

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage("", "Attachment Not Found."),
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/psp/{pspAttachmentId:int}/UpdatePspAttachment", Name = "UpdatePspAttachment")]
        public JsonResult UpdatePspAttachment([CustomizeValidator(RuleSet = "default,UpdatePspAttachment")] PSPViewModel model, int pspAttachmentId)
        {
            Ensure.Argument.NotNull(model.PspAttachmentViewModel);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            var pspAttachment = _pspAttachmentService.GetById(Convert.ToInt32(pspAttachmentId));
            Ensure.NotNull(pspAttachment, "No PspAttachment found with the specified id");

            pspAttachment.FileName = model.PspAttachmentViewModel.FileName;
            pspAttachment.FileDescription = model.PspAttachmentViewModel.FileDescription;

            using (_unitOfWork.BeginTransaction())
            {
                string oldAbsolutePath = null;
                if (model.PspAttachmentViewModel.AttachmentFile != null)
                {
                    var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_ATTACHMENT_PATH);
                    string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    string fileName = Path.GetFileName(model.PspAttachmentViewModel.AttachmentFile.FileName);
                    string generatedFileName = string.Format("{0}-{1}", time, fileName);

                    string rootPath = Path.Combine(attachmentPath.Value, pspAttachment.PspMaster.PspMasterId.ToString());
                    string relativePath = Path.Combine(pspAttachment.PspMaster.PspMasterId.ToString(), generatedFileName);
                    string absolutePath = Path.Combine(rootPath, generatedFileName);
                    oldAbsolutePath = Path.Combine(attachmentPath.Value, pspAttachment.FileLocation);

                    pspAttachment.FileLocation = relativePath;

                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        model.PspAttachmentViewModel.AttachmentFile.SaveAs(absolutePath);
                }

                _pspAttachmentService.Update(pspAttachment);

                if (oldAbsolutePath.IsNotNullOrEmpty())
                    if (System.IO.File.Exists(oldAbsolutePath))
                        System.IO.File.Delete(oldAbsolutePath);

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspAttachmentId:int}/getPspAttachmentDetail", Name = "GetPspAttachmentDetail")]
        public JsonResult GetPspAttachmentDetail(int pspAttachmentId)
        {
            var pspAttachment = _pspAttachmentService.GetById(pspAttachmentId);
            Ensure.NotNull(pspAttachment, "No PspAttachment found with the specified id");

            var filePath = pspAttachment.FileLocation;
            //HttpPostedFileBase httpFile;
            //httpFile.FileName = pspAttachment.FileName;

            var model = new PSPViewModel();
            model.PspAttachmentViewModel.PspAttachmentId = pspAttachment.PspAttachmentId;
            model.PspAttachmentViewModel.PspMasterId = pspAttachment.PspMaster.PspMasterId;
            model.PspAttachmentViewModel.FileName = pspAttachment.FileName;
            model.PspAttachmentViewModel.FileDescription = pspAttachment.FileDescription;
            model.PspAttachmentViewModel.RowVersion = pspAttachment.RowVersion;

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpGet, Route("~/api/psp/{pspAttachmentId:int}/downloadPspAttachment", Name = "DownloadPspAttachment")]
        public FileResult DownloadPspAttachment(int pspAttachmentId)
        {
            // Get the PspAttachment record by given the ID
            var pspAttachment = _pspAttachmentService.GetById(pspAttachmentId);
            Ensure.NotNull(pspAttachment, "No PspAttachment found with the specified id");

            // Get the root path of the Attachment from DB and combine with the FileLocation to get
            // the Absolute Path that the file actually stored at
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_ATTACHMENT_PATH);
            string rootPath = attachmentPath.Value;
            var absolutePath = Path.Combine(rootPath, pspAttachment.FileLocation);
            // ( [Root Folder of Attachment] \ [PspMasterId Folder] \ [File Name] )

            // Set the file name for saving
            string fileName = pspAttachment.FileName + Path.GetExtension(Path.GetFileName(pspAttachment.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        [PspsAuthorize(Allow.PspMaster)]
        [HttpPost, Route("~/api/complaint/{pspAttachmentId:int}/deletePspAttachment", Name = "DeletePspAttachment")]
        public JsonResult DeletePspAttachment(int pspAttachmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var pspAttachment = _pspAttachmentService.GetById(pspAttachmentId);
            Ensure.NotNull(pspAttachment, "No PspAttachment found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_ATTACHMENT_PATH);
                var absolutePath = Path.Combine(attachmentPath.Value, pspAttachment.FileLocation);

                _pspAttachmentService.Delete(pspAttachment);
                if (System.IO.File.Exists(absolutePath))
                    System.IO.File.Delete(absolutePath);

                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion Attachment

        #region FRAS

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("Fras", Name = "PspFras")]
        public ActionResult Fras()
        {
            PSPViewModel model = new PSPViewModel();
            ViewData["SUBMIT_GOV_HK"] = _messageService.GetMessage(SystemMessage.Info.GovHK);
            return View(model);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpGet, Route("~/api/psp/frs/list", Name = "ListPspFras")]
        public JsonResult ListPspFras(GridSettings grid, PSPViewModel model)
        {
            var session = this.HttpContext.Session[PSP_SEARCH_SESSION];
            if (session != null)
            {
                model = (PSPViewModel)session;
            }

            grid.AddDefaultRule(new Rule()
            {
                field = "ReSubmit",
                data = "1",
                op = WhereOperation.Equal.ToEnumValue()
            });

            return ListPspSearch(grid, model);
        }

        [PspsAuthorize(Allow.PspApprove)]
        [HttpPost, Route("~/api/psp/frs/reSubmit", Name = "PspSubmitFRAS")]
        [ValidateAntiForgeryToken]
        public JsonResult PspSubmitFRAS(int pspMasterId)
        {
            Ensure.Argument.NotNull(pspMasterId);

            int success = 0;
            List<PspEventScheduleDto> proformas = _pspEventService.GetPspEventsForFrasByCharityID(pspMasterId).Where(u => u.Status != "C").ToList();

            using (_unitOfWork.BeginTransaction())
            {
                foreach (var proforma in proformas)
                {
                    string message;
                    bool result = _pspEventService.SendToFRAS(proforma, out message);

                    if (result)
                        success++;

                    foreach (PspEvent pspEvent in proforma.PspEvents)
                    {
                        if (result)
                        {
                            if (pspEvent.FrasStatus == "RR")
                                pspEvent.FrasCharityEventId = string.Empty;

                            pspEvent.FrasStatus = "C";
                        }

                        pspEvent.FrasResponse = message;
                        _pspEventService.UpdatePspEvent(pspEvent);
                    }
                }

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated) + " with {0} of {1} event group(s) submitted to FRAS".FormatWith(success, proformas.Count),
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion FRAS


        #region Common Function
        //CR-005 01
        private String getPermitNum(PspMaster pspMaster)
        {
            if (pspMaster.PermitNum.IsNullOrEmpty())
            {
                if (pspMaster.PreviousPspMasterId.IsNullOrDefault())
                {
                    return this._pspService.GetNextPermitNum(DateTime.Today.Year.ToString());
                }
                else
                {
                    return this._pspService.GetNextPermitNum(pspMaster.PreviousPspMasterId.Value);
                }
            }
            else
                return pspMaster.PermitNum;
        }

        private void UpdatePspEventDate(ref PspMaster pspMaster)
        {
            var period = _pspEventService.GetEventPeriodDateByPspId(pspMaster.PspMasterId);
            pspMaster.EventPeriodFrom = period != null ? period.Item1 : null;
            pspMaster.EventPeriodTo = period != null ? period.Item2 : null;
        }
        #endregion

    }
}