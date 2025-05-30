using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.ValueObjects;
public record TaskDescription
{
    private TaskDescription() { }
    internal TaskDescription(string value) => _description = value.Trim();
    private string _description { get; init; }
    public string Description => _description;


    public static Result<TaskDescription> Create(string description)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(description))
            errors.Add("Task description cannot be empty or null");
        if (errors.Any())
            return Result.Fail<TaskDescription>(string.Join("; ", errors));


        if (description.Length > 500)
            errors.Add("Task description cannot exceed 500 characters");

        if (errors.Any())
            return Result.Fail<TaskDescription>(string.Join("; ", errors));

        return Result.Ok(new TaskDescription(description.Trim()));
    }
}
