using Microworkers.Domain.Core.Events;
using Microworkers.Infrastructure.Data;
using Microworkers.Infrastructure.Data.Models;

namespace Microworkers.Application.Common;

public class EventDispatcher : IEventDispatcher
{
    private readonly EventStoreContext _context;
    private readonly ISerializer _serializer;

    public EventDispatcher(EventStoreContext context, ISerializer serializer)
    {
        _context = context;
        _serializer = serializer;
    }

    public async Task Dispatch(DomainEvent @event)
    {
        var storedEvent = new StoredEvent
        {
            Id = @event.EventId,
            EventType = @event.EventType,
            Payload = _serializer.Serialize(@event),
            OccurredOn = @event.OccurredOn,
            AggregateId = GetAggregateId(@event) // Extrai UserId/TaskiId etc.
        };

        await _context.Events.AddAsync(storedEvent);
        await _context.SaveChangesAsync();
    }

    private string GetAggregateId(DomainEvent @event)
    {
        return @event switch
        {
            UserPasswordChangedEvent e => e.id.ToString(),
            UserCreatedEvent e => e.UserId.ToString(),
            _ => throw new NotImplementedException()
        };
    }
}
