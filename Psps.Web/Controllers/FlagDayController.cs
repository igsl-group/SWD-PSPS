using DocxGenerator.Library;
using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Organisation;
using Psps.Services.ComplaintMasters;
using Psps.Services.FlagDays;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.Report;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.FlagDay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("FlagDay"), Route("{action=Index}")]
    public class FlagDayController : BaseController
    {
        #region "Field & Construtor"

        private readonly string FlagDay_SEARCH_SESSION = Constant.FLAGDAY_SEARCH_SESSION;
        private readonly string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILookupService _lookupService;
        private readonly IFlagDayService _flagDayService;
        private readonly IFlagDayDocService _flagDayDocService;
        private readonly IFdEventService _fdEventService;
        private readonly IFdApprovalHistoryService _fdApprovalHistoryService;
        private readonly IOrganisationService _organisationService;
        private readonly IParameterService _parameterService;
        private readonly IReportService _reportService;
        private readonly IFdAttachmentService _fdAttachmentService;
        private readonly IFlagDayListService _flagDayListService;
        private readonly IWithholdingHistoryService _withholdingHistoryService;
        private readonly IOrgEditLatestPspFdViewRepository _orgEditLatestPspFdViewRepository;
        private readonly IComplaintMasterService _complaintMasterService;

        private readonly Dictionary<bool, string> boolStringDict;
        private readonly IDictionary<string, string> activityConcerned;
        private readonly IDictionary<string, string> chiSalutes;
        private readonly IDictionary<string, string> collectionMethods;
        private readonly IDictionary<string, string> complaintSources;
        private readonly IDictionary<string, string> complaintResults;
        private readonly IDictionary<string, string> docSubmissions;
        private readonly IDictionary<string, string> engSalutes;
        private readonly IDictionary<string, string> fdApplicationResults;
        private readonly IDictionary<string, string> fdEventRemarks;
        private readonly IDictionary<string, string> fdGroupings;
        private readonly IDictionary<string, string> fdLotResults;
        private readonly IDictionary<string, string> issueLetterType;
        private readonly IDictionary<string, string> languages;
        private readonly IDictionary<string, string> orgStatus;
        private readonly IDictionary<string, string> processStatus;
        private readonly IDictionary<string, string> twrs;
        private readonly IDictionary<string, string> twrDistricts;
        private readonly IDictionary<string, string> yearOfFlagDay;
        private readonly IDictionary<string, string> benchmarkTWFD;
        private readonly IDictionary<string, string> benchmarkRFD;

        public FlagDayController(IUnitOfWork unitOfWork, IMessageService messageService, ILookupService lookupService
            , IFlagDayService flagDayService, IFlagDayDocService flagDayDocService, IParameterService parameterService, IOrganisationService organisationService,
            IFdEventService fdEventService, IReportService reportService, IFdApprovalHistoryService fdApprovalHistoryService,
            IFdAttachmentService fdAttachmentService, IFlagDayListService flagDayListService, IWithholdingHistoryService withholdingHistoryService,
            IOrgEditLatestPspFdViewRepository orgEditLatestPspFdViewRepository, IComplaintMasterService complaintMasterService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._lookupService = lookupService;
            this._flagDayService = flagDayService;
            this._flagDayDocService = flagDayDocService;
            this._fdApprovalHistoryService = fdApprovalHistoryService;
            this._parameterService = parameterService;
            this._organisationService = organisationService;
            this._fdEventService = fdEventService;
            this._reportService = reportService;
            this._fdAttachmentService = fdAttachmentService;
            this._flagDayListService = flagDayListService;
            this._withholdingHistoryService = withholdingHistoryService;
            this._orgEditLatestPspFdViewRepository = orgEditLatestPspFdViewRepository;
            this._complaintMasterService = complaintMasterService;

            boolStringDict = new Dictionary<bool, string>();
            boolStringDict.Add(false, "No");
            boolStringDict.Add(true, "Yes");

            this.activityConcerned = _lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            if (activityConcerned.Count == 0) activityConcerned.Add("", "");

            this.chiSalutes = _lookupService.getAllLkpInChiCodec(LookupType.Salute);
            if (chiSalutes.Count == 0) chiSalutes.Add("", "");

            this.complaintSources = _lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            if (complaintSources.Count == 0) complaintSources.Add("", "");

            this.complaintResults = _lookupService.getAllLkpInCodec(LookupType.ComplaintResult);

            this.fdApplicationResults = _lookupService.getAllLkpInCodec(LookupType.FdApplicationResult);
            if (fdApplicationResults.Count == 0) fdApplicationResults.Add("", "");

            this.collectionMethods = _lookupService.getAllLkpInCodec(LookupType.FdCollectionMethod);
            if (collectionMethods.Count == 0) collectionMethods.Add("", "");

            this.docSubmissions = _lookupService.getAllLkpInCodec(LookupType.FdDocSubmitted);
            if (docSubmissions.Count == 0) docSubmissions.Add("", "");

            this.engSalutes = _lookupService.getAllLkpInCodec(LookupType.Salute);
            if (engSalutes.Count == 0) engSalutes.Add("", "");

            this.fdEventRemarks = _lookupService.getAllLkpInCodec(LookupType.FdEventRemark);
            if (fdEventRemarks.Count == 0) fdEventRemarks.Add("", "");

            this.fdGroupings = _lookupService.getAllLkpInCodec(LookupType.FdGrouping);
            if (fdGroupings.Count == 0) fdGroupings.Add("", "");

            this.fdLotResults = _lookupService.getAllLkpInCodec(LookupType.FdLotResult);
            if (fdLotResults.Count == 0) fdLotResults.Add("", "");

            this.issueLetterType = _lookupService.getAllLkpInCodec(LookupType.FollowUpLetterType);
            if (issueLetterType.Count == 0) issueLetterType.Add("", "");

            this.languages = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
            if (languages.Count == 0) languages.Add("", "");

            this.orgStatus = lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            if (orgStatus.Count == 0) orgStatus.Add("", "");

            this.processStatus = _lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);

            this.twrs = _lookupService.getAllLkpInCodec(LookupType.TWR);
            if (twrs.Count == 0) twrs.Add("", "");

            this.twrDistricts = _lookupService.getAllLkpInCodec(LookupType.TWRDistrict);
            if (twrDistricts.Count == 0) twrDistricts.Add("", "");

            this.yearOfFlagDay = _flagDayListService.GetAllFlagDayListYearForDropdown();
            if (yearOfFlagDay.Count == 0) yearOfFlagDay.Add("", "");

            this.benchmarkTWFD = _lookupService.getAllLkpInCodec(LookupType.BenchmarkTWFD);
            this.benchmarkRFD = _lookupService.getAllLkpInCodec(LookupType.BenchmarkRFD);
        }

        #endregion "Field & Construtor"

        #region New

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("New/{createMode}/{fdMasterId:int}", Name = "NewFdMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult New(string createMode, int fdMasterId)
        {
            FlagDayViewModel model = new FlagDayViewModel();

            if (fdMasterId != -1)
            {
                var fdMaster = _flagDayService.GetFDById(fdMasterId);
                model = getDataFromFdMaster(model, fdMaster);
                //model.IsShowNewApplicant = fdMaster.OrgMaster == null ? true : _flagDayService.IsShowNewApplicant(fdMaster.OrgMaster.OrgId);
            }
            //else
            //{
            //    model.IsShowNewApplicant = true;
            //}

            initFdViewModel(model, 1);

            model.CreateMode = createMode;
            if (createMode == "copyToNew")
            {
                model.CreateModelNewApplicant = false;

                //Clear AFS and Track Record info
                model.TrackRecordStartDate = null;
                model.TrackRecordEndDate = null;
                model.TrackRecordDetails = "";
                model.AfsRecordStartDate = null;
                model.AfsRecordEndDate = null;
                model.AfsRecordDetails = "";
            }

            //model.ReferenceNumber = "";
            //model.YearofFlagDay = "";
            //model.TWR = "";
            return View(model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("New/{createMode}/{orgId:int}/createFdMaster", Name = "CreateFdMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult CreateFdMaster(string createMode, int orgId, string returnUrl)
        {
            FlagDayViewModel model = new FlagDayViewModel();

            if (!String.IsNullOrEmpty(returnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + returnUrl;
                model.PrePage = prePage;
            }
            var org = _organisationService.GetOrgById(orgId);
            if (org != null)
            {
                model.CreateModelOrgRef = org.OrgRef;
            }

            initFdViewModel(model, 1);

            model.CreateMode = createMode;
            //model.ReferenceNumber = "";
            //model.YearofFlagDay = "";
            //model.TWR = "";
            return View("New", model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/flagday/create", Name = "CreateFlagDay")]
        public JsonResult CreateFlagDay([CustomizeValidator(RuleSet = "Create")] FlagDayViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var flagDay = newFdmasterByModel(model);
            //if (model.CreateMode == "copyToNew")
            //{
            //    flagDay.PreviousFdMasterId = model.PreviousFdMasterId;
            //}

            using (_unitOfWork.BeginTransaction())
            {
                _flagDayService.CreateFdMaster(flagDay);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = flagDay.FdMasterId,
            }, JsonRequestBehavior.DenyGet);
        }

        public FdMaster newFdmasterByModel(FlagDayViewModel model)
        {
            var flagDay = new FdMaster();
            var orgMaster = _organisationService.GetOrgByRef(model.CreateModelOrgRef);
            var newRef = _flagDayService.GetMaxSeq(model.YearofFlagDay);

            flagDay.FdRef = newRef;
            flagDay.ApplicationReceiveDate = model.DateofReceivingApplication;
            flagDay.UsedLanguage = model.LanguageUsedId;
            flagDay.OrgMaster = orgMaster;
            flagDay.ContactPersonSalute = model.EngSalute;
            flagDay.ContactPersonFirstName = model.ContactPersonFirstName;
            flagDay.ContactPersonLastName = model.ContactPersonLastName;
            flagDay.ContactPersonChiFirstName = model.ContactPersonChiFirstName;
            flagDay.ContactPersonChiLastName = model.ContactPersonChiLastName;
            flagDay.ContactPersonPosition = model.ContactPersonPost;
            flagDay.ContactPersonTelNum = model.TelNo;
            flagDay.ContactPersonFaxNum = model.FaxNo;
            flagDay.ContactPersonEmailAddress = model.ContactPersonEmailAddress;
            flagDay.ApplyForTwr = model.TWR;
            flagDay.ApplicationResult = model.ApplicationResultId;
            flagDay.ResultApplicationRemark = model.ResultApplicationRemark;
            flagDay.FdYear = model.YearofFlagDay;
            flagDay.LotGroup = model.GroupingwithNoofLot;
            flagDay.TargetIncome = model.TargetIncome;
            flagDay.NewApplicantIndicator = model.CreateModelNewApplicant;
            flagDay.TrackRecordStartDate = model.TrackRecordStartDate;
            flagDay.TrackRecordEndDate = model.TrackRecordEndDate;
            flagDay.TrackRecordDetails = model.TrackRecordDetails;
            flagDay.AfsRecordStartDate = model.AfsRecordStartDate;
            flagDay.AfsRecordEndDate = model.AfsRecordEndDate;
            flagDay.AfsRecordDetails = model.AfsRecordDetails;
            flagDay.FundRaisingPurpose = model.FundRaisingPurpose;
            flagDay.FdGroup = model.GroupingId;
            flagDay.FdGroupPercentage = model.PercentageForGrouping;
            flagDay.GroupingResult = model.GroupingResult;
            flagDay.CommunityChest = model.CommunityChest;
            flagDay.ConsentLetter = model.ConsentLetter;
            //flagDay.OutstandingEmailIssueDate = model.OutstandingEmailIssueDate;
            //flagDay.OutstandingEmailReplyDate = model.OutstandingEmailReplyDate;
            flagDay.ApplicationRemark = model.ApplicationRemark;
            //flagDay.OutstandingEmailReminderIssueDate = model.OutstandingEmailReminderIssueDate;
            //flagDay.OutstandingEmailReminderReplyDate = model.OutstandingEmailReminderReplyDate;
            flagDay.FdLotNum = model.FdLotNum;
            flagDay.FdLotResult = model.FdLotResult;
            flagDay.PriorityNum = model.PriorityNum;
            //flagDay.ProposalDetail = model.ProposalDetail;
            //flagDay.ChiProposalDetail = model.ChiProposalDetail;
            //flagDay.FlagSalePurpose = model.FlagSalePurpose;
            //flagDay.ChiFlagSalePurpose = model.ChiFlagSalePurpose;
            flagDay.AcknowledgementReceiveDate = model.AcknowledgementReceiveDate;
            flagDay.ApplyPledgingMechanismIndicator = model.ApplyPledgingMechanismIndicator;
            flagDay.PledgedAmt = model.PledgedAmt;
            flagDay.PledgingAmt = model.PledgingAmt;
            flagDay.PledgingProposal = model.PledgingProposal;
            flagDay.ChiPledgingProposal = model.ChiPledgingProposal;
            flagDay.PledgingApplicationRemark = model.PledgingApplicationRemark;
            flagDay.VettingPanelCaseIndicator = model.VettingPanelCaseIndicator;
            flagDay.ReviewCaseIndicator = model.ReviewCaseIndicator;
            flagDay.ChiFundRaisingPurpose = model.ChiFundRaisingPurpose;
            flagDay.JointApplicationIndicator = model.JointApplicationIndicator;
            flagDay.Section88Indicator = model.Section88Indicator;

            return flagDay;
        }

        //Mode - 0: Search, 1: Create, 2: Edit
        private void initFdViewModel(FlagDayViewModel model, int mode)
        {
            model.OrgStatus = orgStatus;
            model.TerritoryRegion = twrs;
            model.Districts = twrDistricts;
            model.LanguageUseds = languages;
            model.EngSalutes = engSalutes;
            model.ChiSalutes = chiSalutes;
            model.ApplicationResults = fdApplicationResults;
            model.Groupings = fdGroupings;
            model.FdLotResults = fdLotResults;
            model.Subventeds = boolStringDict;
            model.NewApplicants = boolStringDict;
            model.YearofFlagDays = yearOfFlagDay;
            model.PermitRevokeIndicators = boolStringDict;

            if (mode == 0)
            {
                model.FdYears = _flagDayListService.GetAllFlagDayListYearForDropdown();
                model.PermitLists = _flagDayDocService.getAllFdDocTemplateForDropdown();
            }
            else if (mode == 1)
            {
                model.YearofFlagDay = "{0}-{1}".FormatWith(DateTime.Today.AddYears(1).ToString("yy"), DateTime.Today.AddYears(2).ToString("yy"));
            }
            else if (mode == 2)
            {
                model.DocSubmissions = docSubmissions;
                model.WithholdingListIndicators = boolStringDict;
                model.AfsReceiveIndicators = boolStringDict;
                model.RequestPermitteeIndicators = boolStringDict;
                model.AfsReSubmitIndicators = boolStringDict;
                model.AfsReminderIssueIndicators = boolStringDict;
                model.MethodOfCollections = collectionMethods;

                model.ActivityConcerns = activityConcerned;
                model.ComplaintSources = complaintSources;
                model.ComplaintResults = complaintResults;
                model.ProcessStatus = processStatus;
            }
        }

        private FlagDayViewModel getDataFromFdMaster(FlagDayViewModel model, FdMaster flagDay)
        {
            //general properties
            model.FdMasterId = flagDay.Id;
            model.ReferenceNumber = flagDay.FdRef;
            model.DateofReceivingApplication = flagDay.ApplicationReceiveDate;
            model.LanguageUsedId = flagDay.UsedLanguage;
            model.CreateModelOrgRef = flagDay.OrgMaster.OrgRef;
            model.OrgEngName = flagDay.OrgMaster.EngOrgName;
            model.OrgChiName = flagDay.OrgMaster.ChiOrgName;
            model.EngSalute = flagDay.ContactPersonSalute;
            model.ChiSalute = flagDay.ContactPersonSalute;
            model.ContactPersonFirstName = flagDay.ContactPersonFirstName;
            model.ContactPersonLastName = flagDay.ContactPersonLastName;
            model.ContactPersonChiFirstName = flagDay.ContactPersonChiFirstName;
            model.ContactPersonChiLastName = flagDay.ContactPersonChiLastName;
            model.ContactPersonPost = flagDay.ContactPersonPosition;
            model.TelNo = flagDay.ContactPersonTelNum;
            model.FaxNo = flagDay.ContactPersonFaxNum;
            model.ContactPersonEmailAddress = flagDay.ContactPersonEmailAddress;
            model.TWR = flagDay.ApplyForTwr;
            model.ApplicationResultId = flagDay.ApplicationResult;
            model.ResultApplicationRemark = flagDay.ResultApplicationRemark;
            model.VettingPanelCaseIndicator = flagDay.VettingPanelCaseIndicator != null ? (bool)flagDay.VettingPanelCaseIndicator : false;
            model.YearofFlagDay = flagDay.FdYear;
            model.GroupingwithNoofLot = flagDay.LotGroup;
            model.TargetIncome = flagDay.TargetIncome;
            model.CreateModelNewApplicant = flagDay.NewApplicantIndicator != null ? (bool)flagDay.NewApplicantIndicator : false;
            model.TrackRecordStartDate = flagDay.TrackRecordStartDate;
            model.TrackRecordEndDate = flagDay.TrackRecordEndDate;
            model.TrackRecordDetails = flagDay.TrackRecordDetails;
            model.AfsRecordStartDate = flagDay.AfsRecordStartDate;
            model.AfsRecordEndDate = flagDay.AfsRecordEndDate;
            model.AfsRecordDetails = flagDay.AfsRecordDetails;
            model.GroupingId = flagDay.FdGroup;
            model.PercentageForGrouping = flagDay.FdGroupPercentage;
            model.GroupingResult = flagDay.GroupingResult;
            model.CommunityChest = flagDay.CommunityChest != null ? (bool)flagDay.CommunityChest : false;
            model.ConsentLetter = flagDay.ConsentLetter != null ? (bool)flagDay.ConsentLetter : false;
            //model.OutstandingEmailIssueDate = flagDay.OutstandingEmailIssueDate;
            //model.OutstandingEmailReplyDate = flagDay.OutstandingEmailReplyDate;
            model.ApplicationRemark = flagDay.ApplicationRemark;
            //model.OutstandingEmailReminderIssueDate = flagDay.OutstandingEmailReminderIssueDate;
            //model.OutstandingEmailReminderReplyDate = flagDay.OutstandingEmailReminderReplyDate;
            model.FdLotNum = flagDay.FdLotNum;
            model.FdLotResult = flagDay.FdLotResult;
            model.PriorityNum = flagDay.PriorityNum;
            //model.ProposalDetail = flagDay.ProposalDetail;
            //model.ChiProposalDetail = flagDay.ChiProposalDetail;
            //model.FlagSalePurpose = flagDay.FlagSalePurpose;
            //model.ChiFlagSalePurpose = flagDay.ChiFlagSalePurpose;
            model.AcknowledgementReceiveDate = flagDay.AcknowledgementReceiveDate;
            model.ApplyPledgingMechanismIndicator = flagDay.ApplyPledgingMechanismIndicator != null ? (bool)flagDay.ApplyPledgingMechanismIndicator : false;
            model.PledgingProposal = flagDay.PledgingProposal;
            model.ChiPledgingProposal = flagDay.ChiPledgingProposal;
            model.PledgingApplicationRemark = flagDay.PledgingApplicationRemark;
            model.DocSubmission = flagDay.DocSubmission;
            model.SubmissionDueDate = flagDay.SubmissionDueDate;
            model.FirstReminderIssueDate = flagDay.FirstReminderIssueDate;
            model.FirstReminderDeadline = flagDay.FirstReminderDeadline;
            model.SecondReminderIssueDate = flagDay.SecondReminderIssueDate;
            model.SecondReminderDeadline = flagDay.SecondReminderDeadline;
            model.AuditReportReceivedDate = flagDay.AuditReportReceivedDate;
            model.PublicationReceivedDate = flagDay.PublicationReceivedDate;
            model.DocReceiveRemark = flagDay.DocReceiveRemark;
            model.DocRemark = flagDay.DocRemark;
            model.StreetCollection = flagDay.StreetCollection;
            model.GrossProceed = flagDay.GrossProceed;
            model.Expenditure = flagDay.Expenditure;
            model.NetProceed = flagDay.NetProceed;
            model.ExpPerGpPercentage = flagDay.GrossProceed != 0 ? flagDay.Expenditure / flagDay.GrossProceed * 100 : 0;
            model.NewspaperPublishDate = flagDay.NewspaperPublishDate;
            model.PledgingAmt = flagDay.PledgingAmt;
            model.PledgedAmt = flagDay.PledgedAmt;
            model.AcknowledgementEmailIssueDate = flagDay.AcknowledgementEmailIssueDate;
            model.WithholdingListIndicator = flagDay.WithholdingListIndicator != null ? flagDay.WithholdingListIndicator : null;
            model.WithholdingBeginDate = flagDay.WithholdingBeginDate;
            model.WithholdingEndDate = flagDay.WithholdingEndDate;
            model.WithholdingRemark = flagDay.WithholdingRemark;
            model.AfsReceiveIndicator = flagDay.AfsReceiveIndicator != null ? flagDay.AfsReceiveIndicator : null;
            model.RequestPermitteeIndicator = flagDay.RequestPermitteeIndicator != null ? flagDay.RequestPermitteeIndicator : null;
            model.AfsReSubmitIndicator = flagDay.AfsReSubmitIndicator != null ? flagDay.AfsReSubmitIndicator : null;
            model.AfsReminderIssueIndicator = flagDay.AfsReminderIssueIndicator != null ? flagDay.AfsReminderIssueIndicator : null;
            model.Remark = flagDay.Remark;
            model.RowVersion = flagDay.RowVersion;

            model.ReviewCaseIndicator = flagDay.ReviewCaseIndicator != null ? (bool)flagDay.ReviewCaseIndicator : false;
            model.FundRaisingPurpose = flagDay.FundRaisingPurpose;
            model.ChiFundRaisingPurpose = flagDay.ChiFundRaisingPurpose;
            model.JointApplicationIndicator = flagDay.JointApplicationIndicator != null ? (bool)flagDay.JointApplicationIndicator : false;
            model.Section88Indicator = flagDay.Section88Indicator != null ? (bool)flagDay.Section88Indicator : false;

            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(flagDay.OrgMaster.OrgId);
            var WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate;
            var WithholdingEndDate = WithHoldingDate.WithholdingEndDate;
            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(flagDay.OrgMaster.OrgId);
            model.OrgRef = flagDay.OrgMaster.OrgRef;
            model.LblWithholdingBeginDate = WithholdingBeginDate;
            model.LblWithholdingEndDate = WithholdingEndDate;
            model.LblPspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "";
            model.LblPspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "";
            model.LblPspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "";
            model.LblFdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "";
            model.LblFdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "";
            model.LblFdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "";

            // Form the Past two Flag Year String
            string Past1stFdYear = _flagDayService.switchToPreviousFdYear(flagDay.FdYear);
            string Past2ndFdYear = _flagDayService.switchToPreviousFdYear(Past1stFdYear);

            // Get the Past Benchmark Status
            model.LblFdBenchmarkStatusPast1st = _flagDayService.GetFdBenchmarkStatusByFdYearAndOrg(Past1stFdYear, flagDay.OrgMaster, benchmarkTWFD, benchmarkRFD);
            model.LblFdBenchmarkStatusPast2nd = _flagDayService.GetFdBenchmarkStatusByFdYearAndOrg(Past2ndFdYear, flagDay.OrgMaster, benchmarkTWFD, benchmarkRFD);

            return model;
        }

        #endregion New

        #region Edit

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("{fdMasterId:int}/Edit", Name = "EditFdMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Update", "CreateFdEvent", "UpdateFdEvent")]
        public ActionResult Edit(int fdMasterId, string ReturnUrl)
        {
            FlagDayViewModel model = new FlagDayViewModel();
            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + ReturnUrl;
                model.PrePage = prePage;
            }
            var languages = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
            if (languages.Count == 0) languages.Add("", "");
            var flagDay = _flagDayService.GetFDById(fdMasterId);
            //var fdEvent = _fdEventService.GetEveByFdMasterId(model.FdMasterId);
            model.FlagDayEventStartTime = _parameterService.GetParameterByCode("FlagDayEventStartTime").Value;
            model.FlagDayEventEndTime = _parameterService.GetParameterByCode("FlagDayEventEndTime").Value;
            //drop down list properties
            initFdViewModel(model, 2);

            model.EventRemarks = fdEventRemarks;

            //general properties
            model = getDataFromFdMaster(model, flagDay);
            model.RowVersion = flagDay.RowVersion;
            model.OrgMasterId = flagDay.OrgMaster == null ? "" : flagDay.OrgMaster.OrgId.ToString();
            //model.IsShowNewApplicant = flagDay.OrgMaster == null ? true : _flagDayService.IsShowNewApplicant(flagDay.OrgMaster.OrgId);

            ViewData["IS_FD_APPROVER"] = HttpContext.User.GetPspsUser().IsSysAdmin || HttpContext.User.IsInRole(Allow.FdApprove.GetName());
            ViewData["FD_REMINDER_DEADLINE"] = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE).Value);
            ViewData["FD_REMINDER_DEADLINE2"] = Convert.ToInt32(_parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE2).Value);

            return View(model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/flagday/update", Name = "UpdateFlagDay")]
        public JsonResult UpdateFlagDay([CustomizeValidator(RuleSet = "Update")] FlagDayViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var fdReminderDeadlineParam = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE);
            var fdReminderDeadlineParam2 = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE2);
            var flagDay = _flagDayService.GetFDById(model.FdMasterId);
            var fdEvent = _fdEventService.GetEveByFdMasterId(model.FdMasterId);

            var oldFlagDay = new FdMaster();
            AutoMapper.Mapper.Map(flagDay, oldFlagDay);
            var orgMaster = _organisationService.GetOrgByRef(model.CreateModelOrgRef);

            flagDay.FdRef = model.ReferenceNumber;
            flagDay.ApplicationReceiveDate = model.DateofReceivingApplication;
            flagDay.UsedLanguage = model.LanguageUsedId;
            flagDay.OrgMaster = orgMaster;
            flagDay.ContactPersonSalute = model.EngSalute;
            flagDay.ContactPersonFirstName = model.ContactPersonFirstName;
            flagDay.ContactPersonLastName = model.ContactPersonLastName;
            flagDay.ContactPersonChiFirstName = model.ContactPersonChiFirstName;
            flagDay.ContactPersonChiLastName = model.ContactPersonChiLastName;
            flagDay.ContactPersonPosition = model.ContactPersonPost;
            flagDay.ContactPersonTelNum = model.TelNo;
            flagDay.ContactPersonFaxNum = model.FaxNo;
            flagDay.ContactPersonEmailAddress = model.ContactPersonEmailAddress;
            flagDay.ApplyForTwr = model.TWR;
            flagDay.ApplicationResult = model.ApplicationResultId;
            flagDay.ResultApplicationRemark = model.ResultApplicationRemark;
            flagDay.VettingPanelCaseIndicator = model.VettingPanelCaseIndicator;
            flagDay.FdYear = model.YearofFlagDay;
            flagDay.LotGroup = model.GroupingwithNoofLot;
            flagDay.TargetIncome = model.TargetIncome;
            flagDay.NewApplicantIndicator = model.CreateModelNewApplicant;
            flagDay.TrackRecordStartDate = model.TrackRecordStartDate;
            flagDay.TrackRecordEndDate = model.TrackRecordEndDate;
            flagDay.TrackRecordDetails = model.TrackRecordDetails;
            flagDay.AfsRecordStartDate = model.AfsRecordStartDate;
            flagDay.AfsRecordEndDate = model.AfsRecordEndDate;
            flagDay.AfsRecordDetails = model.AfsRecordDetails;
            flagDay.FdGroup = model.GroupingId;
            flagDay.FdGroupPercentage = model.PercentageForGrouping;
            flagDay.GroupingResult = model.GroupingResult;
            flagDay.CommunityChest = model.CommunityChest;
            flagDay.ConsentLetter = model.ConsentLetter;
            //flagDay.OutstandingEmailIssueDate = model.OutstandingEmailIssueDate;
            //flagDay.OutstandingEmailReplyDate = model.OutstandingEmailReplyDate;
            flagDay.ApplicationRemark = model.ApplicationRemark;
            //flagDay.OutstandingEmailReminderIssueDate = model.OutstandingEmailReminderIssueDate;
            //flagDay.OutstandingEmailReminderReplyDate = model.OutstandingEmailReminderReplyDate;
            flagDay.FdLotNum = model.FdLotNum;
            flagDay.FdLotResult = model.FdLotResult;
            flagDay.PriorityNum = model.PriorityNum;
            //flagDay.ProposalDetail = model.ProposalDetail;
            //flagDay.ChiProposalDetail = model.ChiProposalDetail;
            //flagDay.FlagSalePurpose = model.FlagSalePurpose;
            //flagDay.ChiFlagSalePurpose = model.ChiFlagSalePurpose;
            flagDay.AcknowledgementReceiveDate = model.AcknowledgementReceiveDate;
            flagDay.ApplyPledgingMechanismIndicator = model.ApplyPledgingMechanismIndicator;
            flagDay.PledgingAmt = model.PledgingAmt;
            flagDay.PledgingProposal = model.PledgingProposal;
            flagDay.ChiPledgingProposal = model.ChiPledgingProposal;
            flagDay.PledgingApplicationRemark = model.PledgingApplicationRemark;
            flagDay.ReviewCaseIndicator = model.ReviewCaseIndicator;
            flagDay.FundRaisingPurpose = model.FundRaisingPurpose;
            flagDay.ChiFundRaisingPurpose = model.ChiFundRaisingPurpose;
            flagDay.JointApplicationIndicator = model.JointApplicationIndicator;
            flagDay.Section88Indicator = model.Section88Indicator;

            flagDay.DocSubmission = model.DocSubmission;
            flagDay.SubmissionDueDate = model.SubmissionDueDate;
            flagDay.FirstReminderIssueDate = model.FirstReminderIssueDate;
            //            flagDay.FirstReminderDeadline = model.FirstReminderDeadline;
            flagDay.FirstReminderDeadline = model.FirstReminderIssueDate != null ? (DateTime?)model.FirstReminderIssueDate.Value.AddDays(int.Parse(fdReminderDeadlineParam.Value)) : null;
            flagDay.SecondReminderIssueDate = model.SecondReminderIssueDate;
            //            flagDay.SecondReminderDeadline = model.SecondReminderDeadline;
            flagDay.SecondReminderDeadline = model.SecondReminderIssueDate != null ? (DateTime?)model.SecondReminderIssueDate.Value.AddDays(int.Parse(fdReminderDeadlineParam2.Value)) : null;
            flagDay.AuditReportReceivedDate = model.AuditReportReceivedDate;
            flagDay.PublicationReceivedDate = model.PublicationReceivedDate;
            flagDay.DocReceiveRemark = model.DocReceiveRemark;
            flagDay.DocRemark = model.DocRemark;
            flagDay.StreetCollection = model.StreetCollection;
            flagDay.GrossProceed = model.GrossProceed;
            flagDay.Expenditure = model.Expenditure;
            flagDay.NetProceed = model.NetProceed;
            flagDay.NewspaperPublishDate = model.NewspaperPublishDate;
            flagDay.PledgedAmt = model.PledgedAmt;
            flagDay.AcknowledgementEmailIssueDate = model.AcknowledgementEmailIssueDate;
            flagDay.WithholdingListIndicator = model.WithholdingListIndicator;
            flagDay.WithholdingBeginDate = model.WithholdingBeginDate;
            flagDay.WithholdingEndDate = model.WithholdingEndDate;
            flagDay.WithholdingRemark = model.WithholdingRemark;
            flagDay.AfsReceiveIndicator = model.AfsReceiveIndicator;
            flagDay.RequestPermitteeIndicator = model.RequestPermitteeIndicator;
            flagDay.AfsReSubmitIndicator = model.AfsReSubmitIndicator;
            flagDay.AfsReminderIssueIndicator = model.AfsReminderIssueIndicator;
            flagDay.Remark = model.Remark;
            flagDay.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                _flagDayService.UpdateFdMaster(oldFlagDay, flagDay);

                if (fdEvent != null && fdEvent.FrasCharityEventId.IsNotNullOrEmpty() && oldFlagDay.OrgMaster.OrgId != flagDay.OrgMaster.OrgId)
                {
                    fdEvent.FrasStatus = fdEvent.FrasStatus == "RR" ? "UR" : "RU";
                    _fdEventService.Update(fdEvent);
                }
                _unitOfWork.Commit();
            }

            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(flagDay.OrgMaster.OrgId);
            var WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate;
            var WithholdingEndDate = WithHoldingDate.WithholdingEndDate;

            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(flagDay.OrgMaster.OrgId);

            model.OrgRef = flagDay.OrgMaster.OrgRef;
            model.OrgEngName = flagDay.OrgMaster.EngOrgName;
            model.OrgChiName = flagDay.OrgMaster.ChiOrgName;
            model.LblWithholdingBeginDate = WithholdingBeginDate;
            model.LblWithholdingEndDate = WithholdingEndDate;

            model.LblPspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "";
            model.LblPspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "";
            model.LblPspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "";
            model.LblFdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "";
            model.LblFdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "";
            model.LblFdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "";

            // Form the Past two Flag Year String
            string Past1stFdYear = _flagDayService.switchToPreviousFdYear(flagDay.FdYear);
            string Past2ndFdYear = _flagDayService.switchToPreviousFdYear(Past1stFdYear);

            // Get the Past Benchmark Status
            model.LblFdBenchmarkStatusPast1st = _flagDayService.GetFdBenchmarkStatusByFdYearAndOrg(Past1stFdYear, flagDay.OrgMaster, benchmarkTWFD, benchmarkRFD);
            model.LblFdBenchmarkStatusPast2nd = _flagDayService.GetFdBenchmarkStatusByFdYearAndOrg(Past2ndFdYear, flagDay.OrgMaster, benchmarkTWFD, benchmarkRFD);

            model.RowVersion = flagDay.RowVersion;
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = model
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagday/{fdMasterId:int}/CalFdEditRecCnt", Name = "CalFdEditRecCnt")]
        public JsonResult CalFdEditRecCnt(int fdMasterId)
        {
            Ensure.Argument.NotNull(fdMasterId);
            var map = _flagDayService.CalFdEditRecCnt(fdMasterId);
            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Edit

        #region Search

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("Search", Name = "SearchFdMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            FlagDayViewModel model = new FlagDayViewModel();
            model.isFirstSearch = true;
            model.IsFdApprover = HttpContext.User.GetPspsUser().IsSysAdmin || HttpContext.User.IsInRole(Allow.FdApprove.GetName());
            model.FlagDayEnableImport = (DateTime.ParseExact(_parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_ENABLE_IMPORT).Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToUniversalTime().Ticks -
                                         (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks) / 10000;

            initFdViewModel(model, 0);

            return View(model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("~/flagday/returnSearch", Name = "FlagDaySearchPage")]
        public ActionResult ReturnSearch()
        {
            FlagDayViewModel model = new FlagDayViewModel();
            var session = this.HttpContext.Session[FlagDay_SEARCH_SESSION];
            if (session != null)
            {
                model = ((FlagDayViewModel)(this.HttpContext.Session[FlagDay_SEARCH_SESSION]));
                model.isFirstSearch = false;
            }

            initFdViewModel(model, 0);

            return View("Search", model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagday/list", Name = "ListFlagDaySearch")]
        public JsonResult ListFlagDaySearch(GridSettings grid, FlagDayViewModel model)
        {
            Ensure.Argument.NotNull(grid);

            var flagDays = _flagDayService.GetPageByFlagDaySearchDto(grid);

            var gridResult = new GridResult
            {
                TotalPages = flagDays.TotalPages,
                CurrentPageIndex = flagDays.CurrentPageIndex,
                TotalCount = flagDays.TotalCount,
                Data = (
                        from u in flagDays
                        select new
                        {
                            OrgRef = u.OrgRef,
                            OrgName = u.OrgName,
                            EngOrgName = u.EngOrgName,
                            ChiOrgName = u.ChiOrgName,
                            SubventedIndicator = u.SubventedIndicator,
                            FdRef = u.FdRef,
                            PermitNum = u.PermitNum,
                            FlagDay = u.FlagDay,
                            TWR = string.IsNullOrEmpty(u.TWR) ? "" : twrs[u.TWR],
                            NewApplicantIndicator = u.NewApplicantIndicator,
                            ApplicationResult = string.IsNullOrEmpty(u.ApplicationResult) ? "" : fdApplicationResults[u.ApplicationResult],
                            ApplicationReceiveDate = u.ApplicationReceiveDate,
                            DisableIndicator = u.DisableIndicator,
                            ContactPersonName = u.ContactPersonName,
                            ContactPersonChiName = u.ContactPersonChiName,
                            //TwrDistrict = string.IsNullOrEmpty(u.TwrDistrict) ? "" : twrDistricts[u.TwrDistrict],
                            TwrDistrict = u.TwrDistrict,
                            EngMailingAddress = String.Join(" ", new String[] { u.EngMailingAddress1, u.EngMailingAddress2, u.EngMailingAddress3, u.EngMailingAddress4, u.EngMailingAddress5 }),
                            ChiMailingAddress = String.Join(" ", new String[] { u.ChiMailingAddress1, u.ChiMailingAddress2, u.ChiMailingAddress3, u.ChiMailingAddress4, u.ChiMailingAddress5 }),
                            ContactPerson = !String.IsNullOrEmpty(u.ContactPersonSalute) ? _lookupService.GetDescription(LookupType.Salute, u.ContactPersonSalute) + " " + u.ContactPersonFirstName + " " + u.ContactPersonLastName : u.ContactPersonFirstName + " " + u.ContactPersonLastName,
                            ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                            EmailAddress = u.EmailAddress,
                            FdMasterId = u.FdMasterId,
                            PermitRevokeIndicator = u.PermitRevokeIndicator,
                            ApplicationResultInLastYear = u.ApplicationResultInLastYear,
                            LotGroupInLastYear = u.LotGroupInLastYear,
                            RefLotGroup = u.RefLotGroup
                        }
                       ).ToArray()
            };

            this.HttpContext.Session[FlagDay_SEARCH_SESSION] = model;

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagday/organisation/list", Name = "ListFlagDayOrganisation")]
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

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagday/ExportFlagDay/{gridRecCnt}", Name = "ExportFlagDay")]
        public JsonResult ExportFlagDay(ExportSettings exportSettings)
        {
            var ms = _flagDayService.GetPageToXls(exportSettings.GridSettings);

            return JsonFileResult("FdMaster", "FdMaster" + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagday/ImportFlagDay", Name = "ImportFlagDay")]
        public JsonResult ImportFlagDay([CustomizeValidator(RuleSet = "ImportFlagDay")] FlagDayViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            MemoryStream logStream;
            int warning;

            using (_unitOfWork.BeginTransaction())
            {
                logStream = _flagDayService.InserFlagDayByImportXls(model.ImportFile.InputStream, out warning);
                if (warning < 2)
                    _unitOfWork.Commit();

                if (logStream == null )
                {   
                    return Json(new JsonResponse(true)
                    {
                        Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                    }, "text/html", JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    String sessionId = "ImportFlagDayLog";
                    return JsonFileResult(sessionId, "ImportFlagDayLog" + time + ".txt", logStream, "text/html", warning == 1 ? _messageService.GetMessage(SystemMessage.Info.RecordUpdated).TrimEnd('.') + ", but with warning" : "");
                }
            }
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagday/exportFlagDayGridData", Name = "ExportFlagDayGridData")]
        public JsonResult ExportFlagDayGridData(ExportSettings exportSettings)
        {
            FlagDayViewModel model = ((FlagDayViewModel)(this.HttpContext.Session[FlagDay_SEARCH_SESSION]));

            var flagDays = _flagDayService.GetPageByFlagDaySearchDto(exportSettings.GridSettings);
            var dataList = (from u in flagDays
                            select new
                            {
                                OrgRef = u.OrgRef,
                                OrgName = u.OrgName,
                                EngOrgName = u.EngOrgName,
                                ChiOrgName = u.ChiOrgName,
                                SubventedIndicator = u.SubventedIndicator ? "YES" : "NO",
                                FdGroup =  string.IsNullOrEmpty(u.FdGroup) ? "" : fdGroupings[u.FdGroup],
                                FdRef = u.FdRef,
                                PermitNum = u.PermitNum,
                                FlagDay = u.FlagDay,
                                TWR = string.IsNullOrEmpty(u.TWR) ? "" : twrs[u.TWR],
                                NewApplicantIndicator = u.NewApplicantIndicator != null ? (u.NewApplicantIndicator.Value ? "YES" : "NO") : "",
                                ApplicationResult = string.IsNullOrEmpty(u.ApplicationResult) ? "" : fdApplicationResults[u.ApplicationResult],
                                ApplicationReceiveDate = u.ApplicationReceiveDate,
                                DisableIndicator = u.DisableIndicator != null ? (u.DisableIndicator.Value ? "YES" : "NO") : "",
                                ContactPersonName = u.ContactPersonName,
                                ContactPersonChiName = u.ContactPersonChiName,
                                TwrDistrict = string.IsNullOrEmpty(u.TwrDistrict) ? "" : twrDistricts[u.TwrDistrict],
                                EngMailingAddress = String.Join(" ", new String[] {u.EngMailingAddress1, u.EngMailingAddress2, u.EngMailingAddress3, u.EngMailingAddress4, u.EngMailingAddress5}),
                                ChiMailingAddress = String.Join(" ", new String[] {u.ChiMailingAddress1, u.ChiMailingAddress2, u.ChiMailingAddress3, u.ChiMailingAddress4, u.ChiMailingAddress5}),
                                ContactPerson = !String.IsNullOrEmpty(u.ContactPersonSalute) ? _lookupService.GetDescription(LookupType.Salute, u.ContactPersonSalute) + " " + u.ContactPersonFirstName + " " + u.ContactPersonLastName : u.ContactPersonFirstName + " " + u.ContactPersonLastName,
                                ContactPersonEmailAddress = u.ContactPersonEmailAddress,
                                EmailAddress = u.EmailAddress,
                                FdMasterId = u.FdMasterId,
                                ApplicationResultInLastYear = u.ApplicationResultInLastYear,
                                LotGroupInLastYear = u.LotGroupInLastYear,
                                RefLotGroup = u.RefLotGroup
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<FlagDayViewModel>();

            if (model.OrgRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgRef"] + " : ORG" + model.OrgRef);

            if (model.OrgName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgName"] + " : " + model.OrgName);

            if (model.OrgStatusId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + this._lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrgStatusId]);
            }

            if (model.ReferenceNumber.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ReferenceNumber"] + " : " + model.ReferenceNumber);

            if (model.FromDateofReceiveingApplication != null && model.ToDateofReceiveingApplication != null)
            {
                tmpVal = "From " + model.FromDateofReceiveingApplication.Value.ToString(DATE_FORMAT) + " to " + model.ToDateofReceiveingApplication.Value.ToString(DATE_FORMAT);
            }
            else if (model.FromDateofReceiveingApplication != null)
            {
                tmpVal = "From " + model.FromDateofReceiveingApplication.Value.ToString(DATE_FORMAT);
            }
            else if (model.ToDateofReceiveingApplication != null)
            {
                tmpVal = "To " + model.ToDateofReceiveingApplication.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["DateofReceivingApplication"] + " : " + tmpVal);

            tmpVal = "";
            if (model.PermitNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PermitNo"] + " : " + model.PermitNo);
            if (model.FromFlagDay.IsNotNullOrEmpty() && model.ToFlagDay.IsNotNullOrEmpty())
            {
                tmpVal = "From " + model.FromFlagDay + " to " + model.ToFlagDay;
            }
            else if (model.FromFlagDay.IsNotNullOrEmpty())
            {
                tmpVal = "From " + model.FromFlagDay;
            }
            else if (model.ToFlagDay.IsNotNullOrEmpty())
            {
                tmpVal = "To " + model.ToFlagDay;
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["FromFlagDay"] + " : " + tmpVal);

            if (model.OrgStatusId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + model.OrgStatusId);

            if (model.Subvented != null)
                filterCriterias.Add(fieldNames["Subvented"] + " : " + (model.Subvented.Value ? "False" : "True"));

            if (model.ContactPersonName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ContactPersonName"] + " : " + model.ContactPersonName);

            if (model.NewApplicant != null)
                filterCriterias.Add(fieldNames["NewApplicant"] + " : " + (model.NewApplicant.Value ? "False" : "True"));

            if (model.ApplyForTWR != null)
            {
                filterCriterias.Add(fieldNames["ApplyForTWR"] + " : " + _lookupService.getAllLkpInCodec(LookupType.TWR)[model.ApplyForTWR]);
            }

            if (model.GroupingId != null)
            {
                filterCriterias.Add(fieldNames["GroupingId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.FdGrouping)[model.GroupingId]);
            }

            if (model.TWR != null)
            {
                filterCriterias.Add(fieldNames["TerritoryRegion"] + " : " + _lookupService.getAllLkpInCodec(LookupType.TWR)[model.TWR]);
            }

            if (model.DistrictId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["DistrictId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.TWRDistrict)[model.DistrictId]);
            }

            if (model.FdYear != null)
            {
                filterCriterias.Add(fieldNames["FdYear"] + " : " + String.Join(", ", model.FdYear));
            }

            if (model.LanguageUsedId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["LanguageUsedId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.LanguageUsed)[model.LanguageUsedId]);
            }

            if (model.FdLotResult.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["FdLotResult"] + " : " + _lookupService.getAllLkpInCodec(LookupType.FdLotResult)[model.FdLotResult]);
            }

            if (model.SearchApplyPledgingMechanismIndicator.HasValue) 
            {
                filterCriterias.Add(fieldNames["SearchApplyPledgingMechanismIndicator"] + " : " + (model.SearchApplyPledgingMechanismIndicator.Value  ? "False" : "True"));
            }

            if (model.SearchPermitRevokeIndicator.HasValue)
            {
                filterCriterias.Add(fieldNames["SearchPermitRevokeIndicator"] + " : " + (model.SearchPermitRevokeIndicator.Value ? "False" : "True"));
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonFileResult(sessionId, time + ".xlsx", ms);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Update")]
        public PartialViewResult RenderFlagDayTemplateModal()
        {
            FlagDayDocViewModel model = new FlagDayDocViewModel();
            return PartialView("_FlagDayTemplateModal", model);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagday/templatetab/list", Name = "ListFlagDayTemplateTab")]
        public JsonResult ListFlagDayTemplateTab(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var template = _flagDayDocService.GetPage(grid);

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
                            FdDocId = l.FdDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("{id}/template/{fdDocId}/generate", Name = "GenerateFlagDayTemplate")]
        public ActionResult GenerateFlagDayTemplate(int fdDocId, int id)
        {
            Ensure.Argument.NotNull(fdDocId);
            Ensure.Argument.NotNull(id);

            var template = _flagDayDocService.GetFdDocById(fdDocId);
            var fd = _flagDayDocService.GetFdDocViewById(id);
            var sysParam = _parameterService.GetParameterByCode("FlagDayTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            SimpleDocumentGenerator<FdDocView> docGenerator = new SimpleDocumentGenerator<FdDocView>(new DocumentGenerationInfo
            {
                DataContext = fd,
                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
            });
            return File(docGenerator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", template.DocName + ".docx");
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("template/{fdDocId}/generateMergedTemplate", Name = "GenerateMergedFlagDayTemplate")]
        public ActionResult GenerateMergedFlagDayTemplate(ExportSettings exportSettings, int fdDocId)
        {
            var flagDays = _flagDayService.GetPageByFlagDaySearchDto(exportSettings.GridSettings);
            string tempFolderPath = Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            string tempFilePath = "";
            string time = "";
            MemoryStream stream = null;

            var template = _flagDayDocService.GetFdDocById(fdDocId);

            var sysParam = _parameterService.GetParameterByCode("FlagDayTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            //Create temp folder
            CommonHelper.CreateFolderIfNeeded(tempFolderPath);

            try
            {
                foreach (FlagDaySearchDto flagDay in flagDays)
                {
                    var fd = _flagDayDocService.GetFdDocViewById(flagDay.FdMasterId);

                    System.Diagnostics.Debug.WriteLine(flagDay.FdMasterId);

                    SimpleDocumentGenerator<FdDocView> docGenerator = new SimpleDocumentGenerator<FdDocView>(new DocumentGenerationInfo
                    {
                        DataContext = fd,
                        TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
                    });

                    time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    tempFilePath = Path.Combine(tempFolderPath, time + ".docx");

                    docGenerator.ToFile(tempFilePath);
                }

                List<string> arrayList = Directory.GetFiles(tempFolderPath).ToList();

                time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                tempFilePath = Path.Combine(tempFolderPath, time + ".docx");
                _reportService.DocumentsMerge(tempFilePath, arrayList);

                byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
                stream = new MemoryStream(fileBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Delete Temp Folder
                Directory.Delete(tempFolderPath, true);
            }

            return JsonFileResult("FlagDayTemplate", template.DocName + ".docx", stream);
        }

        #endregion Search

        #region Report

        [PspsAuthorize(Allow.FdReport)]
        [RuleSetForClientSideMessagesAttribute("default", "R2", "R3", "R4", "R24")]
        public ActionResult Report()
        {
            FlagDayReportViewModel model = new FlagDayReportViewModel();
            model.R24_FdYears = new Dictionary<string, string>();
            var fdYears = _flagDayListService.GetAllFlagDayListYearForDropdown();
            foreach (var fdYear in fdYears)
            {
                model.R24_FdYears.Add(fdYear.Key, "20{0} for FD{1}".FormatWith((Convert.ToInt32(fdYear.Value.Substring(0, 2)) - 2).ToString().PadLeft(2, '0'), fdYear.Value));
            }

            model.FdYears = fdYears;
            return View(model);
        }

        [PspsAuthorize(Allow.FdReport)]
        [HttpGet, Route("~/api/flagday/report/list", Name = "ListFlagDayReport")]
        public JsonResult ListFlagDayReport(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var flagDay = this._flagDayDocService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = flagDay.TotalPages,
                CurrentPageIndex = flagDay.CurrentPageIndex,
                TotalCount = flagDay.TotalCount,
                Data = (from s in flagDay
                        select new
                        {
                            fileRefNum = s.Id,
                            description = s.DocName,
                            //parameters = s.FlagDayDate
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #region R3

        [PspsAuthorize(Allow.FdReport)]
        [HttpPost, Route("~/api/report/r3/generate", Name = "R3Generate")]
        public JsonResult R3Generate([CustomizeValidator(RuleSet = "default,R3")]FlagDayReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R03";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR3Excel(templatePath, model.R3FromYear, model.R3ToYear);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion R3

        #region R24

        [PspsAuthorize(Allow.FdReport)]
        [HttpPost, Route("~/api/report/r24/generate", Name = "R24Generate")]
        public JsonResult R24Generate([CustomizeValidator(RuleSet = "default,R24")]FlagDayReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var reportId = "R24";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR24Excel(templatePath, model.R24_Year);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion R24

        #region Export Fd

        [PspsAuthorize(Allow.FdReport)]
        [HttpPost, Route("~/api/report/rawData3/generate", Name = "RawData3Generate")]
        public JsonResult RawData3Generate(FlagDayReportViewModel model)
        {
            var reportId = "RawDataOfFD";
            string strTable = "FdRawView";
            string strWhere = null;

            if (model.Raw3_Year.IsNotNullOrEmpty())
                strWhere = "WHERE YearofFlagDay = '{0}'".FormatWith(model.Raw3_Year);

            Dictionary<string, string> fieldNames = new Dictionary<string, string>();
            var properties = typeof(FlagDayViewModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DisplayAttribute attr = (DisplayAttribute)System.Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
                if (attr != null)
                    fieldNames.Add(property.Name, attr.GetName());
            }

            var ms = _reportService.ExportTableToExcel(reportId, strTable, fieldNames, strWhere);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion Export Fd

        #endregion Report

        #region Complaint & Enquiry

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/fd/listComplaintEnquiry/{type}", Name = "ListfdComplaintEnquiry")]
        public JsonResult ListfdComplaintEnquiry(GridSettings grid, string code, string type)
        {
            grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "fdRef",
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
                            FollowUpAction = c.FollowUpAction,
                            PspRef = c.PspRef,
                            ReplyDueDate = c.ReplyDueDate,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Complaint & Enquiry

        #region Complaint

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/fd/complaint/{fdMasterId}/listFdComplaint", Name = "ListFdComplaint")]
        public JsonResult ListFdComplaint(GridSettings grid, int fdMasterId)
        {
            Ensure.Argument.NotNull(grid);

            var complaintMaster = _organisationService.getPageComplaintByFdMasterId(grid, fdMasterId);

            var gridResult = new GridResult
            {
                TotalPages = complaintMaster.TotalPages,
                CurrentPageIndex = complaintMaster.CurrentPageIndex,
                TotalCount = complaintMaster.TotalCount,
                Data = (
                        from u in complaintMaster
                        select new
                        {
                            ComplaintRef = string.IsNullOrEmpty(u.ComplaintRef) ? "" : u.ComplaintRef,
                            ComplaintSource = string.IsNullOrEmpty(u.ComplaintSource) ? "" : complaintSources[u.ComplaintSource],
                            ActivityConcern = string.IsNullOrEmpty(u.ActivityConcern) ? "" : activityConcerned[u.ActivityConcern],
                            ComplaintDate = u.ComplaintDate,
                            PermitNum = string.IsNullOrEmpty(u.PermitNum) ? "" : u.PermitNum,
                            FollowUpLetterType = string.IsNullOrEmpty(u.FollowUpLetterType) ? "" : issueLetterType[u.FollowUpLetterType],
                            FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                            LetterIssuedNum = u.LetterIssuedNum,
                            ComplaintRemarks = u.ComplaintRemarks,
                            ComplaintMasterId = u.ComplaintMasterId,
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Complaint

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/fd/enquiry/{fdMasterId}/listFdEnquiry", Name = "ListFdEnquiry")]
        public JsonResult ListFdEnquiry(GridSettings grid, int fdMasterId)
        {
            Ensure.Argument.NotNull(grid);

            var complaintMaster = _organisationService.getPageEnquiryByFdMasterId(grid, fdMasterId);

            var gridResult = new GridResult
            {
                TotalPages = complaintMaster.TotalPages,
                CurrentPageIndex = complaintMaster.CurrentPageIndex,
                TotalCount = complaintMaster.TotalCount,
                Data = (
                        from u in complaintMaster
                        select new
                        {
                            ComplaintRef = string.IsNullOrEmpty(u.ComplaintRef) ? "" : u.ComplaintRef,
                            ComplaintSource = string.IsNullOrEmpty(u.ComplaintSource) ? "" : complaintSources[u.ComplaintSource],
                            ActivityConcern = string.IsNullOrEmpty(u.ActivityConcern) ? "" : activityConcerned[u.ActivityConcern],
                            ComplaintDate = u.ComplaintDate,
                            PermitNum = string.IsNullOrEmpty(u.PermitNum) ? "" : u.PermitNum,
                            FollowUpLetterType = string.IsNullOrEmpty(u.FollowUpLetterType) ? "" : issueLetterType[u.FollowUpLetterType],
                            FollowUpLetterIssueDate = u.FollowUpLetterIssueDate,
                            LetterIssuedNum = u.LetterIssuedNum,
                            ComplaintRemarks = u.ComplaintRemarks,
                            ComplaintMasterId = u.ComplaintMasterId,
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #region Template

        #region "Template link"

        [PspsAuthorize(Allow.FdTemplate)]
        public ActionResult Template()
        {
            FlagDayDocViewModel model = new FlagDayDocViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpGet, Route("~/api/flagday/template/list", Name = "ListFlagDayTemplate")]
        public JsonResult ListFlagDayTemplate(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            var flagDay = _flagDayDocService.GetFdDocSummaryViewPage(grid);
            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = flagDay.TotalPages,
                CurrentPageIndex = flagDay.CurrentPageIndex,
                TotalCount = flagDay.TotalCount,
                Data = (from f in flagDay
                        select new
                        {
                            DocNum = f.DocNum,
                            DocName = f.DocName,
                            VersionNum = f.VersionNum,
                            Enabled = f.Enabled,
                            FlagDayDocId = f.FdDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/flagDay/template/new", Name = "NewFlagDayDoc")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,Create,CreateFlagDayDoc")] FlagDayDocViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template file from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_TEMPLATE_PATH);

            // Rename the file name by adding the current times
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path
            string rootPath = templatePath.Value;
            // Form the Relative Path for storing in DB
            // and Absolute Path for actually saving the file
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new FdDoc row and fill the value
            var flagDayDoc = new FdDoc();
            flagDayDoc.DocNum = model.DocNum;
            flagDayDoc.DocStatus = true;
            flagDayDoc.DocName = model.Description;
            flagDayDoc.VersionNum = model.Version;
            flagDayDoc.RowVersion = model.RowVersion;
            flagDayDoc.FileLocation = relativePath;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.File.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                this._flagDayDocService.CreateFdDoc(flagDayDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        //Replaced by function VersionEdit
        //[PspsAuthorize(Allow.FdTemplate)]
        //[ValidateAntiForgeryToken]
        //[HttpPost, Route("~/api/flagday/template/{flagDayDocId:int}/edit", Name = "EditFlagDayVersion")]
        //public JsonResult Edit(int flagDayDocId, [CustomizeValidator(RuleSet = "default,UpdateFlagDayDoc")]  FlagDayDocViewModel model)
        //{
        //    Ensure.Argument.NotNullOrEmpty(flagDayDocId.ToString());
        //    Ensure.Argument.NotNull(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
        //    }
        //    FdDoc flagDay = new FdDoc();
        //    flagDay = this._flagDayDocService.GetFdDocById(flagDayDocId);
        //    flagDay.DocNum = model.DocNum;
        //    flagDay.DocName = model.Description;
        //    flagDay.VersionNum = model.Version;
        //    flagDay.DocStatus = model.IsActive;

        //    using (_unitOfWork.BeginTransaction())
        //    {
        //        this._flagDayDocService.UpdateFdDoc(flagDay);
        //        _unitOfWork.Commit();
        //    }

        //    return Json(new JsonResponse(true)
        //    {
        //        Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
        //    }, JsonRequestBehavior.DenyGet);
        //}

        #endregion "Template link"

        #region FlagDay Version

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpGet, Route("Doc/{flagDayDocId:int}/Version", Name = "FlagDayVersion")]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "NewVersion")]
        public ActionResult Version(int flagDayDocId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var flagDay = this._flagDayDocService.GetFdDocById(flagDayDocId);

            Ensure.NotNull(flagDay, "No letter found with the specified id");
            FlagDayDocViewModel model = new FlagDayDocViewModel();
            model.DocNum = flagDay.DocNum;
            return View(model);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpGet, Route("~/api/flagday/doc/{docNum}/list", Name = "ListFlagDayVersion")]
        public JsonResult ListVersion(GridSettings grid, string docNum)
        {
            Ensure.Argument.NotNull(grid);
            var flagDay = this._flagDayDocService.GetPage(grid, docNum);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = flagDay.TotalPages,
                CurrentPageIndex = flagDay.CurrentPageIndex,
                TotalCount = flagDay.TotalCount,
                Data = (from s in flagDay
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            VersionNum = s.VersionNum,
                            DocStatus = s.DocStatus,
                            RowVersion = s.RowVersion,
                            flagDayDocId = s.FdDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/flagday/doc/new", Name = "NewFlagDayVersion")]
        public JsonResult VersionNew([CustomizeValidator(RuleSet = "default,NewVersion")]FlagDayDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_TEMPLATE_PATH);
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            var flagDayDoc = new FdDoc();
            flagDayDoc.DocNum = model.DocNum;
            flagDayDoc.DocName = model.Description;
            flagDayDoc.DocStatus = model.IsActive;
            flagDayDoc.FileLocation = relativePath;
            flagDayDoc.VersionNum = model.Version;
            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        model.File.SaveAs(absolutePath);

                    _flagDayDocService.CreateFdDoc(flagDayDoc);
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

        [PspsAuthorize(Allow.FdTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/flagday/doc/{flagDayDocId}/edit", Name = "versionEditFlagDay")]
        public JsonResult VersionEdit(int flagDayDocId, [CustomizeValidator(RuleSet = "default,UpdateVersion")]FlagDayDocViewModel model)
        {
            Ensure.Argument.NotNull(flagDayDocId);
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_TEMPLATE_PATH);

            // Set the root path
            string rootPath = templatePath.Value;

            // Paths for new file if needed
            string relativePath = "";
            string absolutePath = "";

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Get the Fddoc record by given the ID
                    var flagDay = _flagDayDocService.GetFdDocById(flagDayDocId);

                    // If new file need to be upload
                    if (model.File != null)
                    {
                        // Reforming the file name by adding current time
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
                        string absolutePathOfOldFile = Path.Combine(rootPath, flagDay.FileLocation);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                            System.IO.File.Delete(absolutePathOfOldFile);

                        // Replace with the Relative Path of the new file
                        flagDay.FileLocation = relativePath;
                    }

                    // Fill the update values
                    flagDay.DocStatus = model.IsActive;
                    flagDay.VersionNum = model.Version;
                    flagDay.DocName = model.Description;

                    // Update DB record and commit
                    _flagDayDocService.UpdateFdDoc(flagDay);
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

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpGet, Route("~/api/flagday/doc/{flagDayDocId:int}", Name = "GetFlagDayDoc")]
        public JsonResult GetFlagDayDoc(int flagDayDocId)
        {
            Ensure.Argument.NotNullOrEmpty(flagDayDocId.ToString());
            var flagDayDoc = this._flagDayDocService.GetFdDocById(flagDayDocId);
            if (flagDayDoc == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            var model = new FlagDayDocViewModel()
            {
                DocNum = flagDayDoc.DocNum,
                Description = flagDayDoc.DocName,
                Version = flagDayDoc.VersionNum,
                IsActive = flagDayDoc.DocStatus,
                RowVersion = flagDayDoc.RowVersion,
                FlagDayDocId = flagDayDoc.Id
            };
            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpPost, Route("~/api/flagday/doc/{flagDayDocId:int}/delete", Name = "DeteteFlagDayDoc")]
        public JsonResult Delete(int flagDayDocId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the Fddoc record by given the ID
            var flagDayDoc = this._flagDayDocService.GetFdDocById(flagDayDocId);
            Ensure.NotNull(flagDayDoc, "No Letter found with the specified id");

            // Get the root path of the Template file from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            string absolutePath = Path.Combine(rootPath, flagDayDoc.FileLocation);

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB
                flagDayDoc.RowVersion = rowVersion;
                _flagDayDocService.DeleteFdDoc(flagDayDoc);

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
                Data = flagDayDoc
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FdTemplate)]
        [HttpGet, Route("Doc/{flagDayDocId:int}/Download", Name = "DownloadFlagDayFile")]
        public FileResult Download(int flagDayDocId)
        {
            // Get the Fddoc record by given the ID
            var flagDay = _flagDayDocService.GetFdDocById(flagDayDocId);

            // Get the root path of the Template from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.FLAGDAY_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            var absolutePath = Path.Combine(rootPath, flagDay.FileLocation);

            // Set the file name for saving
            string fileName = flagDay.DocName + Path.GetExtension(Path.GetFileName(flagDay.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        #endregion FlagDay Version

        #endregion Template

        #region Event

        [HttpGet, Route("~/api/fd/event/list/{fdMasterId}", Name = "ListFdEventsByFdMasterId")]
        public JsonResult ListFdEventsByFdMasterId(GridSettings grid, int fdMasterId)
        {
            Ensure.Argument.NotNull(grid);

            var fdEvent = _fdEventService.GetPageByFdMasterId(grid, fdMasterId);

            var gridResult = new GridResult
            {
                TotalPages = fdEvent.TotalPages,
                CurrentPageIndex = fdEvent.CurrentPageIndex,
                TotalCount = fdEvent.TotalCount,
                Data = (
                        from u in fdEvent
                        select new FdReadEventDto
                        {
                            FdEventId = u.FdEventId,
                            FlagDay = u.FlagDay,
                            Time = u.EventStartTime != null ? u.EventStartTime.Value.ToString("HH:mm") + " - " + u.EventEndTime.Value.ToString("HH:mm") : "",
                            TWR = u.TWR == null ? "" : twrs[u.TWR],
                            //District = u.District == null ? "" : twrDistricts[u.District],
                            District = u.District,
                            PermitNum = u.PermitNum,
                            BagColour = u.BagColour,
                            FlagColour = u.FlagColour,
                            CollectionMethod = decodeByStringArr(u.CollectionMethod, collectionMethods),
                            Remark = decodeByStringArr(u.Remark, fdEventRemarks),
                            PermitRevokeIndicator = u.PermitRevokeIndicator
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/fd/event/{fdEventId}/getFdEventFromGrid", Name = "GetFdEventFromGrid")]
        public JsonResult GetFdEventFromGrid(int fdEventId)
        {
            Ensure.Argument.NotNull(fdEventId);

            var fdEvent = _fdEventService.GetFdEventById(fdEventId);
            var fdMaster = _flagDayService.GetFDById(fdEvent.FdMaster.FdMasterId);

            if (fdEvent == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            
            var model = new FlagDayViewModel();

            //DisasterMasterId = disastersMaster != null ? disastersMaster.DisasterMasterId : 0,
            model.FdEventId = fdEvent.FdEventId;
            model.FlagDay = fdEvent.FlagDay;
            model.FlagTimeFrom = fdEvent.FlagTimeFrom != null ? fdEvent.FlagTimeFrom.Value.ToString("HH") + fdEvent.FlagTimeFrom.Value.ToString("mm") : null;
            model.FlagTimeTo = fdEvent.FlagTimeTo != null ? fdEvent.FlagTimeTo.Value.ToString("HH") + fdEvent.FlagTimeTo.Value.ToString("mm") : null;
            model.PermitNo = fdEvent.PermitNum;
            model.PermitIssueDate = fdEvent.PermitIssueDate;
            model.PermitAcknowledgementReceiveDate = fdEvent.PermitAcknowledgementReceiveDate;
            model.EventTWR = string.IsNullOrEmpty(fdEvent.TWR) ? "" : fdEvent.TWR;
            model.District = string.IsNullOrEmpty(fdEvent.TwrDistrict) ? "" : fdEvent.TwrDistrict;
            model.MethodOfCollection = string.IsNullOrEmpty(fdEvent.CollectionMethod) ? null : fdEvent.CollectionMethod.Split(',').ToList();
            model.BagColour = fdEvent.BagColour;
            model.FlagColour = fdEvent.FlagColour;
            model.PermitRevokeIndicator = fdEvent.PermitRevokeIndicator != null ? (bool)fdEvent.PermitRevokeIndicator : false;
            model.EventRemark = string.IsNullOrEmpty(fdEvent.Remarks) ? new List<string>() : fdEvent.Remarks.Split(',').ToList();
            model.EventRowVersion = fdEvent.RowVersion;
            model.YearofFlagDay = fdMaster.FdYear;
            model.ReferenceNumber = fdMaster.FdRef;
            model.FrasCharityEventId = fdEvent.FrasCharityEventId;
            model.FrasStatus = fdEvent.FrasStatus;

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/updateFdEvent", Name = "UpdateFdEvent")]
        public JsonResult UpdateFdEvent([CustomizeValidator(RuleSet = "UpdateFdEvent")]  FlagDayViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            if (model != null && model.FdEventId.HasValue)
            {
                var fdDueDateParam = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_DUEDATE);
                var fdEvent = _fdEventService.GetFdEventById(model.FdEventId.Value);

                //Validation rule has guarded the FlagDay must be filled
                DateTime flagDay = model.FlagDay.Value;
                DateTime flagTimeFrom = new DateTime(
                                            flagDay.Year,
                                            flagDay.Month,
                                            flagDay.Day,
                                            Convert.ToInt32(model.FlagTimeFrom.Substring(0, 2)),
                                            Convert.ToInt32(model.FlagTimeFrom.Substring(2, 2)), 0);

                DateTime flagTimeTo = new DateTime(
                                            flagDay.Year,
                                            flagDay.Month,
                                            flagDay.Day,
                                            Convert.ToInt32(model.FlagTimeTo.Substring(0, 2)),
                                            Convert.ToInt32(model.FlagTimeTo.Substring(2, 2)), 0);

                //FRAS checking begin
                if (fdEvent.FrasStatus != "RC" && fdEvent.FrasCharityEventId.IsNotNullOrEmpty())
                {
                    if (fdEvent.PermitRevokeIndicator != model.PermitRevokeIndicator)
                    {
                        if (model.PermitRevokeIndicator)
                            fdEvent.FrasStatus = (fdEvent.FrasStatus.IsNullOrEmpty() || fdEvent.FrasStatus == "C") ? "RR" : "UR";
                        else
                            fdEvent.FrasStatus = fdEvent.FrasStatus == "UR" ? "RU" : "";
                    }

                    if (fdEvent.FrasStatus != "UR" && (fdEvent.FlagDay != model.FlagDay
                                                        || fdEvent.FlagTimeFrom != flagTimeFrom
                                                        || fdEvent.FlagTimeTo != flagTimeTo
                                                        || fdEvent.TWR != model.EventTWR
                                                        || (fdEvent.TWR == "2" && fdEvent.TwrDistrict != model.District)
                                                        || (fdEvent.Remarks != (model.EventRemark != null ? string.Join(",", model.EventRemark) : string.Empty))))
                    {
                        if (fdEvent.FrasStatus == "RR")
                            fdEvent.FrasStatus = "UR";
                        else
                            fdEvent.FrasStatus = "RU";
                    }
                }

                fdEvent.FlagDay = model.FlagDay;
                fdEvent.FlagTimeFrom = flagTimeFrom;
                fdEvent.FlagTimeTo = flagTimeTo;
                fdEvent.PermitNum = model.PermitNo;
                fdEvent.PermitIssueDate = model.PermitIssueDate;
                fdEvent.PermitAcknowledgementReceiveDate = model.PermitAcknowledgementReceiveDate;
                fdEvent.TWR = model.EventTWR;
                fdEvent.TwrDistrict = model.District;
                fdEvent.BagColour = model.BagColour;
                fdEvent.FlagColour = model.FlagColour;
                fdEvent.CollectionMethod = model.MethodOfCollection != null ? string.Join(",", model.MethodOfCollection) : string.Empty;
                fdEvent.PermitRevokeIndicator = model.PermitRevokeIndicator;
                fdEvent.Remarks = model.EventRemark != null ? string.Join(",", model.EventRemark) : string.Empty;
                fdEvent.RowVersion = model.EventRowVersion;

                var fdMaster = _flagDayService.GetFDById(fdEvent.FdMaster.FdMasterId);
                fdMaster.SubmissionDueDate = flagDay.AddDays(Convert.ToInt32(fdDueDateParam.Value));
                using (_unitOfWork.BeginTransaction())
                {
                    _fdEventService.Update(fdEvent);
                    _flagDayService.UpdateFdMaster(fdMaster);
                    _unitOfWork.Commit();
                }

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                    Data = new
                    {
                        RowVersion = fdMaster.RowVersion,
                    }
                }, JsonRequestBehavior.DenyGet);
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/fd/event/{fdMasterId}/createFdEvent", Name = "CreateFdEvent")]
        public JsonResult CreateFdEvent([CustomizeValidator(RuleSet = "default,CreateFdEvent")] FlagDayViewModel model, int fdMasterId)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            if (model != null)
            {
                var fdMaster = _flagDayService.GetFDById(fdMasterId);
                var permitNo = _flagDayService.genPermitNo(fdMaster.FdYear, model.EventTWR);
                var fdEvent = new FdEvent();
                //pspEvent = _pspEventService.GetPspEventById(model.PspEventViewModel.PspEventId);
                fdEvent.FdMaster = fdMaster;
                fdEvent.FlagDay = model.FlagDay;

                //Validation rule has guarded the FlagDay must be filled
                DateTime flagDay = model.FlagDay.Value;
                fdEvent.FlagTimeFrom = new DateTime(
                                            flagDay.Year,
                                            flagDay.Month,
                                            flagDay.Day,
                                            Convert.ToInt32(model.FlagTimeFrom.Substring(0, 2)),
                                            Convert.ToInt32(model.FlagTimeFrom.Substring(2, 2)), 0);
                fdEvent.FlagTimeTo = new DateTime(
                                            flagDay.Year,
                                            flagDay.Month,
                                            flagDay.Day,
                                            Convert.ToInt32(model.FlagTimeTo.Substring(0, 2)),
                                            Convert.ToInt32(model.FlagTimeTo.Substring(2, 2)), 0);
                fdEvent.PermitNum = permitNo;
                fdEvent.PermitIssueDate = model.PermitIssueDate;
                fdEvent.PermitAcknowledgementReceiveDate = model.PermitAcknowledgementReceiveDate;
                fdEvent.BagColour = model.BagColour;
                fdEvent.FlagColour = model.FlagColour;
                fdEvent.TWR = model.EventTWR;
                fdEvent.TwrDistrict = model.District;
                fdEvent.CollectionMethod = model.MethodOfCollection != null ? string.Join(",", model.MethodOfCollection) : string.Empty;
                fdEvent.PermitRevokeIndicator = model.PermitRevokeIndicator;
                fdEvent.Remarks = model.EventRemark != null ? string.Join(",", model.EventRemark) : string.Empty;
                fdEvent.FrasStatus = "RC";

                //fdEvent.RowVersion = model.FlagDayEventViewModel.RowVersion;
                var fdDueDateParam = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_DUEDATE);
                fdMaster.SubmissionDueDate = flagDay.AddDays(Convert.ToInt32(fdDueDateParam.Value));
                using (_unitOfWork.BeginTransaction())
                {
                    _fdEventService.Create(fdEvent);
                    _flagDayService.UpdateFdMaster(fdMaster);
                    _unitOfWork.Commit();
                }

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                    Data = new
                    {
                        RowVersion = fdMaster.RowVersion,
                    }
                }, JsonRequestBehavior.DenyGet);
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet, Route("~/api/fd/event/{fdEventId}/deleteFdEvent", Name = "DeleteFdEvent")]
        public JsonResult DeleteFdEvent(int fdEventId)
        {
            Ensure.Argument.NotNull(fdEventId);
            FdEvent fdEvent = _fdEventService.GetFdEventById(fdEventId);
            if (fdEvent != null && fdEvent.FrasCharityEventId.IsNullOrEmpty())
            {
                using (_unitOfWork.BeginTransaction())
                {
                    _fdEventService.Delete(fdEvent);
                    _unitOfWork.Commit();
                }
            }
            else
            {
                if (fdEvent == null)
                    return Json(new JsonResponse(false)
                    {
                        Message = _messageService.GetMessage(SystemMessage.Error.NotFound),
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new JsonResponse(false)
                    {
                        Message = _messageService.GetMessage(SystemMessage.Error.OGCIO.AlreadySubmitted),
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
            }, JsonRequestBehavior.AllowGet);
        }

        //decode, from code to desc
        public string decodeByStringArr(string targetString, IDictionary<string, string> arrDecode)
        {
            if (string.IsNullOrEmpty(targetString))
            {
                return string.Empty;
            }

            var arrColl = targetString.Split(',').ToList();

            for (int i = 0; i < arrColl.Count(); i++)
            {
                arrColl[i] = arrDecode[arrColl[i]];
            }
            return arrColl.Count > 0 ? string.Join(",", arrColl) : string.Empty;
        }

        #endregion Event

        #region Approve

        [PspsAuthorize(Allow.FdApprove)]
        public ActionResult Approve()
        {
            FlagDayApproveViewModel model = new FlagDayApproveViewModel();
            ViewData["SUBMIT_GOV_HK"] = _messageService.GetMessage(SystemMessage.Info.GovHK);

            return View(model);
        }

        [PspsAuthorize(Allow.FdApprove)]
        [HttpGet, Route("~/api/FdApprove/listFdEventSummary", Name = "ListFdEventSummary")]
        public JsonResult ListFdEventSummary(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            var summaryDetails = _flagDayService.GetApproveEveSummaryPage();

            var gridResult = new GridResult
            {
                Data = (
                        from u in summaryDetails
                        select new FdEventApproveSummaryDto
                        {
                            YearOfFlagDay = u.YearOfFlagDay,
                            ApplicationApprovedNum = u.ApplicationApprovedNum,
                            ApplicationReceivedNum = u.ApplicationReceivedNum,
                            ApplicationWithdrawNum = u.ApplicationWithdrawNum,
                            PostOfApprover = u.PostOfApprover,
                            ApproverId = u.ApproverId,
                            ApprovalDate = u.ApprovalDate,
                            SummaryRemarks = u.SummaryRemarks,
                            Approved = u.Approved
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdApprove)]
        [HttpGet, Route("~/api/FdApprove/listFdApplication/{fdYear}", Name = "ListFdApplication")]
        public JsonResult ListFdApplication(GridSettings grid, string fdYear)
        {
            Ensure.Argument.NotNull(grid);

            var approvalDetails = _flagDayService.GetFdApplicationListPage(grid, fdYear);

            var gridResult = new GridResult
            {
                TotalPages = approvalDetails.TotalPages,
                CurrentPageIndex = approvalDetails.CurrentPageIndex,
                TotalCount = approvalDetails.TotalCount,
                Data = (
                        from u in approvalDetails
                        select new FdApplicationListDto
                        {
                            FdMasterId = u.FdMasterId,
                            FdRef = u.FdRef,
                            OrgName = u.OrgName,
                            ApplicationResult = u.ApplicationResult,
                            FlagDay = u.FlagDay,
                            TWR = u.TWR,
                            TwrDistrict = u.TwrDistrict,
                            PermitNo = u.PermitNo,
                            PermitRevokeIndicator = u.PermitRevokeIndicator,
                            ApproveRemarks = decodeByStringArr(u.ApproveRemarks, fdEventRemarks),
                            FrasResponse = u.FrasResponse,
                            Approve = u.Approve,
                            FdEventId = u.FdEventId,
                            RowVersion = u.RowVersion
                        }
                       ).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdApprove)]
        [HttpPost, Route("~/api/FdApprove/approveFdEvents", Name = "ApproveFdEvents")]
        [ValidateAntiForgeryToken]
        public JsonResult ApproveFdEvents([CustomizeValidator(RuleSet = "CreateFdApprovHist")] FlagDayApproveViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            FdApprovalHistory fdApprovHist = new FdApprovalHistory();
            fdApprovHist.FdYear = model.YearofFlagDay;
            fdApprovHist.ApproverPostId = model.PostOfApprover;
            fdApprovHist.ApproverUserId = model.ApproverId;
            fdApprovHist.ApprovalDate = model.ApprovalDate;
            fdApprovHist.ApprovalRemark = model.SummaryRemarks;

            //_fdApprovalHistoryService
            IList<FdApplicationListDto> eventDetails = _flagDayService.GetFdApplicationList(model.YearofFlagDay);
            string message;
            int success = 0;

            using (_unitOfWork.BeginTransaction())
            {
                foreach (FdApplicationListDto @event in eventDetails)
                {
                    FdEvent fdEvent = _fdEventService.GetFdEventById(@event.FdEventId);
                    message = string.Empty;

                    if (fdEvent.FrasCharityEventId.IsNullOrEmpty() && (!fdEvent.PermitRevokeIndicator.HasValue || fdEvent.PermitRevokeIndicator == false))
                    {
                        if (_fdEventService.CreateFRAS(fdEvent, out message, model.ApprovalDate))
                        {
                            success++;
                            fdEvent.FrasCharityEventId = message;
                            fdEvent.FrasResponse = "Success";
                            fdEvent.FrasStatus = "C";
                        }
                        else
                        {
                            fdEvent.FrasResponse = message;
                        }

                        _fdEventService.Update(fdEvent);
                    }
                }

                _fdApprovalHistoryService.CreateFdApprovalHistory(fdApprovHist);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated) + " with {0}/{1} submitted to FRAS".FormatWith(success, eventDetails.Count),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FdApprove)]
        [HttpPost, Route("~/api/fdapprove/submitFRAS", Name = "FDSubmitFRAS")]
        [ValidateAntiForgeryToken]
        public JsonResult FDSubmitFRAS(FdApplicationListDto model)
        {
            Ensure.Argument.NotNull(model);
            FdEvent fdEvent = _fdEventService.GetFdEventById(model.FdEventId);

            if (fdEvent == null || !fdEvent.RowVersion.SequenceEqual(model.RowVersion))
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.ConcurrentUpdate),
                }, JsonRequestBehavior.DenyGet);
            }

            using (_unitOfWork.BeginTransaction())
            {
                string message = string.Empty;
                bool fras = false;

                switch (fdEvent.FrasStatus)
                {
                    case "RC":
                        fras = _fdEventService.CreateFRAS(fdEvent, out message);
                        break;

                    case "RU":
                        fras = _fdEventService.UpdateFRAS(fdEvent, out message);
                        break;

                    case "RR":
                    case "UR":
                        fras = _fdEventService.DeleteFRAS(fdEvent, out message);
                        break;
                }

                if (!fras)
                {
                    fdEvent.FrasResponse = message;
                    _fdEventService.Update(fdEvent);
                    _unitOfWork.Commit();

                    return Json(new JsonResponse(false)
                    {
                        Message = message,
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    if (fdEvent.FrasStatus == "RC")
                        fdEvent.FrasCharityEventId = message;
                    else if (fdEvent.FrasStatus == "RR" || fdEvent.FrasStatus == "UR")
                        fdEvent.FlagDay = null;

                    fdEvent.FrasResponse = "Success";
                    fdEvent.FrasStatus = "C";
                }

                _fdEventService.Update(fdEvent);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion Approve

        #region Attachment

        [RuleSetForClientSideMessagesAttribute("default")]
        public PartialViewResult RenderFdAttachmentModal()
        {
            FlagDayViewModel model = new FlagDayViewModel();
            return PartialView("_EditFlagDayAttachmentModal", model.FlagDayAttachmentViewModel);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagDay/{fdMasterId:int}/listFdAttachment", Name = "ListFdAttachment")]
        public JsonResult ListFdAttachment(int fdMasterId, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(fdMasterId);

            grid.AddDefaultRule(new Rule()
            {
                field = "FdMaster.FdMasterId",
                data = fdMasterId + "",
                op = WhereOperation.Equal.ToEnumValue()
            });
            var fdAttachments = _fdAttachmentService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = fdAttachments.TotalPages,
                CurrentPageIndex = fdAttachments.CurrentPageIndex,
                TotalCount = fdAttachments.TotalCount,
                Data = (from c in fdAttachments
                        select new
                        {
                            FdAttachmentId = c.FdAttachmentId,
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

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagDay/{fdMasterId:int}/CreateFdAttachment", Name = "CreateFdAttachment")]
        public JsonResult CreateFdAttachment(int fdMasterId, [CustomizeValidator(RuleSet = "default,CreateFdAttachment")] FlagDayViewModel model)
        {
            // Get the FdMaster record by given the ID
            Ensure.Argument.NotNull(fdMasterId);
            var fdMaster = _flagDayService.GetFDById(model.FlagDayAttachmentViewModel.FdMasterId);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_ATTACHMENT_PATH);

            // Rename the file by adding current time
            var fileName = model.FlagDayAttachmentViewModel.AttachmentFile == null ? "" : Path.GetFileName(model.FlagDayAttachmentViewModel.AttachmentFile.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path by adding the FdMasterId folder     ( [Root Folder of Attachment] \ [FdMasterId Folder] )
            string rootPath = Path.Combine(attachmentPath.Value, fdMaster.FdMasterId.ToString());
            // Form the Relative Path for storing in DB         ( [FdMasterId Folder] \ [File Name] )
            // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [FdMasterId Folder] \ [File Name] )
            string relativePath = Path.Combine(fdMaster.FdMasterId.ToString(), generatedFileName);
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new FdAttachment row and fill the value
            var fdAttachment = new FdAttachment();
            fdAttachment.FdMaster = fdMaster;
            fdAttachment.FileDescription = model.FlagDayAttachmentViewModel.FileDescription;
            fdAttachment.FileLocation = relativePath;
            fdAttachment.FileName = model.FlagDayAttachmentViewModel.FileName;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.FlagDayAttachmentViewModel.AttachmentFile.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                _fdAttachmentService.Create(fdAttachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/flagDay/{fdAttachmentId:int}/UpdateFdAttachment", Name = "UpdateFdAttachment")]
        public JsonResult UpdateFdAttachment([CustomizeValidator(RuleSet = "default,UpdateFdAttachment")] FlagDayViewModel model, int fdAttachmentId)
        {
            Ensure.Argument.NotNull(model.FlagDayAttachmentViewModel);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // // Commented by Pong at 2015.04.16 10:44
            // //   : New attachment file is not mandatory for attachment updating
            //if (model.FlagDayAttachmentViewModel.AttachmentFile != null)
            //{
            // Get the FdAttachment record by given the ID
            var fdAttachment = _fdAttachmentService.GetById(Convert.ToInt32(fdAttachmentId));
            Ensure.NotNull(fdAttachment, "No FdAttachment found with the specified id");

            // Fill the update values
            fdAttachment.FileName = model.FlagDayAttachmentViewModel.FileName;
            fdAttachment.FileDescription = model.FlagDayAttachmentViewModel.FileDescription;

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_ATTACHMENT_PATH);

            using (_unitOfWork.BeginTransaction())
            {
                // If new file need to be upload
                if (model.FlagDayAttachmentViewModel.AttachmentFile != null)
                {
                    // Reforming the file name by adding current time
                    var fileName = Path.GetFileName(model.FlagDayAttachmentViewModel.AttachmentFile.FileName);
                    var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    string generatedFileName = string.Format("{0}-{1}", time, fileName);

                    // Set the root path by adding the FdMasterId folder     ( [Root Folder of Attachment] \ [FdMasterId Folder] )
                    string rootPath = Path.Combine(attachmentPath.Value, fdAttachment.FdMaster.FdMasterId.ToString());
                    // Form the Relative Path for storing in DB         ( [FdMasterId Folder] \ [File Name] )
                    // and Absolute Path for actually saving the file   ( [Root Folder of Attachment] \ [FdMasterId Folder] \ [File Name] )
                    string relativePath = Path.Combine(fdAttachment.FdMaster.FdMasterId.ToString(), generatedFileName);
                    string absolutePath = Path.Combine(rootPath, generatedFileName);

                    // Save the new file
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.FlagDayAttachmentViewModel.AttachmentFile.SaveAs(absolutePath);
                    }

                    // Form the path of the old file
                    string absolutePathOfOldFile = Path.Combine(attachmentPath.Value, fdAttachment.FileLocation);

                    // Delete the old file
                    if (System.IO.File.Exists(absolutePathOfOldFile))
                    {
                        System.IO.File.Delete(absolutePathOfOldFile);
                    }

                    // Replace with the new path
                    fdAttachment.FileLocation = relativePath;
                }

                // Update DB record and commit
                _fdAttachmentService.Update(fdAttachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, JsonRequestBehavior.DenyGet);

            // // Commented by Pong at 2015.04.16 10:44
            // //   : New attachment file is not mandatory for attachment updating
            //}
            //else
            //{
            //    return Json(new JsonResponse(true)
            //    {
            //        Message = _messageService.GetMessage("", "Attachment Not Found."),
            //    }, JsonRequestBehavior.DenyGet);
            //}
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagDay/{fdAttachmentId:int}/getFdAttachmentDetail", Name = "GetFdAttachmentDetail")]
        public JsonResult GetFdAttachmentDetail(int fdAttachmentId)
        {
            var fdAttachment = _fdAttachmentService.GetById(fdAttachmentId);
            Ensure.NotNull(fdAttachment, "No FdAttachment found with the specified id");

            var filePath = fdAttachment.FileLocation;
            //HttpPostedFileBase httpFile;
            //httpFile.FileName = fdAttachment.FileName;

            var model = new FlagDayViewModel();
            model.FlagDayAttachmentViewModel.FdAttachmentId = fdAttachment.FdAttachmentId;
            model.FlagDayAttachmentViewModel.FdMasterId = fdAttachment.FdMaster.FdMasterId;
            model.FlagDayAttachmentViewModel.FileName = fdAttachment.FileName;
            model.FlagDayAttachmentViewModel.FileDescription = fdAttachment.FileDescription;
            model.FlagDayAttachmentViewModel.RowVersion = fdAttachment.RowVersion;

            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpGet, Route("~/api/flagDay/{fdAttachmentId:int}/downloadFdAttachment", Name = "DownloadFdAttachment")]
        public FileResult DownloadFdAttachment(int fdAttachmentId)
        {
            // Get the FdAttachment record by given the ID
            var fdAttachment = _fdAttachmentService.GetById(fdAttachmentId);
            Ensure.NotNull(fdAttachment, "No FdAttachment found with the specified id");

            // Get the root path of the Attachment from DB
            // and combine with the FileLocation to get the Absolute Path that the file actually stored at
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_ATTACHMENT_PATH);
            string rootPath = attachmentPath.Value;
            var absolutePath = Path.Combine(rootPath, fdAttachment.FileLocation);
            // ( [Root Folder of Attachment] \ [FdMasterId Folder] \ [File Name] )

            // Set the file name for saving
            string fileName = fdAttachment.FileName + Path.GetExtension(Path.GetFileName(fdAttachment.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        [PspsAuthorize(Allow.FdMaster)]
        [HttpPost, Route("~/api/complaint/{fdAttachmentId:int}/deleteFdAttachment", Name = "DeleteFdAttachment")]
        public JsonResult DeleteFdAttachment(int fdAttachmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the FdAttachment record by given the ID
            var fdAttachment = _fdAttachmentService.GetById(fdAttachmentId);
            Ensure.NotNull(fdAttachment, "No FdAttachment found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                // Get the root path of the Attachment file from DB
                // and combine with the FileLocation to get the Absolute Path that the file actually stored at
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_ATTACHMENT_PATH);
                var absolutePath = Path.Combine(attachmentPath.Value, fdAttachment.FileLocation);
                // ( [Root Folder of Attachment] \ [FdMasterId Folder] \ [File Name] )

                // Delete the record in DB (set IsDeleted flag)
                _fdAttachmentService.Delete(fdAttachment);

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

        #endregion Attachment
    }
}