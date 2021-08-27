using Lfpis.Models.Domain;
using NHibernate.Envers;
using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using System;

namespace Psps.Data.DB.Listeners
{
    public class RevInfoListener : IEntityTrackingRevisionListener
    {
        public void NewRevision(object revisionEntity)
        {
            var revInfo = revisionEntity as RevInfo;
            if (revInfo != null)
            {
                var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                revInfo.RevisionedById = currentUser.UserId;
                revInfo.RevisionedByPost = currentUser.PostId;
                revInfo.RevisionedOn = DateTime.Now;
            }
        }

        public void EntityChanged(System.Type entityClass, string entityName, object entityId, RevisionType revisionType, object revisionEntity)
        {
            var revInfo = revisionEntity as RevInfo;

            revInfo.RevisionChanges.Add(new RevChange
            {
                RevInfo = revInfo,
                RevisionType = revisionType,
                EntityId = entityId.ToString(),
                EntityName = entityName
            });
        }
    }
}