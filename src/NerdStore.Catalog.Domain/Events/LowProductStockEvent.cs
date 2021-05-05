
using NerdStore.Core.CommonMessages.DomainEvents;
using System;

namespace NerdStore.Catalog.Domain.Events
{
    public class LowProductStockEvent : DomainEvent
    {
        public int StockRemaining { get; private set; }

        public LowProductStockEvent(Guid aggregateId, int stockRemaining) : base(aggregateId)
        {
            StockRemaining = stockRemaining;
        }
    }
}
