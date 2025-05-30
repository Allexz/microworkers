using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Factories;
public static class ProposalFactory
{
    public static Result<Proposal> Create(
        Taski task,
        Guid serviceProvider,
        ProposalDescription jobDescription,
        decimal price,
        DateTime projectedDate)
    {
        List<string> errors = new();

        if (task is null)
            return Result.Fail<Proposal>("Task cannot be null");

        if (task.Id.Equals(Guid.Empty))
            errors.Add("TaskId cannot be empty");

        if (task.Status != TaskiStatus.Opened)
            errors.Add("Cannot create a proposal for a task that is not in the Opened status");

        if (serviceProvider.Equals(Guid.Empty))
            errors.Add("ServiceProvider cannot be empty");

        if (price <= 0)
            errors.Add("Price must be greater than zero");

        if (jobDescription is null)
            errors.Add("JobDescription cannot be null");
        else
        {
            Result<ProposalDescription> proposalDescriptionResult = ProposalDescription.Create(jobDescription.Description);
            if (proposalDescriptionResult.IsFailure)
                errors.Add(proposalDescriptionResult.Error);
        }

        if (errors.Any())
            return Result.Fail<Proposal>(string.Join("; ", errors));

        Proposal proposal =  new Proposal(task.Id,
            serviceProvider,
            jobDescription,
            price,
            projectedDate);

        return Result.Ok(proposal);
    }
}
