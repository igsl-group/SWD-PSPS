using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Psps.Data.Infrastructure
{
    /// <summary>
    /// Interface for the unit of work pattern, inherits from IDisposable to dispose of the Session
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        bool Commit();

        bool Rollback();
    }
}