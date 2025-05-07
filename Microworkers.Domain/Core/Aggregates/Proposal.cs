using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Aggregates;
public class Proposal
{
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid ServiceProviderId { get; private set; }
    public ProposalDescription JobDescription { get; private set; } // Value object
    public ProposalStatus Status { get; private set; }
    public decimal Price { get; private set; }
    public DateTime ProposalDate { get; private set; }
    public DateTime ProjectedDate { get; private set; }
    public int MyProperty { get; private set; }

    private List<Proposal> _proposals = new();
    public IReadOnlyCollection<Proposal> ProposalList => _proposals.AsReadOnly();

    private Proposal() { }

    private static Proposal Build(
        Guid taskId,
        Guid serviceProvider,
        ProposalDescription jobDescription,
        decimal price,
        DateTime projectedDate)
    {
        return new Proposal
        {
            TaskId = taskId,
            ServiceProviderId = serviceProvider,
            JobDescription = jobDescription,
            Price = price,
            ProposalDate = DateTime.UtcNow,
            ProjectedDate = projectedDate,
            Status = ProposalStatus.Pending
        };
    }
     

    public static Proposal Create(
        Taski task,
        Guid serviceProvider,
        ProposalDescription jobDescription,
        decimal price,
        DateTime projectedDate)
    {
        if (task.Id.Equals(Guid.Empty))
            throw new DomainException("TaskId cannot be empty", nameof(task));

        if (task.Status != TaskiStatus.Opened)
            throw new DomainException("Cannot create a proposal for a task that is not in the Opened status", nameof(task.Status));

        if (serviceProvider.Equals(Guid.Empty))
            throw new DomainException("ServiceProvider cannot be empty", nameof(serviceProvider));

        if (price <= 0)
            throw new DomainException("Price must be greater than zero", nameof(price));

        return Build(task.Id, serviceProvider, jobDescription, price, projectedDate);
    }

    public void AcceptProposal()
    {
        if (Status != ProposalStatus.Pending)
            throw new DomainException("Proposal can only be accepted from Pending status", nameof(Status));
        Status = ProposalStatus.Accepted;
    }

    public void Cancel(Guid requestingServiceProviderId)
    {
        if (requestingServiceProviderId != ServiceProviderId)
            throw new DomainException("Only the service provider who created the proposal can cancel it", nameof(requestingServiceProviderId));

        if (Status != ProposalStatus.Pending)
            throw new DomainException("Proposal can only be cancelled from Pending status", nameof(Status));

        Status = ProposalStatus.Cancelled;
        
    }
}
