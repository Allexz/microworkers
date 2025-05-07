using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.ValueObjects;

public readonly record struct ProposalDescription
{
    public string Value { get; }
    private ProposalDescription(string value) => Value = value;

    public static ProposalDescription Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Proposal description cannot be empty", nameof(description));
        
        if (description.Length > 500)
            throw new DomainException("Proposal description cannot exceed 500 characters", nameof(description));
        return new ProposalDescription(description.Trim());
    }

}