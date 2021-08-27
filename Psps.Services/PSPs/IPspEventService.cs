using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial interface IPspEventService
    {
        List<PspEventScheduleDto> GetPspEventsForFras(int pspMasterId);

        List<PspEventScheduleDto> GetPspEventsForFrasByCharityID(int pspMasterId);

        bool SendToFRAS(PspEventScheduleDto proforma, out string content);

        PspEvent GetPspEventById(int pspId);

        IPagedList<PspEvent> GetPage(GridSettings grid);

        IPagedList<PspReadEventDto> GetPageByPspMasterId(GridSettings grid, int pspMasterId);

        IPagedList<PspEvent> GetRecommendApproveCancelPspEvents(GridSettings grid, int pspApprovalHistoryId, string approvalType, string pspPermitNum);

        MemoryStream InsertPspEventByImportXls(Stream xlsxStream, int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByApprovalHistoryId(int approvalHistoryId);

        void UpdatePspEvent(PspEvent pspEvent);

        void CreatePspEvent(PspEvent pspEvent);

        void DeletePspEvent(PspEvent pspEvent);

        //int GetNumOfRemainingRecs(int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByPspMasterId(int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByRange(int lastRecIdx, int pspMasterId, string status);

        Dictionary<int, PspEvent> GetPspEventsByPspEventList(int pspMasterId, string[] eventIds, string status);

        Dictionary<int, PspEvent> GetPspEventsByCutoffDt(DateTime dateFrom, DateTime dateTo, int pspMasterId, string status);

        //IPagedList<PspApproveEventDto> GetRecommendPspEvents(GridSettings grid);

        int GetNumApprovedEvents(int pspMasterId);

        MemoryStream GetMemoryStreamByPspMasterId(int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByCancelHistoryId(int CancelHistoryId);

        IList<int> GetRecommendApproveCancelEventsList(int pspApprovalHistoryId, string approvalType, string pspPermitNum);

        List<PspEvent> GetPspEventIds(int pspMasterId);

        //DateTime? GetMaxEventDateFromRs(string[] pspEventIds);

        //DateTime? GetMinEventDateFromRs(string[] pspEventIds);

        bool ifLstRepeated(string[] eventIds);

        Tuple<DateTime?, DateTime?> GetEventPeriodDateByPspId(int pspMasterId);

        //DateTime? GetMaxEndDateByPspId(int pspMasterId);

        //DateTime? GetMinStartDateByPspId(int pspMasterId);

        bool HasEveDupAndTimeOverlap(int pspMasterId, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, string location, string chiLoc, int? pspEventId = null);

        bool IsValidHrAndMin(string time);
    }
}