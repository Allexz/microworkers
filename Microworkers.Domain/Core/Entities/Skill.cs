using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.Entities;

public readonly record struct Skill
{
    public Guid Id { get; }
    public string? Name { get; }

    public Skill(Guid Id, string name)
    {
        Id = ValidateId(Id);
        Name = ValidateName(name);
    }

    private static string? ValidateName(string name) =>
                !string.IsNullOrWhiteSpace(name) && name.Length <= 75
            ? name
            : throw new DomainException("Name is required and must have up to 75 characters", nameof(name));

    private static Guid ValidateId(Guid id) =>
        id != Guid.Empty
            ? id
            : throw new DomainException("Id cannot be empty", nameof(id));
}