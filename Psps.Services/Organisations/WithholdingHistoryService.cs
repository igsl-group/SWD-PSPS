using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.Organisation;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial class WithholdingHistoryService : IWithholdingHistoryService
    {
        private readonly IWithholdingHistoryRepository _withholdingHistoryRepository;

        public WithholdingHistoryService(IWithholdingHistoryRepository withholdingHistoryRepository)
        {
            this._withholdingHistoryRepository = withholdingHistoryRepository;
        }

        public IPagedList<WithholdingHistory> GetPage(GridSettings grid)
        {
            return _withholdingHistoryRepository.GetPage(grid);
        }

        public int GetWithholdingHistoryAmountByOrgId(string OrgId)
        {
            return _withholdingHistoryRepository.Table.Count(a => a.OrgId == Convert.ToInt32(OrgId));
        }

        public IList<WithholdingHistory> GetByOrgId(int OrgId)
        {
            _withholdingHistoryRepository.Table.Where(a => a.OrgId == OrgId).Max(a => a.WithholdingBeginDate);
            return _withholdingHistoryRepository.Table.Where(a => a.OrgId == OrgId).ToList();
        }

        public WithHoldingDate GetWithholdingDateByOrgId(int OrgId)
        {
            string dateFormat = "dd/MM/yyyy";
            WithHoldingDate result = new WithHoldingDate();

            var tmp = _withholdingHistoryRepository.Table
                .Where(a => a.OrgId == OrgId && a.WithholdingBeginDate <= DateTime.Now.Date)
                .Select(i => new WithHoldingDate()
                {
                    WithholdingBeginDate = i.WithholdingBeginDate == null ? "" : i.WithholdingBeginDate.Value.ToString(dateFormat),
                    WithholdingEndDate = i.WithholdingEndDate == null ? "" : i.WithholdingEndDate.Value.ToString(dateFormat)
                }
                ).ToList();

            if (tmp == null) return result;
            else
            {
                //1st : With Begin Date but no End Date
                if (tmp.Where(i => i.WithholdingEndDate == null || i.WithholdingEndDate == "").Any())
                {
                    result = tmp.Where(i => i.WithholdingEndDate == null || i.WithholdingEndDate == "").OrderBy(j => j.WithholdingBeginDate).FirstOrDefault();
                }
                else
                {
                    //2nd: Max Begin Date and End Date
                    result = tmp.Where(i => i.WithholdingEndDate != null && i.WithholdingEndDate != "" && CommonHelper.ConvertStringToDateTime(i.WithholdingEndDate, dateFormat) >= DateTime.Now.Date).OrderByDescending(j => j.WithholdingEndDate).ThenByDescending(k => k.WithholdingBeginDate).FirstOrDefault();
                }
            }

            return result == null ? new WithHoldingDate() : result;
        }

        //public string GetMaxWithholdingBeginDateByOrgId(int OrgId)
        //{
        //    var result = "";
        //    var WithholdingBeginDate = _withholdingHistoryRepository.Table.Where(a => a.OrgId == OrgId).Max(a => a.WithholdingBeginDate);
        //    if (WithholdingBeginDate != null)
        //    {
        //        result = CommonHelper.ConvertDateTimeToString(WithholdingBeginDate.Value, "dd/MM/yyyy");
        //    }
        //    return result;
        //}

        //public string GetMaxWithholdingEndDateByOrgId(int OrgId)
        //{
        //    var result = "";
        //    var WithholdingEndDate = _withholdingHistoryRepository.Table.Where(a => a.OrgId == OrgId).Max(a => a.WithholdingEndDate);
        //    if (WithholdingEndDate != null)
        //    {
        //        result = CommonHelper.ConvertDateTimeToString(WithholdingEndDate.Value, "dd/MM/yyyy");
        //    }
        //    return result;
        //}

        public bool GetWithHoldBySysDt(int OrgId)
        {
            var query = this._withholdingHistoryRepository.Table.Where(x => x.OrgId == OrgId);

            if (query.Count() > 0)
            {
                DateTime today = DateTime.Today;
                return query.Any(x => x.WithholdingBeginDate <= today && (x.WithholdingEndDate == null || x.WithholdingEndDate >= today));                    

                //DateTime minDt = (DateTime)query.Min(x => x.WithholdingBeginDate);
                //DateTime? maxDt = null;

                //if (query.Any(x => x.WithholdingEndDate != null))
                //    maxDt = (DateTime?)query.Max(x => x.WithholdingEndDate);
                //else
                //    maxDt = null;

                
                //if ((maxDt == null || maxDt == DateTime.MinValue) && minDt <= today)
                //{
                //    return true;
                //}
                //else if ((minDt <= today && today <= maxDt))
                //{
                //    return true;
                //}
            }
            return false;
        }
    }
}