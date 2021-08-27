using OfficeOpenXml;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.Lookups;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.Organisations;
using Psps.Services.UserLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Services.FlagDays
{
    public partial class FlagDayService : IFlagDayService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IOrganisationService _organisationService;
        private readonly IFDMasterRepository _fdMasterRepository;
        private readonly IFdEventRepository _fdEventRepository;
        private readonly IFlagDayListService _flagDayListService;
        private readonly IFdEventService _fdEventService;
        private readonly IFdApprovalHistoryRepository _fdApprovalHistoryRepository;
        private readonly ILookupService _lookupService;
        private readonly IOrgMasterRepository _orgMasterRepository;
        private readonly IFdDocSummaryViewRepository _fdDocSummaryViewRepository;
        private readonly IFdAttachmentRepository _fdAttachmentRepository;
        private readonly IFdAcSummaryViewRepository _fdAcSummaryViewRepository;
        private readonly ISystemParameterRepository _systemParameterRepository;
        private readonly IFdDocRepository _fdDocRepository;
        private readonly IFdApproveApplicationListGridViewRepository _fdApproveApplicationListGridViewRepository;
        private readonly IOrgFdTabGridViewRepository _orgFdTabGridViewRepository;
        private readonly IFdExportViewRepository _fdExportViewRepository;

        private readonly IDictionary<string, string> languages;
        private readonly IDictionary<string, string> engSalutes;
        private readonly IDictionary<string, string> fdApplicationResults;
        private readonly IDictionary<string, string> twrs;
        private readonly IDictionary<string, string> fdGroupings;
        private readonly IDictionary<string, string> fdLotResults;
        private readonly IDictionary<string, string> twrDistricts;
        private readonly IDictionary<string, string> docSubmissions;
        private readonly IDictionary<string, string> collectionMethods;
        private readonly IDictionary<string, string> fdEventRemarks;

        private readonly IUserLogService _userLogService;

        #endregion Fields

        #region Ctor

        public FlagDayService(IEventPublisher eventPublisher, IFDMasterRepository fdMasterRepository, IOrganisationService organisationService,
                                IFdEventRepository fdEventRepository, ILookupService lookupService, IFdApprovalHistoryRepository fdApprovalHistoryRepository,
                                IOrgMasterRepository orgMasterRepository, IFdDocSummaryViewRepository fdDocSummaryViewRepository,
                                IFdAttachmentRepository fdAttachmentRepository, IFdAcSummaryViewRepository fdAcSummaryViewRepository,
                                IFlagDayListService flagDayListService, ISystemParameterRepository systemParameterRepository, IFdEventService fdEventService,
                                IFdDocRepository fdDocRepository, IFdApproveApplicationListGridViewRepository fdApproveApplicationListGridViewRepository, IUserLogService userLogService,
                                IOrgFdTabGridViewRepository orgFdTabGridViewRepository, IFdExportViewRepository fdExportViewRepository)
        {
            this._eventPublisher = eventPublisher;
            this._fdMasterRepository = fdMasterRepository;
            this._organisationService = organisationService;
            this._flagDayListService = flagDayListService;
            this._fdEventRepository = fdEventRepository;
            this._fdApprovalHistoryRepository = fdApprovalHistoryRepository;
            this._lookupService = lookupService;
            this._orgMasterRepository = orgMasterRepository;
            this._fdDocSummaryViewRepository = fdDocSummaryViewRepository;
            this._fdAttachmentRepository = fdAttachmentRepository;
            this._fdAcSummaryViewRepository = fdAcSummaryViewRepository;
            this._fdDocRepository = fdDocRepository;
            this._fdApproveApplicationListGridViewRepository = fdApproveApplicationListGridViewRepository;

            this._systemParameterRepository = systemParameterRepository;
            this._fdEventService = fdEventService;

            this.languages = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
            if (languages.Count == 0) languages.Add("", "");
            this.engSalutes = _lookupService.getAllLkpInCodec(LookupType.Salute);
            if (engSalutes.Count == 0) engSalutes.Add("", "");
            this.fdApplicationResults = _lookupService.getAllLkpInCodec(LookupType.FdApplicationResult);
            if (fdApplicationResults.Count == 0) fdApplicationResults.Add("", "");
            this.twrs = _lookupService.getAllLkpInCodec(LookupType.TWR);
            if (twrs.Count == 0) twrs.Add("", "");
            this.fdGroupings = _lookupService.getAllLkpInCodec(LookupType.FdGrouping);
            if (fdGroupings.Count == 0) fdGroupings.Add("", "");
            this.fdLotResults = _lookupService.getAllLkpInCodec(LookupType.FdLotResult);
            if (fdLotResults.Count == 0) fdLotResults.Add("", "");
            this.twrDistricts = _lookupService.getAllLkpInCodec(LookupType.TWRDistrict);
            if (twrDistricts.Count == 0) twrDistricts.Add("", "");
            this.docSubmissions = _lookupService.getAllLkpInCodec(LookupType.FdDocSubmitted);
            if (docSubmissions.Count == 0) docSubmissions.Add("", "");
            this.collectionMethods = _lookupService.getAllLkpInCodec(LookupType.FdCollectionMethod);
            if (collectionMethods.Count == 0) collectionMethods.Add("", "");
            this.fdEventRemarks = _lookupService.getAllLkpInChiCodec(LookupType.FdEventRemark);

            this._userLogService = userLogService;
            this._orgFdTabGridViewRepository = orgFdTabGridViewRepository;
            this._fdExportViewRepository = fdExportViewRepository;
        }

        #endregion Ctor

        #region Methods

        public FdMaster GetFDById(int fdId)
        {
            return _fdMasterRepository.Get(u => u.FdMasterId == fdId);
        }

        public IPagedList<FdMaster> GetPage(GridSettings grid)
        {
            return _fdMasterRepository.GetPage(grid);
        }

        public void CreateFdMaster(FdMaster model)
        {
            _fdMasterRepository.Add(model);
            _eventPublisher.EntityInserted<FdMaster>(model);
        }

        public void UpdateFdMaster(FdMaster model)
        {
            _fdMasterRepository.Update(model);
            _eventPublisher.EntityUpdated<FdMaster>(model);
        }

        public void UpdateFdMaster(FdMaster oldFDMaster, FdMaster newFDMaster)
        {
            Ensure.Argument.NotNull(oldFDMaster, "Old Flag Day");
            Ensure.Argument.NotNull(newFDMaster, "New Flag Day");

            _userLogService.LogFlagDayInformation(oldFDMaster, newFDMaster);

            _fdMasterRepository.Update(newFDMaster);
            _eventPublisher.EntityUpdated<FdMaster>(newFDMaster);
        }

        public int GetFdTabGridViewAmountByOrgId(string OrgId)
        {
            Ensure.Argument.NotNullOrEmpty(OrgId, "OrgId");
            return _orgFdTabGridViewRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId));
        }

        public IPagedList<OrgFdTabGridView> GetPageByOrgFdTabGridView(GridSettings grid)
        {
            return _orgFdTabGridViewRepository.GetPage(grid);
        }

        public IPagedList<FlagDaySearchDto> GetPageByFlagDaySearchDto(GridSettings grid)
        {
            return _fdMasterRepository.GetPageByFlagDaySearchDto(grid);
        }

        public IPagedList<FdMaster> GetPage(GridSettings grid, string permitNum)
        {
            return _fdMasterRepository.GetPage(grid, permitNum);
        }

        public string GetMaxSeq(string fdYear)
        {
            //var table = this.Table.Where(x => x.PspYear == pspYear);
            return _fdMasterRepository.GetMaxSeq(fdYear);
        }

        public string genPermitNo(string fdYear, string TWR)
        {
            //var table = this.Table.Where(x => x.PspYear == pspYear);
            return _fdMasterRepository.genPermitNo(fdYear, TWR);
        }

        public string switchToPreviousFdYear(string fdYear)
        {
            return (Convert.ToInt16(fdYear.Substring(0, 2)) - 1).ToString().PadLeft(2, '0') +
                    "-" + fdYear.Substring(0, 2);
        }

        public FdMaster GetFdMasterByFdYearAndOrg(string FdYear, OrgMaster OrgMaster)
        {
            return _fdMasterRepository.GetFdMasterByFdYearAndOrg(FdYear, OrgMaster);
        }

        public String GetFdBenchmarkStatusByFdYearAndOrg(string FdYear, OrgMaster OrgMaster,
                                                         IDictionary<string, string> BenchmarkTWFD,
                                                         IDictionary<string, string> BenchmarkRFD)
        {
            String FdBenchmarkStatus = "FD" + FdYear;
            FdMaster fdMaster = GetFdMasterByFdYearAndOrg(FdYear, OrgMaster);
            try
            {
                FdBenchmarkStatus += (fdMaster.ApplyPledgingMechanismIndicator == true ? "(P)" : "").ToString();

                FdEvent fdEvent = fdMaster.FdEvent.FirstOrDefault();

                int intBenchmark = 0;

                if (fdEvent.TWR == "1")
                {
                    intBenchmark = Convert.ToInt32(BenchmarkTWFD[FdYear]);
                }
                else // if (fdEvent.TWR == "2")
                {
                    intBenchmark = Convert.ToInt32(BenchmarkRFD[FdYear]);
                }

                if (fdMaster.NetProceed != null)
                {
                    FdBenchmarkStatus += ": " + (fdMaster.NetProceed >= intBenchmark ? "Meet" : "Below").ToString();
                }
                else
                {
                    FdBenchmarkStatus += ": O/S";
                }
            }
            catch
            {
                FdBenchmarkStatus += ": N/A";
            }

            return FdBenchmarkStatus;
        }

        public MemoryStream GetPageToXls(GridSettings Grid)
        {
            //local vars
            IDictionary<string, IDictionary<string, string>> tilteAndComment = new Dictionary<string, IDictionary<string, string>>();

            var fdExportView = _fdExportViewRepository.GetExportData(Grid);

            // Lookup Table
            var engSalutes = _lookupService.getAllLkpInCodec(LookupType.Salute);
            var chiSalutes = _lookupService.getAllLkpInChiCodec(LookupType.Salute);
            var orgRegistrationType = _lookupService.getAllLkpInCodec(LookupType.OrgRegistrationType);

            /*
             * PIR #103
             * The position of columns “CG” (i.e. “PledgedAmt”) and “DM” (i.e. “PledgingAmt”) on the FdMaster should be swapped.
             */
            //// 2015/06/25
            // Columns which needed to display, and the display ordering (FdMaster, FdEvent)
            String[] l_arrHeader =
            { "FdMasterId", "FdYear", "ApplicationReceiveDate", "FdRef", "ApplyForTwr",
              "OrgMaster",
              "ContactPersonSalute", "ContactPersonFirstName", "ContactPersonLastName",
              "ContactPersonChiFirstName", "ContactPersonChiLastName", "ContactPersonPosition",
              "ContactPersonTelNum", "ContactPersonFaxNum", "ContactPersonEmailAddress",
              "JointApplicationIndicator", "CommunityChest", "ConsentLetter", "NewApplicantIndicator",
              "TrackRecordStartDate", "TrackRecordEndDate", "TrackRecordDetails",
              "AfsRecordStartDate", "AfsRecordEndDate", "AfsRecordDetails",
              "TargetIncome", "FundRaisingPurpose", "ChiFundRaisingPurpose", "UsedLanguage",
              "FdGroup", "FdGroupPercentage", "GroupingResult", "VettingPanelCaseIndicator", "ReviewCaseIndicator",
              "ApplicationRemark", "ApplicationResult", "RefLotGroup", "LotGroup", "FdLotNum", "FdLotResult", "PriorityNum",
              "ApplyPledgingMechanismIndicator", "PledgingAmt", "PledgingProposal", "ChiPledgingProposal", "PledgingApplicationRemark",
              "FlagDay", "FlagTimeFrom", "FlagTimeTo", "TWR", "TwrDistrict",
              "CollectionMethod", "PermitNum", "PermitIssueDate", "PermitAcknowledgementReceiveDate", "PermitRevokeIndicator", "FlagColour", "BagColour", "Remarks",
              "DocSubmission", "SubmissionDueDate",
              "FirstReminderIssueDate", "FirstReminderDeadline", "SecondReminderIssueDate", "SecondReminderDeadline",
              "AuditReportReceivedDate", "PublicationReceivedDate", "DocReceiveRemark", "DocRemark",
              "StreetCollection", "GrossProceed", "Expenditure", "NetProceed", "NewspaperPublishDate",
              "PledgedAmt", "AcknowledgementEmailIssueDate", "WithholdingListIndicator", "WithholdingBeginDate", "WithholdingEndDate",
              "AfsReceiveIndicator", "RequestPermitteeIndicator", "AfsReSubmitIndicator", "AfsReminderIssueIndicator", "Remark"};

            // Column Ordering (OrgMaster)
            String[] l_arrOrgCol =
            { "OrgMaster", "EngOrgName", "ChiOrgName",
              "EngRegisteredAddress1", "EngRegisteredAddress2", "EngRegisteredAddress3", "EngRegisteredAddress4", "EngRegisteredAddress5",
              "EngMailingAddress1", "EngMailingAddress2", "EngMailingAddress3", "EngMailingAddress4", "EngMailingAddress5",
              "ChiRegisteredAddress1", "ChiRegisteredAddress2", "ChiRegisteredAddress3", "ChiRegisteredAddress4", "ChiRegisteredAddress5",
              "ChiMailingAddress1", "ChiMailingAddress2", "ChiMailingAddress3", "ChiMailingAddress4", "ChiMailingAddress5",
              "URL", "TelNum", "FaxNum", "EmailAddress",
              "EngApplicantSalute", "ApplicantLastName", "ApplicantFirstName",
              "ChiApplicantSalute", "ApplicantChiLastName", "ApplicantChiFirstName",
              "ApplicantTelNum", "PresidentName", "SecretaryName", "TreasurerName", "OrgObjective",
              "SubventedIndicator", "Section88Indicator", "RegistrationType", "AddressProofIndicator", "MaaConstitutionIndicator" };

            String[] l_arrExample =
            {
                "142", "16-17", "24/04/2015", "006(2016/17)", "Regional", "ORG0058", "The Version Seven", "第七版", "36/F., Sunlight Tower,", "248 Queen's Road East,", "Wanchai, Hong Kong", "", "",
                "Room 3601-02, 36/F., Sunlight Tower,", "248 Queen's Road East,", "Wanchai, Hong Kong", "", "", "香港灣仔", "皇后大道東248號", "陽光中心36樓", "", "", "香港灣仔", "皇后大道東248號", "陽光中心36樓3601-02室", "", "",
                "www.versionseven.org", "12345678", "12345678", "V7@version.com", "Dr", "Seven", "CHAN", "博士", "七", "陳", "23456789", "Seven Chan", "Hello Chan", "Today Chan", "Mission Impossible", "Yes", "Yes", "Companies Ordinace",
                "Yes", "Yes", "Mr", "Sang", "CHAN", "生", "陳", "Manager", "23456780", "23456781", "sang@versionseven.com", "No", "No", "No", "No", "01/04/2010", "31/03/2014", "Service Records (2010-2014)", "01/03/2011", "31/01/2015",
                "Annual Financial Report", "1000000", "Serve the public", "為人民服務", "Chinese", "A", "", "", "No", "No", "", "Eligible", "", "RA4", "12", "Successful", "", "No", "", "", "", "", "01/04/2016", "0700", "1230",
                "Regional", "NT", "Flag Bag", "R001", "", "", "No", "", "", "", "", "30/06/2016", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };

            String[] l_format =
            {
                "", "yy-yy", "dd/mm/yyyy", "", "TW/Regional (refer to lookup 'TWR')",
                "Org. Ref.", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "Dr/Mr/Mrs…(refer to lookup 'Salute')", "text", "text", "text", "text", "text", "text", "text", "text", "Yes/No", "Yes/No", "Yes/No", "Yes/No", "dd/mm/yyyy", "dd/mm/yyyy", "text", "dd/mm/yyyy", "dd/mm/yyyy", "text", "numeric", "text", "text",
                "English/Chinese (refer to lookup 'Language Used')", "A/B (refer to lookup '(FD) Grouping')", "numeric", "text", "Yes/No", "Yes/No", "text", "Eligible/Ineligible etc.. (refer to lookup '(FD) Application Result')", "", "text", "text",
                "Successful/Waiting (refer to lookup '(FD) Lot Result')", "text", "Yes/No", "numeric", "text", "text", "text", "dd/mm/yyyy", "dd/mm/yyyy", "dd/mm/yyyy", "TW/Regional (refer to lookup '(FD) Territiry-wide / Regional')",
                "HKI/KLN/NT (refer to lookup '(FD) Regional - Region')", "Octopus card readers/Flag Bag (refer to lookup '(FD) Collection Method')", "", "dd/mm/yyyy", "dd/mm/yyyy", "Yes/No", "text", "text",
                "(refer to the code value of lookup '(FD) Event Remark')", "(refer to lookup '(FD) Document Submitted')", "dd/mm/yyyy", "dd/mm/yyyy", "", "dd/mm/yyyy", "", "dd/mm/yyyy", "dd/mm/yyyy", "text", "text", "numeric", "numeric", "numeric",
                "", "dd/mm/yyyy", "numeric", "dd/mm/yyyy", "Yes/No", "dd/mm/yyyy", "dd/mm/yyyy", "Yes/No", "Yes/No", "Yes/No", "Yes/No", "text"
            };

            String[] l_bgColor =
            {
                "BasicInformation", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "ApplicationDetails", "", "", "", "", "", "", "", "", "", "", "", "", "", "VettingDetails", "", "", "", "", "", "ApplicationResult", "", "", "", "", "", "PledgingApplicationResult", "", "", "", "", 
                "FDEventDetails", "", "", "", "", "", "", "", "", "", "", "", "", "ACSummary", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };

            Dictionary<string, System.Drawing.Color> colorMap = new Dictionary<string, System.Drawing.Color>() {
                { "BasicInformation", System.Drawing.Color.FromArgb(253, 233, 217) },
                { "ApplicationDetails", System.Drawing.Color.FromArgb(218, 238, 243) },
                { "VettingDetails", System.Drawing.Color.FromArgb(228, 223, 236) },
                { "ApplicationResult", System.Drawing.Color.FromArgb(235, 241, 222) },
                { "PledgingApplicationResult", System.Drawing.Color.FromArgb(242, 220, 219) },
                { "FDEventDetails", System.Drawing.Color.FromArgb(220, 230, 241) },
                { "ACSummary", System.Drawing.Color.FromArgb(221, 217, 196) }
            };

            // Get all of the columns property of FdExportView and filter and sort by the defined array list
            var propsFd = typeof(FdExportView).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)
            .Select(x => new
            {
                Property = x
            })
            .OrderBy(x => Array.IndexOf(l_arrHeader, x.Property.Name))
            .Select(x => x.Property).Where(x => Array.IndexOf(l_arrHeader, x.Name) > -1)
            .ToArray();

            // Define a String List for storing the Column Header
            List<string> headers = propsFd.Select(x => x.Name).ToList();

            tilteAndComment.Add("UsedLanguage", languages);
            tilteAndComment.Add("ContactPersonSalute", engSalutes);
            tilteAndComment.Add("ApplicationResult", fdApplicationResults);
            tilteAndComment.Add("ApplyForTwr", twrs);
            tilteAndComment.Add("TWR", twrs);
            tilteAndComment.Add("FdGroup", fdGroupings);
            tilteAndComment.Add("FdLotResult", fdLotResults);
            tilteAndComment.Add("TwrDistrict", twrDistricts);
            tilteAndComment.Add("DocSubmission", docSubmissions);

            var path = Path.Combine(Path.GetTempPath(), "fdMaster.xlsx");
            FileInfo tempfile = new FileInfo(path);
            MemoryStream resultStream = new MemoryStream();
            using (var package = new ExcelPackage(tempfile))
            {
                package.Workbook.Worksheets.Add("FdMaster");
                ExcelWorksheet ws = package.Workbook.Worksheets[1];

                const int example = 1;
                const int format = 2;
                const int header = 3;

                for (int i = 0, j = 1; i < l_arrExample.Count(); i++, j++)
                {
                    // Write the example
                    ws.Cells[example, j].Value = l_arrExample[i];
                }

                for (int i = 0, j = 1; i < l_format.Count(); i++, j++)
                {
                    // Write the format
                    ws.Cells[format, j].Value = l_format[i];
                }

                for (int i = 0, j = 1; i < headers.Count; i++, j++)
                {
                    // Write the Column Headers
                    ws.Cells[header, j].Value = headers[i];
                    if (headers[i] == "OrgMaster")
                    {
                        foreach (String orgCol in l_arrOrgCol)
                        {
                            ws.Cells[header, j].Value = orgCol;
                            j++;
                        }
                        j--;
                    }

                    if (tilteAndComment.Keys.Contains(headers[i]))
                    {
                        string comment = "";
                        foreach (var desp in tilteAndComment[headers[i]]) { comment = comment + desp.Value + System.Environment.NewLine; }
                    }
                }

                SetXlsStyle(ws.Cells[example, 1, format, l_arrExample.Count()], System.Drawing.Color.FromArgb(255, 255, 255), System.Drawing.Color.FromArgb(0, 0, 0));
                SetXlsStyle(ws.Cells[header, 1, header, l_arrExample.Count()], System.Drawing.Color.FromArgb(255, 255, 0), System.Drawing.Color.FromArgb(151, 71, 6));

                // data
                ws.Column(3).Style.Numberformat.Format = "dd/mm/yyyy";// ApplicationReceiveDate

                string bgColor = string.Empty;
                int bgColorIndex = 0;
                for (int i = 0; i < l_bgColor.Count(); i++)
                {
                    if (l_bgColor[i].IsNotNullOrEmpty() && bgColor != l_bgColor[i])
                    {
                        if (i > bgColorIndex)
                        {
                            SetXlsStyle(ws.Cells[header + 1, bgColorIndex + 1, header + fdExportView.Count, i + 1], null, colorMap[bgColor]);
                        }
                        bgColor = l_bgColor[i];
                        bgColorIndex = i;
                    }
                }

                SetXlsStyle(ws.Cells[header + 1, bgColorIndex + 1, header + fdExportView.Count + 1, l_bgColor.Count()], null, colorMap[bgColor]);

                int rowIndex = 1;
                foreach (var fdExportData in fdExportView)
                {
                    var colIndex = 1;

                    // Loop all of the Columns
                    foreach (PropertyInfo prop in propsFd)
                    {
                        // OrgMaster's Columns
                        if (prop.Name == "OrgMaster")
                        {
                            foreach (String orgCol in l_arrOrgCol)
                            {
                                if (orgCol == "OrgMaster")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.OrgRef) ? "" : fdExportData.OrgMaster.OrgRef.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngOrgName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngOrgName) ? "" : fdExportData.OrgMaster.EngOrgName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiOrgName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiOrgName) ? "" : fdExportData.OrgMaster.ChiOrgName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngRegisteredAddress1")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngRegisteredAddress1) ? "" : fdExportData.OrgMaster.EngRegisteredAddress1.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngRegisteredAddress2")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngRegisteredAddress2) ? "" : fdExportData.OrgMaster.EngRegisteredAddress2.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngRegisteredAddress3")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngRegisteredAddress3) ? "" : fdExportData.OrgMaster.EngRegisteredAddress3.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngRegisteredAddress4")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngRegisteredAddress4) ? "" : fdExportData.OrgMaster.EngRegisteredAddress4.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngRegisteredAddress5")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngRegisteredAddress5) ? "" : fdExportData.OrgMaster.EngRegisteredAddress5.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngMailingAddress1")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngMailingAddress1) ? "" : fdExportData.OrgMaster.EngMailingAddress1.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngMailingAddress2")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngMailingAddress2) ? "" : fdExportData.OrgMaster.EngMailingAddress2.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngMailingAddress3")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngMailingAddress3) ? "" : fdExportData.OrgMaster.EngMailingAddress3.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngMailingAddress4")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngMailingAddress4) ? "" : fdExportData.OrgMaster.EngMailingAddress4.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngMailingAddress5")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EngMailingAddress5) ? "" : fdExportData.OrgMaster.EngMailingAddress5.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiRegisteredAddress1")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiRegisteredAddress1) ? "" : fdExportData.OrgMaster.ChiRegisteredAddress1.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiRegisteredAddress2")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiRegisteredAddress2) ? "" : fdExportData.OrgMaster.ChiRegisteredAddress2.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiRegisteredAddress3")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiRegisteredAddress3) ? "" : fdExportData.OrgMaster.ChiRegisteredAddress3.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiRegisteredAddress4")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiRegisteredAddress4) ? "" : fdExportData.OrgMaster.ChiRegisteredAddress4.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiRegisteredAddress5")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiRegisteredAddress5) ? "" : fdExportData.OrgMaster.ChiRegisteredAddress5.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiMailingAddress1")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiMailingAddress1) ? "" : fdExportData.OrgMaster.ChiMailingAddress1.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiMailingAddress2")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiMailingAddress2) ? "" : fdExportData.OrgMaster.ChiMailingAddress2.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiMailingAddress3")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiMailingAddress3) ? "" : fdExportData.OrgMaster.ChiMailingAddress3.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiMailingAddress4")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiMailingAddress4) ? "" : fdExportData.OrgMaster.ChiMailingAddress4.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiMailingAddress5")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ChiMailingAddress5) ? "" : fdExportData.OrgMaster.ChiMailingAddress5.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "URL")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.URL) ? "" : fdExportData.OrgMaster.URL.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "TelNum")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.TelNum) ? "" : fdExportData.OrgMaster.TelNum.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "FaxNum")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.FaxNum) ? "" : fdExportData.OrgMaster.FaxNum.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EmailAddress")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.EmailAddress) ? "" : fdExportData.OrgMaster.EmailAddress.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "EngApplicantSalute")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantSalute) ? "" : engSalutes[fdExportData.OrgMaster.ApplicantSalute];
                                    colIndex++;
                                }
                                else if (orgCol == "ApplicantLastName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantLastName) ? "" : fdExportData.OrgMaster.ApplicantLastName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ApplicantFirstName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantFirstName) ? "" : fdExportData.OrgMaster.ApplicantFirstName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ChiApplicantSalute")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantSalute) ? "" : chiSalutes[fdExportData.OrgMaster.ApplicantSalute];
                                    colIndex++;
                                }
                                else if (orgCol == "ApplicantChiLastName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantChiLastName) ? "" : fdExportData.OrgMaster.ApplicantChiLastName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ApplicantChiFirstName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantChiFirstName) ? "" : fdExportData.OrgMaster.ApplicantChiFirstName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "ApplicantTelNum")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.ApplicantTelNum) ? "" : fdExportData.OrgMaster.ApplicantTelNum.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "PresidentName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.PresidentName) ? "" : fdExportData.OrgMaster.PresidentName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "SecretaryName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.SecretaryName) ? "" : fdExportData.OrgMaster.SecretaryName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "TreasurerName")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.TreasurerName) ? "" : fdExportData.OrgMaster.TreasurerName.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "OrgObjective")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.OrgObjective) ? "" : fdExportData.OrgMaster.OrgObjective.ToString();
                                    colIndex++;
                                }
                                else if (orgCol == "SubventedIndicator")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                        fdExportData.OrgMaster == null ? "" : (fdExportData.OrgMaster.SubventedIndicator == true ? "Yes" : "No");
                                    colIndex++;
                                }
                                else if (orgCol == "Section88Indicator")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                        fdExportData.OrgMaster.Section88Indicator == null ? "" : (fdExportData.OrgMaster.Section88Indicator == true ? "Yes" : "No");
                                    colIndex++;
                                }
                                else if (orgCol == "RegistrationType")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                    string.IsNullOrEmpty(fdExportData.OrgMaster.RegistrationType1) ? "" : orgRegistrationType[fdExportData.OrgMaster.RegistrationType1.ToString()];
                                    ws.Cells[rowIndex + header, colIndex].Value = ws.Cells[rowIndex + header, colIndex].Value +
                                    (string.IsNullOrEmpty(fdExportData.OrgMaster.RegistrationType1) || string.IsNullOrEmpty(fdExportData.OrgMaster.RegistrationType2) ? "" : ", ") +
                                    (string.IsNullOrEmpty(fdExportData.OrgMaster.RegistrationType2) ? "" : orgRegistrationType[fdExportData.OrgMaster.RegistrationType2.ToString()]);
                                    colIndex++;
                                }
                                else if (orgCol == "AddressProofIndicator")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                        fdExportData.OrgMaster.AddressProofIndicator == null ? "" : (fdExportData.OrgMaster.AddressProofIndicator == true ? "Yes" : "No");
                                    colIndex++;
                                }
                                else if (orgCol == "MaaConstitutionIndicator")
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value =
                                        fdExportData.OrgMaster.MaaConstitutionIndicator == null ? "" : (fdExportData.OrgMaster.MaaConstitutionIndicator == true ? "Yes" : "No");
                                    colIndex++;
                                }
                            }
                            colIndex--;
                        }
                        else if (prop.Name == "ContactPersonSalute")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : engSalutes[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "UsedLanguage")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : languages[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "FdGroup")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : fdGroupings[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "ApplicationResult")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : fdApplicationResults[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "FdLotResult")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : fdLotResults[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "DocSubmission")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : docSubmissions[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        else if (prop.Name == "ApplyForTwr" || prop.Name == "TWR")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : twrs[prop.GetValue(fdExportData, null).ToString()] : "";
                        }
                        //else if (prop.Name == "TwrDistrict")
                        //{
                        //    ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : twrDistricts[prop.GetValue(fdExportData, null).ToString()] : "";
                        //}
                        else if (prop.Name == "CollectionMethod")
                        {
                            if (prop.GetValue(fdExportData, null) != null && !string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()))
                            {
                                var value = prop.GetValue(fdExportData, null).ToString();
                                Regex reg = new Regex(",");
                                Match match = reg.Match(value);
                                if (match.Success)
                                {
                                    var arrValue = value.Split(',');
                                    for (var x = 0; x < arrValue.Length; x++)
                                    {
                                        arrValue[x] = collectionMethods[arrValue[x]];
                                    }
                                    value = string.Join(",", arrValue);
                                    ws.Cells[rowIndex + header, colIndex].Value = value;
                                }
                                else
                                {
                                    ws.Cells[rowIndex + header, colIndex].Value = collectionMethods[value];
                                }
                            }
                        }
                        else if (prop.GetValue(fdExportData, null) != null && prop.PropertyType.FullName.Contains(typeof(System.Boolean).FullName))
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : Convert.ToBoolean(prop.GetValue(fdExportData, null)) == true ? "Yes" : "No" : "";
                        }
                        else if (prop.GetValue(fdExportData, null) != null && prop.PropertyType.FullName.Contains(typeof(System.DateTime).FullName) && prop.Name != "FlagTimeFrom" && prop.Name != "FlagTimeTo")
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? "" : string.Format("{0:dd/MM/yyyy }", prop.GetValue(fdExportData, null)).ToString() : "";
                        }
                        else if (prop.GetValue(fdExportData, null) != null && prop.PropertyType.FullName.Contains(typeof(System.DateTime).FullName) && (prop.Name == "FlagTimeFrom" || prop.Name == "FlagTimeTo"))
                        {
                            DateTime? dt = prop.GetValue(fdExportData, null) != null ? string.IsNullOrEmpty(prop.GetValue(fdExportData, null).ToString()) ? (DateTime?)null : Convert.ToDateTime(prop.GetValue(fdExportData, null)) : null;
                            if (dt != null)
                                ws.Cells[rowIndex + header, colIndex].Value = dt.Value.ToString("HH:mm").Replace(":", "");
                        }
                        else
                        {
                            ws.Cells[rowIndex + header, colIndex].Value = prop.GetValue(fdExportData, null) == null ? "" : prop.GetValue(fdExportData, null).ToString();
                        }

                        colIndex++;
                    } // End For each prop

                    rowIndex++;
                } // End for each row

                package.SaveAs(resultStream);
            }
            return resultStream;
        }

        public MemoryStream InserFlagDayByImportXls(Stream xlsxStream, out int result)
        {
            var package = new ExcelPackage(xlsxStream);

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            int headerRow = 3;
            var target = workSheet.Cells[headerRow, 1]; // starting from list header
            var start = target.Start;
            var end = workSheet.Dimension.End;
            var endCol = 0;
            var fdProps = typeof(FdMaster).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var fdEveProps = typeof(FdEvent).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var errFound = false;

            var fdMasterCol = false;
            var orgRefCol = false;
            string fday = "";
            string fromTime = "";
            string toTime = "";
            string orgRef = "";
            string fdYear = "";
            int fdMasterColIdx = 0;
            int orgRefColIdx = 0;
            var orgMaster = new OrgMaster();
            List<PropertyInfo[]> propClasses = new List<PropertyInfo[]>();
            List<string> fdMasterheaders = new List<string>();
            List<string> fdEventheaders = new List<string>();
            List<FdMaster> updateFdList = new List<FdMaster>();
            List<FdMaster> createFdList = new List<FdMaster>();
            List<FdEvent> createFdEventList = new List<FdEvent>();
            List<FdEvent> updateFdEventList = new List<FdEvent>();

            IList<FdEventApproveSummaryDto> approved = GetApproveEveSummaryPage();

            result = 0;

            foreach (PropertyInfo prop in fdProps)
            {
                fdMasterheaders.Add(prop.Name);
            }
            foreach (PropertyInfo prop in fdEveProps)
            {
                fdEventheaders.Add(prop.Name);
            }

            for (int i = 1; i <= end.Column; i++) //use the column header to locate end of cloumns
            {
                if (!workSheet.Cells[start.Row, i].Text.Equals(""))
                {
                    endCol = i;
                }
                else break;

                if (workSheet.Cells[headerRow, i].Text.ToLower().Equals("fdmasterid"))
                {
                    fdMasterCol = true;
                    fdMasterColIdx = i;
                }

                if (workSheet.Cells[headerRow, i].Text.ToLower().Equals("orgmaster"))
                {
                    orgRefCol = true;
                    orgRefColIdx = i;
                }
            }

            MemoryStream logStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(logStream, Encoding.UTF8);
            List<string> fdMasterIdLst = new List<string>();
            List<string> orgMastLst = new List<string>();
            for (int y = start.Column; y <= endCol; y++)
            {
                if (workSheet.Cells[headerRow, y].Text.ToLower().Equals("fdmasterid"))
                {
                    for (int x = start.Row + 1; x <= end.Row; x++) //locate duplicated master id
                    {  //Row by row...
                        if (fdMasterIdLst.Contains(Convert.ToString(workSheet.Cells[x, fdMasterColIdx].Value)) && workSheet.Cells[x, fdMasterColIdx].Value != null)
                        {
                            writer.WriteLine("Row " + x + ": FdMasterId " + Convert.ToString(workSheet.Cells[x, fdMasterColIdx].Value) + " is duplicated.");
                            errFound = true;
                        }
                        fdMasterIdLst.Add(Convert.ToString(workSheet.Cells[x, fdMasterColIdx].Value));
                    }
                }
            }

            if (!fdMasterCol)
            {
                writer.WriteLine("FdMasterId column cannot be found");
                errFound = true;
            }
            if (!orgRefCol)
            {
                writer.WriteLine("OrgMaster column cannot be found");
                errFound = true;
            }

            int rowIndex = 0;
            string fieldname = "";
            try
            {
                if (!errFound)
                {
                    for (rowIndex = start.Row + 1; rowIndex <= end.Row; rowIndex++)
                    { // Row by row...
                        if (!workSheet.Cells[rowIndex, fdMasterColIdx + 2].Text.Equals("") || !workSheet.Cells[rowIndex, fdMasterColIdx + 3].Text.Equals(""))
                        {
                            string strId = workSheet.Cells[rowIndex, fdMasterColIdx].Value == null ? "" : workSheet.Cells[rowIndex, fdMasterColIdx].Value.ToString();
                            var fdMasterId = Convert.ToInt32(workSheet.Cells[rowIndex, fdMasterColIdx].Value);
                            var fdMaster = string.IsNullOrEmpty(strId) ? new FdMaster() : _fdMasterRepository.GetById(fdMasterId) == null ? new FdMaster() : _fdMasterRepository.GetById(fdMasterId);
                            var fdEvent = _fdEventRepository.GetRecByFdMasterId(fdMasterId) == null ? new FdEvent() : _fdEventRepository.GetRecByFdMasterId(fdMasterId);
                            Boolean warn = false;

                            List<string> list = new List<string>();
                            for (int columIndex = start.Column; columIndex <= endCol; columIndex++)
                            {
                                if (fdMasterheaders.Contains(workSheet.Cells[headerRow, columIndex].Text)) //if it is fd property
                                {
                                    PropertyInfo pc = fdProps.First(x => x.Name == workSheet.Cells[headerRow, columIndex].Text);
                                    var value = workSheet.Cells[rowIndex, columIndex].Text.Trim();
                                    var valueType = Nullable.GetUnderlyingType(pc.PropertyType) ?? pc.PropertyType;
                                    
                                    fieldname = pc.Name;

                                    if (pc.Name == "OrgMaster")
                                    {
                                        orgMaster = _orgMasterRepository.GetOrgByRef(value);
                                        if (orgMaster != null)
                                        {
                                            fdMaster.OrgMaster = orgMaster;
                                        }
                                        orgRef = value;
                                    }
                                    else if (pc.Name == "EngOrgName" || pc.Name == "FdRef" || pc.Name == "FirstReminderDeadline" || pc.Name == "SecondReminderDeadline" || pc.Name == "NetProceed")
                                    {
                                        //Skip the column
                                    }
                                    else if (pc.Name == "FdYear")
                                    {
                                        fdYear = value;
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "ApplyForTwr" && !string.IsNullOrEmpty(value))
                                    {
                                        value = twrs.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "ContactPersonSalute" && !string.IsNullOrEmpty(value))
                                    {
                                        value = engSalutes.FirstOrDefault(p => p.Value == value).Key;
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "UsedLanguage" && !string.IsNullOrEmpty(value))
                                    {
                                        value = languages.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "FdGroup" && !string.IsNullOrEmpty(value))
                                    {
                                        value = fdGroupings.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "ApplicationResult" && !string.IsNullOrEmpty(value))
                                    {
                                        value = fdApplicationResults.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "FdLotResult" && !string.IsNullOrEmpty(value))
                                    {
                                        value = fdLotResults.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (pc.Name == "DocSubmission" && !string.IsNullOrEmpty(value))
                                    {
                                        value = docSubmissions.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                    else if (!string.IsNullOrEmpty(value) && (value.ToLower() == "yes" || value.ToLower() == "no"))
                                    {
                                        bool boolVal = value.ToLower() == "yes" ? true : false;
                                        pc.SetValue(fdMaster, Convert.ChangeType(boolVal, valueType));
                                    }
                                    else if (!string.IsNullOrEmpty(value) && valueType.FullName.ToLower() == "system.datetime")
                                    {
                                        //var dt2 = CommonHelper.ConvertStringToDateTime(value);
                                        //pc.SetValue(fdMaster, Convert.ChangeType(dt2, valueType));
                                        DateTime? dt = null;
                                        dt = DateTime.ParseExact(value, new string[] { "d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                                        pc.SetValue(fdMaster, Convert.ChangeType(dt, valueType));
                                    }
                                    else if (!string.IsNullOrEmpty(value))
                                    {
                                        pc.SetValue(fdMaster, Convert.ChangeType(value, valueType));
                                    }
                                }
                                else if (fdEventheaders.Contains(workSheet.Cells[headerRow, columIndex].Text)) //if it is fdEvent property
                                {
                                    PropertyInfo pc = fdEveProps.First(x => x.Name == workSheet.Cells[headerRow, columIndex].Text);
                                    var value = workSheet.Cells[rowIndex, columIndex].Text.Trim();
                                    var valueType = Nullable.GetUnderlyingType(pc.PropertyType) ?? pc.PropertyType;

                                    fieldname = pc.Name;

                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        if (approved.Any(p => p.YearOfFlagDay == fdMaster.FdYear && p.Approved))
                                        {
                                            result = 1;

                                            if (!warn)
                                            {
                                                writer.WriteLine("Row " + rowIndex + ": Flag Day events already approved, please create/edit the flag day event details in maintenance page instead.");
                                                warn = true;
                                            }

                                            continue;
                                        }

                                        if (pc.Name == "PermitNum")
                                        {
                                            //Skip the column
                                        }
                                        else if (pc.Name == "TWR" && !string.IsNullOrEmpty(value))
                                        {
                                            value = twrs.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                            pc.SetValue(fdEvent, Convert.ChangeType(value, valueType));
                                        }
                                        else if (pc.Name == "TwrDistrict" && !string.IsNullOrEmpty(value))
                                        {
                                            //value = twrDistricts.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                            pc.SetValue(fdEvent, Convert.ChangeType(value, valueType));
                                        }
                                        else if (pc.Name == "Remarks" && !string.IsNullOrEmpty(value))
                                        {
                                            Regex reg = new Regex(",");
                                            Match match = reg.Match(value);
                                            if (match.Success)
                                            {
                                                var arrValue = value.Split(',');
                                                for (var x = 0; x < arrValue.Length; x++)
                                                {
                                                    if (!fdEventRemarks.ContainsKey(arrValue[x]))
                                                    {
                                                        errFound = true;
                                                        writer.WriteLine("Row " + rowIndex + ": Cannot find lookup code " + arrValue[x] + " from " + LookupType.FdEventRemark.ToEnumValue() + " lookup type");
                                                    }
                                                }
                                                if (!errFound)
                                                {
                                                    pc.SetValue(fdEvent, Convert.ChangeType(string.Join(",", arrValue), valueType));
                                                }
                                            }
                                            else
                                            {
                                                if (!fdEventRemarks.ContainsKey(value))
                                                {
                                                    errFound = true;
                                                    writer.WriteLine("Row " + rowIndex + ": Cannot find lookup code '" + value + "' from " + LookupType.FdEventRemark.ToEnumValue() + " lookup type");
                                                }
                                                else
                                                {
                                                    pc.SetValue(fdEvent, Convert.ChangeType(value, valueType));
                                                }
                                            }
                                        }
                                        else if (pc.Name == "CollectionMethod" && !string.IsNullOrEmpty(value))
                                        {
                                            Regex reg = new Regex(",");
                                            Match match = reg.Match(value);
                                            if (match.Success)
                                            {
                                                var arrValue = value.Split(',');
                                                for (var x = 0; x < arrValue.Length; x++)
                                                {
                                                    if (!collectionMethods.Values.Contains(arrValue[x]))
                                                    {
                                                        errFound = true;
                                                        writer.WriteLine("Row " + rowIndex + ": Cannot find lookup code " + arrValue[x] + " from " + LookupType.FdCollectionMethod.ToEnumValue() + " lookup type");
                                                    }
                                                    else
                                                        arrValue[x] = collectionMethods.Where(u => u.Value == arrValue[x]).FirstOrDefault().Key;
                                                }
                                                if (!errFound)
                                                {
                                                    pc.SetValue(fdEvent, Convert.ChangeType(string.Join(",", arrValue), valueType));
                                                }
                                            }
                                            else
                                            {
                                                if (!collectionMethods.Values.Contains(value))
                                                {
                                                    errFound = true;
                                                    writer.WriteLine("Row " + rowIndex + ": Cannot find lookup code '" + value + "' from " + LookupType.FdCollectionMethod.ToEnumValue() + " lookup type");
                                                }
                                                else
                                                {
                                                    pc.SetValue(fdEvent, Convert.ChangeType(collectionMethods.Where(u => u.Value == value).FirstOrDefault().Key, valueType));
                                                }
                                            }
                                        }
                                        else if (pc.Name == "FlagDay" && !string.IsNullOrEmpty(value))
                                        {
                                            DateTime? dt = null;
                                            dt = DateTime.ParseExact(value, new string[] { "d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                                            pc.SetValue(fdEvent, Convert.ChangeType(dt, valueType));
                                            fday = value;
                                            fdMaster.SubmissionDueDate = dt.Value.AddDays(90);
                                        }
                                        else if (pc.Name != "FlagTimeFrom" && pc.Name != "FlagTimeTo" && !string.IsNullOrEmpty(value) && valueType.FullName.ToLower() == "system.datetime")
                                        {
                                            DateTime? dt = null;
                                            dt = DateTime.ParseExact(value, new string[] { "d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                                            pc.SetValue(fdEvent, Convert.ChangeType(dt, valueType));
                                        }
                                        else if (pc.Name == "FlagTimeFrom" || pc.Name == "FlagTimeTo")
                                        { }
                                        else if (!string.IsNullOrEmpty(value) && (value.ToLower() == "yes" || value.ToLower() == "no"))
                                        {
                                            bool boolVal = value.ToLower() == "yes" ? true : false;
                                            pc.SetValue(fdEvent, Convert.ChangeType(boolVal, valueType));
                                        }
                                        else if (!string.IsNullOrEmpty(value))
                                        {
                                            pc.SetValue(fdEvent, Convert.ChangeType(value, valueType));
                                        }
                                    }
                                    // if value input at xls is not empty
                                    if (pc.Name == "FlagTimeFrom" || pc.Name == "FlagTimeTo") // ignore xls input
                                    {
                                        //var dt = CommonHelper.ConvertStringToDateTime(value);

                                        PropertyInfo pcFlagDay = fdEveProps.First(x => x.Name == "FlagDay");
                                        DateTime flagday = Convert.ToDateTime(pcFlagDay.GetValue(fdEvent, null));
                                        fromTime = _systemParameterRepository.getValByCode("FlagDayEventStartTime");
                                        toTime = _systemParameterRepository.getValByCode("FlagDayEventEndTime");
                                        if (flagday != DateTime.MinValue && pc.Name == "FlagTimeFrom")
                                        {
                                            pc.SetValue(fdEvent, flagday.AddHours(Convert.ToInt32(fromTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(fromTime.Substring(2, 2))));
                                        }
                                        if (flagday != DateTime.MinValue && pc.Name == "FlagTimeTo")
                                        {
                                            pc.SetValue(fdEvent, flagday.AddHours(Convert.ToInt32(toTime.Substring(0, 2))).AddMinutes(Convert.ToInt32(toTime.Substring(2, 2))));
                                        }
                                    }
                                }
                            }

                            var a = updateFdList.Where(x => x.OrgMaster != null);
                            var b = createFdList.Where(x => x.OrgMaster != null);

                            var id = from u in a.Where(x => x.OrgMaster.OrgRef == orgRef && x.FdYear == fdYear && x.FdMasterId != fdMasterId)
                                     select u.FdMasterId;

                            if (a.Any(x => x.OrgMaster.OrgRef == orgRef && x.FdYear == fdYear && x.FdMasterId != fdMasterId) || b.Any(x => x.OrgMaster.OrgRef == orgRef && x.FdYear == fdYear))
                            {
                                writer.WriteLine("Row " + rowIndex + ": Org. Reference " + orgRef + ", Flag Day Year " + fdYear + " is duplicated in this worksheet. ");
                                errFound = true;
                            }

                            //update the list for insertion
                            if (fdMaster.FdMasterId != default(int))
                            {
                                updateFdList.Add(fdMaster);
                            }
                            else
                            {
                                createFdList.Add(fdMaster);
                            }

                            if (!approved.Any(p => p.YearOfFlagDay == fdMaster.FdYear && p.Approved))
                            {
                                if (fdEvent.FdEventId != default(int))
                                {
                                    fdEvent.FdMaster = fdMaster;
                                    updateFdEventList.Add(fdEvent);
                                }
                                else
                                {
                                    if (eveHasValue(fdEvent, writer, rowIndex, ref errFound)) // prevent create empty event record
                                    {
                                        fdEvent.FdMaster = fdMaster;
                                        createFdEventList.Add(fdEvent);
                                    }
                                }
                            }

                            //validation
                            if (fdEvent.TWR == "2" && fdEvent.TwrDistrict.IsNullOrEmpty())
                            {
                                writer.WriteLine("Row " + rowIndex + ": Region must be selected when category is regional: TWR: " + twrs[fdEvent.TWR] + "; and FlagDay: " + fdEvent.FlagDay.ToString());
                                errFound = true;
                            }

                            if (!string.IsNullOrEmpty(fdMaster.FdYear) && !string.IsNullOrEmpty(fdEvent.TWR) && fdEvent.FlagDay != null && fdEvent.FlagDay != DateTime.MinValue) //flagday list
                            {
                                if (!_flagDayListService.MatchFlagDayInFdList((DateTime)fdEvent.FlagDay, fdEvent.TWR, fdMaster.FdYear))
                                {
                                    writer.WriteLine("Row " + rowIndex + ": Flag Day does not in Flag Day List. FdYear: " + fdMaster.FdYear + "; TWR: " + twrs[fdEvent.TWR] + "; and FlagDay: " + fdEvent.FlagDay.ToString());
                                    errFound = true;
                                }
                            }
                            if (!string.IsNullOrEmpty(fdMaster.FdYear) && !string.IsNullOrEmpty(fdEvent.TWR) && fdEvent.FlagDay != null && fdEvent.FlagDay != DateTime.MinValue) //flagday list
                            {
                                if (!_fdEventService.AvaliableFlagDay((DateTime)fdEvent.FlagDay, fdEvent.TWR, fdEvent.TwrDistrict, fdMaster.FdYear, fdEvent.FdEventId))
                                {
                                    if (string.IsNullOrEmpty(fdEvent.TwrDistrict))
                                    {
                                        writer.WriteLine("Row " + rowIndex + ": Flag Day was already been assigned. " + " FlagDay: " + fdEvent.FlagDay.ToString() + "; TWR: " + twrs[fdEvent.TWR]);
                                    }
                                    else
                                    {
                                        writer.WriteLine("Row " + rowIndex + ": Flag Day was already been assigned. " + " FlagDay: " + fdEvent.FlagDay.ToString() + " TWR: " + twrs[fdEvent.TWR] + " TwrDistrict: " + fdEvent.TwrDistrict);
                                    }
                                    errFound = true;
                                }
                            }

                            if (string.IsNullOrEmpty(orgRef))
                            {
                                writer.WriteLine("Row " + rowIndex + ": Org. Reference cannot be empty. ");
                                errFound = true;
                            }
                            if (string.IsNullOrEmpty(fdYear))
                            {
                                writer.WriteLine("Row " + rowIndex + ": Flag Day Year cannot be empty. ");
                                errFound = true;
                            }
                            if (!_fdMasterRepository.ifOrgMasterHasFdevent(orgRef, fdYear) && fdMaster.FdMasterId == default(int))
                            {
                                writer.WriteLine("Row " + rowIndex + ": Only one application is allowed for each Organisation. ");
                                errFound = true;
                            }
                            if (orgMaster == null)
                            {
                                writer.WriteLine("Row " + rowIndex + ": Cannot find organisation by Org. Reference or organisation is not unique. ");
                                errFound = true;
                            }
                        }
                        else break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Row " + rowIndex + ": Field [" + fieldname + "] " + ex.Message);
            }


            workSheet = null;
            package.Dispose();

            if (!errFound)
            {
                if (createFdList.Count > 0)
                {
                    foreach (var fd in createFdList)
                    {
                        fd.FdRef = _fdMasterRepository.GetMaxSeq(fdYear);
                        _fdMasterRepository.Add(fd);
                        _eventPublisher.EntityInserted<FdMaster>(fd);
                    }
                }
                if (updateFdList.Count > 0)
                {
                    //_fdMasterRepository.Update(updateList);
                    foreach (var fd in updateFdList)
                    {
                        _fdMasterRepository.Update(fd);
                        _eventPublisher.EntityUpdated<FdMaster>(fd);
                    }
                }
                if (createFdEventList.Count > 0)
                {
                    //_fdMasterRepository.Update(updateList);
                    foreach (var eve in createFdEventList)
                    {
                        var permitNum = eve.FlagDay != null ? this.genPermitNo(eve.FdMaster.FdYear, eve.TWR) : "";
                        eve.PermitNum = permitNum;
                        _fdEventRepository.Add(eve);
                        _eventPublisher.EntityUpdated<FdEvent>(eve);
                    }
                }
                if (updateFdEventList.Count > 0)
                {
                    foreach (var eve in updateFdEventList)
                    {
                        //var permitNum = eve.FlagDay != null ? this.genPermitNo(eve.FdMaster.FdYear, eve.TWR) : "";
                        //eve.PermitNum = permitNum;
                        _fdEventRepository.Update(eve);
                        _eventPublisher.EntityUpdated<FdEvent>(eve);
                    }
                }

                if (result != 0)
                {
                    writer.Flush();
                    return logStream;
                } else
                    return null;
            }
            else
            {
                writer.Flush();
                result = 2;
                return logStream;
            }
        }

        public IList<FdApplicationListDto> GetFdApplicationList(string fdYear)
        {
            return (from u in _fdApproveApplicationListGridViewRepository.Table
                    where u.FdYear == fdYear
                    select new FdApplicationListDto
                    {
                        FdRef = u.FdRef,
                        OrgName = u.OrgName,
                        FlagDay = u.FlagDay,
                        TWR = u.TWR,
                        TwrDistrict = u.TwrDistrict,
                        PermitNo = u.PermitNum,
                        ApproveRemarks = u.ApproveRemarks,
                        Approve = u.Approve,
                        FdEventId = u.FdEventId,
                        RowVersion = u.RowVersion
                    }).ToList();
        }

        public IPagedList<FdApplicationListDto> GetFdApplicationListPage(GridSettings grid, string fdYear)
        {
            return _fdApproveApplicationListGridViewRepository.GetFdApplicationListPage(grid, fdYear);
        }

        public List<FdEventApproveSummaryDto> GetApproveEveSummaryPage()
        {
            return _fdMasterRepository.GetApproveEveSummaryPage();
        }

        public Hashtable CalFdEditRecCnt(int fdMasterId)
        {
            var map = new Hashtable();
            var fdEventCnt = _fdEventRepository.Table.Count(a => a.FdMaster.FdMasterId == fdMasterId);
            var fdEnquiryCnt = _orgMasterRepository.getEnqRecCntByFdMasterId(fdMasterId);
            var fdComplaintCnt = _orgMasterRepository.getCompRecCntByFdMasterId(fdMasterId);
            var fdLetterCnt = _fdDocRepository.Table.Count(x => x.DocStatus == true);
            var fdAttachmentCnt = _fdAttachmentRepository.Table.Count(a => a.FdMaster.FdMasterId == fdMasterId);

            map.Add("fdEventCnt", fdEventCnt);
            map.Add("fdEnquiryCnt", fdEnquiryCnt);
            map.Add("fdComplaintCnt", fdComplaintCnt);
            map.Add("fdLetterCnt", fdLetterCnt);
            map.Add("fdAttachmentCnt", fdAttachmentCnt);
            return map;
        }

        public IPagedList<FdAcSummaryView> GetPageByFdAcSummaryView(GridSettings grid, string permitNum, string flagDay)
        {
            return _fdAcSummaryViewRepository.GetPage(grid, permitNum, flagDay);
        }

        public bool NewApplicantExists(string orgRef, int fdMasterId)
        {
            return _fdMasterRepository.Table.Any(x => x.OrgMaster.OrgRef == orgRef && x.NewApplicantIndicator == true && x.FdMasterId != fdMasterId);
        }

        private void SetXlsStyle(ExcelRange range, System.Drawing.Color? fontColor, System.Drawing.Color? bgColor)
        {
            if (fontColor.HasValue)
                range.Style.Font.Color.SetColor(fontColor.Value);

            if (bgColor.HasValue)
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(bgColor.Value);
            }
        }

        //private FdMaster CreateOrUpdateFdMaster(List<string> rec, FdMaster fdMaster, OrgMaster orgMaster)
        //{
        //    fdMaster.OrgMaster = orgMaster;
        //    fdMaster.FdRef = rec[2];
        //    fdMaster.ApplicationReceiveDate = string.IsNullOrEmpty(rec[3]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[3])));
        //    fdMaster.UsedLanguage = string.IsNullOrEmpty(rec[4]) ? "" : languages.Where(p => p.Value == rec[4]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.ContactPersonSalute = string.IsNullOrEmpty(rec[5]) ? "" : engSalutes.Where(p => p.Value == rec[5]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.ContactPersonFirstName = rec[6];
        //    fdMaster.ContactPersonLastName = rec[7];
        //    fdMaster.ContactPersonChiFirstName = rec[8];
        //    fdMaster.ContactPersonChiLastName = rec[9];
        //    fdMaster.ContactPersonPosition = rec[10];
        //    fdMaster.ContactPersonTelNum = rec[11];
        //    fdMaster.ContactPersonFaxNum = rec[12];
        //    fdMaster.ContactPersonEmailAddress = rec[13];
        //    fdMaster.FundRaisingPurpose = rec[14];
        //    fdMaster.ApplyForTwr = string.IsNullOrEmpty(rec[15]) ? "" : twrDistricts.Where(p => p.Value == rec[15]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.ApplicationResult = string.IsNullOrEmpty(rec[16]) ? "" : fdApplicationResults.Where(p => p.Value == rec[16]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.VettingPanelCaseIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[17]) ? 0 : Convert.ToInt32(rec[17]));
        //    fdMaster.FdYear = rec[18];
        //    fdMaster.LotGroup = rec[19];
        //    fdMaster.FdCategory = rec[20];
        //    fdMaster.FdDistrict = rec[21];
        //    fdMaster.TargetIncome = Convert.ToInt32(string.IsNullOrEmpty(rec[22]) ? "0" : rec[22]);
        //    fdMaster.NewApplicantIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[23]) ? 0 : Convert.ToInt32(rec[23]));
        //    //fdMaster.TrackRecord = Convert.ToBoolean(string.IsNullOrEmpty(rec[24]) ? 0 : Convert.ToInt32(rec[24]));
        //    //fdMaster.PastAfsRecord = rec[25];
        //    fdMaster.FdGroup = string.IsNullOrEmpty(rec[26]) ? "" : fdGroupings.Where(p => p.Value == rec[26]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.FdGroupPercentage = Convert.ToDecimal(string.IsNullOrEmpty(rec[27]) ? "0" : rec[27]);
        //    fdMaster.GroupingResult = rec[28];
        //    fdMaster.CommunityChest = Convert.ToBoolean(string.IsNullOrEmpty(rec[29]) ? 0 : Convert.ToInt32(rec[29]));
        //    fdMaster.ConsentLetter = Convert.ToBoolean(string.IsNullOrEmpty(rec[29]) ? 0 : Convert.ToInt32(rec[30]));
        //    //fdMaster.OutstandingEmailIssueDate = string.IsNullOrEmpty(rec[31]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[31])));
        //    //fdMaster.OutstandingEmailReplyDate = string.IsNullOrEmpty(rec[32]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[32])));
        //    fdMaster.ApplicationRemark = rec[33];
        //    //fdMaster.OutstandingEmailReminderIssueDate = string.IsNullOrEmpty(rec[34]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[34])));
        //    //fdMaster.OutstandingEmailReminderReplyDate = string.IsNullOrEmpty(rec[35]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[35])));
        //    fdMaster.FdLotNum = rec[36];
        //    fdMaster.FdLotResult = string.IsNullOrEmpty(rec[37]) ? "" : fdLotResults.Where(p => p.Value == rec[37]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.PriorityNum = rec[38];
        //    //fdMaster.ProposalDetail = rec[39];
        //    //fdMaster.ChiProposalDetail = rec[40];
        //    //fdMaster.FlagSalePurpose = rec[41];
        //    //fdMaster.ChiFlagSalePurpose = rec[42];
        //    fdMaster.AcknowledgementReceiveDate = string.IsNullOrEmpty(rec[43]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[43])));
        //    fdMaster.ApplyPledgingMechanismIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[44]) ? 0 : Convert.ToInt32(rec[44]));
        //    fdMaster.PledgedAmt = Convert.ToDecimal(string.IsNullOrEmpty(rec[45]) ? "0" : rec[45]);
        //    fdMaster.PledgingProposal = rec[46];
        //    fdMaster.PledgingApplicationRemark = rec[47];
        //    fdMaster.DocSubmission = string.IsNullOrEmpty(rec[48]) ? "" : docSubmissions.Where(p => p.Value == rec[48]).Select(p => p.Key).FirstOrDefault();
        //    fdMaster.SubmissionDueDate = string.IsNullOrEmpty(rec[49]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[49])));
        //    fdMaster.FirstReminderIssueDate = string.IsNullOrEmpty(rec[50]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[50])));
        //    fdMaster.FirstReminderDeadline = string.IsNullOrEmpty(rec[51]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[51])));
        //    fdMaster.SecondReminderIssueDate = string.IsNullOrEmpty(rec[52]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[52])));
        //    fdMaster.SecondReminderDeadline = string.IsNullOrEmpty(rec[53]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[53])));
        //    //fdMaster.AuditReportReceivedDate = string.IsNullOrEmpty(rec[54]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[54])));
        //    //fdMaster.PublicationReceivedDate = string.IsNullOrEmpty(rec[55]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[55])));
        //    fdMaster.AuditReportReceivedDate = rec[54];
        //    fdMaster.PublicationReceivedDate = rec[55];
        //    //fdMaster.OfficialReceiptReceivedDate = string.IsNullOrEmpty(rec[56]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[56])));
        //    fdMaster.DocReceiveRemark = rec[56];
        //    fdMaster.DocRemark = rec[57];
        //    fdMaster.StreetCollection = Convert.ToDecimal(string.IsNullOrEmpty(rec[58]) ? "0" : rec[58]);
        //    fdMaster.GrossProceed = Convert.ToDecimal(string.IsNullOrEmpty(rec[59]) ? "0" : rec[59]);
        //    fdMaster.Expenditure = Convert.ToDecimal(string.IsNullOrEmpty(rec[60]) ? "0" : rec[60]);
        //    fdMaster.NetProceed = Convert.ToDecimal(string.IsNullOrEmpty(rec[61]) ? "0" : rec[61]);
        //    fdMaster.NewspaperPublishDate = string.IsNullOrEmpty(rec[62]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[62])));
        //    fdMaster.PledgingAmt = Convert.ToDecimal(string.IsNullOrEmpty(rec[63]) ? "0" : rec[63]);
        //    fdMaster.AcknowledgementEmailIssueDate = string.IsNullOrEmpty(rec[64]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[64])));
        //    fdMaster.AfsReceiveIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[65]) ? 0 : Convert.ToInt32(rec[65]));
        //    fdMaster.RequestPermitteeIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[66]) ? 0 : Convert.ToInt32(rec[66]));
        //    fdMaster.AfsReSubmitIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[67]) ? 0 : Convert.ToInt32(rec[67]));
        //    fdMaster.AfsReminderIssueIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[68]) ? 0 : Convert.ToInt32(rec[68]));
        //    fdMaster.Remark = rec[69];
        //    return fdMaster;
        //}

        private FdEvent CreateOrUpdateFdEvent(List<string> rec, FdEvent fdEvent, FdMaster fdMaster)
        {
            fdEvent.FdMaster = fdMaster;
            if (!string.IsNullOrEmpty(rec[71])) { fdEvent.FlagDay = Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[71]))); }
            fdEvent.FlagTimeFrom = string.IsNullOrEmpty(rec[72]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[72])));
            fdEvent.FlagTimeTo = string.IsNullOrEmpty(rec[73]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[73])));
            fdEvent.TWR = string.IsNullOrEmpty(rec[74]) ? "" : twrDistricts.Where(p => p.Value == rec[74]).Select(p => p.Key).FirstOrDefault();
            fdEvent.TwrDistrict = string.IsNullOrEmpty(rec[75]) ? "" : twrDistricts.Where(p => p.Value == rec[75]).Select(p => p.Key).FirstOrDefault();
            fdEvent.CollectionMethod = rec[76];
            fdEvent.PermitNum = rec[77];
            fdEvent.PermitIssueDate = string.IsNullOrEmpty(rec[78]) ? (DateTime?)null : Convert.ToDateTime(DateTime.FromOADate(Convert.ToInt32(rec[78])));
            fdEvent.PermitRevokeIndicator = Convert.ToBoolean(string.IsNullOrEmpty(rec[79]) ? 0 : Convert.ToInt32(rec[79])); ;
            fdEvent.Remarks = rec[80];

            return fdEvent;
        }

        private bool eveHasValue(FdEvent eve, StreamWriter writer, int rowIndex, ref bool errFound)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(eve.BagColour))
            { result = true; }
            if (!string.IsNullOrEmpty(eve.FlagColour))
            { result = true; }
            if (!string.IsNullOrEmpty(eve.CollectionMethod))
            { result = true; }
            if (eve.FlagDay != DateTime.MinValue && eve.FlagDay != null)
            { result = true; }
            if (eve.FlagTimeFrom != null)
            { result = true; }
            if (eve.FlagTimeTo != null)
            { result = true; }
            if (eve.PermitAcknowledgementReceiveDate != null)
            { result = true; }
            if (eve.PermitIssueDate != null)
            { result = true; }
            if (!string.IsNullOrEmpty(eve.PermitNum))
            { result = true; }
            if (eve.PermitRevokeIndicator != null)
            { result = true; }
            if (!string.IsNullOrEmpty(eve.Remarks))
            { result = true; }
            if (eve.TWR != null)
            { result = true; }
            if (eve.TwrDistrict != null)
            { result = true; }

            if (result && eve.FlagDay == DateTime.MinValue)
            {
                errFound = true;
                writer.WriteLine("Row " + rowIndex + ": Create FdEvent information found but Flagday is missing ");
            }
            if (result && eve.TWR == null)
            {
                errFound = true;
                writer.WriteLine("Row " + rowIndex + ": Create FdEvent information found but TWR is missing ");
            }
            if (result && eve.TWR == "2" && string.IsNullOrEmpty(eve.TwrDistrict))
            {
                errFound = true;
                writer.WriteLine("Row " + rowIndex + ": TwrDistrict is missing while TWR is regional.");
            }

            return result;
        }

        private bool chkLstDupOrgRefByFdYr(List<FdMaster> list, string orgRef, string fdYear)
        {
            return list.Any(x => x.OrgMaster.OrgRef == orgRef && x.FdYear == fdYear);
        }

        #endregion Methods
    }
}