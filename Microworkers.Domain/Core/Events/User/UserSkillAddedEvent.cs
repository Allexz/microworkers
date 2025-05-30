﻿using Microworkers.Domain.Core.Entities;

namespace Microworkers.Domain.Core.Events ;
public record UserSkillAddedEvent(Guid UserId, Skill Skill)
    : DomainEvent
{
    public override string EventType => nameof(UserSkillAddedEvent);
}

