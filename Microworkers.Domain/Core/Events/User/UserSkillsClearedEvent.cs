namespace Microworkers.Domain.Core.Events ;
public record UserSkillsClearedEvent(Guid UserId)
    : DomainEvent
{
    public override string EventType => nameof(UserSkillsClearedEvent);
}
