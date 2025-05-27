using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Factories;
public static class ProposalFactory
{
    public static Proposal Create(
        Taski task,
        Guid serviceProvider,
        ProposalDescription jobDescription,
        decimal price,
        DateTime projectedDate)
    {
        List<string> errors = new();
        if (task.Id.Equals(Guid.Empty))
            errors.Add("TaskId cannot be empty");

        if (task.Status != TaskiStatus.Opened)
            errors.Add("Cannot create a proposal for a task that is not in the Opened status");

        if (serviceProvider.Equals(Guid.Empty))
            errors.Add("ServiceProvider cannot be empty");

        if (price <= 0)
            errors.Add("Price must be greater than zero");

        return new Proposal(task.Id,
            serviceProvider,
            jobDescription,
            price,
            projectedDate);
    }
}
