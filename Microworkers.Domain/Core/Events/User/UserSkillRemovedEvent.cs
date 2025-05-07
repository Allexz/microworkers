namespace Microworkers.Domain.Core.Events.User;
public record UserSkillRemovedEvent(Guid UserId, Guid SkillId)
    : DomainEvent
{
    public override string EventType => nameof(UserSkillRemovedEvent);
}

