namespace Microworkers.Domain.Core.Events.User;
public record UserAddTaskEvent(Guid customer,
        Guid serviceProvider,
        Guid requiredSkill,
        string description) : DomainEvent
{
    public override string EventType => nameof(UserAddTaskEvent);
}
