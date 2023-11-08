using NHibernate;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Data.Repositories
{
    public interface IPspEventRepository : IRepository<PspEvent, int>
    {
        PspEvent GetPspEventById(int PspMasterId);

        IPagedList<PspReadEventDto> GetPageByPspMasterId(GridSettings grid, int pspMasterId);

        IPagedList<PspEvent> GetPspEventPageByPspMasterId(GridSettings grid, int pspApprovalHistoryId, string approvalType, string pspPermitNum);

        Dictionary<int, PspEvent> GetPspEventsByApprovalHistoryId(int approvalHistoryId);

        //int GetNumOfRemainingRecs(int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByPspMasterId(int pspMasterId);

        Dictionary<int, PspEvent> GetPspEventsByRange(int lastRecIdx, int pspMasterId, string status);

        Dictionary<int, PspEvent> GetPspEventsByPspEventList(int pspMasterId, string[] eventIds, string ApprovalStatus);

        Dictionary<int, PspEvent> GetPspEventsByCancelHistoryId(int CancelHistoryId);

        Tuple<DateTime?, DateTime?> GetEventPeriodDateByPspId(int pspMasterId);

        //DateTime? GetMinStartDateByPspId(int pspMasterId);

        //DateTime? GetMinStartDateByPspEventIds(string[] pspEventIds);

        //DateTime? GetMaxEndDateByPspId(int pspMasterId);

        //DateTime? GetMaxStartDateByPspEventIds(string[] pspEventIds);

        bool isExactMatched(PspEvent pspevent);

        bool ifLstRepeated(string[] eventIds);

        Dictionary<int, PspEvent> GetPspEventsByCutoffDt(DateTime dateFrom, DateTime dateTo, int pspMasterId, string status);
    }

    public class PspEventRepository : BaseRepository<PspEvent, int>, IPspEventRepository
    {
        private readonly IPspApprovalHistoryRepository _pspApprovalHistoryRepository;
        private readonly int _limit;

        public PspEventRepository(ISession session, IPspApprovalHistoryRepository pspApprovalHistoryRepository)
            : base(session)
        {
            this._limit = 2000;
            this._pspApprovalHistoryRepository = pspApprovalHistoryRepository;
        }

        public PspEvent GetPspEventById(int pspEventId)
        {
            return this.Table.Where(x => x.PspEventId == pspEventId).FirstOrDefault();
        }

        //public int GetNumOfRemainingRecs(int pspMasterId)
        //{
        //    if (_pspApprovalHistoryRepository.Table.Where(x => x.ApprovalStatus == "CA" && x.PspMaster.PspMasterId == pspMasterId).ToList().Count() > 0)
        //    {
        //        //return 0;
        //        return this.Table.Where(x => x.PspApprovalHistory != null && x.PspCancelHistory == null && x.PspMaster.PspMasterId == pspMasterId).ToList().Count();
        //    }
        //    else if (_pspApprovalHistoryRepository.Table.Where(x => x.ApprovalStatus == "RC" && x.PspMaster.PspMasterId == pspMasterId).ToList().Count() > 0)
        //    {
        //        return this.Table.Where(x => x.PspApprovalHistory != null && x.PspCancelHistory == null && x.PspMaster.PspMasterId == pspMasterId).ToList().Count();
        //    }
        //    else if (_pspApprovalHistoryRepository.Table.Where(x => x.ApprovalStatus == "AP" && x.PspMaster.PspMasterId == pspMasterId).ToList().Count() > 0)
        //    {
        //        return this.Table.Where(x => x.PspApprovalHistory.ApprovalStatus == "AP" && x.PspMaster.PspMasterId == pspMasterId).ToList().Count();
        //    }
        //    else
        //    {
        //        return this.Table.Where(x => x.PspApprovalHistory == null && x.PspMaster.PspMasterId == pspMasterId).ToList().Count();
        //    }
        //}

        public IPagedList<PspEvent> GetPspEventPageByPspMasterId(GridSettings grid, int pspApprovalHistoryId, string approvalType, string pspPermitNum)
        {
            var query = this.Table;

            if (approvalType == "TW" || approvalType == "AM" || approvalType == "NM")
            {
                query = this.Table.Where(x => x.PspApprovalHistory.PspApprovalHistoryId == pspApprovalHistoryId
                                                && x.PspApprovalHistory.ApprovalType.Equals(approvalType)
                                                && x.PspApprovalHistory.ApprovalStatus.Equals("RA")
                                                && x.PspApprovalHistory.PermitNum.Equals(pspPermitNum)
                                                && x.IsDeleted == false);
            }
            else if (approvalType == "CE")
            {
                query = this.Table.Where(x => x.PspCancelHistory.PspApprovalHistoryId == pspApprovalHistoryId
                    //&& x.PspApprovalHistory.ApprovalType.Equals(approvalType)
                    && x.PspCancelHistory.ApprovalStatus.Equals("RC")
                    //&& x.PspApprovalHistory.PermitNum.Equals(pspPermitNum)
                                                    && x.IsDeleted == false);
            }

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspEvent>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspEvent>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public override IPagedList<PspEvent> GetPage(GridSettings grid)
        {
            var query = this.Table.Where(x => x.IsDeleted == false);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspEvent>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspEvent>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<PspReadEventDto> GetPageByPspMasterId(GridSettings grid, int pspMasterId)
        {
            //DateTime? nullDate = (DateTime?)null;

            var query = from u in this.Table
                        where (u.PspMaster.PspMasterId == pspMasterId)
                        select new PspReadEventDto
                        {
                            PspEventId = u.PspEventId,
                            EventStartDate = u.EventStartDate,
                            EventStartTime = u.EventStartTime,
                            EventEndDate = u.EventEndDate,
                            EventEndTime = u.EventEndTime,
                            District = u.District,
                            Location = u.Location,
                            ChiLocation = u.ChiLocation,
                            CollectionMethod = u.CollectionMethod,
                            PublicPlaceIndicator = u.PublicPlaceIndicator,
                            EventStatus = u.EventStatus,
                            Remarks = u.Remarks,
                            UpdatedOn = u.UpdatedOn,
                            ValidationMessage = u.ValidationMessage,
                            EventCount = u.EventCount,
                            Time = u.Time,
                        };

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
            {
                query = query.OrderBy<PspReadEventDto>(grid.SortColumn, grid.SortOrder);
            }

            var page = new PagedList<PspReadEventDto>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public Dictionary<int, PspEvent> GetPspEventsByPspMasterId(int pspMasterId)
        {
            var query = from u in this.Table
                        //where (u.PspMaster.PspMasterId == pspMasterId && u.PspApprovalHistory.PspApprovalHistoryId == null)
                        where (u.PspMaster.PspMasterId == pspMasterId)
                        select new
                            {
                                u.PspEventId,
                                u
                            };
            var dict = query.ToDictionary(g => g.PspEventId, g => g.u);

            return dict;
        }

        public Dictionary<int, PspEvent> GetPspEventsByApprovalHistoryId(int approvalHistoryId)
        {
            var query = from u in this.Table
                        where (u.PspApprovalHistory.PspApprovalHistoryId == approvalHistoryId && u.IsDeleted == false)
                        select new
                            {
                                u.PspEventId,
                                u
                            };
            var dict = query.ToDictionary(g => g.PspEventId, g => g.u);

            return dict;

            //    (x => x.)
            //.ToDictionary(x => x.PspEventId , x => x);

            //Where(x => x.IsDeleted == false && x.PspApprovalHistory.PspApprovalHistoryId == approvalHistoryId)
        }

        public Dictionary<int, PspEvent> GetPspEventsByCancelHistoryId(int CancelHistoryId)
        {
            var query = from u in this.Table
                        where (u.PspCancelHistory.PspApprovalHistoryId == CancelHistoryId && u.IsDeleted == false)
                        select new
                        {
                            u.PspEventId,
                            u
                        };
            var dict = query.ToDictionary(g => g.PspEventId, g => g.u);

            return dict;
        }

        public Dictionary<int, PspEvent> GetPspEventsByCutoffDt(DateTime dateFrom, DateTime dateTo, int pspMasterId, string status)
        {
            var query = from u in this.Table
                        where (dateFrom <= u.EventStartDate && u.EventEndDate <= dateTo && u.PspMaster.PspMasterId == pspMasterId && u.IsDeleted == false)
                        select new
                        {
                            u.PspEventId,
                            u
                        };
            var dict = query.ToDictionary(g => g.PspEventId, g => g.u);
            return dict;
        }

        public Dictionary<int, PspEvent> GetPspEventsByRange(int lastRecIdx, int pspMasterId, string appStatus)
        {
            if (appStatus == "RC")
            {
                var query = (from u in this.Table
                             where (u.PspMaster.PspMasterId == pspMasterId && u.PspApprovalHistory != null && u.EventStatus == "AP" && u.IsDeleted == false)
                             orderby u.EventStartDate, u.EventEndDate, u.EventStartTime, u.EventEndTime, u.Location
                             select new
                             {
                                 u.PspEventId,
                                 u
                             }).Take(lastRecIdx);
                return query.ToDictionary(g => g.PspEventId, g => g.u);
            }
            else // RA
            {
                var query = (from u in this.Table
                             where (u.PspMaster.PspMasterId == pspMasterId && u.PspApprovalHistory == null && u.IsDeleted == false)
                             orderby u.EventStartDate, u.EventEndDate, u.EventStartTime, u.EventEndTime, u.Location
                             select new
                             {
                                 u.PspEventId,
                                 u
                             }).Take(lastRecIdx);
                return query.ToDictionary(g => g.PspEventId, g => g.u);
            }
        }

        public Dictionary<int, PspEvent> GetPspEventsByPspEventList(int pspMasterId, string[] eventIds, string ApprovalStatus)
        {
            var strEventIds = string.Join(",", eventIds);
            //var intEventIds = Convert.ToInt32(eventIds);
            //int[] intEventIds = Array.ConvertAll(eventIds, int.Parse);

            if (ApprovalStatus == "RA" || ApprovalStatus == "" || ApprovalStatus == "RC")
            {
                int[] intEventIds = null;
                Dictionary<int, PspEvent> dict = new Dictionary<int, PspEvent>();

                for (int i = 0; i <= Math.Ceiling(eventIds.Count() * 1.0 / this._limit); i++)
                {
                    intEventIds = eventIds.Select(int.Parse).Skip(i * this._limit).Take(this._limit).ToArray();
                    if (ApprovalStatus == "RA" || ApprovalStatus == "")
                    {
                        var query = (from u in this.Table
                                     where (u.PspMaster.PspMasterId == pspMasterId &&
                                             (u.PspApprovalHistory == null) &&
                                             u.IsDeleted == false && intEventIds.Contains(u.PspEventId))
                                     select new
                                     {
                                         u.PspEventId,
                                         u
                                     }).ToArray();

                        foreach (var data in query)
                        {
                            dict.Add(data.PspEventId, data.u);
                        }
                    }
                    else if (ApprovalStatus == "RC")
                    {
                        var query = (from u in this.Table
                                     where (u.PspMaster.PspMasterId == pspMasterId &&
                                             (u.PspApprovalHistory.ApprovalStatus == "AP") &&
                                             u.IsDeleted == false && intEventIds.Contains(u.PspEventId))
                                     select new
                                     {
                                         u.PspEventId,
                                         u
                                     }).ToArray(); ;
                        //var dict = query.ToDictionary(g => g.PspEventId, g => g.u);

                        foreach (var data in query)
                        {
                            dict.Add(data.PspEventId, data.u);
                        }
                    }
                }

                return dict;
            }
            else
                return null;
        }

        public bool ifLstRepeated(string[] eventIds)
        {
            int[] intEventIds = null;

            for (int i = 0; i <= Math.Ceiling(eventIds.Count() * 1.0 / this._limit); i++)
            {
                intEventIds = eventIds.Select(int.Parse).Skip(i * this._limit).Take(this._limit).ToArray();
                var query = from u in this.Table
                            where (u.IsDeleted == false && intEventIds.Contains(u.PspEventId))
                            group u by new { u.EventStartDate, u.EventEndDate, u.EventStartTime, u.EventEndTime, u.Location }
                                into grp
                                select new
                                {
                                    pspEventId = grp.Key,
                                    count = grp.Select(x => x.EventStartDate).Count()
                                };

                if (query.Any(x => x.count > 1))
                    return true;
            }

            return false;
        }

        //public DateTime? GetMinStartDateByPspId(int pspMasterId)
        //{
        //    return (from x in
        //                (
        //                    from p in this.Table
        //                    where p.PspMaster.PspMasterId == pspMasterId && p.PspCancelHistory == null
        //                    group p by p.EventStatus into g
        //                    select new
        //                    {
        //                        Status = g.Key,
        //                        MinDate = g.Min(p => p.EventStartDate),
        //                        Order = (g.Key == "" || g.Key == null ? 3 : (g.Key == "AP" ? 1 : (g.Key == "RA" ? 2 : 99)))
        //                    }
        //                 )
        //            orderby x.Order
        //            select x.MinDate
        //           ).FirstOrDefault();
        //}

        //public DateTime? GetMinStartDateByPspEventIds(string[] pspEventIds)
        //{
        //    int[] intEventIds = null;
        //    List<DateTime?> dtList = new List<DateTime?>();
        //    for (int i = 0; i <= Math.Ceiling(pspEventIds.Count() * 1.0 / this._limit); i++)
        //    {
        //        intEventIds = pspEventIds.Select(int.Parse).Skip(i * this._limit).Take(this._limit).ToArray();
        //        var result = (from u in this.Table
        //                      where intEventIds.Contains(u.PspEventId)
        //                      select u.EventStartDate).ToList();

        //        dtList.AddRange(result);
        //    }

        //    return dtList.ToList().Min();
        //}

        public Tuple<DateTime?, DateTime?> GetEventPeriodDateByPspId(int pspMasterId)
        {
            return (from x in
                        (
                            from p in this.Table
                            where p.PspMaster.PspMasterId == pspMasterId && p.PspCancelHistory == null
                            group p by p.EventStatus into g
                            select new
                            {
                                Status = g.Key,
                                MaxDate = g.Max(p => p.EventEndDate),
                                MinDate = g.Min(p => p.EventStartDate),
                                Order = (g.Key == "" || g.Key == null ? 3 : (g.Key == "AP" ? 1 : (g.Key == "RA" ? 2 : 99)))
                            }
                        )
                    orderby x.Order
                    select new Tuple<DateTime?, DateTime?>(x.MinDate, x.MaxDate)
                   ).FirstOrDefault();
        }

        //public DateTime? GetMaxStartDateByPspEventIds(string[] pspEventIds)
        //{
        //    int[] intEventIds = null;
        //    List<DateTime?> dtList = new List<DateTime?>();
        //    for (int i = 0; i <= Math.Ceiling(pspEventIds.Count() * 1.0 / this._limit); i++)
        //    {
        //        intEventIds = pspEventIds.Select(int.Parse).Skip(i * this._limit).Take(this._limit).ToArray();
        //        var result = (from u in this.Table
        //                      where intEventIds.Contains(u.PspEventId)
        //                      select u.EventStartDate).ToList();

        //        dtList.AddRange(result);
        //    }

        //    return dtList.ToList().Max();
        //}

        public bool isExactMatched(PspEvent pspevent)
        {
            var count = this.Table.Where(x => x.EventStartDate == pspevent.EventStartDate &&
                                                x.EventEndDate == pspevent.EventEndDate &&
                                                x.EventStartTime == pspevent.EventStartTime &&
                                                x.EventEndTime == pspevent.EventEndTime &&
                                                x.District == pspevent.District &&
                                                x.Location == pspevent.Location &&
                                                x.ChiLocation == pspevent.ChiLocation &&
                                                x.SimpChiLocation == pspevent.SimpChiLocation &&
                                                x.PspMaster.PspMasterId == pspevent.PspMaster.PspMasterId).ToList().Count();

            return count > 0 ? true : false;
        }
    }
}