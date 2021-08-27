using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public static class NHTest
    {
        public class NHContext : AutoRollbackContextSpecification
        {
            protected Post post;
            protected GridSettings grid;
            protected IPagedList<Post> page;
            protected IPostRepository _postRepository;

            public NHContext()
            {
                _postRepository = EngineContext.Current.Resolve<IPostRepository>();
            }
        }

        [TestClass]
        public class when_left_join : NHContext
        {
            [TestMethod]
            public void should_be_ok()
            {
                var q = from p in _postRepository.Table
                        from a in p.ActedOn.DefaultIfEmpty()
                        select new { p1 = p.PostId, p2 = a.PostToBeActed.PostId };

                Console.Write(q.ToList().Count);
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