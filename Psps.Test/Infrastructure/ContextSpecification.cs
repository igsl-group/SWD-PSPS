using Psps.Core.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Test.Infrastructure
{
    public abstract class ContextSpecification
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            EngineContext.Initialize(false);
            Context();
            BecauseOf();
        }

        [TestCleanup]
        public void TestCleanup()
        {
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