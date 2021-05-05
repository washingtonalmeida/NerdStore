using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Data
{
    public static class MediatrExtension
    {
        public static async Task PublishEvents(this IMediatrHandler mediatr, SalesContext salesContext)
        {
            var domainEntities = salesContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(x => x.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediatr.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
