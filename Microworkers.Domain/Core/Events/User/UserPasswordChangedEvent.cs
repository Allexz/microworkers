namespace Microworkers.Domain.Core.Events;
public record UserPasswordChangedEvent(Guid id) : DomainEvent
{
    public override string EventType => nameof(UserPasswordChangedEvent);
}
