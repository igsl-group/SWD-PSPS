using Psps.Core.Infrastructure;
using System;

namespace Psps.Test.Infrastructure
{
    public abstract class BaseTest
    {
        public BaseTest()
        {
            EngineContext.Initialize(false);
        }
    }
}