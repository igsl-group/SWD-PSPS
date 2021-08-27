using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Core.Models;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.DB.Listeners
{
    public class AuditFlushEntityEventListener : IFlushEntityEventListener, ISaveOrUpdateEventListener, IMergeEventListener
    {
        public AuditFlushEntityEventListener()
        {
            CurrentDateTimeProvider = () => DateTime.Now;
        }

        public Func<DateTime> CurrentDateTimeProvider { get; set; }

        public void OnFlushEntity(FlushEntityEvent @event)
        {
            var entity = @event.Entity;
            var entityEntry = @event.EntityEntry;

            if (entityEntry.Status == Status.Deleted)
                return;

            var auditable = entity as IAuditable;
            if (auditable == null)
                return;

            if (HasDirtyProperties(@event))
            {
                Audit(auditable);
            }
        }

        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {
            Audit(@event.Entity as IAuditable);
        }

        public void OnMerge(MergeEvent @event)
        {
            Audit(@event.Entity as IAuditable);
        }

        public void OnMerge(MergeEvent @event, IDictionary copiedAlready)
        {
            Audit(@event.Entity as IAuditable);
        }

        public void Register(Configuration cfg)
        {
            var listeners = cfg.EventListeners;

            listeners.FlushEntityEventListeners = new[] { this }
                .Concat(listeners.FlushEntityEventListeners)
                .ToArray();

            listeners.SaveEventListeners = new[] { this }
                .Concat(listeners.SaveEventListeners)
                .ToArray();

            listeners.SaveOrUpdateEventListeners = new[] { this }
                .Concat(listeners.SaveOrUpdateEventListeners)
                .ToArray();

            listeners.UpdateEventListeners = new[] { this }
                .Concat(listeners.UpdateEventListeners)
                .ToArray();

            listeners.MergeEventListeners = new[] { this }
                .Concat(listeners.MergeEventListeners)
                .ToArray();
        }

        private bool HasDirtyProperties(FlushEntityEvent @event)
        {
            ISessionImplementor session = @event.Session;
            EntityEntry entry = @event.EntityEntry;
            var entity = @event.Entity;
            if (!entry.RequiresDirtyCheck(entity) || !entry.ExistsInDatabase || entry.LoadedState == null)
            {
                return false;
            }
            IEntityPersister persister = entry.Persister;

            object[] currentState = persister.GetPropertyValues(entity, session.EntityMode);
            object[] loadedState = entry.LoadedState;

            return persister.EntityMetamodel.Properties
                .Where((property, i) => !LazyPropertyInitializer.UnfetchedProperty.Equals(currentState[i]) && property.Type.IsDirty(loadedState[i], currentState[i], session))
                .Any();
        }

        private void Audit(IAuditable auditable)
        {
            if (auditable == null)
                return;

            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;

            if (auditable.CreatedById == null)
            {
                auditable.CreatedOn = CurrentDateTimeProvider();
                auditable.CreatedById = currentUser.UserId;
                auditable.CreatedByPost = currentUser.PostId;
            }
            auditable.UpdatedOn = CurrentDateTimeProvider();
            auditable.UpdatedById = currentUser.UserId;
            auditable.UpdatedByPost = currentUser.PostId;
        }
    }
}