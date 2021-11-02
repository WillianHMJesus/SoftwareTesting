using Core.Messages;
using System;
using System.Collections.Generic;

namespace Core.DomainObjects
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        private List<Event> _events;
        public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

        public void AddEvent(Event _event)
        {
            _events = _events ?? new List<Event>();
            _events.Add(_event);
        }

        public void RemoveEvent(Event _event)
        {
            _events?.Remove(_event);
        }

        public void ClearEvents()
        {
            _events?.Clear();
        }
    }
}
