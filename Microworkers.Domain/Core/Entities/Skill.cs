using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Entities;

public class Skill
{
    private Skill() { }
    internal Skill(Guid id, string  name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private init; }
    public string  Name { get;private init; }
    public ICollection<User> Users { get; init; }

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