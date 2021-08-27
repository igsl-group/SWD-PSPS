using DocxGenerator.Library;
using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Organisation;
using Psps.Models.Dto.Reports;
using Psps.Services.ComplaintMasters;
using Psps.Services.Disaster;
using Psps.Services.FlagDays;
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
using Psps.Web.ViewModels.Complaint;
using Psps.Web.ViewModels.FlagDay;
using Psps.Web.ViewModels.Organisation;
using Psps.Web.ViewModels.PSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Organisation"), Route("{action=index}")]
    public class OrganisationController : BaseController
    {
        #region "Init & Fields"

        private readonly static string OrgControllerSearchSessionName = "OrgControllerSearch";
        private readonly static string OrgRefGuideSearchSessionName = "OrgRefGuideSearchSession";
        private readonly static string PSPACSummarySearchSessionName = "PSPACSummarySearch";
        private readonly static string FDACSummarySearchSessionName = "FDACSummarySearch";
        private readonly string DATE_FORMAT = "dd/MM/yyyy";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IOrganisationService _organisationService;
        private readonly ILookupService _lookupService;
        private readonly IPspService _PSPService;
        private readonly IFlagDayService _flagDayService;
        private readonly IOrganisationDocService _organizationDocService;
        private readonly IParameterService _parameterService;
        private readonly IOrgNameChangeHistoryService _orgNameChangeHistoryService;
        private readonly IOrgAttachmentService _orgAttachmentService;
        private readonly IPspEventService _pspEventService;
        private readonly IFdEventService _fdEventService;
        private readonly IComplaintMasterService _complaintMasterService;
        private readonly IOrgRefGuidePromulgationService _orgRefGuidePromulgationService;
        private readonly IReportService _reportService;
        private readonly IOrgProvisionNotAdoptService _orgProvisionNotAdoptService;
        private readonly IFdApprovalHistoryService _fdApprovalHistoryService;
        private readonly IWithholdingHistoryService _withholdingHistoryService;
        private readonly IOrgEditLatestPspFdViewRepository _orgEditLatestPspFdViewRepository;
        private readonly IOrgAfsTrViewService _orgAfsTrViewService;
        private readonly IFlagDayListService _flagDayListService;
        private readonly IPostsInRolesService _postsInRolesService;
        private readonly IDisasterMasterService _disasterMasterService;

        private readonly IDictionary<string, string> fundUseds;
        private readonly IDictionary<int, string> disasterNames;

        public OrganisationController(IUnitOfWork unitOfWork, IMessageService messageService,
            IOrganisationService _organisationService, ILookupService lookupService, IPspService PSPService,
            IFlagDayService flagDayService, IOrganisationDocService organizationDocService,
            IParameterService parameterService, IOrgNameChangeHistoryService orgNameChangeHistoryService,
            IOrgAttachmentService orgAttachmentService, IPspEventService pspEventService, IFdEventService fdEventService,
             IComplaintMasterService complaintMasterService, IOrgRefGuidePromulgationService orgRefGuidePromulgationService,
            IReportService reportService, IOrgProvisionNotAdoptService orgProvisionNotAdoptService
            , IFdApprovalHistoryService fdApprovalHistoryService, IWithholdingHistoryService withholdingHistoryService,
            IOrgEditLatestPspFdViewRepository orgEditLatestPspFdViewRepository, IOrgAfsTrViewService orgAfsTrViewService,
            IFlagDayListService flagDayListService, IPostsInRolesService postsInRolesService, IDisasterMasterService disasterMasterService)
        {
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._organisationService = _organisationService;
            this._lookupService = lookupService;
            this._PSPService = PSPService;
            this._flagDayService = flagDayService;
            this._organizationDocService = organizationDocService;
            this._parameterService = parameterService;
            this._orgNameChangeHistoryService = orgNameChangeHistoryService;
            this._orgAttachmentService = orgAttachmentService;
            this._pspEventService = pspEventService;
            this._fdEventService = fdEventService;
            this._complaintMasterService = complaintMasterService;
            this._orgRefGuidePromulgationService = orgRefGuidePromulgationService;
            this._reportService = reportService;
            this._orgProvisionNotAdoptService = orgProvisionNotAdoptService;
            this._fdApprovalHistoryService = fdApprovalHistoryService;
            this._withholdingHistoryService = withholdingHistoryService;
            this._orgEditLatestPspFdViewRepository = orgEditLatestPspFdViewRepository;
            this._orgAfsTrViewService = orgAfsTrViewService;
            this._flagDayListService = flagDayListService;
            this._postsInRolesService = postsInRolesService;
            this.fundUseds = _lookupService.getAllLkpInCodec(LookupType.FundUsed);
            this._disasterMasterService = disasterMasterService;

            if (fundUseds.Count == 0) fundUseds.Add("", "");
            this.disasterNames = this._disasterMasterService.GetAllDisasterMasterForDropdown();
        }

        #endregion "Init & Fields"

        #region Edit

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/Organisation/{code}/Edit", Name = "EditOrgMaster")]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult EditOrgMaster(string code, string ReturnUrl)
        {
            Ensure.Argument.NotNullOrEmpty(code);
            OrgMaster orgMaster = _organisationService.GetOrgById(Convert.ToInt32(code));
            Ensure.Argument.NotNull(orgMaster);

            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(orgMaster.OrgId);
            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(orgMaster.OrgId);

            var model = new OrganisationViewModel
            {
                OrgMasterId = orgMaster.OrgId.ToString(),
 
                PspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "",
                PspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "",
                PspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "",

                FdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "",
                FdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "",
                FdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "",

                WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate,
                WithholdingEndDate = WithHoldingDate.WithholdingEndDate                
            };

            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                var currentUrl = this.HttpContext.Request.Url;
                var prePage = currentUrl.Scheme + "://" + currentUrl.Authority + ReturnUrl;
                model.PrePage = prePage;
            }

            //if (!String.IsNullOrEmpty(PreRecordId))
            //{
            //    model.PreRecordId = PreRecordId;
            //}

            initOrgViewModel(model, 2);
            model.RowVersion = orgMaster.RowVersion;

            bool isProcessingOfficer = false;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;
            var list = _postsInRolesService.GetByPostId(postId);
            foreach (var postsInRoles in list)
            {
                if (postsInRoles.RoleId.Contains("Processing Officer"))
                {
                    isProcessingOfficer = true;
                    break;
                }
            }
            model.isProcessingOfficer = isProcessingOfficer;

            ViewData["OrgDisabledOrWithheld"] = _messageService.GetMessage(SystemMessage.Warning.Psp.OrgDisabledOrWithheld);

            return View("New", model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "CreateOrgAttachment", "UpdateOrgAttachment")]
        public PartialViewResult RenderOrgAttachmentModal()
        {
            OrganisationViewModel model = new OrganisationViewModel();
            return PartialView("_OrgAttachmentModal", model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Update")]
        public PartialViewResult RenderOrgNameChangeHistoryModal()
        {
            OrgNameChangeHistoryViewModel model = new OrgNameChangeHistoryViewModel();
            return PartialView("_OrgNameChangeHistoryModal", model);
        }

        #endregion Edit

        public static int n = 0;

        #region "Search"

        [PspsAuthorize(Allow.OrgMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        [HttpGet, Route("Search", Name = "OrgSearch")]
        public ActionResult Search()
        {
            OrganisationViewModel model = new OrganisationViewModel();
            model.isFirstSearch = true;

            this.HttpContext.Session[OrgControllerSearchSessionName] = null;

            initOrgViewModel(model, 0);

            return View(model);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        [HttpGet, Route("~/Organisation/ReturnSearchOrg", Name = "ReturnSearchOrg")]
        public ActionResult ReturnSearchOrg()
        {
            OrganisationViewModel model = new OrganisationViewModel();

            if (this.HttpContext.Session[OrgControllerSearchSessionName] != null)
            {
                model = ((OrganisationViewModel)(this.HttpContext.Session[OrgControllerSearchSessionName]));
                model.isFirstSearch = false;
            }

            initOrgViewModel(model, 0);

            return View("Search", model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Update")]
        public PartialViewResult RenderOrgTemplateModal()
        {
            OrganisationDocViewModel model = new OrganisationDocViewModel();
            return PartialView("_OrgTemplateModal", model);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/Organisation/templatetab/list", Name = "ListOrgTemplateTab")]
        public JsonResult ListOrgTemplateTab(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var template = _organizationDocService.GetPage(grid);

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
                            OrgDocId = l.OrgDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("{id}/template/{orgDocId}/generate", Name = "GenerateOrgTemplate")]
        public ActionResult GenerateOrgTemplate(int orgDocId, int id)
        {
            Ensure.Argument.NotNull(orgDocId);
            Ensure.Argument.NotNull(id);

            var template = _organizationDocService.GetOrgDocById(orgDocId);
            var org = _organizationDocService.GetOrgDocViewById(id);
            var sysParam = _parameterService.GetParameterByCode("OrganisationTemplatePath");
            var inputFilePath = Path.Combine(@sysParam.Value, template.FileLocation);

            if (!System.IO.File.Exists(inputFilePath))
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "Template not found");

            SimpleDocumentGenerator<OrgDocView> docGenerator = new SimpleDocumentGenerator<OrgDocView>(new DocumentGenerationInfo
            {
                DataContext = org,
                TemplateData = System.IO.File.ReadAllBytes(inputFilePath)
            });
            return File(docGenerator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", template.DocName + ".docx");
        }

        [HttpGet, Route("~/api/Organisation/{orgRef}", Name = "GetOrganisation")]
        public JsonResult GetOrganisation(string orgRef)
        {
            Ensure.Argument.NotNullOrEmpty(orgRef);
            var orgMaster = _organisationService.GetOrgByRef(orgRef);

            return GetOrgMaster(orgMaster.OrgId.ToString());
            //return Json(new JsonResponse(true)
            //{
            //    Data = new OrganisationViewModel
            //    {
            //        OrganisationReference = orgMaster.OrgRef,
            //        OrganisationDisabled = orgMaster.DisableIndicator,
            //        OrganisationName = orgMaster.EngOrgName,
            //        OrganisationChiName = orgMaster.ChiOrgName,
            //        OrganisationSimpliChiName = orgMaster.SimpChiOrgName,
            //        EngOrgNameSorting = orgMaster.EngOrgNameSorting,
            //        OtherEngOrgName = orgMaster.OtherEngOrgName,
            //        OtherChiOrgName = orgMaster.OtherChiOrgName,
            //        OtherSimpChiOrgName = orgMaster.OtherSimpChiOrgName,
            //        EngRegisteredAddress1 = orgMaster.EngRegisteredAddress1,
            //        EngRegisteredAddress2 = orgMaster.EngRegisteredAddress2,
            //        EngRegisteredAddress3 = orgMaster.EngRegisteredAddress3,
            //        EngRegisteredAddress4 = orgMaster.EngRegisteredAddress4,
            //        EngRegisteredAddress5 = orgMaster.EngRegisteredAddress5,
            //        ChiRegisteredAddress1 = orgMaster.ChiRegisteredAddress1,
            //        ChiRegisteredAddress2 = orgMaster.ChiRegisteredAddress2,
            //        ChiRegisteredAddress3 = orgMaster.ChiRegisteredAddress3,
            //        ChiRegisteredAddress4 = orgMaster.ChiRegisteredAddress4,
            //        ChiRegisteredAddress5 = orgMaster.ChiRegisteredAddress5,
            //        EngMailingAddress1 = orgMaster.EngMailingAddress1,
            //        EngMailingAddress2 = orgMaster.EngMailingAddress2,
            //        EngMailingAddress3 = orgMaster.EngMailingAddress3,
            //        EngMailingAddress4 = orgMaster.EngMailingAddress4,
            //        EngMailingAddress5 = orgMaster.EngMailingAddress5,
            //        ChiMailingAddress1 = orgMaster.ChiMailingAddress1,
            //        ChiMailingAddress2 = orgMaster.ChiMailingAddress2,
            //        ChiMailingAddress3 = orgMaster.ChiMailingAddress3,
            //        ChiMailingAddress4 = orgMaster.ChiMailingAddress4,
            //        ChiMailingAddress5 = orgMaster.ChiMailingAddress5,
            //        OrganisationWebsite = orgMaster.URL,
            //        TelNum = orgMaster.TelNum,
            //        Fax = orgMaster.FaxNum,
            //        Email = orgMaster.EmailAddress,
            //        ApplicantSaluteId = orgMaster.ApplicantSalute,
            //        ApplicantFirstName = orgMaster.ApplicantFirstName,
            //        ApplicantLastName = orgMaster.ApplicantLastName,
            //        ApplicantChiFirstName = orgMaster.ApplicantChiFirstName,
            //        ApplicantChiLastName = orgMaster.ApplicantChiLastName,
            //        ApplicantPosition = orgMaster.ApplicantPosition,
            //        ApplicantTelNum = orgMaster.ApplicantTelNum,
            //        President = orgMaster.PresidentName,
            //        Secretary = orgMaster.SecretaryName,
            //        Treasurer = orgMaster.TreasurerName,
            //        Objectives = orgMaster.OrgObjective,
            //        Subvented = orgMaster.SubventedIndicator,
            //        Section88 = Convert.ToBoolean(orgMaster.Section88Indicator),
            //        Section88Date = orgMaster.Section88StartDate,
            //        RegistrationType1 = orgMaster.RegistrationType1,
            //        RegistrationOtherName1 = orgMaster.RegistrationOtherName1,
            //        RegistrationDate1 = orgMaster.RegistrationDate1,
            //        RegistrationType2 = orgMaster.RegistrationType2,
            //        RegistrationOtherName2 = orgMaster.RegistrationOtherName2,
            //        RegistrationDate2 = orgMaster.RegistrationDate2,
            //        AddressProofIndicator = Convert.ToBoolean(orgMaster.AddressProofIndicator),
            //        AddressProofDate = orgMaster.AddressProofDate,
            //        MaaConstitutionIndicator = Convert.ToBoolean(orgMaster.MaaConstitutionIndicator),
            //        QualifiedOpinionIndicator = Convert.ToBoolean(orgMaster.QualifiedOpinionIndicator),
            //        OtherSupportDocIndicator = Convert.ToBoolean(orgMaster.OtherSupportDocIndicator),
            //        QualifiedOpinionRemark = orgMaster.QualifiedOpinionRemark,
            //        OtherSupportDocRemark = orgMaster.OtherSupportDocRemark,
            //        OverallRemark = orgMaster.OverallRemark,
            //        withholdingIndicator = this._withholdingHistoryService.GetWithHoldBySysDt(orgMaster.OrgId)
            //    }
            //}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("~/api/Organisation/search", Name = "SearchOrganisation")]
        public JsonResult SearchOrganisation(string searchTerm, int pageSize, int pageNum)
        {
            GridSettings grid = new GridSettings
            {
                PageIndex = pageNum,
                PageSize = pageSize
            };

            grid.AddDefaultRule(new List<Rule> {
                new Rule { data = searchTerm, field = "OrgRef", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "EngOrgName", op = WhereOperation.Contains.ToEnumValue()},
                new Rule { data = searchTerm, field = "ChiOrgName", op = WhereOperation.Contains.ToEnumValue()},
            }, GroupOp.OR);

            var organisations = _organisationService.GetPage(grid);

            return Json(new JsonResponse(true)
            {
                Data = new
                {
                    Total = organisations.TotalCount,
                    Results = (from p in organisations
                               select new
                               {
                                   id = p.OrgRef,
                                   text = p.EngOrgName,
                                   cText = p.ChiOrgName,
                                   section88 = p.Section88Indicator,
                                   disableIndicator = p.DisableIndicator,
                                   withholdingIndicator = this._withholdingHistoryService.GetWithHoldBySysDt(p.OrgId)
                               }).ToArray()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        //Mode - 0: Search, 1: Create???, 2: Edit
        private void initOrgViewModel(OrganisationViewModel model, int Mode)
        {
            model.RegistrationTitles = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);

            var salute = _lookupService.getAllLkpInCodec(LookupType.Salute);
            var chiSalute = _lookupService.getAllLkpInChiCodec(LookupType.Salute);
            model.ApplicantSalutes = salute;
            model.ApplicantChiSalutes = chiSalute;

            if (Mode == 0)
            {
                model.OrganisationStatus = _lookupService.getAllLkpInCodec(LookupType.OrgStatus);
                var YesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);
                model.Subventeds = YesNo;
                model.Sections = YesNo;
                model.AppliedPSPBefores = YesNo;
                model.AppliedFDBefores = YesNo;
                model.AppliedSSAFBefores = YesNo;
                model.ReceivedComplaintBefores = YesNo;
                model.ReceivedEnquiryBefores = YesNo;
                model.PSPIssuedBefores = YesNo;
                model.FDIssuedBefores = YesNo;
                model.SSAFIssuedBefores = YesNo;
                model.WithholdingInds = YesNo;
                model.FlagYears = _flagDayListService.GetAllFlagDayListYearForDropdown();
                //model.OrgReplys = _lookupService.getAllLkpInCodec(LookupType.ORG_REPLY);
            }
            else if (Mode == 2)
            {
                model.ProcessStatus = this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);
                model.ComplaintResults = this._lookupService.getAllLkpInCodec(LookupType.ComplaintResult);
                model.ComplaintSources = _lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
                model.ActivityConcerns = _lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
                model.FdApplicationResults = _lookupService.getAllLkpInCodec(LookupType.FdApplicationResult);
                model.FdGroupings = _lookupService.getAllLkpInCodec(LookupType.FdGrouping);
                model.TWRs = _lookupService.getAllLkpInCodec(LookupType.TWR);
                model.PSPApplicationResults = _lookupService.getAllLkpInCodec(LookupType.PSPApplicationResult);
                model.FollowUpLetterTypes = _lookupService.getAllLkpInCodec(LookupType.FollowUpLetterType);
                model.CheckIndicator = _lookupService.getAllLkpInCodec(LookupType.CheckIndicator);
            }
        }

        #endregion "Search"

        #region "PSPSummary"

        [PspsAuthorize(Allow.PspACSummary)]
        public ActionResult PSPSummary()
        {
            this.HttpContext.Session[PSPACSummarySearchSessionName] = null;

            PSPAccountSummaryViewModel model = new PSPAccountSummaryViewModel();

            initPspSummaryViewModel(model);

            model.isFirstSearch = true;

            return View(model);
        }

        [PspsAuthorize(Allow.PspACSummary)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("~/Organisation/ReturnSearchPSPSummary", Name = "PSPSummarySearchPage")]
        public ActionResult ReturnPSPSummarySearch()
        {
            PSPAccountSummaryViewModel model = new PSPAccountSummaryViewModel();
            var session = this.HttpContext.Session[PSPACSummarySearchSessionName];
            if (session != null)
            {
                model = ((PSPAccountSummaryViewModel)(this.HttpContext.Session[PSPACSummarySearchSessionName]));
                model.isFirstSearch = false;
            }

            initPspSummaryViewModel(model);

            return View("PSPSummary", model);
        }

        [PspsAuthorize(Allow.FdACSummary)]
        public ActionResult FDSummary()
        {
            this.HttpContext.Session[FDACSummarySearchSessionName] = null;

            FDAccountSummaryViewModel model = new FDAccountSummaryViewModel();
            
            initFDSummaryViewModel(model);

            model.isFirstSearch = true;

            ViewData["FD_BENCHMARK"] = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_BENCHMARK).Value;

            return View(model);
        }

        [PspsAuthorize(Allow.FdACSummary)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        [HttpGet, Route("~/Organisation/ReturnSearchFDSummary", Name = "FDSummarySearchPage")]
        public ActionResult ReturnFDSummarySearch()
        {
            FDAccountSummaryViewModel model = new FDAccountSummaryViewModel();
            var session = this.HttpContext.Session[FDACSummarySearchSessionName];
            if (session != null)
            {
                model = ((FDAccountSummaryViewModel)(this.HttpContext.Session[FDACSummarySearchSessionName]));
                model.isFirstSearch = false;
            }

            initFDSummaryViewModel(model);
            ViewData["FD_BENCHMARK"] = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_BENCHMARK).Value;
            return View("FDSummary", model);
        }

        private void initPspSummaryViewModel(PSPAccountSummaryViewModel model)
        {
            model.OrgStatus = _lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            model.SubventedIndicators = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.Registrations = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.Sections = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.Overdues = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.YesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.CheckIndicator = _lookupService.getAllLkpInCodec(LookupType.CheckIndicator);
            //model.CheckIndicator.Add("", "");
            model.CheckIndicator = model.CheckIndicator.OrderBy(x => x.Key == "" ? "0" : x.Key).ToDictionary(k => k.Key, v => v.Value);
            model.EventYears = _PSPService.GetAllEventYearForDropdown();

            // Reform the Dictionary of DisasterNames by adding the Sorting Index in the Key
            // (Because the JQGrid of Advance Search would reorder the list by the Key (selectedValue))
            int intSortingIndex = 0;
            model.DisasterNames =
            disasterNames.ToDictionary(k => (++intSortingIndex).ToString() + "[SortingDelimiter]" + k.Key.ToString(), v => v.Value);
        }

        private void initFDSummaryViewModel(FDAccountSummaryViewModel model)
        {
            model.OrgStatus = _lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            model.SubventedIndicators = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.Registrations = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.Sections = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.Overdues = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.YesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.FlagDayYears = _flagDayListService.GetAllFlagDayListYearForDropdown();
            model.TWRs = _lookupService.getAllLkpInCodec(LookupType.TWR);
        }

        #endregion "PSPSummary"

        #region ListOrgMaster

        //[PspsAuthorize(Allow.ListLegalAdviceMaster)]
        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/listOrgMaster", Name = "ListOrgMaster")]
        public JsonResult ListOrgMaster(GridSettings grid, OrganisationViewModel model)
        {
            this.HttpContext.Session[OrgControllerSearchSessionName] = model;

            grid = GetSearchGrid(grid, model);

            var org = _organisationService.GetPageByOrgMasterSearchView(grid, model.WithholdingInd, model.ReceivedComplaintBeforeId, model.ReceivedEnquiryBeforeId, 
                                                                        model.AppliedPSPBeforeId, model.FromPspApplicationDate, model.ToPspApplicationDate, 
                                                                        model.AppliedFDBeforeId, model.AppliedFDBeforeFdYear, 
                                                                        model.AppliedSSAFBeforeId, model.FromSSAFApplicationDate, model.ToSSAFApplicationDate,
                                                                        model.PSPIssuedBeforeId, model.FromPspPermitIssueDate, model.ToPspPermitIssueDate, 
                                                                        model.FDIssuedBeforeId, model.FdIssuedBeforeFdYear,
                                                                        model.SSAFIssuedBeforeId, model.FromSSAFPermitIssueDate, model.ToSSAFPermitIssueDate);

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
                            EngChiOrgNameSorting = p.EngOrgNameSorting + "<br>" + p.ChiOrgName,
                            EngOrgNameSorting = p.EngOrgNameSorting,
                            EngOrgName = p.EngOrgName,
                            ChiOrgName = p.ChiOrgName,
                            SubventedIndicator = p.SubventedIndicator,
                            Section88Indicator = p.Section88Indicator,
                            RegistrationType = (!String.IsNullOrEmpty(p.RegistrationType1) ? _lookupService.GetDescription(LookupType.OrgRegistrationType, p.RegistrationType1) : "")
                                                + (!String.IsNullOrEmpty(p.RegistrationType2) ? "\n" + _lookupService.GetDescription(LookupType.OrgRegistrationType, p.RegistrationType2) : ""),
                            PSPIssuedNum = p.PSPIssuedNum,
                            FDPermitIssuedNum = p.FDPermitIssuedNum,
                            SSAFPermitIssuedNum = p.SSAFPermitIssuedNum,
                            ComplaintReceivedNum = p.ComplaintReceivedNum,
                            EngMailingAddress = (!String.IsNullOrEmpty(p.EngMailingAddress1) ? p.EngMailingAddress1 + "<br>" : "") + (!String.IsNullOrEmpty(p.EngMailingAddress2) ? p.EngMailingAddress2 + "<br>" : "") +
                                                (!String.IsNullOrEmpty(p.EngMailingAddress3) ? p.EngMailingAddress3 + "<br>" : "") + (!String.IsNullOrEmpty(p.EngMailingAddress4) ? p.EngMailingAddress4 + "<br>" : "") +
                                                (!String.IsNullOrEmpty(p.EngMailingAddress5) ? p.EngMailingAddress5 : ""),
                            ChiMailingAddress = p.ChiMailingAddress1 + p.ChiMailingAddress2 + p.ChiMailingAddress3 + p.ChiMailingAddress4 + p.ChiMailingAddress5,
                            ContactPerson = p.ContactPerson,
                            ContactPersonEmail = p.EmailAddress,
                            OrgEmailAddress = p.EmailAddress,
                            WithHoldingInd = p.WithholdingInd
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ListOrgMaster

        #region GetSearchGrid

        private GridSettings GetSearchGrid(GridSettings grid, OrganisationViewModel model)
        {
            if (model.OrganisationReference != "" && model.OrganisationReference != null)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OrgRef",
                    data = model.OrganisationReference,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.OrganisationName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "EngOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "OrgNameChangeHistory>EngOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "OrgNameChangeHistory>ChiOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.TelephoneNo))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "TelNum",
                    data = model.TelephoneNo,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.Address))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "EngRegisteredAddress1",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "EngRegisteredAddress2",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "EngRegisteredAddress3",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "EngRegisteredAddress4",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "EngRegisteredAddress5",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiRegisteredAddress1",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiRegisteredAddress2",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiRegisteredAddress3",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiRegisteredAddress4",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiRegisteredAddress5",
                        data = model.Address,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.OrganisationStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "DisableIndicator",
                    data = model.OrganisationStatusId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.SubventedId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SubventedIndicator",
                    data = model.SubventedId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.NameofContactPerson))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "ApplicantFirstName",
                        data = model.NameofContactPerson,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ApplicantLastName",
                        data = model.NameofContactPerson,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ApplicantChiFirstName",
                        data = model.NameofContactPerson,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ApplicantChiLastName",
                        data = model.NameofContactPerson,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.PrincipalActivities))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OrgObjective",
                    data = model.PrincipalActivities,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.RegistrationTitleId))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationType1",
                        data = model.RegistrationTitleId,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationType2",
                        data = model.RegistrationTitleId,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.Registration))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationOtherName1",
                        data = model.Registration,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationOtherName2",
                        data = model.Registration,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.SectionId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Section88Indicator",
                    data = model.SectionId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.AppliedPSPBeforeId) && !model.FromPspApplicationDate.HasValue && !model.ToPspApplicationDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedPSPBefore",
                    data = model.AppliedPSPBeforeId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.AppliedFDBeforeId) && (model.AppliedFDBeforeFdYear == null || model.AppliedFDBeforeFdYear.Count == 0))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedFDBefore",
                    data = model.AppliedFDBeforeId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.AppliedSSAFBeforeId) && !model.FromSSAFApplicationDate.HasValue && !model.ToSSAFApplicationDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedSSAFBefore",
                    data = model.AppliedSSAFBeforeId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.PSPIssuedBeforeId) && !model.FromPspPermitIssueDate.HasValue && !model.ToPspPermitIssueDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PSPIssuedNum",
                    data = "0",
                    op = model.PSPIssuedBeforeId.Equals("1") ? WhereOperation.GreaterThan.ToEnumValue() : WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.FDIssuedBeforeId) && (model.FdIssuedBeforeFdYear == null || model.FdIssuedBeforeFdYear.Count == 0))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FDPermitIssuedNum",
                    data = "0",
                    op = model.FDIssuedBeforeId.Equals("1") ? WhereOperation.GreaterThan.ToEnumValue() : WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SSAFIssuedBeforeId) && !model.FromSSAFPermitIssueDate.HasValue && !model.ToSSAFPermitIssueDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SSAFPermitIssuedNum",
                    data = "0",
                    op = model.SSAFIssuedBeforeId.Equals("1") ? WhereOperation.GreaterThan.ToEnumValue() : WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.ReceivedComplaintBeforeId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ReceivedComplaintEnquiryBefore",
                    data = model.ReceivedComplaintBeforeId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.ReceivedEnquiryBeforeId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ReceivedComplaintEnquiryBefore",
                    data = model.ReceivedEnquiryBeforeId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.WithholdingInd))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingInd",
                    data = model.WithholdingInd == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            return grid;
        }

        #endregion GetSearchGrid

        #region ExportOrgMaster

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/exportOrgMaster", Name = "ExportOrgMaster")]
        public JsonResult ExportOrgMaster(ExportSettings exportSettings)
        {
            OrganisationViewModel model = ((OrganisationViewModel)(this.HttpContext.Session[OrgControllerSearchSessionName]));
            exportSettings.GridSettings = GetSearchGrid(exportSettings.GridSettings, model);

            var list = _organisationService.GetPageByOrgMasterSearchView(exportSettings.GridSettings, model.WithholdingInd, model.ReceivedComplaintBeforeId, model.ReceivedEnquiryBeforeId, 
                                                                         model.AppliedPSPBeforeId, model.FromPspApplicationDate, model.ToPspApplicationDate,                                                                         
                                                                         model.AppliedFDBeforeId, model.AppliedFDBeforeFdYear,
                                                                         model.AppliedSSAFBeforeId, model.FromSSAFApplicationDate, model.ToSSAFApplicationDate,
                                                                         model.PSPIssuedBeforeId, model.FromPspPermitIssueDate, model.ToPspPermitIssueDate,                                                                         
                                                                         model.FDIssuedBeforeId, model.FdIssuedBeforeFdYear,
                                                                         model.SSAFIssuedBeforeId, model.FromSSAFPermitIssueDate, model.ToSSAFPermitIssueDate);

            var dataList = (from o in list
                            select new
                            {
                                orgRef = o.OrgRef,
                                engChiOrgNameSorting = o.EngOrgNameSorting + Environment.NewLine + o.ChiOrgName,
                                chiOrgName = o.ChiOrgName,
                                engOrgName = o.EngOrgName,
                                engOrgNameSorting = o.EngOrgNameSorting,
                                subventedIndicator = o.SubventedIndicator ? "Yes" : "No",
                                section88Indicator = (o.Section88Indicator.HasValue && o.Section88Indicator.Value) ?  "Yes" : "No",
                                registrationType = (!String.IsNullOrEmpty(o.RegistrationType1) ? _lookupService.GetDescription(LookupType.OrgRegistrationType, o.RegistrationType1) + " \n\r" : "")
                                                    + (!String.IsNullOrEmpty(o.RegistrationType1) ? _lookupService.GetDescription(LookupType.OrgRegistrationType, o.RegistrationType1) : ""),
                                pspIssuedNum = o.PSPIssuedNum,
                                fdPermitIssuedNum = o.FDPermitIssuedNum,
                                ssafPermitIssuedNum = o.SSAFPermitIssuedNum,
                                complaintReceivedNum = o.ComplaintReceivedNum,
                                EngMailingAddress = (!String.IsNullOrEmpty(o.EngMailingAddress1) ? o.EngMailingAddress1 + Environment.NewLine : "") + (!String.IsNullOrEmpty(o.EngMailingAddress2) ? o.EngMailingAddress2 + Environment.NewLine : "") +
                                                    (!String.IsNullOrEmpty(o.EngMailingAddress3) ? o.EngMailingAddress3 + Environment.NewLine : "") + (!String.IsNullOrEmpty(o.EngMailingAddress4) ? o.EngMailingAddress4 + Environment.NewLine : "") +
                                                    (!String.IsNullOrEmpty(o.EngMailingAddress5) ? o.EngMailingAddress5 : ""),
                                ChiMailingAddress = o.ChiMailingAddress1 + o.ChiMailingAddress2 + o.ChiMailingAddress3 + o.ChiMailingAddress4 + o.ChiMailingAddress5,
                                contactPerson = o.ContactPerson,
                                contactPersonEmail = o.EmailAddress,
                                orgEmailAddress = o.EmailAddress,
                                orgId = o.OrgId,
                                withHoldingInd = o.WithholdingInd
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<OrganisationViewModel>();
            var yesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);

            if (model.OrganisationReference.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationReference"] + " : ORG" + model.OrganisationReference);

            if (model.OrganisationName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationName"] + " : " + model.OrganisationName);

            if (model.TelephoneNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["TelephoneNo"] + " : " + model.TelephoneNo);

            if (model.Address.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["Address"] + " : " + model.Address);

            if (model.OrganisationStatusId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationStatusId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrganisationStatusId]);

            if (model.SubventedId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SubventedId"] + " : " + yesNo[model.SubventedId]);
            }

            if (model.NameofContactPerson.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["NameofContactPerson"] + " : " + model.NameofContactPerson);

            if (model.PrincipalActivities.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PrincipalActivities"] + " : " + model.PrincipalActivities);

            if (model.RegistrationTitleId.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationTitleId];
                if (model.RegistrationTitleId == "Others")
                {
                    if (model.Registration.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc + " ( " + model.Registration + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc);
                }
            }

            if (model.SectionId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SectionId"] + " : " + yesNo[model.SectionId]);

            if (model.AppliedPSPBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedPSPBeforeId"] + " : " + yesNo[model.AppliedPSPBeforeId]);

                if (model.FromPspApplicationDate.HasValue && model.ToPspApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspApplicationDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromPspApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToPspApplicationDate.HasValue)
                {
                    tmpVal = "To " + model.ToPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                if (tmpVal != "") filterCriterias.Add(fieldNames["AppliedPSPBeforeId"] + " : " + tmpVal);
            }

            tmpVal = "";
            if (model.AppliedSSAFBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedSSAFBeforeId"] + " : " + yesNo[model.AppliedSSAFBeforeId]);

                if (model.FromSSAFApplicationDate.HasValue && model.ToSSAFApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFApplicationDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromSSAFApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToSSAFApplicationDate.HasValue)
                {
                    tmpVal = "To " + model.ToSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                if (tmpVal != "") filterCriterias.Add(fieldNames["AppliedSSAFBeforeId"] + " : " + tmpVal);
            }

            tmpVal = "";
            if (model.AppliedFDBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedFDBeforeId"] + " : " + yesNo[model.AppliedFDBeforeId]);
                if (model.AppliedFDBeforeFdYear != null)
                {
                    string FdYearDescs = "";
                    foreach (string FdYearId in model.AppliedFDBeforeFdYear)
                    {
                        string FdYearDesc = FdYearId;
                        FdYearDescs += FdYearDesc + ";";
                    }
                    filterCriterias.Add(fieldNames["AppliedFDBeforeFdYear"] + " : " + FdYearDescs);
                }
            }

            tmpVal = "";
            if (model.PSPIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["PSPIssuedBeforeId"] + " : " + yesNo[model.PSPIssuedBeforeId]);
                if (model.FromPspPermitIssueDate.HasValue && model.ToPspPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspPermitIssueDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromPspPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToPspPermitIssueDate.HasValue)
                {
                    tmpVal = "To " + model.ToPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }

                if (tmpVal != "") filterCriterias.Add(fieldNames["PSPIssuedBeforeId"] + " : " + tmpVal);
            }

            tmpVal = "";
            if (model.SSAFIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SSAFIssuedBeforeId"] + " : " + yesNo[model.SSAFIssuedBeforeId]);
                if (model.FromSSAFPermitIssueDate.HasValue && model.ToSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "To " + model.ToSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }

                if (tmpVal != "") filterCriterias.Add(fieldNames["SSAFIssuedBeforeId"] + " : " + tmpVal);
            }

            if (model.FDIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["FDIssuedBeforeId"] + " : " + yesNo[model.FDIssuedBeforeId]);
                if (model.FdIssuedBeforeFdYear != null)
                {
                    string FdYearDescs = "";
                    foreach (string FdYearId in model.FdIssuedBeforeFdYear)
                    {
                        string FdYearDesc = FdYearId;
                        FdYearDescs += FdYearDesc + ";";
                    }
                    filterCriterias.Add(fieldNames["FdIssuedBeforeFdYear"] + " : " + FdYearDescs);
                }
            }

            if (model.ReceivedComplaintBeforeId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ReceivedComplaintBeforeId"] + " : " + yesNo[model.ReceivedComplaintBeforeId]);

            if (model.ReceivedEnquiryBeforeId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["ReceivedEnquiryBeforeId"] + " : " + yesNo[model.ReceivedEnquiryBeforeId]);

            if (model.WithholdingInd.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["WithholdingInd"] + " : " + yesNo[model.WithholdingInd]);

            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion ExportOrgMaster

        #region OrgMaster tabList

        #region ListOrgMasterHistory

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listOrgMasterHistory", Name = "ListOrgMasterHistory")]
        public JsonResult ListOrgMasterHistory(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "OrgMaster.OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var orgHistory = _orgNameChangeHistoryService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgHistory.TotalPages,
                CurrentPageIndex = orgHistory.CurrentPageIndex,
                TotalCount = orgHistory.TotalCount,
                Data = (from p in orgHistory
                        select new
                        {
                            OrgNameChangeId = p.OrgNameChangeId,
                            ChangeDate = p.ChangeDate,
                            EngOrgName = p.EngOrgName,
                            ChiOrgName = p.ChiOrgName,
                            Remarks = p.Remarks,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/updateOrgNameChangeHistory", Name = "UpdateOrgNameChangeHistory")]
        public JsonResult UpdateOrgNameChangeHistory([CustomizeValidator(RuleSet = "default,Update")] OrgNameChangeHistoryViewModel model, string orgId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var orgNameChangeHistory = _orgNameChangeHistoryService.GetById(Convert.ToInt32(model.OrgNameChangeId));
            orgNameChangeHistory.ChangeDate = CommonHelper.ConvertStringToDateTime(model.OrgNameChangeDate);
            orgNameChangeHistory.Remarks = model.OrgNameChangeRemarks;
            using (_unitOfWork.BeginTransaction())
            {
                _orgNameChangeHistoryService.Update(orgNameChangeHistory);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/{orgNameChangeId:int}/deleteOrgNameChangeHistory", Name = "DeleteOrgNameChangeHistory")]
        public JsonResult DeleteOrgNameChangeHistory(int orgNameChangeId)
        {
            Ensure.NotNull(orgNameChangeId);

            using (_unitOfWork.BeginTransaction())
            {
                _orgNameChangeHistoryService.Delete(orgNameChangeId);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion ListOrgMasterHistory

        #region ExportNameChangeHistory

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportNameChangeHistory", Name = "ExportNameChangeHistory")]
        public JsonResult ExportNameChangeHistory(ExportSettings exportSettings, string orgId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgMaster.OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });
            var orgHistory = _orgNameChangeHistoryService.GetPage(exportSettings.GridSettings);
            var dataList = (from p in orgHistory
                            select new
                            {
                                OrgNameChangeId = p.OrgNameChangeId,
                                ChangeDate = p.ChangeDate.ToString("dd/MM/yyyy"),
                                EngOrgName = p.EngOrgName,
                                ChiOrgName = p.ChiOrgName,
                                Remarks = p.Remarks,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion ExportNameChangeHistory

        #region ListWithholdingHistory

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listWithholdingHistory", Name = "ListWithholdingHistory")]
        public JsonResult ListWithholdingHistory(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "orgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });
            grid.SortColumn = "withholdingBeginDate";
            var withholdingHistory = this._withholdingHistoryService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = withholdingHistory.TotalPages,
                CurrentPageIndex = withholdingHistory.CurrentPageIndex,
                TotalCount = withholdingHistory.TotalCount,
                Data = (from w in withholdingHistory
                        select new
                        {
                            // PspEventEndDate = p.PspEventEndDate,
                            WithholdSource = w.WithholdSource,
                            RecordKey = w.RecordKey,
                            WithholdingBeginDate = w.WithholdingBeginDate,
                            WithholdingEndDate = w.WithholdingEndDate,
                            EventEndDate = w.EventEndDate,
                            PermitNum = w.PermitNum,
                            DocSubmission = w.DocSubmission != null ? "Yes" : "No",
                            OfficialReceiptReceivedDate = w.OfficialReceiptReceivedDate,
                            NewspaperCuttingReceivedDate = w.NewspaperCuttingReceivedDate,
                            WithholdingReason = w.WithholdingReason,
                            DocRemark = w.DocRemark,
                            Ref = w.Ref
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ListWithholdingHistory

        #region ExportWithholdingHistory

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportWithholdingHistory", Name = "ExportWithholdingHistory")]
        public JsonResult ExportWithholdingHistory(ExportSettings exportSettings, string orgId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var pspEvent = _withholdingHistoryService.GetPage(exportSettings.GridSettings);
            {
                var dataList = (from w in pspEvent
                                select new
                                {
                                    WithholdSource = w.WithholdSource,
                                    RecordKey = w.RecordKey,
                                    WithholdingBeginDate = w.WithholdingBeginDate,
                                    WithholdingEndDate = w.WithholdingEndDate,
                                    EventEndDate = w.EventEndDate,
                                    PermitNum = w.PermitNum,
                                    DocSubmission = w.DocSubmission != null ? "Yes" : "No",
                                    OfficialReceiptReceivedDate = w.OfficialReceiptReceivedDate != null ? "Yes" : "No",
                                    NewspaperCuttingReceivedDate = w.NewspaperCuttingReceivedDate != null ? "Yes" : "No",
                                    DocRemark = w.DocRemark,
                                }).ToArray();

                MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

                var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

                String sessionId = "GridReport";

                return JsonReportResult(sessionId, time + ".xlsx", ms);
            }
        }

        #endregion ExportWithholdingHistory

        #region ListReferenceGuide

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listReferenceGuide", Name = "ListReferenceGuide")]
        public JsonResult ListReferenceGuide(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "orgMaster.OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });
            var referenceGuide = this._orgRefGuidePromulgationService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = referenceGuide.TotalPages,
                CurrentPageIndex = referenceGuide.CurrentPageIndex,
                TotalCount = referenceGuide.TotalCount,
                Data = (from r in referenceGuide
                        select new
                        {
                            OrgRefGuidePromulgationId = r.OrgRefGuidePromulgationId,
                            SendDate = r.SendDate,
                            PartNum = r.PartNum,
                            EnclosureNum = r.EnclosureNum,
                            ReplySlipDate = r.ReplySlipDate,
                            OrgReply = !String.IsNullOrEmpty(r.OrgReply) ? _lookupService.GetDescription(LookupType.OrgReply, r.OrgReply) : "",
                            PromulgationReason = r.PromulgationReason,
                            Remarks = r.Remarks,
                            ReplySlipReceiveDate = r.ReplySlipReceiveDate,
                            EngMailingAddress = r.PromulgationReason,
                            ReplySlipPartNum = r.ReplySlipPartNum,
                            ReplySlipEnclosureNum = r.ReplySlipEnclosureNum,

                            ActivityConcern = r.ActivityConcern
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ListReferenceGuide

        #region ListAFS

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{orgId}/listAfs", Name = "ListAfs")]
        public JsonResult ListAFS(GridSettings grid, int orgId)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "orgId",
                data = orgId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule()
            {
                field = "afsRecordStartDate",
                data = null,
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            rules.Add(new Rule()
            {
                field = "afsRecordEndDate",
                data = null,
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            rules.Add(new Rule()
            {
                field = "AfsRecordDetails",
                data = "",
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            grid.AddDefaultRule(rules, GroupOp.OR);

            var orgAfs = this._orgAfsTrViewService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgAfs.TotalPages,
                CurrentPageIndex = orgAfs.CurrentPageIndex,
                TotalCount = orgAfs.TotalCount,
                Data = (from w in orgAfs
                        select new
                        {
                            RecordKey = w.RecordKey,
                            OrgId = w.OrgId,
                            OrgRef = w.OrgRef,
                            PermitType = w.PermitType,
                            FileRef = w.FileRef,
                            AfsRecordStartDate = w.AfsRecordStartDate,
                            AfsRecordEndDate = w.AfsRecordEndDate,
                            AfsRecordDetails = w.AfsRecordDetails,
                            CreatedOn = w.CreatedOn,
                            ApplicationReceiveDate = w.ApplicationReceiveDate
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ListAFS

        #region ListTrackRecord

        [HttpGet, Route("~/api/org/{orgId}/listTrackRecord", Name = "ListTrackRecord")]
        public JsonResult ListTrackRecord(GridSettings grid, int orgId)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "orgId",
                data = orgId.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule()
            {
                field = "trackRecordStartDate",
                data = null,
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            rules.Add(new Rule()
            {
                field = "trackRecordEndDate",
                data = null,
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            rules.Add(new Rule()
            {
                field = "trackRecordDetails",
                data = "",
                op = WhereOperation.NotEqual.ToEnumValue()
            });

            grid.AddDefaultRule(rules, GroupOp.OR);

            var orgAfs = this._orgAfsTrViewService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgAfs.TotalPages,
                CurrentPageIndex = orgAfs.CurrentPageIndex,
                TotalCount = orgAfs.TotalCount,
                Data = (from w in orgAfs
                        select new
                        {
                            RecordKey = w.RecordKey,
                            OrgId = w.OrgId,
                            OrgRef = w.OrgRef,
                            PermitType = w.PermitType,
                            FileRef = w.FileRef,
                            TrackRecordStartDate = w.TrackRecordStartDate,
                            TrackRecordEndDate = w.TrackRecordEndDate,
                            TrackRecordDetails = w.TrackRecordDetails,
                            CreatedOn = w.CreatedOn,
                            ApplicationReceiveDate = w.ApplicationReceiveDate
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion ListTrackRecord

        #region listPSPTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listPSPTab", Name = "ListPSPTab")]
        public JsonResult listPSPTab(GridSettings grid, string Code, bool isSSAF = false)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            grid.AddDefaultRule(new Rule()
            {
                field = "IsSsaf",
                data = isSSAF.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var pageList = _organisationService.getPageByOrgEditPspView(grid);
            var gridResult = new GridResult
            {
                TotalPages = pageList.TotalPages,
                CurrentPageIndex = pageList.CurrentPageIndex,
                TotalCount = pageList.TotalCount,
                Data = (from p in pageList
                        select new
                        {
                            PspMasterId = p.PspMasterId,
                            PspRef = p.PspRef,
                            ApplicationReceiveDate = p.ApplicationReceiveDate,
                            ApplicationDisposalDate = p.ApplicationDisposalDate,
                            EngFundRaisingPurpose = p.EngFundRaisingPurpose,
                            ApplicationResult = !String.IsNullOrEmpty(p.ApplicationResult) ? _lookupService.GetDescription(LookupType.PSPApplicationResult, p.ApplicationResult) : "",
                            TwoBatchApproachIndicator = p.TwoBatchApproachIndicator,
                            ProcessingOfficerPost = p.ProcessingOfficerPost,
                            EventPeriodFrom = p.EventPeriodFrom,
                            EventPeriodTo = p.EventPeriodTo,
                            EventApprovedNum = p.EventApprovedNum,
                            EventHeldNum = p.EventHeldNum,
                            EventCancelledNum = p.EventCancelledNum,
                            EventHeldPercent = p.EventHeldPercent,
                            ArCheckIndicator = p.ArCheckIndicator,
                            PermitNum = p.PermitNum,
                            SpecialRemark = p.SpecialRemark
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion listPSPTab

        #region ExportPSPTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportPSPTab", Name = "ExportPSPTab")]
        public JsonResult ExportPSPTab(ExportSettings exportSettings, string orgId, bool isSSAF = false)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });

            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "IsSsaf",
                data = isSSAF.ToString(),
                op = WhereOperation.Equal.ToEnumValue()
            });

            var pageList = _organisationService.getPageByOrgEditPspView(exportSettings.GridSettings);
            var dataList = (from p in pageList
                            select new
                            {
                                PspRef = p.PspRef,
                                PermitNum = p.PermitNum,
                                ApplicationReceiveDate = p.ApplicationReceiveDate == null ? "" : p.ApplicationReceiveDate.Value.ToString("dd/MM/yyyy"),
                                ApplicationDisposalDate = p.ApplicationDisposalDate == null ? "" : p.ApplicationDisposalDate.Value.ToString("dd/MM/yyyy"),
                                EngFundRaisingPurpose = p.EngFundRaisingPurpose,
                                ApplicationResult = !String.IsNullOrEmpty(p.ApplicationResult) ? _lookupService.GetDescription(LookupType.PSPApplicationResult, p.ApplicationResult) : "",
                                EventPeriodFrom = p.EventPeriodFrom == null ? "" : p.EventPeriodFrom.Value.ToString("dd/MM/yyyy"),
                                EventPeriodTo = p.EventPeriodTo == null ? "" : p.EventPeriodTo.Value.ToString("dd/MM/yyyy"),
                                TwoBatchApproachIndicator = (p.TwoBatchApproachIndicator == null || p.TwoBatchApproachIndicator.Value == false) ? "No" : "Yes",
                                ProcessingOfficerPost = p.ProcessingOfficerPost,
                                EventApprovedNum = p.EventApprovedNum,
                                EventHeldNum = p.EventHeldNum,
                                EventCancelledNum = p.EventCancelledNum,
                                EventHeldPercent = p.EventHeldPercent,
                                ArCheckIndicator = p.ApplicationResult == "01" ? p.ArCheckIndicator : "NA",
                                PspMasterId = p.PspMasterId,
                                SpecialRemark = p.SpecialRemark
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion ExportPSPTab

        #region listFDTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listFDTab", Name = "ListFDTab")]
        public JsonResult listFDTab(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var temp = _flagDayService.GetPageByOrgFdTabGridView(grid);

            var gridResult = new GridResult
            {
                TotalPages = temp.TotalPages,
                CurrentPageIndex = temp.CurrentPageIndex,
                TotalCount = temp.TotalCount,
                Data = (from p in temp
                        select new
                        {
                            FdRef = p.FdRef,
                            FdYear = p.FdYear,
                            FlagDay = p.FlagDay,
                            TWR = p.TWR,
                            FdGroup = p.FdGroup,
                            FdGroupDesc = p.FdGroupDesc,
                            ApplicationReceiveDate = p.ApplicationReceiveDate,
                            ApplicationResult = p.ApplicationResult,
                            AfsReceiveIndicator = p.AfsReceiveIndicator,
                            ApplyPledgingMechanismIndicator = p.ApplyPledgingMechanismIndicator,
                            NetProceed = p.NetProceed,
                            PermitNum = p.PermitNum,
                            PermitIssueDate = p.PermitIssueDate,
                            ProposalDetail = p.FundRaisingPurpose,
                            FdMasterId = p.FdMasterId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion listFDTab

        #region ExportFDTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportFDTab", Name = "ExportFDTab")]
        public JsonResult ExportFDTab(ExportSettings exportSettings, string orgId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var temp = _flagDayService.GetPageByOrgFdTabGridView(exportSettings.GridSettings);

            var dataList = temp;

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion ExportFDTab

        #region listComplaintTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/{code}/listComplaintEnquiry/{type}", Name = "ListOrgComplaintEnquiry")]
        public JsonResult ListOrgComplaintEnquiry(GridSettings grid, string code, string type)
        {
            grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgRef",
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
                            PspRef = "{0} {1}".FormatWith(c.PspPermitNum, c.PspRef).Trim(),
                            PspPermitNum = c.PspPermitNum,
                            ComplaintResult = c.NonComplianceNatureResult,
                            ReplyDueDate = c.ReplyDueDate,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/{code}/exportComplaintEnquiry/{type}", Name = "ExportOrgComplaintEnquiry")]
        public JsonResult ExportOrgComplaintEnquiry(ExportSettings exportSettings, string code, string type)
        {
            exportSettings.GridSettings.AddDefaultRule(new List<Rule>{
                new Rule()
                    {
                        field = "orgRef",
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

            var list = this._complaintMasterService.GetPageByComplaintMasterSearchView(exportSettings.GridSettings, false, false, false, false, null, null);

            var dataList = (from c in list
                            select new
                            {
                                ComplaintMasterId = c.ComplaintMasterId,
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
                                PspRef = "{0} {1}".FormatWith(c.PspPermitNum, c.PspRef).Trim(),
                                ComplaintResult = c.NonComplianceNatureResult,
                                ReplyDueDate = c.ReplyDueDate != null ? CommonHelper.ConvertDateTimeToString(c.ReplyDueDate.Value, "dd/MM/yyyy") : "",
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listComplaintTab", Name = "ListComplaintTab")]
        public JsonResult listComplaintTab(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var complaintMaster = _organisationService.getPageByOrgEditComplaintView(grid);

            var gridResult = new GridResult
            {
                TotalPages = complaintMaster.TotalPages,
                CurrentPageIndex = complaintMaster.CurrentPageIndex,
                TotalCount = complaintMaster.TotalCount,
                Data = (from p in complaintMaster
                        select new
                        {
                            ComplaintRef = p.ComplaintRef,
                            ComplaintSource = !String.IsNullOrEmpty(p.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, p.ComplaintSource) : "",
                            ActivityConcern = !String.IsNullOrEmpty(p.ActivityConcern) ? _lookupService.GetDescription(LookupType.ComplaintActivityConcern, p.ActivityConcern) : "",
                            ComplaintDate = p.ComplaintDate,
                            PermitNum = p.PermitNum,
                            FollowUpLetterType = !String.IsNullOrEmpty(p.FollowUpLetterType) ? _lookupService.GetDescription(LookupType.FollowUpLetterType, p.FollowUpLetterType) : "",
                            FollowUpLetterIssueDate = p.FollowUpLetterIssueDate,
                            LetterIssuedNum = p.LetterIssuedNum,
                            ComplaintRemarks = p.ComplaintRemarks,
                            ComplaintMasterId = p.ComplaintMasterId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/org/{code}/listEnquiryTab", Name = "ListEnquiryTab")]
        public JsonResult ListEnquiryTab(GridSettings grid, string Code)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = Code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var complaintMaster = _organisationService.getPageByOrgEditEnquiryView(grid);

            var gridResult = new GridResult
            {
                TotalPages = complaintMaster.TotalPages,
                CurrentPageIndex = complaintMaster.CurrentPageIndex,
                TotalCount = complaintMaster.TotalCount,
                Data = (from p in complaintMaster
                        select new
                        {
                            ComplaintRef = p.ComplaintRef,
                            ComplaintSource = !String.IsNullOrEmpty(p.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, p.ComplaintSource) : "",
                            ActivityConcern = !String.IsNullOrEmpty(p.ActivityConcern) ? _lookupService.GetDescription(LookupType.ComplaintActivityConcern, p.ActivityConcern) : "",
                            ComplaintDate = p.ComplaintDate,
                            PermitNum = p.PermitNum,
                            FollowUpLetterType = !String.IsNullOrEmpty(p.FollowUpLetterType) ? _lookupService.GetDescription(LookupType.FollowUpLetterType, p.FollowUpLetterType) : "",
                            FollowUpLetterIssueDate = p.FollowUpLetterIssueDate,
                            LetterIssuedNum = p.LetterIssuedNum,
                            ComplaintRemarks = p.ComplaintRemarks,
                            ComplaintMasterId = p.ComplaintMasterId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion listComplaintTab

        #region ExportComplaintTab

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportComplaintTab", Name = "ExportComplaintTab")]
        public JsonResult ExportComplaintTab(ExportSettings exportSettings, string orgId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var complaintMaster = _organisationService.getPageByOrgEditComplaintView(exportSettings.GridSettings);
            var dataList = (from p in complaintMaster
                            select new
                            {
                                ComplaintRef = p.ComplaintRef,
                                ComplaintSource = !String.IsNullOrEmpty(p.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, p.ComplaintSource) : "",
                                ActivityConcern = !String.IsNullOrEmpty(p.ActivityConcern) ? _lookupService.GetDescription(LookupType.ComplaintActivityConcern, p.ActivityConcern) : "",
                                ComplaintDate = p.ComplaintDate == null ? "" : p.ComplaintDate.Value.ToString("dd/MM/yyyy"),
                                PermitNum = p.PermitNum,                                
                                FollowUpLetterType = !String.IsNullOrEmpty(p.FollowUpLetterType) ? _lookupService.GetDescription(LookupType.FollowUpLetterType, p.FollowUpLetterType) : "",
                                FollowUpLetterIssueDate = p.FollowUpLetterIssueDate == null ? "" : p.FollowUpLetterIssueDate.Value.ToString("dd/MM/yyyy"),
                                LetterIssuedNum = p.LetterIssuedNum,
                                ComplaintRemarks = p.ComplaintRemarks,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/organisation/{orgId}/exportEnquiryTab", Name = "ExportEnquiryTab")]
        public JsonResult ExportEnquiryTab(ExportSettings exportSettings, string orgId)
        {
            exportSettings.GridSettings.AddDefaultRule(new Rule()
            {
                field = "OrgId",
                data = orgId,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var enquiryMaster = _organisationService.getPageByOrgEditEnquiryView(exportSettings.GridSettings);
            var dataList = (from p in enquiryMaster
                            select new
                            {
                                ComplaintRef = p.ComplaintRef,
                                ComplaintSource = !String.IsNullOrEmpty(p.ComplaintSource) ? _lookupService.GetDescription(LookupType.ComplaintSource, p.ComplaintSource) : "",
                                ActivityConcern = !String.IsNullOrEmpty(p.ActivityConcern) ? _lookupService.GetDescription(LookupType.ComplaintActivityConcern, p.ActivityConcern) : "",
                                ComplaintDate = p.ComplaintDate.Value == null ? "" : p.ComplaintDate.Value.ToString("dd/MM/yyyy"),
                                PermitNum = p.PermitNum,
                                FollowUpLetterType = !String.IsNullOrEmpty(p.FollowUpLetterType) ? _lookupService.GetDescription(LookupType.FollowUpLetterType, p.FollowUpLetterType) : "",
                                FollowUpLetterIssueDate = p.FollowUpLetterIssueDate == null ? "" : p.FollowUpLetterIssueDate.Value.ToString("dd/MM/yyyy"),
                                LetterIssuedNum = p.LetterIssuedNum,
                                ComplaintRemarks = p.ComplaintRemarks,
                            }).ToArray();

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        #endregion ExportComplaintTab

        #region GetTWRDescription

        private string GetTWRDescription(FdEvent fdEvent)
        {
            string description = "";

            if (!String.IsNullOrEmpty(fdEvent.TWR))
            {
                description = _lookupService.GetDescription(LookupType.TWR, fdEvent.TWR);
            }
            if (!String.IsNullOrEmpty(fdEvent.TwrDistrict))
            {
                description = description + "-" + _lookupService.GetDescription(LookupType.TWRDistrict, fdEvent.TwrDistrict);
            }
            return description;
        }

        #endregion GetTWRDescription

        #region Amount

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/{code}/calRelevantRecordsforOrg", Name = "CalRelevantRecordsforOrg")]
        public JsonResult CalRelevantRecordsforOrg(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code);
            var TemplateAmount = _organizationDocService.GetOrgDocAmount();
            var AttachmentAmount = _orgAttachmentService.GetOrgAttachmentAmountByCode(code);
            var NameChangeHistoryAmount = _orgNameChangeHistoryService.GetOrgNameChangeHistoryAmountByOrgId(code);
            var WithholdingHistoryAmount = this._withholdingHistoryService.GetWithholdingHistoryAmountByOrgId(code);

            var PspAmount = _organisationService.GetOrgEditPspViewAmountByOrgId(code);
            var FdAmount = _flagDayService.GetFdTabGridViewAmountByOrgId(code);
            var SSAFAmount = _organisationService.GetOrgEditPspViewAmountByOrgId(code, true);
            var ComplaintAmount = _organisationService.GetOrgEditComplaintViewAmountByOrgId(code);
            var EnquiryAmount = _organisationService.GetOrgEditEnquiryViewAmountByOrgId(code);
            var orgAfsAmt = this._orgAfsTrViewService.GetAfsViewAmt(code);
            var orgTrAmt = this._orgAfsTrViewService.GetTrViewAmt(code);
            var referenceGuideAmount = _organisationService.GetOrgEditReferenceGuideViewAmountByOrgId(code);

            var map = new Hashtable();
            map.Add("templateAmount", TemplateAmount);
            map.Add("attachmentAmount", AttachmentAmount);
            map.Add("nameChangeHistoryAmount", NameChangeHistoryAmount);
            map.Add("withholdingHistoryAmount", WithholdingHistoryAmount);
            map.Add("pspAmount", PspAmount);
            map.Add("fdAmount", FdAmount);
            map.Add("ssafAmount", SSAFAmount);
            map.Add("complaintAmount", ComplaintAmount);
            map.Add("enquiryAmount", EnquiryAmount);
            map.Add("afsAmount", orgAfsAmt);
            map.Add("trackRecordAmount", orgTrAmt);
            map.Add("referenceGuideAmount", referenceGuideAmount);

            return Json(new JsonResponse(true)
            {
                Data = map,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Amount

        #region Template

        [HttpGet, Route("~/api/org/listTemplateforOrg", Name = "ListTemplateforOrg")]
        public JsonResult ListTemplateforOrg(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);

            grid.AddDefaultRule(new Rule()
            {
                field = "DocStatus",
                data = "true",
                op = WhereOperation.Equal.ToEnumValue()
            });

            var orgDoc = _organizationDocService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgDoc.TotalPages,
                CurrentPageIndex = orgDoc.CurrentPageIndex,
                TotalCount = orgDoc.TotalCount,
                Data = (from s in orgDoc
                        select new
                        {
                            DocNum = s.DocNum,
                            DocName = s.DocName,
                            OrgDocId = s.OrgDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Template

        #region OrgAttachment

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/listOrgAttachment", Name = "ListOrgAttachment")]
        public JsonResult ListOrgAttachment(GridSettings grid, string code)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNullOrEmpty(code);

            grid.AddDefaultRule(new Rule()
            {
                field = "OrgMaster.OrgId",
                data = code,
                op = WhereOperation.Equal.ToEnumValue()
            });

            var orgAttachment = _orgAttachmentService.GetPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgAttachment.TotalPages,
                CurrentPageIndex = orgAttachment.CurrentPageIndex,
                TotalCount = orgAttachment.TotalCount,
                Data = (from s in orgAttachment
                        select new
                        {
                            OrgAttachmentId = s.OrgAttachmentId,
                            FileName = s.FileName,
                            CreatedById = s.CreatedById,
                            CreatedOn = s.CreatedOn,
                            FileDescription = s.FileDescription,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/{code}/createOrgAttachment", Name = "CreateOrgAttachment")]
        public JsonResult CreateOrgAttachment([CustomizeValidator(RuleSet = "default,CreateOrgAttachment")] OrganisationViewModel model, string code)
        {
            Ensure.Argument.NotNull(model);
            Ensure.Argument.NotNullOrEmpty(code, "code");
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the OrgMaster record by given the ID
            var orgMaster = _organisationService.GetOrgById(Convert.ToInt32(code));

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);

            // Rename the file by adding current time
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.AttachmentDocument.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path by adding the OrgId folder ( [Root Folder of Attachment] \ [OrgId
            // Folder] )
            string rootPath = Path.Combine(attachmentPath.Value, orgMaster.OrgId.ToString());
            // Form the Relative Path for storing in DB ( [OrgId Folder] \ [File Name] ) and
            // Absolute Path for actually saving the file ( [Root Folder of Attachment] \ [OrgId
            // Folder] \ [File Name] )
            string relativePath = Path.Combine(orgMaster.OrgId.ToString(), generatedFileName);
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new OrgAttachment row and fill the value
            var attachment = new OrgAttachment();
            attachment.OrgMaster = orgMaster;
            attachment.FileLocation = relativePath;
            attachment.FileName = model.AttachmentName;
            attachment.FileDescription = model.AttachmentRemark;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.AttachmentDocument.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                _orgAttachmentService.CreateOrgAttachment(attachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/updateOrgAttachment", Name = "UpdateOrgAttachment")]
        public JsonResult UpdateOrgAttachment([CustomizeValidator(RuleSet = "default,UpdateOrgAttachment")] OrganisationViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the OrgAttachment record by given the ID
            var AttachmentId = Convert.ToInt32(model.AttachmentId);
            var attachment = _orgAttachmentService.GetOrgAttachmentById(AttachmentId);

            // Fill the update values
            attachment.FileName = model.AttachmentName;
            attachment.FileDescription = model.AttachmentRemark;

            // Get the root path of the Attachment from DB
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);

            using (_unitOfWork.BeginTransaction())
            {
                // If new file need to be upload
                if (model.AttachmentDocument != null)
                {
                    // Reforming the file name by adding current time
                    string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                    string fileName = Path.GetFileName(model.AttachmentDocument.FileName);
                    string generatedFileName = string.Format("{0}-{1}", time, fileName);

                    // Set the root path by adding the OrgId folder ( [Root Folder of Attachment] \
                    // [OrgId Folder] )
                    string rootPath = Path.Combine(attachmentPath.Value, attachment.OrgMaster.OrgId.ToString());
                    // Form the Relative Path for storing in DB ( [OrgId Folder] \ [File Name] ) and
                    // Absolute Path for actually saving the file ( [Root Folder of Attachment] \
                    // [OrgId Folder] \ [File Name] )
                    string relativePath = Path.Combine(attachment.OrgMaster.OrgId.ToString(), generatedFileName);
                    string absolutePath = Path.Combine(rootPath, generatedFileName);

                    // Save the new file
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.AttachmentDocument.SaveAs(absolutePath);
                    }

                    // Form the path of the old file
                    string absolutePathOfOldFile = Path.Combine(attachmentPath.Value, attachment.FileLocation);

                    // Delete the old file
                    if (System.IO.File.Exists(absolutePathOfOldFile))
                    {
                        System.IO.File.Delete(absolutePathOfOldFile);
                    }

                    // Replace with the new path
                    attachment.FileLocation = relativePath;
                }

                // Update DB record and commit
                _orgAttachmentService.UpdateOrgAttachment(attachment);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/api/Org/{attachmentId:int}/downloadOrgAttachment", Name = "DownloadOrgAttachment")]
        public FileResult DownloadOrgAttachment(int attachmentId)
        {
            // Get the OrgAttachment record by given the ID
            var orgAttachment = _orgAttachmentService.GetOrgAttachmentById(attachmentId);
            Ensure.NotNull(orgAttachment, "No Organisation Attachment found with the specified id");

            // Get the root path of the Attachment from DB and combine with the FileLocation to get
            // the Absolute Path that the file actually stored at
            var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);
            string rootPath = attachmentPath.Value;
            var absolutePath = Path.Combine(rootPath, orgAttachment.FileLocation);
            // ( [Root Folder of Attachment] \ [OrgId Folder] \ [File Name] )

            // Set the file name for saving
            string fileName = orgAttachment.FileName + Path.GetExtension(Path.GetFileName(orgAttachment.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/{attachmentId:int}/deleteOrgAttachment", Name = "DeleteOrgAttachment")]
        public JsonResult DeleteOrgAttachment(int attachmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the OrgAttachment record by given the ID
            var orgAttachment = _orgAttachmentService.GetOrgAttachmentById(attachmentId);
            Ensure.NotNull(orgAttachment, "No Organisation Attachment found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                // Get the root path of the Attachment file from DB and combine with the
                // FileLocation to get the Absolute Path that the file actually stored at
                var attachmentPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);
                var absolutePath = Path.Combine(attachmentPath.Value, orgAttachment.FileLocation);
                // ( [Root Folder of Attachment] \ [OrgId Folder] \ [File Name] )

                // Delete the record in DB (set IsDeleted flag)
                _orgAttachmentService.DeleteOrgAttachment(orgAttachment);

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

        #endregion OrgAttachment

        #region OrgMaster Action

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/OrgMaster/createOrgMaster", Name = "CreateOrgMaster")]
        public JsonResult CreateOrgMaster([CustomizeValidator(RuleSet = "default,Create")] OrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var orgMaster = new OrgMaster();
            orgMaster.OrgRef = _organisationService.CreateOrgRef();
            orgMaster.DisableIndicator = model.OrganisationDisabled;
            orgMaster.EngOrgName = model.OrganisationName;
            orgMaster.ChiOrgName = model.OrganisationChiName;
            orgMaster.SimpChiOrgName = model.OrganisationSimpliChiName;
            orgMaster.EngOrgNameSorting = model.EngOrgNameSorting;
            orgMaster.OtherEngOrgName = model.OtherEngOrgName;
            orgMaster.OtherChiOrgName = model.OtherChiOrgName;
            orgMaster.OtherSimpChiOrgName = model.OtherSimpChiOrgName;
            orgMaster.EngRegisteredAddress1 = model.EngRegisteredAddress1;
            orgMaster.EngRegisteredAddress2 = model.EngRegisteredAddress2;
            orgMaster.EngRegisteredAddress3 = model.EngRegisteredAddress3;
            orgMaster.EngRegisteredAddress4 = model.EngRegisteredAddress4;
            orgMaster.EngRegisteredAddress5 = model.EngRegisteredAddress5;
            orgMaster.ChiRegisteredAddress1 = model.ChiRegisteredAddress1;
            orgMaster.ChiRegisteredAddress2 = model.ChiRegisteredAddress2;
            orgMaster.ChiRegisteredAddress3 = model.ChiRegisteredAddress3;
            orgMaster.ChiRegisteredAddress4 = model.ChiRegisteredAddress4;
            orgMaster.ChiRegisteredAddress5 = model.ChiRegisteredAddress5;
            orgMaster.EngMailingAddress1 = model.EngMailingAddress1;
            orgMaster.EngMailingAddress2 = model.EngMailingAddress2;
            orgMaster.EngMailingAddress3 = model.EngMailingAddress3;
            orgMaster.EngMailingAddress4 = model.EngMailingAddress4;
            orgMaster.EngMailingAddress5 = model.EngMailingAddress5;
            orgMaster.ChiMailingAddress1 = model.ChiMailingAddress1;
            orgMaster.ChiMailingAddress2 = model.ChiMailingAddress2;
            orgMaster.ChiMailingAddress3 = model.ChiMailingAddress3;
            orgMaster.ChiMailingAddress4 = model.ChiMailingAddress4;
            orgMaster.ChiMailingAddress5 = model.ChiMailingAddress5;
            orgMaster.URL = model.OrganisationWebsite;
            orgMaster.TelNum = model.TelNum;
            orgMaster.FaxNum = model.Fax;
            orgMaster.EmailAddress = model.Email;
            orgMaster.ApplicantSalute = model.ApplicantSaluteId;
            orgMaster.ApplicantFirstName = model.ApplicantFirstName;
            orgMaster.ApplicantLastName = model.ApplicantLastName;
            orgMaster.ApplicantChiFirstName = model.ApplicantChiFirstName;
            orgMaster.ApplicantChiLastName = model.ApplicantChiLastName;
            orgMaster.ApplicantPosition = model.ApplicantPosition;
            orgMaster.ApplicantTelNum = model.ApplicantTelNum;
            orgMaster.PresidentName = model.President;
            orgMaster.SecretaryName = model.Secretary;
            orgMaster.TreasurerName = model.Treasurer;
            orgMaster.OrgObjective = model.Objectives;
            orgMaster.SubventedIndicator = model.Subvented;
            orgMaster.Section88Indicator = model.Section88;
            orgMaster.Section88StartDate = model.Section88Date;
            orgMaster.RegistrationType1 = model.RegistrationType1;
            orgMaster.RegistrationOtherName1 = model.RegistrationOtherName1;
            orgMaster.RegistrationDate1 = model.RegistrationDate1;
            orgMaster.RegistrationType2 = model.RegistrationType2;
            orgMaster.RegistrationOtherName2 = model.RegistrationOtherName2;
            orgMaster.RegistrationDate2 = model.RegistrationDate2;

            orgMaster.AddressProofIndicator = model.AddressProofIndicator;
            orgMaster.AddressProofDate = model.AddressProofDate;
            orgMaster.MaaConstitutionIndicator = model.MaaConstitutionIndicator;

            orgMaster.QualifiedOpinionIndicator = model.QualifiedOpinionIndicator;
            orgMaster.OtherSupportDocIndicator = model.OtherSupportDocIndicator;
            orgMaster.QualifiedOpinionRemark = model.QualifiedOpinionRemark;
            orgMaster.OtherSupportDocRemark = model.OtherSupportDocRemark;
            orgMaster.OverallRemark = model.OverallRemark;

            using (_unitOfWork.BeginTransaction())
            {
                _organisationService.CreateOrgMaster(orgMaster);
                if (model.AddressProofAttachmentId != null)
                {
                    string fileDescription = string.Format("Address Proof File as of {0}", CommonHelper.ConvertDateTimeToString(orgMaster.CreatedOn));
                    var orgAttachment = SaveOrgAttachmentFile(model.AddressProofAttachmentId, orgMaster, fileDescription);
                    orgMaster.AddressProofAttachmentId = orgAttachment.OrgAttachmentId;
                }
                if (model.MaaConstitutionAttachmentId != null)
                {
                    string fileDescription = string.Format("MAA / Consitution File as of {0}", CommonHelper.ConvertDateTimeToString(orgMaster.CreatedOn));
                    var orgAttachment = SaveOrgAttachmentFile(model.MaaConstitutionAttachmentId, orgMaster, fileDescription);
                    orgMaster.MaaConstitutionAttachmentId = orgAttachment.OrgAttachmentId;
                }
                if (model.OtherSupportDocAttachmentId != null)
                {
                    string remark = "";
                    if (!String.IsNullOrEmpty(model.OtherSupportDocRemark))
                    {
                        remark = model.OtherSupportDocRemark.Length > 150 ? model.OtherSupportDocRemark.Substring(0, 150) : model.OtherSupportDocRemark;
                    }
                    string fileDescription = string.Format("Other Support Document - {0}, as of {1}", remark, DateTime.Now.ToString("dd/MM/yyyy"));
                    var orgAttachment = SaveOrgAttachmentFile(model.OtherSupportDocAttachmentId, orgMaster, fileDescription);
                    orgMaster.OtherSupportDocAttachmentId = orgAttachment.OrgAttachmentId;
                }

                _organisationService.UpdateOrgMaster(orgMaster);
                InsertOrgNameChangeHistory(orgMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = orgMaster.OrgId,
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/OrgMaster/isExistedOrgName", Name = "IsExistedOrgName")]
        public JsonResult IsExistedOrgName(OrganisationViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var result = _organisationService.IsExistedOrgName(model.OrganisationName, model.OrganisationChiName);
            return Json(new JsonResponse(true)
            {
                Data = result,
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/OrgMaster/{orgId}/updateOrgMaster", Name = "UpdateOrgMaster")]
        public JsonResult UpdateOrgMaster([CustomizeValidator(RuleSet = "default,Update")] OrganisationViewModel model, string orgId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var orgMaster = _organisationService.GetOrgById(Convert.ToInt32(orgId));
            var oldOrgMaster = new OrgMaster();
            AutoMapper.Mapper.Map(orgMaster, oldOrgMaster);

            string engOrgName = orgMaster.EngOrgName;
            string chiOrgName = orgMaster.ChiOrgName;
            string simpChiOrgName = orgMaster.SimpChiOrgName;
            orgMaster.DisableIndicator = model.OrganisationDisabled;
            orgMaster.EngOrgName = model.OrganisationName;
            orgMaster.ChiOrgName = model.OrganisationChiName;
            orgMaster.SimpChiOrgName = model.OrganisationSimpliChiName;
            orgMaster.EngOrgNameSorting = model.EngOrgNameSorting;
            orgMaster.OtherEngOrgName = model.OtherEngOrgName;
            orgMaster.OtherChiOrgName = model.OtherChiOrgName;
            orgMaster.OtherSimpChiOrgName = model.OtherSimpChiOrgName;
            orgMaster.EngRegisteredAddress1 = model.EngRegisteredAddress1;
            orgMaster.EngRegisteredAddress2 = model.EngRegisteredAddress2;
            orgMaster.EngRegisteredAddress3 = model.EngRegisteredAddress3;
            orgMaster.EngRegisteredAddress4 = model.EngRegisteredAddress4;
            orgMaster.EngRegisteredAddress5 = model.EngRegisteredAddress5;
            orgMaster.ChiRegisteredAddress1 = model.ChiRegisteredAddress1;
            orgMaster.ChiRegisteredAddress2 = model.ChiRegisteredAddress2;
            orgMaster.ChiRegisteredAddress3 = model.ChiRegisteredAddress3;
            orgMaster.ChiRegisteredAddress4 = model.ChiRegisteredAddress4;
            orgMaster.ChiRegisteredAddress5 = model.ChiRegisteredAddress5;
            orgMaster.EngMailingAddress1 = model.EngMailingAddress1;
            orgMaster.EngMailingAddress2 = model.EngMailingAddress2;
            orgMaster.EngMailingAddress3 = model.EngMailingAddress3;
            orgMaster.EngMailingAddress4 = model.EngMailingAddress4;
            orgMaster.EngMailingAddress5 = model.EngMailingAddress5;
            orgMaster.ChiMailingAddress1 = model.ChiMailingAddress1;
            orgMaster.ChiMailingAddress2 = model.ChiMailingAddress2;
            orgMaster.ChiMailingAddress3 = model.ChiMailingAddress3;
            orgMaster.ChiMailingAddress4 = model.ChiMailingAddress4;
            orgMaster.ChiMailingAddress5 = model.ChiMailingAddress5;
            orgMaster.URL = model.OrganisationWebsite;
            orgMaster.TelNum = model.TelNum;
            orgMaster.FaxNum = model.Fax;
            orgMaster.EmailAddress = model.Email;
            orgMaster.ApplicantSalute = model.ApplicantSaluteId;
            orgMaster.ApplicantFirstName = model.ApplicantFirstName;
            orgMaster.ApplicantLastName = model.ApplicantLastName;
            orgMaster.ApplicantChiFirstName = model.ApplicantChiFirstName;
            orgMaster.ApplicantChiLastName = model.ApplicantChiLastName;
            orgMaster.ApplicantPosition = model.ApplicantPosition;
            orgMaster.ApplicantTelNum = model.ApplicantTelNum;
            orgMaster.PresidentName = model.President;
            orgMaster.SecretaryName = model.Secretary;
            orgMaster.TreasurerName = model.Treasurer;
            orgMaster.OrgObjective = model.Objectives;
            orgMaster.SubventedIndicator = model.Subvented;
            orgMaster.Section88Indicator = model.Section88;
            orgMaster.Section88StartDate = model.Section88Date;
            orgMaster.RegistrationType1 = model.RegistrationType1;
            orgMaster.RegistrationOtherName1 = model.RegistrationOtherName1;
            orgMaster.RegistrationDate1 = model.RegistrationDate1;
            orgMaster.RegistrationType2 = model.RegistrationType2;
            orgMaster.RegistrationOtherName2 = model.RegistrationOtherName2;
            orgMaster.RegistrationDate2 = model.RegistrationDate2;

            orgMaster.AddressProofIndicator = model.AddressProofIndicator;
            orgMaster.AddressProofDate = model.AddressProofDate;
            orgMaster.MaaConstitutionIndicator = model.MaaConstitutionIndicator;

            orgMaster.QualifiedOpinionIndicator = model.QualifiedOpinionIndicator;
            orgMaster.OtherSupportDocIndicator = model.OtherSupportDocIndicator;
            orgMaster.QualifiedOpinionRemark = model.QualifiedOpinionRemark;
            orgMaster.OtherSupportDocRemark = model.OtherSupportDocRemark;
            orgMaster.OverallRemark = model.OverallRemark;
            orgMaster.RowVersion = model.RowVersion;

            using (_unitOfWork.BeginTransaction())
            {
                if (model.AddressProofAttachmentId != null)
                {
                    string fileDescription = string.Format("Address Proof File as of {0}", CommonHelper.ConvertDateTimeToString(orgMaster.CreatedOn));
                    if (orgMaster.AddressProofAttachmentId == null)
                    {
                        var orgAttachment = SaveOrgAttachmentFile(model.AddressProofAttachmentId, orgMaster, fileDescription);
                        orgMaster.AddressProofAttachmentId = orgAttachment.OrgAttachmentId;
                    }
                    else
                    {
                        var orgAttachment = _orgAttachmentService.GetOrgAttachmentById(orgMaster.AddressProofAttachmentId.Value);
                        UpdateOrgAttachmentFile(model.AddressProofAttachmentId, orgAttachment, fileDescription);
                    }
                }
                if (model.MaaConstitutionAttachmentId != null)
                {
                    string fileDescription = string.Format("MAA / Consitution File as of {0}", CommonHelper.ConvertDateTimeToString(orgMaster.CreatedOn));
                    if (orgMaster.MaaConstitutionAttachmentId == null)
                    {
                        var orgAttachment = SaveOrgAttachmentFile(model.MaaConstitutionAttachmentId, orgMaster, fileDescription);
                        orgMaster.MaaConstitutionAttachmentId = orgAttachment.OrgAttachmentId;
                    }
                    else
                    {
                        var orgAttachment = _orgAttachmentService.GetOrgAttachmentById(orgMaster.MaaConstitutionAttachmentId.Value);
                        UpdateOrgAttachmentFile(model.MaaConstitutionAttachmentId, orgAttachment, fileDescription);
                    }
                }
                if (model.OtherSupportDocAttachmentId != null)
                {
                    string remark = "";
                    if (!String.IsNullOrEmpty(model.OtherSupportDocRemark))
                    {
                        remark = model.OtherSupportDocRemark.Length > 150 ? model.OtherSupportDocRemark.Substring(0, 150) : model.OtherSupportDocRemark;
                    }
                    string fileDescription = string.Format("Other Support Document - {0}, as of {1}", remark, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (orgMaster.OtherSupportDocAttachmentId == null)
                    {
                        var orgAttachment = SaveOrgAttachmentFile(model.OtherSupportDocAttachmentId, orgMaster, fileDescription);
                        orgMaster.OtherSupportDocAttachmentId = orgAttachment.OrgAttachmentId;
                    }
                    else
                    {
                        var orgAttachment = _orgAttachmentService.GetOrgAttachmentById(orgMaster.OtherSupportDocAttachmentId.Value);
                        UpdateOrgAttachmentFile(model.OtherSupportDocAttachmentId, orgAttachment, fileDescription);
                    }
                }
                _organisationService.UpdateOrgMaster(oldOrgMaster, orgMaster);
                if (engOrgName != orgMaster.EngOrgName || chiOrgName != orgMaster.ChiOrgName)
                {
                    InsertOrgNameChangeHistory(orgMaster);
                }
                _unitOfWork.Commit();
            }

            WithHoldingDate WithHoldingDate = _withholdingHistoryService.GetWithholdingDateByOrgId(orgMaster.OrgId);
            var orgEditLatestPspFd = _orgEditLatestPspFdViewRepository.GetById(orgMaster.OrgId);

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = new
                {   
                    PspRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspRef : "",
                    PspContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonName : "",
                    PspContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.PspContactPersonEmailAddress : "",
                    
                    FdRef = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdRef : "",
                    FdContactPersonName = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonName : "",
                    FdContactPersonEmailAddress = orgEditLatestPspFd != null ? orgEditLatestPspFd.FdContactPersonEmailAddress : "",
                    
                    WithholdingBeginDate = WithHoldingDate.WithholdingBeginDate,
                    WithholdingEndDate = WithHoldingDate.WithholdingEndDate,

                    OrganisationDisabled = orgMaster.DisableIndicator,
                    withholdingIndicator = this._withholdingHistoryService.GetWithHoldBySysDt(orgMaster.OrgId),

                    RowVersion = orgMaster.RowVersion
                },
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/OrgMaster/{code}/getOrgMaster", Name = "GetOrgMaster")]
        public JsonResult GetOrgMaster(string orgId)
        {
            Ensure.Argument.NotNullOrEmpty(orgId);

            var orgMaster = _organisationService.GetOrgById(Convert.ToInt32(orgId));
            //System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            //dtFormat.ShortDatePattern = "yyyy/MM/dd";

            return Json(new JsonResponse(true)
            {
                Data = new OrganisationViewModel
                {
                    OrganisationReference = orgMaster.OrgRef,
                    OrganisationDisabled = orgMaster.DisableIndicator,
                    OrganisationName = orgMaster.EngOrgName,
                    OrganisationChiName = orgMaster.ChiOrgName,
                    OrganisationSimpliChiName = orgMaster.SimpChiOrgName,
                    EngOrgNameSorting = orgMaster.EngOrgNameSorting,
                    OtherEngOrgName = orgMaster.OtherEngOrgName,
                    OtherChiOrgName = orgMaster.OtherChiOrgName,
                    OtherSimpChiOrgName = orgMaster.OtherSimpChiOrgName,
                    EngRegisteredAddress1 = orgMaster.EngRegisteredAddress1,
                    EngRegisteredAddress2 = orgMaster.EngRegisteredAddress2,
                    EngRegisteredAddress3 = orgMaster.EngRegisteredAddress3,
                    EngRegisteredAddress4 = orgMaster.EngRegisteredAddress4,
                    EngRegisteredAddress5 = orgMaster.EngRegisteredAddress5,
                    ChiRegisteredAddress1 = orgMaster.ChiRegisteredAddress1,
                    ChiRegisteredAddress2 = orgMaster.ChiRegisteredAddress2,
                    ChiRegisteredAddress3 = orgMaster.ChiRegisteredAddress3,
                    ChiRegisteredAddress4 = orgMaster.ChiRegisteredAddress4,
                    ChiRegisteredAddress5 = orgMaster.ChiRegisteredAddress5,
                    EngMailingAddress1 = orgMaster.EngMailingAddress1,
                    EngMailingAddress2 = orgMaster.EngMailingAddress2,
                    EngMailingAddress3 = orgMaster.EngMailingAddress3,
                    EngMailingAddress4 = orgMaster.EngMailingAddress4,
                    EngMailingAddress5 = orgMaster.EngMailingAddress5,
                    ChiMailingAddress1 = orgMaster.ChiMailingAddress1,
                    ChiMailingAddress2 = orgMaster.ChiMailingAddress2,
                    ChiMailingAddress3 = orgMaster.ChiMailingAddress3,
                    ChiMailingAddress4 = orgMaster.ChiMailingAddress4,
                    ChiMailingAddress5 = orgMaster.ChiMailingAddress5,
                    OrganisationWebsite = orgMaster.URL,
                    TelNum = orgMaster.TelNum,
                    Fax = orgMaster.FaxNum,
                    Email = orgMaster.EmailAddress,
                    ApplicantSaluteId = orgMaster.ApplicantSalute,
                    ApplicantFirstName = orgMaster.ApplicantFirstName,
                    ApplicantLastName = orgMaster.ApplicantLastName,
                    ApplicantChiFirstName = orgMaster.ApplicantChiFirstName,
                    ApplicantChiLastName = orgMaster.ApplicantChiLastName,
                    ApplicantPosition = orgMaster.ApplicantPosition,
                    ApplicantTelNum = orgMaster.ApplicantTelNum,
                    President = orgMaster.PresidentName,
                    Secretary = orgMaster.SecretaryName,
                    Treasurer = orgMaster.TreasurerName,
                    Objectives = orgMaster.OrgObjective,
                    Subvented = orgMaster.SubventedIndicator,
                    Section88 = Convert.ToBoolean(orgMaster.Section88Indicator),
                    Section88Date = orgMaster.Section88StartDate,
                    RegistrationType1 = orgMaster.RegistrationType1,
                    RegistrationOtherName1 = orgMaster.RegistrationOtherName1,
                    RegistrationDate1 = orgMaster.RegistrationDate1,
                    RegistrationType2 = orgMaster.RegistrationType2,
                    RegistrationOtherName2 = orgMaster.RegistrationOtherName2,
                    RegistrationDate2 = orgMaster.RegistrationDate2,
                    AddressProofIndicator = Convert.ToBoolean(orgMaster.AddressProofIndicator),
                    AddressProofDate = orgMaster.AddressProofDate,
                    MaaConstitutionIndicator = Convert.ToBoolean(orgMaster.MaaConstitutionIndicator),
                    QualifiedOpinionIndicator = Convert.ToBoolean(orgMaster.QualifiedOpinionIndicator),
                    OtherSupportDocIndicator = Convert.ToBoolean(orgMaster.OtherSupportDocIndicator),
                    QualifiedOpinionRemark = orgMaster.QualifiedOpinionRemark,
                    OtherSupportDocRemark = orgMaster.OtherSupportDocRemark,
                    OverallRemark = orgMaster.OverallRemark,
                    withholdingIndicator = this._withholdingHistoryService.GetWithHoldBySysDt(orgMaster.OrgId),
                    RowVersion = orgMaster.RowVersion
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/OrgMaster/{orgMasterId:int}/deleteOrgMaster", Name = "DeleteOrgMaster")]
        public JsonResult DeleteOrgMaster(int orgMasterId)
        {
            var orgMaster = _organisationService.GetOrgById(orgMasterId);
            Ensure.NotNull(orgMaster, "No Org Master found with the specified id");

            using (_unitOfWork.BeginTransaction())
            {
                _organisationService.Delete(orgMaster);
                _unitOfWork.Commit();
            }
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted)
            }, JsonRequestBehavior.DenyGet);
        }

        private void InsertOrgNameChangeHistory(OrgMaster orgMaster)
        {
            int maxid = 0;
            if (orgMaster.OrgId == 0)
            {
                maxid = _organisationService.GetLeastOrgMaster();
            }
            else
            {
                maxid = orgMaster.OrgId;
            }
            OrgNameChangeHistory onch = new OrgNameChangeHistory();
            onch.ChiOrgName = orgMaster.ChiOrgName;
            onch.EngOrgName = orgMaster.EngOrgName;
            //onch.SimpChiOrgName = orgMaster.SimpChiOrgName;
            onch.ChangeDate = DateTime.Now;
            onch.OrgNameChangeId = maxid;
            onch.OrgMaster = orgMaster;
            _orgNameChangeHistoryService.CreateOrgNameChangeHistory(onch);
        }

        private OrgAttachment SaveOrgAttachmentFile(HttpPostedFileBase file, OrgMaster orgMaster, string fileDescription)
        {
            var LetterPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);
            var fileName = Path.GetFileName(file.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string filePath = Path.Combine(LetterPath.Value, string.Format("{0}-{1}", time, fileName));
            var orgAttachment = new OrgAttachment();
            orgAttachment.FileName = fileName;
            orgAttachment.FileLocation = filePath;
            orgAttachment.FileDescription = fileDescription;
            orgAttachment.OrgMaster = orgMaster;
            _orgAttachmentService.CreateOrgAttachment(orgAttachment);
            if (CommonHelper.CreateFolderIfNeeded(LetterPath.Value))
            {
                file.SaveAs(filePath);
            }
            return orgAttachment;
        }

        private void UpdateOrgAttachmentFile(HttpPostedFileBase file, OrgAttachment orgAttachment, string fileDescription)
        {
            if (System.IO.File.Exists(orgAttachment.FileLocation))
            {
                System.IO.File.Delete(orgAttachment.FileLocation);
            }
            var LetterPath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_ATTACHMENT_PATH);
            var fileName = Path.GetFileName(file.FileName);
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string filePath = Path.Combine(LetterPath.Value, string.Format("{0}-{1}", time, fileName));
            orgAttachment.FileName = fileName;
            orgAttachment.FileLocation = filePath;
            orgAttachment.FileDescription = fileDescription;
            if (CommonHelper.CreateFolderIfNeeded(LetterPath.Value))
            {
                file.SaveAs(filePath);
            }
            _orgAttachmentService.UpdateOrgAttachment(orgAttachment);
        }

        #endregion OrgMaster Action

        #region Reference Guide

        public PartialViewResult RenderImportRefGuideXlsFileModal()
        {
            return PartialView("_ImportRefGuideXlsFileModal");
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/Organisation/ReferenceGuide")]        
        public ActionResult ReferenceGuide()
        {
            OrganisationViewModel model = new OrganisationViewModel();
            model.isFirstSearch = true;
            this.HttpContext.Session[OrgRefGuideSearchSessionName] = null;

            initOrgRefGuideViewModel(model, 0);

            return View(model);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/Organisation/ReferenceGuide/New/{orgId}", Name = "NewOrgRefGuide")]
        [RuleSetForClientSideMessagesAttribute("default", "ReferenceGuideInsert")]
        public ActionResult NewReferenceGuide(int orgId)
        {
            var orgMaster = _organisationService.GetOrgById(orgId);
            String strReferrer = f_getReferrerUrl();

            var model = new OrganisationViewModel
            {
                OrgMasterId = orgMaster.OrgId.ToString(),
                OrganisationReference = orgMaster.OrgRef,
                OrganisationDisabled = orgMaster.DisableIndicator,
                OrganisationName = orgMaster.EngOrgName,
                OrganisationChiName = orgMaster.ChiOrgName,
                OrganisationSimpliChiName = orgMaster.SimpChiOrgName,
                EngOrgNameSorting = orgMaster.EngOrgNameSorting,
                OtherEngOrgName = orgMaster.OtherEngOrgName,
                OtherChiOrgName = orgMaster.OtherChiOrgName,
                OtherSimpChiOrgName = orgMaster.OtherSimpChiOrgName,
                EngRegisteredAddress1 = orgMaster.EngRegisteredAddress1,
                EngRegisteredAddress2 = orgMaster.EngRegisteredAddress2,
                EngRegisteredAddress3 = orgMaster.EngRegisteredAddress3,
                EngRegisteredAddress4 = orgMaster.EngRegisteredAddress4,
                EngRegisteredAddress5 = orgMaster.EngRegisteredAddress5,
                ChiRegisteredAddress1 = orgMaster.ChiRegisteredAddress1,
                ChiRegisteredAddress2 = orgMaster.ChiRegisteredAddress2,
                ChiRegisteredAddress3 = orgMaster.ChiRegisteredAddress3,
                ChiRegisteredAddress4 = orgMaster.ChiRegisteredAddress4,
                ChiRegisteredAddress5 = orgMaster.ChiRegisteredAddress5,
                EngMailingAddress1 = orgMaster.EngMailingAddress1,
                EngMailingAddress2 = orgMaster.EngMailingAddress2,
                EngMailingAddress3 = orgMaster.EngMailingAddress3,
                EngMailingAddress4 = orgMaster.EngMailingAddress4,
                EngMailingAddress5 = orgMaster.EngMailingAddress5,
                ChiMailingAddress1 = orgMaster.ChiMailingAddress1,
                ChiMailingAddress2 = orgMaster.ChiMailingAddress2,
                ChiMailingAddress3 = orgMaster.ChiMailingAddress3,
                ChiMailingAddress4 = orgMaster.ChiMailingAddress4,
                ChiMailingAddress5 = orgMaster.ChiMailingAddress5,
                OrganisationWebsite = orgMaster.URL,
                TelNum = orgMaster.TelNum,
                Fax = orgMaster.FaxNum,
                Email = orgMaster.EmailAddress,
                ApplicantSaluteId = orgMaster.ApplicantSalute,
                ApplicantFirstName = orgMaster.ApplicantFirstName,
                ApplicantLastName = orgMaster.ApplicantLastName,
                ApplicantChiFirstName = orgMaster.ApplicantChiFirstName,
                ApplicantChiLastName = orgMaster.ApplicantChiLastName,
                ApplicantPosition = orgMaster.ApplicantPosition,
                ApplicantTelNum = orgMaster.ApplicantTelNum,
                President = orgMaster.PresidentName,
                Secretary = orgMaster.SecretaryName,
                Treasurer = orgMaster.TreasurerName,
                Objectives = orgMaster.OrgObjective,
                Subvented = orgMaster.SubventedIndicator,
                Section88 = Convert.ToBoolean(orgMaster.Section88Indicator),
                Section88Date = orgMaster.Section88StartDate,
                RegistrationType1 = orgMaster.RegistrationType1,
                RegistrationOtherName1 = orgMaster.RegistrationOtherName1,
                RegistrationDate1 = orgMaster.RegistrationDate1,
                RegistrationType2 = orgMaster.RegistrationType2,
                RegistrationOtherName2 = orgMaster.RegistrationOtherName2,
                RegistrationDate2 = orgMaster.RegistrationDate2,
                ReplyFromId = (Url.RouteUrl("OrgSearch") == Request.UrlReferrer.AbsolutePath) ? "5" : String.Empty,
                PrePage = strReferrer
            };

            initOrgRefGuideViewModel(model, 2);

            return View("Detail", model);
        }

        public String f_getReferrerUrl()
        {
            String strReferrer = "";
            Uri referrer = Request.UrlReferrer;

            if (referrer != null)
            {
                strReferrer = referrer.ToString();
            }

            return strReferrer;
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/Organisation/ReferenceGuide/{orgRefGuidePromulgationId}", Name = "EditOrgGuide")]
        [RuleSetForClientSideMessagesAttribute("default", "ReferenceGuideUpdate")]
        public ActionResult Detail(int orgRefGuidePromulgationId)
        {
            var orgRefGuidePromulgation = _orgRefGuidePromulgationService.GetOrgRefGuidePromulgationByOrgRefGuidePromulgationId(orgRefGuidePromulgationId);
            var orgMaster = orgRefGuidePromulgation.OrgMaster;
            String strReferrer = f_getReferrerUrl();

            var model = new OrganisationViewModel
            {
                OrgMasterId = orgRefGuidePromulgation.OrgMaster.OrgId.ToString(),
                OrganisationReference = orgMaster.OrgRef,
                OrganisationDisabled = orgMaster.DisableIndicator,
                OrganisationName = orgMaster.EngOrgName,
                OrganisationChiName = orgMaster.ChiOrgName,
                OrganisationSimpliChiName = orgMaster.SimpChiOrgName,
                EngOrgNameSorting = orgMaster.EngOrgNameSorting,
                OtherEngOrgName = orgMaster.OtherEngOrgName,
                OtherChiOrgName = orgMaster.OtherChiOrgName,
                OtherSimpChiOrgName = orgMaster.OtherSimpChiOrgName,
                EngRegisteredAddress1 = orgMaster.EngRegisteredAddress1,
                EngRegisteredAddress2 = orgMaster.EngRegisteredAddress2,
                EngRegisteredAddress3 = orgMaster.EngRegisteredAddress3,
                EngRegisteredAddress4 = orgMaster.EngRegisteredAddress4,
                EngRegisteredAddress5 = orgMaster.EngRegisteredAddress5,
                ChiRegisteredAddress1 = orgMaster.ChiRegisteredAddress1,
                ChiRegisteredAddress2 = orgMaster.ChiRegisteredAddress2,
                ChiRegisteredAddress3 = orgMaster.ChiRegisteredAddress3,
                ChiRegisteredAddress4 = orgMaster.ChiRegisteredAddress4,
                ChiRegisteredAddress5 = orgMaster.ChiRegisteredAddress5,
                EngMailingAddress1 = orgMaster.EngMailingAddress1,
                EngMailingAddress2 = orgMaster.EngMailingAddress2,
                EngMailingAddress3 = orgMaster.EngMailingAddress3,
                EngMailingAddress4 = orgMaster.EngMailingAddress4,
                EngMailingAddress5 = orgMaster.EngMailingAddress5,
                ChiMailingAddress1 = orgMaster.ChiMailingAddress1,
                ChiMailingAddress2 = orgMaster.ChiMailingAddress2,
                ChiMailingAddress3 = orgMaster.ChiMailingAddress3,
                ChiMailingAddress4 = orgMaster.ChiMailingAddress4,
                ChiMailingAddress5 = orgMaster.ChiMailingAddress5,
                OrganisationWebsite = orgMaster.URL,
                TelNum = orgMaster.TelNum,
                Fax = orgMaster.FaxNum,
                Email = orgMaster.EmailAddress,
                ApplicantSaluteId = orgMaster.ApplicantSalute,
                ApplicantFirstName = orgMaster.ApplicantFirstName,
                ApplicantLastName = orgMaster.ApplicantLastName,
                ApplicantChiFirstName = orgMaster.ApplicantChiFirstName,
                ApplicantChiLastName = orgMaster.ApplicantChiLastName,
                ApplicantPosition = orgMaster.ApplicantPosition,
                ApplicantTelNum = orgMaster.ApplicantTelNum,
                President = orgMaster.PresidentName,
                Secretary = orgMaster.SecretaryName,
                Treasurer = orgMaster.TreasurerName,
                Objectives = orgMaster.OrgObjective,
                Subvented = orgMaster.SubventedIndicator,
                Section88 = Convert.ToBoolean(orgMaster.Section88Indicator),
                Section88Date = orgMaster.Section88StartDate,
                RegistrationType1 = orgMaster.RegistrationType1,
                RegistrationOtherName1 = orgMaster.RegistrationOtherName1,
                RegistrationDate1 = orgMaster.RegistrationDate1,
                RegistrationType2 = orgMaster.RegistrationType2,
                RegistrationOtherName2 = orgMaster.RegistrationOtherName2,
                RegistrationDate2 = orgMaster.RegistrationDate2,
                SendDate = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.SendDate : null,
                ReplySlipReceiveDate = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.ReplySlipReceiveDate : null,
                ReplySlipDate = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.ReplySlipDate : null,
                LanguageUsedId = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.LanguageUsed : null,
                ReplyFromId = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.OrgReply : "",

                PromulgationReason = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.PromulgationReason : "",
                PartNum = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.PartNum : "",
                EnclosureNum = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.EnclosureNum : "",

                ReplySlipPartNum = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.ReplySlipPartNum : "",
                ReplySlipEnclosureNum = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.ReplySlipEnclosureNum : "",

                Remarks = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.Remarks : "",

                PrePage = strReferrer,
                ReferenceGuideActivityConcern = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.ActivityConcern : null,
                FileRef = orgRefGuidePromulgation != null ? orgRefGuidePromulgation.FileRef : ""
            };

            initOrgRefGuideViewModel(model, 2);

            model.isFirstSearch = true;

            var list = _orgProvisionNotAdoptService.GetAllByOrgRefGuidePromulgationId(orgRefGuidePromulgation.OrgRefGuidePromulgationId);
            model.ProvisionsNotBeAdopted = list.Select(x => x.ProvisionId).ToArray();

            return View("Detail", model);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpGet, Route("~/Organisation/ReturnReferenceGuide", Name = "ReturnSearchOrgGuide")]
        public ActionResult ReturnSearchOrgGuide()
        {
            OrganisationViewModel model = new OrganisationViewModel();

            if (this.HttpContext.Session[OrgRefGuideSearchSessionName] != null)
            {
                model = ((OrganisationViewModel)(this.HttpContext.Session[OrgRefGuideSearchSessionName]));
                model.isFirstSearch = false;
            }

            initOrgRefGuideViewModel(model, 0);

            return View("ReferenceGuide", model);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/listOrgGuide", Name = "ListOrgGuide")]
        public JsonResult ListOrgGuide(GridSettings grid, OrganisationViewModel model)
        {
            grid = GetReferenceGuideSearchGrid(model, grid);

            var org =
                _organisationService.GetPageByReferenceGuideSearchView(grid,
                                        model.AppliedPSPBeforeId, model.FromPspApplicationDate, model.ToPspApplicationDate, 
                                        model.PSPIssuedBeforeId, model.FromPspPermitIssueDate, model.ToPspPermitIssueDate, 
                                        model.AppliedFDBeforeFdYear, model.FdIssuedBeforeFdYear,
                                        model.AppliedFDBeforeId, model.FDIssuedBeforeId,
                                        model.AppliedSSAFBeforeId, model.FromSSAFApplicationDate, model.ToSSAFApplicationDate,
                                        model.SSAFIssuedBeforeId, model.FromSSAFPermitIssueDate, model.ToSSAFPermitIssueDate,
                                        model.ReferenceGuideActivityConcern);

            this.HttpContext.Session[OrgRefGuideSearchSessionName] = model;

            var gridResult = new GridResult
            {
                TotalPages = org.TotalPages,
                CurrentPageIndex = org.CurrentPageIndex,
                TotalCount = org.TotalCount,
                Data = (from p in org
                        select new
                        {
                            RowNumber = p.RowNumber,
                            OrgRefGuidePromulgationId = p.OrgRefGuidePromulgationId,
                            OrgMasterId = p.OrgId,
                            OrgRef = p.OrgRef,
                            EngOrgNameSorting = p.EngOrgNameSorting + "<br>" + p.ChiOrgName,
                            EngOrgName = p.EngOrgName,
                            ChiOrgName = p.ChiOrgName,
                            SendDate = p.SendDate,
                            OrgReply = !String.IsNullOrEmpty(p.OrgReply) ? _lookupService.GetDescription(LookupType.OrgReply, p.OrgReply) : "",
                            ReplySlipReceiveDate = p.ReplySlipReceiveDate,
                            EngMailingAddress1 = p.EngMailingAddress1,
                            EmailAddress = p.EmailAddress,
                            EngMailingAddress = p.EngMailingAddress1 + "<br>" + p.EngMailingAddress2,
                            ChiMailingAddress = p.ChiMailingAddress1 + "<br>" + p.ChiMailingAddress2,
                            ContactPerson = p.ContactPerson,
                            ContactPersonEmail = p.EmailAddress,
                            OrgEmailAddress = p.EmailAddress,
                            PartNum = p.PartNum,
                            EnclosureNum = p.EnclosureNum,
                            ReplySlipPartNum = p.ReplySlipPartNum,
                            ReplySlipEnclosureNum = p.ReplySlipEnclosureNum,

                            FileRef = p.FileRef,
                            Approved = p.Approved,
                            ActivityConcern = p.ActivityConcern,
                            ActivityConcernDesc = p.ActivityConcernDesc,
                            OrgProvisionNotAdopts = p.OrgProvisionNotAdopts,
                            PromulgationReason = p.PromulgationReason,
                            Remarks = p.Remarks
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/exportReferenceGuide", Name = "ExportReferenceGuide")]
        public JsonResult ExportReferenceGuide(ExportSettings exportSettings)
        {
            OrganisationViewModel model = ((OrganisationViewModel)this.HttpContext.Session[OrgRefGuideSearchSessionName]);
            if (model != null)
            {
                exportSettings.GridSettings = GetReferenceGuideSearchGrid(model, exportSettings.GridSettings);
            }
            var org = _organisationService.GetPageByReferenceGuideSearchView(exportSettings.GridSettings,
                                        model.AppliedPSPBeforeId, model.FromPspApplicationDate, model.ToPspApplicationDate,
                                        model.PSPIssuedBeforeId, model.FromPspPermitIssueDate, model.ToPspPermitIssueDate,
                                        model.AppliedFDBeforeFdYear, model.FdIssuedBeforeFdYear,
                                        model.AppliedFDBeforeId, model.FDIssuedBeforeId,
                                        model.AppliedSSAFBeforeId, model.FromSSAFApplicationDate, model.ToSSAFApplicationDate,
                                        model.SSAFIssuedBeforeId, model.FromSSAFPermitIssueDate, model.ToSSAFPermitIssueDate,
                                        model.ReferenceGuideActivityConcern);

            var dataList = (from p in org
                            select new
                            {
                                OrgRefGuidePromulgationId = p.OrgRefGuidePromulgationId,
                                OrgMasterId = p.Id,
                                OrgRef = p.OrgRef,
                                EngOrgNameSorting = p.EngOrgNameSorting + Environment.NewLine + p.ChiOrgName,
                                EngOrgName = p.EngOrgName,
                                ChiOrgName = p.ChiOrgName,
                                SendDate = p.SendDate,
                                OrgReply = !String.IsNullOrEmpty(p.OrgReply) ? _lookupService.GetDescription(LookupType.OrgReply, p.OrgReply) : "",
                                ReplySlipReceiveDate = p.ReplySlipReceiveDate == null ? "" : p.ReplySlipReceiveDate.Value.ToString("dd/MM/yyyy"),
                                EngMailingAddress1 = p.EngMailingAddress1,
                                EmailAddress = p.EmailAddress,
                                EngMailingAddress = String.Join(Environment.NewLine, new String[] {p.EngMailingAddress1, p.EngMailingAddress2, p.EngMailingAddress3, p.EngMailingAddress4, p.EngMailingAddress5 }),
                                ChiMailingAddress = String.Join(Environment.NewLine, new String[] {p.ChiMailingAddress1, p.ChiMailingAddress2, p.ChiMailingAddress3, p.ChiMailingAddress4, p.ChiMailingAddress5 }),
                                ContactPerson = p.ContactPerson,
                                ContactPersonEmail = p.EmailAddress,
                                OrgEmailAddress = p.EmailAddress,
                                PartNum = p.PartNum,
                                EnclosureNum = p.EnclosureNum,
                                ReplySlipPartNum = p.ReplySlipPartNum,
                                ReplySlipEnclosureNum = p.ReplySlipEnclosureNum,

                                FileRef = p.FileRef,
                                Approved = p.Approved,
                                ActivityConcern = p.ActivityConcern,
                                ActivityConcernDesc = p.ActivityConcernDesc,
                                OrgProvisionNotAdopts = p.OrgProvisionNotAdopts,
                                PromulgationReason = p.PromulgationReason,
                                Remarks = p.Remarks
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            string tmpVal = "";
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<OrganisationViewModel>();

            if (model.OrganisationReference.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationReference"] + " : ORG" + model.OrganisationReference);

            if (model.OrganisationName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationName"] + " : " + model.OrganisationName);

            if (model.OrganisationStatusId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrganisationStatusId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrganisationStatusId]);

            if (model.SubventedId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SubventedId"] + " : " + (model.SubventedId == "0" ? "False" : "True"));
            }

            if (model.RegistrationTitleId.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationTitleId];
                if (model.RegistrationTitleId == "Others")
                {
                    if (model.Registration.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc + " ( " + model.Registration + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationTitleId"] + " : " + tempDesc);
                }
            }

            if (model.SectionId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SectionId"] + " : " + (model.SectionId != "1" ? "False" : "True"));

            if (model.OrgReply != null)
            {
                string OrgReplyDescs = "";
                var lookup = this._lookupService.getAllLkpInCodec(LookupType.OrgReply);
                foreach (string OrgReplyId in model.OrgReply)
                {
                    string OrgReplyDesc = lookup[OrgReplyId];
                    OrgReplyDescs += OrgReplyDesc + ";";
                }
                filterCriterias.Add(fieldNames["OrgReply"] + " : " + OrgReplyDescs);
            }

            if (model.ReferenceGuideActivityConcern != null)
            {
                var lookup = _lookupService.getAllLkpInCodec(LookupType.ReferenceGuideActivityConcern);
                var activityConcern = lookup.FirstOrDefault(o => o.Key.Equals(model.ReferenceGuideActivityConcern));
                
                filterCriterias.Add(fieldNames["ReferenceGuideActivityConcern"] + " : " + activityConcern != null ? activityConcern.Value : "");
            }

            if (model.ReplySlipReceiveDateStart != null && model.ReplySlipReceiveDateEnd != null)
            {
                tmpVal = "From " + model.ReplySlipReceiveDateStart.Value.ToString(DATE_FORMAT) + " to " + model.ReplySlipReceiveDateEnd.Value.ToString(DATE_FORMAT);
            }
            else if (model.ReplySlipReceiveDateStart != null)
            {
                tmpVal = "From " + model.ReplySlipReceiveDateStart.Value.ToString(DATE_FORMAT);
            }
            else if (model.ReplySlipReceiveDateEnd != null)
            {
                tmpVal = "To " + model.ReplySlipReceiveDateEnd.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["ReplySlipReceiveDateStart"] + " : " + tmpVal);
            
            tmpVal = "";
            if (model.SendDateStart != null && model.SendDateEnd != null)
            {
                tmpVal = "From " + model.SendDateStart.Value.ToString(DATE_FORMAT) + " to " + model.SendDateEnd.Value.ToString(DATE_FORMAT);
            }
            else if (model.SendDateStart != null)
            {
                tmpVal = "From " + model.SendDateStart.Value.ToString(DATE_FORMAT);
            }
            else if (model.SendDateEnd != null)
            {
                tmpVal = "To " + model.SendDateEnd.Value.ToString(DATE_FORMAT);
            }
            if (tmpVal != "") filterCriterias.Add(fieldNames["SendDateStart"] + " : " + tmpVal);

            tmpVal = "";
            if (model.AppliedPSPBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedPSPBeforeId"] + " : " + (model.AppliedPSPBeforeId == "0" ? "False" : "True"));
                if (model.FromPspApplicationDate.HasValue && model.ToPspApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspApplicationDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromPspApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToPspApplicationDate.HasValue)
                {
                    tmpVal = "To " + model.ToPspApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                if (tmpVal != "") filterCriterias.Add(fieldNames["AppliedPSPBeforeId"] + " : " + tmpVal);
            }

            tmpVal = "";
            if (model.AppliedSSAFBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedSSAFBeforeId"] + " : " + (model.AppliedSSAFBeforeId == "0" ? "False" : "True"));
                if (model.FromSSAFApplicationDate.HasValue && model.ToSSAFApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFApplicationDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromSSAFApplicationDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToSSAFApplicationDate.HasValue)
                {
                    tmpVal = "To " + model.ToSSAFApplicationDate.Value.ToString("yyyy-MM-dd");
                }
                if (tmpVal != "") filterCriterias.Add(fieldNames["AppliedSSAFBeforeId"] + " : " + tmpVal);
            }

            tmpVal = "";
            if (model.AppliedFDBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["AppliedFDBeforeId"] + " : " + (model.AppliedFDBeforeId == "0" ? "False" : "True"));

                if (model.AppliedFDBeforeFdYear != null)
                {
                    string FdYearDescs = "";
                    foreach (string FdYearId in model.AppliedFDBeforeFdYear)
                    {
                        string FdYearDesc = FdYearId;
                        FdYearDescs += FdYearDesc + ";";
                    }
                    filterCriterias.Add(fieldNames["AppliedFDBeforeFdYear"] + " : " + FdYearDescs);
                }
            }

            tmpVal = "";
            if (model.PSPIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["PSPIssuedBeforeId"] + " : " + (model.PSPIssuedBeforeId == "0" ? "False" : "True"));
                if (model.FromPspPermitIssueDate.HasValue && model.ToPspPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspPermitIssueDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromPspPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToPspPermitIssueDate.HasValue)
                {
                    tmpVal = "To " + model.ToPspPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                //PIR: 
                //if (tmpVal != "") filterCriterias.Add(fieldNames["FromPspPermitIssueDate"] + " : " + tmpVal);
                if (tmpVal != "") filterCriterias.Add(tmpVal);
            }

            tmpVal = "";
            if (model.SSAFIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SSAFIssuedBeforeId"] + " : " + (model.SSAFIssuedBeforeId == "0" ? "False" : "True"));
                if (model.FromSSAFPermitIssueDate.HasValue && model.ToSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd") + " to " + model.ToSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.FromSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "From " + model.FromSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }
                else if (model.ToSSAFPermitIssueDate.HasValue)
                {
                    tmpVal = "To " + model.ToSSAFPermitIssueDate.Value.ToString("yyyy-MM-dd");
                }

                if (tmpVal != "") filterCriterias.Add(tmpVal);
            }

            if (model.FDIssuedBeforeId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["FDIssuedBeforeId"] + " : " + (model.FDIssuedBeforeId == "0" ? "False" : "True"));
                if (model.FdIssuedBeforeFdYear != null)
                {
                    string FdYearDescs = "";
                    foreach (string FdYearId in model.FdIssuedBeforeFdYear)
                    {
                        string FdYearDesc = FdYearId;
                        FdYearDescs += FdYearDesc + ";";
                    }
                    filterCriterias.Add(fieldNames["FdIssuedBeforeFdYear"] + " : " + FdYearDescs);
                }
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/org/exportRec", Name = "ExportRec")]
        public JsonResult ExportRec(ExportSettings exportSettings)
        {
            OrganisationViewModel model = ((OrganisationViewModel)this.HttpContext.Session[OrgRefGuideSearchSessionName]);
            if (model != null)
            {
                exportSettings.GridSettings = GetReferenceGuideSearchGrid(model, exportSettings.GridSettings);
            }
            var org = _organisationService.GetPageByReferenceGuideSearchView(exportSettings.GridSettings,
                                        model.AppliedPSPBeforeId, model.FromPspApplicationDate, model.ToPspApplicationDate,
                                        model.PSPIssuedBeforeId, model.FromPspPermitIssueDate, model.ToPspPermitIssueDate,
                                        model.AppliedFDBeforeFdYear, model.FdIssuedBeforeFdYear,
                                        model.AppliedFDBeforeId, model.FDIssuedBeforeId,
                                        model.AppliedSSAFBeforeId, model.FromSSAFApplicationDate, model.ToSSAFApplicationDate,
                                        model.SSAFIssuedBeforeId, model.FromSSAFPermitIssueDate, model.ToSSAFPermitIssueDate,
                                        model.ReferenceGuideActivityConcern);

            var dataList = (from p in org
                            select new
                            {
                                OrgMasterId = p.Id,
                                OrgRef = p.OrgRef,
                                EngOrgNameSorting = p.EngOrgNameSorting + Environment.NewLine + p.ChiOrgName,
                                EngOrgName = p.EngOrgName,
                                ChiOrgName = p.ChiOrgName,
                                SendDate = p.SendDate,
                                OrgReply = !String.IsNullOrEmpty(p.OrgReply) ? _lookupService.GetDescription(LookupType.OrgReply, p.OrgReply) : "",
                                ReplySlipReceiveDate = p.ReplySlipReceiveDate == null ? "" : p.ReplySlipReceiveDate.Value.ToString("dd/MM/yyyy"),
                                EngMailingAddress1 = p.EngMailingAddress1,
                                EmailAddress = p.EmailAddress,
                                EngMailingAddress = p.EngMailingAddress1 + Environment.NewLine + p.EngMailingAddress2,
                                ChiMailingAddress = p.ChiMailingAddress1 + Environment.NewLine + p.ChiMailingAddress2,
                                ContactPerson = p.ContactPerson,
                                ContactPersonEmail = p.EmailAddress,
                                OrgEmailAddress = p.EmailAddress,
                                PartNum = p.PartNum,
                                EnclosureNum = p.EnclosureNum,
                                ReplySlipPartNum = p.ReplySlipPartNum,
                                ReplySlipEnclosureNum = p.ReplySlipEnclosureNum,

                                FileRef = p.FileRef,
                                ActivityConcern = p.ActivityConcern,
                                ActivityConcernDesc = p.ActivityConcernDesc,
                                OrgProvisionNotAdopts = p.OrgProvisionNotAdopts,
                                PromulgationReason = p.PromulgationReason,
                                Remarks = p.Remarks
                            }).ToArray();
            var start = 1;
            for (var i = start; i < exportSettings.ColumnModel.models.Count; i++)
            {
                if (exportSettings.ColumnModel.models[i].name != "orgRef" &&
                    exportSettings.ColumnModel.models[i].name != "engOrgNameSorting" &&
                    exportSettings.ColumnModel.models[i].name != "sendDate" &&
                    exportSettings.ColumnModel.models[i].name != "partNum" &&
                    exportSettings.ColumnModel.models[i].name != "enclosureNum" &&

                    exportSettings.ColumnModel.models[i].name != "fileRef" &&
                    exportSettings.ColumnModel.models[i].name != "activityConcern" &&
                    exportSettings.ColumnModel.models[i].name != "orgProvisionNotAdopts" &&
                    exportSettings.ColumnModel.models[i].name != "promulgationReason" &&
                    exportSettings.ColumnModel.models[i].name != "remarks")
                {
                    exportSettings.ColumnModel.models[i].hidedlg = true;
                }
                //else
                //{
                //    exportSettings.ColumnModel.models[i].hidedlg = false; //e.g. reset activityConcern field to be exportable
                //}

                if (exportSettings.ColumnModel.models[i].name == "sendDate")
                {
                    exportSettings.ColumnModel.models[i].formatter = "date_string";
                }
            }
            //exportSettings.ColumnModel.models[start - 2].hidedlg = true;
            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, null, false);
            
            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/ImportRefGuideXlsFile", Name = "ImportRefGuideXlsFile")]
        public JsonResult ImportRefGuideXlsFile(OrganisationViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (model.ImportFile == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = "Import file can not be empty.",
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            var fileName = model.ImportFile.FileName;
            string type = fileName.Substring(fileName.LastIndexOf(".") + 1);
            if (!type.ToLower().Equals("xlsx"))
            {
                return Json(new JsonResponse(false)
                {
                    Message = "File is not correct.",
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            string errorMsg = "";
            using (_unitOfWork.BeginTransaction())
            {
                errorMsg = _organisationService.ImportRefGuideXlsFile(model.ImportFile.InputStream);

                _unitOfWork.Commit();
            }
            if (!String.IsNullOrEmpty(errorMsg))
            {
                return Json(new JsonResponse(false)
                {
                    Message = CommonHelper.ConvertHtmlToString(errorMsg)
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
            else
            {
                var flagDayListYears = _flagDayListService.GetAllFlagDayListYearForDropdown();

                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Info.FileUploaded),
                    Data = flagDayListYears,
                }, "text/html", JsonRequestBehavior.DenyGet);
            }
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/getOrgGuide/{orgRefGuidePromulgationId}", Name = "GetOrgGuide")]
        public JsonResult GetOrgGuide(string orgRefGuidePromulgationId)
        {
            Ensure.Argument.NotNullOrEmpty(orgRefGuidePromulgationId);

            var orgGuide = _orgRefGuidePromulgationService.GetOrgRefGuidePromulgationByOrgRefGuidePromulgationId(Convert.ToInt32(orgRefGuidePromulgationId));
            var orgMaster = _organisationService.GetOrgById(orgGuide.OrgMaster.OrgId);

            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";

            return Json(new JsonResponse(true)
            {
                Data = new OrganisationViewModel
                {
                    OrganisationReference = orgMaster.OrgRef,
                    OrganisationDisabled = orgMaster.DisableIndicator,
                    OrganisationName = orgMaster.EngOrgName,
                    OrganisationChiName = orgMaster.ChiOrgName,
                    OrganisationSimpliChiName = orgMaster.SimpChiOrgName,
                    EngOrgNameSorting = orgMaster.EngOrgNameSorting,
                    OtherEngOrgName = orgMaster.OtherEngOrgName,
                    OtherChiOrgName = orgMaster.OtherChiOrgName,
                    OtherSimpChiOrgName = orgMaster.OtherSimpChiOrgName,
                    EngRegisteredAddress1 = orgMaster.EngRegisteredAddress1,
                    EngRegisteredAddress2 = orgMaster.EngRegisteredAddress2,
                    EngRegisteredAddress3 = orgMaster.EngRegisteredAddress3,
                    EngRegisteredAddress4 = orgMaster.EngRegisteredAddress4,
                    EngRegisteredAddress5 = orgMaster.EngRegisteredAddress5,
                    ChiRegisteredAddress1 = orgMaster.ChiRegisteredAddress1,
                    ChiRegisteredAddress2 = orgMaster.ChiRegisteredAddress2,
                    ChiRegisteredAddress3 = orgMaster.ChiRegisteredAddress3,
                    ChiRegisteredAddress4 = orgMaster.ChiRegisteredAddress4,
                    ChiRegisteredAddress5 = orgMaster.ChiRegisteredAddress5,
                    EngMailingAddress1 = orgMaster.EngMailingAddress1,
                    EngMailingAddress2 = orgMaster.EngMailingAddress2,
                    EngMailingAddress3 = orgMaster.EngMailingAddress3,
                    EngMailingAddress4 = orgMaster.EngMailingAddress4,
                    EngMailingAddress5 = orgMaster.EngMailingAddress5,
                    ChiMailingAddress1 = orgMaster.ChiMailingAddress1,
                    ChiMailingAddress2 = orgMaster.ChiMailingAddress2,
                    ChiMailingAddress3 = orgMaster.ChiMailingAddress3,
                    ChiMailingAddress4 = orgMaster.ChiMailingAddress4,
                    ChiMailingAddress5 = orgMaster.ChiMailingAddress5,
                    OrganisationWebsite = orgMaster.URL,
                    TelNum = orgMaster.TelNum,
                    Fax = orgMaster.FaxNum,
                    Email = orgMaster.EmailAddress,
                    ApplicantSaluteId = orgMaster.ApplicantSalute,
                    ApplicantFirstName = orgMaster.ApplicantFirstName,
                    ApplicantLastName = orgMaster.ApplicantLastName,
                    ApplicantChiFirstName = orgMaster.ApplicantChiFirstName,
                    ApplicantChiLastName = orgMaster.ApplicantChiLastName,
                    ApplicantPosition = orgMaster.ApplicantPosition,
                    ApplicantTelNum = orgMaster.ApplicantTelNum,
                    President = orgMaster.PresidentName,
                    Secretary = orgMaster.SecretaryName,
                    Treasurer = orgMaster.TreasurerName,
                    Objectives = orgMaster.OrgObjective,
                    Subvented = orgMaster.SubventedIndicator,
                    Section88 = Convert.ToBoolean(orgMaster.Section88Indicator),
                    Section88Date = orgMaster.Section88StartDate,
                    RegistrationType1 = orgMaster.RegistrationType1,
                    RegistrationOtherName1 = orgMaster.RegistrationOtherName1,
                    RegistrationDate1 = orgMaster.RegistrationDate1,
                    RegistrationType2 = orgMaster.RegistrationType2,
                    RegistrationOtherName2 = orgMaster.RegistrationOtherName2,
                    RegistrationDate2 = orgMaster.RegistrationDate2,
                    SendDate = orgGuide != null ? orgGuide.SendDate : null,
                    ReplySlipReceiveDate = orgGuide != null ? orgGuide.ReplySlipReceiveDate : null,
                    ReplySlipDate = orgGuide != null ? orgGuide.ReplySlipDate : null,
                    LanguageUsedId = orgGuide != null ? orgGuide.LanguageUsed : null,
                    ReplyFromId = orgGuide != null ? orgGuide.OrgReply : "",

                    PromulgationReason = orgGuide != null ? orgGuide.PromulgationReason : "",
                    PartNum = orgGuide != null ? orgGuide.PartNum : "",
                    EnclosureNum = orgGuide != null ? orgGuide.EnclosureNum : "",
                    ReplySlipPartNum = orgGuide != null ? orgGuide.ReplySlipPartNum : "",
                    ReplySlipEnclosureNum = orgGuide != null ? orgGuide.ReplySlipEnclosureNum : "",
                    Remarks = orgGuide != null ? orgGuide.Remarks : "",
                    ReferenceGuideActivityConcern = orgGuide.ActivityConcern,
                    FileRef = orgGuide.FileRef
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/InsertOrgGuide", Name = "InsertOrgGuide")]
        public JsonResult InsertOrgGuide([CustomizeValidator(RuleSet = "default,ReferenceGuideInsert")] OrganisationViewModel model, int orgId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            OrgMaster orgMaster = _organisationService.GetOrgById(orgId);
            var orgRefGuidePromulgation = new OrgRefGuidePromulgation
            {
                OrgMaster = orgMaster,
                SendDate = model.SendDate,
                ReplySlipReceiveDate = model.ReplySlipReceiveDate,
                ReplySlipDate = model.ReplySlipDate,
                LanguageUsed = model.LanguageUsedId,
                OrgReply = model.ReplyFromId,
                PromulgationReason = model.PromulgationReason,
                PartNum = model.PartNum,
                EnclosureNum = model.EnclosureNum,
                ReplySlipPartNum = model.ReplySlipPartNum,
                ReplySlipEnclosureNum = model.ReplySlipEnclosureNum,
                Remarks = model.Remarks,
                ActivityConcern = model.ReferenceGuideActivityConcern,
                FileRef = model.FileRef
            };

            using (_unitOfWork.BeginTransaction())
            {
                _orgRefGuidePromulgationService.CreateOrgRefGuidePromulgation(orgRefGuidePromulgation);
                SaveOrgProvisionNotAdopt(orgRefGuidePromulgation, orgMaster.OrgRef, model.ProvisionsNotBeAdopted);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgMaster)]
        [HttpPost, Route("~/api/Org/UpdateOrgGuide", Name = "UpdateOrgGuide")]
        public JsonResult UpdateOrgGuide([CustomizeValidator(RuleSet = "default,ReferenceGuideUpdate")] OrganisationViewModel model, string orgId, string orgRefGuidePromulgationId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var orgGuide = _orgRefGuidePromulgationService.GetOrgRefGuidePromulgationByOrgRefGuidePromulgationId(Convert.ToInt32(orgRefGuidePromulgationId));

            if (orgGuide == null)//insert
            {
                OrgMaster orgMaster = _organisationService.GetOrgById(Convert.ToInt32(orgId));
                var orgRefGuidePromulgation = new OrgRefGuidePromulgation
                {
                    OrgMaster = orgMaster,
                    SendDate = model.SendDate,
                    ReplySlipReceiveDate = model.ReplySlipReceiveDate,
                    ReplySlipDate = model.ReplySlipDate,
                    LanguageUsed = model.LanguageUsedId,
                    OrgReply = model.ReplyFromId,
                    PromulgationReason = model.PromulgationReason,
                    PartNum = model.PartNum,
                    EnclosureNum = model.EnclosureNum,
                    ReplySlipPartNum = model.ReplySlipPartNum,
                    ReplySlipEnclosureNum = model.ReplySlipEnclosureNum,
                    Remarks = model.Remarks,
                    FileRef = model.FileRef,
                    ActivityConcern = model.ReferenceGuideActivityConcern
                };

                using (_unitOfWork.BeginTransaction())
                {
                    _orgRefGuidePromulgationService.CreateOrgRefGuidePromulgation(orgRefGuidePromulgation);
                    SaveOrgProvisionNotAdopt(orgRefGuidePromulgation, orgMaster.OrgRef, model.ProvisionsNotBeAdopted);
                    _unitOfWork.Commit();
                }
            }
            else //update
            {
                orgGuide.SendDate = model.SendDate;
                orgGuide.ReplySlipReceiveDate = model.ReplySlipReceiveDate;
                orgGuide.ReplySlipDate = model.ReplySlipDate;
                orgGuide.LanguageUsed = model.LanguageUsedId;
                orgGuide.OrgReply = model.ReplyFromId;
                orgGuide.PromulgationReason = model.PromulgationReason;
                orgGuide.PartNum = model.PartNum;
                orgGuide.EnclosureNum = model.EnclosureNum;
                orgGuide.ReplySlipPartNum = model.ReplySlipPartNum;
                orgGuide.ReplySlipEnclosureNum = model.ReplySlipEnclosureNum;
                orgGuide.Remarks = model.Remarks;

                orgGuide.FileRef = model.FileRef;
                orgGuide.ActivityConcern = model.ReferenceGuideActivityConcern;

                using (_unitOfWork.BeginTransaction())
                {
                    _orgRefGuidePromulgationService.UpdateOrgRefGuidePromulgation(orgGuide);
                    _orgProvisionNotAdoptService.DeleteByGuidePromulgationId(orgGuide.OrgRefGuidePromulgationId);
                    SaveOrgProvisionNotAdopt(orgGuide, orgGuide.OrgMaster.OrgRef, model.ProvisionsNotBeAdopted);
                    _unitOfWork.Commit();
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        //Mode - 0: Search, 1: Create???, 2: Edit
        private void initOrgRefGuideViewModel(OrganisationViewModel model, int Mode)
        {
            var registrationType = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.RegistrationTitles = registrationType;

            var orgReply = _lookupService.getAllLkpInCodec(LookupType.OrgReply);
            model.OrgRefGuideFileNum = _parameterService.GetParameterByCode(Constant.SystemParameter.ORG_REF_GUIDE_FILE_NUM).Value;

            if (Mode == 0)
            {
                var OrganisationStatus = _lookupService.getAllLkpInCodec(LookupType.OrgStatus);
                model.OrganisationStatus = OrganisationStatus;

                var YesNo = _lookupService.getAllLkpInCodec(LookupType.YesNo);
                model.Subventeds = YesNo;
                model.Sections = YesNo;
                model.AppliedPSPBefores = YesNo;
                model.AppliedSSAFBefores = YesNo;
                model.AppliedFDBefores = YesNo;
                model.PSPIssuedBefores = YesNo;
                model.FDIssuedBefores = YesNo;
                model.SSAFIssuedBefores = YesNo;

                model.OrgReplys = orgReply;
                model.FdYears = _flagDayListService.GetAllFlagDayListYearForDropdown();

                model.ActivityConcerns = _lookupService.getAllLkpInCodec(LookupType.ReferenceGuideActivityConcern);
            }
            else if (Mode == 2)
            {
                var salute = _lookupService.getAllLkpInCodec(LookupType.Salute);
                var chiSalute = _lookupService.getAllLkpInChiCodec(LookupType.Salute);

                var languageUsed = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
                var provisionsNotBeAdopteds = _lookupService.getAllLkpInCodec(LookupType.ReferenceGuideProvision);

                model.ApplicantSalutes = salute;
                model.ApplicantChiSalutes = chiSalute;
                model.LanguageUseds = languageUsed;
                model.ReplyFroms = orgReply;
                model.ProvisionsNotBeAdopteds = provisionsNotBeAdopteds;

                model.ActivityConcerns = _lookupService.getAllLkpInCodec(LookupType.ReferenceGuideActivityConcern);
            }
        }

        private GridSettings GetReferenceGuideSearchGrid(OrganisationViewModel model, GridSettings grid)
        {
            if (!String.IsNullOrEmpty(model.OrganisationReference))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "OrgRef",
                    data = model.OrganisationReference,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.OrganisationName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "EngOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "ChiOrgName",
                        data = model.OrganisationName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.OrganisationStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "DisableIndicator",
                    data = model.OrganisationStatusId == "0" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.SubventedId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SubventedIndicator",
                    data = model.SubventedId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.RegistrationTitleId))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationType1",
                        data = model.RegistrationTitleId,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationType2",
                        data = model.RegistrationTitleId,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.Registration))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "RegistrationOtherName1",
                        data = model.Registration,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "RegistrationOtherName2",
                        data = model.Registration,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.SectionId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Section88Indicator",
                    data = model.SectionId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (model.ReplySlipReceiveDateStart != null && !model.ReplySlipReceiveDateStart.Equals(""))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ReplySlipReceiveDate",
                    data = model.ReplySlipReceiveDateStart.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (model.ReplySlipReceiveDateEnd != null && !model.ReplySlipReceiveDateEnd.Equals(""))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ReplySlipReceiveDate",
                    data = model.ReplySlipReceiveDateEnd.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }
            if (model.SendDateStart != null && !model.SendDateStart.Equals(""))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SendDate",
                    data = model.SendDateStart.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (model.SendDateEnd != null && !model.SendDateEnd.Equals(""))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SendDate",
                    data = model.SendDateEnd.Value.ToString("dd/MM/yyyy"),
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            if (model.OrgReply != null && model.OrgReply.Count() > 0)
            {
                List<Rule> rules = new List<Rule>();
                foreach (var reply in model.OrgReply)
                {
                    if (!String.IsNullOrEmpty(reply.Trim()))
                    {
                        Rule rule = new Rule();
                        rule.field = "OrgReply";
                        rule.data = reply;
                        rule.op = WhereOperation.Equal.ToEnumValue();
                        rules.Add(rule);
                    }
                    else
                    {
                        var list = model.OrgReply.ToList();
                        list.RemoveAt(list.IndexOf(""));
                        model.OrgReply = list.ToArray();
                    }
                }
                if (rules.Count() > 0)
                {
                    grid.AddDefaultRule(rules, GroupOp.OR);
                }
            }

            if (!String.IsNullOrEmpty(model.AppliedPSPBeforeId) && !model.FromPspApplicationDate.HasValue && !model.ToPspApplicationDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedPSPBefore",
                    data = model.AppliedPSPBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.AppliedSSAFBeforeId) && !model.FromSSAFApplicationDate.HasValue && !model.ToSSAFApplicationDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedSSAFBefore",
                    data = model.AppliedSSAFBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.AppliedFDBeforeId) && (model.AppliedFDBeforeFdYear == null || model.AppliedFDBeforeFdYear.Count == 0))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "AppliedFDBefore",
                    data = model.AppliedFDBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.PSPIssuedBeforeId) && !model.FromPspPermitIssueDate.HasValue && !model.ToPspPermitIssueDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PSPIssuedBefore",
                    data = model.PSPIssuedBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SSAFIssuedBeforeId) && !model.FromSSAFPermitIssueDate.HasValue && !model.ToSSAFPermitIssueDate.HasValue)
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "SSAFIssuedBefore",
                    data = model.SSAFIssuedBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.FDIssuedBeforeId) && (model.FdIssuedBeforeFdYear == null || model.FdIssuedBeforeFdYear.Count == 0))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FDPermitIssuedBefore",
                    data = model.FDIssuedBeforeId == "1" ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            
            return grid;
        }

        private void SaveOrgProvisionNotAdopt(OrgRefGuidePromulgation orgRefGuidePromulgation, string orgRef, string[] provisionIds)
        {
            if (provisionIds != null)
            {
                foreach (var provisionId in provisionIds)
                {
                    OrgProvisionNotAdopt o = new OrgProvisionNotAdopt();
                    o.OrgRef = orgRef;
                    o.OrgRefGuidePromulgationId = orgRefGuidePromulgation.OrgRefGuidePromulgationId;
                    o.ProvisionId = provisionId;
                    _orgProvisionNotAdoptService.CreateOrgProvisionNotAdopt(o);
                }
            }
        }

        #endregion Reference Guide

        #endregion OrgMaster tabList

        #region "Report"

        [PspsAuthorize(Allow.OrgReport)]
        public ActionResult Report()
        {
            OrganisationReportViewModel model = new OrganisationReportViewModel();
            return View(model);
        }

        #region R25

        [PspsAuthorize(Allow.OrgReport)]
        [HttpPost, Route("~/api/report/r25/generate", Name = "R25Generate")]
        public JsonResult R25Generate(OrganisationReportViewModel model)
        {
            var reportId = "R25";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR25Excel(templatePath, model.R25_DateFrom, model.R25_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion R25

        #region ExportOrg

        [PspsAuthorize(Allow.OrgReport, Allow.PspReport, Allow.FdReport)]
        [HttpPost, Route("~/api/report/rawData1/generate", Name = "RawData1Generate")]
        public JsonResult RawData1Generate(OrganisationReportViewModel model)
        {
            var reportId = "RawDataOfOrganization";
            string strTable = "OrgRawView";
            string strWhere = null;

            if (model.Raw1_OrgRef.IsNotNullOrEmpty())
                strWhere = "WHERE OrganisationReference = '{0}'".FormatWith(model.Raw1_OrgRef);

            Dictionary<string, string> fieldNames = new Dictionary<string, string>();
            var properties = typeof(OrganisationViewModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                DisplayAttribute attr = (DisplayAttribute)System.Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
                if (attr != null)
                    fieldNames.Add(property.Name, attr.GetName());
            }

            var ms = _reportService.ExportTableToExcel(reportId, strTable, fieldNames, strWhere);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        #endregion ExportOrg

        #endregion "Report"

        #region "Template"

        [PspsAuthorize(Allow.OrgTemplate)]
        public ActionResult Template()
        {
            OrganisationDocViewModel model = new OrganisationDocViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpGet, Route("~/org/listTemplate", Name = "ListOrgTemplate")]
        public JsonResult ListOrgTemplate(GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            //var org = this._organizationDocService.GetPage(grid);
            //converting in grid format
            var orgs = this._organizationDocService.GetOrgDocSummaryViewPage(grid);

            var gridResult = new GridResult
            {
                TotalPages = orgs.TotalPages,
                CurrentPageIndex = orgs.CurrentPageIndex,
                TotalCount = orgs.TotalCount,
                Data = (from o in orgs
                        select new
                        {
                            DocNum = o.DocNum,
                            DocName = o.DocName,
                            VersionNum = o.VersionNum,
                            Enabled = o.Enabled,
                            OrgDocId = o.OrgDocId,
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/org/newdoc", Name = "NewOrgDoc")]
        public JsonResult New([CustomizeValidator(RuleSet = "default,Create,CreateOrgDoc")] OrganisationDocViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template file from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORGANISATION_TEMPLATE_PATH);
            Ensure.NotNull(templatePath, "No letter found with the specified code");

            // Rename the file name by adding the current times
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Form the Relative Path for storing in DB and Absolute Path for actually saving the file
            string rootPath = templatePath.Value;
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new orgDoc row and fill the value
            var orgDoc = new OrgDoc();
            orgDoc.DocNum = model.DocNum;
            orgDoc.DocStatus = true;
            orgDoc.DocName = model.Description;
            orgDoc.VersionNum = model.Version;
            orgDoc.RowVersion = model.RowVersion;
            orgDoc.FileLocation = relativePath;

            using (_unitOfWork.BeginTransaction())
            {
                // Save the file to the Absolute Path
                if (CommonHelper.CreateFolderIfNeeded(rootPath))
                {
                    model.File.SaveAs(absolutePath);
                }

                // Insert record to DB and commit
                this._organizationDocService.CreateOrgDoc(orgDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/org/{orgDocId:int}/edit", Name = "EditOrgDoc")]
        public JsonResult Edit(int orgDocId, [CustomizeValidator(RuleSet = "default")]  OrganisationDocViewModel model)
        {
            Ensure.Argument.NotNullOrEmpty(orgDocId.ToString());
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            OrgDoc orgDoc = new OrgDoc();
            orgDoc = this._organizationDocService.GetOrgDocById(orgDocId);
            orgDoc.DocNum = model.DocNum;
            orgDoc.DocName = model.Description;
            orgDoc.VersionNum = model.Version;
            orgDoc.DocStatus = model.IsActive;

            using (_unitOfWork.BeginTransaction())
            {
                this._organizationDocService.UpdateOrgDoc(orgDoc);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion "Template"

        #region "Suggestion Version"

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpGet, Route("Doc/{orgDocId:int}/Version", Name = "OrgVersion")]
        //[RuleSetForClientSideMessagesAttribute("default", "Create", "NewVersion")]
        public ActionResult Version(int orgDocId)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }
            var org = this._organizationDocService.GetOrgDocById(orgDocId);

            Ensure.NotNull(org, "No letter found with the specified id");
            OrganisationDocViewModel model = new OrganisationDocViewModel();
            model.DocNum = org.DocNum;
            return View(model);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpGet, Route("~/api/org/{docNum}/listVersion", Name = "ListOrgVersion")]
        public JsonResult ListVersion(GridSettings grid, string docNum)
        {
            Ensure.Argument.NotNull(grid);
            if (!String.IsNullOrEmpty(docNum))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "DocNum",
                    data = docNum,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            var org = this._organizationDocService.GetPage(grid);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = org.TotalPages,
                CurrentPageIndex = org.CurrentPageIndex,
                TotalCount = org.TotalCount,
                Data = (from o in org
                        select new
                        {
                            DocNum = o.DocNum,
                            DocName = o.DocName,
                            VersionNum = o.VersionNum,
                            DocStatus = o.DocStatus,
                            RowVersion = o.RowVersion,
                            orgDocId = o.OrgDocId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/org/versionNew", Name = "NewOrgVersion")]
        public JsonResult VersionNew([CustomizeValidator(RuleSet = "default,NewVersion")]OrganisationDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORGANISATION_TEMPLATE_PATH);

            // Rename the file name by adding the current time
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string fileName = Path.GetFileName(model.File.FileName);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path
            string rootPath = templatePath.Value;
            // Form the Relative Path for storing in DB and Absolute Path for actually saving the file
            string relativePath = generatedFileName;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new OrgDoc row
            var orgDoc = new OrgDoc();
            // Fill the values
            orgDoc.DocNum = model.DocNum;
            orgDoc.DocName = model.Description;
            orgDoc.DocStatus = model.IsActive;
            orgDoc.FileLocation = relativePath;
            orgDoc.VersionNum = model.Version;
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
                    _organizationDocService.CreateOrgDoc(orgDoc);
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

        [PspsAuthorize(Allow.OrgTemplate)]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("~/api/org/versionEdit", Name = "VersionEditOrg")]
        public JsonResult VersionEdit([CustomizeValidator(RuleSet = "default,UpdateOrgVersion")]OrganisationDocViewModel model)
        {
            Ensure.Argument.NotNull(model);

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }

            // Get the root path of the Template from DB
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORGANISATION_TEMPLATE_PATH);

            // Set the root path
            string rootPath = templatePath.Value;
            // Paths for new file if needed
            string relativePath = string.Empty;
            string absolutePath = string.Empty;

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Get the OrgDoc record by given the ID
                    var org = _organizationDocService.GetOrgDocById(model.OrgDocId);
                    Ensure.NotNull(org, "No letter found with the specified id");

                    // If new file need to be upload
                    if (model.File != null)
                    {
                        // Rename the file name by adding the current time
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
                        string absolutePathOfOldFile = Path.Combine(rootPath, org.FileLocation);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                            System.IO.File.Delete(absolutePathOfOldFile);

                        // Replace with the new path
                        org.FileLocation = relativePath;
                    }

                    // Fill the update values
                    org.DocStatus = model.IsActive;
                    org.VersionNum = model.Version;
                    org.DocName = model.Description;

                    // Update DB record and commit
                    _organizationDocService.UpdateOrgDoc(org);
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

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpGet, Route("~/org/{orgDocId:int}/GetVersion", Name = "GetOrgDoc")]
        public JsonResult GetOrgDoc(int orgDocId)
        {
            Ensure.Argument.NotNullOrEmpty(orgDocId.ToString());
            var org = this._organizationDocService.GetOrgDocById(orgDocId);
            if (org == null)
            {
                return Json(new JsonResponse(false)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.NotFound)
                }, JsonRequestBehavior.AllowGet);
            }
            var model = new OrganisationDocViewModel()
            {
                DocNum = org.DocNum,
                Description = org.DocName,
                Version = org.VersionNum,
                IsActive = org.DocStatus,
                RowVersion = org.RowVersion,
                OrgDocId = org.Id
            };
            return Json(new JsonResponse(true)
            {
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpPost, Route("~/api/org/{orgDocId:int}/delete", Name = "DeteteOrgDoc")]
        public JsonResult Delete(int orgDocId, byte[] rowVersion)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the OrgDoc record by given the ID
            var org = this._organizationDocService.GetOrgDocById(orgDocId);
            Ensure.NotNull(org, "No Letter found with the specified id");

            // Get the root path of the Template file from DB and combine with the FileLocation to
            // get the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORGANISATION_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            string absolutePath = Path.Combine(rootPath, org.FileLocation);

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB
                org.RowVersion = rowVersion;
                _organizationDocService.DeleteOrgDoc(org);

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
                Data = org
            }, JsonRequestBehavior.DenyGet);
        }

        [PspsAuthorize(Allow.OrgTemplate)]
        [HttpGet, Route("{orgDocId:int}/download", Name = "DownloadOrgFile")]
        public FileResult Download(int orgDocId)
        {
            // Get the OrgDoc record by given the ID
            var org = _organizationDocService.GetOrgDocById(orgDocId);

            // Get the root path of the Template from DB and combine with the FileLocation to get
            // the Absolute Path that the file actually stored at
            var templatePath = _parameterService.GetParameterByCode(Constant.SystemParameter.ORGANISATION_TEMPLATE_PATH);
            string rootPath = templatePath.Value;
            var absolutePath = Path.Combine(rootPath, org.FileLocation);

            // Set the file name for saving
            string fileName = org.DocName + Path.GetExtension(Path.GetFileName(org.FileLocation));
            return FileDownload(absolutePath, fileName);
        }

        #endregion "Suggestion Version"

        #region "PSP A/C Summary"

        [PspsAuthorize(Allow.PspACSummary)]
        [HttpPost, Route("~/api/org/retrievePSPAccountSummary", Name = "RetrievePSPAccountSummary")]
        public JsonResult RetrievePSPAccountSummary(PSPAccountSummaryViewModel model, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(model);
            this.HttpContext.Session[PSPACSummarySearchSessionName] = model;

            grid = GetPSPACSummarySearchGrid(model, grid);
            var pspMasters = this._PSPService.GetPspAcSummaryPage(grid, model.PermitNo);
            var gridResult = new GridResult
            {
                TotalPages = pspMasters.TotalPages,
                CurrentPageIndex = pspMasters.CurrentPageIndex,
                TotalCount = pspMasters.TotalCount,
                Data = (from p in pspMasters
                        select new
                        {
                            Id = p.PspMasterId,
                            PspMasterId = p.PspMasterId,
                            EngChiOrgName = p.OrgMaster != null ? p.OrgMaster.EngOrgNameSorting + "<br>" + p.OrgMaster.ChiOrgName : "",
                            FileRef = p.PspRef + (p.ChildIndicator ? " *" : ""),
                            ProcessingOfficer = p.ProcessingOfficerPost,
                            ApplicationDisposalDate = p.ApplicationDisposalDate,
                            EventPeriodFrom = p.EventPeriodFrom != null ? p.EventPeriodFrom : (DateTime?)null,
                            EventPeriodTo = p.EventPeriodTo != null ? p.EventPeriodTo : (DateTime?)null,
                            PermitNo = p.PermitNo,
                            RelatedPermitNo = p.RelatedPermitNo,
                            FundUsed = string.IsNullOrEmpty(p.FundUsed) ? "" : fundUseds[p.FundUsed],
                            PspYear = p.PspYear,
                            FundRaisingPurpose = p.EngFundRaisingPurpose.IsNullOrEmpty() ? p.ChiFundRaisingPurpose : p.EngFundRaisingPurpose,
                            //FundUsed = !String.IsNullOrEmpty(p.FundUsed) ? _lookupService.GetDescription(LookupType.FUND_USED, p.FundUsed) : "",
                            //                            DocSubmission = !String.IsNullOrEmpty(p.DocSubmission) ? _lookupService.GetDescription(LookupType.PSP_DOC_SUBMITTED, p.DocSubmission) : "",
                            SubmissionDueDate = p.SubmissionDueDate,
                            FirstReminderIssueDate = p.FirstReminderIssueDate,
                            FirstReminderDeadline = p.FirstReminderDeadline,
                            SecondReminderIssueDate = p.SecondReminderIssueDate,
                            SecondReminderDeadline = p.SecondReminderDeadline,
                            AuditedReportReceivedDate = p.AuditedReportReceivedDate,
                            PublicationReceivedDate = p.PublicationReceivedDate,
                            OfficialReceiptReceivedDate = p.OfficialReceiptReceivedDate,
                            NewspaperCuttingReceivedDate = p.NewspaperCuttingReceivedDate,
                            DocReceivedRemark = p.DocReceivedRemark,
                            GrossProceed = p.GrossProceed,
                            Expenditure = p.Expenditure,
                            NetProceed = p.NetProceed,
                            PspPercent = p.PspPercent != null && p.PspPercent.Value > 0 ? p.PspPercent.Value + "%" : "",
                            OrgAnnualIncome = p.OrgAnnualIncome,
                            SanctionListIndicator = p.SanctionListIndicator,
                            QualifyOpinionIndicator = p.QualifyOpinionIndicator,
                            QualityOpinionDetail = p.QualityOpinionDetail,
                            WithholdingListIndicator = p.WithholdingListIndicator,
                            WithholdingBeginDate = p.WithholdingBeginDate,
                            WithholdingEndDate = p.WithholdingEndDate,
                            WithholdingRemark = p.WithholdingRemark,
                            ArCheckIndicator = p.ArCheckIndicator,
                            PublicationCheckIndicator = p.PublicationCheckIndicator,
                            OfficialReceiptCheckIndicator = p.OfficialReceiptCheckIndicator,
                            NewspaperCheckIndicator = p.NewspaperCheckIndicator,
                            DocRemark = p.DocRemark,
                            Overdue = p.Overdue,
                            Late = p.Late,
                            DisasterMasterId = p.DisasterMasterId,
                            IsSsaf = p.IsSsaf
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.PspACSummary)]
        [HttpPost, Route("~/api/org/exportPSPAccountSummary", Name = "ExportPSPAccountSummary")]
        public JsonResult ExportPSPAccountSummary(ExportSettings exportSettings)
        {
            PSPAccountSummaryViewModel model = ((PSPAccountSummaryViewModel)this.HttpContext.Session[PSPACSummarySearchSessionName]);
            if (model != null)
            {
                exportSettings.GridSettings = GetPSPACSummarySearchGrid(model, exportSettings.GridSettings);
            }

            var pspMasters = this._PSPService.GetPspAcSummaryPage(exportSettings.GridSettings, model.PermitNo);

            var dataList = (from p in pspMasters
                            select new
                            {
                                Id = p.PspMasterId,
                                PspYear = p.PspYear,
                                PspMasterId = p.PspMasterId,
                                EngChiOrgName = p.OrgMaster != null ? p.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + p.OrgMaster.ChiOrgName : "",
                                //FileRef = p.OrgMaster.OrgRef,
                                FileRef = p.PspRef,
                                ProcessingOfficer = p.ProcessingOfficerPost,
                                ApplicationDisposalDate = p.ApplicationDisposalDate,
                                EventPeriodFrom = p.EventPeriodFrom,
                                EventPeriodTo = p.EventPeriodTo,
                                PermitNo = p.PermitNo,
                                RelatedPermitNo = p.RelatedPermitNo,
                                FundRaisingPurpose = p.EngFundRaisingPurpose.IsNullOrEmpty() ? p.ChiFundRaisingPurpose : p.EngFundRaisingPurpose,
                                FundUsed = !String.IsNullOrEmpty(p.FundUsed) ? _lookupService.GetDescription(LookupType.FundUsed, p.FundUsed) : "",
                                // DocSubmission = !String.IsNullOrEmpty(p.DocSubmission) ?
                                // _lookupService.GetDescription(LookupType.PSP_DOC_SUBMITTED,
                                // p.DocSubmission) : "",
                                SubmissionDueDate = p.SubmissionDueDate,
                                FirstReminderIssueDate = p.FirstReminderIssueDate,
                                FirstReminderDeadline = p.FirstReminderDeadline,
                                SecondReminderIssueDate = p.SecondReminderIssueDate,
                                SecondReminderDeadline = p.SecondReminderDeadline,
                                AuditedReportReceivedDate = p.AuditedReportReceivedDate,
                                PublicationReceivedDate = p.PublicationReceivedDate,
                                OfficialReceiptReceivedDate = p.OfficialReceiptReceivedDate,
                                NewspaperCuttingReceivedDate = p.NewspaperCuttingReceivedDate,
                                DocReceivedRemark = p.DocReceivedRemark,
                                GrossProceed = p.GrossProceed,
                                Expenditure = p.Expenditure,
                                NetProceed = p.NetProceed,
                                PspPercent = p.PspPercent != null && p.PspPercent.Value > 0 ? p.PspPercent.Value + "%" : "",
                                OrgAnnualIncome = p.OrgAnnualIncome,
                                SanctionListIndicator = p.SanctionListIndicator != null ? (p.SanctionListIndicator.Value ? "Yes" : "No") : "",
                                QualifyOpinionIndicator = p.QualifyOpinionIndicator != null ? (p.QualifyOpinionIndicator.Value ? "Yes" : "No") : "",
                                QualityOpinionDetail = p.QualityOpinionDetail,
                                WithholdingListIndicator = p.WithholdingListIndicator != null ? (p.WithholdingListIndicator.Value ? "Yes" : "No") : "N/A",
                                WithholdingBeginDate = p.WithholdingBeginDate != null ? CommonHelper.ConvertDateTimeToString(p.WithholdingBeginDate.Value) : "",
                                WithholdingEndDate = p.WithholdingEndDate,
                                WithholdingRemark = p.WithholdingRemark,
                                ArCheckIndicator = p.ArCheckIndicator.IsNotNullOrEmpty() ? p.ArCheckIndicator : "",
                                PublicationCheckIndicator = p.PublicationCheckIndicator.IsNotNullOrEmpty() ? p.PublicationCheckIndicator : "",
                                OfficialReceiptCheckIndicator = p.OfficialReceiptCheckIndicator.IsNotNullOrEmpty() ? p.OfficialReceiptCheckIndicator : "",
                                NewspaperCheckIndicator = p.NewspaperCheckIndicator.IsNotNullOrEmpty() ? p.NewspaperCheckIndicator : "",
                                DocRemark = p.DocRemark,
                                Overdue = p.Overdue == null ? "" : (p.Overdue.Value ? "Yes" : "No"),
                                Late = p.Late == null ? "" : (p.Late.Value ? "Yes" : "No"),
                                IsSsaf = p.IsSsaf == null ? "" : (p.IsSsaf.Value ? "Yes" : "No"),
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<PSPAccountSummaryViewModel>();

            if (model.OrgRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgRef"] + " : ORG" + model.OrgRef);

            if (model.OrgName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgName"] + " : " + model.OrgName);

            if (model.OrgStatusId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrgStatusId]);

            if (model.SubventedIndicatorId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SubventedIndicatorId"] + " : " + (model.SubventedIndicatorId != "1" ? "False" : "True"));
            }

            if (model.RegistrationId.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationId];
                if (model.RegistrationId == "Others")
                {
                    if (model.RegistrationOtherName.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc + " ( " + model.RegistrationOtherName + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc);
                }
            }

            if (model.SectionId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SectionId"] + " : " + (model.SectionId != "1" ? "False" : "True"));

            if (model.PSPRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PSPRef"] + " : " + model.PSPRef);

            if (model.PermitNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PermitNo"] + " : " + model.PermitNo);

            if (model.DateofApplicationDisposal.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["DateofApplicationDisposal"] + " : " + model.DateofApplicationDisposal);

            if (model.DateofApplicationDisposalFr.IsNotNullOrEmpty() || model.DateofApplicationDisposalTo.IsNotNullOrEmpty())
            {
                string tempVal = "";
                if (model.DateofApplicationDisposalFr.IsNotNullOrEmpty() && model.DateofApplicationDisposalTo.IsNotNullOrEmpty())
                    tempVal = "From " + model.DateofApplicationDisposalFr + " to " + model.DateofApplicationDisposalTo;
                else if (model.DateofApplicationDisposalFr.IsNotNullOrEmpty())
                    tempVal = "From " + model.DateofApplicationDisposalFr;
                else if (model.DateofApplicationDisposalTo.IsNotNullOrEmpty())
                    tempVal = "To " + model.DateofApplicationDisposalTo;

                filterCriterias.Add(fieldNames["DateofApplicationDisposal"] + " : " + tempVal);
            }

            if (model.WithholdingBeginDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["WithholdingBeginDate"] + " : " + model.WithholdingBeginDate);

            if (model.WithholdingEndDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["WithholdingEndDate"] + " : " + model.WithholdingEndDate);

            if (model.Overdue.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["Overdue"] + " : " + (model.Overdue != "1" ? "False" : "True"));

            if (model.Late.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["Late"] + " : " + (model.Late != "1" ? "False" : "True"));

            if (model.IsSsaf.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["IsSsaf"] + " : " + (model.IsSsaf != "1" ? "False" : "True"));

            if (model.EventYear.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["EventYear"] + " : " + _PSPService.GetAllEventYearForDropdown()[model.EventYear]);

            if (model.EventStartDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["EventStartDate"] + " : " + model.EventStartDate);

            if (model.EventEndDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["EventEndDate"] + " : " + model.EventEndDate);

            if (model.EventCancel.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["EventCancel"] + " : " + (model.EventCancel != "1" ? "False" : "True"));
            
            if (model.DisasterMasterId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["DisasterMasterId"] + " : " + disasterNames[Convert.ToInt16(model.DisasterMasterId.Split(new String[] { "[SortingDelimiter]" }, StringSplitOptions.None)[1])]);
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.PspACSummary)]
        [HttpPost, Route("~/api/org/editPSPAccountSummary", Name = "EditPSPAccountSummary")]
        public JsonResult EditPSPAccountSummary(PSPAccountSummaryViewModel model)
        {
            Ensure.Argument.NotNull(model);

            var errorMsg = ValidateReminderIssuedMustEarlierThanDeadline(model);
            if (!String.IsNullOrEmpty(errorMsg))
            {
                return Json(new JsonResponse(true)
                {
                    Success = false,
                    Errors = errorMsg,
                }, JsonRequestBehavior.DenyGet);
            }

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }
            var pspDeadlineParam = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE);
            var pspDeadlineParam2 = _parameterService.GetParameterByCode(Constant.SystemParameter.PSP_REMINDER_DEADLINE2);

            var pspMaster = _PSPService.GetPSPById(Convert.ToInt32(model.Id));
            //pspMaster.FundUsed
            Ensure.NotNull(pspMaster, "No PspMaster found with the specified Id");
            pspMaster.FundUsed = model.FundUsed;
            pspMaster.DocSubmission = model.DocSubmission;

            //if (!String.IsNullOrEmpty(model.SubmissionDueDate))
            //{
            //    pspMaster.SubmissionDueDate = CommonHelper.ConvertStringToDateTime(model.SubmissionDueDate);
            //}
            //if (pspMaster.EventPeriodTo != null)
            //{
            //    pspMaster.SubmissionDueDate = pspMaster.EventPeriodTo.Value.AddDays(90);
            //}

            if (!String.IsNullOrEmpty(model.FirstReminderIssueDate))
            {
                DateTime firstReminderIssueDate = CommonHelper.ConvertStringToDateTime(model.FirstReminderIssueDate);
                pspMaster.FirstReminderIssueDate = firstReminderIssueDate;
                pspMaster.FirstReminderDeadline = firstReminderIssueDate.AddDays(Convert.ToInt32(pspDeadlineParam.Value));
            }

            if (!String.IsNullOrEmpty(model.SecondReminderIssueDate))
            {
                DateTime secondReminderIssueDate = CommonHelper.ConvertStringToDateTime(model.SecondReminderIssueDate);
                pspMaster.SecondReminderIssueDate = secondReminderIssueDate;
                pspMaster.SecondReminderDeadline = secondReminderIssueDate.AddDays(Convert.ToInt32(pspDeadlineParam2.Value));
            }

            pspMaster.AuditedReportReceivedDate = model.AuditedReportReceivedDate;
            pspMaster.PublicationReceivedDate = model.PublicationReceivedDate;
            pspMaster.OfficialReceiptReceivedDate = model.OfficialReceiptReceivedDate;
            pspMaster.NewspaperCuttingReceivedDate = model.NewspaperCuttingReceivedDate;
            if (!String.IsNullOrEmpty(model.WithholdingBeginDate))
            {
                pspMaster.WithholdingBeginDate = CommonHelper.ConvertStringToDateTime(model.WithholdingBeginDate);
            }
            if (!String.IsNullOrEmpty(model.WithholdingEndDate))
            {
                pspMaster.WithholdingEndDate = CommonHelper.ConvertStringToDateTime(model.WithholdingEndDate);
            }
            pspMaster.WithholdingRemark = model.WithholdingRemark;
            pspMaster.DocReceivedRemark = model.DocReceivedRemark;
            pspMaster.GrossProceed = model.GrossProceed;
            pspMaster.Expenditure = model.Expenditure;
            pspMaster.NetProceed = model.GrossProceed - model.Expenditure;

            if (pspMaster.NetProceed < 0)
                pspMaster.NetProceed = 0;

            pspMaster.OrgAnnualIncome = model.OrgAnnualIncome;
            if (!String.IsNullOrEmpty(model.QualifyOpinionIndicator))
            {
                pspMaster.QualifyOpinionIndicator = (!String.IsNullOrEmpty(model.QualifyOpinionIndicator) && model.QualifyOpinionIndicator.Equals("1")) ? true : false;
            }
            else
            {
                pspMaster.QualifyOpinionIndicator = null;
            }
            pspMaster.QualityOpinionDetail = model.QualityOpinionDetail;
            if (!String.IsNullOrEmpty(model.WithholdingListIndicator))
            {
                pspMaster.WithholdingListIndicator = model.WithholdingListIndicator.Equals("1") ? true : false;
            }
            else
            {
                pspMaster.WithholdingListIndicator = null;
            }

            if (!String.IsNullOrEmpty(model.ArCheckIndicator))
            {
                pspMaster.ArCheckIndicator = model.ArCheckIndicator;
            }
            else
            {
                pspMaster.ArCheckIndicator = null;
            }
            if (!String.IsNullOrEmpty(model.PublicationCheckIndicator))
            {
                pspMaster.PublicationCheckIndicator = model.PublicationCheckIndicator;
            }
            else
            {
                pspMaster.PublicationCheckIndicator = null;
            }
            if (!String.IsNullOrEmpty(model.OfficialReceiptCheckIndicator))
            {
                pspMaster.OfficialReceiptCheckIndicator = model.OfficialReceiptCheckIndicator;
            }
            else
            {
                pspMaster.OfficialReceiptCheckIndicator = null;
            }
            if (!String.IsNullOrEmpty(model.NewspaperCheckIndicator))
            {
                pspMaster.NewspaperCheckIndicator = model.NewspaperCheckIndicator;
            }
            else
            {
                pspMaster.NewspaperCheckIndicator = null;
            }
            pspMaster.DocRemark = model.DocRemark;

            //if (!String.IsNullOrEmpty(model.IsSsaf))
            //{
            //    pspMaster.IsSsaf = model.IsSsaf.Equals("1") ? true : false;
            //}
            //else
            //{
            //    pspMaster.IsSsaf = null;
            //}

            using (_unitOfWork.BeginTransaction())
            {
                _PSPService.UpdatePspMaster(pspMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Success = true,
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        protected bool ValidateFirstReminderIssuedMustEarlierThanDeadline(PSPAccountSummaryViewModel model)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime firstReminderIssueDate;
            DateTime.TryParseExact(model.FirstReminderIssueDate, "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderIssueDate);
            DateTime firstReminderDeadline;
            DateTime.TryParseExact(model.FirstReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderDeadline);
            if (firstReminderIssueDate > firstReminderDeadline)
            {
                return false;
            }
            return true;
        }

        protected bool ValidateSecondReminderIssuedMustEarlierThanDeadline(PSPAccountSummaryViewModel model)
        {
            //model.SecondReminderIssueDate
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime secondReminderIssueDate;
            DateTime.TryParseExact(model.SecondReminderIssueDate, "d/M/yyyy", enUS, DateTimeStyles.None, out secondReminderIssueDate);
            DateTime SecondReminderDeadline;
            DateTime.TryParseExact(model.SecondReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out SecondReminderDeadline);
            if (secondReminderIssueDate > SecondReminderDeadline)
            {
                return false;
            }
            return true;
        }

        private GridSettings GetPSPACSummarySearchGrid(PSPAccountSummaryViewModel model, GridSettings grid)
        {
            if (!String.IsNullOrEmpty(model.OrgRef))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.OrgRef",
                    data = model.OrgRef,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.OrgName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.EngOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.ChiOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.OrgStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.DisableIndicator",
                    data = model.OrgStatusId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SubventedIndicatorId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.SubventedIndicator",
                    data = model.SubventedIndicatorId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.RegistrationId))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.RegistrationType1",
                        data = model.RegistrationId,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.RegistrationType2",
                        data = model.RegistrationId,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.RegistrationOtherName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.RegistrationOtherName1",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.RegistrationOtherName2",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.SectionId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Section88Indicator",
                    data = model.SectionId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.PSPRef))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PspRef",
                    data = model.PSPRef,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.WithholdingBeginDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingBeginDate",
                    data = model.WithholdingBeginDate,
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.WithholdingEndDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingEndDate",
                    data = model.WithholdingEndDate,
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            if (model.WithholdingListIndicator.IsNotNullOrEmpty())
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingListIndicator",
                    data = model.WithholdingListIndicator.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.Overdue))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Overdue",
                    data = model.Overdue.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });

                //grid.AddDefaultRule(new List<Rule>{
                //    new Rule()
                //    {
                //        field = "Overdue",
                //        data =model.Overdue.Equals("1") ? "true" : "false",
                //        op = WhereOperation.Equal.ToEnumValue()
                //    },
                //    new Rule()
                //    {
                //        field = "Overdue",
                //        data = "",
                //        op = WhereOperation.NotEqual.ToEnumValue()
                //    }
                //}, GroupOp.AND);
            }

            if (!String.IsNullOrEmpty(model.Late))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Late",
                    data = model.Late.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });

                //grid.AddDefaultRule(new List<Rule>{
                //    new Rule()
                //    {
                //        field = "Late",
                //        data =model.Late.Equals("1") ? "true" : "false",
                //        op = WhereOperation.Equal.ToEnumValue()
                //    },
                //    new Rule()
                //    {
                //        field = "Late",
                //        data = "",
                //        op = WhereOperation.NotEqual.ToEnumValue()
                //    }
                //}, GroupOp.AND);
            }


            if (!String.IsNullOrEmpty(model.EventCancel))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "CancelIndicator",
                    data = model.EventCancel.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.IsSsaf))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "IsSsaf",
                    data = model.IsSsaf.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.EventYear))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "PspYear",
                    data = model.EventYear,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            //CR-005 03
            //CR-005-TIR 01
            if (!String.IsNullOrEmpty(model.DateofApplicationDisposalFr))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ApplicationDisposalDate",
                    data = model.DateofApplicationDisposalFr,
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.DateofApplicationDisposalTo))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "ApplicationDisposalDate",
                    data = model.DateofApplicationDisposalTo,
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.EventStartDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "EventPeriodFrom",
                    data = model.EventStartDate,
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.EventEndDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "EventPeriodTo",
                    data = model.EventEndDate,
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.DisasterMasterId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "DisasterMasterId",
                    data = model.DisasterMasterId.ToString(),
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            return grid;
        }

        private string ValidateReminderIssuedMustEarlierThanDeadline(PSPAccountSummaryViewModel model)
        {
            StringBuilder error = new StringBuilder("");
            if (!String.IsNullOrEmpty(model.FirstReminderIssueDate) && !String.IsNullOrEmpty(model.FirstReminderDeadline))
            {
                if (!ValidateFirstReminderIssuedMustEarlierThanDeadline(model))
                {
                    error.Append(_messageService.GetMessage(SystemMessage.Error.Organisation.FirstReminderIssuedMustEarlierThanFirstReminderDeadline));
                }
            }

            if (!String.IsNullOrEmpty(model.SecondReminderIssueDate) && !String.IsNullOrEmpty(model.SecondReminderDeadline))
            {
                if (!ValidateSecondReminderIssuedMustEarlierThanDeadline(model))
                {
                    error.Append(_messageService.GetMessage(SystemMessage.Error.Organisation.SecondReminderIssuedMustEarlierThanSecondReminderDeadline));
                }
            }
            return error.ToString();
        }

        #endregion "PSP A/C Summary"

        #region "FD A/C Summary"

        [PspsAuthorize(Allow.FdACSummary)]
        [HttpGet, Route("~/org/retrieveFdACSummaryDropdownData", Name = "RetrieveFdACSummaryDropdownData")]
        public JsonResult RetrieveFdACSummaryDropdownData()
        {
            var docSubmissions = _lookupService.GetAllLookupListByType(LookupType.FdDocSubmitted);
            var yesNo = _lookupService.GetAllLookupListByType(LookupType.YesNo);
            var map = new Hashtable();
            map.Add("DocSubmissions", docSubmissions);
            map.Add("YesNo", yesNo);
            return Json(new JsonResponse(true)
            {
                Data = map
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdACSummary)]
        [HttpPost, Route("~/org/retrieveFdAccountSummary", Name = "RetrieveFdAccountSummary")]
        public JsonResult RetrieveFdAccountSummary(FDAccountSummaryViewModel model, GridSettings grid)
        {
            Ensure.Argument.NotNull(grid);
            Ensure.Argument.NotNull(model);
            this.HttpContext.Session[FDACSummarySearchSessionName] = model;
            grid = GetFDACSummarySearchGrid(model, grid);
            var fdMasters = this._flagDayService.GetPageByFdAcSummaryView(grid, model.PermitNo, model.FlagDay);
            var gridResult = new GridResult
            {
                TotalPages = fdMasters.TotalPages,
                CurrentPageIndex = fdMasters.CurrentPageIndex,
                TotalCount = fdMasters.TotalCount,
                Data = (from f in fdMasters
                        select new
                        {
                            Id = f.FdMasterId,
                            FdMasterId = f.FdMasterId,
                            FlagDay = getFlagDay(f.FdEvent),
                            Twr = getTWR(f.FdEvent),
                            PermitNum = getPermitNum(f.FdEvent),
                            //FdRef = f.OrgMaster.OrgRef,
                            FdRef = f.FdRef,
                            engChiOrgName = f.OrgMaster != null ? f.OrgMaster.EngOrgNameSorting + "<BR>" + f.OrgMaster.ChiOrgName : "",
                            DocSubmission = !String.IsNullOrEmpty(f.DocSubmission) ? _lookupService.GetDescription(LookupType.FdDocSubmitted, f.DocSubmission) : "",
                            SubmissionDueDate = f.SubmissionDueDate,
                            FirstReminderIssueDate = f.FirstReminderIssueDate,
                            FirstReminderDeadline = f.FirstReminderDeadline,
                            SecondReminderIssueDate = f.SecondReminderIssueDate,
                            SecondReminderDeadline = f.SecondReminderDeadline,
                            AuditReportReceivedDate = f.AuditReportReceivedDate,
                            PublicationReceivedDate = f.PublicationReceivedDate,
                            DocReceiveRemark = f.DocReceiveRemark,
                            DocRemark = f.DocRemark,
                            StreetCollection = f.StreetCollection,
                            GrossProceed = f.GrossProceed,
                            Expenditure = f.Expenditure,
                            NetProceed = f.NetProceed,
                            FdPercent = f.FdPercent != null && f.FdPercent.Value > 0 ? f.FdPercent.Value + "%" : "",
                            NewspaperPublishDate = f.NewspaperPublishDate,
                            PledgingAmt = f.PledgingAmt,
                            AcknowledgementReceiveDate = f.AcknowledgementReceiveDate,
                            AcknowledgementEmailIssueDate = f.AcknowledgementEmailIssueDate,
                            AfsReceiveIndicator = f.AfsReceiveIndicator,
                            RequestPermitteeIndicator = f.RequestPermitteeIndicator,
                            AfsReSubmitIndicator = f.AfsReSubmitIndicator,
                            AfsReminderIssueIndicator = f.AfsReminderIssueIndicator,
                            WithholdingListIndicator = f.WithholdingListIndicator,
                            WithholdingBeginDate = f.WithholdingBeginDate,
                            WithholdingEndDate = f.WithholdingEndDate,
                            WithholdingRemark = f.WithholdingRemark,
                            remark = f.Remark,
                            Overdue = f.Overdue,
                            Late = f.Late,
                            JointApplicationIndicator = f.JointApplicationIndicator
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [PspsAuthorize(Allow.FdACSummary)]
        [HttpPost, Route("~/api/org/exportFDAccountSummary", Name = "ExportFDAccountSummary")]
        public JsonResult ExportFDAccountSummary(ExportSettings exportSettings)
        {
            FDAccountSummaryViewModel model = ((FDAccountSummaryViewModel)this.HttpContext.Session[FDACSummarySearchSessionName]);
            if (model != null)
            {
                exportSettings.GridSettings = GetFDACSummarySearchGrid(model, exportSettings.GridSettings);
            }
            var fdMasters = this._flagDayService.GetPageByFdAcSummaryView(exportSettings.GridSettings, model.PermitNo, model.FlagDay);
            var dataList = (from f in fdMasters
                            select new
                            {
                                Id = f.FdMasterId,
                                FdMasterId = f.FdMasterId,
                                FlagDay = getFlagDay(f.FdEvent),
                                Twr = getTWR(f.FdEvent),
                                PermitNum = getPermitNum(f.FdEvent),
                                //FdRef = f.OrgMaster.OrgRef,
                                FdRef = f.FdRef,
                                engChiOrgName = f.OrgMaster != null ? f.OrgMaster.EngOrgNameSorting + System.Environment.NewLine + f.OrgMaster.ChiOrgName : "",
                                DocSubmission = !String.IsNullOrEmpty(f.DocSubmission) ? _lookupService.GetDescription(LookupType.FdDocSubmitted, f.DocSubmission) : "",
                                SubmissionDueDate = f.SubmissionDueDate,
                                FirstReminderIssueDate = f.FirstReminderIssueDate,
                                FirstReminderDeadline = f.FirstReminderDeadline,
                                SecondReminderIssueDate = f.SecondReminderIssueDate,
                                SecondReminderDeadline = f.SecondReminderDeadline,
                                AuditReportReceivedDate = f.AuditReportReceivedDate,
                                PublicationReceivedDate = f.PublicationReceivedDate,
                                DocReceiveRemark = f.DocReceiveRemark,
                                DocRemark = f.DocRemark,
                                StreetCollection = f.StreetCollection,
                                GrossProceed = f.GrossProceed,
                                Expenditure = f.Expenditure,
                                NetProceed = f.NetProceed,
                                FdPercent = f.FdPercent != null && f.FdPercent.Value > 0 ? f.FdPercent.Value + "%" : "",
                                NewspaperPublishDate = f.NewspaperPublishDate,
                                PledgingAmt = f.PledgingAmt,
                                AcknowledgementReceiveDate = f.AcknowledgementReceiveDate,
                                AcknowledgementEmailIssueDate = f.AcknowledgementEmailIssueDate,
                                AfsReceiveIndicator = (f.AfsReceiveIndicator != null && f.AfsReceiveIndicator.Value) ? "Yes" : "No",
                                RequestPermitteeIndicator = (f.RequestPermitteeIndicator != null && f.RequestPermitteeIndicator.Value) ? "Yes" : "No",
                                AfsReSubmitIndicator = (f.AfsReSubmitIndicator != null && f.AfsReSubmitIndicator.Value) ? "Yes" : "No",
                                AfsReminderIssueIndicator = (f.AfsReminderIssueIndicator != null && f.AfsReminderIssueIndicator.Value) ? "Yes" : "No",
                                WithholdingListIndicator = (f.WithholdingListIndicator != null && f.WithholdingListIndicator.Value) ? "Yes" : "No",
                                WithholdingBeginDate = f.WithholdingBeginDate,
                                WithholdingEndDate = f.WithholdingEndDate,
                                WithholdingRemark = f.WithholdingRemark,
                                remark = f.Remark,
                                Overdue = f.Overdue,
                                Late = f.Late,
                            }).ToArray();

            //------------------------------------------------------------------------Get Filtering List-----------------------------------------------------------------
            List<string> filterCriterias = new List<string>();
            Dictionary<string, string> fieldNames = GetViewModelDescList<FDAccountSummaryViewModel>();

            if (model.OrgRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgRef"] + " : ORG" + model.OrgRef);

            if (model.OrgName.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgName"] + " : " + model.OrgName);

            if (model.OrgStatusId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["OrgStatusId"] + " : " + _lookupService.getAllLkpInCodec(LookupType.OrgStatus)[model.OrgStatusId]);

            if (model.SubventedIndicatorId.IsNotNullOrEmpty())
            {
                filterCriterias.Add(fieldNames["SubventedIndicatorId"] + " : " + (model.SubventedIndicatorId != "1" ? "False" : "True"));
            }

            if (model.RegistrationId.IsNotNullOrEmpty())
            {
                var tempDesc = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType)[model.RegistrationId];
                if (model.RegistrationId == "Others")
                {
                    if (model.RegistrationOtherName.IsNotNullOrEmpty())
                    {
                        filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc + " ( " + model.RegistrationOtherName + " ) ");
                    }
                    else
                    {
                        filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc);
                    }
                }
                else
                {
                    filterCriterias.Add(fieldNames["RegistrationId"] + " : " + tempDesc);
                }
            }

            if (model.SectionId.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["SectionId"] + " : " + (model.SectionId != "1" ? "False" : "True"));

            if (model.FDRef.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["FDRef"] + " : " + model.FDRef);

            if (model.PermitNo.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["PermitNo"] + " : " + model.PermitNo);

            if (model.WithholdingBeginDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["WithholdingBeginDate"] + " : " + model.WithholdingBeginDate);

            if (model.WithholdingEndDate.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["WithholdingEndDate"] + " : " + model.WithholdingEndDate);

            if (model.FlagDay.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["FlagDay"] + " : " + model.FlagDay);

            if (model.FlagDayYear.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["FlagDayYear"] + " : " + _flagDayListService.GetAllFlagDayListYearForDropdown()[model.FlagDayYear]);

            if (model.Overdue.IsNotNullOrEmpty())
                filterCriterias.Add(fieldNames["Overdue"] + " : " + (model.Overdue != "1" ? "False" : "True"));
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

            MemoryStream ms = _reportService.ExportToExcel(dataList, null, exportSettings.ColumnModel, filterCriterias, false);

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);

            String sessionId = "GridReport";

            return JsonReportResult(sessionId, time + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.FdACSummary)]
        [HttpPost, Route("~/api/org/editFDAccountSummary", Name = "EditFDAccountSummary")]
        public JsonResult EditFDAccountSummary(FDAccountSummaryViewModel model)
        {
            Ensure.Argument.NotNull(model);

            var errorMsg = ValidateFDReminderIssuedMustEarlierThanDeadline(model);
            if (!String.IsNullOrEmpty(errorMsg))
            {
                return Json(new JsonResponse(true)
                {
                    Success = false,
                    Errors = errorMsg,
                }, JsonRequestBehavior.DenyGet);
            }
            var fdReminderDeadlineParam = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE);
            var fdReminderDeadlineParam2 = _parameterService.GetParameterByCode(Constant.SystemParameter.FD_REMINDER_DEADLINE2);
            var fdMaster = _flagDayService.GetFDById(Convert.ToInt32(model.Id));
            if (!String.IsNullOrEmpty(model.DocSubmission))
            {
                fdMaster.DocSubmission = model.DocSubmission;
            }
            if (!String.IsNullOrEmpty(model.SubmissionDueDate))
            {
                fdMaster.SubmissionDueDate = CommonHelper.ConvertStringToDateTime(model.SubmissionDueDate);
            }
            if (!String.IsNullOrEmpty(model.FirstReminderIssueDate))
            {
                var firstReminderIssueDate = CommonHelper.ConvertStringToDateTime(model.FirstReminderIssueDate);
                fdMaster.FirstReminderIssueDate = firstReminderIssueDate;
                fdMaster.FirstReminderDeadline = firstReminderIssueDate.AddDays(Convert.ToInt32(fdReminderDeadlineParam.Value));
            }
            if (!String.IsNullOrEmpty(model.SecondReminderIssueDate))
            {
                var secondReminderIssueDate = CommonHelper.ConvertStringToDateTime(model.SecondReminderIssueDate);
                fdMaster.SecondReminderIssueDate = secondReminderIssueDate;
                fdMaster.SecondReminderDeadline = secondReminderIssueDate.AddDays(Convert.ToInt32(fdReminderDeadlineParam2.Value));
            }

            if (!String.IsNullOrEmpty(model.AuditReportReceivedDate))
            {
                fdMaster.AuditReportReceivedDate = model.AuditReportReceivedDate;
            }
            if (!String.IsNullOrEmpty(model.PublicationReceivedDate))
            {
                fdMaster.PublicationReceivedDate = model.PublicationReceivedDate;
            }

            fdMaster.DocReceiveRemark = model.DocReceiveRemark;
            fdMaster.DocRemark = model.DocRemark;
            fdMaster.StreetCollection = model.StreetCollection;
            fdMaster.GrossProceed = model.GrossProceed;
            fdMaster.Expenditure = model.Expenditure;
            fdMaster.NetProceed = model.GrossProceed - model.Expenditure;
            if (!String.IsNullOrEmpty(model.NewspaperPublishDate))
            {
                fdMaster.NewspaperPublishDate = CommonHelper.ConvertStringToDateTime(model.NewspaperPublishDate);
            }
            fdMaster.PledgingAmt = model.PledgingAmt;
            if (!String.IsNullOrEmpty(model.AcknowledgementReceiveDate))
            {
                fdMaster.AcknowledgementReceiveDate = CommonHelper.ConvertStringToDateTime(model.AcknowledgementReceiveDate);
            }
            if (!String.IsNullOrEmpty(model.AcknowledgementEmailIssueDate))
            {
                fdMaster.AcknowledgementEmailIssueDate = CommonHelper.ConvertStringToDateTime(model.AcknowledgementEmailIssueDate);
            }
            if (!string.IsNullOrEmpty(model.AfsReceiveIndicator))
            {
                fdMaster.AfsReceiveIndicator = model.AfsReceiveIndicator.Equals("1") ? true : false;
            }
            else
            {
                fdMaster.AfsReceiveIndicator = null;
            }
            if (!string.IsNullOrEmpty(model.RequestPermitteeIndicator))
            {
                fdMaster.RequestPermitteeIndicator = model.RequestPermitteeIndicator.Equals("1") ? true : false;
            }
            else
            {
                fdMaster.RequestPermitteeIndicator = null;
            }
            if ((!string.IsNullOrEmpty(model.AfsReSubmitIndicator)))
            {
                fdMaster.AfsReSubmitIndicator = model.AfsReSubmitIndicator.Equals("1") ? true : false;
            }
            else
            {
                fdMaster.AfsReSubmitIndicator = null;
            }
            if (!string.IsNullOrEmpty(model.AfsReminderIssueIndicator))
            {
                fdMaster.AfsReminderIssueIndicator = model.AfsReminderIssueIndicator.Equals("1") ? true : false;
            }
            else
            {
                fdMaster.AfsReminderIssueIndicator = null;
            }
            if (!string.IsNullOrEmpty(model.WithholdingListIndicator))
            {
                fdMaster.WithholdingListIndicator = model.WithholdingListIndicator.Equals("1") ? true : false;
            }
            else
            {
                fdMaster.WithholdingListIndicator = null;
            }

            if (!String.IsNullOrEmpty(model.WithholdingBeginDate))
            {
                fdMaster.WithholdingBeginDate = CommonHelper.ConvertStringToDateTime(model.WithholdingBeginDate);
            }
            if (!String.IsNullOrEmpty(model.WithholdingEndDate))
            {
                fdMaster.WithholdingEndDate = CommonHelper.ConvertStringToDateTime(model.WithholdingEndDate);
            }
            fdMaster.WithholdingRemark = model.WithholdingRemark;
            fdMaster.Remark = model.Remark;

            using (_unitOfWork.BeginTransaction())
            {
                _flagDayService.UpdateFdMaster(fdMaster);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Success = true,
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated)
            }, JsonRequestBehavior.DenyGet);
        }

        //[PspsAuthorize(Allow.FdACSummary)]
        //[HttpPost, Route("~/api/org/RetrieveFDAccountSummarya", Name = "RetrieveFDAccountSummarya")]
        //public JsonResult RetrieveFDAccountSummarya(PSPViewModel model, GridSettings grid)
        //{
        //    Ensure.Argument.NotNull(model);
        //    Ensure.Argument.NotNull(grid);

        // grid.AddDefaultRule(new Rule() { field = "IsDeleted", data = "false", op =
        // WhereOperation.Equal.ToEnumValue() });

        // if (!CommonHelper.AreNullOrEmpty(model.OrgRef)) { grid.Where.rules.Add(new Rule() { field
        // = "orgMaster.OrgRef", data = model.RefGuideId, op = WhereOperation.Equal.ToEnumValue()
        // }); }

        // if (!CommonHelper.AreNullOrEmpty(model.OrgName)) { grid.Where.rules.Add(new Rule() {
        // field = "orgMaster.EngOrgName", data = model.OrgName, op =
        // WhereOperation.Contains.ToEnumValue() });

        // grid.Where.rules.Add(new Rule() { field = "orgMaster.ChiOrgName", data = model.OrgName,
        // op = WhereOperation.Contains.ToEnumValue() }); }

        // if (!CommonHelper.AreNullOrEmpty(model.TelNo)) { grid.Where.rules.Add(new Rule() { field
        // = "ContactPersonTelNum", data = model.TelNo, op = WhereOperation.Equal.ToEnumValue() }); }

        // if (model.Subvented.HasValue) { grid.Where.rules.Add(new Rule() { field =
        // "orgMaster.SubventedIndicator", data = model.Subvented.Value ? "true" : "false", op =
        // WhereOperation.Equal.ToEnumValue() }); }

        // //Name of Contact Person (Eng / Chi) if
        // (!CommonHelper.AreNullOrEmpty(model.ContactPersonName)) { grid.Where.rules.Add(new Rule()
        // { field = "ContactPersonChiFirstName", data = model.ContactPersonName, op =
        // WhereOperation.Contains.ToEnumValue() }); } //Principal Activities / Services

        // //Section 88

        // //Registration

        // //Adopt "Reference Guide"

        // var flagDays = this._flagDayService.GetPage(grid); var gridResult = new GridResult {
        // TotalPages = flagDays.TotalPages, CurrentPageIndex = flagDays.CurrentPageIndex,
        // TotalCount = flagDays.TotalCount, Data = (from f in flagDays select new { fdYear =
        // f.FdYear, tw = "", fdPermitNo = "", fdRef = f.FdRef, engOrgName = f.OrgMaster.EngOrgName,
        // chiOrgName = f.OrgMaster.ChiOrgName }).ToArray() };

        //    return Json(new JsonResponse(true)
        //    {
        //        Data = gridResult
        //    }, JsonRequestBehavior.AllowGet);
        //}

        protected bool ValidateFDFirstReminderIssuedMustEarlierThanDeadline(FDAccountSummaryViewModel model)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime firstReminderIssueDate;
            DateTime.TryParseExact(model.FirstReminderIssueDate, "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderIssueDate);
            DateTime firstReminderDeadline;
            DateTime.TryParseExact(model.FirstReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out firstReminderDeadline);
            if (firstReminderIssueDate > firstReminderDeadline)
            {
                return false;
            }
            return true;
        }

        protected bool ValidateFDSecondReminderIssuedMustEarlierThanDeadline(FDAccountSummaryViewModel model)
        {
            //model.SecondReminderIssueDate
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime secondReminderIssueDate;
            DateTime.TryParseExact(model.SecondReminderIssueDate, "d/M/yyyy", enUS, DateTimeStyles.None, out secondReminderIssueDate);
            DateTime SecondReminderDeadline;
            DateTime.TryParseExact(model.SecondReminderDeadline.ToString(), "d/M/yyyy", enUS, DateTimeStyles.None, out SecondReminderDeadline);
            if (secondReminderIssueDate > SecondReminderDeadline)
            {
                return false;
            }
            return true;
        }

        private GridSettings GetFDACSummarySearchGrid(FDAccountSummaryViewModel model, GridSettings grid)
        {
            if (!String.IsNullOrEmpty(model.OrgRef))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.OrgRef",
                    data = model.OrgRef,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.OrgName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.EngOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.ChiOrgName",
                        data = model.OrgName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }

            if (!String.IsNullOrEmpty(model.OrgStatusId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.DisableIndicator",
                    data = model.OrgStatusId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.SubventedIndicatorId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "orgMaster.SubventedIndicator",
                    data = model.SubventedIndicatorId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }

            if (!String.IsNullOrEmpty(model.RegistrationId))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.RegistrationType1",
                        data = model.RegistrationId,
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.RegistrationType2",
                        data = model.RegistrationId,
                        op = WhereOperation.Equal.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.RegistrationOtherName))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "orgMaster.RegistrationOtherName1",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "orgMaster.RegistrationOtherName2",
                        data = model.RegistrationOtherName,
                        op = WhereOperation.Contains.ToEnumValue()
                    }
                }, GroupOp.OR);
            }
            if (!String.IsNullOrEmpty(model.SectionId))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "Section88Indicator",
                    data = model.SectionId.Equals("1") ? "true" : "false",
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.FDRef))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FdRef",
                    data = model.FDRef,
                    op = WhereOperation.Contains.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.WithholdingBeginDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingBeginDate",
                    data = model.WithholdingBeginDate,
                    op = WhereOperation.GreaterThanOrEqual.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.WithholdingEndDate))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "WithholdingEndDate",
                    data = model.WithholdingEndDate,
                    op = WhereOperation.LessThanOrEqual.ToEnumValue()
                });
            }
            if (!String.IsNullOrEmpty(model.Overdue))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "Overdue",
                        data =model.Overdue.Equals("1") ? "true" : "false",
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "Overdue",
                        data = "",
                        op = WhereOperation.NotEqual.ToEnumValue()
                    }
                }, GroupOp.AND);
            }

            if (!String.IsNullOrEmpty(model.Late))
            {
                grid.AddDefaultRule(new List<Rule>{
                    new Rule()
                    {
                        field = "Late",
                        data =model.Late.Equals("1") ? "true" : "false",
                        op = WhereOperation.Equal.ToEnumValue()
                    },
                    new Rule()
                    {
                        field = "Late",
                        data = "",
                        op = WhereOperation.NotEqual.ToEnumValue()
                    }
                }, GroupOp.AND);
            }

            if (!String.IsNullOrEmpty(model.FlagDayYear))
            {
                grid.AddDefaultRule(new Rule()
                {
                    field = "FdYear",
                    data = model.FlagDayYear,
                    op = WhereOperation.Equal.ToEnumValue()
                });
            }
            return grid;
        }

        private string getTWR(IList<FdEvent> fdEvents)
        {
            string result = "";
            if (fdEvents != null && fdEvents.Count() > 0)
            {
                foreach (var fdEvent in fdEvents)
                {
                    if (!String.IsNullOrEmpty(fdEvent.TWR))
                    {
                        var twr = _lookupService.GetDescription(LookupType.TWR, fdEvent.TWR);
                        if (!twr.Equals("Territory-wide") && !twr.Equals("TW"))
                        {
                            if (!String.IsNullOrEmpty(fdEvent.TwrDistrict))
                            {
                                var twrDistrict = _lookupService.GetDescription(LookupType.TWRDistrict, fdEvent.TwrDistrict);
                                result = result + twrDistrict + ",";
                            }
                        }
                        else
                        {
                            result = result + twr + ",";
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(fdEvent.TwrDistrict))
                        {
                            var twrDistrict = _lookupService.GetDescription(LookupType.TWRDistrict, fdEvent.TwrDistrict);
                            result = result + twrDistrict + ",";
                        }
                    }
                }
                result = result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            }
            return result;
        }

        private string getFlagDay(IList<FdEvent> fdEvents)
        {
            string result = "";
            if (fdEvents != null && fdEvents.Count() > 0)
            {
                foreach (var fdEvent in fdEvents)
                {
                    result = result + (fdEvent.FlagDay != null ? fdEvent.FlagDay.Value.ToString("dd/MM/yyyy") : "") + ",";
                }
                result = result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            }
            return result;
        }

        private string getPermitNum(IList<FdEvent> fdEvents)
        {
            string result = "";
            if (fdEvents != null && fdEvents.Count() > 0)
            {
                foreach (var fdEvent in fdEvents)
                {
                    result = result + fdEvent.PermitNum + ",";
                }
                result = result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            }
            return result;
        }

        private string ValidateFDReminderIssuedMustEarlierThanDeadline(FDAccountSummaryViewModel model)
        {
            StringBuilder error = new StringBuilder("");
            if (!String.IsNullOrEmpty(model.FirstReminderIssueDate) && !String.IsNullOrEmpty(model.FirstReminderDeadline))
            {
                if (!ValidateFDFirstReminderIssuedMustEarlierThanDeadline(model))
                {
                    error.Append(_messageService.GetMessage(SystemMessage.Error.Organisation.FirstReminderIssuedMustEarlierThanFirstReminderDeadline));
                }
            }

            if (!String.IsNullOrEmpty(model.SecondReminderIssueDate) && !String.IsNullOrEmpty(model.SecondReminderDeadline))
            {
                if (!ValidateFDSecondReminderIssuedMustEarlierThanDeadline(model))
                {
                    error.Append(_messageService.GetMessage(SystemMessage.Error.Organisation.SecondReminderIssuedMustEarlierThanSecondReminderDeadline));
                }
            }
            return error.ToString();
        }

        #endregion "FD A/C Summary"
    }
}