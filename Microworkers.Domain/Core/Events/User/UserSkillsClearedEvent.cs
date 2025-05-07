namespace Microworkers.Domain.Core.Events.User;
public record UserSkillsClearedEvent(Guid UserId)
    : DomainEvent
{
    public override string EventType => nameof(UserSkillsClearedEvent);
}
