using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections;

namespace Psps.Data.DB.Interceptors
{
    public class InterceptorDecorator : IInterceptor
    {
        #region Fields

        private readonly IInterceptor innerInterceptor;

        #endregion Fields

        public InterceptorDecorator(IInterceptor innerInterceptor)
        {
            this.innerInterceptor = this.innerInterceptor ?? new EmptyInterceptor();
        }

        public virtual bool OnLoad(object entity, object id, object[] state,
          string[] propertyNames, IType[] types)
        {
            return this.innerInterceptor.OnLoad(entity, id, state, propertyNames, types);
        }

        public virtual bool OnFlushDirty(object entity, object id, object[] currentState,
          object[] previousState, string[] propertyNames, IType[] types)
        {
            return this.innerInterceptor.OnFlushDirty(entity, id, currentState,
              previousState, propertyNames, types);
        }

        public virtual bool OnSave(object entity, object id, object[] state,
          string[] propertyNames, IType[] types)
        {
            return this.innerInterceptor.OnSave(entity, id, state, propertyNames, types);
        }

        public virtual void OnDelete(object entity, object id, object[] state,
          string[] propertyNames, IType[] types)
        {
            this.innerInterceptor.OnDelete(entity, id, state, propertyNames, types);
        }

        public virtual void OnCollectionRecreate(object collection, object key)
        {
            this.innerInterceptor.OnCollectionRecreate(collection, key);
        }

        public virtual void OnCollectionRemove(object collection, object key)
        {
            this.innerInterceptor.OnCollectionRemove(collection, key);
        }

        public virtual void OnCollectionUpdate(object collection, object key)
        {
            this.innerInterceptor.OnCollectionUpdate(collection, key);
        }

        public virtual void PreFlush(ICollection entities)
        {
            this.innerInterceptor.PreFlush(entities);
        }

        public virtual void PostFlush(ICollection entities)
        {
            this.innerInterceptor.PostFlush(entities);
        }

        public virtual bool? IsTransient(object entity)
        {
            return this.innerInterceptor.IsTransient(entity);
        }

        public virtual int[] FindDirty(object entity, object id, object[] currentState,
          object[] previousState, string[] propertyNames, IType[] types)
        {
            return this.innerInterceptor.FindDirty(entity, id,
            currentState, previousState, propertyNames, types);
        }

        public virtual object Instantiate(string entityName, EntityMode entityMode, object id)
        {
            return this.innerInterceptor.Instantiate(entityName, entityMode, id);
        }

        public virtual string GetEntityName(object entity)
        {
            return this.innerInterceptor.GetEntityName(entity);
        }

        public virtual object GetEntity(string entityName, object id)
        {
            return this.innerInterceptor.GetEntity(entityName, id);
        }

        public virtual void AfterTransactionBegin(ITransaction tx)
        {
            this.innerInterceptor.AfterTransactionBegin(tx);
        }

        public virtual void BeforeTransactionCompletion(ITransaction tx)
        {
            this.innerInterceptor.BeforeTransactionCompletion(tx);
        }

        public virtual void AfterTransactionCompletion(ITransaction tx)
        {
            this.innerInterceptor.AfterTransactionCompletion(tx);
        }

        public virtual SqlString OnPrepareStatement(SqlString sql)
        {
            return this.innerInterceptor.OnPrepareStatement(sql);
        }

        public virtual void SetSession(ISession session)
        {
            this.innerInterceptor.SetSession(session);
        }
    }
}