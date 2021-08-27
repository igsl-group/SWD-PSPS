using OfficeOpenXml;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.Lookups;
using Psps.Models.Dto.OGCIO;
using Psps.Services.Events;
using Psps.Services.Lookups;
using Psps.Services.OGCIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial class FdEventService : IFdEventService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IFdEventRepository _fdEventRepository;

        private readonly IFDMasterRepository _fdMasterRepository;

        private readonly ILookupService _lookupService;

        private readonly IComplaintFdPermitNumSearchViewRepository _complaintFdPermitNumSearchViewRepository;

        private readonly IDictionary<string, string> districts;

        private readonly IDictionary<string, string> twrs;

        private readonly IFundRaisingActivityApi _fundRaisingActivityApi;

        #endregion Fields

        #region Ctor

        public FdEventService(IEventPublisher eventPublisher, IFdEventRepository fdEventRepository, IFDMasterRepository FDMasterRepository,
            IComplaintFdPermitNumSearchViewRepository complaintFdPermitNumSearchViewRepository, ILookupService lookupService, IFundRaisingActivityApi fundRaisingActivityApi)
        {
            this._lookupService = lookupService;
            this._eventPublisher = eventPublisher;
            this._fdEventRepository = fdEventRepository;
            this._fdMasterRepository = FDMasterRepository;
            this._complaintFdPermitNumSearchViewRepository = complaintFdPermitNumSearchViewRepository;
            this._fundRaisingActivityApi = fundRaisingActivityApi;

            this.districts = _lookupService.getAllLkpInCodec(LookupType.TWRDistrict);
            if (districts.Count == 0) districts.Add("", "");
            this.twrs = _lookupService.getAllLkpInCodec(LookupType.TWR);
            if (twrs.Count == 0) twrs.Add("", ""); ;
        }

        #endregion Ctor

        #region Methods

        public FdEvent GetFdEventById(int fdEventId)
        {
            return _fdEventRepository.Get(u => u.FdEventId == fdEventId && u.IsDeleted == false);
        }

        public IPagedList<FdEvent> GetPage(GridSettings grid)
        {
            return _fdEventRepository.GetPage(grid);
        }

        public IPagedList<FdReadEventDto> GetPageByFdMasterId(GridSettings grid, int fdMasterId)
        {
            return _fdEventRepository.GetPageByFdMasterId(grid, fdMasterId);
        }

        public bool InsertFdEventByImportXls(Stream xlsxStream, int fdMasterId)
        {
            //Delete records from pspEvent where Approval status does not equal 'AP'
            //The original :
            //var events = _fdEventRepository.GetMany(x => x.FdApprovalHistory.FdApprovalHistoryId == null && x.FdMaster.FdMasterId == fdMasterId);
            var events = _fdEventRepository.GetMany(x => x.FdMaster.FdMasterId == fdMasterId);
            _fdEventRepository.Delete(events);

            foreach (var eve in events)
            {
                _eventPublisher.EntityDeleted<FdEvent>(eve);
            }

            var package = new ExcelPackage(xlsxStream);

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            //Console.Write(package.Workbook.Worksheets.Count);
            //var start = workSheet.Dimension.Start;
            var target = workSheet.Cells[1, 1]; // starting from list header
            var start = target.Start;
            var end = workSheet.Dimension.End;
            var endCol = 0;

            for (int i = 1; i <= end.Column; i++) //use the column header to locate end of cloumns
            {
                if (!workSheet.Cells[start.Row, i].Text.Equals(""))
                {
                    endCol = i - 1;
                }
                else break;
            }

            List<List<string>> resultList = new List<List<string>>();

            for (int i = start.Row + 1; i <= end.Row; i++)
            { // Row by row...
                if (!workSheet.Cells[i, 1].Text.Equals(""))
                {
                    List<string> list = new List<string>();
                    for (int j = start.Column; j <= endCol; j++)
                    { // ... Cell by cell...
                        //object cellValue = workSheet.Cells[i, j].Text; // This got me the actual value I needed.
                        list.Add(workSheet.Cells[i, j].Text);
                    }
                    list.Add(i.ToString());

                    resultList.Add(list);
                }
                else break;
            }

            workSheet = null;
            package.Dispose();

            if (resultList.Count > 0)
            {
                foreach (var rec in resultList)
                {
                    FdEvent fdEvent = new FdEvent();
                    Regex regex = new Regex(@"(d+)/(d+)/(d+)");
                    Match match = regex.Match(rec[0]);
                    var startDate = new DateTime(Convert.ToInt32(match.Groups[1]), Convert.ToInt32(match.Groups[2]), Convert.ToInt32(match.Groups[3]), Convert.ToInt32(rec[1].Substring(0, 2)), Convert.ToInt32(rec[1].Substring(2, 2)), 0);
                    var fromDate = new DateTime(Convert.ToInt32(match.Groups[1]), Convert.ToInt32(match.Groups[2]), Convert.ToInt32(match.Groups[3]), Convert.ToInt32(rec[2].Substring(0, 2)), Convert.ToInt32(rec[2].Substring(2, 2)), 0);

                    fdEvent.FlagDay = startDate;
                    fdEvent.FlagTimeFrom = startDate;
                    fdEvent.FlagTimeTo = fromDate;
                    fdEvent.TWR = rec[3];
                    fdEvent.TwrDistrict = rec[4];
                    fdEvent.CollectionMethod = rec[5];
                    fdEvent.Remarks = rec[6];

                    _fdEventRepository.Add(fdEvent);
                    _eventPublisher.EntityInserted<FdEvent>(fdEvent);
                }
                return true;
            }
            else
                return false;
        }

        public IList<FdEvent> GetAllByFdMasterId(int fdMasterId)
        {
            Ensure.Argument.NotNull(fdMasterId, "fdMasterId");
            return _fdEventRepository.Table.Where(x => x.FdMaster.FdMasterId == fdMasterId).ToList();
        }

        public FdEvent GetEveByFdMasterId(int fdMasterId)
        {
            Ensure.Argument.NotNull(fdMasterId, "fdMasterId");
            return _fdEventRepository.Table.FirstOrDefault(x => x.FdMaster.FdMasterId == fdMasterId);
        }

        public IPagedList<ComplaintFdPermitNumSearchView> GetPageByComplaintFdPermitNumSearchView(GridSettings grid)
        {
            return _complaintFdPermitNumSearchViewRepository.GetPage(grid);
        }

        public int GetFdEventCountByFlagDay(DateTime targetDay)
        {
            return _fdEventRepository.GetFdEventCountByFlagDay(targetDay);
        }

        public void Update(FdEvent fdEvent)
        {
            _fdEventRepository.Update(fdEvent);
        }

        public void Create(FdEvent fdEvent)
        {
            _fdEventRepository.Add(fdEvent);
        }

        public void Delete(FdEvent fdEvent)
        {
            if (fdEvent.FrasCharityEventId.IsNotNullOrEmpty())
            {
                var result = _fundRaisingActivityApi.Delete(fdEvent.FrasCharityEventId);

                if (result.HasError)
                {
                    throw new ApplicationException(result.Content);
                }
            }

            _fdEventRepository.Delete(fdEvent);
        }

        public bool AvaliableFlagDay(DateTime flagDay, string type, string district, string fdYear, int? fdEventId)
        {
            type = twrs[type];
            //district = string.IsNullOrEmpty(district) ? "" : districts[district];
            var rtnResult = false;

            if (type == "Territory-wide" || type == "TW")
            {
                var count = _fdMasterRepository.Table.Where(u => u.FdEvent.Any(x => x.FlagDay == flagDay && x.FdEventId != fdEventId) && u.FdYear == fdYear).ToList().Count();
                rtnResult = count > 0 ? false : true;
            }
            else if ((type == "Regional"))
            {
                var count = _fdMasterRepository.Table.Where(u => u.FdEvent.Any(x => x.FlagDay == flagDay && x.FdEventId != fdEventId && x.TwrDistrict == district) && u.FdYear == fdYear).ToList().Count();
                rtnResult = count > 0 ? false : true;
            }

            return rtnResult;
        }

        #region OGCIO FRAS

        public bool CreateFRAS(FdEvent fdEvent, out string content, DateTime? approvedOn = null)
        {
            var sendParam = EventToSendParam(fdEvent);

            if (approvedOn != null)
                sendParam.ApprovedOn = approvedOn.Value.ToString("yyyy-MM-dd");

            Result result = null;

            try
            {
                result = _fundRaisingActivityApi.Create(sendParam);

                if (result.HasError)
                {
                    content = result.Content;
                    return false;
                }
                else
                {
                    content = sendParam.CharityEventId;
                    return true;
                }
            }
            catch (Exception ex)
            {
                content = ex.Message;
                return false;
            }
        }

        public bool UpdateFRAS(FdEvent fdEvent, out string content)
        {
            var sendParam = EventToSendParam(fdEvent);

            Result result = null;

            try
            {
                result = _fundRaisingActivityApi.Update(sendParam);

                content = result.Content;

                if (result.HasError)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                content = ex.Message;
                return false;
            }
        }

        public bool DeleteFRAS(FdEvent fdEvent, out string content)
        {
            var sendParam = EventToSendParam(fdEvent);

            Result result = null;

            try
            {
                result = _fundRaisingActivityApi.Delete(sendParam.CharityEventId);

                content = result.Content;

                if (result.HasError)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                content = ex.Message;
                return false;
            }
        }

        protected ActivitySendParam EventToSendParam(FdEvent fdEvent)
        {
            return new ActivitySendParam
            {
                CharityEventId = "SWD-FD-{0}".FormatWith(fdEvent.FdEventId),
                DistrictId = fdEvent.TWR == "1" ? 1 : _fundRaisingActivityApi.LookupDistrictId(fdEvent.TwrDistrict),
                LocationNameEnglish = "",
                LocationNameTChinese = "",
                LocationNameSChinese = "",
                ActivityId = 1,
                OrganisationId = fdEvent.FdMaster.OrgMaster.FrasOrganisationId.HasValue ? fdEvent.FdMaster.OrgMaster.FrasOrganisationId.Value : 0,
                Charitable = 1,
                PermitNumber = fdEvent.PermitNum,
                EnquiryContact = "",
                InputBy = "",
                ApprovedBy = "",
                ApprovedOn = DateTime.Now.ToString("yyyy-MM-dd"),
                GovHKRemarkList = fdEvent.Remarks.IsNotNullOrEmpty() ? fdEvent.Remarks.Split(',').ToList() : new List<string>(),
                Schedule = new List<Schedule>() { new Schedule { DateFrom = fdEvent.FlagTimeFrom.Value.ToString("yyyy-MM-dd"),
                                                                 DateTo = fdEvent.FlagTimeTo.Value.ToString("yyyy-MM-dd"),
                                                                 TimeFrom = fdEvent.FlagTimeFrom.Value.ToString("HH:mm"),
                                                                 TimeTo = fdEvent.FlagTimeTo.Value.ToString("HH:mm") } }
            };
        }

        #endregion OGCIO FRAS

        #endregion Methods
    }
}