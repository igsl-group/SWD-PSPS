using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.Lookups;
using Psps.Services.ComplaintMasters;
using Psps.Services.FlagDays;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.PSPs;
using Psps.Services.Report;
using Psps.Services.SystemMessages;
using Psps.Services.UserLog;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.Mappings;
using Psps.Web.ViewModels.Enquiry;
using Psps.Web.ViewModels.Lookup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("Enquiry"), Route("{action=index}")]
    public class EnquiryController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserLogService _UserLogService;
        private readonly ILookupService _lookupService;
        private readonly IComplaintMasterService _complaintMasterService;
        private readonly IMessageService _messageService;
        private readonly IOrganisationService _organisationService;
        private readonly IReportService _reportService;
        private readonly IPspApprovalHistoryService _pspApprovalHistoryService;
        private readonly IFdApprovalHistoryService _fdApprovalHistoryService;

        public EnquiryController(IUnitOfWork unitOfWork, IUserLogService UserLogService, ILookupService lookupService, IComplaintMasterService complaintMasterService,
            IMessageService messageService, IOrganisationService organisationService, IReportService reportService,
            IPspApprovalHistoryService pspApprovalHistoryService, IFdApprovalHistoryService fdApprovalHistoryService)
        {
            this._unitOfWork = unitOfWork;
            this._UserLogService = UserLogService;
            this._lookupService = lookupService;
            this._complaintMasterService = complaintMasterService;
            this._messageService = messageService;
            this._organisationService = organisationService;
            this._reportService = reportService;
            this._pspApprovalHistoryService = pspApprovalHistoryService;
            this._fdApprovalHistoryService = fdApprovalHistoryService;
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        public ActionResult Index()
        {
            EnquiryViewModel model = new EnquiryViewModel();
            return View(model);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        [RuleSetForClientSideMessagesAttribute("default", "Create", "Update")]
        public ActionResult Search()
        {
            EnquiryComplaintSearchViewModel model = new EnquiryComplaintSearchViewModel();

            model.OrgStatus = this._lookupService.getAllLkpInCodec(LookupType.OrgStatus);
            model.SubventedIndicators = this._lookupService.getAllLkpInCodec(LookupType.YesNo);
            model.RegistrationTypes = this._lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);
            model.RecordTypes = this._lookupService.getAllLkpInCodec(LookupType.ComplaintRecordType);
            model.ComplaintSources = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);
            model.ActivityConcerns = this._lookupService.getAllLkpInCodec(LookupType.ComplaintActivityConcern);
            model.NonComplianceNatures = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.NonComplianceNature1 = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.NonComplianceNature2 = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.NonComplianceNature3 = this._lookupService.getAllLkpInCodec(LookupType.ComplaintNonComplianceNature);
            model.ProcessStatus = this._lookupService.getAllLkpInCodec(LookupType.ComplaintProcessStatus);
            return View(model);
        }

        [PspsAuthorize(Allow.ComplaintMaster)]
        public ActionResult New()
        {
            EnquiryViewModel model = new EnquiryViewModel();
            return View(model);
        }

        #region Report

        [PspsAuthorize(Allow.ComplaintReport)]
        [RuleSetForClientSideMessagesAttribute("default", "R8", "R9", "R10", "R11", "R12", "R13", "R14", "R15")]
        public ActionResult Report()
        {
            EnquiryReportViewModel model = new EnquiryReportViewModel();
            model.R14_ComplaintSources = this._lookupService.getAllLkpInCodec(LookupType.ComplaintSource);

            return View(model);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r8/generate", Name = "R8Generate")]
        public JsonResult R8Generate([CustomizeValidator(RuleSet = "default,R8")] EnquiryReportViewModel model)
        {
            if (!ModelState.IsValid) return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);

            var reportId = "R08";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR8Excel(templatePath, model.R8_DateFrom, model.R8_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r9/generate", Name = "R9Generate")]
        public JsonResult R9Generate([CustomizeValidator(RuleSet = "default,R9")] EnquiryReportViewModel model)
        {
            if (!ModelState.IsValid) return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);

            var reportId = "R09";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR9Excel(templatePath, model.R9_DateFrom, model.R9_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r10/generate", Name = "R10Generate")]
        public JsonResult R10Generate([CustomizeValidator(RuleSet = "default,R10")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            if (!ModelState.IsValid) return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);

            var reportId = "R10";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var ms = _reportService.GenerateR10Excel(templatePath, model.R10_DateFrom, model.R10_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", ms);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r11/generate", Name = "R11Generate")]
        public JsonResult R11Generate([CustomizeValidator(RuleSet = "default,R11")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R11";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR11Excel(templatePath, model.R11_DateFrom, model.R11_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r12/generate", Name = "R12Generate")]
        public JsonResult R12Generate([CustomizeValidator(RuleSet = "default,R12")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R12";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR12Excel(templatePath, model.R12_DateFrom, model.R12_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r13/generate", Name = "R13Generate")]
        public JsonResult R13Generate([CustomizeValidator(RuleSet = "default,R13")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);

            var reportId = "R13";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR13Excel(templatePath, model.R13_DateFrom, model.R13_DateTo);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r14/generate", Name = "R14Generate")]
        public JsonResult R14Generate([CustomizeValidator(RuleSet = "default")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);

            var reportId = "R14";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR14Excel(templatePath, model.R14_FromYear, model.R14_ToYear, model.R14_ComplaintSource);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r15/generate", Name = "R15Generate")]
        public JsonResult R15Generate([CustomizeValidator(RuleSet = "default")] EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R15";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR15Excel(templatePath);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r16/generate", Name = "R16Generate")]
        public JsonResult R16Generate(EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R16";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR16Excel(templatePath, model.R16_Date);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r18/generate", Name = "R18Generate")]
        public JsonResult R18Generate(EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R18";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR18Excel(templatePath);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        [PspsAuthorize(Allow.ComplaintReport)]
        [HttpPost, Route("~/api/report/r19/generate", Name = "R19Generate")]
        public JsonResult R19Generate(EnquiryReportViewModel model)
        {
            Ensure.Argument.NotNull(model);
            var reportId = "R19";
            var templatePath = Server.MapPath("~/Reports/" + reportId + ".xlsx");
            var memoryStream = _reportService.GenerateR19Excel(templatePath);
            return JsonFileResult(reportId, reportId + ".xlsx", memoryStream);
        }

        #endregion Report
    }
}