using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.ValueObjects;
public readonly record struct TaskDescription
{
    public string Value { get; }

    private TaskDescription(string value) => Value = value;

    public static TaskDescription Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Task description cannot be empty", nameof(description));

        if (description.Length > 500)
            throw new DomainException("Task description cannot exceed 500 characters", nameof(description));

        return new TaskDescription(description.Trim());
    }
}
