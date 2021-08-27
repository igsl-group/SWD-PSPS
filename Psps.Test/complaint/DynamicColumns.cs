using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Data
{
    public static class DynamicColumns
    {
        public class DynamicColumnstest : AutoRollbackContextSpecification
        {
            protected ISession _session;

            public DynamicColumnstest()
            {
                _session = EngineContext.Current.Resolve<ISession>();
            }
        }

        [TestClass]
        public class DynCols : DynamicColumnstest
        {
            [TestMethod]
            public void GenerateDynCols()
            {
                //var q = from p in _postRepository.Table
                //        from a in p.ActedOn.DefaultIfEmpty()
                //        select new { p1 = p.PostId, p2 = a.PostToBeActed.PostId };

                //Console.Write(q.ToList().Count);

                string testSql = " select EngDescription from [lookup] where [type] = 'complaintRecordType'  ";

                IList<object[]> list = this._session.CreateSQLQuery(testSql).List<object[]>();
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