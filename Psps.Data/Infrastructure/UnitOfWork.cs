using Psps.Core.Helper;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Psps.Data.Infrastructure
{
    /// <summary>
    /// Concrete class for the unit of work pattern, inherits from IDisposable to dispose of the Session
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private bool _disposed;
        private ITransaction _transaction;

        public UnitOfWork(ISession session)
        {
            this._session = session;
            this._session.FlushMode = FlushMode.Auto;
        }

        public IUnitOfWork BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_session.Connection.State != ConnectionState.Open)
            {
                _session.Connection.Open();
            }

            if (_transaction == null)
                _transaction = _session.BeginTransaction(isolationLevel);

            return this;
        }

        /// <summary>
        /// Commit the changes of the DbContext wrapped by the UoW
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            Ensure.NotNull(_transaction);
            _transaction.Commit();
            return true;
        }

        public bool Rollback()
        {
            Ensure.NotNull(_transaction);
            _transaction.Rollback();
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (_session != null && _session.Connection.State == ConnectionState.Open)
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the _dataContext
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
            }
            _disposed = true;
        }
    }
}