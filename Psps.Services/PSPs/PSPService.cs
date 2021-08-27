using OfficeOpenXml;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.Psp;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.UserLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial class PSPService : IPspService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IPSPMasterRepository _pspMasterRepository;

        private readonly IPspCountEventsRepository _pspCountEventsRepository;

        private readonly IPspEventRepository _pspEventRepository;

        private readonly IOrgMasterRepository _orgMasterRepository;

        private readonly IPspDocSummaryViewRepository _pspDocSummaryViewRepository;

        private readonly IPspAttachmentRepository _pspAttachmentRepository;

        private readonly IPspApprovalHistoryRepository _pspApprovalHistoryRepository;

        private readonly ICacheManager _cacheManager;

        private readonly IPspAcSummaryViewRepository _pspAcSummaryViewRepository;

        private readonly IPspSearchViewRepository _pspSearchViewRepository;

        private readonly ILookupService _lookupService;

        private readonly IUserLogService _userLogService;

        private readonly IDictionary<string, string> languages;
        private readonly IDictionary<string, string> engSalutes;
        private readonly IDictionary<string, string> collectionMethods;
        private readonly IDictionary<string, string> rejectReason;
        private readonly IDictionary<string, string> pspNotRequireReason;
        private readonly IDictionary<string, string> caseCloseReason;
        private readonly IDictionary<string, string> specialRemark;
        private readonly IDictionary<string, string> docSubmission;
        private readonly IDictionary<string, string> applicationResult;
        private readonly IDictionary<string, string> fundUsed;

        #endregion Fields

        #region Ctor

        public PSPService(IEventPublisher eventPublisher, IPSPMasterRepository pspMasterRepository, IPspCountEventsRepository pspCountEventsRepository, IOrgMasterRepository orgMasterRepository,
                        IPspDocSummaryViewRepository pspDocSummaryViewRepository, IPspAttachmentRepository pspAttachmentRepository, IPspEventRepository pspEventRepository,
                        IPspApprovalHistoryRepository pspApprovalHistoryRepository, ICacheManager cacheManager, IPspAcSummaryViewRepository pspAcSummaryViewRepository,
                        IPspSearchViewRepository pspSearchViewRepository, ILookupService lookupService, IUserLogService userLogService)
        {
            this._eventPublisher = eventPublisher;
            this._pspMasterRepository = pspMasterRepository;
            this._pspCountEventsRepository = pspCountEventsRepository;
            this._orgMasterRepository = orgMasterRepository;
            this._pspDocSummaryViewRepository = pspDocSummaryViewRepository;
            this._pspAttachmentRepository = pspAttachmentRepository;
            this._pspEventRepository = pspEventRepository;
            this._pspApprovalHistoryRepository = pspApprovalHistoryRepository;
            this._cacheManager = cacheManager;
            this._pspAcSummaryViewRepository = pspAcSummaryViewRepository;
            this._pspSearchViewRepository = pspSearchViewRepository;
            this._lookupService = lookupService;
            this.languages = _lookupService.getAllLkpInCodec(LookupType.LanguageUsed);
            if (languages.Count == 0) languages.Add("", "");
            this.engSalutes = _lookupService.getAllLkpInCodec(LookupType.Salute);
            if (engSalutes.Count == 0) engSalutes.Add("", "");
            this.collectionMethods = _lookupService.getAllLkpInCodec(LookupType.PspCollectionMethod);
            if (collectionMethods.Count == 0) collectionMethods.Add("", "");
            this.rejectReason = _lookupService.getAllLkpInCodec(LookupType.PSPRejectReason);
            if (rejectReason.Count == 0) rejectReason.Add("", "");
            this.pspNotRequireReason = _lookupService.getAllLkpInCodec(LookupType.PSPNotRequireReason);
            if (pspNotRequireReason.Count == 0) pspNotRequireReason.Add("", "");
            this.caseCloseReason = _lookupService.getAllLkpInCodec(LookupType.CaseCloseReason);
            if (caseCloseReason.Count == 0) caseCloseReason.Add("", "");
            this.specialRemark = _lookupService.getAllLkpInCodec(LookupType.PspSpecialRemark);
            if (specialRemark.Count == 0) specialRemark.Add("", "");
            this.docSubmission = _lookupService.getAllLkpInCodec(LookupType.PspDocSubmitted);
            if (docSubmission.Count == 0) docSubmission.Add("", "");
            this.applicationResult = _lookupService.getAllLkpInCodec(LookupType.PSPApplicationResult);
            if (applicationResult.Count == 0) applicationResult.Add("", "");
            this.fundUsed = _lookupService.getAllLkpInCodec(LookupType.FundUsed);
            if (fundUsed.Count == 0) fundUsed.Add("", "");
            this._userLogService = userLogService;
        }

        #endregion Ctor

        #region Methods

        public PspMaster GetPSPById(int pspId)
        {
            return _pspMasterRepository.GetById(pspId);
        }

        public IPagedList<PspMaster> GetPage(GridSettings grid)
        {
            return _pspMasterRepository.GetPage(grid);
        }

        public void CreatePspMaster(PspMaster model)
        {
            _pspMasterRepository.Add(model);

            _userLogService.LogPSPInformation(null, model);

            _eventPublisher.EntityInserted<PspMaster>(model);
        }

        public void UpdatePspMaster(PspMaster model)
        {
            _pspMasterRepository.Update(model);

            _eventPublisher.EntityUpdated<PspMaster>(model);
        }

        public void UpdatePspMaster(PspMaster oldPSPMaster, PspMaster newPSPMaster)
        {
            Ensure.Argument.NotNull(oldPSPMaster, "Old Flag Day");
            Ensure.Argument.NotNull(newPSPMaster, "New Flag Day");

            _userLogService.LogPSPInformation(oldPSPMaster, newPSPMaster);

            _pspMasterRepository.Update(newPSPMaster);
            _eventPublisher.EntityUpdated<PspMaster>(newPSPMaster);
        }

        public IPagedList<PspMaster> GetPage(GridSettings grid, string permitNum)
        {
            return _pspMasterRepository.GetPage(grid, permitNum);
        }

        #endregion Methods
        //CR-005 01
        //Assign the PermitNum to PspMaster instead of PspApprovalHistory
        public string GetNextPermitNum(string pspYear)
        {
            var query = this._pspMasterRepository.Table.Where(x => x.PermitNum != null && x.PermitNum.Substring(0, 4) == pspYear).Select(x => x.PermitNum).AsEnumerable<String>();

            var nextPermit = (from p1 in query
                              join p2 in query on (int?)Convert.ToInt32(p1.Substring(5, 3)) + 1 equals (int?)Convert.ToInt32(p2.Substring(5, 3)) into p3
                              from p2 in p3.DefaultIfEmpty()
                              select new
                              {
                                  permitNum = Convert.ToInt32(p1.Substring(5, 3)),
                                  nextPermitNum = (int?)(p2 != null ? Convert.ToInt32(p2.Substring(5, 3)) : (int?)null)
                              }).Where(x => !x.nextPermitNum.HasValue).Select(x => x.permitNum).DefaultIfEmpty().Min();
            ;            
            
            if (nextPermit != null)
            {
                nextPermit++;
                return pspYear + "/" + Convert.ToString(nextPermit).PadLeft(3, '0') + "/" + "1";
            }
            else
                return pspYear + "/" + "001" + "/1";
        }

        //CR-005
        //Assign the PermitNum to PspMaster instead of PspApprovalHistory
        public string GetNextPermitNum(int pspMasterId)
        {
            int approved = this._pspApprovalHistoryRepository.Table.Count(x => x.PspMaster.PspMasterId == pspMasterId && "AP".Equals(x.ApprovalStatus));

            if (approved > 0)
            {
                var query = from p in this._pspMasterRepository.Table.Where(x => (x.PspMasterId == pspMasterId || x.PreviousPspMasterId == pspMasterId) && x.PermitNum != null)
                            select new
                            {
                                midNum = p.PermitNum.Substring(5, 3),
                                endNum = p.PermitNum.Substring(9),
                                year = p.PermitNum.Substring(0, 4)
                            };

                //var endNum = Convert.ToInt32(query.Max(x => x.endNum)) + 1;
                var endNumList = query.Select(x => Convert.ToInt32(x.endNum)).OrderBy(x => x).ToList<int>();
                //To fill up the Permit Number hold
                int lastNum = 0;
                foreach (int num in endNumList)
                {
                    if (lastNum + 1 != num)
                        break;
                    else
                        lastNum = num;
                }

                if (query.Count() > 0)
                {
                    var midNum = query.First().midNum;

                    string pspYear = query.First().year;

                    return pspYear + "/" + Convert.ToString(midNum) + "/" + Convert.ToString(++lastNum);
                }
                else
                    return null;
            }
            else
                return null;
            
        }

        public IPagedList<PspSearchDto> GetPagePspSearchDto(GridSettings grid)
        {
            return _pspSearchViewRepository.GetPagePspSearchDto(grid);
        }

        public IDictionary<string, string> GetAllPspYearForDropdown()
        {
            IDictionary<string, string> yearList = new Dictionary<string, string>();
            //var query = from u in this._pspMasterRepository.Table
            //            select new
            //            {
            //                appRecvDate = u.ApplicationReceiveDate
            //            };

            int minAppRecvYear = this._pspMasterRepository.Table.Min(u => u.ApplicationReceiveDate).Value.Year;

            //var minAppRecvYear = query.Min(x => x.appRecvDate).Value.Year;
            int currYear = DateTime.Now.Year;

            for (int i = minAppRecvYear; i <= currYear; i++)
            {
                yearList.Add(i.ToString(), i.ToString());
            }

            return yearList;
        }

        public string GetMaxSeq(string yearOfPsp)
        {
            return _pspMasterRepository.GetMaxSeq(yearOfPsp);
        }

        public IPagedList<PspApproveEventDto> GetRecommendPspEvents(GridSettings grid)
        {
            return _pspMasterRepository.GetRecommendPspEvents(grid);
        }

        public Hashtable CalPspEditRecCnt(int pspMasterId)
        {
            var map = new Hashtable();
            var pspEventCnt = _pspEventRepository.Table.Count(a => a.PspMaster.PspMasterId == pspMasterId);
            var pspEnquiryCnt = _orgMasterRepository.getEnqRecCntByPspMasterId(pspMasterId);
            var pspComplaintCnt = _orgMasterRepository.getCompRecCntByPspMasterId(pspMasterId);
            var pspLetterCnt = _pspDocSummaryViewRepository.Table.Count(a => a.Enabled == true);
            var pspAttachmentCnt = _pspAttachmentRepository.Table.Count(a => a.PspMaster.PspMasterId == pspMasterId);
            var pspApprvHistCnt = _pspApprovalHistoryRepository.GetPspApprovHistSummary(new GridSettings(), pspMasterId).TotalCount;

            //CR-005 02
            var events = (from u in _pspEventRepository.Table
                          where u.PspMaster.PspMasterId == pspMasterId && u.IsDeleted == false
                          select u).ToList();

            if (events.Any(x => x.EventStatus == "CA" || x.EventStatus == "RA" || x.EventStatus == "RC" || x.EventStatus == "AP"))
                if (events.Any(x => x.EventStatus == "CA" || x.EventStatus == "RC" || x.EventStatus == "AP"))
                    events = events.Where(x => new string[] { "AP", "RC" }.Contains(x.EventStatus)).ToList();
                else
                    events = events.Where(x => "RA".Equals(x.EventStatus)).ToList();

            map.Add("pspEventCnt", pspEventCnt);
            map.Add("pspEnquiryCnt", pspEnquiryCnt);
            map.Add("pspComplaintCnt", pspComplaintCnt);
            map.Add("pspLetterCnt", pspLetterCnt);
            map.Add("pspAttachmentCnt", pspAttachmentCnt);
            map.Add("pspApprvHistCnt", pspApprvHistCnt);
            map.Add("pspTotalEvent", Convert.ToInt32(events.Sum(x => (x.EventEndDate - x.EventStartDate).Value.TotalDays + 1)));
            map.Add("pspTotalLocation", events.GroupBy(x => new { x.District, x.Location, x.ChiLocation, x.SimpChiLocation }).Select(x => x.First()).Count());
            return map;
        }

        public IDictionary<string, string> GetAllEventYearForDropdown()
        {
            string key = Constant.EVENTYEARLIST_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._pspMasterRepository.Table
                     .OrderBy(p => p.PspYear)
                     .Where(p => p.IsDeleted == false)
                     .Select(p => new { Key = p.PspYear, Value = p.PspYear })
                     .Distinct().ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public PspMaster GetChildPspmaster(int pspMasterId)
        {
            return _pspMasterRepository.Table.Where(x => x.PreviousPspMasterId == pspMasterId).FirstOrDefault();
        }

        public bool NewApplicantExists(string orgRef, int pspMasterId)
        {
            return _pspMasterRepository.Table.Any(x => x.OrgMaster.OrgRef == orgRef && x.NewApplicantIndicator == true && x.PspMasterId != pspMasterId);
        }

        public IPagedList<PspAcSummaryView> GetPspAcSummaryPage(GridSettings grid, string permitNum)
        {
            return _pspAcSummaryViewRepository.GetPage(grid, permitNum);
        }

        public bool getByPassVal(int pspMasterId)
        {
            var pspMaster = this.GetPSPById(pspMasterId);
            return pspMaster.BypassValidationIndicator != null ? (bool)pspMaster.BypassValidationIndicator : false;
        }

        public MemoryStream GetPageToXls(GridSettings Grid)
        {
            //local vars
            IDictionary<string, IDictionary<string, string>> tilteAndComment = new Dictionary<string, IDictionary<string, string>>();
            var data = _pspSearchViewRepository.GetPspList(Grid);
            var props = typeof(PspMaster).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            List<PropertyInfo[]> propClasses = new List<PropertyInfo[]>();
            List<string> headers = new List<string>();
            string[] unwantHeaders = { "Id", "PspAttachment", "PspEvent", "ContactPersonName", "ContactPersonChiName", "PspMaster", "PspEventId", "PspApprovalHistory", "DisasterMaster", "ReferenceGuideSearchView" };

            //assigments
            propClasses.Add(props);
            tilteAndComment.Add("UsedLanguage", languages);
            tilteAndComment.Add("ContactPersonSalute", engSalutes);

            foreach (var forProp in propClasses)
            {
                foreach (PropertyInfo propertyInfo in forProp)
                {
                    if (!unwantHeaders.Contains(propertyInfo.Name))
                        headers.Add(propertyInfo.Name);
                }
            }

            var path = Path.Combine(Path.GetTempPath(), "pspMaster.xlsx");
            FileInfo tempfile = new FileInfo(path);
            MemoryStream resultStream = new MemoryStream();
            using (var package = new ExcelPackage(tempfile))
            {
                package.Workbook.Worksheets.Add("PspMaster");
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                var col = 1;
                for (var i = 0; i < headers.Count; i++)
                {
                    ws.Cells[1, col].Value = headers[i];
                    if (headers[i] == "OrgMaster")
                    {
                        col++;
                        ws.Cells[1, col].Value = "EngOrgName";
                    }

                    if (tilteAndComment.Keys.Contains(headers[i]))
                    {
                        string comment = "";
                        foreach (var desp in tilteAndComment[headers[i]]) { comment = comment + desp.Value + System.Environment.NewLine; }
                    }
                    col++;
                }
                //data

                //ws.Column(3).Style.Numberformat.Format = "dd/mm/yyyy";// ApplicationReceiveDate

                for (var rowIndex = 0; rowIndex < data.Count(); rowIndex++)
                {
                    var pspMaster = _pspMasterRepository.GetById(data[rowIndex].PspMasterId);

                    //cn = (T)Activator.(pspMaster);

                    //private readonly IDictionary<string, string> rejectReason;
                    //private readonly IDictionary<string, string> pspNotRequireReason;
                    //private readonly IDictionary<string, string> caseCloseReason;
                    //private readonly IDictionary<string, string> specialRemark;

                    var colIndex = 1;
                    foreach (PropertyInfo prop in props)
                    {
                        if (!unwantHeaders.Contains(prop.Name))
                        {
                            if (prop.Name == "OrgMaster")
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = string.IsNullOrEmpty(pspMaster.OrgMaster.OrgRef) ? "" : pspMaster.OrgMaster.OrgRef.ToString();
                                colIndex++;
                                ws.Cells[rowIndex + 2, colIndex].Value = string.IsNullOrEmpty(pspMaster.OrgMaster.EngOrgName) ? "" : pspMaster.OrgMaster.EngOrgName.ToString();
                            }
                            else if (prop.Name == "ContactPersonSalute")
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : engSalutes[prop.GetValue(pspMaster, null).ToString()] : "";
                            }
                            else if (prop.Name == "UsedLanguage")
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : languages[prop.GetValue(pspMaster, null).ToString()] : "";
                            }
                            else if (prop.Name == "RejectReason")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, rejectReason[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : rejectReason[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "PspNotRequireReason")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, pspNotRequireReason[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : pspNotRequireReason[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "CaseCloseReason")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, caseCloseReason[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : caseCloseReason[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "SpecialRemark")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, specialRemark[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : specialRemark[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "DocSubmission")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, docSubmission[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : docSubmission[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "ApplicationResult")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, applicationResult[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : applicationResult[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.Name == "FundUsed")
                            {
                                var val = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : prop.GetValue(pspMaster, null).ToString() : "";
                                val = val.Trim();
                                if (val.ToString().Contains(','))
                                {
                                    var arrVal = val.ToString().Split(',');

                                    foreach (var i in arrVal)
                                    {
                                        val = val.Replace(i, fundUsed[i]);
                                    }
                                }
                                else
                                {
                                    val = string.IsNullOrEmpty(val) ? "" : fundUsed[val];
                                }
                                ws.Cells[rowIndex + 2, colIndex].Value = val;
                            }
                            else if (prop.GetValue(pspMaster, null) != null && prop.PropertyType.FullName.Contains(typeof(System.Boolean).FullName))
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : Convert.ToBoolean(prop.GetValue(pspMaster, null)) == true ? "Yes" : "No" : "";
                            }
                            else if (prop.GetValue(pspMaster, null) != null && prop.PropertyType.FullName.Contains(typeof(System.DateTime).FullName))
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = prop.GetValue(pspMaster, null) != null ? string.IsNullOrEmpty(prop.GetValue(pspMaster, null).ToString()) ? "" : string.Format("{0:dd/MM/yyyy }", prop.GetValue(pspMaster, null)).ToString() : "";
                            }
                            else
                            {
                                ws.Cells[rowIndex + 2, colIndex].Value = prop.GetValue(pspMaster, null) == null ? "" : prop.GetValue(pspMaster, null).ToString();
                            }
                            colIndex++;
                        }
                    }
                }

                package.SaveAs(resultStream);
            }
            return resultStream;
        }

        public MemoryStream InserPspByImportXls(Stream xlsxStream)
        {
            var package = new ExcelPackage(xlsxStream);

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            var target = workSheet.Cells[1, 1]; // starting from list header
            var start = target.Start;
            var end = workSheet.Dimension.End;
            var endCol = 0;
            var pspProps = typeof(PspMaster).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var errFound = false;
            var pspMasterCol = false;
            var orgRefCol = false;
            string orgRef = "";
            string pspYear = "";
            var orgMaster = new OrgMaster();
            List<PropertyInfo[]> propClasses = new List<PropertyInfo[]>();
            List<string> pspMasterheaders = new List<string>();
            List<PspMaster> updatePspList = new List<PspMaster>();
            List<PspMaster> createPspList = new List<PspMaster>();

            foreach (PropertyInfo prop in pspProps)
            {
                pspMasterheaders.Add(prop.Name);
            }

            for (int i = 1; i <= end.Column; i++) //use the column header to locate end of cloumns
            {
                if (!workSheet.Cells[start.Row, i].Text.Equals(""))
                {
                    endCol = i;
                }
                else break;

                if (workSheet.Cells[1, 1].Text.ToLower().Equals("pspmasterid"))
                {
                    pspMasterCol = true;
                }

                if (workSheet.Cells[1, i].Text.ToLower().Equals("orgmaster"))
                {
                    orgRefCol = true;
                }
            }

            MemoryStream logStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(logStream, Encoding.UTF8);
            List<string> pspMasterIdLst = new List<string>();
            List<string> orgMastLst = new List<string>();
            for (int y = start.Column; y <= endCol; y++)
            {
                if (workSheet.Cells[1, y].Text.ToLower().Equals("pspmasterid"))
                {
                    for (int x = start.Row + 1; x <= end.Row; x++) //locate duplicated master id
                    {  //Row by row...
                        if (pspMasterIdLst.Contains(Convert.ToString(workSheet.Cells[x, 1].Value)) && workSheet.Cells[x, 1].Value != null)
                        {
                            writer.WriteLine("Row " + x + ": PspMasterId " + Convert.ToString(workSheet.Cells[x, 1].Value) + " is duplicated.");
                            errFound = true;
                        }
                        pspMasterIdLst.Add(Convert.ToString(workSheet.Cells[x, 1].Value));
                    }
                }
            }

            if (!pspMasterCol)
            {
                writer.WriteLine("PspMasterId column cannot be found");
                errFound = true;
            }
            if (!orgRefCol)
            {
                writer.WriteLine("OrgMaster column cannot be found");
                errFound = true;
            }

            for (int rowIndex = start.Row + 1; rowIndex <= end.Row; rowIndex++)
            { // Row by row...
                if (!workSheet.Cells[rowIndex, 2].Text.Equals("") || !workSheet.Cells[rowIndex, 3].Text.Equals("") || !workSheet.Cells[rowIndex, 4].Text.Equals(""))
                {
                    string strId = workSheet.Cells[rowIndex, 1].Value == null ? "" : workSheet.Cells[rowIndex, 1].Value.ToString();
                    var pspMasterId = Convert.ToInt32(workSheet.Cells[rowIndex, 1].Value);
                    var pspMaster = string.IsNullOrEmpty(strId) ? new PspMaster() : _pspMasterRepository.GetById(pspMasterId) == null ? new PspMaster() : _pspMasterRepository.GetById(pspMasterId);
                    List<string> list = new List<string>();
                    for (int columIndex = start.Column; columIndex <= endCol; columIndex++)
                    {
                        if (pspMasterheaders.Contains(workSheet.Cells[1, columIndex].Text)) //if it is fd property
                        {
                            PropertyInfo pc = pspProps.First(x => x.Name == workSheet.Cells[1, columIndex].Text);
                            var value = workSheet.Cells[rowIndex, columIndex].Text.Trim();
                            var valueType = Nullable.GetUnderlyingType(pc.PropertyType) ?? pc.PropertyType;
                            System.Console.WriteLine(pc.Name);
                            if (pc.Name == "OrgMaster")
                            {
                                if (pspMaster.OrgMaster == null)
                                {
                                    orgMaster = _orgMasterRepository.GetOrgByRef(value);
                                    pspMaster.OrgMaster = orgMaster;
                                }
                                orgRef = value;
                            }
                            else if (pc.Name == "EngOrgName")
                            {
                            }
                            else if (pc.Name == "PspRef")
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                }
                            }
                            else if (pc.Name == "PreviousPspMasterId")
                            {
                            }
                            else if (pc.Name == "PspYear")
                            {
                                pspYear = value;
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "ContactPersonSalute" && !string.IsNullOrEmpty(value))
                            {
                                value = engSalutes.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "UsedLanguage" && !string.IsNullOrEmpty(value))
                            {
                                value = languages.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "RejectReason" && !string.IsNullOrEmpty(value))
                            {
                                value = rejectReason.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "PspNotRequireReason" && !string.IsNullOrEmpty(value))
                            {
                                value = pspNotRequireReason.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "CaseCloseReason" && !string.IsNullOrEmpty(value))
                            {
                                value = caseCloseReason.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "SpecialRemark" && !string.IsNullOrEmpty(value))
                            {
                                var arr = value.ToString().Split(',');
                                for (var v = 0; v < arr.Length; v++)
                                {
                                    arr[v] = specialRemark.Where(p => p.Value == arr[v].Trim()).Select(p => p.Key).FirstOrDefault();
                                }
                                value = string.Join(",", arr);
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "FundUsed" && !string.IsNullOrEmpty(value))
                            {
                                value = fundUsed.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "DocSubmission" && !string.IsNullOrEmpty(value))
                            {
                                var arr = value.ToString().Split(',');
                                for (var v = 0; v < arr.Length; v++)
                                {
                                    arr[v] = docSubmission.Where(p => p.Value == arr[v].Trim()).Select(p => p.Key).FirstOrDefault();
                                }
                                value = string.Join(",", arr);
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (pc.Name == "ApplicationResult" && !string.IsNullOrEmpty(value))
                            {
                                value = applicationResult.Where(p => p.Value == value).Select(p => p.Key).FirstOrDefault();
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                            else if (!string.IsNullOrEmpty(value) && (value.ToLower() == "yes" || value.ToLower() == "no"))
                            {
                                bool boolVal = value.ToLower() == "yes" ? true : false;
                                pc.SetValue(pspMaster, Convert.ChangeType(boolVal, valueType));
                            }
                            else if (!string.IsNullOrEmpty(value) && valueType.FullName.ToLower() == "system.datetime")
                            {
                                //var dt2 = CommonHelper.ConvertStringToDateTime(value);
                                //pc.SetValue(pspMaster, Convert.ChangeType(dt2, valueType));
                                DateTime? dt = null;
                                dt = DateTime.ParseExact(value, new string[] { "d/M/yyyy", "M/d/yyyy" }, new CultureInfo("en-US"), DateTimeStyles.None);
                                pc.SetValue(pspMaster, Convert.ChangeType(dt, valueType));
                            }
                            else if (!string.IsNullOrEmpty(value))
                            {
                                pc.SetValue(pspMaster, Convert.ChangeType(value, valueType));
                            }
                        }
                    }

                    if (pspMaster.PspMasterId != default(int))
                        updatePspList.Add(pspMaster);
                    else
                    {
                        createPspList.Add(pspMaster);
                    }

                    //validation
                    if (string.IsNullOrEmpty(orgRef))
                    {
                        writer.WriteLine("Row " + rowIndex + ": OrgRef cannot be empty. ");
                        errFound = true;
                    }
                    if (string.IsNullOrEmpty(pspYear))
                    {
                        writer.WriteLine("Row " + rowIndex + ": PspYear cannot be empty. ");
                        errFound = true;
                    }
                    if (orgMaster == null)
                    {
                        writer.WriteLine("Row " + rowIndex + ": Cannot find organisation by orgref or organisation is not unique. ");
                        errFound = true;
                    }
                }
                else break;
            }

            workSheet = null;
            package.Dispose();

            if (!errFound)
            {
                if (createPspList.Count > 0)
                {
                    foreach (var psp in createPspList)
                    {
                        psp.PspRef = _pspMasterRepository.GetMaxSeq(pspYear);
                        _pspMasterRepository.Add(psp);
                        _eventPublisher.EntityInserted<PspMaster>(psp);
                    }
                }
                if (updatePspList.Count > 0)
                {
                    //_fdMasterRepository.Update(updateList);
                    foreach (var psp in updatePspList)
                    {
                        _pspMasterRepository.Update(psp);
                        _eventPublisher.EntityUpdated<PspMaster>(psp);
                    }
                }
                return null;
            }
            else
            {
                writer.WriteLine("");
                writer.WriteLine("Psp Master create records: " + 0);
                writer.WriteLine("Psp Master update records: " + 0);
                writer.WriteLine("Psp Event update records: " + 0);
                writer.WriteLine("Psp Event update records: " + 0);
                writer.Flush();
                return logStream;
            }
        }
    }
}