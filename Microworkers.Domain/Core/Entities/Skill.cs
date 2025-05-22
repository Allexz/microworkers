using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Entities;

public record Skill
{
    private Skill() { }
    internal Skill(Guid id, string? name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string? Name { get; }

    public static Result<Skill> Create(Guid Id, string name)
    {
        List<string> errors = new();
        if (Id == Guid.Empty)
            errors.Add("Id cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name cannot be null nor empty");

        if (name.Length > 75)
            errors.Add("Name cannot be longer than 75 characters");

        if (errors.Any())
            return Result.Fail<Skill>(string.Join("; ", errors));

        return Result.Ok(new Skill(Id, name));

    }
}