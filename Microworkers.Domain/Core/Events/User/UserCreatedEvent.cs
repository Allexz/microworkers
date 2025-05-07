using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Events.User;
public record UserCreatedEvent(Guid UserId, string Name, string Document, Phone Phone, string Username)
    : DomainEvent
{
    public override string EventType => nameof(UserCreatedEvent);
}
