using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class DisasterTest
    {
        public class DisasterContext : AutoRollbackContextSpecification
        {
            protected DisasterMaster disasterMaster;
            protected GridSettings grid;
            protected IPagedList<DisasterMaster> page;
            protected IDisasterMasterRepository _disasterMasterRepository;
            protected IDisasterStatisticsRepository _disasterStatisticsRepository;
            protected IPSPMasterRepository _pspMasterRepository;

            public DisasterContext()
            {
                _disasterMasterRepository = EngineContext.Current.Resolve<IDisasterMasterRepository>();
                _pspMasterRepository = EngineContext.Current.Resolve<IPSPMasterRepository>();
            }
        }

        [TestClass]
        public class when_left_join : DisasterContext
        {
            [TestMethod]
            public void disaster_left_join_test()
            {
                var query = from dm in _disasterMasterRepository.Table
                            from ds in dm.DisasterStatistics.DefaultIfEmpty()
                            select new
                            {
                                DisasterMasterId = dm.DisasterMasterId,
                                BeginDate = dm.BeginDate,
                                DisasterStatisticsId = ds == null ? 0 : ds.DisasterStatisticsId,
                                PspScopePublicCount = ds == null ? 0 : ds.PspScopePublicCount
                            };

                //select many to flatten the result
                //var query =
                //            (
                //                from dm in _disasterMasterRepository.Table
                //                from ds1 in dm.DisasterStatistics.DefaultIfEmpty()
                //                group dm by new
                //                {
                //                    dm.DisasterMasterId,
                //                    dm.DisasterName,
                //                    dm.BeginDate,
                //                    dm.EndDate
                //                } into dmds
                //                select new
                //                {
                //                    DisasterMasterId = dmds.Key.DisasterMasterId,
                //                    BeginDate = dmds.Key.BeginDate,
                //                    //PspApplicationProcedureOtherCount = dmds.SelectMany(dm => dm.DisasterStatistics).Sum(ds => ds.PspApplicationProcedureOtherCount != null)
                //                    PspApplicationProcedureOtherCount =
                //                        dmds.SelectMany
                //                            (
                //                                dm => dm.DisasterStatistics
                //                                        .Select(ds2 => ds2.PspApplicationProcedureOtherCount)
                //                            ).Sum()
                //                }

                //)
                //.AsQueryable();

                //var query =
                //            (
                //                from dm in _disasterMasterRepository.Table
                //                from ds in dm.DisasterStatistics.DefaultIfEmpty()
                //                group new { dm, ds } by new
                //                {
                //                    dm.DisasterMasterId,
                //                    dm.DisasterName,
                //                    dm.BeginDate,
                //                    dm.EndDate
                //                } into dmds
                //                select new
                //                {
                //                    DisasterMasterId = dmds.Key.DisasterMasterId,
                //                    DisasterName = dmds.Key.DisasterName,
                //                    BeginDate = dmds.Key.BeginDate.ToString("d/M/yyyy"),
                //                    EndDate = dmds.Key.EndDate.HasValue ? ((DateTime)dmds.Key.EndDate).ToString("d/M/yyyy") : "",
                //                    PspApplicationProcedureOtherCount = dmds.Sum(g => (int?)g.ds.PspApplicationProcedureOtherCount).GetValueOrDefault(0),
                //                    PspApplicationProcedurePublicCount = dmds.Sum(g => (int?)g.ds.PspApplicationProcedurePublicCount).GetValueOrDefault(0),
                //                    PspApplicationStatusOthersCount = dmds.Sum(g => (int?)g.ds.PspApplicationStatusOthersCount).GetValueOrDefault(0),
                //                    PspApplicationStatusPublicCount = dmds.Sum(g => (int?)g.ds.PspApplicationStatusPublicCount).GetValueOrDefault(0),
                //                    PspPermitConditionComplianceOtherCount = dmds.Sum(g => (int?)g.ds.PspPermitConditionComplianceOtherCount).GetValueOrDefault(0),
                //                    PspScopeOtherCount = dmds.Sum(g => (int?)g.ds.PspScopeOtherCount).GetValueOrDefault(0),
                //                    PspScopePublicCount = dmds.Sum(g => (int?)g.ds.PspScopePublicCount).GetValueOrDefault(0),
                //                }

                //)
                //.AsQueryable();

                //var query = (from u in _pspMasterRepository.Table
                //             from eve in u.PspEvent
                //             group new { u, eve } by new
                //             {
                //                 u.PspMasterId
                //             }
                //                 into v
                //                 select new
                //                 {
                //                     PspMasterId = v.Key.PspMasterId,
                //                     PspEveCount = v.Count(z => z.eve.IsDeleted == true),
                //                     EventStartDate = v.Min(z => z.eve.EventStartDate),
                //                     EventEndDate = v.Max(z => z.eve.EventEndDate),
                //                 }).ToList();
                //var query2 = (from u in _pspMasterRepository.Table
                //              join x in query
                //              on new { u.PspMasterId } equals new { PspMasterId = x.PspMasterId }
                //              select new
                //              {
                //                  u.PspMasterId,
                //                  //OrgRef = u.OrgMaster.OrgRef,
                //                  //OrgName = u.OrgMaster.EngOrgName + System.Environment.NewLine + u.OrgMaster.ChiOrgName,
                //                  //EngOrgName = u.OrgMaster.EngOrgName,
                //                  //ChiOrgName = u.OrgMaster.ChiOrgName,
                //                  //SubventedIndicator = u.OrgMaster.SubventedIndicator,
                //                  //AddressProofIndicator = u.OrgMaster.AddressProofIndicator,
                //                  //PspRef = u.PspRef,
                //                  //ApplicationReceiveDate = u.ApplicationReceiveDate,
                //                  //DisableIndicator = u.OrgMaster.DisableIndicator,
                //                  //ContactPersonName = u.ContactPersonName,
                //                  //ContactPersonChiName = u.ContactPersonChiName,
                //                  //Section88Indicator = u.BeneficiaryOrgSection88Indicator,
                //                  //RegType = u.OrgMaster.RegistrationType,
                //                  //RegOtherName = u.OrgMaster.RegistrationOtherName,
                //                  //AnnualReportIndicator = u.OrgMaster.AnnualReport1Indicator == false && u.OrgMaster.AnnualReport2Indicator == false && u.OrgMaster.AnnualReport3Indicator == false ? false : true,
                //                  //AfsIndicator = u.OrgMaster.Afs1Indicator == false && u.OrgMaster.Afs2Indicator == false && u.OrgMaster.Afs3Indicator == false ? false : true,
                //                  //EventStartDate = x.EventStartDate,
                //                  //EventEndDate = x.EventEndDate,
                //                  x.PspEveCount
                //              }).AsQueryable();

                //var query = (from u in _pspMasterRepository.Table
                //             from eve in u.PspEvent
                //             group new { u, eve } by new
                //             {
                //                 u.PspMasterId
                //             }
                //             into v

                //                 select new
                //                 {
                //                     PspMasterId = v.Key.PspMasterId,
                //                     PspEveCount = v.Count(z => z.eve.IsDeleted == true),
                //                     EventStartDate = v.Min(z => z.eve.EventStartDate),
                //                     EventEndDate = v.Max(z => z.eve.EventEndDate),
                //                 }).ToList();

                //var query =
                //            from y in _pspMasterRepository.Table
                //            from ah in y.PspApprovalHistory
                //            join eve in
                //                (
                //                from u in _pspMasterRepository.Table
                //                from eve in u.PspEvent
                //                group new { u, eve } by new
                //                {
                //                    u.PspMasterId
                //                }
                //                    into v
                //                    select new
                //                    {
                //                        PspMasterId = v.Key.PspMasterId,
                //                        PspEveCount = v.Count(z => z.eve.IsDeleted == true),
                //                        EventStartDate = v.Min(z => z.eve.EventStartDate),
                //                        EventEndDate = v.Max(z => z.eve.EventEndDate),
                //                    }
                //                )
                //                on new { y.PspMasterId } equals new { eve.PspMasterId }

                //            select new PspSearchDto
                //            {
                //                PspMasterId = y.PspMasterId,
                //                PermitNum = ah.PermitNum,
                //                TotEvent = eve.PspEveCount
                //            };

                Console.Write(query.ToList().Count);
            }

            protected override void Context()
            {
            }

            protected override void BecauseOf()
            {
            }
        }
    }
}