using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AddEvent(Event notification)
        {
            _notifications = _notifications ?? new List<Event>();
            _notifications.Add(notification);
        }

        public void RemoveEvent(Event notification)
        {
            _notifications?.Remove(notification);
        }

        public bool HasNotifications()
        {
            return Notifications?.Count > 0;
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo))
                return true;
            if (ReferenceEquals(null, compareTo))
                return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity objA, Entity objB)
        {
            if (ReferenceEquals(objA, null) && ReferenceEquals(objB, null))
                return true;

            if (ReferenceEquals(objA, null) || ReferenceEquals(objB, null))
                return false;

            return objA.Equals(objB);
        }

        public static bool operator !=(Entity objA, Entity objB)
        {
            return !(objA == objB);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

    }
}
