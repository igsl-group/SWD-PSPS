using Psps.Core.Infrastructure;
using Psps.Data.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Infrastructure
{
    public abstract class AutoRollbackContextSpecification
    {
        protected IUnitOfWork _unitOfWork;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            EngineContext.Initialize(false);
            _unitOfWork = EngineContext.Current.Resolve<IUnitOfWork>();

            _unitOfWork.BeginTransaction();
            Context();
            BecauseOf();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _unitOfWork.Rollback();
            Cleanup();
        }

        protected virtual void Context()
        {
        }

        protected virtual void BecauseOf()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}