using OfficeOpenXml;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.OGCIO;
using Psps.Models.Dto.Psp;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.OGCIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Services.PSPs
{
    public partial class PspEventService : IPspEventService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IPspEventRepository _pspEventRepository;
        private readonly IPSPMasterRepository _pspMasterRepository;
        private readonly IPspEventsToProformaRepository _pspEventsToProformaRepository;
        private readonly IFdEventRepository _fdEventRepository;
        private readonly ILookupService _lookupService;
        private readonly IFlagDayListRepository _flagDayListRepository;
        private readonly IFundRaisingActivityApi _fundRaisingActivityApi;

        #endregion Fields

        #region Ctor

        public PspEventService(IEventPublisher eventPublisher, IPspEventRepository pspEventRepository,
            IPSPMasterRepository pspMasterRepository, IPspEventsToProformaRepository pspEventsToProformaRepository,
            IFdEventRepository fdEventRepository, ILookupService lookupService, IFlagDayListRepository flagDayListRepository, IFundRaisingActivityApi fundRaisingActivityApi)
        {
            this._eventPublisher = eventPublisher;
            this._pspEventRepository = pspEventRepository;
            this._pspMasterRepository = pspMasterRepository;
            this._pspEventsToProformaRepository = pspEventsToProformaRepository;
            this._fdEventRepository = fdEventRepository;
            this._lookupService = lookupService;
            this._flagDayListRepository = flagDayListRepository;
            this._fundRaisingActivityApi = fundRaisingActivityApi;
        }

        #endregion Ctor

        #region OGCIO FRAS

        public List<PspEventScheduleDto> GetPspEventsForFras(int pspMasterId)
        {
            string district = string.Empty;
            string location = string.Empty;
            string chiLocation = string.Empty;
            string remark = string.Empty;
            string startTime = string.Empty;
            string endTime = string.Empty;

            List<PspEvent> events = (from u in _pspEventRepository.Table
                                     where u.PspMaster.PspMasterId == pspMasterId && u.PspApprovalHistory != null && (u.EventStatus == "AP" || u.FrasStatus == "RC")
                                     orderby u.District, u.Location, u.ChiLocation, u.Remarks, u.EventStartTime, u.EventEndTime, u.EventStartDate, u.EventEndDate
                                     select u).ToList();

            List<PspEventScheduleDto> proformas = new List<PspEventScheduleDto>();
            PspEventScheduleDto proforma = null;

            foreach (PspEvent pspEvent in events)
            {
                if (pspEvent.District != district ||
                    pspEvent.Location != location ||
                    pspEvent.ChiLocation != chiLocation ||
                    pspEvent.Remarks != remark)
                {
                    if (proforma != null)
                        proformas.Add(proforma);

                    proforma = new PspEventScheduleDto(pspEvent.PspMaster.IsSsaf.GetValueOrDefault(false));

                    district = pspEvent.District;
                    location = pspEvent.Location;
                    chiLocation = pspEvent.ChiLocation;
                    remark = pspEvent.Remarks;

                    proforma.CharityEventId = "SWD-PSP-{0}".FormatWith(pspEvent.PspEventId);
                    proforma.Status = "RC";
                    proforma.SendParam.CharityEventId = proforma.CharityEventId;
                    proforma.SendParam.DistrictId = _fundRaisingActivityApi.LookupDistrictId(district);
                    proforma.SendParam.LocationNameEnglish = location;
                    proforma.SendParam.LocationNameTChinese = chiLocation;
                    proforma.SendParam.LocationNameSChinese = pspEvent.SimpChiLocation;
                    proforma.SendParam.OrganisationId = pspEvent.PspMaster.OrgMaster.FrasOrganisationId.Value;
                    proforma.SendParam.PermitNumber = pspEvent.PspApprovalHistory.PermitNum;
                    proforma.SendParam.ApprovedOn = pspEvent.PspApprovalHistory.PermitIssueDate.Value.ToString("yyyy-MM-dd");

                    if (remark.IsNotNullOrEmpty())
                        proforma.SendParam.GovHKRemarkList.AddRange(remark.Split(','));
                }

                pspEvent.FrasCharityEventId = proforma.CharityEventId;
                proforma.PspEvents.Add(pspEvent);
                startTime = pspEvent.EventStartTime.Value.ToString("HH:mm");
                endTime = pspEvent.EventEndTime.Value.ToString("HH:mm");

                proforma.SendParam.Schedule.Add(new Schedule
                {
                    DateFrom = pspEvent.EventStartDate.Value.ToString("yyyy-MM-dd"),
                    DateTo = pspEvent.EventEndDate.Value.ToString("yyyy-MM-dd"),
                    TimeFrom = startTime,
                    TimeTo = (startTime == "00:00" && endTime == "23:59") ? startTime : endTime
                });
            }

            if (proforma != null)
            {
                if (proforma.SendParam.Schedule.Count == 0)
                {
                    if (proforma.Status == "RC")
                        proforma.Status = "C";
                    else
                        proforma.Status = "RR";
                }

                proformas.Add(proforma);
            }

            return proformas;
        }

        public List<PspEventScheduleDto> GetPspEventsForFrasByCharityID(int pspMasterId)
        {
            string charityEventId = string.Empty;
            string startTime = string.Empty;
            string endTime = string.Empty;

            List<PspEvent> events = (from u in _pspEventRepository.Table
                                     where u.PspMaster.PspMasterId == pspMasterId && u.FrasCharityEventId != null && u.FrasCharityEventId != string.Empty
                                     orderby u.FrasCharityEventId, u.EventStartTime, u.EventEndTime, u.EventStartDate, u.EventEndDate
                                     select u).ToList();

            List<PspEventScheduleDto> proformas = new List<PspEventScheduleDto>();
            PspEventScheduleDto proforma = null;

            foreach (PspEvent pspEvent in events)
            {
                if (charityEventId != pspEvent.FrasCharityEventId)
                {
                    if (proforma != null)
                    {
                        if (proforma.SendParam.Schedule.Count == 0)
                        {
                            if (proforma.Status == "RC")
                                proforma.Status = "C";
                            else
                                proforma.Status = "RR";
                        }

                        proformas.Add(proforma);
                    }

                    proforma = new PspEventScheduleDto(pspEvent.PspMaster.IsSsaf.GetValueOrDefault(false));

                    charityEventId = pspEvent.FrasCharityEventId;

                    proforma.CharityEventId = charityEventId;
                    proforma.Status = pspEvent.FrasStatus;
                    proforma.SendParam.CharityEventId = charityEventId;
                    proforma.SendParam.DistrictId = _fundRaisingActivityApi.LookupDistrictId(pspEvent.District);
                    proforma.SendParam.LocationNameEnglish = pspEvent.Location;
                    proforma.SendParam.LocationNameTChinese = pspEvent.ChiLocation;
                    proforma.SendParam.LocationNameSChinese = pspEvent.SimpChiLocation;
                    proforma.SendParam.OrganisationId = pspEvent.PspMaster.OrgMaster.FrasOrganisationId.Value;
                    proforma.SendParam.PermitNumber = pspEvent.PspApprovalHistory.PermitNum;
                    proforma.SendParam.ApprovedOn = pspEvent.PspApprovalHistory.PermitIssueDate.Value.ToString("yyyy-MM-dd");

                    if (pspEvent.Remarks.IsNotNullOrEmpty())
                        proforma.SendParam.GovHKRemarkList.AddRange(pspEvent.Remarks.Split(','));
                }

                proforma.PspEvents.Add(pspEvent);
                startTime = pspEvent.EventStartTime.Value.ToString("HH:mm");
                endTime = pspEvent.EventEndTime.Value.ToString("HH:mm");

                if (pspEvent.FrasStatus == "RC")
                    proforma.Status = "RC";
                else if (proforma.Status != "RC" && proforma.Status != pspEvent.FrasStatus)
                    proforma.Status = "RU";

                if (pspEvent.FrasStatus != "RR" && pspEvent.EventStatus != "CA")
                {
                    proforma.SendParam.Schedule.Add(new Schedule
                    {
                        DateFrom = pspEvent.EventStartDate.Value.ToString("yyyy-MM-dd"),
                        DateTo = pspEvent.EventEndDate.Value.ToString("yyyy-MM-dd"),
                        TimeFrom = startTime,
                        TimeTo = (startTime == "00:00" && endTime == "23:59") ? startTime : endTime
                    });
                }
            }

            if (proforma != null)
            {
                if (proforma.SendParam.Schedule.Count == 0)
                {
                    if (proforma.Status == "RC")
                        proforma.Status = "C";
                    else
                        proforma.Status = "RR";
                }

                proformas.Add(proforma);
            }

            return proformas;
        }

        public bool SendToFRAS(PspEventScheduleDto proforma, out string content)
        {
            Result result = null;

            try
            {
                content = "Success";

                if (proforma.Status == "RC")
                    result = _fundRaisingActivityApi.Create(proforma.SendParam);
                else if (proforma.Status == "RU")
                    result = _fundRaisingActivityApi.Update(proforma.SendParam);
                else if (proforma.Status == "RR")
                    result = _fundRaisingActivityApi.Delete(proforma.CharityEventId);
                else if (proforma.Status == "C")
                    return true;

                if (result != null && result.HasError)
                {
                    content = result.Content;
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                content = ex.Message;
                return false;
            }
        }

        #endregion OGCIO FRAS

        #region Methods

        public PspEvent GetPspEventById(int pspEventId)
        {
            return _pspEventRepository.Get(u => u.PspEventId == pspEventId && u.IsDeleted == false);
        }

        public IPagedList<PspEvent> GetPage(GridSettings grid)
        {
            return _pspEventRepository.GetPage(grid);
        }

        public IPagedList<PspReadEventDto> GetPageByPspMasterId(GridSettings grid, int pspMasterId)
        {
            return _pspEventRepository.GetPageByPspMasterId(grid, pspMasterId);
        }

        public void CreatePspEvent(PspEvent pspEvent)
        {
            _pspEventRepository.Add(pspEvent);
            _eventPublisher.EntityUpdated<PspEvent>(pspEvent);
        }

        public void UpdatePspEvent(PspEvent pspEvent)
        {
            _pspEventRepository.Update(pspEvent);
            _eventPublisher.EntityUpdated<PspEvent>(pspEvent);
        }

        public void DeletePspEvent(PspEvent pspEvent)
        {
            _pspEventRepository.Delete(pspEvent);
            _eventPublisher.EntityDeleted<PspEvent>(pspEvent);
        }

        public IPagedList<PspEvent> GetRecommendApproveCancelPspEvents(GridSettings grid, int pspApprovalHistoryId, string approvalType, string pspPermitNum)
        {
            return _pspEventRepository.GetPspEventPageByPspMasterId(grid, pspApprovalHistoryId, approvalType, pspPermitNum);
        }

        public Dictionary<int, PspEvent> GetPspEventsByApprovalHistoryId(int approvalHistoryId)
        {
            return _pspEventRepository.GetPspEventsByApprovalHistoryId(approvalHistoryId);
        }

        //public DateTime? GetMaxEventDateFromRs(string[] pspEventIds)
        //{
        //    return _pspEventRepository.GetMaxStartDateByPspEventIds(pspEventIds);
        //}

        //public DateTime? GetMinEventDateFromRs(string[] pspEventIds)
        //{
        //    return _pspEventRepository.GetMinStartDateByPspEventIds(pspEventIds);
        //}

        public Dictionary<int, PspEvent> GetPspEventsByCancelHistoryId(int CancelHistoryId)
        {
            return _pspEventRepository.GetPspEventsByCancelHistoryId(CancelHistoryId);
        }

        public IList<int> GetRecommendApproveCancelEventsList(int pspApprovalHistoryId, string approvalType, string pspPermitNum)
        {
            if (approvalType == "TW" || approvalType == "AM" || approvalType == "NM")
            {
                var query = (from u in _pspEventRepository.Table.Where(x => x.PspApprovalHistory.PspApprovalHistoryId == pspApprovalHistoryId
                                                && x.PspApprovalHistory.ApprovalType.Equals(approvalType)
                                                && x.PspApprovalHistory.ApprovalStatus.Equals("RA")
                                                && x.PspApprovalHistory.PermitNum.Equals(pspPermitNum)
                                                && x.IsDeleted == false)
                             select (u.PspEventId)).ToList();
                return query;
            }
            else if (approvalType == "CE")
            {
                var query = (from u in _pspEventRepository.Table.Where(x => x.PspCancelHistory.PspApprovalHistoryId == pspApprovalHistoryId
                                                                        && x.PspCancelHistory.ApprovalStatus.Equals("RC")
                                                                        && x.IsDeleted == false)
                             select (u.PspEventId)).ToList();
                return query;
            }
            else
                return null;
        }

        public List<PspEvent> GetPspEventIds(int pspMasterId)
        {
            return (from u in _pspEventRepository.Table
                    where u.PspMaster.PspMasterId == pspMasterId && u.IsDeleted == false
                    select u).ToList();
        }

        public MemoryStream InsertPspEventByImportXls(Stream xlsxStream, int pspMasterId)
        {
            var pspMaster = _pspMasterRepository.GetPspMasterById(pspMasterId);
            var byPass = (bool)pspMaster.BypassValidationIndicator;
            MemoryStream logStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(logStream, Encoding.UTF8);
            IDictionary<string, string> collectionMethod = this._lookupService.getAllLkpInCodec(LookupType.PspCollectionMethod);
            Regex regex = new Regex("");
            Match match = null;

            //Delete records from pspEvent where Approval status does not equal 'AP'
            var events = _pspEventRepository.GetMany(x => x.PspApprovalHistory == null && x.PspMaster.PspMasterId == pspMasterId);
            _pspEventRepository.Delete(events);

            foreach (var eve in events)
            {
                _eventPublisher.EntityDeleted<PspEvent>(eve);
            }

            var package = new ExcelPackage(xlsxStream);

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1]; // = package.Workbook.Worksheets[3];

            var target = workSheet.Cells[2, 1]; // starting from list header
            var start = target.Start;
            var end = workSheet.Dimension.End;
            var endCol = 0;

            var headerStartRow = 1;

            for (int i = 1; i <= end.Column; i++) //use the column header to locate end of cloumns
            {
                if (!workSheet.Cells[headerStartRow, i].Text.Equals(""))
                {
                    endCol = i;
                }
                else break;
            }

            List<PspEvent> resultList = new List<PspEvent>();
            var districts = _lookupService.GetAllLookupListByType(LookupType.PspRegion);

            for (int i = start.Row; i <= end.Row; i++)
            { // Row by row...
                if (!workSheet.Cells[i, 1].Text.Equals(""))
                {
                    List<string> list = new List<string>();
                    for (int j = start.Column; j <= endCol; j++)
                    { // ... Cell by cell...
                        list.Add(workSheet.Cells[i, j].Text);
                    }
                    list.Add(i.ToString());
                    
                    if (!districts.Any(d => d.Code == list[7].Trim()))
                    {
                        writer.WriteLine("Row " + i + ": District Code is invalid.");
                        continue;
                    }

                    regex = new Regex(",");
                    match = regex.Match(list[2]);
                    if (match.Success) // check ","  existence in date
                    {
                        Array dates = list[2].Split(',');
                        foreach (var d in dates)
                        {
                            if (d.ToString().Trim() != "")
                            {
                                list[2] = d.ToString().Trim();

                                regex = new Regex("-");
                                match = regex.Match(list[2]);
                                if (match.Success) // check "-"  existence in date
                                {
                                    string[] dates2 = list[2].Split('-');
                                    var startIdx = Convert.ToInt32(dates2[0]);
                                    var endIdx = Convert.ToInt32(dates2[1]);

                                    while (startIdx <= endIdx)
                                    {
                                        list[2] = startIdx.ToString();

                                        if (IsValidEveDt(list, i, writer))
                                        {
                                            PspEvent eve = new PspEvent();
                                            insertEve(eve, list, collectionMethod, pspMasterId, writer, resultList, i);
                                            resultList.Add(eve);
                                        }

                                        startIdx++;
                                    }
                                }
                                else if (IsValidEveDt(list, i, writer)) // if the time is valid
                                {
                                    PspEvent eve = new PspEvent();
                                    insertEve(eve, list, collectionMethod, pspMasterId, writer, resultList, i);
                                    resultList.Add(eve);
                                }
                            }
                        }
                    }
                    else
                    {
                        regex = new Regex("-");
                        match = regex.Match(list[2]);
                        if (match.Success) // check "-"  existence in date
                        {
                            string[] dates2 = list[2].Split('-');
                            var startIdx = Convert.ToInt32(dates2[0]);
                            var endIdx = Convert.ToInt32(dates2[1]);

                            while (startIdx <= endIdx)
                            {
                                list[2] = startIdx.ToString();

                                if (IsValidEveDt(list, i, writer))
                                {
                                    PspEvent eve = new PspEvent();
                                    insertEve(eve, list, collectionMethod, pspMasterId, writer, resultList, i);
                                    resultList.Add(eve);
                                }

                                startIdx++;
                            }
                        }
                        else if (IsValidEveDt(list, i, writer)) // if the time is valid
                        {
                            PspEvent eve = new PspEvent();
                            insertEve(eve, list, collectionMethod, pspMasterId, writer, resultList, i);
                            resultList.Add(eve);
                        }
                    }
                }
                else break;
            }

            workSheet = null;
            package.Dispose();
            writer.Flush();
            //update min start date and max end date

            //logStream != null && logStream.Length > 3
            if ((bool)byPass && logStream != null) //bypassind is true, update the record regardlessly
            {
                //pspMaster.EventPeriodFrom = _pspEventRepository.GetMinStartDateByPspId(pspMasterId);
                //pspMaster.EventPeriodTo = _pspEventRepository.GetMaxEndDateByPspId(pspMasterId);
                //_pspMasterRepository.Update(pspMaster);
                //_eventPublisher.EntityUpdated<PspMaster>(pspMaster);
                return null;
            }
            else
                return logStream;
        }

        //public int GetNumOfRemainingRecs(int pspMasterId)
        //{
        //    return _pspEventRepository.GetNumOfRemainingRecs(pspMasterId);
        //}

        public int GetNumApprovedEvents(int pspMasterId)
        {
            return _pspEventRepository.Table.Where(x => x.PspMaster.PspMasterId == pspMasterId && x.EventStatus == "AP").ToList().Count();
        }

        public Dictionary<int, PspEvent> GetPspEventsByPspMasterId(int pspMasterId)
        {
            return _pspEventRepository.GetPspEventsByPspMasterId(pspMasterId);
        }

        public Dictionary<int, PspEvent> GetPspEventsByRange(int lastRecIdx, int pspMasterId, string appStatus)
        {
            return _pspEventRepository.GetPspEventsByRange(lastRecIdx, pspMasterId, appStatus);
        }

        public Dictionary<int, PspEvent> GetPspEventsByPspEventList(int pspMasterId, string[] eventIds, string status)
        {
            return _pspEventRepository.GetPspEventsByPspEventList(pspMasterId, eventIds, status);
        }

        public Dictionary<int, PspEvent> GetPspEventsByCutoffDt(DateTime dateFrom, DateTime dateTo, int pspMasterId, string status)
        {
            return _pspEventRepository.GetPspEventsByCutoffDt(dateFrom, dateTo, pspMasterId, status);
        }

        public MemoryStream GetMemoryStreamByPspMasterId(int pspMasterId) // export proforma
        {
            var proformaList = _pspEventsToProformaRepository.getListByPspMasterId(pspMasterId);

            var path = Path.Combine(Path.GetTempPath(), "Proforma" + ".xlsx");

            FileInfo tempfile = new FileInfo(path);
            MemoryStream resultStream = new MemoryStream();
            using (var package = new ExcelPackage(tempfile))
            {
                package.Workbook.Worksheets.Add("Blank");
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                var startRowIdx = 2;

                //subsidiary info
                //ws.Cells[1, 1].Value = "收集款項的方法(請只需填寫編號):";
                //ws.Cells[2, 1].Value = "Method of Collection(Please fill in the No. only):";
                //ws.Cells[3, 1].Value = "1.";
                //ws.Cells[3, 2].Value = "在指定地點設置捐款收集箱";
                //ws.Cells[4, 2].Value = "Set up donation boxes in stationed counter";
                //ws.Cells[5, 1].Value = "2.";
                //ws.Cells[5, 2].Value = "在指定地點設置捐款收集箱並在其附近範圍攜帶捐款收集箱/袋以流動方式募捐";
                //ws.Cells[6, 2].Value = "Set up donation boxes in stationed counter with moving around solicitation with donation boxes/money collection bags in the vicinity of the stationed counter";
                //ws.Cells[7, 1].Value = "3.";
                //ws.Cells[7, 2].Value = "慈善義賣 _(請填寫義賣物品名稱)___________________________________";
                //ws.Cells[8, 2].Value = "Charity sale of __(please fill in the item names)_____________________________";
                //ws.Cells[9, 1].Value = "4.";
                //ws.Cells[9, 2].Value = "傳奉獻袋、奉獻箱";
                //ws.Cells[10, 2].Value = "Passing of the offering bags or boxes";
                //ws.Cells[11, 1].Value = "5.";
                //ws.Cells[11, 2].Value = "上門募捐";
                //ws.Cells[12, 2].Value = "Door to Door Collection";
                //ws.Cells[13, 1].Value = "6.";
                //ws.Cells[13, 2].Value = "其他：___(請填寫)___________________________";
                //ws.Cells[14, 2].Value = "Others:___(Please fill in)_________________________";

                //headers
                //ws.Cells[16, 1].Value = "Year" + System.Environment.NewLine + "年份";
                //ws.Cells[16, 2].Value = "Month" + System.Environment.NewLine + "月份";
                //ws.Cells[16, 3].Value = "Dates" + System.Environment.NewLine + "(D or DD, with hyphen or comma)" + System.Environment.NewLine + "活動日子" + System.Environment.NewLine + "(以逗點或減號分隔)";
                //ws.Cells[16, 4].Value = "Start time" + System.Environment.NewLine + "(24h)" + System.Environment.NewLine + "開始時間" + System.Environment.NewLine + "(24小時 )";
                //ws.Cells[16, 5].Value = "End time" + System.Environment.NewLine + "(24h)" + System.Environment.NewLine + "開始時間" + System.Environment.NewLine + "(24小時 )";
                //ws.Cells[16, 6].Value = "Specific location where the event would take place in English" + System.Environment.NewLine +
                //                        "(Free text up to 500 characters)" + System.Environment.NewLine + "英文活動地點" + System.Environment.NewLine + "(500字之內)";
                //ws.Cells[16, 7].Value = "Specific location where the event would take place in Traditional Chinese" + System.Environment.NewLine +
                //                        "(Free text up to 500 characters)" + System.Environment.NewLine + "中文活動地點" + System.Environment.NewLine + "(500字之內)";
                //ws.Cells[16, 8].Value = "Region 地區" + System.Environment.NewLine +
                //                        "HKI: Hong Kong Island 香港島" + System.Environment.NewLine +
                //                        "KLN: Kowloon 九龍" + System.Environment.NewLine +
                //                        "NT: New Territories & Islands (including Tseung Kwan O) 新界及離島 (包括將軍澳)";
                //ws.Cells[16, 9].Value = "Method of Collection" + System.Environment.NewLine + "收集款項的方法";
                //ws.Cells[16, 10].Value = "Ref. of Venue Approval" + System.Environment.NewLine + "場地批文附件編號";

                ws.Cells[1, 1].Value = "Year" + System.Environment.NewLine + "年份";
                ws.Cells[1, 2].Value = "Month" + System.Environment.NewLine + "月份";
                ws.Cells[1, 3].Value = "Dates(D or DD, with hyphen or comma)" + System.Environment.NewLine + "活動日子(以逗點或減號分隔)";
                ws.Cells[1, 4].Value = "Start time(24h)" + System.Environment.NewLine + "開始時間(24小時)";
                ws.Cells[1, 5].Value = "End time(24h)" + System.Environment.NewLine + "結束時間(24小時)";
                ws.Cells[1, 6].Value = "Specific location where the event would take place in English(Free text up to 500 characters)" + System.Environment.NewLine + "英文活動地點(500字之內)";
                ws.Cells[1, 7].Value = "Specific location where the event would take place in Traditional Chinese(Free text up to 500 characters)" + System.Environment.NewLine + "中文活動地點(500字之內)";
                ws.Cells[1, 8].Value = "Region 地區" + System.Environment.NewLine +
                                       "HKI: Hong Kong Island 香港島" + System.Environment.NewLine +
                                       "KLN: Kowloon 九龍" + System.Environment.NewLine +
                                       "NT: New Territories & Islands (including Tseung Kwan O) 新界及離島 (包括將軍澳)" + System.Environment.NewLine +
                                       "TW: Territory-Wide 全港";
                ws.Cells[1, 9].Value = "Method of Collection " + System.Environment.NewLine + "收集款項的方法";
                ws.Cells[1, 10].Value = "Ref. of Venue Approval" + System.Environment.NewLine + "場地批文附件編號";

                for (var rowIndex = 0; rowIndex < proformaList.Count(); rowIndex++)
                {
                    var pspEvent = proformaList[rowIndex];

                    ws.Cells[rowIndex + startRowIdx, 1].Value = pspEvent.EventStartYear;
                    ws.Cells[rowIndex + startRowIdx, 2].Value = pspEvent.EventStartMonth;
                    ws.Cells[rowIndex + startRowIdx, 3].Value = pspEvent.EventDays;
                    ws.Cells[rowIndex + startRowIdx, 4].Value = pspEvent.EventStartTime;
                    ws.Cells[rowIndex + startRowIdx, 5].Value = pspEvent.EventEndTime;
                    ws.Cells[rowIndex + startRowIdx, 6].Value = pspEvent.Location;
                    ws.Cells[rowIndex + startRowIdx, 7].Value = pspEvent.ChiLocation;
                    ws.Cells[rowIndex + startRowIdx, 8].Value = pspEvent.District;
                    ws.Cells[rowIndex + startRowIdx, 9].Value = pspEvent.CollectionMethod;
                }
                //ws.Cells[ws.Dimension.Address].AutoFitColumns();

                // Fit the columns according to its content
                for (int i = 1; i <= ws.Dimension.End.Column; i++)
                {
                    ws.Column(i).AutoFit();
                }
                package.SaveAs(resultStream);
            }
            return resultStream;
        }

        public Tuple<DateTime?, DateTime?> GetEventPeriodDateByPspId(int pspMasterId)
        {
            return _pspEventRepository.GetEventPeriodDateByPspId(pspMasterId);
        }

        //public DateTime? GetMinStartDateByPspId(int pspMasterId)
        //{
        //    return _pspEventRepository.GetMinStartDateByPspId(pspMasterId);
        //}

        //public DateTime? GetMaxEndDateByPspId(int pspMasterId)
        //{
        //    return _pspEventRepository.GetMaxEndDateByPspId(pspMasterId);
        //}

        public bool ifLstRepeated(string[] eventIds)
        {
            return _pspEventRepository.ifLstRepeated(eventIds);
        }

        public bool HasEveDupAndTimeOverlap(int pspMasterId, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, string location, string chiLoc, int? pspEventId = null)
        {
            var query = _pspEventRepository.Table.Where(x =>
                                               x.PspMaster.PspMasterId == pspMasterId && x.PspEventId != pspEventId &&
                                               (x.Location == location && x.ChiLocation == chiLoc));

            var i = query.Any(x => x.EventStartDate <= startDate && startDate <= x.EventEndDate);
            var j = query.Any(x => x.EventStartDate <= endDate && endDate <= x.EventEndDate);
            var k = query.Any(x => startDate <= x.EventStartDate && x.EventEndDate <= endDate);

            var s = query.Any(x => x.EventStartTime <= startTime && startTime <= x.EventEndTime);
            var t = query.Any(x => x.EventStartTime <= endTime && endTime <= x.EventEndTime);
            var o = query.Any(x => startTime <= x.EventStartTime && x.EventEndTime <= endTime);

            return (s || t || o) && (i || j || k);
        }

        public bool IsValidHrAndMin(string time)
        {
            int number;

            bool result = Int32.TryParse(time, out number);

            if (time.Length != 4 || !result || Convert.ToInt32(time.Substring(0, 2)) > 23 || Convert.ToInt32(time.Substring(0, 2)) < 0 || Convert.ToInt32(time.Substring(2, 2)) > 59 || Convert.ToInt32(time.Substring(2, 2)) < 0)
            {
                return false;
            }
            return true;
        }

        private void insertEve(PspEvent eve, List<string> list, IDictionary<string, string> collectionMethod, int pspMasterId, StreamWriter writer, List<PspEvent> resultList, int i)
        {
            bool isSolicit = false;
            bool isFdConflicited = false;
            var pspMaster = _pspMasterRepository.GetPspMasterById(pspMasterId);
            eve = ManagePspEventDateTime(eve, list, pspMaster.BypassValidationIndicator);                                // update time data of eve
            eve = ManagePspEventData(eve, list, collectionMethod, pspMasterId, i);     // update general data of eve
            isFdConflicited = IsFlagDayconflicted(writer, resultList, i, eve);                        // if flag day conflicted
            isSolicit = IsSolicitDate(writer, resultList, i, eve);                              // if conflicited with solictation date
            var isLaterThenPspYearPlus2Years = IsLaterThenPspYearPlus2Years(writer, i, int.Parse(pspMaster.PspYear), eve);                              // if event end date later then psp year + 2
            if ((!isDupRec(writer, resultList, i, eve) || (bool)pspMaster.BypassValidationIndicator) && IsStartDtLessThanEndDt(writer, i, eve))                              // if duplicated
            {
                if (!isFdConflicited && !isSolicit && !isLaterThenPspYearPlus2Years)
                {
                    eve.ValidationMessage = string.IsNullOrEmpty(eve.ValidationMessage) ? "Validated." : eve.ValidationMessage;
                }
                _pspEventRepository.Add(eve);
                _eventPublisher.EntityInserted<PspEvent>(eve);
            }
        }

        private PspEvent ManagePspEventData(PspEvent pspEvent, List<string> rec, IDictionary<string, string> collectionMethod, int pspMasterId, int rowIdx)
        {
            var pspMaster = _pspMasterRepository.GetById(pspMasterId);

            if (pspMaster != null)
            {
                pspEvent.PspMaster = pspMaster;
            }
            pspEvent.Location = rec[5].Trim();
            pspEvent.ChiLocation = rec[6].Trim();
            pspEvent.SimpChiLocation = LcMap.ToSimplified(rec[6].Trim());
            pspEvent.District = rec[7].Trim();

            //if (rec[8].Contains("Others"))
            //{ pspEvent.OtherCollectionMethod = collectionMethod["6"]; }
            if (rec[8].Contains("3"))
            { pspEvent.CharitySalesItem = collectionMethod["3"]; }
            pspEvent.CollectionMethod = (rec[8].Equals("6") ? "Others" : rec[8]);
            pspEvent.PublicPlaceIndicator = rec[9].ToUpper().Trim().Contains("NP") ? false : true;
            pspEvent.ProformaRowNum = rec.Count() > 10 ? Convert.ToInt32(rec[10]) : rowIdx;
            return pspEvent;
        }

        private PspEvent ManagePspEventDateTime(PspEvent pspEvent, List<string> rec, bool? byPass)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime startTime;
            DateTime endTime;
            string startTimeIn = rec[3].Replace(":", "");
            string endTimeIn = rec[4].Replace(":", "");

            startDate = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), 0, 0, 0);
            endDate = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), 0, 0, 0);
            startTime = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), Convert.ToInt32(startTimeIn.Substring(0, 2)), Convert.ToInt32(startTimeIn.Substring(2, 2)), 0);
            endTime = new DateTime(Convert.ToInt32(rec[0]), Convert.ToInt32(rec[1]), Convert.ToInt32(rec[2]), Convert.ToInt32(endTimeIn.Substring(0, 2)), Convert.ToInt32(endTimeIn.Substring(2, 2)), 0);

            pspEvent.EventStartDate = startDate;
            pspEvent.EventEndDate = endDate;
            pspEvent.EventStartTime = startTime;
            pspEvent.EventEndTime = endTime;

            if (byPass != null && byPass != true)
            {
                pspEvent.EventStatus = CheckFlagDayconflicted(startDate, startTime);
            }

            return pspEvent;
        }

        private bool IsValidEveDt(List<string> list, int i, StreamWriter writer = null)
        {
            var valid = true;

            //validate month
            if (Convert.ToInt32(list[1]) > 12 || Convert.ToInt32(list[1]) < 0)
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Month is invalid. ");
                }
                valid = false;
            }

            //validate day
            if (Convert.ToInt32(list[2]) > 31 || Convert.ToInt32(list[2]) < 0)
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Date is invalid. ");
                }
                valid = false;
            }

            //validate hour
            list[3] = list[3].Trim().Length == 4 && list[3].Trim().IndexOf(':') > 0 ? "0" + list[3].Trim() : list[3].Trim();
            list[4] = list[4].Trim().Length == 4 && list[4].Trim().IndexOf(':') > 0 ? "0" + list[4].Trim() : list[4].Trim();
            var stDt = list[3].Trim().Length == 5 ? list[3].Trim().Substring(0, 2) + list[3].Trim().Substring(3, 2) : list[3];
            var endDt = list[3].Trim().Length == 5 ? list[4].Trim().Substring(0, 2) + list[4].Trim().Substring(3, 2) : list[4];

            if (!IsValidHrAndMin(stDt))
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Start time is invalid. ");
                }
            }

            //validate hour
            if (!IsValidHrAndMin(endDt))
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": End time is invalid. ");
                }
            }

            //validate start time against end time
            if (Convert.ToInt32(stDt) > Convert.ToInt32(endDt))
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Start time cannot exceed end time. ");
                }
                valid = false;
            }

            //validate start time against end time
            if (Convert.ToInt32(stDt) == Convert.ToInt32(endDt))
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Start time cannot equal to end time. ");
                }
                valid = false;
            }

            //validate end date against psp year
            if (Convert.ToInt32(stDt) == Convert.ToInt32(endDt))
            {
                if (writer != null)
                {
                    writer.WriteLine("Row " + i + ": Start time cannot equal to end time. ");
                }
                valid = false;
            }

            return valid;
        }

        private bool isDupRec(StreamWriter writer, List<PspEvent> resultList, int rowIdx, PspEvent eve = null)
        {
            if (eve != null)
            {
                var query = (from u in resultList.AsQueryable().Where(x =>
                                                                        x.PspMaster.PspMasterId == eve.PspMaster.PspMasterId &&
                                                                        x.EventStartDate == eve.EventStartDate &&
                                                                        x.EventEndDate == eve.EventEndDate &&
                                                                        (x.EventStartTime <= eve.EventStartTime && eve.EventStartTime <= x.EventEndTime ||
                                                                        x.EventStartTime <= eve.EventEndTime && eve.EventEndTime <= x.EventEndTime ||
                                                                        eve.EventStartTime <= x.EventStartTime && x.EventEndTime <= eve.EventEndTime) &&
                                                                        (x.EventStartDate <= eve.EventStartDate && eve.EventStartDate <= x.EventEndDate ||
                                                                        x.EventStartDate <= eve.EventEndDate && eve.EventEndDate <= x.EventEndDate ||
                                                                        eve.EventStartTime <= x.EventStartDate && x.EventEndDate <= eve.EventEndDate) &&
                                                                        (x.Location == eve.Location || x.ChiLocation == eve.ChiLocation))
                             select u);
                                
                if (query.ToList().Count() > 0)
                {
                    var query2 = (from u in query select u.ProformaRowNum);
                    var strdupRows = string.Join(",", query2.ToList());

                    DateTime stDt = (DateTime)eve.EventStartDate;
                    var strStDt = stDt.ToString("dd/MM/yyyy");
                    if (query.ToList().Count() == 1) // 1st dup rec has to update the val message of dup rec parent as well.
                    {
                        foreach (var e in query)
                        {
                            if (e.ValidationMessage == "Validated.")
                            {
                                e.ValidationMessage = System.Environment.NewLine + "Row " + e.ProformaRowNum + ": Duplicated with proforma row number " + rowIdx + " (" + strStDt + ")";
                            }
                            else
                            {
                                e.ValidationMessage = e.ValidationMessage + "," + rowIdx + " (" + strStDt + ")";
                            }
                            _pspEventRepository.Update(e);
                            _eventPublisher.EntityInserted<PspEvent>(e);
                        }
                    }
                    writer.WriteLine("Row " + rowIdx + ": Duplicated with row " + strdupRows + " (" + strStDt + ")");
                    eve.ValidationMessage = eve.ValidationMessage + System.Environment.NewLine + "Row " + rowIdx + ": Duplicated with proforma row number " + strdupRows + " (" + strStDt + ")";
                    return true;
                }
            }

            return false;
        }

        private bool IsStartDtLessThanEndDt(StreamWriter writer, int rowIdx, PspEvent eve)
        {
            if (eve.EventStartTime > eve.EventEndTime)
            {
                writer.WriteLine("Row " + rowIdx + ": Start time must not exceed end time.");
                return false;
            }
            else
                return true;
        }

        private bool IsSolicitDate(StreamWriter writer, List<PspEvent> list, int rowIdx, PspEvent eve = null)
        {
            if (eve != null)
            {
                if ((eve.CollectionMethod != null && eve.CollectionMethod.Split(',').Any(x => "2".Equals(x))) && _lookupService.IsSolicitDate(eve.EventStartDate.Value))
                {
                    DateTime stDt = (DateTime)eve.EventStartDate;
                    var strStDt = stDt.ToString("dd/MM/yyyy");
                    writer.WriteLine("Row " + rowIdx + ": Conflicted with solicitation Date (" + strStDt + ")");

                    eve.ValidationMessage = eve.ValidationMessage + System.Environment.NewLine + " Conflicted with solicitation Date (" + strStDt + ")";
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private bool IsFlagDayconflicted(StreamWriter writer, List<PspEvent> list, int rowIdx, PspEvent eve = null)
        {
            DateTime stDt = (DateTime)eve.EventStartDate;
            var strStDt = stDt.ToString("dd/MM/yyyy");
            var isFdConflict = _flagDayListRepository.Table.Any(x => x.FlagDayDate != null && x.FlagDayDate.Value == stDt);
            var dt = ((DateTime)eve.EventStartDate).AddHours(13);
            if (isFdConflict && eve.EventStartTime < dt)
            {
                writer.WriteLine("Row " + rowIdx + ": Conflicted with Flag Date (" + strStDt + ")");
                eve.ValidationMessage = eve.ValidationMessage + System.Environment.NewLine + " Conflicted with Flag Date (" + strStDt + ")";
                return true;
            }
            else
                return false;
        }

        private bool IsLaterThenPspYearPlus2Years(StreamWriter writer, int rowIdx, int pspYear, PspEvent eve = null)
        {
            if (eve.EventEndDate.Value.Year > pspYear + 2) {
                var msg = "Event end date must be earlier than " + new DateTime(pspYear + 2, 12, 31).AddDays(1).ToString("dd/MM/yyyy");
                writer.WriteLine("Row " + rowIdx + ": " + msg);
                eve.ValidationMessage = eve.ValidationMessage + System.Environment.NewLine + msg;
                return true;
            }
            else
                return false;
        }

        private string CheckFlagDayconflicted(DateTime startDate, DateTime startTime)
        {
            //var fdConflictCount = _fdEventRepository.GetFdEventCountByFlagDay(startDate); // check if start day is conflicted with flagday
            var isFdConflict = _flagDayListRepository.Table.Any(x => x.FlagDayDate != null && x.FlagDayDate.Value == startDate);
            var dt = startDate.AddHours(13);
            if (isFdConflict)
            {
                if (startTime < dt)
                {
                    return "CF";
                }
                else
                    return "";
            }
            else
            {
                return "";
            }
        }

        #endregion Methods
    }
}