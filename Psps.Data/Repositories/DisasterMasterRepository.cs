using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Disaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IDisasterMasterRepository : IRepository<DisasterMaster, int>
    {
        IPagedList<DisasterInfoDto> GetPageWithDto(GridSettings grid);
    }

    public class DisasterMasterRepository : BaseRepository<DisasterMaster, int>, IDisasterMasterRepository
    {
        public DisasterMasterRepository(ISession session)
            : base(session)
        {
            //this.Session = session;
        }

        public override IPagedList<DisasterMaster> GetPage(GridSettings grid)
        {
            var query = this.Table;

            //eager fetching
            //query = query.Fetch(x => x.DisasterMaster);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<DisasterMaster>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<DisasterMaster>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        //override the original GetPage Method in getting a DisasterInfoDto output
        public IPagedList<DisasterInfoDto> GetPageWithDto(GridSettings grid)
        {
            DisasterInfoDto disStatDo = new DisasterInfoDto();
            //DisasterStatisticsRepository dsr = new DisasterStatisticsRepository(this.Session);

            //DisasterStatistics disStat = new DisasterStatistics();
            //List<DisasterStatistics> dsrDefault = new List<DisasterStatistics> { disStat };
            //dsrDefault.Add(disStat);

            var query = (from dm in this.Table
                         from ds in dm.DisasterStatistics.DefaultIfEmpty()
                         group new { dm, ds } by new
                         {
                             dm.DisasterMasterId,
                             dm.DisasterName,
                             dm.ChiDisasterName,
                             dm.DisasterDate,
                             dm.BeginDate,
                             dm.EndDate
                         } into dmds
                         select new DisasterInfoDto
                         {
                             DisasterMasterId = dmds.Key.DisasterMasterId,
                             DisasterName = dmds.Key.DisasterName,
                             ChiDisasterName = dmds.Key.ChiDisasterName,
                             DisasterDate = dmds.Key.DisasterDate,
                             BeginDate = dmds.Key.BeginDate,
                             EndDate = dmds.Key.EndDate,
                             PspApplicationProcedureOtherCount = dmds.Sum(g => (decimal?)g.ds.PspApplicationProcedureOtherCount).GetValueOrDefault(0),
                             PspApplicationProcedurePublicCount = dmds.Sum(g => (decimal?)g.ds.PspApplicationProcedurePublicCount).GetValueOrDefault(0),
                             PspApplicationStatusOthersCount = dmds.Sum(g => (decimal?)g.ds.PspApplicationStatusOthersCount).GetValueOrDefault(0),
                             PspApplicationStatusPublicCount = dmds.Sum(g => (decimal?)g.ds.PspApplicationStatusPublicCount).GetValueOrDefault(0),
                             PspPermitConditionCompliancePublicCount = dmds.Sum(g => (decimal?)g.ds.PspPermitConditionCompliancePublicCount).GetValueOrDefault(0),
                             PspPermitConditionComplianceOtherCount = dmds.Sum(g => (decimal?)g.ds.PspPermitConditionComplianceOtherCount).GetValueOrDefault(0),
                             PspScopeOtherCount = dmds.Sum(g => (decimal?)g.ds.PspScopeOtherCount).GetValueOrDefault(0),
                             PspScopePublicCount = dmds.Sum(g => (decimal?)g.ds.PspScopePublicCount).GetValueOrDefault(0),
                             OtherEnquiryOtherCount = dmds.Sum(g => (decimal?)g.ds.OtherEnquiryOtherCount).GetValueOrDefault(0),
                             OtherEnquiryPublicCount = dmds.Sum(g => (decimal?)g.ds.OtherEnquiryPublicCount).GetValueOrDefault(0),
                             SubTotal = dmds.Sum(g => (decimal?)g.ds.PspApplicationProcedureOtherCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspApplicationProcedurePublicCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspApplicationStatusOthersCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspApplicationStatusPublicCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspPermitConditionComplianceOtherCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspPermitConditionCompliancePublicCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspScopeOtherCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.PspScopePublicCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.OtherEnquiryOtherCount).GetValueOrDefault(0)
                                         + dmds.Sum(g => (decimal?)g.ds.OtherEnquiryPublicCount).GetValueOrDefault(0)
                         }).AsQueryable();

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<DisasterInfoDto>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<DisasterInfoDto>(query, grid.PageIndex, grid.PageSize, query.ToList().Count());

            return page;
        }
    }
}