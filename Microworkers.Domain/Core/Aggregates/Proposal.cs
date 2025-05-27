using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Aggregates;
public class Proposal
{
    private Proposal() { }
    internal Proposal(
        Guid taskId,
        Guid serviceProvider,
        ProposalDescription jobDescription,
        decimal price,
        DateTime projectedDate)
    {
        TaskId = taskId;
        ServiceProviderId = serviceProvider;
        JobDescription = jobDescription;
        Price = price;
        ProposalDate = DateTime.UtcNow;
        ProjectedDate = projectedDate;
        Status = ProposalStatus.Pending;
    }
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
