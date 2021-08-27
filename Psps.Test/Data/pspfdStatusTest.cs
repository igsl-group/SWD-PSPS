using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Transform;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.FdStatus;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    internal class pspfdStatusTest
    {
        public class pspfdStatusContext : AutoRollbackContextSpecification
        {
            protected readonly IFDMasterRepository _fdMasterRepository;
            protected readonly IFdApprovalHistoryRepository _fdApprovalHistoryRepository;
            protected DisasterMaster disasterMaster;
            protected GridSettings grid;
            protected IPagedList<FdMaster> page;

            public pspfdStatusContext()
            {
                _fdApprovalHistoryRepository = EngineContext.Current.Resolve<IFdApprovalHistoryRepository>();
                _fdMasterRepository = EngineContext.Current.Resolve<IFDMasterRepository>();
            }
        }

        [TestClass]
        public class pspfdStatusSummary : pspfdStatusContext
        {
            [TestMethod]
            public void pspfdStatus_left_join_test()
            {
                var FdStatus = _fdMasterRepository.GetFdStatusSummary(2013);
                //var cnt = (IList<FdStatus>).count();
                //Console.Write(cnt);
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