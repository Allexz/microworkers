using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.ValueObjects;

public record ProposalDescription
{
    public string Description { get; private init; }
    private ProposalDescription(string value) => Description = value;

    public static Result<ProposalDescription> Create(string description)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(description))
            errors.Add("Proposal description cannot be empty");
        
        if (description.Length > 500)
            errors.Add("Proposal description cannot exceed 500 characters");

        if (errors.Any())
            return Result.Fail<ProposalDescription>(string.Join("; ", errors));


        return Result.Ok( new ProposalDescription(description.Trim()));
    }

}