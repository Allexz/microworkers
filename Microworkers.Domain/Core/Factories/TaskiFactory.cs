using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Factories;
public class TaskiFactory
{
    internal static Result<Taski> Create(
        Guid customer,
        Guid serviceProvider,
        Guid requiredSkill,
        string description)
    {
        List<string> errors = new();
        
        if (customer.Equals(Guid.Empty))
            errors.Add("Customer cannot be empty");
        if (requiredSkill.Equals(Guid.Empty))
            errors.Add("ServiceProvider cannot be empty");

        Result<TaskDescription> taskDescriptionResult = TaskDescription.Create(description);
        if (taskDescriptionResult.IsFailure)
            errors.Add(taskDescriptionResult.Error);

        if (errors.Any())
            return Result.Fail<Taski>(string.Join("; ", errors));

        return Result.Ok( new Taski
        (Guid.NewGuid(),customer,serviceProvider,requiredSkill, DateTime.UtcNow, taskDescriptionResult.Value,
        TaskiStatus.Opened));
    }
}
