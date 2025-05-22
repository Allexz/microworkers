using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Events ;
public record UserPhoneChangedEvent(Guid UserId, Phone NewPhone)
    : DomainEvent
{
    public override string EventType => nameof(UserPhoneChangedEvent);
}

