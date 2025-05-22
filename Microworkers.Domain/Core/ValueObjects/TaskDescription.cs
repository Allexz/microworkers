using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.ValueObjects;
public record TaskDescription
{
    private string Value { get; init; }

    internal TaskDescription(string value) => Value = value;

    public static Result<TaskDescription> Create(string description)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(description))
            errors.Add("Task description cannot be empty");
        
        if (description.Length > 500)
            errors.Add("Task description cannot exceed 500 characters");

        if (errors.Any())
            return Result.Fail<TaskDescription>(string.Join("; ", errors));

        return Result.Ok(new TaskDescription(description.Trim()));
    }
}
