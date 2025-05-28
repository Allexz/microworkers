using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Factories;
public class TaskiFactory
{
    public static Result<Taski> Create(
        Guid customer,
        User serviceProvider,
        Guid requiredSkill,
        string description)
    {
        List<string> errors = new();
        
        if (customer.Equals(Guid.Empty))
            errors.Add("Customer cannot be empty");
        if (requiredSkill.Equals(Guid.Empty))
            errors.Add("Task without required skill");
        if(serviceProvider == null)
            errors.Add("ServiceProvider cannot be null");

        if (errors.Any())
            return Result.Fail<Taski>(string.Join("; ", errors));

        Result<TaskDescription> taskDescriptionResult = TaskDescription.Create(description);
        if (taskDescriptionResult.IsFailure)
            errors.Add(taskDescriptionResult.Error);

        var resultCountUserTaskis = serviceProvider.CanAcceptNewTaskInProgress();
        if (resultCountUserTaskis.IsFailure)
            errors.Add(resultCountUserTaskis.Error);

        if (errors.Any())
            return Result.Fail<Taski>(string.Join("; ", errors));

        Taski task = new Taski
        (
            Guid.NewGuid(),
            customer,
            serviceProvider.Id,
            requiredSkill,
            DateTime.UtcNow,
            taskDescriptionResult.Value,
            TaskiStatus.Opened
        );
        return Result.Ok( task);
    }
}
