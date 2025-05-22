using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Events ;
public record UserAddressChangedEvent(Guid UserId, Address NewAddress)
    : DomainEvent
{
    public override string EventType => nameof(UserAddressChangedEvent);
}

