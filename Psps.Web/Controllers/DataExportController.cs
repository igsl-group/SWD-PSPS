using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Report;
using Psps.Services.Security;
using Psps.Services.SystemMessages;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Extensions;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.DataExport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("DataExport"), Route("{action=index}")]
    public class DataExportController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IRoleService _roleService;
        private readonly IReportService _reportService;

        public DataExportController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IMessageService messageService, IRoleService roleService,
            IReportService reportService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._messageService = messageService;
            this._roleService = roleService;
            this._reportService = reportService;
        }

        [PspsAuthorize(Allow.DataExport)]
        [HttpGet, Route(Name = "DataExportUrl")]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult Index()
        {
            DataExportViewModel model = new DataExportViewModel()
            {
                Tables = GetExportList()
            };
            return View(model);
        }

        [PspsAuthorize(Allow.DataExport)]
        [HttpPost, Route("ExportAll", Name = "ExportAll")]
        public FileResult ExportAll()
        {
            string filePath = "";
            string tempFolderPath = Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            string zFileName = "";

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            zFileName = time + ".zip";

            Dictionary<string, string> Tables = GetExportList();

            filePath = _reportService.ExportTablesToZipFile(tempFolderPath, zFileName, Tables.Keys.ToList());

            return FileDownload(filePath, zFileName);
        }

        [PspsAuthorize(Allow.DataExport)]
        [HttpPost, Route("ExportSelected", Name = "ExportSelected")]
        public FileResult ExportSelected(DataExportViewModel model)
        {
            string filePath = "";
            string tempFolderPath = Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            string zFileName = "";

            var time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            zFileName = time + ".zip";

            if (model.TablesToBeExport != null)
            {
                filePath = _reportService.ExportTablesToZipFile(tempFolderPath, zFileName, model.TablesToBeExport.ToList());
            }

            return FileDownload(filePath, zFileName);
        }

        private Dictionary<string, string> GetExportList()
        {
            Dictionary<string, string> listExport = new Dictionary<string, string>();

            listExport.Add("PspMaster", "PSP Master");
            listExport.Add("FdMaster", "FD Master");
            listExport.Add("Acting", "Acting");
            listExport.Add("DisasterStatistics", "Disaster Statistics");
            listExport.Add("ComplaintMaster", "Complaint Master");
            listExport.Add("PspApprovalHistory", "PSP Approval History");
            listExport.Add("PspAttachment", "PSP Attachment");
            listExport.Add("DisasterMaster", "Disaster Master");
            listExport.Add("OrgAttachment", "Organization Attachment");
            listExport.Add("OrgNameChangeHistory", "Organization Name Change History");
            listExport.Add("OrgMaster", "Organization Master");
            //listExport.Add("FunctionsInRoles", "");
            listExport.Add("[Function]", "Function");
            //listExport.Add("OrgRefGuidePromulgation", "");
            listExport.Add("DocumentLibrary", "Document Library");
            listExport.Add("ActivityLog", "Activity Log");
            //listExport.Add("OrgProvisionNotAdopt", "");
            listExport.Add("Letter", "Letter");
            listExport.Add("ComplaintOtherDepartmentEnquiry", "Complaint Other Department Enquiry");
            listExport.Add("Document", "Document");
            listExport.Add("ComplaintPoliceCase", "Complaint Police Case");
            listExport.Add("[Lookup]", "Lookup");
            listExport.Add("Post", "Post");
            //listExport.Add("PostsInRoles", "");
            listExport.Add("ComplaintAttachment", "Complaint Attachment");
            listExport.Add("[Rank]", "Rank");
            //listExport.Add("RevInfo", "");
            listExport.Add("[Role]", "Role");
            //listExport.Add("SystemMessage", "System Message");
            //listExport.Add("SystemParameter", "System Parameter");
            //listExport.Add("User", "User");
            listExport.Add("SuggestionAttachment", "Suggestion Attachment");
            listExport.Add("SuggestionDoc", "Suggestion Document");
            listExport.Add("PspDoc", "Psp Doc");
            listExport.Add("LegalAdviceDoc", "Legal Advice Document");
            listExport.Add("FdEvent", "FD Event");
            listExport.Add("ComplaintDoc", "Complaint Document");
            listExport.Add("LegalAdviceMaster", "Legal Advice Master");
            listExport.Add("OrgDoc", "Organization Document");
            listExport.Add("FdDoc", "FD Document");
            listExport.Add("SuggestionMaster", "Suggestion Master");
            listExport.Add("FdList", "FD List");
            listExport.Add("FdAttachment", "FD Attachment");
            listExport.Add("PublicHoliday", "Public Holiday");
            listExport.Add("FdApprovalHistory", "Fd Approval History");
            listExport.Add("PspEvent", "PSP Event");
            listExport.Add("ComplaintFollowUpAction", "Complaint FollowUp Action");
            listExport.Add("ComplaintTelRecord", "Complaint Tel Record");

            return listExport;
        }
    }
}