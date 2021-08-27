using Psps.Core.Events;
using Psps.Core.Models;

namespace Psps.Services.Events
{
    public static class EventPublisherExtensions
    {
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity)
        {
            eventPublisher.Publish(new EntityDeleted<T>(entity));
        }

        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity)
        {
            eventPublisher.Publish(new EntityInserted<T>(entity));
        }

        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity)
        {
            eventPublisher.Publish(new EntityUpdated<T>(entity));
        }
    }
}